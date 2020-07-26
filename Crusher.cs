using System;
using CTRE.Phoenix.MotorControl.CAN;
using Microsoft.SPOT;

namespace CanBot
{
    /// <summary>
    /// This class handles the crusher subsystem
    /// for crushing.
    /// </summary>
    class Crusher
    {
        //Victor SPX Object
        private VictorSPX crusher;

        public Crusher(int victorID)
        {
            crusher = new VictorSPX(victorID);
            crusher.ConfigFactoryDefault();
        }

        /// <summary>
        /// Sets the crusher demand as a boolean where true is 100%
        /// power and false is 0% power
        /// </summary>
        /// <param name="run">Whether to run or not</param> 
        public void SetDemand(bool run)
        {
            crusher.Set(CTRE.Phoenix.MotorControl.ControlMode.PercentOutput, (run ? 1.0 : 0.0));
        }
    }
}
