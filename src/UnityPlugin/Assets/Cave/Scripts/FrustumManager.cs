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

        transform.parent = _main.gameObject.transform;
    }

    void Start () {
        

    }

    void Update () {

        if (_main == null) return;
        
        foreach(KeyValuePair<int,CameraManager.ViewInfo>  var in _cameramanager.FullViewInfo)
        {
            //cannot access to value-pair as reference in dict
            Camera cLeft = var.Value.Left.cam;
            Camera cRight = var.Value.Right.cam;

            //cLeft.ResetWorldToCameraMatrix();
            //cRight.ResetWorldToCameraMatrix();



            //Plane p = new Plane(var.Value.CAVESide.normal, var.Value.CAVESide.center);

            //Vector3 eyeLeft = var.Value.Left.offset;
            //Vector3 eyeRight = var.Value.Left.offset;

            ////float distance = Math.Abs(p.GetDistanceToPoint(_main.currentTrackedObject));
            ////float distance = Math.Abs(var.Value.CAVESide.Plane.GetDistanceToPoint(eye));
            //float distanceLeft = Math.Abs(p.GetDistanceToPoint(eyeLeft));
            //float distanceRight = Math.Abs(p.GetDistanceToPoint(eyeLeft));

            //// set the position of the camera
            ////c.transform.position = eye;

            //// set LookAt of camera
            ////c.transform.LookAt(eye - var.Value.CAVESide.normal, var.Value.CAVESide.up);

            ////use trackedObject and calc them to screen coordinates
            ////Vector3 screenCoords = Quaternion.Inverse(var.Value.Left.transform.rotation) * _main.currentTrackedObject;
            //eyeLeft = Quaternion.Inverse(var.Value.Left.cam.transform.rotation) * eyeLeft;
            //eyeRight = Quaternion.Inverse(var.Value.Right.cam.transform.rotation) * eyeRight;

            //CameraManager.ViewInfo vi = var.Value;

            ////if(var.Key < 2 )
            ////{ 
            ////Frustum.setFrustum(ref vi,
            ////     (-eyeLeft.x - 0.5f * var.Value.CAVESide.width) * var.Value.Left.cam.nearClipPlane / distanceLeft,
            ////                            (-eyeLeft.x + 0.5f * var.Value.CAVESide.width) * var.Value.Left.cam.nearClipPlane / distanceLeft,
            ////                            (-eyeLeft.y - 0.5f * var.Value.CAVESide.height) * var.Value.Left.cam.nearClipPlane / distanceLeft,
            ////                            (-eyeLeft.y + 0.5f * var.Value.CAVESide.height) * var.Value.Left.cam.nearClipPlane / distanceLeft);
            ////}
            ////else {
            ////    Frustum.setFrustum(ref cLeft, (-eyeLeft.x - 0.5f * var.Value.CAVESide.width) * var.Value.Left.cam.nearClipPlane / distanceLeft,
            ////                                (-eyeLeft.x + 0.5f * var.Value.CAVESide.width) * var.Value.Left.cam.nearClipPlane / distanceLeft,
            ////                                (-eyeLeft.y - 0.5f * var.Value.CAVESide.height) * var.Value.Left.cam.nearClipPlane / distanceLeft,
            ////                                (-eyeLeft.y + 0.5f * var.Value.CAVESide.height) * var.Value.Left.cam.nearClipPlane / distanceLeft);

            ////    Frustum.setFrustum(ref cRight, (-eyeRight.x - 0.5f * var.Value.CAVESide.width) * var.Value.Right.cam.nearClipPlane / distanceRight,
            ////                                (-eyeRight.x + 0.5f * var.Value.CAVESide.width) * var.Value.Right.cam.nearClipPlane / distanceRight,
            ////                                (-eyeRight.y - 0.5f * var.Value.CAVESide.height) * var.Value.Right.cam.nearClipPlane / distanceRight,
            ////                                (-eyeRight.y + 0.5f * var.Value.CAVESide.height) * var.Value.Right.cam.nearClipPlane / distanceRight);
            ////}


            // http://forum.unity3d.com/threads/code-sample-off-center-projection-code-for-vr-cave-or-just-for-fun.142383/

            //var.Value.Left.cam.projectionMatrix = Frustum.GeneralizedPerspectiveProjection(bottomLeft, bottomRight, topLeft, headTrackerLocation, near, far);


            Frustum.GeneralizedPerspectiveProjection(ref cLeft, var.Value.CAVESide.TransformXXL.TransformPoint(var.Value.CAVESide.corners.bottomleft),
                                                    var.Value.CAVESide.TransformXXL.TransformPoint(var.Value.CAVESide.corners.bottomright),
                                                    var.Value.CAVESide.TransformXXL.TransformPoint(var.Value.CAVESide.corners.topleft),
                                                    var.Value.Left.cam.transform.position,
                                                    var.Value.Left.cam.nearClipPlane,
                                                    var.Value.Left.cam.farClipPlane);
            Frustum.GeneralizedPerspectiveProjection(ref cRight, var.Value.CAVESide.TransformXXL.TransformPoint(var.Value.CAVESide.corners.bottomleft),
                                                    var.Value.CAVESide.TransformXXL.TransformPoint(var.Value.CAVESide.corners.bottomright),
                                                    var.Value.CAVESide.TransformXXL.TransformPoint(var.Value.CAVESide.corners.topleft),
                                                    var.Value.Right.cam.transform.position,
                                                    var.Value.Right.cam.nearClipPlane,
                                                    var.Value.Right.cam.farClipPlane);
        }


    }
}
