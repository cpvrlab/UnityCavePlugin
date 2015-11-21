﻿using UnityEngine;
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
        
        public AxisSensivity MovementSensivity;
        public BlockPositionAxis PositionAxisConstraints;
        public GameObject PositionTarget;
        public MovementSmoothing PositionMovementConstraints;

        [Header("Rotation")]
        public bool TrackRotation = true;
        public BlockRotationAxisEyes RotationAxisConstraints;
        public GameObject RotationTarget;
        public MovementSmoothing RotationMovementConstraints;


    }
}