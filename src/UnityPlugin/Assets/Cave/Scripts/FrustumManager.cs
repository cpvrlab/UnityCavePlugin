using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Cave;

public class FrustumManager : MonoBehaviour
{
    #region "private vars"
    private FrustumMode _frustumMode;
    #endregion

    void Awake()
    {
        transform.parent = API.Instance.Cave.gameObject.transform;
        _frustumMode = API.Instance.Cave.CaveSettings.FrustumMode;
    }

    void Start () {
        

    }

    //void Update() {
    void FixedUpdate() {

        if (API.Instance.Cave == null) return;
        
        foreach(KeyValuePair<int,CameraManager.ViewInfo>  var in API.Instance.CameraManager.FullViewInfo)
        {
            //cannot access to value-pair as reference in dict
            Camera cLeft = var.Value.Left.Cam;
            Camera cRight = var.Value.Right.Cam;
            Camera cLeftGUI = var.Value.Left.CamGUI;
            Camera cRightGUI = var.Value.Right.CamGUI;
            Camera cCursorLeft = var.Value.Left.CamCursor;
            Camera cCursorRight = var.Value.Right.CamCursor;

   
            if (_frustumMode == FrustumMode.CAVEXXL)
            {
                Frustum.GeneralizedPerspectiveProjection(ref cLeft, var.Value.CAVESide.TransformXXL.TransformPoint(var.Value.CAVESide.Corners.Bottomleft),
                                                        var.Value.CAVESide.TransformXXL.TransformPoint(var.Value.CAVESide.Corners.Bottomright),
                                                        var.Value.CAVESide.TransformXXL.TransformPoint(var.Value.CAVESide.Corners.Topleft),
                                                        var.Value.Left.Cam.transform.position,
                                                        var.Value.Left.Cam.nearClipPlane,
                                                        var.Value.Left.Cam.farClipPlane);

                Frustum.GeneralizedPerspectiveProjection(ref cRight, var.Value.CAVESide.TransformXXL.TransformPoint(var.Value.CAVESide.Corners.Bottomleft),
                                                        var.Value.CAVESide.TransformXXL.TransformPoint(var.Value.CAVESide.Corners.Bottomright),
                                                        var.Value.CAVESide.TransformXXL.TransformPoint(var.Value.CAVESide.Corners.Topleft),
                                                        var.Value.Right.Cam.transform.position,
                                                        var.Value.Right.Cam.nearClipPlane,
                                                        var.Value.Right.Cam.farClipPlane);
                if(cLeftGUI != null && cLeftGUI.enabled)
                {
                    Frustum.GeneralizedPerspectiveProjection(ref cLeftGUI, var.Value.CAVESide.TransformXXL.TransformPoint(var.Value.CAVESide.Corners.Bottomleft),
                                                        var.Value.CAVESide.TransformXXL.TransformPoint(var.Value.CAVESide.Corners.Bottomright),
                                                        var.Value.CAVESide.TransformXXL.TransformPoint(var.Value.CAVESide.Corners.Topleft),
                                                        var.Value.Left.CamGUI.transform.position,
                                                        var.Value.Left.CamGUI.nearClipPlane,
                                                        var.Value.Left.CamGUI.farClipPlane);
                }

                if (cRightGUI != null && cRightGUI.enabled)
                {
                    Frustum.GeneralizedPerspectiveProjection(ref cRightGUI, var.Value.CAVESide.TransformXXL.TransformPoint(var.Value.CAVESide.Corners.Bottomleft),
                                                        var.Value.CAVESide.TransformXXL.TransformPoint(var.Value.CAVESide.Corners.Bottomright),
                                                        var.Value.CAVESide.TransformXXL.TransformPoint(var.Value.CAVESide.Corners.Topleft),
                                                        var.Value.Right.CamGUI.transform.position,
                                                        var.Value.Right.CamGUI.nearClipPlane,
                                                        var.Value.Right.CamGUI.farClipPlane);
                }

                if (cCursorLeft != null && cCursorLeft.enabled)
                {
                    Frustum.GeneralizedPerspectiveProjection(ref cCursorLeft, var.Value.CAVESide.TransformXXL.TransformPoint(var.Value.CAVESide.Corners.Bottomleft),
                                                        var.Value.CAVESide.TransformXXL.TransformPoint(var.Value.CAVESide.Corners.Bottomright),
                                                        var.Value.CAVESide.TransformXXL.TransformPoint(var.Value.CAVESide.Corners.Topleft),
                                                        var.Value.Left.Cam.transform.position,
                                                        var.Value.Left.Cam.nearClipPlane,
                                                        var.Value.Left.Cam.farClipPlane);
                }

                if (cCursorRight != null && cCursorRight.enabled)
                {
                    Frustum.GeneralizedPerspectiveProjection(ref cCursorRight, var.Value.CAVESide.TransformXXL.TransformPoint(var.Value.CAVESide.Corners.Bottomleft),
                                                        var.Value.CAVESide.TransformXXL.TransformPoint(var.Value.CAVESide.Corners.Bottomright),
                                                        var.Value.CAVESide.TransformXXL.TransformPoint(var.Value.CAVESide.Corners.Topleft),
                                                        var.Value.Right.Cam.transform.position,
                                                        var.Value.Right.Cam.nearClipPlane,
                                                        var.Value.Right.Cam.farClipPlane);
                }

            }
            else
            {
                Frustum.GPP_Kooima(ref cLeft, var.Value.CAVESide.Transform.TransformPoint(var.Value.CAVESide.Corners.Bottomleft),
                                         var.Value.CAVESide.Transform.TransformPoint(var.Value.CAVESide.Corners.Bottomright),
                                         var.Value.CAVESide.Transform.TransformPoint(var.Value.CAVESide.Corners.Topleft),
                                         var.Value.Left.Cam.transform.position,
                                         var.Value.Left.Cam.nearClipPlane,
                                         var.Value.Left.Cam.farClipPlane,
                                         var.Value.CAVESide.Up);
                Frustum.GPP_Kooima(ref cRight, var.Value.CAVESide.Transform.TransformPoint(var.Value.CAVESide.Corners.Bottomleft),
                                                        var.Value.CAVESide.Transform.TransformPoint(var.Value.CAVESide.Corners.Bottomright),
                                                        var.Value.CAVESide.Transform.TransformPoint(var.Value.CAVESide.Corners.Topleft),
                                                        var.Value.Right.Cam.transform.position,
                                                        var.Value.Right.Cam.nearClipPlane,
                                                        var.Value.Right.Cam.farClipPlane,
                                                        var.Value.CAVESide.Up);
            }
        }
    }
}