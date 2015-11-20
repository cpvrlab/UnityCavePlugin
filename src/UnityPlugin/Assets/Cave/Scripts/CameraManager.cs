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

        public struct CameraInfo
        {
            public Camera cam;
            public Vector3 offset;
        }

        public struct ViewInfo
        {
            public CameraInfo Left;
            public CameraInfo Right;
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
            public Transform TransformXXL;
            public Vector3 up;
            public corners corners;
        }

        public struct corners
        {
            public Vector3 topleft;
            public Vector3 bottomleft;
            public Vector3 bottomright;
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

            transform.parent = _main.gameObject.transform;
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
                Left = new CameraInfo
                {
                    cam = _cameraLeftLeft,
                    offset = new Vector3(0f, 0f, -(_main.EyeDistance / 2))
                },
                Right = new CameraInfo
                {
                    cam = _cameraLeftRight,
                    offset = new Vector3(0f, 0f, +(_main.EyeDistance / 2))
                },
                CAVESide = new CAVEPlanesettings
                {
                    name = "left",
                    width = _main.CAVELeft.transform.localScale.z * 10f,
                    height = _main.CAVELeft.transform.localScale.x * 10f,
                    center = new Vector3(_main.CAVELeft.transform.localScale.x * 10f / 2, 0f, 0f),
                    normal = new Vector3(-1f, 0f, 0f),
                    Transform = _main.CAVELeft,
                    TransformXXL = _main.CAVELeftXXL,
                    up = new Vector3(0, 1, 0),
                    corners = new corners
                    {
                        topleft = new Vector3(5f, 0f,5f),
                        bottomleft = new Vector3(-5f, 0f, 5f),
                        bottomright = new Vector3(-5f, 0f, -5f)
                    }
        }
            });

            // Add Settings Front
            _viewInfo.Add(1, new ViewInfo
            {
                Left = new CameraInfo
                {
                    cam = _cameraFrontLeft,
                    offset = new Vector3(-(_main.EyeDistance / 2), 0f, 0f)
                },
                Right = new CameraInfo
                {
                    cam = _cameraFrontRight,
                    offset = new Vector3(+(_main.EyeDistance / 2), 0f, 0f)
                },
                CAVESide = new CAVEPlanesettings
                {
                    name = "front",
                    width = _main.CAVEFront.transform.localScale.z * 10f,
                    height = _main.CAVEFront.transform.localScale.x * 10f,
                    center = new Vector3(0f, 0f, _main.CAVEFront.transform.localScale.z * 10f / 2),
                    normal = new Vector3(0f, 0f, 1f),
                    Transform = _main.CAVEFront,
                    TransformXXL = _main.CAVEFrontXXL,
                    up = new Vector3(0, 1, 0),
                    corners = new corners
                    {
                        topleft = new Vector3(5f, 0f,5f),
                        bottomleft = new Vector3(-5f, 0f, 5f),
                        bottomright = new Vector3(-5f, 0f, -5f)
                    }
                }
            });

            // Add Settings Right
            _viewInfo.Add(2, new ViewInfo
            {
                Left = new CameraInfo
                {
                    cam = _cameraRightLeft,
                    offset = new Vector3(0f,0f,+(_main.EyeDistance / 2))
                },
                Right = new CameraInfo
                {
                    cam = _cameraRightRight,
                    offset = new Vector3(0f, 0f, -(_main.EyeDistance / 2))
                },
                CAVESide = new CAVEPlanesettings
                {
                    name = "right",
                    width = _main.CAVERight.transform.localScale.z * 10f,
                    height = _main.CAVERight.transform.localScale.x * 10f,
                    center = new Vector3(_main.CAVERight.transform.localScale.x * 10f / 2, 0f, 0f),
                    normal = new Vector3(1f, 0f, 0f),
                    Transform = _main.CAVERight,
                    TransformXXL = _main.CAVERightXXL,
                    up = new Vector3(0, 1, 0),
                    corners = new corners
                    {
                        topleft = new Vector3(5f, 0f,5f),
                        bottomleft = new Vector3(-5f, 0f, 5f),
                        bottomright = new Vector3(-5f, 0f, -5f)
                    }

                }
            });

            // Add Settings Bottom
            _viewInfo.Add(3, new ViewInfo
            {
                Left = new CameraInfo
                {
                    cam = _cameraBottomLeft,
                    offset = new Vector3(-(_main.EyeDistance / 2), 0f, 0f)
                },
                Right = new CameraInfo
                {
                    cam = _cameraBottomRight,
                    offset = new Vector3(+(_main.EyeDistance / 2), 0f, 0f)
                },
                CAVESide = new CAVEPlanesettings
                {
                    name = "bottom",
                    width = _main.CAVEBottom.transform.localScale.z * 10f,
                    height = _main.CAVEBottom.transform.localScale.x * 10f,
                    center = new Vector3(0f, _main.CAVEBottom.transform.localScale.x * 10f, 0f),
                    normal = new Vector3(0f, 1f, 0f),
                    Transform = _main.CAVEBottom,
                    TransformXXL = _main.CAVEBottomXXL,
                    up = new Vector3(1, 0, 0),
                    corners = new corners
                    {
                        topleft = new Vector3(5f, 0f,5f),
                        bottomleft = new Vector3(-5f, 0f, 5f),
                        bottomright = new Vector3(-5f, 0f, -5f)
                    }
                }
            });

            // copy settings from main camera
            foreach (var vi in _viewInfo.Values)
            {
                vi.Left.cam.CopyFrom(Camera.main);
                vi.Right.cam.CopyFrom(Camera.main);

                // initially deactivate all cams
                vi.Left.cam.enabled = false;
                vi.Left.cam.depth = 1;
                vi.Left.cam.fieldOfView = 90;

                vi.Right.cam.enabled = false;
                vi.Right.cam.depth = 1;
                vi.Right.cam.fieldOfView = 90;


                vi.Left.cam.transform.localPosition = vi.Left.offset;
                vi.Right.cam.transform.localPosition = vi.Right.offset;
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



            //_cameraLeftLeft.transform.localPosition = new Vector3(-(_main.EyeDistance / 2), 0f, 0f);
            //_cameraLeftRight.transform.position.Set(_main.EyeDistance / 2, 0f, 0f);
            //_cameraFrontLeft.transform.position.Set(-(_main.EyeDistance / 2), 0f, 0f);
            //_cameraFrontRight.transform.position.Set(_main.EyeDistance / 2, 0f, 0f);

            //_cameraRightLeft.transform.position.Set(0f, 0f, -(_main.EyeDistance / 2));
            //_cameraRightRight.transform.position.Set(0f, 0f, -(_main.EyeDistance / 2));

            //_cameraBottomLeft.transform.position.Set(-(_main.EyeDistance / 2), 0f, 0f);
            //_cameraBottomRight.transform.position.Set(_main.EyeDistance / 2, 0f, 0f);

            if (_main.myCAVEMode == CAVEMode.FourScreen)
            {
                _cameraLeftLeft.rect = new Rect(new Vector2(0f, 0.5f), new Vector2(0.5f, 0.5f));
                _cameraFrontLeft.rect = new Rect(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f));
                _cameraRightLeft.rect = new Rect(new Vector2(0f, 0f), new Vector2(0.5f, 0.5f));
                _cameraBottomLeft.rect = new Rect(new Vector2(0.5f, 0f), new Vector2(0.5f, 0.5f));

                foreach (var vi in _viewInfo.Values)
                {
                    vi.Left.cam.enabled = true;
                    vi.Right.cam.enabled = false;
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
                    vi.Left.cam.enabled = true;
                    vi.Right.cam.enabled = true;
                }
            };
        }

        void Update()
        {
            if (_main == null) return;

            int camIndexX, camIndexY;
            camIndexX = camIndexY = 0;


#if UNITY_EDITOR
            int divisorX = 2;
            int divisorY = 2;
            if (_main.myCAVEMode == CAVEMode.FourScreenStereo) { divisorX = 4; }

            camIndexX = Convert.ToInt32(Mathf.Floor(Convert.ToInt32(Input.mousePosition.x) / Convert.ToInt32(API.GetMainGameViewSize()[0] / divisorX)));
            camIndexY = Convert.ToInt32(Mathf.Floor(Convert.ToInt32(Input.mousePosition.y) / Convert.ToInt32(API.GetMainGameViewSize()[1] / divisorY)));
            
#else
            camIndexX = Math.Max(Convert.ToInt32(Mathf.Floor(Convert.ToInt32(Input.mousePosition.x) / _main.BeamerResolutionWidth)), 0);
            camIndexY = Math.Max(Convert.ToInt32(Mathf.Floor(Convert.ToInt32(Input.mousePosition.y) / _main.BeamerResolutionHeight)), 0);
#endif

            // Get Camera based on cursor position
            
 
            switch ((camIndexX*10)+camIndexY)
            {
                case 0:
                    _cameraWithCursor = _viewInfo[2].Left.cam;
                    break;
                case 1:
                    _cameraWithCursor = _viewInfo[0].Left.cam;
                    break;
                case 10:
                    if(_main.myCAVEMode == CAVEMode.FourScreen) { _cameraWithCursor = _viewInfo[3].Left.cam; }
                    else { _cameraWithCursor = _viewInfo[2].Right.cam; }
                    break;
                case 11:
                    if (_main.myCAVEMode == CAVEMode.FourScreen) { _cameraWithCursor = _viewInfo[1].Left.cam; }
                    else { _cameraWithCursor = _viewInfo[0].Right.cam; }
                    break;
                case 20:
                     _cameraWithCursor = _viewInfo[3].Left.cam; 
                    break;
                case 21:
                    _cameraWithCursor = _viewInfo[1].Left.cam;
                    break;
                case 30:
                    _cameraWithCursor = _viewInfo[3].Right.cam;
                    break;
                case 31:
                    _cameraWithCursor = _viewInfo[1].Right.cam;
                    break;
            }

            //Debug.Log(string.Format("window pos x:{0}, window pos y:{1}, window w:{2}, window h:{3}, mouse x:{4}, mouse y:{5} , {6} {7} {8}",
            //API.GetEditorWindow().position.x,
            //API.GetEditorWindow().position.y,
            //API.GetMainGameViewSize()[0],
            //API.GetMainGameViewSize()[1],
            //Input.mousePosition.x,
            //Input.mousePosition.y, camIndexX, camIndexY, _cameraWithCursor.name
            //));


            // Rotate CAVE according to Maincamera
            _main.transform.rotation = Camera.main.transform.rotation;
        }
    }
}