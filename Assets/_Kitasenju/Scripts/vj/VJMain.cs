using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class VJMain : MonoBehaviour
{

    [SerializeField] private EffectControlMain _main;
    [SerializeField] private TextMeshProUGUI _text;

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.LoadScene("main", LoadSceneMode.Additive); 
        Invoke("_onHoge",1f);
    }

    private void _onHoge(){
        
        var mainGo = GameObject.Find("EffectControl");
        _main = mainGo.GetComponent<EffectControlMain>();//.SetFilter(1);

        //メニューは消す
        var menu = GameObject.Find("MenuCanvas");
        menu.gameObject.SetActive(false);

        _Loop();

    }

    private void _Loop(){
        
        Debug.Log("loop");
        int id = Mathf.FloorToInt( _main._filters.Count * Random.value );
        _text.text = "f"+id;
        _main.SetFilter( id );
        Invoke("_Loop",1f);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
