using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Cave;

public class FrustumManager : MonoBehaviour
{
    #region "private vars"
    private FrustumMode _myMode;
    #endregion

    void Awake()
    {
        transform.parent = API.Instance.Cave.gameObject.transform;
        _myMode = API.Instance.Cave.FrustumMode;
    }

    void Start () {
        

    }

    void Update () {

        if (API.Instance.Cave == null) return;
        
        foreach(KeyValuePair<int,CameraManager.ViewInfo>  var in API.Instance.CameraManager.FullViewInfo)
        {
            //cannot access to value-pair as reference in dict
            Camera cLeft = var.Value.Left.cam;
            Camera cRight = var.Value.Right.cam;
            Camera cLeftGUI = var.Value.Left.camGUI;
            Camera cRightGUI = var.Value.Right.camGUI;
            Camera cCursorLeft = var.Value.Left.camCursor;
            Camera cCursorRight = var.Value.Right.camCursor;

   
            if (_myMode == FrustumMode.CAVEXXL)
            {
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
                if(cLeftGUI != null)
                {
                    Frustum.GeneralizedPerspectiveProjection(ref cLeftGUI, var.Value.CAVESide.TransformXXL.TransformPoint(var.Value.CAVESide.corners.bottomleft),
                                                        var.Value.CAVESide.TransformXXL.TransformPoint(var.Value.CAVESide.corners.bottomright),
                                                        var.Value.CAVESide.TransformXXL.TransformPoint(var.Value.CAVESide.corners.topleft),
                                                        var.Value.Left.camGUI.transform.position,
                                                        var.Value.Left.camGUI.nearClipPlane,
                                                        var.Value.Left.camGUI.farClipPlane);
                }

                if (cRightGUI != null)
                {
                    Frustum.GeneralizedPerspectiveProjection(ref cRightGUI, var.Value.CAVESide.TransformXXL.TransformPoint(var.Value.CAVESide.corners.bottomleft),
                                                        var.Value.CAVESide.TransformXXL.TransformPoint(var.Value.CAVESide.corners.bottomright),
                                                        var.Value.CAVESide.TransformXXL.TransformPoint(var.Value.CAVESide.corners.topleft),
                                                        var.Value.Right.camGUI.transform.position,
                                                        var.Value.Right.camGUI.nearClipPlane,
                                                        var.Value.Right.camGUI.farClipPlane);
                }

                if (cCursorLeft != null && cCursorLeft.enabled)
                {
                    Frustum.GeneralizedPerspectiveProjection(ref cCursorLeft, var.Value.CAVESide.TransformXXL.TransformPoint(var.Value.CAVESide.corners.bottomleft),
                                                        var.Value.CAVESide.TransformXXL.TransformPoint(var.Value.CAVESide.corners.bottomright),
                                                        var.Value.CAVESide.TransformXXL.TransformPoint(var.Value.CAVESide.corners.topleft),
                                                        var.Value.Left.cam.transform.position,
                                                        var.Value.Left.cam.nearClipPlane,
                                                        var.Value.Left.cam.farClipPlane);
                }

                if (cCursorRight != null && cCursorRight.enabled)
                {
                    Frustum.GeneralizedPerspectiveProjection(ref cCursorRight, var.Value.CAVESide.TransformXXL.TransformPoint(var.Value.CAVESide.corners.bottomleft),
                                                        var.Value.CAVESide.TransformXXL.TransformPoint(var.Value.CAVESide.corners.bottomright),
                                                        var.Value.CAVESide.TransformXXL.TransformPoint(var.Value.CAVESide.corners.topleft),
                                                        var.Value.Right.cam.transform.position,
                                                        var.Value.Right.cam.nearClipPlane,
                                                        var.Value.Right.cam.farClipPlane);
                }

            }
            else
            {
                Frustum.GPP_Kooima(ref cLeft, var.Value.CAVESide.Transform.TransformPoint(var.Value.CAVESide.corners.bottomleft),
                                         var.Value.CAVESide.Transform.TransformPoint(var.Value.CAVESide.corners.bottomright),
                                         var.Value.CAVESide.Transform.TransformPoint(var.Value.CAVESide.corners.topleft),
                                         var.Value.Left.cam.transform.position,
                                         var.Value.Left.cam.nearClipPlane,
                                         var.Value.Left.cam.farClipPlane,
                                         var.Value.CAVESide.up);
                Frustum.GPP_Kooima(ref cRight, var.Value.CAVESide.Transform.TransformPoint(var.Value.CAVESide.corners.bottomleft),
                                                        var.Value.CAVESide.Transform.TransformPoint(var.Value.CAVESide.corners.bottomright),
                                                        var.Value.CAVESide.Transform.TransformPoint(var.Value.CAVESide.corners.topleft),
                                                        var.Value.Right.cam.transform.position,
                                                        var.Value.Right.cam.nearClipPlane,
                                                        var.Value.Right.cam.farClipPlane,
                                                        var.Value.CAVESide.up);
            }
        }


    }
}
