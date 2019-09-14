using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugManager : MonoBehaviour
{


    public static DebugManager Instance;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField,Space(10)] private EffectControlMain _main;
    [SerializeField] private GameObject cameraConfig;

    // Start is called before the first frame update
    void Awake()
    {
        _text.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

         if (Input.touchCount >= 2){
            
            var touch = Input.touches[0];
            if (touch.phase == TouchPhase.Began)
            {
                _text.gameObject.SetActive( 
                    !_text.gameObject.activeSelf
                );
                cameraConfig.SetActive(_text.gameObject.activeSelf);

            }

         }

         if( !_text.gameObject.activeSelf ) return;

        _text.text = "";

        if( _main ){

            Texture2D humanStencil  = _main._humanBodyManager.humanStencilTexture;
            Texture2D humanDepth    = _main._humanBodyManager.humanDepthTexture;

            _text.text += Screen.width + " " + Screen.height + "\n";
            _text.text += Params._stencilAscpect + "\n";

            if(humanStencil){

                //stringBuilder.AppendFormat("   format : {0}\n", texture.format.ToString());
                //stringBuilder.AppendFormat("   width  : {0}\n", texture.width);
                //stringBuilder.AppendFormat("   height : {0}\n", texture.height);
                //stringBuilder.AppendFormat("   mipmap : {0}\n", texture.mipmapCount);

                _text.text += humanStencil.format.ToString() + "\n";
                _text.text += humanStencil.width + "_" + humanStencil.height + "\n";
                _text.text += humanStencil.mipmapCount + "\n";
                _text.text += humanStencil.streamingMipmaps + "\n\n";
            }


            if(humanDepth){

                _text.text += humanDepth.format.ToString() + "\n";
                _text.text += humanDepth.width + "_" + humanDepth.height + "\n";
                _text.text += humanDepth.mipmapCount + "\n";
                _text.text += humanDepth.streamingMipmaps + "\n\n";

                //_text.text += humanStencil + "\n";

            }

        }

    }
}
