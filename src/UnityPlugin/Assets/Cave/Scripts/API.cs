using UnityEngine;
using System.Collections;


namespace Cave
{
    public sealed class API
    {
        static readonly API _instance = new API();

        public CaveMain Cave
        {
            get
            {
                if(_main == null)
                {
                    return _main = GameObject.Find("Cave").GetComponent<CaveMain>();
                }

                return _main;
             }
        }

        public Eyes Eyes
        {
            get
            {
                if(_eyes == null)
                {
                    return _eyes = GameObject.Find("WorldVizEyes").GetComponent<Eyes>();
                }

                return _eyes;
            }
        }

        public Wand Wand
        {
            get
            {
                if (_wand == null)
                {
                    return _wand = GameObject.Find("WorldVizWand").GetComponent<Wand>();
                }

                return _wand;
            }
        }

        public CameraManager CameraManager
        {
            get
            {
                if(_cameraManager == null)
                {
                    return _cameraManager = GameObject.Find("CameraManager").GetComponent<CameraManager>();
                }

                return _cameraManager;
            }
        }

        public FrustumManager FrustumManager
        {
            get
            {
                if (_frustumManager == null)
                {
                    return _frustumManager = GameObject.Find("FrustumManager").GetComponent<FrustumManager>();
                }

                return _frustumManager;
            }
        }

        public GameObject CameraContainer
        {
            get
            {
                if (_cameraContainer == null)
                {
                    return _cameraContainer = GameObject.Find("CameraContainer");
                }

                return _cameraContainer;
            }
        }

        public Vector2 GameViewSize { get { return GetMainGameViewSize(); } }
        public Quaternion AngleWandEyes { get { return Quaternion.Inverse(Eyes.transform.rotation) * Wand.transform.rotation; } }
        public Vector3 DirectionWandEyes { get { return Eyes.transform.position - Wand.transform.position; } }

        public Vector2 WandJoystick { get { return Wand.JoystickPosition; } }

        private CaveMain _main;
        private Eyes _eyes;
        private Wand _wand;
        private CameraManager _cameraManager;
        private FrustumManager _frustumManager;
        private GameObject _cameraContainer;

        public static API Instance
        {
            get
            {
                return _instance;
            }
        }

        API()
        { }

        public static Vector2 GetMainGameViewSize()
        {
            System.Type T = System.Type.GetType("UnityEditor.GameView,UnityEditor");
            System.Reflection.MethodInfo GetSizeOfMainGameView = T.GetMethod("GetSizeOfMainGameView", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            System.Object Res = GetSizeOfMainGameView.Invoke(null, null);
            return (Vector2)Res;
        }

        //public static UnityEditor.EditorWindow GetEditorWindow()
        //{
        //    System.Reflection.Assembly assembly = typeof(UnityEditor.EditorWindow).Assembly;
        //    System.Type type = assembly.GetType("UnityEditor.GameView");
        //    UnityEditor.EditorWindow gameview = UnityEditor.EditorWindow.GetWindow(type);
        //    return gameview;
        //}
    }
}