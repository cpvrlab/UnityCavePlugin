using UnityEngine;
using System.Collections;


namespace Cave
{
    public sealed class API
    {
        static readonly API _instance = new API();

        public Wand Wand { get { return _wand; } }
        public Eyes Eyes { get { return _eyes; } }
        public CameraManager CameraManager { get { return _main.CameraManager; } }

        public Vector2 GameViewSize {  get { return GetMainGameViewSize(); } }

        public Quaternion AngleWandEyes { get { return _angleWandEyes; } }
        public Vector3 DirectionWandEyes { get { return _directionWandEyes; } }

        private CaveMain _main;
        private Wand _wand;
        private Eyes _eyes;

        private Quaternion _angleWandEyes;
        private Vector3 _directionWandEyes;

        public static API Instance
        {
            get
            {
                return _instance;
            }
        }

        API()
        {
            _main = GameObject.Find("Cave").GetComponent<CaveMain>();
            _wand = GameObject.Find("Wand").GetComponent<Wand>();
            _eyes = GameObject.Find("Eyes").GetComponent<Eyes>();
        }

        public void Calculate()
        {
            // Calculate Angle between Wand / Eyes
            _angleWandEyes = Quaternion.Inverse(API.Instance.Eyes.transform.rotation) * API.Instance.Wand.transform.rotation;

            // Calculate Vector between Wand / Eyes
            _directionWandEyes = _eyes.transform.position - _wand.transform.position;
        }

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