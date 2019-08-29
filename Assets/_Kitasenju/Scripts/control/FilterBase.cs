using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class FilterBase : MonoBehaviour
{

    [SerializeField] public Sprite _icon;
    [SerializeField] public bool _hasInvert = false;
    protected EffectControlMain _main;
    public string filterName;

    public virtual void Show(EffectControlMain main){
        
        gameObject.SetActive(true);
        _main = main;

    }

    public virtual void Hide(){

        gameObject.SetActive(false);

    }

    public virtual void SetInvert(bool b){

    }

    public virtual void UpdateFilter(){

    }


}
