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

        // all cameras for one eye&cave side grouped
        public struct CameraInfo
        {
            public Camera Cam;
            public Camera CamGUI;
            public Camera CamCursor;
            public Vector3 Offset;
        }

        public struct ViewInfo
        {
            public CameraInfo Left;
            public CameraInfo Right;
            public CAVEPlanesettings CAVESide;
        }

        public struct CAVEPlanesettings
        {
            public BasicSettings.Sides Name; //name
            public float Width; //dimensions
            public float Height; //dimensions
            public Vector3 Center; //center of the plane
            public Vector3 Normal; //normal of the plane
            public Transform Transform; //wall regular cave
            public Transform TransformXXL; //wall imaginary xxl cave
            public Vector3 Up; //up plane
            public Corners Corners; //corners of the plane
        }

        public struct Corners
        {
            public Vector3 Topleft;
            public Vector3 Bottomleft;
            public Vector3 Bottomright;
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
        private Canvas _canvasMouseCursorDuplicate;
        private RectTransform _canvasMouseCursorDuplicateRectTransform;

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

            _canvasMouseCursorDuplicate = GameObject.Find("CanvasMouseCursorDuplicate").GetComponent<Canvas>();
            _canvasMouseCursorDuplicateRectTransform = _canvasMouseCursorDuplicate.GetComponent<RectTransform>();

            FetchCameras();
            CreateSettings();
            AdjustCameras();
            AdjustSecondaryCameras();
            PlaceUIElements();

            // After configuring alle cameras, don't render anything with obsolete Main-Camera in order to gain better performance
            Camera.main.cullingMask = 0;
            Camera.main.clearFlags = CameraClearFlags.Color;
            Camera.main.farClipPlane = 1.01f;
            Camera.main.nearClipPlane = 1f;
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
            camIndexX = Math.Max(Convert.ToInt32(Mathf.Floor(Convert.ToInt32(Input.mousePosition.x) / API.Instance.Cave.CaveSettings.BeamerResolutionWidth)), 0);
            camIndexY = Math.Max(Convert.ToInt32(Mathf.Floor(Convert.ToInt32(Input.mousePosition.y) / API.Instance.Cave.CaveSettings.BeamerResolutionHeight)), 0);
#endif



            // Get Camera based on cursor position
            switch ((camIndexX*10)+camIndexY)
            {
                case 0:
                    _cameraWithCursor = _viewInfo[2].Left.Cam;
                    break;
                case 1:
                    _cameraWithCursor = _viewInfo[0].Left.Cam;
                    break;
                case 10:
                    //if(API.Instance.Cave.CAVEMode == CAVEMode.FourScreen) { _cameraWithCursor = _viewInfo[3].Left.cam; }
                    //else { _cameraWithCursor = _viewInfo[2].Right.cam; }
                    _cameraWithCursor = _viewInfo[2].Right.Cam;
                    break;
                case 11:
                    //if (API.Instance.Cave.CAVEMode == CAVEMode.FourScreen) { _cameraWithCursor = _viewInfo[1].Left.cam; }
                    //else { _cameraWithCursor = _viewInfo[0].Right.cam; }
                    _cameraWithCursor = _viewInfo[0].Right.Cam;
                    break;
                case 20:
                     _cameraWithCursor = _viewInfo[3].Left.Cam; 
                    break;
                case 21:
                    _cameraWithCursor = _viewInfo[1].Left.Cam;
                    break;
                case 30:
                    _cameraWithCursor = _viewInfo[3].Right.Cam;
                    break;
                case 31:
                    _cameraWithCursor = _viewInfo[1].Right.Cam;
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
            //API.Instance.Cave.transform.rotation = Camera.main.transform.rotation;
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
                    Cam = _cameraLeftLeft,
                    CamGUI = API.Instance.Cave.CaveSettings.GUILocation == BasicSettings.Sides.Left ? Instantiate(_cameraLeftLeft) : null,
                    CamCursor = Instantiate(_cameraLeftLeft),
                    Offset = new Vector3(0f, 0f, -(API.Instance.Cave.CaveSettings.EyeDistance / 2))
                },
                Right = new CameraInfo
                {
                    Cam = _cameraLeftRight,
                    CamGUI = API.Instance.Cave.CaveSettings.GUILocation == BasicSettings.Sides.Left ? Instantiate(_cameraLeftRight) : null,
                    CamCursor = Instantiate(_cameraLeftRight),
                    Offset = new Vector3(0f, 0f, +(API.Instance.Cave.CaveSettings.EyeDistance / 2))
                },
                CAVESide = new CAVEPlanesettings
                {
                    Name = BasicSettings.Sides.Left,
                    Width = API.Instance.Cave.CAVELeft.transform.localScale.z * 10f,
                    Height = API.Instance.Cave.CAVELeft.transform.localScale.x * 10f,
                    Center = new Vector3(API.Instance.Cave.CAVELeft.transform.localScale.x * 10f / 2, 0f, 0f),
                    Normal = new Vector3(-1f, 0f, 0f),
                    Transform = API.Instance.Cave.CAVELeft,
                    TransformXXL = API.Instance.Cave.CAVELeftXXL,
                    Up = new Vector3(0, 1, 0),
                    Corners = new Corners
                    {
                        Topleft = new Vector3(5f, 0f, 5f),
                        Bottomleft = new Vector3(-5f, 0f, 5f),
                        Bottomright = new Vector3(-5f, 0f, -5f)
                    }
                }
            });

            // Add Settings Front
            _viewInfo.Add(1, new ViewInfo
            {
                Left = new CameraInfo
                {
                    Cam = _cameraFrontLeft,
                    CamGUI = API.Instance.Cave.CaveSettings.GUILocation == BasicSettings.Sides.Front ? Instantiate(_cameraFrontLeft) : null,
                    CamCursor = Instantiate(_cameraFrontLeft),
                    Offset = new Vector3(-(API.Instance.Cave.CaveSettings.EyeDistance / 2), 0f, 0f)
                },
                Right = new CameraInfo
                {
                    Cam = _cameraFrontRight,
                    CamGUI = API.Instance.Cave.CaveSettings.GUILocation == BasicSettings.Sides.Front ? Instantiate(_cameraFrontRight) : null,
                    CamCursor = Instantiate(_cameraFrontRight),
                    Offset = new Vector3(+(API.Instance.Cave.CaveSettings.EyeDistance / 2), 0f, 0f)
                },
                CAVESide = new CAVEPlanesettings
                {
                    Name = BasicSettings.Sides.Front,
                    Width = API.Instance.Cave.CAVEFront.transform.localScale.z * 10f,
                    Height = API.Instance.Cave.CAVEFront.transform.localScale.x * 10f,
                    Center = new Vector3(0f, 0f, API.Instance.Cave.CAVEFront.transform.localScale.z * 10f / 2),
                    Normal = new Vector3(0f, 0f, 1f),
                    Transform = API.Instance.Cave.CAVEFront,
                    TransformXXL = API.Instance.Cave.CAVEFrontXXL,
                    Up = new Vector3(0, 1, 0),
                    Corners = new Corners
                    {
                        Topleft = new Vector3(5f, 0f, 5f),
                        Bottomleft = new Vector3(-5f, 0f, 5f),
                        Bottomright = new Vector3(-5f, 0f, -5f)
                    }
                }
            });

            // Add Settings Right
            _viewInfo.Add(2, new ViewInfo
            {
                Left = new CameraInfo
                {
                    Cam = _cameraRightLeft,
                    CamGUI = API.Instance.Cave.CaveSettings.GUILocation == BasicSettings.Sides.Right ? Instantiate(_cameraRightLeft) : null,
                    CamCursor = Instantiate(_cameraRightLeft),
                    Offset = new Vector3(0f, 0f, +(API.Instance.Cave.CaveSettings.EyeDistance / 2))
                },
                Right = new CameraInfo
                {
                    Cam = _cameraRightRight,
                    CamGUI = API.Instance.Cave.CaveSettings.GUILocation == BasicSettings.Sides.Right ? Instantiate(_cameraRightRight) : null,
                    CamCursor = Instantiate(_cameraRightRight),
                    Offset = new Vector3(0f, 0f, -(API.Instance.Cave.CaveSettings.EyeDistance / 2))
                },
                CAVESide = new CAVEPlanesettings
                {
                    Name = BasicSettings.Sides.Right,
                    Width = API.Instance.Cave.CAVERight.transform.localScale.z * 10f,
                    Height = API.Instance.Cave.CAVERight.transform.localScale.x * 10f,
                    Center = new Vector3(API.Instance.Cave.CAVERight.transform.localScale.x * 10f / 2, 0f, 0f),
                    Normal = new Vector3(1f, 0f, 0f),
                    Transform = API.Instance.Cave.CAVERight,
                    TransformXXL = API.Instance.Cave.CAVERightXXL,
                    Up = new Vector3(0, 1, 0),
                    Corners = new Corners
                    {
                        Topleft = new Vector3(5f, 0f, 5f),
                        Bottomleft = new Vector3(-5f, 0f, 5f),
                        Bottomright = new Vector3(-5f, 0f, -5f)
                    }

                }
            });

            // Add Settings Bottom
            _viewInfo.Add(3, new ViewInfo
            {
                Left = new CameraInfo
                {
                    Cam = _cameraBottomLeft,
                    CamGUI = API.Instance.Cave.CaveSettings.GUILocation == BasicSettings.Sides.Bottom ? Instantiate(_cameraBottomLeft) : null,
                    CamCursor = Instantiate(_cameraBottomLeft),
                    Offset = new Vector3(-(API.Instance.Cave.CaveSettings.EyeDistance / 2), 0f, 0f)
                },
                Right = new CameraInfo
                {
                    Cam = _cameraBottomRight,
                    CamGUI = API.Instance.Cave.CaveSettings.GUILocation == BasicSettings.Sides.Bottom ? Instantiate(_cameraBottomRight) : null,
                    CamCursor = Instantiate(_cameraBottomRight),
                    Offset = new Vector3(+(API.Instance.Cave.CaveSettings.EyeDistance / 2), 0f, 0f)
                },
                CAVESide = new CAVEPlanesettings
                {
                    Name = BasicSettings.Sides.Bottom,
                    Width = API.Instance.Cave.CAVEBottom.transform.localScale.z * 10f,
                    Height = API.Instance.Cave.CAVEBottom.transform.localScale.x * 10f,
                    Center = new Vector3(0f, API.Instance.Cave.CAVEBottom.transform.localScale.x * 10f, 0f),
                    Normal = new Vector3(0f, 1f, 0f),
                    Transform = API.Instance.Cave.CAVEBottom,
                    TransformXXL = API.Instance.Cave.CAVEBottomXXL,
                    Up = new Vector3(1, 0, 0),
                    Corners = new Corners
                    {
                        Topleft = new Vector3(5f, 0f, 5f),
                        Bottomleft = new Vector3(-5f, 0f, 5f),
                        Bottomright = new Vector3(-5f, 0f, -5f)
                    }
                }
            });
        }
        
        private void AdjustCameras()
        {
            // copy settings from main camera
            foreach (var vi in _viewInfo.Values)
            {
                vi.Left.Cam.CopyFrom(Camera.main);
                vi.Right.Cam.CopyFrom(Camera.main);

                // initially deactivate all cams
                vi.Left.Cam.enabled = false;
                vi.Left.Cam.depth = (int)CameraDepths.Main;
                vi.Left.Cam.fieldOfView = 90;

                vi.Right.Cam.enabled = false;
                vi.Right.Cam.depth = (int)CameraDepths.Main;
                vi.Right.Cam.fieldOfView = 90;

                vi.Left.Cam.transform.localPosition = vi.Left.Offset;
                vi.Right.Cam.transform.localPosition = vi.Right.Offset;
            }

            Camera.main.depth = (int)CameraDepths.Disabled;

            _cameraLeftLeft.transform.localRotation = Quaternion.Euler(new Vector3(0f, 270f, 0f));
            _cameraLeftLeft.transform.localRotation = Quaternion.Euler(new Vector3(0f, 270f, 0f));

            _cameraFrontLeft.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0, 0f));
            _cameraFrontRight.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0, 0f));

            _cameraRightLeft.transform.localRotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
            _cameraRightRight.transform.localRotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));

            _cameraBottomLeft.transform.localRotation = Quaternion.Euler(new Vector3(90f, 0f, 0f));
            _cameraBottomRight.transform.localRotation = Quaternion.Euler(new Vector3(90f, 0f, 0f));

            //_cameraLeftLeft.transform.Rotate(new Vector3(0f, 270f, 0f));
            //_cameraLeftRight.transform.Rotate(new Vector3(0f, 270f, 0f));

            //_cameraRightLeft.transform.Rotate(new Vector3(0f, 90f, 0f));
            //_cameraRightRight.transform.Rotate(new Vector3(0f, 90f, 0f));

            //_cameraBottomLeft.transform.Rotate(new Vector3(90f, 0f, 0f));
            //_cameraBottomRight.transform.Rotate(new Vector3(90f, 0f, 0f));




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
                vi.Left.Cam.enabled = true;
                vi.Right.Cam.enabled = true;

                // adjust cursor cams
                ConfigureCamCursor(vi.Left.CamCursor, vi.Left.Cam, vi.CAVESide.Name, true);
                ConfigureCamCursor(vi.Right.CamCursor, vi.Right.Cam, vi.CAVESide.Name, false);
            }
            //};
        }

        public void AdjustSecondaryCameras()
        {
            foreach(var c in API.Instance.Cave.SecondaryCameraSettings)
            {
                if (c.Camera == null) break;

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
            if (API.Instance.Cave.CaveSettings.GUILocation == BasicSettings.Sides.None) return;

            Transform caveSide;
            Vector2 rot = Vector2.zero;

            int settingsIndex = 0;

            switch (API.Instance.Cave.CaveSettings.GUILocation)
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
            ConfigureCamGUI(FullViewInfo[settingsIndex].Left.CamGUI, FullViewInfo[settingsIndex].Left.Cam);
            ConfigureCamGUI(FullViewInfo[settingsIndex].Right.CamGUI, FullViewInfo[settingsIndex].Right.Cam, false);

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
                    c.worldCamera = FullViewInfo[settingsIndex].Left.Cam;
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
            camGUI.orthographic = true;
            camGUI.farClipPlane = 10f;
            camGUI.nearClipPlane = 5f;
        }

        private void ConfigureCamCursor(Camera camCursor, Camera cam, BasicSettings.Sides caveSide, bool isEyeLeft = true)
        {
            camCursor.CopyFrom(cam);
            camCursor.cullingMask = LayerMask.GetMask("UI");

            camCursor.name = "CameraCursor" + caveSide.ToString() + (isEyeLeft ? "Left" : "Right");
            camCursor.clearFlags = CameraClearFlags.Depth;
            camCursor.transform.SetParent(cam.transform.parent);
            camCursor.depth = (int)CameraDepths.Cursor;
            camCursor.enabled = false;
            camCursor.orthographic = true;
            camCursor.farClipPlane = 10f;
            camCursor.nearClipPlane = 5f;
        }

        public void AdjustCamCursor(BasicSettings.Sides caveSide)
        {
            if(_wandCaveIntersection != caveSide)
            {
                _wandCaveIntersection = caveSide;

                //Debug.Log("AdjustCamCursor, new side: " + caveSide);

                // Deactivate all Cursor Cams
                foreach (var fvi in FullViewInfo)
                {
                    fvi.Value.Left.CamCursor.enabled = false;
                    fvi.Value.Right.CamCursor.enabled = false;
                }

                // Get Cam
                var camCursorLeft = FullViewInfo.FirstOrDefault(x => x.Value.CAVESide.Name == caveSide).Value.Left.CamCursor;
                var camCursorRight = FullViewInfo.FirstOrDefault(x => x.Value.CAVESide.Name == caveSide).Value.Right.CamCursor;

                if (camCursorLeft != null && API.Instance.Cave.CaveSettings.GUILocation != caveSide)
                {
                    camCursorLeft.enabled = camCursorRight.enabled = true;
                }

                // Adjust MouseCanvas
                AdjustMouseCursorCanvas(caveSide);
            }
        }

        private void AdjustMouseCursorCanvas(BasicSettings.Sides side)
        {
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
            
            _canvasMouseCursorDuplicate.renderMode = RenderMode.ScreenSpaceOverlay; // To avoid bug
            _canvasMouseCursorDuplicate.renderMode = RenderMode.WorldSpace;
            _canvasMouseCursorDuplicateRectTransform.sizeDelta = new Vector2(caveSide.transform.localScale.z * 1000f, caveSide.transform.localScale.x * 1000f);
            _canvasMouseCursorDuplicateRectTransform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            _canvasMouseCursorDuplicateRectTransform.SetParent(API.Instance.Cave.transform);
            _canvasMouseCursorDuplicateRectTransform.localPosition = caveSide.transform.localPosition;
            _canvasMouseCursorDuplicateRectTransform.eulerAngles = new Vector3(rot.x, rot.y, 0f);
        }
    }
}