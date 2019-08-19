using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class FilterBase : MonoBehaviour
{

    [SerializeField] Sprite _icon;
    protected EffectControlMain _main;
    public string filterName;

    public virtual void Show(EffectControlMain main){
        
        gameObject.SetActive(true);
        _main = main;

    }

    public virtual void Hide(){

        gameObject.SetActive(false);

    }

    public virtual void UpdateFilter(){



    }


}
