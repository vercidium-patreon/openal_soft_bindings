#if DEBUG
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OpenAL.scripts;

// Generated with Claude Sonnet 4.5
partial class Program
{
    const string nativeLibName = "soft_oal.dll";

    static async Task Main()
    {
        var headerFiles = new Dictionary<string, (string constantsFile, string bindingsFile, string wrapperFile, string className, bool isFirst)>
        {
            { "https://raw.githubusercontent.com/kcat/openal-soft/master/include/AL/al.h", (@"..\public\ALConstants.cs", @"..\internal\ALBindings.cs", @"..\public\AL.cs", "AL", true) },
            { "https://raw.githubusercontent.com/kcat/openal-soft/master/include/AL/alc.h", (@"..\public\ALCConstants.cs", @"..\internal\ALCBindings.cs", @"..\public\ALC.cs", "AL", false) },
            { "https://raw.githubusercontent.com/kcat/openal-soft/master/include/AL/AL.h", (@"..\public\ALEXTConstants.cs", @"..\internal\ALEXTBindings.cs", @"..\public\AL.cs", "AL", false) },
            { "https://raw.githubusercontent.com/kcat/openal-soft/master/include/AL/AL.h", (@"..\public\EFXConstants.cs", @"..\internal\EFXBindings.cs", @"..\public\AL.cs", "AL", false) }
        };

        foreach (var header in headerFiles)
        {
            string url = header.Key;
            var (constantsFile, bindingsFile, wrapperFile, className, isFirst) = header.Value;
            
            Console.WriteLine($"Processing {className} from {url}...");
            
            string content = await DownloadContent(url);
            
            GenerateConstants(content, constantsFile);

            List<FunctionDeclaration> functions = ParseFunctions(content);
            string bindings = GenerateBindings(functions, className, isFirst);
            File.WriteAllText(bindingsFile, bindings);
            
            string wrappers = GenerateWrappers(functions, className);
            File.WriteAllText(wrapperFile, wrappers);
        }
        
        Console.WriteLine("\nAll bindings generated successfully!");
    }

    static async Task<string> DownloadContent(string url)
    {
        using var client = new HttpClient();
        return await client.GetStringAsync(url);
    }

    static void GenerateConstants(string content, string outputFile)
    {
        string[] lines = content.Split('\n');
        var definePattern = ConstantsRegex();

        using var writer = new StreamWriter(outputFile);

        writer.WriteLine("namespace OpenAL;");
        writer.WriteLine("");
        writer.WriteLine("public static partial class AL");
        writer.WriteLine("{");

        foreach (string line in lines)
        {
            Match match = definePattern.Match(line);
            if (match.Success)
            {
                string name = match.Groups[1].Value;
                string value = match.Groups[2].Value;

                string output = $"    public const int {name} = {value};";

                if (writer != null)
                {
                    writer.WriteLine(output);
                }
                else
                {
                    Console.WriteLine(output);
                }
            }
        }

        writer.WriteLine("}");
    }

    static List<FunctionDeclaration> ParseFunctions(string content)
    {
        var functions = new List<FunctionDeclaration>();
        
        // Pattern to match: AL_API/ALC_API returnType AL_APIENTRY/ALC_APIENTRY functionName(params) AL_API_NOEXCEPT;
        string pattern = @"(?:AL_API|ALC_API)\s+(?<returnType>[\w\s\*]+?)\s+(?:AL_APIENTRY|ALC_APIENTRY)\s+(?<functionName>\w+)\s*\((?<params>[^)]*)\)\s*(?:AL_API_NOEXCEPT|ALC_API_NOEXCEPT|AL_API_NOEXCEPT17)?\s*;";
        
        var regex = new Regex(pattern, RegexOptions.Multiline);
        var matches = regex.Matches(content);
        
        foreach (Match match in matches)
        {
            string returnType = match.Groups["returnType"].Value.Trim();
            string functionName = match.Groups["functionName"].Value.Trim();
            string paramsStr = match.Groups["params"].Value.Trim();
            
            var func = new FunctionDeclaration
            {
                ReturnType = returnType,
                FunctionName = functionName,
                Parameters = ParseParameters(paramsStr)
            };
            
            functions.Add(func);
        }
        
        return functions;
    }

