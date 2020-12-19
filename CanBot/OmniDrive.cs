using CTRE.Phoenix.MotorControl;
using CTRE.Phoenix.MotorControl.CAN;
using Microsoft.SPOT;


namespace CanBot
{
    /// <summary>
    /// This class handles the operation of a 3 wheeled omni drive,
    /// also know as a 'Kiwi' drive. This means that the wheels
    /// are placed on a circle at 120 degrees from each other.
    /// </summary>
    class OmniDrive
    {
        // Victor SPX Objects
        private VictorSPX upperRight, upperLeft, back;

        // For a given demand this matrix will project a vector in the form
        // [demandX, demandY, demandRotation] and maps it to 
        // [upperLeftWheelPower, upperRightWheelPower, backWheelPower]
        // Note that the input vectors is the demands relative to the robots
        // coordinate system (x is right, y is forward, and rotation is counter-clock-wise)
        private static double[] projectMatrix = new double[9];

        static OmniDrive()
        {

            // { System.Math.Sqrt(3) / 2, -1 / 2, 1 },
            // { -System.Math.Sqrt(3) / 2, 1 / 2, 1 },
            // { 0, -1, 1 }
            projectMatrix[0] = System.Math.Sqrt(3.0) / 2.0;
            projectMatrix[1] = -1.0 / 2.0;
            projectMatrix[2] = 1.0;
            projectMatrix[3] = -System.Math.Sqrt(3.0) / 2.0;
            projectMatrix[4] = 1.0 / 2.0;
            projectMatrix[5] = 1.0;
            projectMatrix[6] = 0.0;
            projectMatrix[7] = -1.0;
            projectMatrix[8] = 1.0;

        }
        /// <summary>
        /// Define an omni drive with the ID's of the motor controller
        /// </summary>
        /// <param name="upperRightID">The id of the upper right wheel motor controller</param>
        /// <param name="upperLeftID">The id of the upper left wheel motor controller</param>
        /// <param name="backID">The id of the back motor controller</param>
        public OmniDrive(int upperRightID, int upperLeftID, int backID)
        {
            upperRight = new VictorSPX(upperRightID);
            upperLeft = new VictorSPX(upperLeftID);
            back = new VictorSPX(backID);

            upperRight.ConfigFactoryDefault();
            upperLeft.ConfigFactoryDefault();
            back.ConfigFactoryDefault();
        }

        /// <summary>
        /// Calculate the needed wheel demand to go in the direction and scale of the 
        /// demand vector. Note that this represents the scaling of the speed of each 
        /// wheel not an absolute speed.
        /// </summary>
        /// <param name="demandVector">The demand vector represented 
        /// as a length 3 array of doubles</param>
        /// <returns></returns>
        private double[] CalculateDemand(double[] demandVector)
        {
            // This just simple matrix multiplication. projectMatrix * demand_matrix = wheel_matrix
            double[] wheelVector = new double[3];

            // Solve for upper left wheel
            wheelVector[0] = projectMatrix[0] * demandVector[0] + projectMatrix[1] * demandVector[1] + projectMatrix[2] * demandVector[2];

            // Solve for upper right wheel
            wheelVector[1] = projectMatrix[3] * demandVector[0] + projectMatrix[4] * demandVector[1] + projectMatrix[5] * demandVector[2];

            // Solve for back wheel
            wheelVector[2] = projectMatrix[6] * demandVector[0] + projectMatrix[7] * demandVector[1] + projectMatrix[8] * demandVector[2];

            return wheelVector;
        }

        /// <summary>
        /// Set the wheel demand based a demand in the x direction
        /// y direction and some rotational demand.
        /// </summary>
        /// <param name="dX">Demand in the X direction</param>
        /// <param name="dY">Demand in the Y direction</param>
        /// <param name="dR">Rotation demand</param>
        public void SetDemand(double dX, double dY, double dR)
        {
            double[] demand = { dX, dY, dR };
            double[] wheelDemand = CalculateDemand(demand);
            Debug.Print("Upper Left: " + wheelDemand[0] + " Upper Right: " + wheelDemand[1] + " Back: " + wheelDemand[2] + "\n");
            upperLeft.Set(ControlMode.PercentOutput, wheelDemand[0]);
            upperRight.Set(ControlMode.PercentOutput, wheelDemand[1]);
            back.Set(ControlMode.PercentOutput, wheelDemand[2]);
        }
    }
} // namespace CanBot