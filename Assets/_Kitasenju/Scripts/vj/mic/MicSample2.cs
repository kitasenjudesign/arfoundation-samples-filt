using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;


[RequireComponent(typeof(AudioSource))]
public class MicSample2 : MonoBehaviour {
//http://mizutanikirin.net/unity-android%E3%81%A7microphone%E3%81%A7%E9%9F%B3%E5%A3%B0%E5%8F%96%E5%BE%97%E3%81%97%E3%81%A6%E5%91%A8%E6%B3%A2%E6%95%B0%E5%88%A5%E3%81%AB%E5%88%86%E8%A7%A3%E3%81%99%E3%82%8B

    public RectTransform[] cubes;
 
    private float[] _spectrum = new float[1024];// * 8];
    private AudioSource audio;
    private List<float> values;
    [SerializeField] private int _targetIndex = 0;
    [SerializeField] private List<int> _indicies;
    [SerializeField,Space(10)] private Slider _slider;
    [SerializeField] private Button _btnIndex; 
    [SerializeField] private Button _btnAuto;
    [SerializeField] private TextMeshProUGUI _btnAutoTxt;

    [SerializeField] private TextMeshProUGUI _btnIndexText;

    [SerializeField] private VJMain _main;

    private float _past =0;
    private bool _isAuto = false;

    void Start() {
 
        audio = GetComponent<AudioSource>();
 
        // Microphone
        // 引数は、デバイス名（null ならデフォルト）、ループ、何秒取るか、サンプリング周波数
        audio.loop = true;
        audio.clip = Microphone.Start(null, true, 43, 96000);
        while (Microphone.GetPosition(null) <= 0) { }
        audio.Play();
        
            /*
            string devName = Microphone.devices[0]; // 複数見つかってもとりあえず0番目のマイクを使用
            int minFreq, maxFreq;
            Microphone.GetDeviceCaps(devName, out minFreq, out maxFreq); // 最大最小サンプリング数を得る
            int ms = minFreq / SampleNum; // サンプリング時間を適切に取る
            audio.loop = true; // ループにする
            audio.clip = Microphone.Start(devName, true, ms, minFreq); // clipをマイクに設定
            while (!(Microphone.GetPosition(devName) > 0)) { } // きちんと値をとるために待つ
            Microphone.GetPosition(null);
            audio.Play();
           
            Debug.Log(" _____ " + ms);
            Debug.Log(" _____ " + maxFreq);
             */


        _indicies = new List<int>();
        values = new List<float>();

        for(int i=0;i<cubes.Length;i++){

            values.Add( 0 );  
            _indicies.Add( i * 1000 / cubes.Length );

        }

        _btnIndex.onClick.AddListener(_changeIndex);
        _btnAuto.onClick.AddListener(_setAuto);
    }

    private void _setAuto(){
        //kaeru
        _isAuto = !_isAuto;
        _btnAutoTxt.text = _isAuto?"AUTO:ON":"AUTO:OFF";
    }

    private void _changeIndex(){

        _targetIndex++;
        _targetIndex = _targetIndex % _indicies.Count;
        
        _btnIndexText.text = "FREQ" + _targetIndex;

    }

    void Update() {
        audio.GetSpectrumData(_spectrum, 0, FFTWindow.BlackmanHarris);

        float limit = 3.5f;

        for(int i=0;i<cubes.Length-1;i++){
            var ss = 0f; 
            
            var startIndex = _indicies[i];
            var endIndex = _indicies[i+1];
            float sum = endIndex - startIndex;
            
            for(int j=startIndex;j<endIndex;j++){
                
                ss += (float)_spectrum[ j ] / (float)sum;

            }
            
            ss *= _slider.value;
           
            
            if(ss>limit) ss = limit;
            values[i] = ss;

            var scale = cubes[i].localScale;
            var tgt = new Vector3(
                ss,
                1f,
                1f
            );
            
            

            //scale += ( tgt - scale ) / 8f;
            if(tgt.x>scale.x){
                scale += (tgt-scale)/2f;
                cubes[i].localScale = scale;
            }else{
                scale += (tgt-scale)/8f;
                cubes[i].localScale = scale;
            }

        }

        //1sec以上たったら変える。

        float vv = values[ _targetIndex] / limit;

        if(vv>0.5f && Time.realtimeSinceStartup-_past>0.5f && _isAuto){
            _past = Time.realtimeSinceStartup;
            _main.SetRandom();
        }

        Params.SetGlobalIntensity( vv );
        

    }
 
}