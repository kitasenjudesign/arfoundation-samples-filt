using UnityEngine;


public class DataManager : MonoBehaviour
{

    public static DataManager Instance;
    public RenderTexManager texStorage;

    void Awake(){
        
        Instance = this;

    }

    public void InitTexStorage(){
        
        if(texStorage==null){
            texStorage = new RenderTexManager();
            texStorage.Init();
        }
        
    }

}