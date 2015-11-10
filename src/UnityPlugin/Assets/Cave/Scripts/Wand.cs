using UnityEngine;
using System.Collections;
using System.Drawing;
using System;

namespace Cave
{
    public class Wand : MonoBehaviour
    {

        private CaveMain _main;

        // Use this for initialization
        void Start()
        {
            _main = GameObject.Find("Cave").GetComponent<CaveMain>();
        }

        // Update is called once per frame
        void Update()
        {
            HandlePosition();
            HandleRotation();
            //HandleButtons();
            //SetCursor();
        }

        private void HandlePosition()
        {
            if (_main.WandSettings.TrackPosition)
            {
                // Position
                var posOri = transform.position;
                var pos = VRPN.vrpnTrackerPos(_main.WandSettings.WorldVizObject + "@" + _main.Host, _main.WandSettings.Channel);

                // Block Axis
                if (_main.WandSettings.PositionAxisConstraints.X) pos.x = posOri.x;
                if (_main.WandSettings.PositionAxisConstraints.Y) pos.x = posOri.z;
                if (_main.WandSettings.PositionAxisConstraints.Z) pos.x = posOri.z;

                transform.position = pos;
            }
        }

        private void HandleRotation()
        {
            if (_main.WandSettings.TrackRotation)
            {
                var rot = VRPN.vrpnTrackerQuat(_main.WandSettings.WorldVizObject + "@" + _main.Host, _main.WandSettings.Channel);
                transform.rotation = rot;
                if(_main.rotateCave == true) { _main.transform.rotation = rot; }
            }
        }

        private void HandleButtons()
        {
            //System.Windows.Forms.SendKeys.Send();

            // NOTE: Convert string to keycode: KeyCode thisKeyCode = (KeyCode) System.Enum.Parse(typeof(KeyCode), "Whatever") ;

            Debug.Log("Joystick X: " + VRPN.vrpnAnalog(_main.WandSettings.WorldVizObjectButtons + "@" + _main.Host + ":" + _main.WandSettings.Port, 0));
            Debug.Log("Joystick Y: " + VRPN.vrpnAnalog(_main.WandSettings.WorldVizObjectButtons + "@" + _main.Host + ":" + _main.WandSettings.Port, 1));

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

                //Debug.Log("localSpaceHitPoint.x: " + localSpaceHitPoint.x);

                //Debug.Log(string.Format("{0}, {1}", targetX, targetY));

                System.Windows.Forms.Cursor.Position = new Point(Convert.ToInt32(targetX), Convert.ToInt32(targetY));
            }
        }
    }
}