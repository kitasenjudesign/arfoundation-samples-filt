using UnityEngine;

namespace RDSystem
{
    public class RDSystemUpdater : MonoBehaviour
    {
        [SerializeField] CustomRenderTexture _texture;
        [SerializeField, Range(1, 16)] int _stepsPerFrame = 4;

        void Start()
        {
            _texture.Initialize();
        }

        private void OnGUI()
        {
                /*
                GUI.DrawTexture(
                    new Rect(0, 0, 562/2, 1218/2), 
                    _texture, 
                    ScaleMode.StretchToFill,
                    false
                );*/
        }


        void Update()
        {
            _texture.Update(_stepsPerFrame);
        }
    }
}
