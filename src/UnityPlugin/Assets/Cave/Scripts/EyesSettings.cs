using UnityEngine;
using System.Collections;

namespace Cave
{
    [System.Serializable]
    public class EyesSettings : BasicSettings
    {
        [Header("General")]
        public string WorldVizObject = "PPT0";
        public int Channel = 0;

        [Header("Position")]
        public bool TrackPosition = true;
        [Range(0.0f, 1.0f)]
        public float MovementSensivity;
        public BlockPositionAxis PositionAxisConstraints;
        public GameObject PositionTarget;

        [Header("Rotation")]
        public bool TrackRotation = true;
        public BlockRotationAxisEyes RotationAxisConstraints;
        public GameObject RotationTarget;
    }
}