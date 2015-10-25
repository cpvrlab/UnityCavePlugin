using UnityEngine;
using System.Collections;
using Cave;
using System;
using System.Collections.Generic;

namespace Cave
{
    public class CameraManager : MonoBehaviour
    {

        public Camera CameraWithCursor
        {
            get
            {
                return _cameraWithCursor;
            }
        }

        private CaveMain _main;

        private Camera _cameraLeft;
        private Camera _cameraFront;
        private Camera _cameraRight;
        private Camera _cameraBottom;

        private Camera _cameraWithCursor;

        List<List<Camera>> _camMatrix;

        void Start()
        {
            _main = GameObject.Find("Cave").GetComponent<CaveMain>();

            // TODO go from 4 to 8 cams
            _cameraLeft = GameObject.Find("CameraLeft").GetComponent<Camera>();
            _cameraFront = GameObject.Find("CameraFront").GetComponent<Camera>();
            _cameraRight = GameObject.Find("CameraRight").GetComponent<Camera>();
            _cameraBottom = GameObject.Find("CameraBottom").GetComponent<Camera>();

            _camMatrix = new List<List<Camera>> {
            new List<Camera> { _cameraLeft, _cameraFront },
            new List<Camera> { _cameraRight, _cameraBottom }
        };
        }

        void Update()
        {

            // Get Camera based on cursor position
            int camIndexX = Math.Max(Convert.ToInt32(Mathf.Floor(Convert.ToInt32(Input.mousePosition.x) / _main.BeamerResolutionWidth)), 0);
            int camIndexY = Math.Max(Convert.ToInt32(Mathf.Floor(Convert.ToInt32(Screen.currentResolution.height - Input.mousePosition.y) / _main.BeamerResolutionHeight)), 0);

            _cameraWithCursor = _camMatrix[camIndexY][camIndexX];
        }
    }
}