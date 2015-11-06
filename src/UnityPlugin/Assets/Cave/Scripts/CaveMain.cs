using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Cave
{

    public enum CAVEMode
    {
        FourScreen, FourScreenStereo
    };

        
    public class CaveMain : MonoBehaviour
    {
        

        [Header("Main")]
        public int BeamerResolutionWidth = 1280;
        public int BeamerResolutionHeight = 960;
        public string Host = "192.168.0.201";
        public CAVEMode myCAVEMode = CAVEMode.FourScreen;

        [Header("Wand")]
        public WandSettings WandSettings;

        [Header("Eyes")]
        public EyesSettings EyesSettings;

        [Header("Gamepad")]
        public GamepadSettings GamepadSettings;

        [Header("Weiteres")]
        public float EyeDistance = 0.07f;

        
        private List<Camera> mySecondaryCameras = new List<Camera>();
        
        // Use this for initialization
        void Start()
        {
           
            foreach(Camera c in Camera.allCameras)
            {
                if (c != Camera.main) { mySecondaryCameras.Add(c); }
            };

        }

        // Update is called once per frame
        void Update()
        {
            //performance ?
            CalculatedValues.Instance.Calculate();

           // Debug.Log(CalculatedValues.Instance.AngleWandEyes);
        }

        private void SetCameraTag()
        {

        }
    }
}