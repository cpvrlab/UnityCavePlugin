using UnityEngine;
using System.Collections;
using Cave;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cave
{
    public class CameraManager : MonoBehaviour
    {

#region "structs"

        public struct CameraInfo
        {
            public Camera cam;
            public Camera camGUI;
            public Camera camCursor;
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
            public BasicSettings.Sides name;
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

        public enum CameraDepths
        {
            Disabled = -1,
            Main = 1,
            Secondary = 2,
            GUI = 3,
            Cursor = 4
        }

        #endregion

        #region "public properties"
        public Camera CameraWithCursor { get { return _cameraWithCursor; } }
        public Dictionary<int,ViewInfo> FullViewInfo { get { return _viewInfo; } }

#endregion

#region "private vars"
        
        private Dictionary<int, ViewInfo> _viewInfo;

        private Camera _cameraLeftLeft = null;
        private Camera _cameraLeftRight = null;

        private Camera _cameraFrontLeft = null;
        private Camera _cameraFrontRight = null;

        private Camera _cameraRightLeft = null;
        private Camera _cameraRightRight = null;

        private Camera _cameraBottomLeft = null;
        private Camera _cameraBottomRight = null;

        private Camera _cameraWithCursor;

        private List<Canvas> _rootCanvas;
        private BasicSettings.Sides _wandCaveIntersection = BasicSettings.Sides.None;
        
#endregion
        
        void Awake()
        {
            transform.parent = API.Instance.Cave.gameObject.transform;
        }

        void Start()
        {
            // Disable UI Layer for Maincamera, leave others as-is
            int layerUI = LayerMask.NameToLayer("UI");
            Camera.main.cullingMask = ~(1 << layerUI);
            
            FetchCameras();
            CreateSettings();
            AdjustCameras();
            AdjustSecondaryCameras();
            PlaceUIElements();
        }

        void Update()
        {
            if (API.Instance.Cave == null) return;

            int camIndexX, camIndexY;
            camIndexX = camIndexY = 0;


#if UNITY_EDITOR
            int divisorX = 4;
            int divisorY = 2;
            //if (API.Instance.Cave.CAVEMode == CAVEMode.FourScreenStereo) { divisorX = 4; }

            camIndexX = Convert.ToInt32(Mathf.Floor(Convert.ToInt32(Input.mousePosition.x) / Convert.ToInt32(API.GetMainGameViewSize()[0] / divisorX)));
            camIndexY = Convert.ToInt32(Mathf.Floor(Convert.ToInt32(Input.mousePosition.y) / Convert.ToInt32(API.GetMainGameViewSize()[1] / divisorY)));
            
#else
            camIndexX = Math.Max(Convert.ToInt32(Mathf.Floor(Convert.ToInt32(Input.mousePosition.x) / API.Instance.Cave.BeamerResolutionWidth)), 0);
            camIndexY = Math.Max(Convert.ToInt32(Mathf.Floor(Convert.ToInt32(Input.mousePosition.y) / API.Instance.Cave.BeamerResolutionHeight)), 0);
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
                    //if(API.Instance.Cave.CAVEMode == CAVEMode.FourScreen) { _cameraWithCursor = _viewInfo[3].Left.cam; }
                    //else { _cameraWithCursor = _viewInfo[2].Right.cam; }
                    _cameraWithCursor = _viewInfo[2].Right.cam;
                    break;
                case 11:
                    //if (API.Instance.Cave.CAVEMode == CAVEMode.FourScreen) { _cameraWithCursor = _viewInfo[1].Left.cam; }
                    //else { _cameraWithCursor = _viewInfo[0].Right.cam; }
                    _cameraWithCursor = _viewInfo[0].Right.cam;
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
            API.Instance.Cave.transform.rotation = Camera.main.transform.rotation;
        }
        
        private void FetchCameras()
        {
            _cameraLeftLeft = GameObject.Find("CameraLeftLeft").GetComponent<Camera>();
            _cameraLeftRight = GameObject.Find("CameraLeftRight").GetComponent<Camera>();

            _cameraFrontLeft = GameObject.Find("CameraFrontLeft").GetComponent<Camera>();
            _cameraFrontRight = GameObject.Find("CameraFrontRight").GetComponent<Camera>();

            _cameraRightLeft = GameObject.Find("CameraRightLeft").GetComponent<Camera>();
            _cameraRightRight = GameObject.Find("CameraRightRight").GetComponent<Camera>();

            _cameraBottomLeft = GameObject.Find("CameraBottomLeft").GetComponent<Camera>();
            _cameraBottomRight = GameObject.Find("CameraBottomRight").GetComponent<Camera>();
        }

        private void CreateSettings()
        {
            _viewInfo = new Dictionary<int, ViewInfo>();

            // Add Settings Left
            _viewInfo.Add(0, new ViewInfo
            {
                Left = new CameraInfo
                {
                    cam = _cameraLeftLeft,
                    camGUI = API.Instance.Cave.GUILocation == BasicSettings.Sides.Left ? Instantiate(_cameraLeftLeft) : null,
                    offset = new Vector3(0f, 0f, -(API.Instance.Cave.EyeDistance / 2))
                },
                Right = new CameraInfo
                {
                    cam = _cameraLeftRight,
                    camGUI = API.Instance.Cave.GUILocation == BasicSettings.Sides.Left ? Instantiate(_cameraLeftRight) : null,
                    camCursor = Instantiate(_cameraLeftRight),
                    offset = new Vector3(0f, 0f, +(API.Instance.Cave.EyeDistance / 2))
                },
                CAVESide = new CAVEPlanesettings
                {
                    name = BasicSettings.Sides.Left,
                    width = API.Instance.Cave.CAVELeft.transform.localScale.z * 10f,
                    height = API.Instance.Cave.CAVELeft.transform.localScale.x * 10f,
                    center = new Vector3(API.Instance.Cave.CAVELeft.transform.localScale.x * 10f / 2, 0f, 0f),
                    normal = new Vector3(-1f, 0f, 0f),
                    Transform = API.Instance.Cave.CAVELeft,
                    TransformXXL = API.Instance.Cave.CAVELeftXXL,
                    up = new Vector3(0, 1, 0),
                    corners = new corners
                    {
                        topleft = new Vector3(5f, 0f, 5f),
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
                    camGUI = API.Instance.Cave.GUILocation == BasicSettings.Sides.Front ? Instantiate(_cameraFrontLeft) : null,
                    offset = new Vector3(-(API.Instance.Cave.EyeDistance / 2), 0f, 0f)
                },
                Right = new CameraInfo
                {
                    cam = _cameraFrontRight,
                    camGUI = API.Instance.Cave.GUILocation == BasicSettings.Sides.Front ? Instantiate(_cameraFrontRight) : null,
                    camCursor = Instantiate(_cameraFrontRight),
                    offset = new Vector3(+(API.Instance.Cave.EyeDistance / 2), 0f, 0f)
                },
                CAVESide = new CAVEPlanesettings
                {
                    name = BasicSettings.Sides.Front,
                    width = API.Instance.Cave.CAVEFront.transform.localScale.z * 10f,
                    height = API.Instance.Cave.CAVEFront.transform.localScale.x * 10f,
                    center = new Vector3(0f, 0f, API.Instance.Cave.CAVEFront.transform.localScale.z * 10f / 2),
                    normal = new Vector3(0f, 0f, 1f),
                    Transform = API.Instance.Cave.CAVEFront,
                    TransformXXL = API.Instance.Cave.CAVEFrontXXL,
                    up = new Vector3(0, 1, 0),
                    corners = new corners
                    {
                        topleft = new Vector3(5f, 0f, 5f),
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
                    camGUI = API.Instance.Cave.GUILocation == BasicSettings.Sides.Right ? Instantiate(_cameraRightLeft) : null,
                    offset = new Vector3(0f, 0f, +(API.Instance.Cave.EyeDistance / 2))
                },
                Right = new CameraInfo
                {
                    cam = _cameraRightRight,
                    camGUI = API.Instance.Cave.GUILocation == BasicSettings.Sides.Right ? Instantiate(_cameraRightRight) : null,
                    camCursor = Instantiate(_cameraRightRight),
                    offset = new Vector3(0f, 0f, -(API.Instance.Cave.EyeDistance / 2))
                },
                CAVESide = new CAVEPlanesettings
                {
                    name = BasicSettings.Sides.Right,
                    width = API.Instance.Cave.CAVERight.transform.localScale.z * 10f,
                    height = API.Instance.Cave.CAVERight.transform.localScale.x * 10f,
                    center = new Vector3(API.Instance.Cave.CAVERight.transform.localScale.x * 10f / 2, 0f, 0f),
                    normal = new Vector3(1f, 0f, 0f),
                    Transform = API.Instance.Cave.CAVERight,
                    TransformXXL = API.Instance.Cave.CAVERightXXL,
                    up = new Vector3(0, 1, 0),
                    corners = new corners
                    {
                        topleft = new Vector3(5f, 0f, 5f),
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
                    camGUI = API.Instance.Cave.GUILocation == BasicSettings.Sides.Bottom ? Instantiate(_cameraBottomLeft) : null,
                    offset = new Vector3(-(API.Instance.Cave.EyeDistance / 2), 0f, 0f)
                },
                Right = new CameraInfo
                {
                    cam = _cameraBottomRight,
                    camGUI = API.Instance.Cave.GUILocation == BasicSettings.Sides.Bottom ? Instantiate(_cameraBottomRight) : null,
                    camCursor = Instantiate(_cameraBottomRight),
                    offset = new Vector3(+(API.Instance.Cave.EyeDistance / 2), 0f, 0f)
                },
                CAVESide = new CAVEPlanesettings
                {
                    name = BasicSettings.Sides.Bottom,
                    width = API.Instance.Cave.CAVEBottom.transform.localScale.z * 10f,
                    height = API.Instance.Cave.CAVEBottom.transform.localScale.x * 10f,
                    center = new Vector3(0f, API.Instance.Cave.CAVEBottom.transform.localScale.x * 10f, 0f),
                    normal = new Vector3(0f, 1f, 0f),
                    Transform = API.Instance.Cave.CAVEBottom,
                    TransformXXL = API.Instance.Cave.CAVEBottomXXL,
                    up = new Vector3(1, 0, 0),
                    corners = new corners
                    {
                        topleft = new Vector3(5f, 0f, 5f),
                        bottomleft = new Vector3(-5f, 0f, 5f),
                        bottomright = new Vector3(-5f, 0f, -5f)
                    }
                }
            });
        }
        
        private void AdjustCameras()
        {
            // copy settings from main camera
            foreach (var vi in _viewInfo.Values)
            {
                vi.Left.cam.CopyFrom(Camera.main);
                vi.Right.cam.CopyFrom(Camera.main);

                // initially deactivate all cams
                vi.Left.cam.enabled = false;
                vi.Left.cam.depth = (int)CameraDepths.Main;
                vi.Left.cam.fieldOfView = 90;

                vi.Right.cam.enabled = false;
                vi.Right.cam.depth = (int)CameraDepths.Main;
                vi.Right.cam.fieldOfView = 90;

                vi.Left.cam.transform.localPosition = vi.Left.offset;
                vi.Right.cam.transform.localPosition = vi.Right.offset;
            }

            Camera.main.depth = (int)CameraDepths.Disabled;

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

            //if (API.Instance.Cave.CAVEMode == CAVEMode.FourScreen)
            //{
            //    _cameraLeftLeft.rect = new Rect(new Vector2(0f, 0.5f), new Vector2(0.5f, 0.5f));
            //    _cameraFrontLeft.rect = new Rect(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f));
            //    _cameraRightLeft.rect = new Rect(new Vector2(0f, 0f), new Vector2(0.5f, 0.5f));
            //    _cameraBottomLeft.rect = new Rect(new Vector2(0.5f, 0f), new Vector2(0.5f, 0.5f));

            //    foreach (var vi in _viewInfo.Values)
            //    {
            //        vi.Left.cam.enabled = true;
            //        vi.Right.cam.enabled = false;
            //    }
            //}
            //else if (API.Instance.Cave.CAVEMode == CAVEMode.FourScreenStereo)
            //{

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

                // adjust cursor cams
                ConfigureCamCursor(vi.Right.camCursor, vi.Right.cam, vi.CAVESide.name);
            }
            //};
        }

        public void AdjustSecondaryCameras()
        {
            foreach(var c in API.Instance.Cave.SecondaryCameraSettings)
            {
                c.Camera.depth = (int)CameraDepths.Secondary;

                // dont render ui elements
                int layerUI = LayerMask.NameToLayer("UI");
                c.Camera.cullingMask = ~(1 << layerUI);

                var rect = c.Camera.rect;
                rect.width /= 4;
                rect.height /= 2;

                float posX = rect.x;
                float posY = rect.y;

                float addedX = 0f;
                float addedY = 0f;

                switch (c.Side)
                {
                    case BasicSettings.Sides.Left:
                        addedX = 0f;
                        addedY = 0.5f;
                        break;

                    case BasicSettings.Sides.Front:
                        addedX = 0.5f;
                        addedY = 0.5f;
                        break;

                    case BasicSettings.Sides.Right:
                        addedX = 0f;
                        addedY = 0f;
                        break;

                    case BasicSettings.Sides.Bottom:
                        addedX = 0.5f;
                        addedY = 0f;
                        break;
                }

                posX = addedX + 0.25f * rect.x;
                posY = addedY + 0.5f * rect.y;

                rect.x = posX;
                rect.y = posY;

                c.Camera.rect = rect;

                // Duplicate camera for right eye
                var camDuplicateRight = Instantiate(c.Camera);
                var rectCamRight = rect;
                rectCamRight.x += 0.25f;
                c.Camera.rect = rectCamRight;
                camDuplicateRight.name = c.Camera.name + "Right";
                camDuplicateRight.transform.SetParent(c.Camera.transform.parent);
            }
        }

        public void PlaceUIElements()
        {
            Transform caveSide;
            Vector2 rot = Vector2.zero;

            int settingsIndex = 0;

            switch (API.Instance.Cave.GUILocation)
            {
                case BasicSettings.Sides.Left:
                    caveSide = API.Instance.Cave.CAVELeftXXL;
                    rot.x = 0f;
                    rot.y = 270f;
                    break;

                case BasicSettings.Sides.Right:
                    caveSide = API.Instance.Cave.CAVERightXXL;
                    rot.x = 0f;
                    rot.y = 90;
                    settingsIndex = 2;
                    break;

                case BasicSettings.Sides.Bottom:
                    caveSide = API.Instance.Cave.CAVEBottomXXL;
                    rot.x = 90f;
                    rot.y = 0f;
                    settingsIndex = 3;
                    break;

                default:
                    caveSide = API.Instance.Cave.CAVEFrontXXL;
                    settingsIndex = 1;
                    break;
            }

            // Configure GUI Cameras
            ConfigureCamGUI(FullViewInfo[settingsIndex].Left.camGUI, FullViewInfo[settingsIndex].Left.cam);
            ConfigureCamGUI(FullViewInfo[settingsIndex].Right.camGUI, FullViewInfo[settingsIndex].Right.cam, false);

            // Procedure to adapt Canvas
            // 1) Get all Canvas elements
            // 2) Check if root canvas (isRootCanvas)
            // 3) save all root canvas in list
            // 4) modify
            _rootCanvas = Resources.FindObjectsOfTypeAll<Canvas>().Where(x => x.isRootCanvas).ToList();

            foreach(var c in _rootCanvas)
            {
                if (c.name != "CanvasMouseCursorDuplicate")
                {
                    var canvasRectTransform = c.GetComponent<RectTransform>();

                    c.renderMode = RenderMode.ScreenSpaceOverlay; // To avoid bug
                    c.renderMode = RenderMode.WorldSpace;
                    canvasRectTransform.sizeDelta = new Vector2(caveSide.transform.localScale.z * 1000f, caveSide.transform.localScale.x * 1000f);
                    canvasRectTransform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
                    canvasRectTransform.SetParent(API.Instance.Cave.transform);
                    canvasRectTransform.localPosition = caveSide.transform.localPosition;
                    canvasRectTransform.eulerAngles = new Vector3(rot.x, rot.y, 0f);
                }
            }
        }

        private void ConfigureCamGUI(Camera camGUI, Camera cam, bool isEyeLeft = true)
        {
            camGUI.CopyFrom(cam);
            camGUI.cullingMask = LayerMask.GetMask("UI");

            camGUI.name = isEyeLeft ? "CameraGUILeft" : "CameraGUIRight";
            camGUI.clearFlags = CameraClearFlags.Depth;
            camGUI.transform.SetParent(cam.transform.parent);
            camGUI.depth = (int)CameraDepths.GUI;
        }

        private void ConfigureCamCursor(Camera camCursor, Camera cam, BasicSettings.Sides caveSide)
        {
            camCursor.CopyFrom(cam);
            camCursor.cullingMask = LayerMask.GetMask("UI");

            camCursor.name = "CameraCursor" + caveSide.ToString();
            camCursor.clearFlags = CameraClearFlags.Depth;
            camCursor.transform.SetParent(cam.transform.parent);
            camCursor.depth = (int)CameraDepths.Cursor;
            camCursor.enabled = false;
        }

        public void AdjustCamCursor(BasicSettings.Sides caveSide)
        {
            if(_wandCaveIntersection != caveSide)
            {
                _wandCaveIntersection = caveSide;

                Debug.Log("AdjustCamCursor, new side: " + caveSide);

                // Deactivate all Cursor Cams
                foreach (var fvi in FullViewInfo)
                {
                    fvi.Value.Right.camCursor.enabled = false;
                }

                // Get Cam
                var camCursor = FullViewInfo.FirstOrDefault(x => x.Value.CAVESide.name == caveSide).Value.Right.camCursor;

                if (camCursor != null && API.Instance.Cave.GUILocation != caveSide)
                {
                    camCursor.enabled = true;
                }

                // Adjust MouseCanvas
                AdjustMouseCursorCanvas(caveSide);
            }
        }

        private void AdjustMouseCursorCanvas(BasicSettings.Sides side)
        {
            var c = GameObject.Find("CanvasMouseCursorDuplicate").GetComponent<Canvas>();

            Transform caveSide;
            Vector2 rot = Vector2.zero;

            switch (side)
            {
                case BasicSettings.Sides.Left:
                    caveSide = API.Instance.Cave.CAVELeftXXL;
                    rot.x = 0f;
                    rot.y = 270f;
                    break;

                case BasicSettings.Sides.Right:
                    caveSide = API.Instance.Cave.CAVERightXXL;
                    rot.x = 0f;
                    rot.y = 90;
                    break;

                case BasicSettings.Sides.Bottom:
                    caveSide = API.Instance.Cave.CAVEBottomXXL;
                    rot.x = 90f;
                    rot.y = 0f;
                    break;

                default:
                    caveSide = API.Instance.Cave.CAVEFrontXXL;
                    break;
            }

            var canvasRectTransform = c.GetComponent<RectTransform>();

            c.renderMode = RenderMode.ScreenSpaceOverlay; // To avoid bug
            c.renderMode = RenderMode.WorldSpace;
            canvasRectTransform.sizeDelta = new Vector2(caveSide.transform.localScale.z * 1000f, caveSide.transform.localScale.x * 1000f);
            canvasRectTransform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            canvasRectTransform.SetParent(API.Instance.Cave.transform);
            canvasRectTransform.localPosition = caveSide.transform.localPosition;
            canvasRectTransform.eulerAngles = new Vector3(rot.x, rot.y, 0f);
        }
    }
}