    static List<Parameter> ParseParameters(string paramsStr)
    {
        var parameters = new List<Parameter>();
        
        if (string.IsNullOrWhiteSpace(paramsStr) || paramsStr == "void")
            return parameters;
        
        // Split by comma, but be careful with function pointers
        var paramParts = SplitParameters(paramsStr);
        
        foreach (var part in paramParts)
        {
            string trimmed = part.Trim();
            if (string.IsNullOrEmpty(trimmed))
                continue;
            
            // Split by whitespace to find parameter name (last token) and type (everything else)
            var tokens = new List<string>();
            var words = SplitRegex().Split(trimmed);
            
            foreach (var word in words)
            {
                if (!string.IsNullOrWhiteSpace(word))
                    tokens.Add(word);
            }
            
            if (tokens.Count == 0)
                continue;
            
            // The last token is the parameter name (might have * attached)
            string lastToken = tokens[^1];
            
            // Extract name, removing any trailing *, [, ], etc
            string paramName = lastToken.TrimEnd('*', '[', ']', ',', ';');
            
            // If the name had a * prefix, we need to keep the * with the type
            string paramType;
            if (lastToken.StartsWith('*'))
            {
                // Case like "* values" - star is part of type
                paramName = lastToken.TrimStart('*');
                paramType = string.Join(" ", tokens.Take(tokens.Count - 1)) + " *";
            }
            else if (lastToken.Contains('*'))
            {
                // Case like "*values" - star attached to name, belongs to type
                int starIndex = lastToken.IndexOf('*');
                paramName = lastToken[(starIndex + 1)..];
                paramType = string.Join(" ", tokens.Take(tokens.Count - 1)) + " *";
            }
            else
            {
                // Normal case - build type from all but last token
                paramType = string.Join(" ", tokens.Take(tokens.Count - 1));
            }
            
            // Handle array notation like "values[10]"
            if (paramName.Contains('['))
            {
                paramName = paramName[..paramName.IndexOf('[')];
            }
            
            parameters.Add(new Parameter
            {
                Type = paramType.Trim(),
                Name = paramName.Trim()
            });
        }
        
        return parameters;
    }

    static List<string> SplitParameters(string paramsStr)
    {
        var parameters = new List<string>();
        int depth = 0;
        int start = 0;
        
        for (int i = 0; i < paramsStr.Length; i++)
        {
            char c = paramsStr[i];
            
            if (c == '(')
                depth++;
            else if (c == ')')
                depth--;
            else if (c == ',' && depth == 0)
            {
                parameters.Add(paramsStr[start..i]);
                start = i + 1;
            }
        }
        
        // Add the last parameter
        if (start < paramsStr.Length)
            parameters.Add(paramsStr[start..]);
        
        return parameters;
    }

