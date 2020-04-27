using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RotateObj : MonoBehaviour
{

    Vector3 basePos;
    [SerializeField] private Mesh _mesh1;
    [SerializeField] private Mesh _mesh2;
    private MeshFilter _filter;
    private bool _isWorld = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Init(){
        basePos = transform.position;
        _filter = GetComponent<MeshFilter>();
        _Rotate();
    }


    void _Rotate(){

        _filter.mesh = _isWorld ? _mesh1 : _mesh2;
        _isWorld=!_isWorld;

        gameObject.SetActive(false);

        DOVirtual.DelayedCall(0.5f*Random.value,_Rotate2);
        DOVirtual.DelayedCall(3.5f,_Rotate);
    }

    void _Rotate2(){

        gameObject.SetActive(true);

         var ss = 0.2f + 0.2f * Random.value;
            transform.localScale = new Vector3(
                ss,ss,ss
            );

        var r = new Vector3(
            1.5f*360f * Random.value,
            1.5f*360f * Random.value,
            1.5f*360f * Random.value
        );

        transform.DORotate(r,1.5f,RotateMode.FastBeyond360)
            .SetEase(Ease.OutCubic)
            .OnComplete(_Hide);

        transform.position = basePos + new Vector3(0,-2f-Random.value,0);
        transform.DOMove(basePos,1f).SetEase(Ease.OutCubic);
    
    }

    void _Hide(){

        var r = new Vector3(
            1.5f * 360f * Random.value,
            1.5f * 360f * Random.value,
            1.5f * 360f * Random.value
        );
        transform.DORotate(r,1.1f,RotateMode.FastBeyond360).SetEase(Ease.InCubic);
        transform.DOMove(basePos + new Vector3(0,3f+Random.value,0),1f).SetEase(Ease.InSine);
        transform.DOScale(Vector3.zero,1f).SetEase(Ease.InSine);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
