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
    [SerializeField] private float _width = 0;
    [SerializeField,Space(10)] private TouchMoving _touch;
    [SerializeField] private float _velocityLimit = 0.1f;

    private RectTransform _containerRect;
    private int _currentIndex = -1;
    private const string SAVE_KEY="_Menu";


    public void Init(List<FilterBase> list){

        _btns = new List<FilterBtn>();
        //静止する
        for(int i=0;i<list.Count;i++){

            var b = Instantiate(_btnPrefab,_container.transform,false);
            var rect = b.GetComponent<RectTransform>();
            b.Init( 
                i, 
                list[i]._icon,
                OnClick
            );
            _btns.Add(b);
            rect.localPosition = GetPositionByIndex(i);

        }
        _btnPrefab.gameObject.SetActive(false);
        
        if(PlayerPrefs.HasKey(SAVE_KEY)){
            SetIndex( PlayerPrefs.GetInt(SAVE_KEY),false );
        }else{
            SetIndex(0, false);
        }
    
    }

    private Vector3 GetPositionByIndex(int idx){
        return new Vector3(
            (float)idx*200,//0,
            0,//(float)idx*200
            0
        );
    }

    private void OnClick(int idx){

        if(_currentIndex==idx){
            Debug.Log("IDが同じ");
            return;
        }

        if( Mathf.Abs( _touch.velocity.x ) >_velocityLimit ){
            return;
        }

        SetIndex(idx);

    }

    private void SetIndex(int idx, bool isAnim=true){
        PlayerPrefs.SetInt(SAVE_KEY,idx);

        _currentIndex = idx;
        //vibe
        VibeManager.Instance.PlaySystemSound(VibeManager.Vibe01);

        _containerRect = _container.GetComponent<RectTransform>();


        for(int i=0;i<_btns.Count;i++){
            _btns[i].SetActiveDot(false);
        }
        _btns[idx].SetActiveDot(true);
       

        var v = GetPositionByIndex(idx);
        var tgt = -v.x + 430f;
        if(tgt>0) tgt = 0;
        if(tgt<-_width) tgt = -_width; 

        //click
        if(isAnim){
            _containerRect.DOLocalMoveX(
                tgt,0.5f
            ).SetEase( Ease.InOutSine );
        }else{
            var lp = _containerRect.localPosition;
            lp.x = tgt;
            _containerRect.localPosition=lp;
        }


        _control.SetFilter(idx);

    }

    public void KillTween(){

        _containerRect.DOKill();

    }



}
