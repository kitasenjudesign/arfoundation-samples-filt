using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SettingToggleBtn : MonoBehaviour
{   
    [SerializeField] private string _title;
    [SerializeField] List<string> _choices;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Button _btn;
    public int selected = -1;
    private System.Action<SettingToggleBtn> _callback; 

    
//PlayerPrefs.SetString("Data", "SaveData");
//PlayerPrefs.SetInt("Data", 10);
//PlayerPrefs.SetFloat("Data", 3.14f);    

    public void Init( string[] list, System.Action<SettingToggleBtn> callback){

        _callback = callback;
        _title = list[0];

        _choices = new List<string>();

        for(int i=1;i<list.Length;i++){
            
            _choices.Add( list[i] );

        }

        
       if( PlayerPrefs.HasKey(_title) ){
           selected=PlayerPrefs.GetInt(_title);
       }else{
           selected=0;
       }


        UpdateText();
        
        _btn.image.color = new Color(0,0,0,0);
        _btn.onClick.AddListener( _onClick );

        if( _callback != null ){
            _callback(this);
        }

    }

    private void _onClick(){
        
        selected++;
        selected = selected%_choices.Count;
        PlayerPrefs.SetInt(_title,selected);

        UpdateText();

        VibeManager.Instance.PlaySystemSound(VibeManager.Vibe01);

        if( _callback != null ){
            _callback(this);
        }
        
    }

    public void UpdateText(){

        _text.text = _title + "\n" + "[ " + _choices[selected] + " ]";
       
    }

}
