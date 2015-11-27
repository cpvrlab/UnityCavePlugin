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


        private CaveMain _main;
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
            

        //private CrossPlatformInputManager.VirtualAxis _hVirtualAxis;
        //private CrossPlatformInputManager.VirtualAxis _vVirtualAxis;

        


        // Use this for initialization
        void Start()
        {
            _main = GameObject.Find("Cave").GetComponent<CaveMain>();
            _usePositionSmoothing = _main.WandSettings.PositionMovementConstraints.useOneEuroSmoothing;
            _useRotationSmoothing = _main.WandSettings.RotationMovementConstraints.useOneEuroSmoothing;

            _rotJitterReduction = _main.WandSettings.RotationMovementConstraints.jitterReduction;
            _rotLagReduction = _main.WandSettings.RotationMovementConstraints.lagReduction;

            _posJitterReduction = _main.WandSettings.PositionMovementConstraints.jitterReduction;
            _posLagReduction = _main.WandSettings.PositionMovementConstraints.lagReduction;

            _joystickPos = Vector2.zero;
            

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

        private void HandlePosition()
        {
            if (_main.WandSettings.TrackPosition)
            {
                // Position
                var posOri = transform.localPosition;
                var pos = VRPN.vrpnTrackerPos(_main.WandSettings.WorldVizObject + "@" + _main.Host, _main.WandSettings.Channel);
                if (_usePositionSmoothing)
                {
                    Vector3 filteredPos = Vector3.zero;
                    Vector3 filteredVelocity = Vector3.zero;
                    OneEuroFilter.ApplyOneEuroFilter(pos, Vector3.zero, posOri, Vector3.zero, ref filteredPos, ref filteredVelocity, _posJitterReduction, _posLagReduction);
                    pos = filteredPos;
                }

                // Block Axis
                if (_main.WandSettings.PositionAxisConstraints.X) pos.x = posOri.x;
                if (_main.WandSettings.PositionAxisConstraints.Y) pos.x = posOri.z;
                if (_main.WandSettings.PositionAxisConstraints.Z) pos.x = posOri.z;

                transform.localPosition = pos;
            }
        }

        private void HandleRotation()
        {
            if (_main.WandSettings.TrackRotation)
            {
                var rot = VRPN.vrpnTrackerQuat(_main.WandSettings.WorldVizObject + "@" + _main.Host, _main.WandSettings.Channel);
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

            //Debug.Log("Joystick X: " + VRPN.vrpnAnalog(_main.WandSettings.WorldVizObjectButtons + "@" + _main.Host + ":" + _main.WandSettings.Port, 0));
            //Debug.Log("Joystick Y: " + VRPN.vrpnAnalog(_main.WandSettings.WorldVizObjectButtons + "@" + _main.Host + ":" + _main.WandSettings.Port, 1));


            Vector2 vCurr = new Vector2();
            vCurr.x = (float)VRPN.vrpnAnalog(_main.WandSettings.WorldVizObjectButtons + "@" + _main.Host + ":" + _main.WandSettings.Port, 0);
            vCurr.y = (float)VRPN.vrpnAnalog(_main.WandSettings.WorldVizObjectButtons + "@" + _main.Host + ":" + _main.WandSettings.Port, 1);

            _joystickPos = vCurr;



        }

        private void HandleButtons()
        {
            //System.Windows.Forms.SendKeys.Send();

            // NOTE: Convert string to keycode: KeyCode thisKeyCode = (KeyCode) System.Enum.Parse(typeof(KeyCode), "Whatever") ;
            // http://inputsimulator.codeplex.com/SourceControl/latest#WindowsInput/Native/VirtualKeyCode.cs

            _topLeft = VRPN.vrpnButton(_main.WandSettings.WorldVizObjectButtons + "@" + _main.Host + ":" + _main.WandSettings.Port, 1);
            _topRight = VRPN.vrpnButton(_main.WandSettings.WorldVizObjectButtons + "@" + _main.Host + ":" + _main.WandSettings.Port, 2);
            _bottomLeft = VRPN.vrpnButton(_main.WandSettings.WorldVizObjectButtons + "@" + _main.Host + ":" + _main.WandSettings.Port, 0);
            _bottomRight = VRPN.vrpnButton(_main.WandSettings.WorldVizObjectButtons + "@" + _main.Host + ":" + _main.WandSettings.Port, 3);
            _joystickPress = VRPN.vrpnButton(_main.WandSettings.WorldVizObjectButtons + "@" + _main.Host + ":" + _main.WandSettings.Port, 4);
            _buttonBack = VRPN.vrpnButton(_main.WandSettings.WorldVizObjectButtons + "@" + _main.Host + ":" + _main.WandSettings.Port, 5);


            //if (_topLeft) Event.KeyboardEvent(_main.WandSettings.ButtonMapping.TopLeft);
            //if (_topRight) Event.KeyboardEvent(_main.WandSettings.ButtonMapping.TopRight);
            //if (_bottomLeft) Event.KeyboardEvent(_main.WandSettings.ButtonMapping.BottomLeft);
            //if (_bottomRight) Event.KeyboardEvent(_main.WandSettings.ButtonMapping.BottomRight);
            //if (_joystickPress) Event.KeyboardEvent(_main.WandSettings.ButtonMapping.Joystick);
            //if (_buttonBack) Event.KeyboardEvent(_main.WandSettings.ButtonMapping.Back);


            WindowsInput.InputSimulator.SimulateKeyPress(WindowsInput.VirtualKeyCode.SPACE);

            Debug.Log("Top / Left: " + VRPN.vrpnButton(_main.WandSettings.WorldVizObjectButtons + "@" + _main.Host + ":" + _main.WandSettings.Port, 1));
            Debug.Log("Top / Right: " + VRPN.vrpnButton(_main.WandSettings.WorldVizObjectButtons + "@" + _main.Host + ":" + _main.WandSettings.Port, 2));
            Debug.Log("Bottom / Left: " + VRPN.vrpnButton(_main.WandSettings.WorldVizObjectButtons + "@" + _main.Host + ":" + _main.WandSettings.Port, 0));
            Debug.Log("Bottom / Right: " + VRPN.vrpnButton(_main.WandSettings.WorldVizObjectButtons + "@" + _main.Host + ":" + _main.WandSettings.Port, 3));
            Debug.Log("Joystick Press: " + VRPN.vrpnButton(_main.WandSettings.WorldVizObjectButtons + "@" + _main.Host + ":" + _main.WandSettings.Port, 4));
            Debug.Log("Button Back: " + VRPN.vrpnButton(_main.WandSettings.WorldVizObjectButtons + "@" + _main.Host + ":" + _main.WandSettings.Port, 5));

            Debug.Log("------------------");

        


        }

        private void SetCursor()
        {
            int _multiplierX = 0;
            int _multiplierY = 0;

            // Raycast
            var fwd = transform.TransformDirection(Vector3.forward);

            // Enable Collider temporarily
            _main.ToggleColliders(true);

            Ray ray = new Ray(transform.position, fwd);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                Vector3 localSpaceHitPoint = hit.transform.worldToLocalMatrix.MultiplyPoint(hit.point);
                Debug.DrawLine(transform.position, hit.point, UnityEngine.Color.cyan);
                //Debug.Log(string.Format(hit.transform.tag + ": {0}, {1}", localSpaceHitPoint.x, localSpaceHitPoint.z));

                switch (hit.transform.tag)
                {
                    case "CaveLeft":
                        _multiplierX = 0;
                        _multiplierY = 0;
                        break;

                    case "CaveFront":
                        _multiplierX = 1;
                        _multiplierY = 0;
                        break;

                    case "CaveRight":
                        _multiplierX = 0;
                        _multiplierY = 1;
                        break;

                    case "CaveBottom":
                        _multiplierX = 1;
                        _multiplierY = 1;
                        break;
                }

                float targetY = ((localSpaceHitPoint.x + 5f) / 10f) * _main.BeamerResolutionHeight + _multiplierY * _main.BeamerResolutionHeight;
                float targetX = ((localSpaceHitPoint.z + 5f) / 10f) * _main.BeamerResolutionWidth + _multiplierX * _main.BeamerResolutionWidth;

                //System.Windows.Forms.Cursor.Position = new Point(Convert.ToInt32(targetX), Convert.ToInt32(targetY));
            }

            _main.ToggleColliders(false);
        }

        //private void OnDisable()
        //{
        //    _hVirtualAxis.Remove();
        //    _vVirtualAxis.Remove();
        //}
    }
}