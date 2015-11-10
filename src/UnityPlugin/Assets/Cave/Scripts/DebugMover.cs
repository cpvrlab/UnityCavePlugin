using UnityEngine;
using System.Collections;

namespace Cave
{

    

    public class DebugMover : MonoBehaviour
    {
    
        

        float TimeCounter = 0f;

        float speed;
        float width;
        float height;
        // Use this for initialization
        void Start()
        {
            speed = 0.5f;
            width = 0.4f;
            height = 0.7f;
        }

        // Update is called once per frame
        void Update()
        {
            TimeCounter += Time.deltaTime * speed;
            float x = Mathf.Abs(Mathf.Cos(TimeCounter) * width);
            float y = Mathf.Abs(Mathf.Sin(TimeCounter) * height);
            float z = 0.1f;

            transform.position = new Vector3(x, y, z);
            transform.rotation = new Quaternion(x, y, z,1f);
        }
    }

}