    static string GenerateBindings(List<FunctionDeclaration> functions, string className, bool includeConstant)
    {
        var sb = new StringBuilder();
        
        sb.AppendLine("namespace OpenAL;");
        sb.AppendLine();
        sb.AppendLine($"public static unsafe partial class {className}");
        sb.AppendLine("{");
        
        // Only include the constant in the first file (ALBindings.cs)
        if (includeConstant)
        {
            sb.AppendLine($"    private const string nativeLibName = \"{nativeLibName}\";");
            sb.AppendLine();
        }
        
        foreach (var func in functions)
        {
            var returnTypeInfo = ConvertType(func.ReturnType, func.FunctionName, false);
            var paramsList = new List<string>();
            var paramTypes = new List<TypeInfo>();
            bool hasStringParam = false;
            bool hasBoolParam = false;
            
            foreach (var param in func.Parameters)
            {
                var paramTypeInfo = ConvertType(param.Type, func.FunctionName, true, param.Name);
                paramTypes.Add(paramTypeInfo);
                paramsList.Add($"{paramTypeInfo.CsType} {param.Name}");
                
                if (paramTypeInfo.CsType == "string")
                    hasStringParam = true;
                
                // Check for bool parameters (not in Span)
                if (paramTypeInfo.CsType == "bool")
                    hasBoolParam = true;
            }
            
            string csParams = string.Join(", ", paramsList);
            
            // Check if we need StringMarshalling attribute
            bool needsStringMarshalling = returnTypeInfo.CsType == "string" || hasStringParam;
            
            // Check if function returns bool
            bool isBoolReturnFunction = returnTypeInfo.CsType == "bool";
            
            if (needsStringMarshalling)
            {
                sb.AppendLine("    [LibraryImport(nativeLibName, StringMarshalling = StringMarshalling.Utf8)]");
            }
            else
            {
                sb.AppendLine("    [LibraryImport(nativeLibName)]");
            }
            sb.AppendLine("    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]");
            
            if (isBoolReturnFunction)
            {
                sb.AppendLine("    [return: MarshalAs(UnmanagedType.I1)]");
            }
            
            // Special handling for bool parameters (e.g., alEventControlSOFT)
            if (hasBoolParam)
            {
                // Rebuild params with MarshalAs attribute for bool params
                var paramsWithAttributes = new List<string>();
                for (int i = 0; i < func.Parameters.Count; i++)
                {
                    var param = func.Parameters[i];
                    var paramTypeInfo = paramTypes[i];
                    
                    if (paramTypeInfo.CsType == "bool")
                    {
                        paramsWithAttributes.Add($"[MarshalAs(UnmanagedType.I1)] {paramTypeInfo.CsType} {param.Name}");
                    }
                    else
                    {
                        paramsWithAttributes.Add($"{paramTypeInfo.CsType} {param.Name}");
                    }
                }
                csParams = string.Join(", ", paramsWithAttributes);
            }
            
            sb.AppendLine($"    internal static partial {returnTypeInfo.CsType} {func.FunctionName}({csParams});");
            sb.AppendLine();
            
            // Special case: Generate alcGetStringPtr for alcGetString
            if (func.FunctionName == "alcGetString")
            {
                sb.AppendLine("    [LibraryImport(nativeLibName, EntryPoint = \"alcGetString\")]");
                sb.AppendLine("    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]");
                sb.AppendLine($"    internal static partial IntPtr alcGetStringPtr({csParams});");
                sb.AppendLine();
            }
            
            // Special case: Generate pointer overload for alcCaptureSamples
            if (func.FunctionName == "alcCaptureSamples")
            {
                // Replace Span<byte> buffer with nint buffer
                var ptrParams = new List<string>();
                foreach (var param in func.Parameters)
                {
                    if (param.Name == "buffer")
                    {
                        ptrParams.Add("nint buffer");
                    }
                    else
                    {
                        var paramTypeInfo = ConvertType(param.Type, func.FunctionName, true, param.Name);
                        ptrParams.Add($"{paramTypeInfo.CsType} {param.Name}");
                    }
                }
                string ptrParamsStr = string.Join(", ", ptrParams);
                
                sb.AppendLine("    [LibraryImport(nativeLibName, EntryPoint = \"alcCaptureSamples\")]");
                sb.AppendLine("    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]");
                sb.AppendLine($"    internal static partial void alcCaptureSamplesPtr({ptrParamsStr});");
                sb.AppendLine();
            }
            
            // Special case: Generate pointer overload for alBufferData
            if (func.FunctionName == "alBufferData")
            {
                // Replace Span<byte> data with nint data
                var ptrParams = new List<string>();
                foreach (var param in func.Parameters)
                {
                    if (param.Name == "data")
                    {
                        ptrParams.Add("nint data");
                    }
                    else
                    {
                        var paramTypeInfo = ConvertType(param.Type, func.FunctionName, true, param.Name);
                        ptrParams.Add($"{paramTypeInfo.CsType} {param.Name}");
                    }
                }
                string ptrParamsStr = string.Join(", ", ptrParams);
                
                sb.AppendLine("    [LibraryImport(nativeLibName, EntryPoint = \"alBufferData\")]");
                sb.AppendLine("    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]");
                sb.AppendLine($"    internal static partial void alBufferDataPtr({ptrParamsStr});");
                sb.AppendLine();
            }
        }
        
        sb.AppendLine("}");
        
        return sb.ToString();
    }

