using UnityEngine;
using System.Collections;

namespace Cave
{
    public sealed class CalculatedValues
    {
        static readonly CalculatedValues _instance = new CalculatedValues();

        public Wand Wand { get { return _wand; } }
        public Eyes Eyes { get { return _eyes; } }
        public CameraManager CameraManager { get { return _cameraManager; } }

        public float AngleWandEyes { get { return _angleWandEyes; } }
        public Vector3 DirectionWandEyes { get { return _directionWandEyes; } }

        private Wand _wand;
        private Eyes _eyes;
        private CameraManager _cameraManager;

        private float _angleWandEyes;
        private Vector3 _directionWandEyes;

        public static CalculatedValues Instance
        {
            get
            {
                return _instance;
            }
        }

        CalculatedValues()
        {
            _wand = GameObject.Find("Wand").GetComponent<Wand>();
            _eyes = GameObject.Find("Eyes").GetComponent<Eyes>();
            _cameraManager = GameObject.Find("CameraManager").GetComponent<CameraManager>();
        }

        public void Calculate()
        {
            // Calculate Angle between Wand / Eyes
            _angleWandEyes = Quaternion.Angle(_wand.transform.rotation, _eyes.transform.rotation);

            // Calculate Vector between Wand / Eyes
            _directionWandEyes = _eyes.transform.position - _wand.transform.position;
        }
    }
}