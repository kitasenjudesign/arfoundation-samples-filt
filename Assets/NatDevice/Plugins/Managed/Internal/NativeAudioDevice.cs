/* 
*   NatDevice
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.Devices.Internal {

    using AOT;
    using UnityEngine;
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    public class NativeAudioDevice : AudioDevice {

        #region --Properties--

        public override string uniqueID {
            get {
                var result = new StringBuilder(1024);
                device.UniqueID(result);
                return result.ToString();
            }
        }

        public override string name {
            get {
                var result = new StringBuilder(1024);
                device.Name(result);
                return result.ToString();
            }
        }
        
        public override bool echoCancellation => device.EchoCancellation();

        public override int sampleRate {
            get => device.SampleRate();
            set => device.SampleRate(value);
        }

        public override int channelCount {
            get => device.ChannelCount();
            set => device.ChannelCount(value);
        }
        #endregion


        #region --Recording--

        public override bool running => device.Running();

        public override void StartRunning (SampleBufferDelegate @delegate) {
            Action<float[], long> handler = (sampleBuffer, timestamp) => {
                try { @delegate(sampleBuffer, timestamp); }
                catch (Exception ex) { Debug.LogError($"NatDevice Error: Sample buffer delegate raised exception: {ex}"); }
            };
            device.StartRunning(OnSampleBuffer, (IntPtr)GCHandle.Alloc(handler, GCHandleType.Normal));
        }

        public override void StopRunning () => device.StopRunning();
        #endregion


        #region --Operations--

        private readonly IntPtr device;

        public NativeAudioDevice (IntPtr device) => this.device = device;

        ~NativeAudioDevice () => device.Dispose();

        [MonoPInvokeCallback(typeof(Bridge.SampleBufferDelegate))]
        private static void OnSampleBuffer (IntPtr context, IntPtr sampleBuffer, int sampleCount, long timestamp) {
            var samples = new float[sampleCount];
            Marshal.Copy(sampleBuffer, samples, 0, sampleCount);
            (((GCHandle)context).Target as Action<float[], long>)(samples, timestamp);
        }
        #endregion
    }
}