using UnityEngine;
using System.Collections;
using System.Drawing;
using System;
using UnityStandardAssets.CrossPlatformInput;

namespace Cave
{
    public class Wand : MonoBehaviour
    {
        public Vector2 JoystickPosition { get { return _joystickPos; } }
        public bool TopLeft { get { return _topLeft; } }
        public bool TopRight { get { return _topRight; } }
        public bool BottomLeft { get { return _bottomLeft; } }
        public bool BottomRight { get { return _bottomRight; } }
        public bool JoystickPress { get { return _joystickPress; } }
        public bool ButtonBack { get { return _buttonBack; } }

        public delegate void DelegateJoystickAnalog(float xAxis, float yAxis);
        public event DelegateJoystickAnalog OnJoystickAnalogUpdate;

        private bool _usePositionSmoothing;
        private bool _useRotationSmoothing;
        private float _rotJitterReduction;
        private float _rotLagReduction;
        private float _posJitterReduction;
        private float _posLagReduction;

        private Vector2 _joystickPos;
        private bool _topLeft;
        private bool _topRight;
        private bool _bottomLeft;
        private bool _bottomRight;
        private bool _joystickPress;
        private bool _buttonBack;

        private RectTransform _caveCursorRect;
        private UnityEngine.UI.Image _caveCursorImage;

        //private CrossPlatformInputManager.VirtualAxis _hVirtualAxis;
        //private CrossPlatformInputManager.VirtualAxis _vVirtualAxis;

        [Range(-1f, 1f)]
        public float JoystickAnalogDebugX;

        [Range(-1f, 1f)]
        public float JoystickAnalogDebugY;

        // Use this for initialization
        void Start()
        {
            _usePositionSmoothing = API.Instance.Cave.WandSettings.PositionMovementConstraints.useOneEuroSmoothing;
            _useRotationSmoothing = API.Instance.Cave.WandSettings.RotationMovementConstraints.useOneEuroSmoothing;

            _rotJitterReduction = API.Instance.Cave.WandSettings.RotationMovementConstraints.jitterReduction;
            _rotLagReduction = API.Instance.Cave.WandSettings.RotationMovementConstraints.lagReduction;

            _posJitterReduction = API.Instance.Cave.WandSettings.PositionMovementConstraints.jitterReduction;
            _posLagReduction = API.Instance.Cave.WandSettings.PositionMovementConstraints.lagReduction;

            _joystickPos = Vector2.zero;

            SetCustomCursor();

            //if (!CrossPlatformInputManager.AxisExists("Horizontal"))
            //{
            //    _hVirtualAxis = new CrossPlatformInputManager.VirtualAxis("Horizontal",true);
            //    CrossPlatformInputManager.RegisterVirtualAxis(_hVirtualAxis);
            //}
            //else
            //{
            //    _hVirtualAxis = CrossPlatformInputManager.VirtualAxisReference("Horizontal");
            //}

            //if (!CrossPlatformInputManager.AxisExists("Vertical"))
            //{

            //    _vVirtualAxis = new CrossPlatformInputManager.VirtualAxis("Vertical",true);
            //    CrossPlatformInputManager.RegisterVirtualAxis(_vVirtualAxis);
            //}
            //else
            //{
            //    _vVirtualAxis = CrossPlatformInputManager.VirtualAxisReference("Vertical");
            //}
        }

        // Update is called once per frame
        void Update()
        {
            HandlePosition();
            HandleRotation();
            HandleButtons();
            HandleJoystick();
            SetCursor();
        }

        private void SetCustomCursor()
        {
            _caveCursorRect = GameObject.FindWithTag("CaveCursor").GetComponent<RectTransform>();
            _caveCursorImage = GameObject.FindWithTag("CaveCursor").GetComponent< UnityEngine.UI.Image >();

            if(API.Instance.Cave.WandSettings.Cursor != null)
            {
                Canvas canvasMouseCursorDuplicate = GameObject.Find("CanvasMouseCursorDuplicate").GetComponent<Canvas>();
                Rect rect = new Rect(0, 0, API.Instance.Cave.WandSettings.Cursor.width, API.Instance.Cave.WandSettings.Cursor.height);
                
                Sprite cursorSprite = Sprite.Create(API.Instance.Cave.WandSettings.Cursor, rect, Vector2.zero);

                _caveCursorImage.sprite = cursorSprite;
                _caveCursorRect.pivot = new Vector2(0.5f, 0.5f);
                _caveCursorRect.sizeDelta = new Vector2(API.Instance.Cave.WandSettings.Cursor.width, API.Instance.Cave.WandSettings.Cursor.height);

                Cursor.SetCursor(API.Instance.Cave.WandSettings.Cursor, Vector2.zero, CursorMode.Auto);
            }
        }

