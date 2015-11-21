using UnityEngine;
using System.Collections;

namespace Cave
{
    public class Eyes : BaseBehaviour
    {
        // Use this for initialization
        void Start()
        { }

        // Update is called once per frame
        void Update()
        {
            HandlePosition();
            HandleRotation();
        }

        private void HandlePosition()
        {
            if (_main.EyesSettings.TrackPosition)
            {
                // Position
                var posOri = transform.position;
                var pos = VRPN.vrpnTrackerPos(_main.EyesSettings.WorldVizObject + "@" + _main.Host, _main.EyesSettings.Channel);

                // Block Axis
                if (_main.EyesSettings.PositionAxisConstraints.X) pos.x = posOri.x;
                if (_main.EyesSettings.PositionAxisConstraints.Y) pos.x = posOri.z;
                if (_main.EyesSettings.PositionAxisConstraints.Z) pos.x = posOri.z;

                //Camera.main.transform.position = pos;
                //_main.CameraContainer.transform.position = pos;
                //_main.CameraContainer.transform.localPosition = pos;
                transform.position = pos;
            }
        }

        private void HandleRotation()
        {
            if (_main.EyesSettings.TrackRotation)
            {
                var rot = VRPN.vrpnTrackerQuat(_main.EyesSettings.WorldVizObject + "@" + _main.Host, _main.EyesSettings.Channel);
                transform.rotation = rot;
            }
        }
    }
}