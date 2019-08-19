using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class FilterMenu : MonoBehaviour
{
    
    [SerializeField] private FilterBtn _btnPrefab;
    [SerializeField,Space(10)] private List<FilterBase> _list;
    [SerializeField] private List<FilterBtn> _btns;
    [SerializeField] private EffectControlMain _control;
    [SerializeField] private GameObject _container;
    private RectTransform _containerRect;


    public void Init(List<FilterBase> list){

        //静止する
        for(int i=0;i<list.Count;i++){

            var b = Instantiate(_btnPrefab,_container.transform,false);
            var rect = b.GetComponent<RectTransform>();
            b.Init( i, OnClick );

            rect.localPosition = GetPositionByIndex(i);

        }
        _btnPrefab.gameObject.SetActive(false);

    }

    private Vector3 GetPositionByIndex(int idx){
        return new Vector3(
            0,
            (float)idx*200
            ,0
        );
    }

    private void OnClick(int idx){

        
        _containerRect = _container.GetComponent<RectTransform>();

        var v = GetPositionByIndex(idx);
        _containerRect.DOLocalMoveY(
            -v.y,0.5f
        ).SetEase( Ease.OutCubic );

        _control.SetFilter(idx);

    }



}
