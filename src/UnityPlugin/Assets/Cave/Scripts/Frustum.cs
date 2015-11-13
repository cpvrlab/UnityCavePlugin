using UnityEngine;
using System.Collections;

public class Frustum : MonoBehaviour {

    #region "Functions"

    public static void setFrustum(ref Camera cam, double left, double right, double bottom, double top, double dist)
    {
        Matrix4x4 m = calcFrustum(left, right, bottom, top, cam.nearClipPlane, dist);

        cam.projectionMatrix = m;
    }

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
    #endregion



}