        private void HandlePosition()
        {
            if (API.Instance.Cave.WandSettings.TrackPosition)
            {
                // Position
                var posOri = transform.localPosition;
                var pos = VRPN.vrpnTrackerPos(API.Instance.Cave.WandSettings.WorldVizObject + "@" + API.Instance.Cave.Host, API.Instance.Cave.WandSettings.Channel);

                //Debug.Log("Wand pos from VRPN: " + pos);

                if (_usePositionSmoothing)
                {
                    Vector3 filteredPos = Vector3.zero;
                    Vector3 filteredVelocity = Vector3.zero;
                    OneEuroFilter.ApplyOneEuroFilter(pos, Vector3.zero, posOri, Vector3.zero, ref filteredPos, ref filteredVelocity, _posJitterReduction, _posLagReduction);
                    pos = filteredPos;
                }

                // Block Axis
                if (API.Instance.Cave.WandSettings.PositionAxisConstraints.X) pos.x = posOri.x;
                if (API.Instance.Cave.WandSettings.PositionAxisConstraints.Y) pos.x = posOri.z;
                if (API.Instance.Cave.WandSettings.PositionAxisConstraints.Z) pos.x = posOri.z;


                transform.localPosition = pos;
            }
        }

        private void HandleRotation()
        {
            if (API.Instance.Cave.WandSettings.TrackRotation)
            {
                var rot = VRPN.vrpnTrackerQuat(API.Instance.Cave.WandSettings.WorldVizObject + "@" + API.Instance.Cave.Host, API.Instance.Cave.WandSettings.Channel);
                Vector3 rotOri = transform.rotation.eulerAngles;

                if (_useRotationSmoothing)
                {
                    Vector3 filteredRot = Vector3.zero;
                    Vector3 filteredVelocity = Vector3.zero;
                    OneEuroFilter.ApplyOneEuroFilter(rot.eulerAngles, Vector3.zero, rotOri, Vector3.zero, ref filteredRot, ref filteredVelocity, _rotJitterReduction, _rotLagReduction);
                    rot.eulerAngles = filteredRot;
                }
                transform.rotation = rot;
            }
        }

        private void HandleJoystick()
        {
            //Debug.Log("Joystick X: " + VRPN.vrpnAnalog(API.Instance.Cave.WandSettings.WorldVizObjectButtons + "@" + API.Instance.Cave.Host + ":" + API.Instance.Cave.WandSettings.Port, 0));
            //Debug.Log("Joystick Y: " + VRPN.vrpnAnalog(API.Instance.Cave.WandSettings.WorldVizObjectButtons + "@" + API.Instance.Cave.Host + ":" + API.Instance.Cave.WandSettings.Port, 1));

            Vector2 vCurr = new Vector2();
            vCurr.x = (float)VRPN.vrpnAnalog(API.Instance.Cave.WandSettings.WorldVizObjectButtons + "@" + API.Instance.Cave.Host + ":" + API.Instance.Cave.WandSettings.Port, 0);
            vCurr.y = (float)VRPN.vrpnAnalog(API.Instance.Cave.WandSettings.WorldVizObjectButtons + "@" + API.Instance.Cave.Host + ":" + API.Instance.Cave.WandSettings.Port, 1);

            if(JoystickAnalogDebugX != 0)
            {
                vCurr.x = JoystickAnalogDebugX;
            }

            if (JoystickAnalogDebugY != 0)
            {
                vCurr.y = JoystickAnalogDebugY;
            }

            if (OnJoystickAnalogUpdate != null)
            {
                OnJoystickAnalogUpdate(vCurr.x, vCurr.y);
            }
            
            //_joystickPos = vCurr;
        }

