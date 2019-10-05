/* 
*   NatMic
*   Copyright (c) 2019 Yusuf Olokoba
*/

namespace NatMic.Recorders {

    using UnityEngine;
    using System;
    using System.IO;
    using Internal;

    /// <summary>
    /// Recorder for recording audio to WAV files
    /// </summary>
    [Doc(@"WAVRecorder")]
    public sealed class WAVRecorder : IAudioRecorder {

        #region --Client API--
        /// <summary>
        /// WAV file path.
        /// This property can still be used after the instance has been disposed.
        /// </summary>
        [Doc(@"WAVRecorderFilePath")]
        public readonly string FilePath;

        /// <summary>
        /// Create a WAV recorder
        /// </summary>
        /// <param name="filePath">Path where WAV file will be written to</param>
        [Doc(@"WAVRecorderCtor")]
        public WAVRecorder (string filePath) {
            this.FilePath = filePath;
        }

        /// <summary>
        /// Stop writing and invoke recording callback
        /// </summary>
        [Doc(@"Dispose")]
        public void Dispose () {
            if (outputStream == null)
                return;
            WriteHeader();
            outputStream.Dispose();
        }
        #endregion


        #region --Operations--

        private int sampleRate, channelCount;
        private FileStream outputStream;
        private int totalSamples;

        void IAudioProcessor.OnSampleBuffer (float[] sampleBuffer, int sampleRate, int channelCount, long timestamp) {
            // Create output stream
            if (outputStream == null) {
                this.sampleRate = sampleRate;
                this.channelCount = channelCount;
                outputStream = new FileStream(FilePath, FileMode.Create);
                // Allocate header
                const byte @null = new byte();
                for (int i = 0; i < 44; i++)
                    outputStream.WriteByte(@null);   
            }
            // Convert to short array then write to byte buffer
            var shortBuffer = new short[sampleBuffer.Length];
            var byteBuffer = new byte[Buffer.ByteLength(shortBuffer)];
            for (int i = 0; i < sampleBuffer.Length; i++) 
                shortBuffer[i] = (short)(sampleBuffer[i] * short.MaxValue);
            Buffer.BlockCopy(shortBuffer, 0, byteBuffer, 0, byteBuffer.Length);
            // Write
            outputStream.Write(byteBuffer, 0, byteBuffer.Length);
            totalSamples += sampleBuffer.Length;
        }

        private void WriteHeader () {
            outputStream.Seek(0, SeekOrigin.Begin);
            outputStream.Write(System.Text.Encoding.UTF8.GetBytes("RIFF"), 0, 4);
            outputStream.Write(BitConverter.GetBytes(outputStream.Length - 8), 0, 4);
            outputStream.Write(System.Text.Encoding.UTF8.GetBytes("WAVE"), 0, 4);
            outputStream.Write(System.Text.Encoding.UTF8.GetBytes("fmt "), 0, 4);
            outputStream.Write(BitConverter.GetBytes(16), 0, 4);
            outputStream.Write(BitConverter.GetBytes((ushort)1), 0, 2);
            outputStream.Write(BitConverter.GetBytes(channelCount), 0, 2);                              // Channel count
            outputStream.Write(BitConverter.GetBytes(sampleRate), 0, 4);                                // Sample rate
            outputStream.Write(BitConverter.GetBytes(sampleRate * channelCount * sizeof(short)), 0, 4); // Output rate in bytes
            outputStream.Write(BitConverter.GetBytes((ushort)(channelCount * 2)), 0, 2);                // Block alignment
            outputStream.Write(BitConverter.GetBytes((ushort)16), 0, 2);                                // Bits per sample
            outputStream.Write(System.Text.Encoding.UTF8.GetBytes("data"), 0, 4);
            outputStream.Write(BitConverter.GetBytes(totalSamples * sizeof(ushort)), 0, 4);             // Total sample count
        }
        #endregion
    }
}