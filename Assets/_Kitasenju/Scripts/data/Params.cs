using UnityEngine;
using NatMic;

public class Params
{

    public static float _stencilAscpect = 1.62433f;

    public static bool usingMicrophone = false;
    public static float microphoneVolume = 3f;
    public static int videoQuality = 0;
    public const int VIDEO_QUALITY_LOW = 0;
    public const int VIDEO_QUALITY_MID = 1;
    public const int VIDEO_QUALITY_HIGH = 2;

    public static int baseWidth =0;
    public static int baseHeight =0;

    public static AudioDevice audioDevice;



    public static void Init(){

        baseWidth = Screen.width;
        baseHeight = Screen.height;
        SetStencilAspect(720f,960f);
        
    }

    public static AudioDevice SetMic(){
        if(audioDevice==null){
            if( DeviceChecker.GetAvailable() ){
                audioDevice = AudioDevice.GetDevices()[0];
            }
        }
        return audioDevice;
    }

    public static void SetStencilAspect(float w, float h){

        _stencilAscpect = ((float)Screen.height/(float)Screen.width) / (w/h);//なんか逆
        Shader.SetGlobalFloat("_GlobalStencilAspect",_stencilAscpect);

    }


    public static void SetScreenSizeByQuality(){
        var size = GetVideoSize();
        Screen.SetResolution(size.x,size.y,false,60);
    }

    public static void ResetScreenSize(){
        Screen.SetResolution(baseWidth,baseHeight,false,60);
    }


    public static Vector2Int GetVideoSize(){

        var screen = new Vector2Int(
            Mathf.FloorToInt( (float)baseWidth * 0.5f ),
            Mathf.FloorToInt( (float)baseHeight * 0.5f )
        );

        switch(videoQuality){

            case VIDEO_QUALITY_LOW:
                screen.x = Mathf.FloorToInt( (float)baseWidth / 2f );
                screen.y = Mathf.FloorToInt( (float)baseHeight / 2f );
                break;
            case VIDEO_QUALITY_MID:
                screen.x = Mathf.FloorToInt( (float)baseWidth * 0.75f );
                screen.y = Mathf.FloorToInt( (float)baseHeight * 0.75f );
                screen.x += screen.x % 2;
                screen.y += screen.y % 2;

                break;
            case VIDEO_QUALITY_HIGH:
                screen.x = baseWidth;
                screen.y = baseHeight;
                break;

        }

        return screen;

    }

    public static int GetVideoBitrate(){
        

        switch(videoQuality){
            case VIDEO_QUALITY_LOW:
                return Mathf.FloorToInt( 5909760*4f );
                break;
            case VIDEO_QUALITY_MID:
                return Mathf.FloorToInt( 5909760*5f );
                break;
            case VIDEO_QUALITY_HIGH:
                return Mathf.FloorToInt( 5909760*6f );
                break;

        }

        return Mathf.FloorToInt( 5909760*4f );

    }    





}