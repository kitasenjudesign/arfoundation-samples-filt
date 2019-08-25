using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageCapture : MonoBehaviour
{

    [SerializeField] private Vector2 size;
    private RenderTexture _output;
    private Texture2D _output2;
    private System.Action<Texture2D> _callback;

    // Start is called before the first frame update
    void Start()
    {
        enabled=false;
       
    }

    public void Capture(System.Action<Texture2D> callback){
        enabled = true;

        if(_output==null){
            _output = new RenderTexture( 
                Mathf.FloorToInt(size.x),
                Mathf.FloorToInt(size.y),
                0
            );
            _output2 = new Texture2D(
                Mathf.FloorToInt(size.x),
                Mathf.FloorToInt(size.y)
            );
        }
        _callback = callback;

    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source,_output);
        Graphics.Blit(source, destination);

        _output2.ReadPixels(new Rect(0, 0, _output.width, _output.height), 0, 0);
        _output2.Apply();

        if(_callback!=null){
            _callback(_output2);
        }
         enabled = false;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
