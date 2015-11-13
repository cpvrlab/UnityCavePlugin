using UnityEngine;
using System.Collections;
using Cave;
using System;
using System.Collections.Generic;

namespace Cave
{
    public class CameraManager : MonoBehaviour
    {

#region "structs"

        public struct ViewInfo
        {
            public Camera Left;
            public Camera Right;
            public CAVEPlanesettings CAVESide;
        }

        public struct CAVEPlanesettings
        {
            public string name;
            public float width;
            public float height;
            public Vector3 center;
            public Vector3 normal;
            public Transform Transform;
            public Plane Plane;
            public Vector3 up;
        }

        #endregion

        #region "public properties"
        public Camera CameraWithCursor { get { return _cameraWithCursor; } }
        public Dictionary<int,ViewInfo> FullViewInfo { get { return _viewInfo; } }

#endregion

#region "private vars"

        private List<Camera> _cameras;
        private Dictionary<int, ViewInfo> _viewInfo;

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
        
#endregion
        
        void Awake()
        {
            _main = GameObject.Find("Cave").GetComponent<CaveMain>();
        }

        void Start()
        {
            _cameraLeftLeft = GameObject.Find("CameraLeftLeft").GetComponent<Camera>();
            _cameraFrontLeft = GameObject.Find("CameraFrontLeft").GetComponent<Camera>();
            _cameraRightLeft = GameObject.Find("CameraRightLeft").GetComponent<Camera>();
            _cameraBottomLeft = GameObject.Find("CameraBottomLeft").GetComponent<Camera>();
            _cameraLeftRight = GameObject.Find("CameraLeftRight").GetComponent<Camera>();
            _cameraFrontRight = GameObject.Find("CameraFrontRight").GetComponent<Camera>();
            _cameraRightRight = GameObject.Find("CameraRightRight").GetComponent<Camera>();
            _cameraBottomRight = GameObject.Find("CameraBottomRight").GetComponent<Camera>();

            _viewInfo = new Dictionary<int, ViewInfo>();

            // Add Settings Left
            _viewInfo.Add(0, new ViewInfo
            {
                Left = _cameraLeftLeft,
                Right = _cameraLeftRight,
                CAVESide = new CAVEPlanesettings
                {
                    name = "left",
                    center = new Vector3(_main.CaveDimensions.lHeight / 2, 0f, 0f),
                    normal = new Vector3(-1f, 0f, 0f),
                    height = _main.CaveDimensions.lHeight,
                    width = _main.CaveDimensions.lWidth,
                    Transform = _main.CAVELeft,
                    up = new Vector3(0, 1, 0)
                }
            });

            Debug.Log("height: " + _main.CAVELeft.transform.localScale);

            // Add Settings Front
            _viewInfo.Add(1, new ViewInfo
            {
                Left = _cameraFrontLeft,
                Right = _cameraFrontRight,
                CAVESide = new CAVEPlanesettings
                {
                    name = "front",
                    center = new Vector3(0f, 0f, _main.CaveDimensions.lWidth / 2),
                    normal = new Vector3(0f, 0f, 1f),
                    width = _main.CaveDimensions.fHeight,
                    height = _main.CaveDimensions.fWidth,
                    Transform = _main.CAVEFront,
                    up = new Vector3(0, 1, 0)
                }
            });

            // Add Settings Right
            _viewInfo.Add(2, new ViewInfo
            {
                Left = _cameraRightLeft,
                Right = _cameraRightRight,
                CAVESide = new CAVEPlanesettings
                {
                    name = "right",
                    center = new Vector3(_main.CaveDimensions.rHeight / 2, 0f, 0f),
                    normal = new Vector3(1f, 0f, 0f),
                    height = _main.CaveDimensions.rHeight,
                    width = _main.CaveDimensions.rWidth,
                    Transform = _main.CAVERight,
                    up = new Vector3(0, 1, 0)
                }
            });

            // Add Settings Bottom
            _viewInfo.Add(3, new ViewInfo
            {
                Left = _cameraBottomLeft,
                Right = _cameraBottomRight,
                CAVESide = new CAVEPlanesettings
                {
                    name = "bottom",
                    center = new Vector3(0f, _main.CaveDimensions.bHeight / 2, 0f),
                    normal = new Vector3(0f, 1f, 0f),
                    height = _main.CaveDimensions.bHeight,
                    width = _main.CaveDimensions.bWidth,
                    Transform = _main.CAVEBottom,
                    up = new Vector3(1, 0, 0)
                }
            });

            // copy settings from main camera
            foreach (var vi in _viewInfo.Values)
            {
                vi.Left.CopyFrom(Camera.main);
                vi.Right.CopyFrom(Camera.main);

                // initially deactivate all cams
                vi.Left.enabled = false;
                vi.Left.depth = 1;
                vi.Left.fieldOfView = 90;

                vi.Right.enabled = false;
                vi.Right.depth = 1;
                vi.Right.fieldOfView = 90;
            }

            // deactivate default main camera
            //Camera.main.enabled = false;
            Camera.main.depth = -1;

            _cameraLeftLeft.transform.Rotate(new Vector3(0f, 270f, 0f));
            _cameraLeftRight.transform.Rotate(new Vector3(0f, 270f, 0f));

            _cameraRightLeft.transform.Rotate(new Vector3(0f, 90f, 0f));
            _cameraRightRight.transform.Rotate(new Vector3(0f, 90f, 0f));

            _cameraBottomLeft.transform.Rotate(new Vector3(90f, 0f, 0f));
            _cameraBottomRight.transform.Rotate(new Vector3(90f, 0f, 0f));


            //one of thise is correct
            _cameraLeftLeft.transform.position.Set(_cameraLeftLeft.transform.position.x, _cameraLeftLeft.transform.position.y, _cameraLeftLeft.transform.position.x - (_main.EyeDistance / 2));
            _cameraLeftRight.transform.position.Set(_cameraLeftRight.transform.position.x, _cameraLeftRight.transform.position.x, _cameraLeftRight.transform.position.x + (_main.EyeDistance / 2));
            _cameraFrontLeft.transform.position.Set(-(_main.EyeDistance / 2), 0f, 0f);
            _cameraFrontRight.transform.position.Set(_main.EyeDistance / 2, 0f, 0f);

            _cameraRightLeft.transform.position.Set(0f, 0f, -(_main.EyeDistance / 2));
            _cameraRightRight.transform.position.Set(0f, 0f, -(_main.EyeDistance / 2));

            _cameraBottomLeft.transform.position.Set(-(_main.EyeDistance / 2), 0f, 0f);
            _cameraBottomRight.transform.position.Set(_main.EyeDistance / 2, 0f, 0f);

            if (_main.myCAVEMode == CAVEMode.FourScreen)
            {
                _cameraLeftLeft.rect = new Rect(new Vector2(0f, 0.5f), new Vector2(0.5f, 0.5f));
                _cameraFrontLeft.rect = new Rect(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f));
                _cameraRightLeft.rect = new Rect(new Vector2(0f, 0f), new Vector2(0.5f, 0.5f));
                _cameraBottomLeft.rect = new Rect(new Vector2(0.5f, 0f), new Vector2(0.5f, 0.5f));

                foreach (var vi in _viewInfo.Values)
                {
                    vi.Left.enabled = true;
                    vi.Right.enabled = false;
                }
            }
            else if (_main.myCAVEMode == CAVEMode.FourScreenStereo)
            {

                _cameraLeftLeft.rect = new Rect(new Vector2(0f, 0.5f), new Vector2(0.25f, 0.5f));
                _cameraFrontLeft.rect = new Rect(new Vector2(0.5f, 0.5f), new Vector2(0.25f, 0.5f));
                _cameraRightLeft.rect = new Rect(new Vector2(0f, 0f), new Vector2(0.25f, 0.5f));
                _cameraBottomLeft.rect = new Rect(new Vector2(0.5f, 0f), new Vector2(0.25f, 0.5f));

                _cameraLeftRight.rect = new Rect(new Vector2(0.25f, 0.5f), new Vector2(0.25f, 0.5f));
                _cameraFrontRight.rect = new Rect(new Vector2(0.75f, 0.5f), new Vector2(0.25f, 0.5f));
                _cameraRightRight.rect = new Rect(new Vector2(0.25f, 0f), new Vector2(0.25f, 0.5f));
                _cameraBottomRight.rect = new Rect(new Vector2(0.75f, 0f), new Vector2(0.25f, 0.5f));

                foreach (var vi in _viewInfo.Values)
                {
                    vi.Left.enabled = true;
                    vi.Right.enabled = true;
                }
            };
        }

