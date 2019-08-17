using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject _menu;
    [SerializeField] public Slider _slider1;
    [SerializeField] public Slider _slider2;

    private GUIStyle _style;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnGUI()
    {
        if(_style==null){
            _style = new GUIStyle();
            _style.fontSize = 30;
            _style.normal.textColor = Color.red;                
        }

        //Debug.Log(timeCode=="");
        GUI.Label(
            new Rect(50,50, 200, 100),
            _slider1.value+"\n"+_slider2.value, 
            _style
        );
        
    }      

    public void ToggleMenu(){
        if(_menu.activeSelf){
            _menu.gameObject.SetActive(false);
        }else{
            _menu.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount >= 2 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            ToggleMenu();
        }
    }
}
