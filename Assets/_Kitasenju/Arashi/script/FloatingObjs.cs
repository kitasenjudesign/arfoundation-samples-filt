using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingObjs : MonoBehaviour
{
    [SerializeField] private RotateObj _prefab;

    // Start is called before the first frame update
    void Start()
    {
        Params.Init();
        Invoke("_OnStart",1f);
    }

    void _OnStart(){

        for(int i=0;i<180;i++){
            var obj = Instantiate(_prefab,transform,false);
            
            obj.transform.position = new Vector3(
                Camera.main.transform.position.x + 10f * (Random.value - 0.5f),
                Camera.main.transform.position.y + 2f*(Random.value-0.5f),
                Camera.main.transform.position.z + 10f * (Random.value - 0.5f)
            );

            obj.transform.rotation = Quaternion.Euler(
                360f * Random.value,
                360f * Random.value,
                360f * Random.value
            );

           

            obj.Init();
        }
        _prefab.gameObject.SetActive(false);

    }


    // Update is called once per frame
    void Update()
    {
        Params.SetRotation();
    }
}
