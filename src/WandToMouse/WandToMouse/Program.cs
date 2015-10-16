using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Collections;
using System.Timers;

namespace WandToMouse
{
    class Program
    {
        public static int Ticks = 0;

        private static Rectangle ScreenDimensions;

        static void Main(string[] args)
        {
            Cursor.Position = new Point(100, 150);
            ScreenDimensions = Screen.PrimaryScreen.Bounds;
            
            var timer = new System.Timers.Timer();
            timer.Interval = 10;
            timer.Elapsed += new ElapsedEventHandler(TimerTicked);
            timer.Enabled = true;

            Console.ReadLine();
        }

        static void TimerTicked(object sender, ElapsedEventArgs e)
        {
            Ticks++;
            //Tracker.Update();

            //MapJoystickToCursor();
            MapRotationToCursor();
        }

        static void MapJoystickToCursor()
        {
            var joystickX = VRPN.vrpnAnalog("PPT_WAND1@192.168.0.201:8945", 0);
            var joystickY = VRPN.vrpnAnalog("PPT_WAND1@192.168.0.201:8945", 1);

            var percentX = (joystickX + 1d) * 50d;
            var percentY = (joystickY + 1d) * 50d;

            int posX = Convert.ToInt32(Convert.ToDouble(ScreenDimensions.Width) * percentX / 100d);
            int posY = Convert.ToInt32(Convert.ToDouble(ScreenDimensions.Height) * percentY / 100d);

            Cursor.Position = new Point(posX, posY);
        }

        static void MapRotationToCursor()
        {
            var yaw = VRPN.Yaw("PPT0@192.168.0.201", 0);
            var pitch = VRPN.Pitch("PPT0@192.168.0.201", 0);

            var percentX = -1d / 0.75d * yaw + 1.25d;
            var percentY = 1 - (pitch + 0.5d);

            Console.WriteLine("pitch: " + pitch);
            Console.WriteLine("percent: " + percentY);

            int posX = Convert.ToInt32(Convert.ToDouble(ScreenDimensions.Width) * percentX);
            int posY = Convert.ToInt32(Convert.ToDouble(ScreenDimensions.Height) * percentY);

            Cursor.Position = new Point(posX, posY);
        }
    }
}
