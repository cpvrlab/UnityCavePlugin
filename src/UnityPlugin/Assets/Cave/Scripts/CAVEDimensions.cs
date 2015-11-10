using UnityEngine;
using System.Collections;

namespace Cave
{
    [System.Serializable]
    public class CAVEDimensions : BasicSettings 
    {
        [Header("Left")]
        public float lWidth = 2.030f;
        public float lHeight = 2.540f;

        [Header("Front")]
        public float fWidth = 2.030f;
        public float fHeight = 2.540f;

        [Header("Right")]
        public float rWidth = 2.030f;
        public float rHeight = 2.540f;

        [Header("Bottom")]
        public float bWidth = 2.030f;
        public float bHeight = 2.540f;

    }

}
