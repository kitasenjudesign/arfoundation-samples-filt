/* 
*   NatDevice
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.Devices {

    using Internal;

    /// <summary>
    /// Photo flash mode.
    /// </summary>
    [Doc(@"FlashMode")]
    public enum FlashMode : int {
        /// <summary>
        /// Never use flash.
        /// </summary>
        [Doc(@"FlashModeOff")]
        Off = 0,
        /// <summary>
        /// Always use flash.
        /// </summary>
        [Doc(@"FlashModeOn")]
        On = 1,
        /// <summary>
        /// Let the sensor detect if it needs flash.
        /// </summary>
        [Doc(@"FlashModeAuto")]
        Auto = 2
    }

    /// <summary>
    /// Frame orientation.
    /// </summary>
    [Doc(@"FrameOrientation")]
    public enum FrameOrientation : int {
        /// <summary>
        /// Landscape left.
        /// </summary>
        [Doc(@"FrameOrientationLandscapeLeft")]
        LandscapeLeft = 0,
        /// <summary>
        /// Portrait.
        /// </summary>
        [Doc(@"FrameOrientationPortrait")]
        Portrait = 1,
        /// <summary>
        /// Landscape right.
        /// </summary>
        [Doc(@"FrameOrientationLandscapeRight")]
        LandscapeRight = 2,
        /// <summary>
        /// Portrait upside down.
        /// </summary>
        [Doc(@"FrameOrientationPortraitUpsideDown")]
        PortraitUpsideDown = 3
    }
}