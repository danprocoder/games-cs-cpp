using System;

namespace Game1
{
    class Rocket
    {
        private float LaunchCost = 0f;
        private float FuelLevel = 100f;
        private float Speed = 13f;

        private float DistanceToTarget = 0f;

        private String State = "idle";

        private int LaunchStart = 0;

        public void SetTarget(float Distance)
        {
            this.DistanceToTarget = Distance;
        }

        private float GetLaunchCost()
        {
            return this.LaunchCost;
        }

        private float GetCurrentDistanceToTarget()
        {
            // s = d/t, d = s*t, t=d/s
            int t = Environment.TickCount - this.LaunchStart;
            int distanceTravelled = this.Speed * (float) t;

            return this.DistanceToTarget - distanceTravelled;
        }

        private void Launch()
        {
            if (this.DistanceToTarget == 0f)
            {
                throw new Exception("Distance not locked in yet.");
            }

            this.State = "launching";
            this.LaunchStart = Environment.TickCount;

            Console.WriteLine("Distance to target: [{0}]km", this.GetCurrentDistanceToTarget());
        }
    }
}
