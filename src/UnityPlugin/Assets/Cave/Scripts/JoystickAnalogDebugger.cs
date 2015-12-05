using UnityEngine;
using System.Collections;

namespace Cave
{
    public class JoystickAnalogDebugger : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            // Register
            API.Instance.Wand.OnJoystickAnalogUpdate += OnJoystickUpdate;
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnJoystickUpdate(float x, float y)
        {
            Vector3 posNew = transform.position;
            posNew.x += x / 10f;
            posNew.z += y / 10f;

            transform.position = posNew;
        }
    }
}