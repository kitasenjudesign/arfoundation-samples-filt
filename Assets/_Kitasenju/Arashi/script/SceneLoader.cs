using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 350, 350),"A")){
            SceneManager.LoadScene("main");
            gameObject.SetActive(false);
        }    
        if (GUI.Button(new Rect(10, 360, 350, 350),"B")){
            SceneManager.LoadScene("ArashiDemo");
            gameObject.SetActive(false);
        }            
    }       

    // Update is called once per frame
    void Update()
    {
        
    }
}
