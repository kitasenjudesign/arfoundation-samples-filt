using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.IO;

public class PngSaver : MonoBehaviour
{

    [SerializeField] private Vector2 size;
    private RenderTexture _output;
    private Texture2D _output2;
    private System.Action<Texture2D> _callback;
    [SerializeField] private string filename;
    private bool _isCap = false;
    //[SerializeField] private bool enabled = false;
    // Start is called before the first frame update
    void Start()
    {
           
    }


    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        
        Graphics.Blit(source, destination);
        
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

        if(_output2!=null & _isCap){
            Graphics.Blit(source,_output);
            _output2.ReadPixels(new Rect(0, 0, _output.width, _output.height), 0, 0);
            _output2.Apply();
            _save(_output2);
            _isCap=false;
        }

        //if(_callback!=null){
        //    _callback(_output2);
        //}
         //enabled = false;
    }


    // Update is called once per frame
    void Update()
    {
        
        //手動でキャプチャしたいとき
        if(Input.GetKeyDown(KeyCode.DownArrow)){
            Debug.Log("capture");
            _isCap=true;
        }

    }

    void _save(Texture2D tex){
            //byte[] bytes = _output2.EncodeToPNG();
            byte[] bytes = _output2.EncodeToPNG();
            //byte[] bytes = _output2.EncodeToJPG(95);

            //Object.Destroy(_output2);

            //Write to a file in the project folder
            var d = System.DateTime.Now.ToString("MMddHHmmss");
            File.WriteAllBytes(Application.dataPath + "/../texTest/"+filename + d +".png", bytes);
    }

}
