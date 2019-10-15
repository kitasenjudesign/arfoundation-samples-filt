using UnityEngine;


public class BlurTexture : MonoBehaviour
{

    public RenderTexture blurTexture;
    private RenderTexture _tempTexture;
    public Material postprocessMaterial;

    private bool _isInit = false;
    void Start(){
    
        Init();

    }

    public void Init(){
        
        if(_isInit) return;
        _isInit=true;

        blurTexture = new RenderTexture(
            Mathf.FloorToInt(Screen.width*0.2f),//0.2f),
            Mathf.FloorToInt(Screen.height*0.2f),
            0
        );
        blurTexture.antiAliasing=4;

        _tempTexture = new RenderTexture(
            blurTexture.width,
            blurTexture.height,
            0
        );

    }



	

    /**
    
     */
	//method which is automatically called by unity after the camera is done rendering
	public RenderTexture UpdateBlur(RenderTexture inputTex){
		
        //draws the pixels from the source texture to the destination texture
		//var temporaryTexture = RenderTexture.GetTemporary(blurTexture.width, blurTexture.height);

		//Graphics.Blit(inputTex, _tempTexture, postprocessMaterial, 0);
		//Graphics.Blit(_tempTexture, blurTexture, postprocessMaterial, 1);
		
        Graphics.Blit(inputTex, blurTexture);//only copy and downscale
        return blurTexture;
        //RenderTexture.ReleaseTemporary(temporaryTexture);

	}



}