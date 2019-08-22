using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    private Vector2Int baseScale = new Vector2Int();
    private int _index = 0;
    // Start is called before the first frame update
    void Start()
    {
        
        baseScale=new Vector2Int(Screen.width,Screen.height);
        //Screen.height

    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.touchCount >= 2){
            
            var touch = Input.touches[0];
            if (touch.phase == TouchPhase.Began)
            {
                var scales = new float[]{
                    0.5f,0.75f,1f
                };
                var ss = scales[_index%scales.Length];

                Screen.SetResolution(
                    Mathf.FloorToInt( baseScale.x * ss),
                    Mathf.FloorToInt( baseScale.y * ss),
                    FullScreenMode.FullScreenWindow
                );

                _index++;
            }

        }

    }
}
