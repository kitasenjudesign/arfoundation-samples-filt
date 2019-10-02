using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasEnabler : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.touchCount >= 3){
            
            var touch = Input.touches[0];
            if (touch.phase == TouchPhase.Began)
            {
                _canvas.enabled = !_canvas.enabled;
            }

         }

    }
}
