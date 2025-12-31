using System.Collections.Concurrent;

namespace OpenAL.managed;

public unsafe class ALStreamSourceBuffer
{
    public byte[] data;
    public byte* dataUnsafe;
    public int offset;
    public int length;

    internal int originalOffset;
    internal int originalLength;
}

public unsafe class ALStreamSource : ALSource
{
    uint bufferID;
    AL.ALBufferCallbackTypeSoft bufferCallback;

    public ALStreamSource(uint sourceID, int inputFormat, int frequency) : base(sourceID)
    {
        bufferID = AL.GenBuffer();

        // Store delegate to prevent GC
        bufferCallback = OnBufferCallback;

        // This source will get its sound data from OnBufferCallback()
        IntPtr callbackPtr = Marshal.GetFunctionPointerForDelegate(bufferCallback);
        AL.BufferCallbackSOFT(bufferID, inputFormat, frequency, callbackPtr, IntPtr.Zero);

        AL.Sourcei(sourceID, AL.AL_BUFFER, (int)bufferID);
        AL.SourcePlay(sourceID);
    }

    public override void Dispose()
    {
        base.Dispose();
        AL.DeleteBuffer(bufferID);
    }

    /// <summary>
    /// Enqueues audio data from a managed byte array to be streamed to OpenAL.
    /// </summary>
    /// <param name="data">The byte array containing the audio data</param>
    /// <param name="offset">The offset within the array to start reading from</param>
    /// <param name="length">The number of bytes to read from the array</param>
    public void EnqueueData(byte[] data, int offset, int length)
    {
        enqueuedBuffers.Add(new()
        {
            data = data,
            offset = offset,
            originalOffset = offset,
            length = length,
            originalLength = length
        });
    }

    /// <summary>
    /// Enqueues audio data from an unmanaged byte pointer to be streamed to OpenAL.
    /// </summary>
    /// <param name="data">Pointer to the audio data buffer</param>
    /// <param name="offset">The offset within the buffer to start reading from</param>
    /// <param name="length">The number of bytes to read from the buffer</param>
    public void EnqueueData(byte* data, int offset, int length)
    {
        enqueuedBuffers.Add(new()
        {
            dataUnsafe = data,
            offset = offset,
            originalOffset = offset,
            length = length,
            originalLength = length
        });
    }

    /// <summary>
    /// Attempts to retrieve a buffer that has been fully consumed by OpenAL and can be freed or recycled.
    /// Keep calling this until it returns false to retrieve all used buffers.
    /// </summary>
    /// <param name="buffer">The consumed buffer that can be freed or recycled</param>
    /// <returns>True if a used buffer was retrieved, false if no buffers are available</returns>
    public bool TryGetUsedData(out ALStreamSourceBuffer buffer)
    {
        return usedBuffers.TryTake(out buffer, 0);
    }

    readonly BlockingCollection<ALStreamSourceBuffer> enqueuedBuffers = [];
    readonly BlockingCollection<ALStreamSourceBuffer> usedBuffers = [];    
    ALStreamSourceBuffer partialBuffer = null;

    int OnBufferCallback(void* userData, void* sampleData, int numBytes)
    {
        Debug.Assert(userData == null);
        Debug.Assert(sampleData != null);

        byte* write = (byte*)sampleData;
        var bytesLeft = numBytes;

        while (bytesLeft > 0)
        {
            ALStreamSourceBuffer buffer;
            
            // Check if we have a partial buffer from last time
            if (partialBuffer != null)
            {
                buffer = partialBuffer;
                partialBuffer = null;
            }
            else if (!enqueuedBuffers.TryTake(out buffer, 0))
            {
                break;
            }

            // Figure out how many bytes to copy
            var bytesToWrite = Math.Min(buffer.length, bytesLeft);

            // Send data to OpenAL
            if (buffer.data != null)
            {
                fixed (byte* src = buffer.data)
                {
                    var read = src + buffer.offset;
                    NativeMemory.Copy(read, write, (nuint)bytesToWrite);
                }
            }
            else
            {
                var read = buffer.dataUnsafe + buffer.offset;
                NativeMemory.Copy(read, write, (nuint)bytesToWrite);
            }

            // Track how much is left
            write += bytesToWrite;
            bytesLeft -= bytesToWrite;

            // If we only partially consumed a buffer, save it for the next callback
            if (bytesToWrite < buffer.length)
            {
                buffer.offset += bytesToWrite;
                buffer.length -= bytesToWrite;
                partialBuffer = buffer;
            }
            else
            {
                // Fully consumed, restore original values
                buffer.offset = buffer.originalOffset;
                buffer.length = buffer.originalLength;
                usedBuffers.Add(buffer);
            }
        }

        // If we didn't have enough input, fill the rest of the buffer with silence
        if (bytesLeft > 0)
        {
            NativeMemory.Clear(write, (nuint)bytesLeft);
            write += bytesLeft;
        }

        // Ensure we filled the buffer
        Debug.Assert(write == (byte*)sampleData + numBytes);

        return numBytes;
    }
}