        void Update()
        {
            if (_main == null) return;


#if UNITY_EDITOR
            int divisorX = 2;
            int divisorY = 2;
            if (_main.myCAVEMode == CAVEMode.FourScreenStereo) { divisorX = 4; }
            int camIndexX, camIndexY;
            camIndexX = camIndexY = 0;
            camIndexX = Convert.ToInt32(Mathf.Floor(Convert.ToInt32(Input.mousePosition.x) / Convert.ToInt32(CalculatedValues.GetMainGameViewSize()[0] / divisorX)));
            camIndexY = Convert.ToInt32(Mathf.Floor(Convert.ToInt32(Input.mousePosition.y) / Convert.ToInt32(CalculatedValues.GetMainGameViewSize()[1] / divisorY)));
            
#else
            camIndeX = Math.Max(Convert.ToInt32(Mathf.Floor(Convert.ToInt32(Input.mousePosition.x) / _main.BeamerResolutionWidth)), 0);
            camIndexY = Math.Max(Convert.ToInt32(Mathf.Floor(Convert.ToInt32(Input.mousePosition.y) / _main.BeamerResolutionHeight)), 0);
#endif

            // Get Camera based on cursor position
            
 
            switch ((camIndexX*10)+camIndexY)
            {
                case 0:
                    _cameraWithCursor = _viewInfo[2].Left;
                    break;
                case 1:
                    _cameraWithCursor = _viewInfo[0].Left;
                    break;
                case 10:
                    if(_main.myCAVEMode == CAVEMode.FourScreen) { _cameraWithCursor = _viewInfo[3].Left; }
                    else { _cameraWithCursor = _viewInfo[2].Right; }
                    break;
                case 11:
                    if (_main.myCAVEMode == CAVEMode.FourScreen) { _cameraWithCursor = _viewInfo[1].Left; }
                    else { _cameraWithCursor = _viewInfo[0].Right; }
                    break;
                case 20:
                     _cameraWithCursor = _viewInfo[3].Left; 
                    break;
                case 21:
                    _cameraWithCursor = _viewInfo[1].Left;
                    break;
                case 30:
                    _cameraWithCursor = _viewInfo[3].Right;
                    break;
                case 31:
                    _cameraWithCursor = _viewInfo[1].Right;
                    break;
            }

            //Debug.Log(string.Format("window pos x:{0}, window pos y:{1}, window w:{2}, window h:{3}, mouse x:{4}, mouse y:{5} , {6} {7} {8}",
            //CalculatedValues.GetEditorWindow().position.x,
            //CalculatedValues.GetEditorWindow().position.y,
            //CalculatedValues.GetMainGameViewSize()[0],
            //CalculatedValues.GetMainGameViewSize()[1],
            //Input.mousePosition.x,
            //Input.mousePosition.y, camIndexX, camIndexY, _cameraWithCursor.name
            //));


            // Rotate CAVE according to Maincamera
            _main.transform.rotation = Camera.main.transform.rotation;
        }
    }
}