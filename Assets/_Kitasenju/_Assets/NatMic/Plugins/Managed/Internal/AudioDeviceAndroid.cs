/* 
*   NatMic
*   Copyright (c) 2019 Yusuf Olokoba
*/

namespace NatMic.Internal {

    using UnityEngine;
    using UnityEngine.Scripting;
    using System;

    public sealed class AudioDeviceAndroid : AudioDevice {

        #region --Intropection--

        public new static AudioDeviceAndroid[] GetDevices () {
            AudioDevice = AudioDevice ?? new AndroidJavaClass(@"com.yusufolokoba.natmic.AudioDevice");
            System = System ?? new AndroidJavaClass(@"java.lang.System");
            using (var devicesArray = AudioDevice.CallStatic<AndroidJavaObject>(@"devices")) {
                var devices = AndroidJNIHelper.ConvertFromJNIArray<AndroidJavaObject[]>(devicesArray.GetRawObject());
                var result = new AudioDeviceAndroid[devices.Length];
                for (var i = 0; i < devices.Length; i++)
                    result[i] = new AudioDeviceAndroid(devices[i]);
                return result;
            }
        }

        public new static long CurrentTimestamp {
            get { return System.CallStatic<long>(@"nanoTime"); }
        }
        #endregion


        #region --AudioDevice--

        public override string Name {
            get { return device.Call<string>(@"name"); }
        }

        public override string UniqueID {
            get { return device.Call<string>(@"uniqueID"); }
        }

        public override bool EchoCancellation {
            get { return device.Call<bool>(@"echoCancellation"); }
        }
        #endregion


        #region --IAudioDevice--

        public override bool IsRecording {
            get { return device.Call<bool>(@"isRecording"); }
        }

        public override void StartRecording (int sampleRate, int channelCount, IAudioProcessor processor) {
            this.processor = processor;
            device.Call(@"startRecording", sampleRate, channelCount, new SampleBufferDelegate(this));
        }

        public override void StopRecording () {
            this.processor = null;
            device.Call(@"stopRecording");
        }
        #endregion


        #region --Operations--
        
        private readonly AndroidJavaObject device;
        private volatile IAudioProcessor processor;
        private static AndroidJavaClass AudioDevice;
        private static AndroidJavaClass System;

        private AudioDeviceAndroid (AndroidJavaObject device) {
            this.device = device;
        }

        ~AudioDeviceAndroid () {
            device.Dispose();
        }

        private class SampleBufferDelegate : AndroidJavaProxy {

            private readonly AudioDeviceAndroid device;

            public SampleBufferDelegate (AudioDeviceAndroid device) : base(@"com.yusufolokoba.natmic.AudioDevice$Callback") {
                this.device = device;
            }

            [Preserve]
            private void onSampleBuffer (AndroidJavaObject frame) {
                // Marshal sample buffer
                float[] sampleBuffer;
                using (var nativeSampleBuffer = frame.Get<AndroidJavaObject>(@"sampleBuffer"))
                    sampleBuffer = AndroidJNI.FromFloatArray(nativeSampleBuffer.GetRawObject());
                var sampleRate = frame.Get<int>(@"sampleRate");
                var channelCount = frame.Get<int>(@"channelCount");
                var timestamp = frame.Get<long>(@"timestamp");
                // Pass to processor
                try {
                    device.processor.OnSampleBuffer(sampleBuffer, sampleRate, channelCount, timestamp);
                } catch (Exception ex) {
                    Debug.LogError("NatMic Error: AudioDevice processor raised exception: "+ex);
                } finally {
                    frame.Dispose();
                }
            }
        }
        #endregion
    }
}