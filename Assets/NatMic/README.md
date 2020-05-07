# NatMic API
NatMic is a low-latency native microphone API for Unity Engine. NatMic provides a minimal API for streaming audio data directly from audio input devices to Unity. NatMic's features include:
+ Low-latency microphone recording.
+ Control microphone format like sample rate and channel count.
+ Mix microphone audio with game audio.
+ Record to audio files, currently supporting recording to WAV files.
+ Record on iOS, Android, macOS, and Windows.

## Fundamentals of Recording
NatMic abstracts audio input devices with the `IAudioDevice` interface. This interface supports fundamental audio recording operations:
```csharp
// Is the device currently recording?
bool IsRecording { get; }
// Start recording
void StartRecording (int requestedSampleRate, int requestedChannelCount, IAudioProcessor processor);
// Stop recording
void StopRecording ();
```

When `StartRecording` is called on an audio device, an `IAudioProcessor` must be provided. This audio processor will receive the raw audio data being streamed from the audio device:
```csharp
public interface IAudioProcessor {
    // Audio data is sent to this function by the audio device
    void OnSampleBuffer (float[] sampleBuffer, int sampleRate, int channelCount, long timestamp);
}
```

NatMic always provides audio data in a floating-point (`float`) sample buffer, interleaved by channel.

## Audio Devices
To get access to physical (hardware) audio input devices, use the `AudioDevice.GetDevices` method. This method provides an array of audio input devices, each corresponding to a hardware microphone. In addition to supporting the methods defined in `IAudioDevice`, each `AudioDevice` instance provides identifying information like its display-friendly name, its unique ID, and whether it supports echo cancellation.
```csharp
// Get a microphone
var microphone = AudioDevice.GetDevices()[0];
// Start recording
microphone.StartRecording(...);
```

## Storing Audio with Recorders
NatMic provides helper classes for storing audio data into a reusable container. These classes are all instances of `IAudioRecorder`. Currently, NatMic includes a `WAVRecorder` for recording audio data to a .wav file on the file system; and a `ClipRecorder` for recording audio data to a Unity `AudioClip`, for use with an `AudioSource` component.

Every `IAudioRecorder` is an audio processor, so it naturally inherits the `IAudioProcessor` interface. As a result, it can be passed directly to an audio device for a hands-free recording process:
```csharp
// Create a WAV recorder
var wavRecorder = new WAVRecorder(filePath);
// Get a microphone
var audioDevice = AudioDevice.GetDevices()[0];
// Record the microphone audio to the WAV recorder
audioDevice.StartRecording(?, ?, wavRecorder);
...
// Stop recording
audioDevice.StopRecording();
wavRecorder.Dispose();
Debug.Log("Microphone audio was recorded to a WAV file at: "+wavRecorder.FilePath);
```

## Using NatMic with NatCorder
Check out the [NatMicCorder Demo](https://github.com/olokobayusuf/NatMicCorder-Demo) for a full NatMic-NatCorder integration example.

## Tutorials
- [Unity Microphone That Works](https://medium.com/@olokobayusuf/natmic-api-unity-microphone-that-works-264d2b73cfa8)

## Requirements
- On Android, NatMic requires API Level 23+
- On iOS, NatMic requires iOS 10+
- On macOS, NatMic requires macOS 10.10+
- On Windows, NatMic requires Windows 10+

## Quick Tips
- Please peruse the included scripting reference [here](https://olokobayusuf.github.io/NatMic-Docs/)
- To discuss or report an issue, visit Unity forums [here](https://forum.unity.com/threads/natmic-native-microphone-api.431677/)
- Contact me at [olokobayusuf@gmail.com](mailto:olokobayusuf@gmail.com)

Thank you very much!