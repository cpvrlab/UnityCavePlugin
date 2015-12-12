using UnityEngine;
using System.Collections;

namespace Cave
{
    [System.Serializable]
    public class SystemSettings : BasicSettings
    {
        public CameraManager CameraManagerPrefab;
        public FrustumManager FrustumManagerPrefab;
        public GameObject CameraContainerPrefab;
    }
}
