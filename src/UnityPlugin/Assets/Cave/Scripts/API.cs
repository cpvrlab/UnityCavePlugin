using UnityEngine;
using System.Collections;


namespace Cave
{
    public sealed class API
    {
        static readonly API _instance = new API();

        public CaveMain Cave { get { return _main; } }
        public Eyes Eyes { get { return _main.Eyes; } }
        public Wand Wand { get { return _main.Wand; } }

        public Vector2 GameViewSize { get { return GetMainGameViewSize(); } }
        public Quaternion AngleWandEyes { get { return _angleWandEyes; } }
        public Vector3 DirectionWandEyes { get { return _directionWandEyes; } }

        private CaveMain _main;
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
        }

        public void Calculate()
        {
            // Calculate Angle between Wand / Eyes
            _angleWandEyes = Quaternion.Inverse(_main.Eyes.transform.rotation) * _main.Wand.transform.rotation;

            // Calculate Vector between Wand / Eyes
            _directionWandEyes = _main.Eyes.transform.position - _main.Wand.transform.position;
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