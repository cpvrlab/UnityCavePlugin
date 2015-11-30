using UnityEngine;
using System.Collections;

public class Frustum : MonoBehaviour {

    #region "Functions"

    //public static void setFrustum(ref Camera cam, double left, double right, double bottom, double top)
    //{


    //    Matrix4x4 m = calcFrustum(left, right, bottom, top, cam.nearClipPlane, cam.farClipPlane);
    //    //Matrix4x4 m2 = PerspectiveOffCenter((float)left, (float)right, (float)bottom, (float)top, cam.nearClipPlane, cam.farClipPlane);


    //    // Debug.Log(string.Format("{0} {1} {2} {3} {4} {5} {6}",cam.name,left,right,top,cam.nearClipPlane,cam.farClipPlane));
    //    cam.projectionMatrix = m;
    //}

    //public static void setFrustum(ref Cave.CameraManager.ViewInfo vi, double left, double right, double bottom, double top)
    //{

    //    Vector3 newposLeft = vi.Left.cam.transform.position;
    //    Vector3 newposRight = vi.Right.cam.transform.position;


    //    float balancePointLeft = 1;
    //    float balancePointRight = 1;

    //    balancePointLeft = Mathf.Clamp(newposLeft.y, 0.001f, 2000f);
    //    balancePointRight = Mathf.Clamp(newposRight.y, 0.001f, 2000f);

    //    // Ratio for intercept theorem
    //    float ratioLeft = balancePointLeft / vi.Left.cam.nearClipPlane;
    //    float ratioRight = balancePointRight / vi.Right.cam.nearClipPlane;

    //    // Compute size for focal
    //    Vector4 vecfocalLeft = new Vector4((-vi.CAVESide.width / 2.0f) - newposLeft.x,
    //                                        (vi.CAVESide.width / 2.0f) - newposLeft.x,
    //                                        (vi.CAVESide.height / 2.0f) - newposLeft.y,
    //                                        (-vi.CAVESide.height / 2.0f) - newposLeft.y);
    //    Vector4 vecfocalRight = new Vector4((-vi.CAVESide.width / 2.0f) - newposRight.x,
    //                                        (vi.CAVESide.width / 2.0f) - newposRight.x,
    //                                        (vi.CAVESide.height / 2.0f) - newposRight.y,
    //                                        (-vi.CAVESide.height / 2.0f) - newposRight.y);

    //    Vector4 nearLeft = new Vector4(vecfocalLeft.x / ratioLeft, vecfocalLeft.y / ratioLeft, vecfocalLeft.z / ratioLeft, vecfocalLeft.w / ratioLeft);
    //    Vector4 nearRight = new Vector4(vecfocalRight.x / ratioRight, vecfocalRight.y / ratioRight, vecfocalRight.z / ratioRight, vecfocalRight.w / ratioRight);




    //    setFrustum(ref vi.Left.cam, nearLeft.x, nearLeft.y, nearLeft.z, nearLeft.w);
    //    setFrustum(ref vi.Right.cam, nearRight.x, nearRight.y, nearRight.z, nearRight.w);

    //}

    // frei nach http://www.songho.ca/opengl/gl_projectionmatrix.html
    public static Matrix4x4 calcFrustum(double left, double right, double bottom, double top, double near, double far)
    {
        //1. calc projectionmatrix http://www.songho.ca/opengl/files/gl_projectionmatrix_eq16.png
        //2. return matrix for camera 

        double a = (right + left) / (right - left);
        double b = (top + bottom) / (top - bottom);
        double c = -(far + near) / (far - near);
        double d = -2.0d * near * far / (far - near);

        double x = 2.0d * near / (right - left);
        double y = 2.0d * near / (top - bottom);

        Matrix4x4 m = new Matrix4x4();

        m[0, 0] = (float)x; m[0, 1] = 0; m[0, 2] = (float)a; m[0, 3] = 0;
        m[1, 0] = 0; m[1, 1] = (float)y; m[1, 2] = (float)b; m[1, 3] = 0;
        m[2, 0] = 0; m[2, 1] = 0; m[2, 2] = (float)c; m[2, 3] = (float)d;
        m[3, 0] = 0; m[3, 1] = 0; m[3, 2] = -1; m[3, 3] = 0;

        return m;

        //Matrix4x4 m = new Matrix4x4();
        //float m11, m12, m13, m14, m21, m22, m23, m24, m31, m32, m33, m34, m41, m42, m43, m44;
        //m11 = m12 = m13 = m14 = m21 = m22 = m23 = m24 = m31 = m32 = m33 = m34 = m41 = m42 = m43 = m44 = 0f;

        //m34 = -1f;
        //m11 = 2 * near / (right - left);
        //m22 = 2 * near / (top - bottom);

        //m31 = (right + left) / (right - left);
        //m32 = (top + bottom) / (top - bottom);
        //m33 = -(far + near) / (far - near);
        //m43 = -2 * near * far / (far - near);

        //m[0, 0] = m11;
        //m[0, 1] = m12;
        //m[0, 2] = m13;
        //m[0, 3] = m14;
        //m[1, 0] = m21;
        //m[1, 1] = m22;
        //m[1, 2] = m23;
        //m[1, 3] = m24;
        //m[2, 0] = m31;
        //m[2, 1] = m32;
        //m[2, 2] = m33;
        //m[2, 3] = m34;
        //m[3, 0] = m41;
        //m[3, 1] = m42;
        //m[3, 2] = m43;
        //m[3, 3] = m44;


        //return m;

    }


