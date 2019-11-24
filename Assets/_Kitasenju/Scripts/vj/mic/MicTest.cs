using UnityEngine;
using System.Collections;

public class MicTest : MonoBehaviour {
    
    private float[] _spectrum;
    [SerializeField] private GameObject _prefab;
    private GameObject[] _balls;
    [SerializeField] private float _intensity = 1f;

    private AudioSource _audio;

    void Start() {
        
        /*
        _spectrum = new float[256];
        _balls= new GameObject[256];
        int idx = 0;

        for(int i=0;i<16;i++){
           for(int j=0;j<16;j++){
            
            var b = Instantiate(_prefab,transform,false);
            b.gameObject.SetActive(true);
            b.transform.localPosition = new Vector3(
                10f * ( (float)i/16f - 0.5f ),
                10f * ( (float)j/16f - 0.5f ),
                0
            );
            _spectrum[idx] = 0;
            _balls[idx] = b;
            idx++;

           }
        }
        _prefab.gameObject.SetActive(false);
        */

        // 空の Audio Sourceを取得
        _audio = GetComponent<AudioSource>();
        // Audio Source の Audio Clip をマイク入力に設定
        // 引数は、デバイス名（null ならデフォルト）、ループ、何秒取るか、サンプリング周波数
        _audio.clip = Microphone.Start(null, false, 59*60, 44100);

              
//http://mizutanikirin.net/unity-android%E3%81%A7microphone%E3%81%A7%E9%9F%B3%E5%A3%B0%E5%8F%96%E5%BE%97%E3%81%97%E3%81%A6%E5%91%A8%E6%B3%A2%E6%95%B0%E5%88%A5%E3%81%AB%E5%88%86%E8%A7%A3%E3%81%99%E3%82%8B

        // マイクが Ready になるまで待機（一瞬）
        while (Microphone.GetPosition(null) <= 0) {}
        // 再生開始（録った先から再生、スピーカーから出力するとハウリングします）
        _audio.Play();
        //_audio.mute = false;  

    }

    void Update()
    {
        /*
        AudioListener.GetSpectrumData(_spectrum, 0, FFTWindow.Rectangular);

        for(int i=0;i<_balls.Length;i++){

            float ss = 0.1f + _spectrum[i] * _intensity;
            _balls[i].transform.localScale = new Vector3(
                ss,
                ss,
                ss
            );

        }*/

       //var spectrum = _audio.GetSpectrumData(256, 0, FFTWindow.Hamming);
        
        //Debug.Log( _spectrum[0] );

        /*for (int i = 1; i < spectrum.Length - 1; ++i) {
            Debug.DrawLine(
                    new Vector3(i - 1, spectrum[i] + 10, 0), 
                    new Vector3(i, spectrum[i + 1] + 10, 0), 
                    Color.red);
            Debug.DrawLine(
                    new Vector3(i - 1, Mathf.Log(spectrum[i - 1]) + 10, 2), 
                    new Vector3(i, Mathf.Log(spectrum[i]) + 10, 2), 
                    Color.cyan);
            Debug.DrawLine(
                    new Vector3(Mathf.Log(i - 1), spectrum[i - 1] - 10, 1), 
                    new Vector3(Mathf.Log(i), spectrum[i] - 10, 1), 
                    Color.green);
            Debug.DrawLine(
                    new Vector3(Mathf.Log(i - 1), Mathf.Log(spectrum[i - 1]), 3), 
                    new Vector3(Mathf.Log(i), Mathf.Log(spectrum[i]), 3), 
                    Color.yellow);
        }*/


    }        

}