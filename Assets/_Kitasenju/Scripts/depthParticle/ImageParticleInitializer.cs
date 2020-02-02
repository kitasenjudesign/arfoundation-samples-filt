using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageParticleInitializer : MonoBehaviour
{
    [SerializeField] private EffectControlMain _main;
    [SerializeField] private ImageParticle _particle;
    [SerializeField] private Camera _camera;
    [SerializeField] private bool _isTest=false;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Show(){
        
        gameObject.SetActive(true);
        _particle.Init();

    }

    public void Hide(){
        
        gameObject.SetActive(false);

    }

    
    private void OnGUI(){
        
        if( Params.isDebug ){

            GUI.DrawTexture(
                new Rect(0, 0, 300, 200), 
                _main._camTex, 
                ScaleMode.StretchToFill,
                false
            );
            GUI.DrawTexture(
                new Rect(0, 200, 300, 200), 
                _main._humanBodyManager.humanDepthTexture,
                //_main._camTex, 
                ScaleMode.StretchToFill,
                false
            );
            GUI.DrawTexture(
                new Rect(0, 400, 300, 200), 
                _main._humanBodyManager.humanStencilTexture,
                //_main._camTex, 
                ScaleMode.StretchToFill,
                false
            );

        }

    }


    // Update is called once per frame
    void Update()
    {
        if(_main){
            
            _particle.SetTextures(
                _main._camTex,
                _main._humanBodyManager.humanStencilTexture,
                _main._humanBodyManager.humanDepthTexture,
                _camera
            );

        }
    }
}