    public static void GeneralizedPerspectiveProjection(ref Camera c, Vector3 bottomLeft, Vector3 bottomRight, Vector3 topLeft, Vector3 headTrackerLocation, float near, float far)
    {
        Vector3 va, vb, vc;
        Vector3 vr, vu, vn;

        float left, right, bottom, top, eyedistance;

        Matrix4x4 transformMatrix;
        Matrix4x4 projectionM;
        Matrix4x4 eyeTranslateM;

        ///Calculate the orthonormal for the screen (the screen coordinate system
        vr = bottomRight - bottomLeft;
        vr.Normalize();
        vu = topLeft - bottomLeft;
        vu.Normalize();
        vn = -Vector3.Cross(vr, vu);
        vn.Normalize();

        //Calculate the vector from eye (pe) to screen corners (pa, pb, pc)
        va = bottomLeft - headTrackerLocation;
        vb = bottomRight - headTrackerLocation;
        vc = topLeft - headTrackerLocation;

        //Get the distance;; from the eye to the screen plane
        eyedistance = -(Vector3.Dot(va, vn));

        //Get the varaibles for the off center projection
        left = (Vector3.Dot(vr, va) * near) / eyedistance;
        right = (Vector3.Dot(vr, vb) * near) / eyedistance;
        bottom = (Vector3.Dot(vu, va) * near) / eyedistance;
        top = (Vector3.Dot(vu, vc) * near) / eyedistance;

        //Get this projection
        projectionM = PerspectiveOffCenter(left, right, bottom, top, near, far);

        //Fill in the transform matrix
        transformMatrix = new Matrix4x4();
        transformMatrix[0, 0] = vr.x;
        transformMatrix[0, 1] = vr.y;
        transformMatrix[0, 2] = vr.z;
        transformMatrix[0, 3] = 0;
        transformMatrix[1, 0] = vu.x;
        transformMatrix[1, 1] = vu.y;
        transformMatrix[1, 2] = vu.z;
        transformMatrix[1, 3] = 0;
        transformMatrix[2, 0] = vn.x;
        transformMatrix[2, 1] = vn.y;
        transformMatrix[2, 2] = vn.z;
        transformMatrix[2, 3] = 0;
        transformMatrix[3, 0] = 0;
        transformMatrix[3, 1] = 0;
        transformMatrix[3, 2] = 0;
        transformMatrix[3, 3] = 1;

        //Now for the eye transform
        eyeTranslateM = new Matrix4x4();
        eyeTranslateM[0, 0] = 1;
        eyeTranslateM[0, 1] = 0;
        eyeTranslateM[0, 2] = 0;
        eyeTranslateM[0, 3] = -headTrackerLocation.x;
        eyeTranslateM[1, 0] = 0;
        eyeTranslateM[1, 1] = 1;
        eyeTranslateM[1, 2] = 0;
        eyeTranslateM[1, 3] = -headTrackerLocation.y;
        eyeTranslateM[2, 0] = 0;
        eyeTranslateM[2, 1] = 0;
        eyeTranslateM[2, 2] = 1;
        eyeTranslateM[2, 3] = -headTrackerLocation.z;
        eyeTranslateM[3, 0] = 0;
        eyeTranslateM[3, 1] = 0;
        eyeTranslateM[3, 2] = 0;
        eyeTranslateM[3, 3] = 1f;


        c.projectionMatrix = projectionM;
        c.worldToCameraMatrix = transformMatrix * eyeTranslateM;


    }

