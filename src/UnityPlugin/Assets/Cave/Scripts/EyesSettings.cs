using UnityEngine;
using System.Collections;

namespace Cave
{
    [System.Serializable]
    public class EyesSettings : BasicSettings
    {
        [Header("Position")]
        public bool TrackPosition = true;
        public AxisSensivity MovementSensivity;
        public BlockPositionAxis PositionAxisConstraints;
        public MovementSmoothing PositionSmoothing;

        [Header("Rotation")]
        public bool TrackRotation = true;
        public BlockRotationAxisEyes RotationAxisConstraints;
        public MovementSmoothing RotationSmoothing;

        [Header("VRPN")]
        public string WorldVizObject = "PPT0";
        public int Channel = 0;
    }
}