using UnityEngine;
using System.Collections;

public class ray : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {


        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit o;
        if(Physics.Raycast(r, out o))
        {
            //Debug.Log(string.Format("info hit: {0}", GetComponent<Camera>().WorldToScreenPoint(o.point)));

            Vector3 localSpaceHitPoint = o.transform.worldToLocalMatrix.MultiplyPoint(o.point);
            Debug.Log(string.Format("localX: {0}, localY: {1}, localZ: {2}", localSpaceHitPoint.x, localSpaceHitPoint.y, localSpaceHitPoint.z));
        }
	}
}
