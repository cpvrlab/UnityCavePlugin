using UnityEngine;
using System.Collections;
using Cave;
using System;
using System.Collections.Generic;

namespace Cave
{
    public class CameraManager : MonoBehaviour
    {

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

            _cameraLeftLeft.CopyFrom(Camera.main);
            _cameraFrontLeft.CopyFrom(Camera.main);
            _cameraRightLeft.CopyFrom(Camera.main);
            _cameraBottomLeft.CopyFrom(Camera.main);
            _cameraLeftRight.CopyFrom(Camera.main);
            _cameraFrontRight.CopyFrom(Camera.main);
            _cameraRightRight.CopyFrom(Camera.main);
            _cameraBottomRight.CopyFrom(Camera.main);

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
           // camIndexX = Math.Max(Convert.ToInt32(Mathf.Floor(Convert.ToInt32(Input.mousePosition.x) / CalculatedValues.GetEditorWindow().position.x-CalculatedValues.GetMainGameViewSize()[0])), 0);
          //  camIndexY = Math.Max(Convert.ToInt32(Mathf.Floor(Convert.ToInt32(Input.mousePosition.y) / CalculatedValues.GetEditorWindow().position.y-CalculatedValues.GetMainGameViewSize()[1])), 0);
          //  Debug.Log(CalculatedValues.GetEditorWindow());
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