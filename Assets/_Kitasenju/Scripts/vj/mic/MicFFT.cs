using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;


[RequireComponent(typeof(AudioSource))]
public class MicFFT : MonoBehaviour {
//http://mizutanikirin.net/unity-android%E3%81%A7microphone%E3%81%A7%E9%9F%B3%E5%A3%B0%E5%8F%96%E5%BE%97%E3%81%97%E3%81%A6%E5%91%A8%E6%B3%A2%E6%95%B0%E5%88%A5%E3%81%AB%E5%88%86%E8%A7%A3%E3%81%99%E3%82%8B

    public RectTransform[] cubes;
 
    private float[] _spectrum = new float[1024];// * 8];
    private AudioSource audio;
    public List<float> values;
    public List<float> subValues;
    private List<float> _oldValues;
    [SerializeField] private int _targetIndex = 0;
    [SerializeField] private List<int> _indicies;
    [SerializeField,Space(10)] private Slider _slider;
    [SerializeField] private Slider _sliderPow; 
    
    [SerializeField] private Button _btnIndex; 
    [SerializeField] private Button _btnAuto;
    [SerializeField] private TextMeshProUGUI _btnAutoTxt;

    [SerializeField] private TextMeshProUGUI _btnIndexText;
    [SerializeField] private VJMain _main;


    private float _past =0;
    private int _isAuto = 0;
    public const int AUTO_OFF=0;
    public const int AUTO_ON=1;
    public const int AUTO_INVERT=2;


    public static MicFFT Instance;

    void Start() {
 
        audio = GetComponent<AudioSource>();
 
        // Microphone
        // 引数は、デバイス名（null ならデフォルト）、ループ、何秒取るか、サンプリング周波数
        //https://qiita.com/ELIXIR/items/595579a9372ef181e0bc
        audio.loop = true;
        audio.clip = Microphone.Start(null, true, 43, 96000);
        while (Microphone.GetPosition(null) <= 0) { }
        audio.Play();
        
        Instance = this;

        _indicies = new List<int>();
        values = new List<float>();
        subValues= new List<float>();
        _oldValues= new List<float>();

        for(int i=0;i<cubes.Length;i++){

            values.Add( 0 );
            subValues.Add( 0 );
            _oldValues.Add( 0 );
            _indicies.Add( i * 1000 / cubes.Length );

        }

        _btnIndex.onClick.AddListener(_changeIndex);
        _btnAuto.onClick.AddListener(_setAuto);

        _targetIndex=2;
        _changeIndex();        
    }

    private void _setAuto(){
        //kaeru
        _isAuto++;
        _isAuto = _isAuto % 3;

        switch(_isAuto){
            case AUTO_OFF:
                _btnAutoTxt.text = "AUTO:OFF";
                break;
            case AUTO_ON:
                _btnAutoTxt.text = "AUTO:ON";
                break;
            case AUTO_INVERT:
                _btnAutoTxt.text = "AUTO:INV";
                break;

        }
        
    }

    private void _changeIndex(){

        _targetIndex++;
        _targetIndex = _targetIndex % _indicies.Count;
        
        _btnIndexText.text = "FREQ" + _targetIndex;

    }

    void Update() {

        audio.GetSpectrumData(_spectrum, 0, FFTWindow.BlackmanHarris);

        float barHeight = 3.5f;

        for(int i=0;i<cubes.Length-1;i++){
            var ss = 0f; 
            
            var startIndex = _indicies[i];
            var endIndex = _indicies[i+1];
            float sum = endIndex - startIndex;
            
            for(int j=startIndex;j<endIndex;j++){
                
                ss += (float)_spectrum[ j ] / (float)sum;

            }
            
            ss *= _slider.value;
           
            if(ss>1f) ss = 1f;
            //values[i] = Mathf.Pow( ss, _sliderPow.value );
            ss = Mathf.Pow( ss, _sliderPow.value );
 
            if(ss>values[i]){
                values[i] = ss;//+= (ss-values[i])/2f;
            }else{
                values[i] += (ss-values[i])/8f;
            }

        }

        //sub value
        for(int i=0;i<values.Count;i++){
            subValues[i] = values[i] - _oldValues[i];
        }

        //old value
        for(int i=0;i<values.Count;i++){
            _oldValues[i] = values[i];
        }


        var tgtValue = values;//subValues;

        float vv = tgtValue[ _targetIndex ];

        for(int i=0;i<cubes.Length;i++){
            cubes[i].localScale = new Vector3(
                tgtValue[i] * barHeight, 1f, 1f
            );
        }


        if(vv>0.5f && Time.realtimeSinceStartup-_past>1f && _isAuto==AUTO_ON){
            //Debug.Log( "change " + vv + " " + Time.realtimeSinceStartup );
            _past = Time.realtimeSinceStartup;
            _main.SetRandom();
        }


        if(vv>0.5f && Time.realtimeSinceStartup-_past>0.2f && _isAuto==AUTO_INVERT){
            //Debug.Log( "change " + vv + " " + Time.realtimeSinceStartup );
            _past = Time.realtimeSinceStartup;
            _main.InvertFilter();
        }



        Params.SetGlobalIntensity( vv );
        

    }
 
}