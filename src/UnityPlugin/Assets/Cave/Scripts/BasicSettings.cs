using UnityEngine;
using System.Collections;

namespace Cave
{
    public class BasicSettings
    {

        [System.Serializable]
        public class BlockPositionAxis
        {
            public bool X;
            public bool Y;
            public bool Z;
        }

        [System.Serializable]
        public class BlockRotationAxisWand
        {
            public bool Yaw;
            public bool Pitch;
            public bool Roll;
        }

        [System.Serializable]
        public class BlockRotationAxisEyes
        {
            public bool Yaw;
            public bool Roll;
        }
    }
}
