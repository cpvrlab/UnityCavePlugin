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

    public enum TrackedObject
    {
        Eyes, Wand, Nothing
    };

    #endregion

    public class CaveMain : MonoBehaviour
    {
        #region "settings"

        [Header("Main")]
        public int BeamerResolutionWidth = 1280;
        public int BeamerResolutionHeight = 1024;
        public string Host = "192.168.0.201";
        public FrustumMode FrustumMode = FrustumMode.CAVEXXL;
        public BasicSettings.Sides GUILocation = BasicSettings.Sides.Front;
        public float EyeDistance = 0.07f;
        //public TrackedObject myTrackingMode = TrackedObject.Eyes;
        //public bool rotateCave = true;
        //this is used to calc the frustum
        //public CAVEDimensions CaveDimensions;

        [Header("Wand")]
        public WandSettings WandSettings;

        [Header("Eyes")]
        public EyesSettings EyesSettings;

        [Header("Gamepad")]
        public GamepadSettings GamepadSettings;

        [Header("Secondary Cameras")]
        public SecondaryCameraSettings[] SecondaryCameraSettings;

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

        //public Eyes Eyes { get { return _eyes; } }
        //public Wand Wand { get { return _wand; } }

        //public CameraManager CameraManager { get { return _cameraManager; } }
        //public FrustumManager FrustumManager { get { return _frustumManager; } }
        //public GameObject CameraContainer { get { return _cameraContainer; } }


        //public Vector3 currentTrackedObject { get { return _TrackedObject; } }

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

        private List<Camera> mySecondaryCameras = new List<Camera>();
        private List<Transform> _walls = new List<Transform>();
        private List<Transform> _wallsXXL = new List<Transform>();

        #endregion

        void Awake()
        {
            Instantiate(SystemSettings.CameraManagerPrefab);
            Instantiate(SystemSettings.FrustumManagerPrefab);
            Instantiate(SystemSettings.CameraContainerPrefab);
        }

        // Use this for initialization
        void Start()
        {
            foreach (Camera c in Camera.allCameras)
            {
                if (c != Camera.main) { mySecondaryCameras.Add(c); }
            };

            _walls.Add(_CAVELeft = GameObject.FindWithTag("CaveLeft").GetComponent<Transform>());
            _walls.Add(_CAVEFront = GameObject.FindWithTag("CaveFront").GetComponent<Transform>());
            _walls.Add(_CAVERight = GameObject.FindWithTag("CaveRight").GetComponent<Transform>());
            _walls.Add(_CAVEBottom = GameObject.FindWithTag("CaveBottom").GetComponent<Transform>());

            _wallsXXL.Add(_CAVELeftXXL = GameObject.FindWithTag("CaveLeftXXL").GetComponent<Transform>());
            _wallsXXL.Add(_CAVEFrontXXL = GameObject.FindWithTag("CaveFrontXXL").GetComponent<Transform>());
            _wallsXXL.Add(_CAVERightXXL = GameObject.FindWithTag("CaveRightXXL").GetComponent<Transform>());
            _wallsXXL.Add(_CAVEBottomXXL = GameObject.FindWithTag("CaveBottomXXL").GetComponent<Transform>());

            if (!SystemSettings.ShowCave)
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

            //_eyes = GameObject.FindWithTag("Eyes").GetComponent<Eyes>();
            //_wand = GameObject.FindWithTag("Wand").GetComponent<Wand>();

            //_cameraManager = GameObject.FindWithTag("CameraManager").GetComponent<CameraManager>();
            //_frustumManager = GameObject.FindWithTag("FrustumManager").GetComponent<FrustumManager>();
            //_cameraContainer = GameObject.FindWithTag("CameraContainer");

            //if (CAVEMode == CAVEMode.FourScreen) EyeDistance = 0f;

            ToggleColliders(false);

            // expand playersettings for mobile ionput, so that we have access to the virtualAxis from wand
            //String scriptDefineSymbols = UnityEditor.PlayerSettings.GetScriptingDefineSymbolsForGroup(UnityEditor.BuildTargetGroup.Standalone);
            //if (scriptDefineSymbols.IndexOf("MOBILE_INPUT") < 0)
            //{
            //    UnityEditor.PlayerSettings.SetScriptingDefineSymbolsForGroup(UnityEditor.BuildTargetGroup.Standalone, scriptDefineSymbols + "; MOBILE_INPUT");
            //}
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