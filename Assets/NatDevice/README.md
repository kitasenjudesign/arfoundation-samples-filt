# NatDevice API
NatDevice is a cross-platform media device API for iOS, Android, macOS, and Windows. NatDevice provides powerful abstractions for hardware cameras and microphones through a concise .NET API. Features include:
- Stream the camera preview and microphone audio into Unity with very little latency.
- Support for high resolution camera previews, at full HD and higher on devices that support it.
- Support for specifying microphone sample rate and channel count.
- Full feature camera control, with the ability to set flash, focus, exposure, white balance, torch, zoom, and orientation.
- Support for microphones with echo cancellation for karaoke and voice call apps.
- OpenCV support, for using vision algorithms on the camera preview.
- VR support, compatible with GearVR and Google Cardboard.
- NatCorder support, for recording the camera and/or the microphone.

## Discovering Devices
NatDevice exposes instances of the `IMediaDevice` interface, each of which abstracts a media device from which a camera preview or microphone audio can be streamed. In order to discover available devices, you will use the `MediaDeviceQuery` class. The device query is responsible for finding available devices and providing them:
```csharp
// Create a device query to search for available devices
var query = new MediaDeviceQuery();
// Now we can get the currently selected device
IMediaDevice device = query.currentDevice;
// Or we can get all of the available devices
IMediaDevice[] devices = query.devices;
```

It is possible to search only for devices that meet a specific set of criteria. The `MediaDeviceQuery` constructor accepts an array of criteria that can be used to filter for devices. Each criterion is provided as a function which takes in an `IMediaDevice` and returns a `bool`. For example, this is how we might filter for camera devices:
```csharp
// Create a device query that only fetches camera devices
var query = new MediaDeviceQuery(
    device => device is ICameraDevice // Returns `true` if a given device is a camera device
);
```

It is possible to filter by several conditions at once. Below, we search for microphones that also support echo cancellation:
```csharp
// Create a device query that only fetches audio devices that perform echo cancellation
var query = new MediaDeviceQuery(
    // Returns `true` if a given device is an audio device
    device => device is IAudioDevice,
    // Returns `true` if the microphone supports AEC
    device => device is AudioDevice microphone && microphone.echoCancellation
)
```

The `MediaDeviceQuery` class includes a set of commonly-used criteria, so you don't have to define them yourself:
```csharp
// Create a device query that fetches front facing cameras
var frontCameraQuery = new MediaDeviceQuery(MediaDeviceQuery.Criteria.FrontFacing);
// Create a device query that fethces microphones
var microphoneQuery = new MediaDeviceQuery(MediaDeviceQuery.Criteria.AudioDevice);
// ...
```

Finally, the `MediaDeviceQuery` keeps a cursor for the devices it has found. The cursor can be advanced using the `Advance` method. When there are no more devices left, the cursor will return to the first available device. This is useful for implementing device switching at runtime:
```csharp
// Create a device query for camera devices
var query = new MediaDeviceQuery(MediaDeviceQuery.Criteria.CameraDevice);
// Use the current camera
var device = query.currentDevice as CameraDevice;
...
// Switch to the next available device
query.Advance();
// Use the new camera
device = query.currentDevice as CameraDevice;
...
```

## Using Audio Devices
All audio devices implement the `IAudioDevice` interface. This interface provides the microphone's audio format: `sampleRate` and `channelCount`. It also provides the `StartRunning` method, which will start streaming interleaved floating-point linear PCM data to the provided delegate:
```csharp
// Get a microphone
var query = new MediaDeviceQuery(MediaDeviceQuery.Criteria.AudioDevice);
var device = query.currentDevice as AudioDevice;
// Start recording
device.StartRecording(OnSampleBuffer);
Debug.Log($"Started recording microphone {device.name} with audio format: {device.sampleRate}Hz, {device.channelCount} channels");
```

The microphone will begin streaming audio sample buffers to the provided `SampleBufferDelegate`:
```csharp
void OnSampleBuffer (float[] sampleBuffer, long timestamp) {
    // `sampleBuffer` is linear PCM, interleaved by channel
}
```

## Using Camera Devices
All camera devices implement the `ICameraDevice` interface. This interface provides the camera's facing, preview resolution, and framerate. It also provides the `StartRunning` method, which will start streaming the camera preview into a `Texture2D`, and return that texture:
```csharp
// Get a rear camera // generic here means we get both hardware- and WebCamTexture-backed cameras
var query = new MediaDeviceQuery(MediaDeviceQuery.Criteria.GenericCameraDevice);
var device = query.currentDevice as ICameraDevice;
// Start the preview and display it
var previewTexture = await device.StartRunning(); // This should be used in an `async` function
rawImage.texture = previewTexture;
```

NatDevice provides two implementations of the `ICameraDevice` interface: `CameraDevice` instances which are backed by a hardware camera; and `WebCameraDevice` instances which are backed by Unity's `WebCamTexture`. The `CameraDevice` implementation contains a full-feature API for camera control, including focus, exposure, flash, torch, zoom, and photo capture. But it is only supported on iOS and Android. The `WebCameraDevice` implementation has a limited API, but is supported on all platforms.

Below is a minimal example illustrating the full-feature `CameraDevice` usage:
```csharp
// Get hardware cameras
var query = new MediaDeviceQuery(MediaDeviceQuery.Criteria.CameraDevice);
var device = query.currentDevice as CameraDevice;
// Get the field of view of the camera
var (fovX, fovY) = device.fieldOfView;
...
// Start the preview
var previewTexture = await device.StartRunning();
...
// Set the focus point to the center of the screen
device.focusPoint = (0.5f, 0.5f);
...
// Capture a photo
var photoTexture = await device.CapturePhoto();
...
```

## Requesting Permissions
On mobile platforms, it is necessary to request permissions from the user before attempting to start media devices. To do so, we provide the `MediaDeviceQuery.RequestPermissions` method:
```csharp
// Request camera permissions
bool cameraPermissionGranted = await MediaDeviceQuery.RequestPermissions<ICameraDevice>();
// Or request microphone permissions
bool microphonePermissionGranted = await MediaDeviceQuery.RequestPermissions<IAudioDevice>();
```

Note that on iOS, you will need to include the `NSCameraUsageDescription` and/or `NSMicrophoneUsageDescription` keys in your Xcode project's `Info.plist` file. You can alternatively specify these description strings in Player Settings.

## Requirements
- Unity 2018.3+
- Android API Level 21+
- iOS 11+
- macOS 10.10+
- Windows 10, 64-bit only

## Quick Tips
- Please peruse the included scripting reference in the `Docs` folder.
- To discuss or report an issue, visit Unity forums [here](https://forum.unity.com/threads/natdevice-media-device-api.374690/).
- Check out [more examples on Github](https://github.com/natsuite).
- Contact me at [olokobayusuf@gmail.com](mailto:olokobayusuf@gmail.com).

Thank you very much!