    //static Matrix4x4 getWorldToCameraMatrix(ref Camera c, Vector3 bottomLeft, Vector3 bottomRight, Vector3 topLeft, Vector3 headTrackerLocation, float near, float far)
    //{
    //    //Fill in the transform matrix
    //    transformMatrix = new Matrix4x4();
    //    transformMatrix[0, 0] = vr.x;
    //    transformMatrix[0, 1] = vr.y;
    //    transformMatrix[0, 2] = vr.z;
    //    transformMatrix[0, 3] = 0;
    //    transformMatrix[1, 0] = vu.x;
    //    transformMatrix[1, 1] = vu.y;
    //    transformMatrix[1, 2] = vu.z;
    //    transformMatrix[1, 3] = 0;
    //    transformMatrix[2, 0] = vn.x;
    //    transformMatrix[2, 1] = vn.y;
    //    transformMatrix[2, 2] = vn.z;
    //    transformMatrix[2, 3] = 0;
    //    transformMatrix[3, 0] = 0;
    //    transformMatrix[3, 1] = 0;
    //    transformMatrix[3, 2] = 0;
    //    transformMatrix[3, 3] = 1;

    //    //Now for the eye transform
    //    eyeTranslateM = new Matrix4x4();
    //    eyeTranslateM[0, 0] = 1;
    //    eyeTranslateM[0, 1] = 0;
    //    eyeTranslateM[0, 2] = 0;
    //    eyeTranslateM[0, 3] = -headTrackerLocation.x;
    //    eyeTranslateM[1, 0] = 0;
    //    eyeTranslateM[1, 1] = 1;
    //    eyeTranslateM[1, 2] = 0;
    //    eyeTranslateM[1, 3] = -headTrackerLocation.y;
    //    eyeTranslateM[2, 0] = 0;
    //    eyeTranslateM[2, 1] = 0;
    //    eyeTranslateM[2, 2] = 1;
    //    eyeTranslateM[2, 3] = -headTrackerLocation.z;
    //    eyeTranslateM[3, 0] = 0;
    //    eyeTranslateM[3, 1] = 0;
    //    eyeTranslateM[3, 2] = 0;
    //    eyeTranslateM[3, 3] = 1f;


    //    c.projectionMatrix = projectionM;
    //    c.worldToCameraMatrix = transformMatrix * eyeTranslateM;
    //}

    static Matrix4x4 PerspectiveOffCenter(float left, float right, float bottom, float top, float near, float far)
    {
        float x = 2.0F * near / (right - left);
        float y = 2.0F * near / (top - bottom);
        float a = (right + left) / (right - left);
        float b = (top + bottom) / (top - bottom);
        float c = -(far + near) / (far - near);
        float d = -(2.0F * far * near) / (far - near);
        float e = -1.0F;
        Matrix4x4 m = new Matrix4x4();
        m[0, 0] = x;
        m[0, 1] = 0;
        m[0, 2] = a;
        m[0, 3] = 0;
        m[1, 0] = 0;
        m[1, 1] = y;
        m[1, 2] = b;
        m[1, 3] = 0;
        m[2, 0] = 0;
        m[2, 1] = 0;
        m[2, 2] = c;
        m[2, 3] = d;
        m[3, 0] = 0;
        m[3, 1] = 0;
        m[3, 2] = e;
        m[3, 3] = 0;

        return m;
    }



