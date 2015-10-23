using UnityEngine;
using System.Collections;
using System.Drawing;
using System;

public class Wand : MonoBehaviour {

    private Main _main;
    private Point _point;
    private int _multiplierX = 0;
    private int _multiplierY = 0;

	// Use this for initialization
	void Start () {
        _main = GameObject.Find("Main").GetComponent<Main>();
	}

    // Update is called once per frame
    void Update()
    {
        transform.position = VRPN.vrpnTrackerPos(_main.Obj + "@" + _main.Host, _main.Channel);

        var rot = VRPN.vrpnTrackerQuat(_main.Obj + "@" + _main.Host, _main.Channel);
        transform.rotation = rot;

        // Raycast
        var fwd = transform.TransformDirection(Vector3.forward);
        Ray ray = new Ray(transform.position, fwd);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            Vector3 localSpaceHitPoint = hit.transform.worldToLocalMatrix.MultiplyPoint(hit.point);
            Debug.DrawLine(transform.position, hit.point, UnityEngine.Color.cyan);
            //Debug.Log(string.Format(hit.transform.tag + ": {0}, {1}", localSpaceHitPoint.x, localSpaceHitPoint.z));

            switch (hit.transform.tag)
            {
                case "CaveLeft":
                    _multiplierX = 0;
                    _multiplierY = 0;
                    break;

                case "CaveFront":
                    _multiplierX = 1;
                    _multiplierY = 0;
                    break;

                case "CaveRight":
                    _multiplierX = 0;
                    _multiplierY = 1;
                    break;

                case "CaveBottom":
                    _multiplierX = 1;
                    _multiplierY = 1;
                    break;
            }

            float targetY = ((localSpaceHitPoint.x + 5f) / 10f) * _main.BeamerResolutionHeight + _multiplierY * _main.BeamerResolutionHeight;
            float targetX = ((localSpaceHitPoint.z + 5f) / 10f) * _main.BeamerResolutionWidth + _multiplierX * _main.BeamerResolutionWidth;

            //Debug.Log("localSpaceHitPoint.x: " + localSpaceHitPoint.x);

            Debug.Log(string.Format("{0}, {1}", targetX, targetY));

            System.Windows.Forms.Cursor.Position = new Point(Convert.ToInt32(targetX), Convert.ToInt32(targetY));
        }

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
