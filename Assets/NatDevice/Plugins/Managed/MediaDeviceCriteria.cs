/* 
*   NatDevice
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.Devices {

    using Internal;

    public sealed partial class MediaDeviceQuery {

        /// <summary>
        /// Criterion used to filter devices.
        /// </summary>
        [Doc(@"Criterion")]
        public delegate bool Criterion (IMediaDevice device);

        /// <summary>
        /// Common criteria used to filter devices.
        /// </summary>
        [Doc(@"Criteria")]
        public static class Criteria {

            /// <summary>
            /// Filter for hardware audio devices.
            /// </summary>
            [Doc(@"CriterionAudioDevice")]
            public static readonly Criterion AudioDevice = device => device is AudioDevice; // Not `IAudioDevice` because there are no other implementations

            /// <summary>
            /// Filter for hardware camera devices.
            /// </summary>
            [Doc(@"CriterionCameraDevice")]
            public static readonly Criterion CameraDevice = device => device is CameraDevice;

            /// <summary>
            /// Filter for generic camera devices.
            /// This criterion will filter for either `CameraDevice` or `WebCameraDevice` instances.
            /// </summary>
            [Doc(@"CriterionGenericCameraDevice")]
            public static readonly Criterion GenericCameraDevice = device => device is ICameraDevice;

            /// <summary>
            /// Filter for rear-facing camera devices.
            /// </summary>
            [Doc(@"CriterionRearFacing")]
            public static readonly Criterion RearFacing = device => device is ICameraDevice camera && !camera.frontFacing;

            /// <summary>
            /// Filter for front-facing camera devices.
            /// </summary>
            [Doc(@"CriterionFrontFacing")]
            public static readonly Criterion FrontFacing = device => device is ICameraDevice camera && camera.frontFacing;

            /// <summary>
            /// Filter for microphones with echo cancellation.
            /// </summary>
            [Doc(@"CriterionEchoCancellation")]
            public static readonly Criterion EchoCancellation = device => device is AudioDevice microphone && microphone.echoCancellation;
            
            /// <summary>
            /// Filter for camera devices that have torches.
            /// </summary>
            [Doc(@"CriterionTorch")]
            public static readonly Criterion Torch = device => device is CameraDevice camera && camera.torchSupported;
        }
    }
}