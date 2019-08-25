using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class About : MonoBehaviour
{

    [SerializeField] private Button _aboutBtn;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        _aboutBtn.onClick.AddListener(_onShowAbout);
    }

    private void _onShowAbout(){
        gameObject.SetActive( !gameObject.activeSelf );
    }

    public void Show(){

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