        private void HandleButtons()
        {
            // NOTE: Convert string to keycode: KeyCode thisKeyCode = (KeyCode) System.Enum.Parse(typeof(KeyCode), "Whatever") ;
            // http://inputsimulator.codeplex.com/SourceControl/latest#WindowsInput/Native/VirtualKeyCode.cs
            
            _topLeft = VRPN.vrpnButton(API.Instance.Cave.WandSettings.WorldVizObjectButtons + "@" + API.Instance.Cave.Host + ":" + API.Instance.Cave.WandSettings.Port, 1);
            _topRight = VRPN.vrpnButton(API.Instance.Cave.WandSettings.WorldVizObjectButtons + "@" + API.Instance.Cave.Host + ":" + API.Instance.Cave.WandSettings.Port, 2);
            _bottomLeft = VRPN.vrpnButton(API.Instance.Cave.WandSettings.WorldVizObjectButtons + "@" + API.Instance.Cave.Host + ":" + API.Instance.Cave.WandSettings.Port, 0);
            _bottomRight = VRPN.vrpnButton(API.Instance.Cave.WandSettings.WorldVizObjectButtons + "@" + API.Instance.Cave.Host + ":" + API.Instance.Cave.WandSettings.Port, 3);
            _joystickPress = VRPN.vrpnButton(API.Instance.Cave.WandSettings.WorldVizObjectButtons + "@" + API.Instance.Cave.Host + ":" + API.Instance.Cave.WandSettings.Port, 4);
            _buttonBack = VRPN.vrpnButton(API.Instance.Cave.WandSettings.WorldVizObjectButtons + "@" + API.Instance.Cave.Host + ":" + API.Instance.Cave.WandSettings.Port, 5);

            if (_topLeft) WindowsInput.InputSimulator.SimulateKeyPress((WindowsInput.VirtualKeyCode)API.Instance.Cave.WandSettings.ButtonMapping.TopLeft);
            if (_topRight) WindowsInput.InputSimulator.SimulateKeyPress((WindowsInput.VirtualKeyCode)API.Instance.Cave.WandSettings.ButtonMapping.TopRight);
            if (_bottomLeft) WindowsInput.InputSimulator.SimulateKeyPress((WindowsInput.VirtualKeyCode)API.Instance.Cave.WandSettings.ButtonMapping.BottomLeft);
            if (_bottomRight) WindowsInput.InputSimulator.SimulateKeyPress((WindowsInput.VirtualKeyCode)API.Instance.Cave.WandSettings.ButtonMapping.BottomRight);
            if (_joystickPress) WindowsInput.InputSimulator.SimulateKeyPress((WindowsInput.VirtualKeyCode)API.Instance.Cave.WandSettings.ButtonMapping.Joystick);
            if (_buttonBack) WindowsInput.InputSimulator.SimulateKeyPress((WindowsInput.VirtualKeyCode)API.Instance.Cave.WandSettings.ButtonMapping.Back);

            //System.Windows.Forms.SendKeys.Send("a"); // Funzt nicht

            //KeyCode keycode = (KeyCode)System.Enum.Parse(typeof(KeyCode), "S"); // funzt so. aber keycode bringt noch nichts

            //WindowsInput.InputSimulator.SimulateKeyPress(WindowsInput.VirtualKeyCode.SPACE); // Funktioniert als Input, erkennt unity!
            //WindowsInput.InputSimulator.SimulateKeyPress(WindowsInput.VirtualKeyCode.VK_S); // Funktioniert als Input, erkennt unity!
            //WindowsInput.InputSimulator.SimulateTextEntry("s"); // Funktioniert leider nicht als "s" input

            // Test CaveInput
            // WindowsInput.VirtualKeyCode foo = (WindowsInput.VirtualKeyCode)65;
            //WindowsInput.InputSimulator.SimulateKeyPress((WindowsInput.VirtualKeyCode)API.Instance.Cave.WandSettings.ButtonMapping.Back);


            //Debug.Log("Top / Left: " + VRPN.vrpnButton(API.Instance.Cave.WandSettings.WorldVizObjectButtons + "@" + API.Instance.Cave.Host + ":" + API.Instance.Cave.WandSettings.Port, 1));
            //Debug.Log("Top / Right: " + VRPN.vrpnButton(API.Instance.Cave.WandSettings.WorldVizObjectButtons + "@" + API.Instance.Cave.Host + ":" + API.Instance.Cave.WandSettings.Port, 2));
            //Debug.Log("Bottom / Left: " + VRPN.vrpnButton(API.Instance.Cave.WandSettings.WorldVizObjectButtons + "@" + API.Instance.Cave.Host + ":" + API.Instance.Cave.WandSettings.Port, 0));
            //Debug.Log("Bottom / Right: " + VRPN.vrpnButton(API.Instance.Cave.WandSettings.WorldVizObjectButtons + "@" + API.Instance.Cave.Host + ":" + API.Instance.Cave.WandSettings.Port, 3));
            //Debug.Log("Joystick Press: " + VRPN.vrpnButton(API.Instance.Cave.WandSettings.WorldVizObjectButtons + "@" + API.Instance.Cave.Host + ":" + API.Instance.Cave.WandSettings.Port, 4));
            //Debug.Log("Button Back: " + VRPN.vrpnButton(API.Instance.Cave.WandSettings.WorldVizObjectButtons + "@" + API.Instance.Cave.Host + ":" + API.Instance.Cave.WandSettings.Port, 5));

            //Debug.Log("------------------");
        }

