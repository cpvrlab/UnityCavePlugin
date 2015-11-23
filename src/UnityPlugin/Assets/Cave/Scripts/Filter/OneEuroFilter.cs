// copyright by http://www.lifl.fr/~casiez/1euro/
// and https://mitsufu.wordpress.com/2012/05/09/lissage-oneeurofilter-implmentation-en-c-et-f/
// and https://github.com/slek120/Unity-Touch-Smoothing-One-Euro-Filter


using UnityEngine;

public class OneEuroFilter  {



    public static void ApplyOneEuroFilter(  Vector3 currentPosition, Vector3 currentVelocity,
                                            Vector3 oldPosition, Vector3 oldVelocity,
                                            ref Vector3 newPosition, ref Vector3 newVelocity,
                                            float jitterReduction, float lagReduction)
    {
        if (Mathf.Approximately((currentVelocity - oldVelocity).sqrMagnitude, 0))
        {
            //Skip if filtering is unnecessary
            newVelocity = currentVelocity;
        }
        else
        {
            //Get a smooth velocity using exponential smoothing
            newVelocity = Filter(currentVelocity, oldVelocity, Alpha(Vector3.one));
        }

        if (Mathf.Approximately((currentPosition - oldPosition).sqrMagnitude, 0))
        {
            //Skip if filtering is unnecessary
            newPosition = currentPosition;
        }
        else
        {
            //Use velocity to get smoothing factor for position
            Vector3 cutoffFrequency;
            cutoffFrequency.x = jitterReduction + 0.01f * lagReduction * Mathf.Abs(oldVelocity.x);
            cutoffFrequency.y = jitterReduction + 0.01f * lagReduction * Mathf.Abs(oldVelocity.y);
            cutoffFrequency.z = jitterReduction + 0.01f * lagReduction * Mathf.Abs(oldVelocity.z);
            //Get a smooth position using exponential smoothing with smoothing factor from velocity
            newPosition = Filter(currentPosition, oldPosition, Alpha(cutoffFrequency));
        }
    }

    public static Vector3 Alpha(Vector3 cutoff)
    {
        float tauX = 1 / (2 * Mathf.PI * cutoff.x);
        float tauY = 1 / (2 * Mathf.PI * cutoff.y);
        float tauZ = 1 / (2 * Mathf.PI * cutoff.z);
        float alphaX = 1 / (1 + tauX / Time.deltaTime);
        float alphaY = 1 / (1 + tauY / Time.deltaTime);
        float alphaZ = 1 / (1 + tauZ / Time.deltaTime);
        alphaX = Mathf.Clamp(alphaX, 0, 1);
        alphaY = Mathf.Clamp(alphaY, 0, 1);
        alphaZ = Mathf.Clamp(alphaZ, 0, 1);
        return new Vector3(alphaX, alphaY,alphaZ );
    }

    public static  Vector3 Filter(Vector3 current, Vector3 previous, Vector3 alpha)
    {
        float x = alpha.x * current.x + (1 - alpha.x) * previous.x;
        float y = alpha.y * current.y + (1 - alpha.y) * previous.y;
        float z = alpha.z * current.z + (1 - alpha.z) * previous.z;
        return new Vector3(x, y,z);
    }
}
