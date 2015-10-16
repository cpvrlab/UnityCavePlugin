//using UnityEngine;
using System.Runtime.InteropServices;
using WandToMouse;

public static class VRPN{
	[DllImport ("unityVrpn")]
	private static extern double vrpnAnalogExtern(string address, int channel, int frameCount);

	[DllImport ("unityVrpn")]
	private static extern bool vrpnButtonExtern(string address, int channel, int frameCount);
	
	[DllImport ("unityVrpn")]
	private static extern double vrpnTrackerExtern(string address, int channel, int component, int frameCount);
	
	public static double vrpnAnalog(string address, int channel){
        return vrpnAnalogExtern(address, channel, Program.Ticks);
	}
	
    //public static bool vrpnButton(string address, int channel){
    //    return vrpnButtonExtern (address, channel, Time.frameCount);	
    //}
	
    //public static Vector3 vrpnTrackerPos(string address, int channel){
    //    return new Vector3(
    //        (float) vrpnTrackerExtern(address, channel, 0, Time.frameCount),
    //        (float) vrpnTrackerExtern(address, channel, 1, Time.frameCount),
    //        (float) vrpnTrackerExtern(address, channel, 2, Time.frameCount));
    //}

    public static double Yaw(string address, int channel)
    {
        return vrpnTrackerExtern(address, channel, 4, Program.Ticks);
    }

    public static double Pitch(string address, int channel)
    {
        return vrpnTrackerExtern(address, channel, 5, Program.Ticks);
    }

    public static string vrpnTrackerQuat(string address, int channel)
    {
        string retval = string.Empty;

        retval += vrpnTrackerExtern(address, channel, 3, Program.Ticks).ToString() + " - ";
        retval += vrpnTrackerExtern(address, channel, 4, Program.Ticks).ToString() + " - ";
        retval += vrpnTrackerExtern(address, channel, 5, Program.Ticks).ToString() + " - ";
        retval += vrpnTrackerExtern(address, channel, 6, Program.Ticks).ToString() + " - ";

        return retval;

        //return new Quaternion(
        //    (float)vrpnTrackerExtern(address, channel, 3, Program.Ticks),
        //    (float)vrpnTrackerExtern(address, channel, 4, Program.Ticks),
        //    (float)vrpnTrackerExtern(address, channel, 5, Program.Ticks),
        //    (float)vrpnTrackerExtern(address, channel, 6, Program.Ticks));
    }
}
