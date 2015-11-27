using UnityEngine;
using System.Collections;

namespace Cave
{
    public class Eyes : MonoBehaviour
    {

        public bool useDebugMover = false;

        private CaveMain _main;
        private bool _usePositionSmoothing;
        private bool _useRotationSmoothing;
        private float _rotJitterReduction;
        private float _rotLagReduction;
        private float _posJitterReduction;
        private float _posLagReduction;

        // Use this for initialization
        void Start()
        {
            _main = GameObject.Find("Cave").GetComponent<CaveMain>();
            _usePositionSmoothing = _main.EyesSettings.PositionMovementConstraints.useOneEuroSmoothing;
            _useRotationSmoothing = _main.EyesSettings.RotationMovementConstraints.useOneEuroSmoothing;

            _rotJitterReduction = _main.EyesSettings.RotationMovementConstraints.jitterReduction;
            _rotLagReduction = _main.EyesSettings.RotationMovementConstraints.lagReduction;

            _posJitterReduction = _main.EyesSettings.PositionMovementConstraints.jitterReduction;
            _posLagReduction = _main.EyesSettings.PositionMovementConstraints.lagReduction;

        }

        // Update is called once per frame
        void Update()
        {
            if (!useDebugMover) {
                HandlePosition();
                HandleRotation();
            }

        }

        private void HandlePosition()
        {
            if (_main.EyesSettings.TrackPosition)
            {
                // Position
                var posOri = transform.localPosition;
                var pos = VRPN.vrpnTrackerPos(_main.EyesSettings.WorldVizObject + "@" + _main.Host, _main.EyesSettings.Channel);
                if (_usePositionSmoothing)
                {
                    Vector3 filteredPos = Vector3.zero;
                    Vector3 filteredVelocity = Vector3.zero;
                    OneEuroFilter.ApplyOneEuroFilter(pos, Vector3.zero, posOri, Vector3.zero, ref filteredPos, ref filteredVelocity, _posJitterReduction, _posLagReduction);
                    pos = filteredPos;
                }

                // Block Axis
                if (_main.EyesSettings.PositionAxisConstraints.X) pos.x = posOri.x;
                if (_main.EyesSettings.PositionAxisConstraints.Y) pos.x = posOri.z;
                if (_main.EyesSettings.PositionAxisConstraints.Z) pos.x = posOri.z;

                //Camera.main.transform.position = pos;
                //_main.CameraContainer.transform.position = pos;
                //_main.CameraContainer.transform.localPosition = pos;
                transform.localPosition = pos;
            }
        }

        private void HandleRotation()
        {
            if (_main.EyesSettings.TrackRotation)
            {
                Vector3 rotOri = transform.rotation.eulerAngles;
                var rot = VRPN.vrpnTrackerQuat(_main.EyesSettings.WorldVizObject + "@" + _main.Host, _main.EyesSettings.Channel);
                
                if (_useRotationSmoothing)
                {
                    Vector3 filteredRot = Vector3.zero;
                    Vector3 filteredVelocity = Vector3.zero;
                    OneEuroFilter.ApplyOneEuroFilter(rot.eulerAngles , Vector3.zero, rotOri, Vector3.zero, ref filteredRot, ref filteredVelocity, _rotJitterReduction, _rotLagReduction);
                    rot.eulerAngles  = filteredRot;
                }

                transform.rotation = rot;
            }
        }
    }
}