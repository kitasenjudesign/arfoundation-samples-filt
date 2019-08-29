using UnityEngine;
using System.Runtime.InteropServices;

public class VibeManager : MonoBehaviour
{
    public static VibeManager Instance; 

    public static int Vibe01 = 1519;//弱め1
    public static int Vibe02 = 1520;//弱め2
    public static int Vibe03 = 1521;//強め


    void Awake(){
        Instance = this;
    }



#if UNITY_IOS && !UNITY_EDITOR
        [DllImport ("__Internal")]
        static extern void _playSystemSound(int n);
#endif

        public void PlaySystemSound(int n) //引数にIDを渡す
        {
#if UNITY_IOS && !UNITY_EDITOR
            _playSystemSound(n);
#endif
        }



}