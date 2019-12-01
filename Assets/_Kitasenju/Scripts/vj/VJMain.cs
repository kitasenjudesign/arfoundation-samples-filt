using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class VJMain : MonoBehaviour
{

    [SerializeField] private EffectControlMain _main;
    [SerializeField,Space(10)] private int _index = 0;
    [SerializeField,Space(10)] private TextMeshProUGUI _text;
    [SerializeField,Space(10)] private Button _btnRandom;
    [SerializeField] private Button _btnNext;
    [SerializeField] private Button _btnPrev;
    [SerializeField] private Button _btnInvert;
    [SerializeField] private Button _btnMenu;
    [SerializeField,Space(10)] private GameObject _menu;
    private bool _isInvert = false;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log( "width" + Screen.width + " height" + Screen.height );

        SceneManager.LoadScene("main", LoadSceneMode.Additive); 
        Invoke("_onInit",1f);
    }




    private void _onInit(){
        
        var mainGo = GameObject.Find("EffectControl");
        _main = mainGo.GetComponent<EffectControlMain>();//.SetFilter(1);

        //メニューは消す
        _menu = GameObject.Find("MenuCanvas");
        _menu.gameObject.SetActive(false);

        _btnNext.onClick.AddListener(_nextFilter);
        _btnPrev.onClick.AddListener(_prevFilter);
        _btnRandom.onClick.AddListener(_randomFilter);
        _btnInvert.onClick.AddListener(_invertFilter);
        _btnMenu.onClick.AddListener(_onClickMenu);

    }

    private void _onClickMenu(){

        _menu.gameObject.SetActive( !_menu.gameObject.activeSelf );

    }

    private void _invertFilter(){

        _isInvert = !_isInvert;
        _main.SetInvert(_isInvert);

    }

    private void _nextFilter(){
        _index++;
       _setFilter();

    }
    private void _prevFilter(){
        _index--;
       _setFilter();

    }

    private void _randomFilter(){
       
        _index = Mathf.FloorToInt( _main._filters.Count * Random.value );

       _setFilter();

       _isInvert = Random.value<0.5f ? true : false;
       _invertFilter();

    }

    public void SetRandom(){
        
        _randomFilter();

    }

    private void _setFilter(){

        if(_index>=_main._filters.Count)_index=0;
        if(_index<0) _index=_main._filters.Count-1;

         _text.text = "" + _main._filters[_index].name.ToUpper();

        _main.SetFilter( _index );

    }












    // Update is called once per frame
    void Update()
    {
        
    }
}
