/* 
*   NatMic
*   Copyright (c) 2019 Yusuf Olokoba
*/

namespace NatMic.Internal {

    using AOT;
    using UnityEngine;
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    public sealed class AudioDeviceiOS : AudioDevice {

        #region --Introspection--

        public new static AudioDeviceiOS[] GetDevices () {
            IntPtr deviceArray;
            int deviceCount;
            AudioDeviceBridge.Devices(out deviceArray, out deviceCount);
            var devices = new AudioDeviceiOS[deviceCount];
            for (int i = 0; i < deviceCount; i++) {
                var device = Marshal.ReadIntPtr(deviceArray, i * Marshal.SizeOf(typeof(IntPtr)));
                devices[i] = new AudioDeviceiOS(device);
            }
            Marshal.FreeCoTaskMem(deviceArray);
            return devices;
        }

        public new static long CurrentTimestamp {
            get { return AudioDeviceBridge.CurrentTimestamp(); }
        }
        #endregion


        #region --AudioDevice--

        public override string Name {
            get {
                var result = new StringBuilder(1024);
                device.DeviceName(result);
                return result.ToString();
            }
        }

        public override string UniqueID {
            get {
                var result = new StringBuilder(1024);
                device.DeviceUniqueID(result);
                return result.ToString();
            }
        }
        
        public override bool EchoCancellation {
            get { return device.DeviceEchoCancellation(); }
        }
        #endregion


        #region --IAudioDevice--

        public override bool IsRecording {
            get { return device.IsRecording(); }
        }

        public override void StartRecording (int sampleRate, int channelCount, IAudioProcessor processor) {
            this.self = GCHandle.Alloc(this, GCHandleType.Normal);
            this.processor = processor;
            device.StartRecording(sampleRate, channelCount, OnSampleBuffer, (IntPtr)self);
        }

        public override void StopRecording () {
            device.StopRecording();
            this.processor = null;
            self.Free();
            self = default(GCHandle);
        }
        #endregion


        #region --Operations--

        private readonly IntPtr device;
        private GCHandle self;
        private volatile IAudioProcessor processor;

        private AudioDeviceiOS (IntPtr device) {
            this.device = device;
        }

        ~AudioDeviceiOS () {
            device.FreeDevice();
        }

        [MonoPInvokeCallback(typeof(AudioDeviceBridge.SampleBufferCallback))]
        private static void OnSampleBuffer (IntPtr context, IntPtr sampleBuffer, int sampleCount, int sampleRate, int channelCount, long timestamp) {            
            // Get device
            var deviceRef = (GCHandle)context;
            var device = deviceRef.Target as AudioDeviceiOS;
            // Null checking
            if (device == null)
                return;
            // Marshal sample buffer
            var samples = new float[sampleCount];
            Marshal.Copy(sampleBuffer, samples, 0, sampleCount);
            // Pass to processor
            try {
                device.processor.OnSampleBuffer(samples, sampleRate, channelCount, timestamp);
            } catch (Exception ex) {
                Debug.LogError("NatMic Error: AudioDevice processor raised exception: "+ex);
            }
        }
        #endregion
    }
}