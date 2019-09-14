using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
using UnityEngine.UI;
 
 //https://qiita.com/MYamate_jp/items/9f26ad6f78f347ebd629
public class DeviceChecker {


        //@"iPhone11,8" :@"iPhone XR",                 // iPhone XR A1984,A2105,A2106,A2108
        //@"iPhone11,2" :@"iPhone XS",                 // iPhone XS A2097,A2098
        //@"iPhone11,4" :@"iPhone XS Max",             // iPhone XS Max A1921,A2103
        //å@"iPhone11,6" :@"iPhone XS Max",  
        // iPhone X

        /*
        @"iPad8,1"   :@"iPad Pro 11inch WiFi",          // iPad Pro 11inch WiFi
        @"iPad8,2"   :@"iPad Pro 11inch WiFi",          // iPad Pro 11inch WiFi
        @"iPad8,3"   :@"iPad Pro 11inch Cell",          // iPad Pro 11inch Cellular
        @"iPad8,4"   :@"iPad Pro 11inch Cell",          // iPad Pro 11inch Cellular
        @"iPad8,5"   :@"iPad Pro 12.9inch WiFi",        // iPad Pro 12.9inch WiFi
        @"iPad8,6"   :@"iPad Pro 12.9inch WiFi",        // iPad Pro 12.9inch WiFi
        @"iPad8,7"   :@"iPad Pro 12.9inch Cell",        // iPad Pro 12.9inch Cellular
        @"iPad8,8"   :@"iPad Pro 12.9inch Cell",        // iPad Pro 12.9i
        */


    public static bool GetAvailable(){

        var devices = new string[]{
            "iPhone11,8",//iphone xr
            "iPhone11,2",//iphone xs
            "iPhone11,4",//iphone xs max
            "iPhone11,6",//iphone xs max

            "iPad8,1",
            "iPad8,2",
            "iPad8,3",
            "iPad8,4",
            "iPad8,5",
            "iPad8,6",
            "iPad8,7",
            "iPad8,8"


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