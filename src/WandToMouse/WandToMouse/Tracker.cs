//using UnityEngine;
using System.Collections;
using System;

public class Tracker
{
    public static string host = "192.168.0.201";
    public static string obj;

    public static bool trackPosition = true;
    public static bool trackRotation = true;

    // Update is called once per frame
    public static void Update()
    {
        if (trackPosition)
        {
            //transform.position = VRPN.vrpnTrackerPos("PPT0@" + host, 0);
        }
        if (trackRotation)
        {
            //transform.rotation = VRPN.vrpnTrackerQuat("PPT0@" + host, 0);
        }

        Console.WriteLine("Joystick X: " + VRPN.vrpnAnalog("PPT_WAND1@" + host + ":8945", 0));
        Console.WriteLine("Joystick Y: " + VRPN.vrpnAnalog("PPT_WAND1@" + host + ":8945", 1));

        //Console.WriteLine("Rotation: " + VRPN.vrpnTrackerQuat("PPT0@" + host, 0));

        Console.WriteLine("Rotation: " + VRPN.Yaw("PPT0@" + host, 0));

        //Console.WriteLine("Top / Left: " + VRPN.vrpnButton("PPT_WAND1@" + host + ":8945", 1));
        //Console.WriteLine("Top / Right: " + VRPN.vrpnButton("PPT_WAND1@" + host + ":8945", 2));
        //Console.WriteLine("Bottom / Left: " + VRPN.vrpnButton("PPT_WAND1@" + host + ":8945", 0));
        //Console.WriteLine("Bottom / Right: " + VRPN.vrpnButton("PPT_WAND1@" + host + ":8945", 3));
        //Console.WriteLine("Joystick Press: " + VRPN.vrpnButton("PPT_WAND1@" + host + ":8945", 4));
        //Console.WriteLine("Button Back: " + VRPN.vrpnButton("PPT_WAND1@" + host + ":8945", 5));

        Console.WriteLine("------------------");   
    }
}
