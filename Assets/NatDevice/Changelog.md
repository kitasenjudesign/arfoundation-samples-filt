## NatDevice 1.0.1
+ Updated top-level namespace to `NatSuite.Devices` for parity with other NatSuite API's.
+ Fixed camera device query crash on Galaxy S10 and S10+.
+ Fixed sporadic crashes on some Android devices when the camera preview is started.
+ Fixed crash due to JNI local reference table overflow on Android.
+ Fixed `MediaDeviceQuery.Criteria.FrontFacing` not finding any cameras on iOS.
+ Fixed iOS archive generating error due to NatDevice not being built with full bitcode generation.

## NatDevice 1.0.0
+ First release.