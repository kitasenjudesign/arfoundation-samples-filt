using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blink : MonoBehaviour {

    private bool isBlinking = false;
    private float waitedTime = 0f;
    public FlashlightController flash;

    private void Update()
    {
        // Timer for the blink
        if(isBlinking)
        {
            if (waitedTime > 1f)
            {
                waitedTime = 0;
                flash.TurnOnOffFlashLight();
            }
            else
                waitedTime += Time.deltaTime;
        }
    }

    public void startStopBlink()
    {
        isBlinking = !isBlinking;
    }
}
