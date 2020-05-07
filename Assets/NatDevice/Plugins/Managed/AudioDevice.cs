/* 
*   NatDevice
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.Devices {

    using Internal;
    
    /// <summary>
    /// Abstraction for a hardware audio input device.
    /// </summary>
    [Doc(@"AudioDevice")]
    public abstract class AudioDevice : IAudioDevice {
        
        #region --Properties--
        /// <summary>
        /// Device unique ID.
        /// </summary>
        [Doc(@"UniqueID")]
        public abstract string uniqueID { get; }

        /// <summary>
        /// Display friendly device name.
        /// </summary>
        [Doc(@"Name")]
        public abstract string name { get; }

        /// <summary>
        /// Does this device have Adaptive Echo Cancellation?
        /// </summary>
        [Doc(@"EchoCancellation")]
        public abstract bool echoCancellation { get; }

        /// <summary>
        /// Audio sample rate.
        /// </summary>
        [Doc(@"SampleRate")]
        public abstract int sampleRate { get; set; }

        /// <summary>
        /// Audio channel count.
        /// </summary>
        [Doc(@"ChannelCount")]
        public abstract int channelCount { get; set; }
        #endregion


        #region --Operations--
        /// <summary>
        /// Is the device running?
        /// </summary>
        [Doc(@"Running")]
        public abstract bool running { get; }

        /// <summary>
        /// Start running.
        /// </summary>
        /// <param name="@delegate">Delegate to receive sample buffers.</param>
        [Doc(@"StartRecording")]
        public abstract void StartRunning (SampleBufferDelegate @delegate);
        
        /// <summary>
        /// Stop running.
        /// </summary>
        [Doc(@"StopRunning")]
        public abstract void StopRunning ();
        #endregion


        #region --Operations--

        public bool Equals (IMediaDevice other) => other != null && other is AudioDevice && other.uniqueID == uniqueID;

        public override string ToString () => $"microphone:{uniqueID}";
        #endregion
    }
}