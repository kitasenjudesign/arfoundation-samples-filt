using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class FlashlightController : MonoBehaviour {

#if UNITY_IOS

    [DllImport("__Internal")]
    private static extern void turnFlashlight(bool state);

    [DllImport("__Internal")]
    private static extern void switchSateFlashlight();

    [DllImport("__Internal")]
    private static extern void changeBrightness(float value);

    [DllImport("__Internal")]
    public static extern bool isFlashON();

    [DllImport("__Internal")]
    private static extern void saveBrightness(bool state);


    public delegate void StateChanged(bool newState);

    /// <summary>
    /// Event triggered when the state of the flashlight has changed
    /// </summary>
    public event StateChanged OnStateChanged;

    public delegate void BrightnessChanged(float newValue);

    /// <summary>
    /// Event triggered when the brightness value has changed
    /// </summary>
    public event BrightnessChanged OnBrightnessChanged;

    /// <summary>
    /// Switch state of the Flashlight
    /// </summary>
    public void TurnOnOffFlashLight()
    {
        switchSateFlashlight();
    }

    /// <summary>
    /// Turn On the Flashlight
    /// </summary>
    public void TurnOn()
    {
        turnFlashlight(true);
    }

    /// <summary>
    /// Turn Off the Flashlight
    /// </summary>
    public void TurnOff()
    {
        turnFlashlight(false);
    }

    /// <summary>
    /// Save or not the value of the actual brightness setting.
    /// <para>Example: If set to true and If you set the brightness value to 0.5, when you turn off and turn on the flash, the brightness will be at 0.5</para>
    /// </summary>
    public void SaveBrightness(bool state)
    {
        saveBrightness(state);
    }

    /// <summary>
    /// Change the brightness value of the Flashlight (value between 0.0 and 1.0)
    /// </summary>
    public void ChangeBrightness(float value)
    {
        // Only float between 0 and 1.0 will have an effect
        changeBrightness(value);
    }

    /// <summary>
    /// <para>Do not use that function.</para>
    /// <para>Please use the <see cref="StateChanged"/> OnStateChanged event.</para>
    /// </summary>
    public void FlashStateChanged(string newState)
    {
        bool state = bool.Parse(newState);
        Debug.Log("The Flash is now " + (state ? "ON" : "OFF"));
        this.OnStateChanged(state);
    }

    /// <summary>
    /// <para>Do not use that function.</para>
    /// <para>Please use the <see cref="BrightnessChanged"/> OnBrightnessChanged event.</para>
    /// </summary>
    public void FlashBrightnessValueChanged(string newValue)
    {
        float value = float.Parse(newValue);
        Debug.Log("The level of brightness is now: " + newValue);
        this.OnBrightnessChanged(value);
    }

#else

    public delegate void StateChanged(bool newState);
    public event StateChanged OnStateChanged;

    public delegate void BrightnessChanged(float newValue);
    public event BrightnessChanged OnBrightnessChanged;

    public void TurnOnOffFlashLight()
    {
        notCompatible();
    }

    public void TurnOn()
    {
        notCompatible();
    }

    public void TurnOff()
    {
        notCompatible();
    }

    public void SaveBrightness(bool state)
    {
        notCompatible();
    }

    public void ChangeBrightness(float value)
    {
        notCompatible();
    }

    public void FlashStateChanged(string newState)
    {
        notCompatible();
    }

    public void FlashBrightnessValueChanged(string newValue)
    {
        notCompatible();
    }

    private void notCompatible()
    {
        Debug.Log("iOS Native Flashlight isn't compatible with this platform. Please change it to iOS in your Build Settings");
    }

#endif


}