    static string GenerateWrappers(List<FunctionDeclaration> functions, string className)
    {
        var sb = new StringBuilder();
        
        sb.AppendLine("namespace OpenAL;");
        sb.AppendLine();
        sb.AppendLine($"public static unsafe partial class {className}");
        sb.AppendLine("{");
        
        foreach (var func in functions)
        {
            var wrappers = GenerateWrapper(func);
            foreach (var wrapper in wrappers)
            {
                sb.AppendLine($"    {wrapper}");
            }
        }
        
        sb.AppendLine("}");
        
        return sb.ToString();
    }

    static List<string> GenerateWrapper(FunctionDeclaration func)
    {
        var wrappers = new List<string>();
        string internalName = func.FunctionName;
        string publicName = RemovePrefix(internalName);
        
        // Special case: Generate GetStringPtr wrapper for alcGetString
        if (internalName == "alcGetString")
        {
            var paramsList = func.Parameters.Select(p =>
            {
                var pType = ConvertType(p.Type, func.FunctionName, true, p.Name);
                return $"{pType.CsType} {p.Name}";
            });
            string paramList = string.Join(", ", paramsList);
            string callParams = string.Join(", ", func.Parameters.Select(p => p.Name));
            
            wrappers.Add($"public static IntPtr GetStringPtr({paramList}) => alcGetStringPtr({callParams});");
            wrappers.Add("");
        }
        
        // Special case: Generate GetIntegerALC helper for alcGetIntegerv
        if (internalName == "alcGetIntegerv")
        {
            wrappers.Add("public static int GetIntegerALC(IntPtr device, int param)");
            wrappers.Add("{");
            wrappers.Add("    int value = 0;");
            wrappers.Add("    alcGetIntegerv(device, param, 1, new Span<int>(ref value));");
            wrappers.Add("    return value;");
            wrappers.Add("}");
            wrappers.Add("");
        }
        
        // Special case: Generate pointer overload for alcCaptureSamples
        if (internalName == "alcCaptureSamples")
        {
            var ptrParams = new List<string>();
            foreach (var param in func.Parameters)
            {
                if (param.Name == "buffer")
                {
                    ptrParams.Add("nint buffer");
                }
                else
                {
                    var paramTypeInfo = ConvertType(param.Type, func.FunctionName, true, param.Name);
                    ptrParams.Add($"{paramTypeInfo.CsType} {param.Name}");
                }
            }
            string ptrParamList = string.Join(", ", ptrParams);
            string ptrCallParams = string.Join(", ", func.Parameters.Select(p => p.Name));
            
            wrappers.Add($"public static void CaptureSamples({ptrParamList}) => alcCaptureSamplesPtr({ptrCallParams});");
            wrappers.Add("");
        }
        
        // Special case: Generate pointer overload for alBufferData
        if (internalName == "alBufferData")
        {
            var ptrParams = new List<string>();
            foreach (var param in func.Parameters)
            {
                if (param.Name == "data")
                {
                    ptrParams.Add("nint data");
                }
                else
                {
                    var paramTypeInfo = ConvertType(param.Type, func.FunctionName, true, param.Name);
                    ptrParams.Add($"{paramTypeInfo.CsType} {param.Name}");
                }
            }
            string ptrParamList = string.Join(", ", ptrParams);
            string ptrCallParams = string.Join(", ", func.Parameters.Select(p => p.Name));
            
            wrappers.Add($"public static void BufferData({ptrParamList}) => alBufferDataPtr({ptrCallParams});");
            wrappers.Add("");
        }
        
        // Gen* pattern - create both single and array versions
        if (internalName.StartsWith("alGen") || internalName.StartsWith("alcGen"))
        {
            string itemName = publicName[3..]; // Remove "Gen"
            if (itemName.EndsWith('s'))
                itemName = itemName[..^1];
            
            // Single version: uint GenBuffer()
            wrappers.Add($"public static uint {publicName.TrimEnd('s')}() => {publicName}(1)[0];");
            
            // Array version: uint[] GenBuffers(int count)
            wrappers.Add($"public static uint[] {publicName}(int count)");
            wrappers.Add("{");
            wrappers.Add("    var result = new uint[count];");
            wrappers.Add($"    {internalName}(count, result);");
            wrappers.Add("    return result;");
            wrappers.Add("}");
            wrappers.Add("");
            
            return wrappers;
        }
        
        // Delete* pattern - create both single and span versions
        if (internalName.StartsWith("alDelete") || internalName.StartsWith("alcDelete"))
        {
            string itemName = publicName[6..]; // Remove "Delete"
            string singularName = itemName.TrimEnd('s');
            
            // Single version: void DeleteBuffer(uint buffer)
            wrappers.Add($"public static void {publicName.TrimEnd('s')}(uint {singularName.ToLower()}) => {publicName}([{singularName.ToLower()}]);");
            
            // Span version: void DeleteBuffers(ReadOnlySpan<uint> buffers)
            wrappers.Add($"public static void {publicName}(ReadOnlySpan<uint> {itemName.ToLower()}) => {internalName}({itemName.ToLower()}.Length, {itemName.ToLower()});");
            wrappers.Add("");
            
            return wrappers;
        }
        
        // Get* functions that return a single value via out parameter
        if ((internalName.StartsWith("alGet") || internalName.StartsWith("alcGet")) && !internalName.EndsWith('v'))
        {
            var returnTypeInfo = ConvertType(func.ReturnType, func.FunctionName, false);
            if (returnTypeInfo.CsType == "void" && func.Parameters.Count >= 2)
            {
                // Find the out parameter
                var lastParam = func.Parameters[^1];
                var lastParamType = ConvertType(lastParam.Type, func.FunctionName, true, lastParam.Name);
                
                if (lastParamType.CsType.StartsWith("out "))
                {
                    string valueType = lastParamType.CsType[4..]; // Remove "out "
                    
                    // Check if this is a multi-value function (e.g., GetSource3f with value1, value2, value3)
                    // Count how many parameters are out parameters that start with "value"
                    var outValueParams = func.Parameters.Where(p =>
                    {
                        var pType = ConvertType(p.Type, func.FunctionName, true, p.Name);
                        return pType.CsType.StartsWith("out ") && p.Name.StartsWith("value");
                    }).ToList();
                    
                    // If we have multiple value parameters (value1, value2, value3), keep them as out parameters
                    if (outValueParams.Count > 1)
                    {
                        var allParams = func.Parameters.Select(p =>
                        {
                            var pType = ConvertType(p.Type, func.FunctionName, true, p.Name);
                            return $"{pType.CsType} {p.Name}";
                        });
                        
                        string paramListMulti = string.Join(", ", allParams);
                        string callParamsMulti = string.Join(", ", func.Parameters.Select(p =>
                        {
                            var pType = ConvertType(p.Type, func.FunctionName, true, p.Name);
                            return pType.CsType.StartsWith("out ") ? $"out {p.Name}" : p.Name;
                        }));
                        
                        wrappers.Add($"public static void {publicName}({paramListMulti}) => {internalName}({callParamsMulti});");
                        wrappers.Add("");
                        
                        return wrappers;
                    }
                    
                    // Single value - return it
                    var otherParams = func.Parameters.Take(func.Parameters.Count - 1).Select(p =>
                    {
                        var pType = ConvertType(p.Type, func.FunctionName, true, p.Name);
                        return $"{pType.CsType} {p.Name}";
                    });
                    
                    string paramListSingle = string.Join(", ", otherParams);
                    string callParamsSingle = string.Join(", ", func.Parameters.Take(func.Parameters.Count - 1).Select(p => p.Name));
                    
                    wrappers.Add($"public static {valueType} {publicName}({paramListSingle})");
                    wrappers.Add("{");
                    wrappers.Add($"    {internalName}({callParamsSingle}, out {valueType} value);");
                    wrappers.Add("    return value;");
                    wrappers.Add("}");
                    wrappers.Add("");
                    
                    return wrappers;
                }
            }
        }

        // *v functions with count parameter - create span wrapper
        //  || internalName.EndsWith("Playv") || internalName.EndsWith("Stopv") || 
        // internalName.EndsWith("Pausev") || internalName.EndsWith("Rewindv")
        if (internalName.EndsWith('v') && 
            func.Parameters.Count >= 2)
        {
            var countParam = func.Parameters[0];
            var arrayParam = func.Parameters[1];
            
            if (countParam.Name == "n" || countParam.Name == "count" || countParam.Name == "nb")
            {
                var arrayParamType = ConvertType(arrayParam.Type, func.FunctionName, true, arrayParam.Name);
                string spanType = arrayParamType.CsType.Contains("ReadOnlySpan") ? arrayParamType.CsType : arrayParamType.CsType.Replace("Span<", "ReadOnlySpan<");
                
                wrappers.Add($"public static void {publicName}({spanType} {arrayParam.Name}) => {internalName}({arrayParam.Name}.Length, {arrayParam.Name});");
                wrappers.Add("");
                
                return wrappers;
            }
        }
        
        // Simple pass-through wrapper - just remove prefix
        var wrapperParams = func.Parameters.Select(p =>
        {
            var pType = ConvertType(p.Type, func.FunctionName, true, p.Name);
            return $"{pType.CsType} {p.Name}";
        }).ToList();
        
        var returnTypeInfoFinal = ConvertType(func.ReturnType, func.FunctionName, false);
        string paramListFinal = string.Join(", ", wrapperParams);
        
        // Build call params, adding "out" keyword where needed
        var callParamsList = new List<string>();
        for (int i = 0; i < func.Parameters.Count; i++)
        {
            var param = func.Parameters[i];
            var pType = ConvertType(param.Type, func.FunctionName, true, param.Name);
            
            if (pType.CsType.StartsWith("out "))
            {
                callParamsList.Add($"out {param.Name}");
            }
            else
            {
                callParamsList.Add(param.Name);
            }
        }
        string callParamsFinal = string.Join(", ", callParamsList);
        
        if (returnTypeInfoFinal.CsType == "void")
        {
            wrappers.Add($"public static void {publicName}({paramListFinal}) => {internalName}({callParamsFinal});");
        }
        else
        {
            wrappers.Add($"public static {returnTypeInfoFinal.CsType} {publicName}({paramListFinal}) => {internalName}({callParamsFinal});");
        }
        wrappers.Add("");
        
        return wrappers;
    }

