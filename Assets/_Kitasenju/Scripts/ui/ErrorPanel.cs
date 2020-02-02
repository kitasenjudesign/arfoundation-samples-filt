using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ErrorPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _device;
    // Start is called before the first frame update
    void Start()
    {
        _device.text = SystemInfo.deviceModel;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
