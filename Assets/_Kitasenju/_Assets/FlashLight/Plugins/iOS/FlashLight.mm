#import "FlashLight.h"
#import <AVFoundation/AVFoundation.h>
#import <Foundation/Foundation.h>

bool isFlashOn = false;
bool saveBrightnessB = false;
float savedBrightness = 0;

void turnFlashlight(bool on) 
{
	// Get class for the camera
    Class captureDeviceClass = NSClassFromString(@"AVCaptureDevice");

	// Check if the device has a camera
    if (captureDeviceClass != nil) 
	{
		// Get the device camera Object
        AVCaptureDevice *device = [AVCaptureDevice defaultDeviceWithMediaType:AVMediaTypeVideo];

		// Check if the device has a flash
        if ([device hasTorch] && [device hasFlash]){

            [device lockForConfiguration:nil];

            if (on) 
			{
				// Turn on with the saved value if the user want it
				if(saveBrightnessB && savedBrightness > 0)
				{
					if(savedBrightness == AVCaptureMaxAvailableTorchLevel)
					{
						[device setFlashMode:AVCaptureFlashModeOn];
					}
					else
					{
						[device setFlashMode:AVCaptureFlashModeOff];
					}

					[device setTorchModeOnWithLevel:savedBrightness error:nil];
				}
				else
				{
					[device setFlashMode:AVCaptureFlashModeOn];
					[device setTorchModeOnWithLevel:AVCaptureMaxAvailableTorchLevel error:nil];
				}

				isFlashOn = true;
            } 
			else 
			{
				// Turn off
                [device setTorchMode:AVCaptureTorchModeOff];
                [device setFlashMode:AVCaptureFlashModeOff];
				isFlashOn = false;
            }

			// Trigger event
			[device unlockForConfiguration];
			alertFlashStateChanged(isFlashOn);

        }
    } 
}

void switchSateFlashlight()
{
	// Get class for the camera
	Class captureDeviceClass = NSClassFromString(@"AVCaptureDevice");

	// Check if the device has a camera
    if (captureDeviceClass != nil) 
	{
		// Get the device camera Object
        AVCaptureDevice *device = [AVCaptureDevice defaultDeviceWithMediaType:AVMediaTypeVideo];

		// Check if the device has a flash
        if ([device hasTorch] && [device hasFlash])
		{
            [device lockForConfiguration:nil];

            if (!isFlashOn) 
			{
				// Check if the user want to get the saved value and turn on the flash
                if(saveBrightnessB && savedBrightness > 0)
				{
					if(savedBrightness == AVCaptureMaxAvailableTorchLevel)
					{
						[device setFlashMode:AVCaptureFlashModeOn];
					}
					else
					{
						[device setFlashMode:AVCaptureFlashModeOff];
					}

					[device setTorchModeOnWithLevel:savedBrightness error:nil];
				}
				else
				{
					[device setTorchMode:AVCaptureTorchModeOn];
					[device setFlashMode:AVCaptureFlashModeOn];
				}
				isFlashOn = true;
            }
			else 
			{
				// Turn off
                [device setTorchMode:AVCaptureTorchModeOff];
                [device setFlashMode:AVCaptureFlashModeOff];
				isFlashOn = false;
            }

			// Trigger event
			[device unlockForConfiguration];
			alertFlashStateChanged(isFlashOn);
        }
    } 
}

void saveBrightness(bool state)
{
	// Set the global boolean to the 'state'
	// The user want to save the brightness value
	saveBrightnessB = state;
}

void changeBrightness(float value)
{
	// Get class for the camera
	Class captureDeviceClass = NSClassFromString(@"AVCaptureDevice");

	// Check if the device has a camera
    if (captureDeviceClass != nil) 
	{
		// Get the device camera Object
		AVCaptureDevice *device = [AVCaptureDevice defaultDeviceWithMediaType:AVMediaTypeVideo];

		// Check if the device has a flash
		if ([device hasTorch] && [device hasFlash])
		{
			[device lockForConfiguration:nil];
			
			// Check if float value is correct
			if(value <= 0)
			{
				value = 0;
				[device setTorchMode:AVCaptureTorchModeOff];
				[device setFlashMode:AVCaptureFlashModeOff];
			}
			else if(value >= 1.0)
			{
				value = AVCaptureMaxAvailableTorchLevel;
				[device setTorchModeOnWithLevel:AVCaptureMaxAvailableTorchLevel error:nil];
				[device setFlashMode:AVCaptureFlashModeOn];
			}
			else
			{
				[device setTorchModeOnWithLevel:value error:nil];
				[device setFlashMode:AVCaptureFlashModeOff];
			}

			// Trigger event
			[device unlockForConfiguration];
			alertFlashBrightnessValueChanged(value);
		}
	}
}

void alertFlashBrightnessValueChanged(float newValue)
{
	// Trigger event
	savedBrightness = newValue;
	UnitySendMessage("FlashlightController", "FlashBrightnessValueChanged", [[NSString stringWithFormat:@"%1.6f", newValue] UTF8String]);

	if(newValue > 0)
	{
		// So the flashlight is now on
		isFlashOn = true;
		alertFlashStateChanged(true);
	}
	else
	{
		// So the flashlight is now off
		isFlashOn = false;
		alertFlashStateChanged(false);
	}
}


void alertFlashStateChanged(bool newState)
{
	// Trigger event
	if(newState)
	{
		UnitySendMessage("FlashlightController", "FlashStateChanged", "true");
	}
	else
	{
		UnitySendMessage("FlashlightController", "FlashStateChanged", "false");
	}
}

bool isFlashON() 
{
	// Return the state of the Flashlight
	return isFlashOn;
}