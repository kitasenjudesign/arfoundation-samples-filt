using UnityEngine;
using UnityEngine.UI;

public class EventTriggerTest : MonoBehaviour {
 
    [SerializeField] private Button _btn;

    void Start(){
        
    }

    public void OnUp() {
        Debug.Log("onup");
    }
    public void OnDown() {
        Debug.Log("ondown");
    }
    public void OnExit() {
        Debug.Log("onExit");
    }

}