        private void SetCursor()
        {
            int _multiplierX = 0;
            int _multiplierY = 0;
            bool _caveHit = true;

            // Raycast
            var fwd = transform.TransformDirection(Vector3.forward);

            // Enable Collider temporarily
            API.Instance.Cave.ToggleColliders(true);

            Ray ray = new Ray(transform.position, fwd);

            // NOTE layers are not exported with assets
            //var mask = LayerMask.GetMask("CAVE");

            var raycast = Physics.RaycastAll(ray, 10f);

            foreach (var h in raycast)
            {
                _caveHit = true;

                //Debug.Log(String.Format("Wand Raycast hits: {0}", hit.collider.name));

                switch (h.transform.tag)
                {
                    case "CaveLeft":
                        _multiplierX = 0;
                        _multiplierY = 0;
                        API.Instance.CameraManager.AdjustCamCursor(BasicSettings.Sides.Left);
                        break;

                    case "CaveFront":
                        _multiplierX = 2;
                        _multiplierY = 0;
                        API.Instance.CameraManager.AdjustCamCursor(BasicSettings.Sides.Front);
                        break;

                    case "CaveRight":
                        _multiplierX = 0;
                        _multiplierY = 1;
                        API.Instance.CameraManager.AdjustCamCursor(BasicSettings.Sides.Right);
                        break;

                    case "CaveBottom":
                        _multiplierX = 2;
                        _multiplierY = 1;
                        API.Instance.CameraManager.AdjustCamCursor(BasicSettings.Sides.Bottom);
                        break;

                    default:
                        _caveHit = false;
                        break;
                }

                if (!_caveHit) continue;

                Vector3 localSpaceHitPoint = h.transform.worldToLocalMatrix.MultiplyPoint(h.point);

                float localHitpointNoramlizedX = 1f - ((localSpaceHitPoint.z + 5f) / 10f);
                float localHitpointNoramlizedY = 1f - ((localSpaceHitPoint.x + 5f) / 10f);

                float posCaveSideX = localHitpointNoramlizedX * API.Instance.Cave.BeamerResolutionWidth;
                float posCaveSideY = localHitpointNoramlizedY * API.Instance.Cave.BeamerResolutionHeight;

                float posCaveX = posCaveSideX + _multiplierX * API.Instance.Cave.BeamerResolutionWidth;
                float posCaveY = posCaveSideY + _multiplierY * API.Instance.Cave.BeamerResolutionHeight;

                float posCaveDuplicateX = posCaveX + API.Instance.Cave.BeamerResolutionWidth;
                float posCaveDuplicateY = -(posCaveY - _caveCursorRect.sizeDelta.y);

                //Debug.Log("posCaveDuplicateX: " + posCaveDuplicateX);
                //Debug.Log("posCaveDuplicateY: " + posCaveDuplicateY);

                //System.Windows.Forms.Cursor.Position = new Point(Convert.ToInt32(posCaveX), Convert.ToInt32(posCaveY));

                // Set Position Duplicate
                if (Cursor.visible)
                {
                    Vector2 v2 = new Vector2(posCaveDuplicateX, posCaveDuplicateY);
                    _caveCursorRect.anchoredPosition = v2;
                }
            }

            API.Instance.Cave.ToggleColliders(false);
        }

        //private void OnDisable()
        //{
        //    _hVirtualAxis.Remove();
        //    _vVirtualAxis.Remove();
        //}
    }
}