using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject _menu;

    // Start is called before the first frame update
    void Start()
    {
        
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
