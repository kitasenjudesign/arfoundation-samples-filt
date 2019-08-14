using UnityEngine;


public class ImageEffectBase : MonoBehaviour
{
    
    public Material material;

    void Awake()
    {
        //_material = new Material(_shader);
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, material);
    }

}