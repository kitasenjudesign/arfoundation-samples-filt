
    using UnityEngine;
    using UnityEngine.Scripting;
    using System;
    using System.Runtime.InteropServices;

public class MyNativeBindings 
{
    #if UNITY_IPHONE
        [DllImport ("__Internal")]
        public static extern string GetSettingsURL();

        [DllImport ("__Internal")]
        public static extern void OpenSettings();
    #endif
}