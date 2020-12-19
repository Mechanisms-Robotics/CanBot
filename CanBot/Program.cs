using System;
using System.Threading;
using System.Text;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

using CTRE.Phoenix;
using CTRE.Phoenix.Controller;
using CTRE.HERO;

namespace CanBot
{
    public class Program
    {
        // Some constants for the robot
        // static double DEADBAND = 0.10;
        static int upperRightID = 2;
        static int upperLeftID = 1;
        static int backID = 3; // 3
        static int crusherID = 4; // 4

        // Set up our drive train
        static OmniDrive omniDrive = new OmniDrive(upperRightID, upperLeftID, backID);
        
        // Set up our crusher
        static Crusher crusher = new Crusher(crusherID);

        // Set up the controller
        static GameController gamepad = new GameController(UsbHostDevice.GetInstance());

        
        // String builder for debug messages
        static StringBuilder debugMessage = new StringBuilder();

        static bool eanbled = true;

        public static void Main()
        {
            Debug.Print("Starting CanBot Programe. v0.1\n");
            while (true)
            {

                Debug.Print("Enable Button: " + gamepad.GetButton(1) + "\n");
                if (gamepad.GetConnectionStatus() == UsbDeviceConnection.Connected && eanbled)
                {
                    // Drive the robot using the gamepad
                    double dX = -gamepad.GetAxis(1);
                    double dY = gamepad.GetAxis(0);
                    double dR = gamepad.GetAxis(2);
                    Debug.Print("X Axis: " + dX + " Y Axis: " + dY + " Rotation Axis: " + dR + "\n");
                    omniDrive.SetDemand(dX, dY, dR);

                    // Crusher
                    //crusher.SetDemand((gamepad.GetAxis(3) > 0.1 ? true : false));

                    // Print any debug messages
                    Debug.Print(debugMessage.ToString());
                    debugMessage.Clear();

                    // Feed the watchdog to keep the Talon's enabled
                    CTRE.Phoenix.Watchdog.Feed();

                    // Only update this loop every 20ms
                    Thread.Sleep(20);
                }
            }
        }
    }
}
