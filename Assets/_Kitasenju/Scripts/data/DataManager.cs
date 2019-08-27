using UnityEngine;


public class DataManager : MonoBehaviour
{

    public static DataManager Instance;
    public RenderTexStorage texStorage;

    void Awake(){
        
        Instance = this;

    }

    public void InitTexStorage(){
        
        if(texStorage==null){
            texStorage = new RenderTexStorage();
            texStorage.Init();
        }
        
    }


}