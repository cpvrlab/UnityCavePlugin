using UnityEngine;
using System.Collections;
using Cave;
using System;
using System.Collections.Generic;

namespace Cave
{
    public class CameraManager : MonoBehaviour
    {
        private List<Camera> _cameras;
        private Dictionary<int, ViewInfo> _viewInfo;

        public Camera CameraWithCursor
        {
            get
            {
                return _cameraWithCursor;
            }
        }

        private CaveMain _main;

        private Camera _cameraLeftLeft = null;
        private Camera _cameraFrontLeft = null;
        private Camera _cameraRightLeft = null;
        private Camera _cameraBottomLeft = null;
        private Camera _cameraLeftRight = null;
        private Camera _cameraFrontRight = null;
        private Camera _cameraRightRight = null;
        private Camera _cameraBottomRight = null;

        private Camera _cameraWithCursor;
        
        struct ViewInfo
        {
            public Camera Left;
            public Camera Right;
            public Plane CAVESide;
        }

        List<List<Camera>> _camMatrix;

        void Start()
        {
            _main = GameObject.Find("Cave").GetComponent<CaveMain>();
   
            _cameraLeftLeft  = GameObject.Find("CameraLeftLeft").GetComponent<Camera>();
            _cameraFrontLeft = GameObject.Find("CameraFrontLeft").GetComponent<Camera>();
            _cameraRightLeft = GameObject.Find("CameraRightLeft").GetComponent<Camera>();
            _cameraBottomLeft  = GameObject.Find("CameraBottomLeft").GetComponent<Camera>();
            _cameraLeftRight = GameObject.Find("CameraLeftRight").GetComponent<Camera>();
            _cameraFrontRight = GameObject.Find("CameraFrontRight").GetComponent<Camera>();
            _cameraRightRight = GameObject.Find("CameraRightRight").GetComponent<Camera>();
            _cameraBottomRight = GameObject.Find("CameraBottomRight").GetComponent<Camera>();

            _viewInfo = new Dictionary<int, ViewInfo>();
            _viewInfo.Add(0, new ViewInfo { Left = _cameraLeftLeft, Right = _cameraLeftRight, CAVESide = _main.CAVELeft });
            _viewInfo.Add(1, new ViewInfo { Left = _cameraFrontLeft, Right = _cameraFrontRight, CAVESide = _main.CAVEFront });
            _viewInfo.Add(2, new ViewInfo { Left = _cameraRightLeft, Right = _cameraRightRight, CAVESide = _main.CAVERight });
            _viewInfo.Add(3, new ViewInfo { Left = _cameraBottomLeft, Right = _cameraBottomRight, CAVESide = _main.CAVEBottom });

            //_cameras = new Dictionary<int, ViewInfo> { _cameraLeftLeft, _cameraFrontLeft, _camera }

            

            // copy settings from main camera
            foreach(var vi in _viewInfo.Values)
            {
                vi.Left.CopyFrom(Camera.main);
                vi.Right.CopyFrom(Camera.main);

                // initially deactivate all cams
                vi.Left.enabled = false;
                vi.Right.enabled = false;
            }

            // deactivate default main camera
            Camera.main.enabled = false;

            _cameraLeftLeft.transform.Rotate(new Vector3(0f, 270f, 0f));
            _cameraLeftRight.transform.Rotate(new Vector3(0f, 270f, 0f));

            _cameraRightLeft.transform.Rotate(new Vector3(0f, 90f, 0f));
            _cameraRightRight.transform.Rotate(new Vector3(0f, 90f, 0f));

            _cameraBottomLeft.transform.Rotate(new Vector3(90f, 0f, 0f));
            _cameraBottomRight.transform.Rotate(new Vector3(90f, 0f, 0f));


            //one of thise is correct
            _cameraLeftLeft.transform.position.Set(_cameraLeftLeft.transform.position.x,_cameraLeftLeft.transform.position.y, _cameraLeftLeft.transform.position.x- (_main.EyeDistance / 2));
            _cameraLeftRight.transform.position.Set(_cameraLeftRight.transform.position.x, _cameraLeftRight.transform.position.x, _cameraLeftRight.transform.position.x+(_main.EyeDistance / 2));
            _cameraFrontLeft.transform.position.Set(-(_main.EyeDistance / 2), 0f, 0f);
            _cameraFrontRight.transform.position.Set(_main.EyeDistance / 2, 0f, 0f);

            _cameraRightLeft.transform.position.Set(0f, 0f, -(_main.EyeDistance / 2));
            _cameraRightRight.transform.position.Set(0f, 0f, -(_main.EyeDistance / 2));

            _cameraBottomLeft.transform.position.Set(-(_main.EyeDistance / 2), 0f, 0f);
            _cameraBottomRight.transform.position.Set(_main.EyeDistance / 2, 0f, 0f);




            _camMatrix = new List<List<Camera>> { };

            if(_main.myCAVEMode == CAVEMode.FourScreen)
            {
                _camMatrix.Add(new List<Camera> { _cameraLeftLeft, _cameraFrontLeft });
                _camMatrix.Add(new List<Camera> { _cameraRightLeft, _cameraBottomLeft });
                _cameraLeftLeft.rect = new Rect(new Vector2(0f, 0.5f), new Vector2(0.5f, 0.5f));
                _cameraFrontLeft.rect = new Rect(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f));
                _cameraRightLeft.rect = new Rect(new Vector2(0f, 0f), new Vector2(0.5f, 0.5f));
                _cameraBottomLeft.rect = new Rect(new Vector2(0.5f, 0f), new Vector2(0.5f, 0.5f));

                

            }
            else if(_main.myCAVEMode == CAVEMode.FourScreenStereo)
            {
                //not correct at the moment
                _camMatrix.Add(new List<Camera> { _cameraLeftLeft, _cameraFrontLeft });
                _camMatrix.Add(new List<Camera> { _cameraRightLeft, _cameraBottomLeft });
                //finished not correct


                _cameraLeftLeft.rect  = new Rect(new Vector2(0f, 0.5f), new Vector2(0.25f, 0.5f));
                _cameraFrontLeft.rect = new Rect(new Vector2(0.5f, 0.5f), new Vector2(0.25f, 0.5f));
                _cameraRightLeft.rect = new Rect(new Vector2(0f, 0f), new Vector2(0.25f, 0.5f));
                _cameraBottomLeft.rect = new Rect(new Vector2(0.5f, 0f), new Vector2(0.25f, 0.5f));

                _cameraLeftRight.rect = new Rect(new Vector2(0.25f, 0.5f), new Vector2(0.25f, 0.5f));
                _cameraFrontRight.rect = new Rect(new Vector2(0.75f, 0.5f), new Vector2(0.25f, 0.5f));
                _cameraRightRight.rect = new Rect(new Vector2(0.25f, 0f), new Vector2(0.25f, 0.5f));
                _cameraBottomRight.rect = new Rect(new Vector2(0.75f, 0f), new Vector2(0.25f, 0.5f));
            };
            
            
        
        }

