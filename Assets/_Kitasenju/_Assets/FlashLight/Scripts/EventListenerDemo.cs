using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventListenerDemo : MonoBehaviour {

	public FlashlightController flashCtrl;
    public Text textInfo, brightnessInfo, textBrightness;
    public Image firstButtonColor, blinkButtonColor, onButton, offButton, brightnesButton;
    public Slider brightnessLevel;

	void Start ()
    {
        // Create events
        flashCtrl.OnStateChanged += FlashCtrl_OnStateChanged;
        flashCtrl.OnBrightnessChanged += FlashCtrl_OnBrightnessChanged;
        brightnessLevel.onValueChanged.AddListener(flashCtrl.ChangeBrightness);
    }

    private void FlashCtrl_OnBrightnessChanged(float newValue)
    {
        brightnessInfo.text = "Current brightness level: " + newValue;
    }

    bool saveBright = false;
    public void SaveBrightness()
    {
        saveBright = !saveBright;
        flashCtrl.SaveBrightness(saveBright);
        textBrightness.text = "Save Brightness (actually " + saveBright + ")";
        if (saveBright)
            brightnesButton.color = new Color32(103, 255, 123, 255);
        else
            brightnesButton.color = new Color32(216, 89, 89, 255);
    }

    private void FlashCtrl_OnStateChanged(bool newState)
    {
        if(newState)
        {
            textInfo.text = "Flashlight: ON";
            firstButtonColor.color = new Color32(238, 234, 86, 255);
            blinkButtonColor.color = new Color32(238, 234, 86, 255);
            onButton.color = new Color32(103, 255, 123, 255);
            offButton.color = new Color32(255, 255, 255, 255);
        }
        else
        {
            textInfo.text = "Flashlight: OFF";
            firstButtonColor.color = new Color32(255, 255, 255, 255);
            blinkButtonColor.color = new Color32(255, 255, 255, 255);
            onButton.color = new Color32(255, 255, 255, 255);
            offButton.color = new Color32(103, 255, 123, 255);
        }
    }

}
