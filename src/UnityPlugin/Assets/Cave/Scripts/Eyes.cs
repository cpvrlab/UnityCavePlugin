using UnityEngine;
using System.Collections;

namespace Cave
{
    public class Eyes : MonoBehaviour
    {

        public bool useDebugMover = false;
        
        private bool _usePositionSmoothing;
        private bool _useRotationSmoothing;
        private float _rotJitterReduction;
        private float _rotLagReduction;
        private float _posJitterReduction;
        private float _posLagReduction;

        // Use this for initialization
        void Start()
        {
            _usePositionSmoothing = API.Instance.Cave.EyesSettings.PositionSmoothing.EnableOneEuroSmoothing;
            _useRotationSmoothing = API.Instance.Cave.EyesSettings.RotationSmoothing.EnableOneEuroSmoothing;

            _rotJitterReduction = API.Instance.Cave.EyesSettings.RotationSmoothing.jitterReduction;
            _rotLagReduction = API.Instance.Cave.EyesSettings.RotationSmoothing.lagReduction;

            _posJitterReduction = API.Instance.Cave.EyesSettings.PositionSmoothing.jitterReduction;
            _posLagReduction = API.Instance.Cave.EyesSettings.PositionSmoothing.lagReduction;

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
            if (API.Instance.Cave.EyesSettings.TrackPosition)
            {
                // Position
                var posOri = transform.localPosition;
                var pos = VRPN.vrpnTrackerPos(API.Instance.Cave.EyesSettings.WorldVizObject + "@" + API.Instance.Cave.CaveSettings.Host, API.Instance.Cave.EyesSettings.Channel);
                if (_usePositionSmoothing)
                {
                    Vector3 filteredPos = Vector3.zero;
                    Vector3 filteredVelocity = Vector3.zero;
                    OneEuroFilter.ApplyOneEuroFilter(pos, Vector3.zero, posOri, Vector3.zero, ref filteredPos, ref filteredVelocity, _posJitterReduction, _posLagReduction);
                    pos = filteredPos;
                }

                // Block Axis
                if (API.Instance.Cave.EyesSettings.PositionAxisConstraints.X) pos.x = posOri.x;
                if (API.Instance.Cave.EyesSettings.PositionAxisConstraints.Y) pos.x = posOri.z;
                if (API.Instance.Cave.EyesSettings.PositionAxisConstraints.Z) pos.x = posOri.z;

                transform.localPosition = pos;
            }
        }

        private void HandleRotation()
        {
            if (API.Instance.Cave.EyesSettings.TrackRotation)
            {
                Vector3 rotOri = transform.rotation.eulerAngles;
                var rot = VRPN.vrpnTrackerQuat(API.Instance.Cave.EyesSettings.WorldVizObject + "@" + API.Instance.Cave.CaveSettings.Host, API.Instance.Cave.EyesSettings.Channel);

                //Debug.Log("eyes vrpn rot: " + rot.eulerAngles);

                //var roll = Mathf.Atan2(2 * rot.y * rot.w - 2 * rot.x * rot.z, 1 - 2 * rot.y * rot.y - 2 * rot.z * rot.z);
                //var pitch = Mathf.Atan2(2 * rot.x * rot.w - 2 * rot.y * rot.z, 1 - 2 * rot.x * rot.x - 2 * rot.z * rot.z);
                //var yaw = Mathf.Asin(2 * rot.x * rot.y + 2 * rot.z * rot.w);

                //Debug.Log("eyes vrpn pitch: " + pitch);

                if (_useRotationSmoothing)
                {
                    Vector3 filteredRot = Vector3.zero;
                    Vector3 filteredVelocity = Vector3.zero;
                    OneEuroFilter.ApplyOneEuroFilter(rot.eulerAngles , Vector3.zero, rotOri, Vector3.zero, ref filteredRot, ref filteredVelocity, _rotJitterReduction, _rotLagReduction);
                    rot.eulerAngles  = filteredRot;
                }

                Vector3 eulerAnglesNew = rot.eulerAngles;
                eulerAnglesNew.x = rotOri.x; // x (Pitch) not possible with eyes
                if (API.Instance.Cave.EyesSettings.RotationAxisConstraints.Yaw) eulerAnglesNew.y = rotOri.y;
                if (API.Instance.Cave.EyesSettings.RotationAxisConstraints.Roll) eulerAnglesNew.z = rotOri.z;

                rot.eulerAngles = eulerAnglesNew;

                transform.rotation = rot;
            }
        }

        public void DisableRenderer()
        {
            GetComponent<Renderer>().enabled = false;
        }
    }
}