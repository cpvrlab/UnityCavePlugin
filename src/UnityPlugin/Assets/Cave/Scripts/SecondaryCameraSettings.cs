using UnityEngine;
using System.Collections;

namespace Cave
{
    [System.Serializable]
    public class SecondaryCameraSettings : BasicSettings
    {
        public Camera Camera;
        public BasicSettings.Sides Side;
        //public Rect ViewportRect;
    }
}