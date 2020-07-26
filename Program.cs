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
        static int backID = 3;
        static int crusherID = 4;

        // Set up our drive train
        static OmniDrive omniDrive = new OmniDrive(upperRightID, upperLeftID, backID);
        
        // Set up our crusher
        static Crusher crusher = new Crusher(crusherID);

        // Set up the controller
        static GameController gamepad = new GameController(UsbHostDevice.GetInstance());

        // Set up the LEDs
        static LEDStripController strip = new LEDStripController(new CTRE.HERO.Port3Definition());
        
        // String builder for debug messages
        static StringBuilder debugMessage = new StringBuilder();

        public static void Main()
        {
            strip.Red = 0;
            strip.Blue = 0;
            strip.Grn = 1;
            strip.Process();
            while (true)
            {
                // Drive the robot using the gamepad
                omniDrive.SetDemand(-gamepad.GetAxis(1), gamepad.GetAxis(0), gamepad.GetAxis(4));

                // Crusher
                crusher.SetDemand((gamepad.GetAxis(3) > 0.1 ? true : false));

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