    public static void OffAxisProjection(ref Camera c , Vector3 bottomLeft, Vector3 bottomRight, Vector3 topLeft, Vector3 headTrackerLocation, float near, float far) {
        // This script should be attached to a Camera object 
        // in Unity. Once a Plane object is specified as the 
        // "projectionScreen", the script computes a suitable
        // view and projection matrix for the camera.
        // The code is based on Robert Kooima's publication  
        // "Generalized Perspective Projection," 2009, 
        // http://csc.lsu.edu/~kooima/pdfs/gen-perspective.pdf 



        Vector3 pa = bottomLeft;
        // lower left corner in world coordinates
        Vector3 pb = bottomRight;
        // lower right corner
        Vector3 pc = topLeft;
        // upper left corner
        Vector3 pe = headTrackerLocation;
        // eye position
        float n = near;
        // distance of near clipping plane
        float f = far;
        // distance of far clipping plane

        Vector3 va; // from pe to pa
        Vector3 vb; // from pe to pb
        Vector3 vc; // from pe to pc
        Vector3 vr; // right axis of screen
        Vector3 vu; // up axis of screen
        Vector3 vn; // normal vector of screen

        float l; // distance to left screen edge
        float r; // distance to right screen edge
        float b; // distance to bottom screen edge
        float t; // distance to top screen edge
        float d; // distance from eye to screen 

        vr = pb - pa;
        vu = pc - pa;
        vr.Normalize();
        vu.Normalize();
        vn = -Vector3.Cross(vr, vu);
        // we need the minus sign because Unity 
        // uses a left-handed coordinate system
        vn.Normalize();

        va = pa - pe;
        vb = pb - pe;
        vc = pc - pe;

        d = -Vector3.Dot(va, vn);
        l = Vector3.Dot(vr, va) * n / d;
        r = Vector3.Dot(vr, vb) * n / d;
        b = Vector3.Dot(vu, va) * n / d;
        t = Vector3.Dot(vu, vc) * n / d;

        Matrix4x4 p = new Matrix4x4(); // projection matrix 
        p[0, 0] = 2.0f * n / (r - l);
        p[0, 1] = 0.0f;
        p[0, 2] = (r + l) / (r - l);
        p[0, 3] = 0.0f;

        p[1, 0] = 0.0f;
        p[1, 1] = 2.0f * n / (t - b);
        p[1, 2] = (t + b) / (t - b);
        p[1, 3] = 0.0f;

        p[2, 0] = 0.0f;
        p[2, 1] = 0.0f;
        p[2, 2] = (f + n) / (n - f);
        p[2, 3] = 2.0f * f * n / (n - f);

        p[3, 0] = 0.0f;
        p[3, 1] = 0.0f;
        p[3, 2] = -1.0f;
        p[3, 3] = 0.0f;

        Matrix4x4 rm = new Matrix4x4(); // rotation matrix;
        rm[0, 0] = vr.x;
        rm[0, 1] = vr.y;
        rm[0, 2] = vr.z;
        rm[0, 3] = 0.0f;

        rm[1, 0] = vu.x;
        rm[1, 1] = vu.y;
        rm[1, 2] = vu.z;
        rm[1, 3] = 0.0f;

        rm[2, 0] = vn.x;
        rm[2, 1] = vn.y;
        rm[2, 2] = vn.z;
        rm[2, 3] = 0.0f;

        rm[3, 0] = 0.0f;
        rm[3, 1] = 0.0f;
        rm[3, 2] = 0.0f;
        rm[3, 3] = 1.0f;

        Matrix4x4 tm = new Matrix4x4(); // translation matrix;
        tm[0, 0] = 1.0f;
        tm[0, 1] = 0.0f;
        tm[0, 2] = 0.0f;
        tm[0, 3] = -pe.x;

        tm[1, 0] = 0.0f;
        tm[1, 1] = 1.0f;
        tm[1, 2] = 0.0f;
        tm[1, 3] = -pe.y;

        tm[2, 0] = 0.0f;
        tm[2, 1] = 0.0f;
        tm[2, 2] = 1.0f;
        tm[2, 3] = -pe.z;

        tm[3, 0] = 0.0f;
        tm[3, 1] = 0.0f;
        tm[3, 2] = 0.0f;
        tm[3, 3] = 1.0f;

        // set matrices
        c.projectionMatrix = p * rm * tm;
        c.worldToCameraMatrix = Matrix4x4.identity;
        // we put everything into the projection matrix: 
        // because our "viewing matrix" might look at a  
        // point that is off the screen.

        if (true)
        {
            // rotate camera to screen for culling to work
            Quaternion q = new Quaternion();
            q.SetLookRotation((0.5f * (pb + pc) - pe), vu);
            // look at center of screen
            c.transform.rotation = q;

            // set fieldOfView to a conservative estimate 
            // to make frustum tall enough
            if (c.aspect >= 1.0f)
            {
                c.fieldOfView = Mathf.Rad2Deg *
                    Mathf.Atan(((pb - pa).magnitude + (pc - pa).magnitude)
                    / va.magnitude);
            }
            else
            {
                // take the camera aspect into account to 
                // make the frustum wide enough 
                c.fieldOfView =
                    Mathf.Rad2Deg / c.aspect *
                    Mathf.Atan(((pb - pa).magnitude + (pc - pa).magnitude)
                    / va.magnitude);
            }
        }
    }
    
    
    #endregion

}