        void Update()
        {
            int camIndexX, camIndexY;
            camIndexX = camIndexY = 0;

#if UNITY_EDITOR

            //not finished now
            camIndexX = Math.Max(Convert.ToInt32(Mathf.Floor(Convert.ToInt32(Input.mousePosition.x) / CalculatedValues.GetEditorWindow().position.x - CalculatedValues.GetMainGameViewSize()[0])), 0);
            camIndexY = Math.Max(Convert.ToInt32(Mathf.Floor(Convert.ToInt32(Input.mousePosition.y) / CalculatedValues.GetEditorWindow().position.y - CalculatedValues.GetMainGameViewSize()[1])), 0);
            Debug.Log(CalculatedValues.GetEditorWindow());
            Debug.Log("camIndexX: " + camIndexX);
            Debug.Log("camIndexY: " + camIndexY);
#else
            camIndeX = Math.Max(Convert.ToInt32(Mathf.Floor(Convert.ToInt32(Input.mousePosition.x) / _main.BeamerResolutionWidth)), 0);
            camIndexY = Math.Max(Convert.ToInt32(Mathf.Floor(Convert.ToInt32(Input.mousePosition.y) / _main.BeamerResolutionHeight)), 0);
#endif

            // Get Camera based on cursor position
            // int camIndexX = Math.Max(Convert.ToInt32(Mathf.Floor(Convert.ToInt32(Input.mousePosition.x) / _main.BeamerResolutionWidth)), 0);
            //int camIndexY = Math.Max(Convert.ToInt32(Mathf.Floor(Convert.ToInt32(Screen.currentResolution.height - Input.mousePosition.y) / _main.BeamerResolutionHeight)), 0);

            // _cameraWithCursor = _camMatrix[camIndexY][camIndexX];

            // Debug.Log(_cameraWithCursor.name);
        }
    }
}