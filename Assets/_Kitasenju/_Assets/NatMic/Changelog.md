## NatMic 1.3.1
+ Refactored `AudioDevice.Devices` property to `AudioDevice.GetDevices` method.
+ Modified `MixerDevice` to take only two source devices, instead of multiple.
+ Improved `AudioDevice.StartRecording` latency on iOS.
+ Fixed AirPods not being used for audio input on iOS.
+ Fixed sporadic `GCHandle` error when audio device stops recording on Windows.
+ Fixed sporadic `NullPointerException` when audio device stops recording on Android.
+ Fixed `AudioDevice.Name` and `AudioDevice.UniqueID` properties returning garbled text on Windows.
+ Removed `NatMic.DSP` namespace.
+ Reduced minimum Android API requirement to API level 18.
+ NatCorder now requires Windows 10 64-bit.

## NatMic 1.3.0
+ Completely overhauled front-end API to provide a more device-oriented approach to handling audio input.
+ Added the concept of an audio device with a corresponding `IAudioDevice` interface, which is a source for audio data.
+ Added `AudioDevice` class where one instance corresponds to an available hardware audio input device.
+ Added `VirtualDevice` class that can be used as an audio device that is backed by a Unity `AudioSource` or `AudioListener` component.
+ Added `MixerDevice` virtual device which mixes audio from multiple `IAudioDevice` instances. This is useful for mixing game and microphone audio into one audio stream.
+ Added `IAudioProcessor` interface which receives audio data from an `IAudioDevice`.
+ Added support for running multiple audio input devices at the same time on platforms and devices that support it.
+ Fixed audio being re-routed to speakers when recording on iOS.
+ Refactored NatMic namespace from `NatMicU.Core` to `NatMic`.
+ Refactored `IRecorder` interface to `IAudioRecorder`.
+ Deprecated `NatMic` class.
+ Deprecated `SampleBufferCallback`.
+ Deprecated `Format` struct.
+ Deprecated `RealtimeClip` audio recorder.
+ NatMic now requires Android API level 23.
+ NatMic now requires iOS 10.

## NatMic 1.2.1
+ Fixed `ArrayOutOfBoundsException` when recording microphone audio with game audio on Android.
+ Fixed crash when recording microphone audio with game audio on iOS.

## NatMic 1.2.0
+ NatMic now supports multi-channel microphone audio. On platforms that don't support this, NatMic will automatically interleave the mono data across the number of requested channels.
+ NatMic now features a device enumeration API. You can now select which audio input device to record from. See the `Device` class.
+ Added ability to specify adaptive echo cancellation (AEC) on recording device.
+ Added `RealtimeClip` class for converting microphone audio to a Unity `AudioClip` in realtime.
+ Added a formal constructor for `Format` struct.
+ Changed `SampleBufferCallback` to take in only the sample buffer and timestamp. The callback will now only be called with new sample buffers, and not on initialize or finalize events.
+ Fixed crash when app is suspended after recording is stopped on iOS.
+ Deprecated `AudioEvent` enumeration.
+ Deprecated experimental WebGL backend as it is too unstable. We might reintroduce it at a later time.
+ Refactored `Format.DefaultWithMixing` to `Format.Unity`.

## NatMic 1.1f1
+ Added experimental support for WebGL! Check out the README for more information.
+ Added `ClipRecorder` for recording microphone audio to an `AudioClip`.
+ Moved `callback` parameter in `WAVRecorder` from `StartRecording` function to constructor.
+ Deprecated `RecordingCallback`.

## NatMic 1.0f1
+ First release