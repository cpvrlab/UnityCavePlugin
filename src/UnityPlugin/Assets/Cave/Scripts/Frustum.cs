using UnityEngine;
using System.Collections;

public class Frustum : MonoBehaviour {

    #region "Functions"

    public static void setFrustum(ref Camera cam, float left,float right, float bottom, float top)
    {
        Matrix4x4 m = calcFrustum(left, right, bottom, top, cam.nearClipPlane, cam.farClipPlane);


        cam.projectionMatrix = m;
    }

    // frei nach http://www.songho.ca/opengl/gl_projectionmatrix.html
    public static Matrix4x4 calcFrustum(float left, float right, float bottom, float top, float near, float far)
    {
        //1. calc projectionmatrix http://www.songho.ca/opengl/files/gl_projectionmatrix_eq16.png
        //2. return matrix for camera 


        Matrix4x4 m = new Matrix4x4();
        float m11, m12, m13, m14, m21, m22, m23, m24, m31, m32, m33, m34, m41, m42, m43, m44;
        m11 = m12 = m13 = m14 = m21 = m22 = m23 = m24 = m31 = m32 = m33 = m34 = m41 = m42 = m43 = m44 = 0f;

        m34 = -1f;
        m11 = 2 * near / (right - left);
        m22 = 2 * near / (top - bottom);

        m31 = (right + left) / (right - left);
        m32 = (top + bottom) / (top - bottom);
        m33 = -(far + near) / (far - near);
        m43 = -2 * near * far / (far - near);

        m[0, 0] = m11;
        m[0, 0] = m12;
        m[0, 0] = m13;
        m[0, 0] = m14;
        m[0, 0] = m21;
        m[0, 0] = m22;
        m[0, 0] = m23;
        m[0, 0] = m24;
        m[0, 0] = m31;
        m[0, 0] = m32;
        m[0, 0] = m33;
        m[0, 0] = m34;
        m[0, 0] = m41;
        m[0, 0] = m42;
        m[0, 0] = m43;
        m[0, 0] = m44;


        return m;

    }
    #endregion



}
