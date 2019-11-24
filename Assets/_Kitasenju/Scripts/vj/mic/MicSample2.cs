using UnityEngine;
using System.Collections;
using UnityEngine.UI;
 
[RequireComponent(typeof(AudioSource))]
public class MicSample2 : MonoBehaviour {
//http://mizutanikirin.net/unity-android%E3%81%A7microphone%E3%81%A7%E9%9F%B3%E5%A3%B0%E5%8F%96%E5%BE%97%E3%81%97%E3%81%A6%E5%91%A8%E6%B3%A2%E6%95%B0%E5%88%A5%E3%81%AB%E5%88%86%E8%A7%A3%E3%81%99%E3%82%8B

    public GameObject[] cube;
    public int[] Hertz;
    public float MeterFactor = 1;
    public float BUZZER_HZ_MIN;
    public int BUZZER_REPEAT_MIN;
 
    private float[] _spectrum = new float[1024];// * 8];
    private AudioSource audio;
    private float[] values;
    [SerializeField] private Slider _slider;
 
    void Start() {
 
 
        audio = GetComponent<AudioSource>();
 
        // Microphone
        // 引数は、デバイス名（null ならデフォルト）、ループ、何秒取るか、サンプリング周波数
        audio.clip = Microphone.Start(null, true, 999, 44100);
        while (Microphone.GetPosition(null) <= 0) { }
        audio.Play();
    }
 
    void Update() {
        audio.GetSpectrumData(_spectrum, 0, FFTWindow.BlackmanHarris);

        for(int i=0;i<cube.Length;i++){
            var ss = 0.1f +_spectrum[ i*100 ]*_slider.value;
            var scale = cube[i].transform.localScale;
            var tgt = new Vector3(
                ss,
                ss,
                ss
            );
            //scale += ( tgt - scale ) / 8f;
            if(tgt.x>scale.x){
                cube[i].transform.localScale = tgt;
            }else{
                scale += (tgt-scale)/8f;
                cube[i].transform.localScale = scale;
            }

        }

        /*
        values = new float[cube.Length];
 
        for (int i = 0; i < _spectrum.Length; ++i) {
            var freq = ((AudioSettings.outputSampleRate * 0.5) / _spectrum.Length) * i;
            var idx = SpectrumToIndex((int)freq);
            if (idx >= 0) {
                values[idx] = Mathf.Max(_spectrum[i], values[idx]);
            }
        }
 
        for (int i = 0; i < cube.Length; ++i) {
            float meterValue = System.Math.Min(1.0f, values[i]);// * int.Parse(MeterFactorInput.text));
            cube[i].transform.localScale = new Vector3(cube[i].transform.localScale.x, meterValue, cube[i].transform.localScale.z);
        }
        */
 
    }
 
    int SpectrumToIndex(int freq) {
        for (int n = 0; n < Hertz.Length; n++) {
            if (Hertz[n] > freq) {
                return n;
            }
        }
        return -1;
    }
}