#import <Foundation/Foundation.h>

extern "C" 
{
	void turnFlashlight(bool on);
	void switchSateFlashlight();
	void changeBrightness(float value);
	void alertFlashStateChanged(bool newState);
	void alertFlashBrightnessValueChanged(float newValue);
	bool isFlashON();
	void saveBrightness(bool state);
}