using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using System.Windows.Forms;
using UnityEngine.UI;

namespace Cave
{
    #region "enum"

    public enum FrustumMode
    {
        CAVEXXL, GPP_Kooima
    };



    #endregion

    public class CaveMain : MonoBehaviour
    {
        #region "settings"
        
        [Header("Wand")]
        public WandSettings WandSettings;

        [Header("Eyes")]
        public EyesSettings EyesSettings;

        [Header("Secondary Cameras")]
        public SecondaryCameraSettings[] SecondaryCameraSettings;
        
        [Header("Cave Settings")]
        public CaveSettings CaveSettings;

        [Header("System")]
        public SystemSettings SystemSettings;
        #endregion

        #region "public properties"
        public Transform CAVELeft { get { return _CAVELeft; } }
        public Transform CAVEFront { get { return _CAVEFront; } }
        public Transform CAVERight { get { return _CAVERight; } }
        public Transform CAVEBottom { get { return _CAVEBottom; } }

        public Transform CAVELeftXXL { get { return _CAVELeftXXL; } }
        public Transform CAVEFrontXXL { get { return _CAVEFrontXXL; } }
        public Transform CAVERightXXL { get { return _CAVERightXXL; } }
        public Transform CAVEBottomXXL { get { return _CAVEBottomXXL; } }

    
        #endregion


        #region "private vars"

        private Transform _CAVELeft;
        private Transform _CAVEFront;
        private Transform _CAVERight;
        private Transform _CAVEBottom;

        private Transform _CAVELeftXXL;
        private Transform _CAVEFrontXXL;
        private Transform _CAVERightXXL;
        private Transform _CAVEBottomXXL;

        //private Eyes _eyes;
        //private Wand _wand;

        //private CameraManager _cameraManager;
        //private FrustumManager _frustumManager;
        //private GameObject _cameraContainer;

        //private List<Camera> _secondaryCameras = new List<Camera>();
        private List<Transform> _walls = new List<Transform>();
        private List<Transform> _wallsXXL = new List<Transform>();

        #endregion

        void Awake()
        {
            Instantiate(SystemSettings.CameraManagerPrefab).name = "CameraManager";
            Instantiate(SystemSettings.FrustumManagerPrefab).name = "FrustumManager";
            Instantiate(SystemSettings.CameraContainerPrefab).name = "CameraContainer";
        }

        // Use this for initialization
        void Start()
        {
            //foreach (Camera c in Camera.allCameras)
            //{
            //    if (c != Camera.main) { _secondaryCameras.Add(c); }
            //};

            _walls.Add(_CAVELeft = GameObject.Find("CaveLeft").GetComponent<Transform>());
            _walls.Add(_CAVEFront = GameObject.Find("CaveFront").GetComponent<Transform>());
            _walls.Add(_CAVERight = GameObject.Find("CaveRight").GetComponent<Transform>());
            _walls.Add(_CAVEBottom = GameObject.Find("CaveBottom").GetComponent<Transform>());

            _wallsXXL.Add(_CAVELeftXXL = GameObject.Find("CaveLeftXXL").GetComponent<Transform>());
            _wallsXXL.Add(_CAVEFrontXXL = GameObject.Find("CaveFrontXXL").GetComponent<Transform>());
            _wallsXXL.Add(_CAVERightXXL = GameObject.Find("CaveRightXXL").GetComponent<Transform>());
            _wallsXXL.Add(_CAVEBottomXXL = GameObject.Find("CaveBottomXXL").GetComponent<Transform>());

            if (!CaveSettings.ShowCave)
            {
                foreach (var w in _walls)
                {
                    w.GetComponent<Renderer>().enabled = false;
                }

                foreach (var w in _wallsXXL)
                {
                    w.GetComponent<Renderer>().enabled = false;
                }

                API.Instance.Wand.DisableRenderer();
                API.Instance.Eyes.DisableRenderer();
            }

            ToggleColliders(false);
        }

        // Update is called once per frame
        void Update()
        {
            transform.position = Camera.main.transform.position;
            transform.rotation = Camera.main.transform.rotation;
        }

        public void ToggleColliders(bool status)
        {
            foreach (var w in _walls)
            {
                w.GetComponent<Collider>().enabled = status;
            }

            foreach (var w in _wallsXXL)
            {
                w.GetComponent<Collider>().enabled = status;
            }
        }
    }
}