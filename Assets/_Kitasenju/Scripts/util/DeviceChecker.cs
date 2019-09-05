using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
using UnityEngine.UI;
 
 //https://qiita.com/MYamate_jp/items/9f26ad6f78f347ebd629
public class DeviceChecker {


                                //@"iPhone11,8" :@"iPhone XR",                 // iPhone XR A1984,A2105,A2106,A2108
                                //@"iPhone11,2" :@"iPhone XS",                 // iPhone XS A2097,A2098
                                //@"iPhone11,4" :@"iPhone XS Max",             // iPhone XS Max A1921,A2103
                                //å@"iPhone11,6" :@"iPhone XS Max",             // iPhone X
    public static bool GetAvailable(){

        var devices = new string[]{
            "iPhone11,8",//iphone xr
            "iPhone11,2",//iphone xs
            "iPhone11,4",//iphone xs max
            "iPhone11,6"//iphone xs max
        };

        bool isAvailable = false;
        for(int i=0;i<devices.Length;i++){
            if( SystemInfo.deviceModel == devices[i] ){
                isAvailable=true;
                break;
            }

        }

        #if UNITY_EDITOR
        isAvailable=true;
        #endif

        return isAvailable;
    }

//available
}