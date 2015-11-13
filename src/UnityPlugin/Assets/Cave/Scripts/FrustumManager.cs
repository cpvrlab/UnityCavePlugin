using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Cave;

public class FrustumManager : MonoBehaviour
{


    #region "private vars"

    private CaveMain _main;
    private CameraManager _cameramanager;


    #endregion

    void Awake()
    {
        _main = GameObject.Find("Cave").GetComponent<CaveMain>();
        _cameramanager = GameObject.FindWithTag("CameraManager").GetComponent<CameraManager>();
    }

    void Start () {
    }
	
	void Update () {

        if (_main == null) return;

        if(_main.myCAVEMode == CAVEMode.FourScreen )
        {
            foreach(KeyValuePair<int,CameraManager.ViewInfo>  var in _cameramanager.FullViewInfo)
            {
                //cannot access to value-pair as reference in dict
                Camera c = var.Value.Left;

                Plane p = new Plane(var.Value.CAVESide.normal, var.Value.CAVESide.center);

                // TODO replace Vector3.zero with eye-translation
                Vector3 eye = new Vector3(-0.035f, 1.01f, 0);

                //float distance = Math.Abs(p.GetDistanceToPoint(_main.currentTrackedObject));
                //float distance = Math.Abs(var.Value.CAVESide.Plane.GetDistanceToPoint(eye));
                float distance = Math.Abs(p.GetDistanceToPoint(eye));

                // set the position of the camera
                //c.transform.position = eye;

                // set LookAt of camera
                //c.transform.LookAt(eye - var.Value.CAVESide.normal, var.Value.CAVESide.up);

                //use trackedObject and calc them to screen coordinates
                //Vector3 screenCoords = Quaternion.Inverse(var.Value.Left.transform.rotation) * _main.currentTrackedObject;
                eye = Quaternion.Inverse(var.Value.Left.transform.rotation) * eye;

                Frustum.setFrustum(ref c, (-eye.x - 0.5f * var.Value.CAVESide.width) * var.Value.Left.nearClipPlane / distance,
                                          (-eye.x + 0.5f * var.Value.CAVESide.width) * var.Value.Left.nearClipPlane / distance,
                                          (-eye.y - 0.5f * var.Value.CAVESide.height) * var.Value.Left.nearClipPlane / distance,
                                          (-eye.y + 0.5f * var.Value.CAVESide.height) * var.Value.Left.nearClipPlane / distance);
            }

        }
        else if(_main.myCAVEMode == CAVEMode.FourScreenStereo)
        {


        }
	}
}
