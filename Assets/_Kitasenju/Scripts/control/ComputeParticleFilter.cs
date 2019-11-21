using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ComputeParticleFilter : SimpleFilter
{

    [SerializeField,Space(10)] private ImageParticleInitializer _particle;
    //[SerializeField] private int _num=20000;//数とか
    //[SerializeField] private Material _particleMat;

    public override void Show(EffectControlMain main){
        
        base.Show(main);
        _particle.Show();

    }
    
    public override void Hide(){

        base.Hide();
        _particle.Hide();

    }


}