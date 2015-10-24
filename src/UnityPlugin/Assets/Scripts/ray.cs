using UnityEngine;
using System.Collections;

public class ray : MonoBehaviour {

	// Use this for initialization
	void Start () {
        System.Windows.Forms.Cursor.Position = new System.Drawing.Point(100, 100);
	}
	
	// Update is called once per frame
	void Update () {

        //Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
        //RaycastHit o;
        //if(Physics.Raycast(r, out o))
        //{
        //    //Debug.Log(string.Format("info hit: {0}", GetComponent<Camera>().WorldToScreenPoint(o.point)));

        //    Vector3 localSpaceHitPoint = o.transform.worldToLocalMatrix.MultiplyPoint(o.point);
        //    //Debug.Log(string.Format(o.transform.tag + ": {0}, {1}", localSpaceHitPoint.x, localSpaceHitPoint.z));

        //    switch (o.transform.tag)
        //    {
        //        case "CaveLeft":
        //            break;

        //        case "CaveFront":
        //            break;

        //        case "CaveRight":
        //            break;

        //        case "CaveBottom":
        //            break;
        //    }

        //}

        string host = "192.168.0.201";
        string obj = "PPT0";
        int channel = 0;

        //if (true)
        //{
        //    transform.position = VRPN.vrpnTrackerPos(obj + "@" + host, channel);
        //}
        //if (true)
        //{
        //    transform.rotation = VRPN.vrpnTrackerQuat(obj + "@" + host, channel);
        //}

        //Debug.Log("Joystick X: " + VRPN.vrpnAnalog("PPT_WAND1@" + host + ":8945", 0));
        //Debug.Log("Joystick Y: " + VRPN.vrpnAnalog("PPT_WAND1@" + host + ":8945", 1));

        //Debug.Log("Top / Left: " + VRPN.vrpnButton("PPT_WAND1@" + host + ":8945", 1));
        //Debug.Log("Top / Right: " + VRPN.vrpnButton("PPT_WAND1@" + host + ":8945", 2));
        //Debug.Log("Bottom / Left: " + VRPN.vrpnButton("PPT_WAND1@" + host + ":8945", 0));
        //Debug.Log("Bottom / Right: " + VRPN.vrpnButton("PPT_WAND1@" + host + ":8945", 3));
        //Debug.Log("Joystick Press: " + VRPN.vrpnButton("PPT_WAND1@" + host + ":8945", 4));
        //Debug.Log("Button Back: " + VRPN.vrpnButton("PPT_WAND1@" + host + ":8945", 5));

        //Debug.Log("------------------");
    }
}
