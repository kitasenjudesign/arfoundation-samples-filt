using UnityEngine;


public class Params
{

    public static float _stencilAscpect = 1.62433f;

    public static void Init(){
        
       SetStencilAspect(720f,960f);

    }

    public static void SetStencilAspect(float w, float h){

        _stencilAscpect = ((float)Screen.height/(float)Screen.width) / (w/h);//なんか逆
        Shader.SetGlobalFloat("_GlobalStencilAspect",_stencilAscpect);

    }


    public static Vector2Int GetVideoSize(){
        return new Vector2Int(
            Mathf.FloorToInt( (float)Screen.width / 2f ),
            Mathf.FloorToInt( (float)Screen.height / 2f )
        );
    }


}