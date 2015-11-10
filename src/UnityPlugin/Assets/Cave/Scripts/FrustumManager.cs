using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Cave;

public class FrustumManager : MonoBehaviour {


    #region "private vars"

    private CaveMain _main;
    private CameraManager _cameramanager;


    #endregion
    // Use this for initialization
    void Start () {
        _main = GameObject.Find("Cave").GetComponent<CaveMain>();
        _cameramanager = GameObject.Find("CameraManager").GetComponent<CameraManager>();

    }
	
	// Update is called once per frame
	void Update () {

        if(_main.myCAVEMode == CAVEMode.FourScreen )
        {
            foreach(KeyValuePair<int,CameraManager.ViewInfo>  var in _cameramanager.FullViewInfo)
            {
                //cannot access to value-pair as reference in dict
                Camera c = var.Value.Left;

                Plane p = new Plane(var.Value.CAVESide.normal, var.Value.CAVESide.center);
                float distance = Math.Abs(p.GetDistanceToPoint(_main.currentTrackedObject));

                //set the position of the  camera here?


                //use trackedObject and calc them to screen coordinates
                Vector3 screenCoords = Quaternion.Inverse(var.Value.Left.transform.rotation) * _main.currentTrackedObject;

                Frustum.setFrustum(ref c, (-screenCoords.x * -0.5f * var.Value.CAVESide.width) * var.Value.Left.nearClipPlane / distance,
                                                    (-screenCoords.x * +0.5f * var.Value.CAVESide.width) * var.Value.Left.nearClipPlane / distance,
                                                    (-screenCoords.y * -0.5f * var.Value.CAVESide.height) * var.Value.Left.nearClipPlane / distance,
                                                    (-screenCoords.y * +0.5f * var.Value.CAVESide.height) * var.Value.Left.nearClipPlane / distance);
            }

        }
        else if(_main.myCAVEMode == CAVEMode.FourScreenStereo)
        {


        }


	
	}
}