    static string RemovePrefix(string functionName)
    {
        if (functionName.StartsWith("al") && !functionName.StartsWith("alc"))
            return functionName[2..];
        if (functionName.StartsWith("alc"))
            return functionName[3..];
        return functionName;
    }

    static TypeInfo ConvertType(string cType, string functionName, bool isParameter = false, string paramName = "")
    {
        // Remove const qualifier
        bool isConst = cType.Contains("const");
        cType = cType.Replace("const", "").Trim();
        
        // Handle pointers
        bool isPointer = cType.Contains('*');
        cType = cType.Replace("*", "").Trim();
        
        // Special handling for function pointer types (delegates)
        if (cType.Contains("CALLBACK") || cType.Contains("PROC") || 
            cType == "ALEVENTPROCSOFT" || cType == "ALBUFFERCALLBACKTYPESOFT" ||
            cType == "LPALFOLDBACKCALLBACK" || cType == "LPALEVENTPROCSOFT")
        {
            // For callback/function pointer types, use IntPtr in bindings
            // (Users should define the delegate type themselves)
            return new TypeInfo { CsType = "IntPtr" };
        }
        
        // Map C types to C# types
        string baseType = cType switch
        {
            "void" => "void",
            "ALvoid" => "void",
            "ALCvoid" => "void",
            "ALboolean" => "bool",
            "ALCboolean" => "bool",
            "ALchar" => "byte",
            "ALCchar" => "byte",
            "ALbyte" => "sbyte",
            "ALCbyte" => "sbyte",
            "ALubyte" => "byte",
            "ALCubyte" => "byte",
            "ALshort" => "short",
            "ALCshort" => "short",
            "ALushort" => "ushort",
            "ALCushort" => "ushort",
            "ALint" => "int",
            "ALCint" => "int",
            "ALuint" => "uint",
            "ALCuint" => "uint",
            "ALsizei" => "int",
            "ALCsizei" => "int",
            "ALenum" => "int",
            "ALCenum" => "int",
            "ALfloat" => "float",
            "ALCfloat" => "float",
            "ALdouble" => "double",
            "ALCdouble" => "double",
            "ALint64SOFT" => "long",
            "ALCint64SOFT" => "long",
            "ALuint64SOFT" => "ulong",
            "ALCuint64SOFT" => "ulong",
            "ALCdevice" => "IntPtr",
            "ALCcontext" => "IntPtr",
            _ => cType
        };
        
        // Special handling for void/bool in Span context - use byte for marshaling
        string spanType = (baseType == "bool" || baseType == "void") ? "byte" : baseType;
        
        // Handle return types
        if (!isParameter)
        {
            if (isPointer)
            {
                // Check if it's a string return type (const ALchar* or const ALCchar*)
                if ((baseType == "byte" && (cType == "ALchar" || cType == "ALCchar") && isConst))
                    return new TypeInfo { CsType = "string" };
                
                return new TypeInfo { CsType = "IntPtr" };
            }
            return new TypeInfo { CsType = baseType };
        }
        
        // Handle parameter types
        if (isPointer)
        {
            // Check if it's a string parameter (const ALchar* or const ALCchar*)
            if (baseType == "byte" && (cType == "ALchar" || cType == "ALCchar") && isConst)
                return new TypeInfo { CsType = "string" };
            
            // Check if it's a void pointer (ALvoid* or void* or ALCvoid*)
            if (baseType == "void")
            {
                // For void* in buffer/data contexts, use Span<byte>
                if (paramName == "buffer" || paramName == "data" || paramName == "ptr")
                {
                    if (isConst)
                        return new TypeInfo { CsType = "ReadOnlySpan<byte>" };
                    else
                        return new TypeInfo { CsType = "Span<byte>" };
                }
                return new TypeInfo { CsType = "IntPtr" };
            }
            
            // Check if it's an ALC device or context pointer
            if (baseType == "IntPtr")
                return new TypeInfo { CsType = "IntPtr" };
            
            // Check if it's a Get function with pointer parameters
            bool isGetFunction = functionName.StartsWith("alGet") || functionName.StartsWith("alcGet");
            
            // Special cases for functions that should use ReadOnlySpan
            bool isUnqueueBuffers = functionName == "alSourceUnqueueBuffers";
            
            if (isGetFunction)
            {
                // Functions ending in 'v' return arrays (e.g., alGetFloatv, alGetIntegerv)
                bool isVectorFunction = functionName.EndsWith('v');
                
                if (isVectorFunction)
                {
                    // Use Span for vector/array parameters (use byte for bool/void)
                    return new TypeInfo { CsType = $"Span<{spanType}>" };
                }
                
                // Check if it's a multi-value parameter (value1, value2, value3)
                bool isIndividualValue = paramName.StartsWith("value") && paramName.Length > 5;
                
                if (isIndividualValue)
                {
                    // Use out parameters for individual values
                    return new TypeInfo { CsType = $"out {baseType}" };
                }
                else if (paramName == "value" || paramName == "data")
                {
                    // Single value parameter - use out
                    return new TypeInfo { CsType = $"out {baseType}" };
                }
                else
                {
                    // Use Span for array parameters (values) (use byte for bool/void)
                    return new TypeInfo { CsType = $"Span<{spanType}>" };
                }
            }
            
            // For non-Get functions with const pointers, use ReadOnlySpan (use byte for bool/void)
            if (isConst || isUnqueueBuffers)
            {
                return new TypeInfo { CsType = $"ReadOnlySpan<{spanType}>" };
            }
            
            // For non-const pointer arrays in non-Get functions, use Span (use byte for bool/void)
            if (paramName.EndsWith('s') || paramName == "data" || paramName == "buffer")
            {
                return new TypeInfo { CsType = $"Span<{spanType}>" };
            }
            
            // For other pointer parameters, use out
            return new TypeInfo { CsType = $"out {baseType}" };
        }
        
        return new TypeInfo { CsType = baseType };
    }

    [GeneratedRegex(@"^\s*#define\s+([A-Z_][A-Z0-9_]*)\s+(0x[0-9A-Fa-f]+|[0-9]+)\s*$", RegexOptions.Compiled)]
    private static partial Regex ConstantsRegex();

    [GeneratedRegex(@"\s+")]
    private static partial Regex SplitRegex();
}

class FunctionDeclaration
{
    public string ReturnType { get; set; }
    public string FunctionName { get; set; }
    public List<Parameter> Parameters { get; set; }
}

class Parameter
{
    public string Type { get; set; }
    public string Name { get; set; }
}

class TypeInfo
{
    public string CsType { get; set; }
}
#endif