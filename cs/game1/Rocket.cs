using System;
using System.Numerics;
using Raylib_cs;

namespace Game1
{
    public delegate void RocketReachedTargetCallback();

    public delegate void RocketReturnedToBaseCallback();

    class Rocket
    {
        private int level = 1;

        private float LaunchCost = 0f;

        public readonly float fuelCapacity = 10f;
        public float totalFuel { get; set; } = 10f;
        public float fuelLevel { get; private set; } = 10f;
        public float burnRate { get; private set; } = 0.34f; // Per s

        private float initialSpeed = 0f;
        private float currentSpeed = 0f;
        private float acceleration = 0.25f; // The rate at which the velocity is changing

        private float distanceToTarget = 0f;

        private String state = "idle";

        private int LaunchStart = 0;

        private float targetX = 0f;
        private float targetY = 0f;

        private Vector v = new Vector(0f, 0f);
        private Vector initialV = new Vector(0f, 0f);

        private float posX = 400f;
        private float posY = 540f;
        private float initialPosX = 400f;
        private float initialPosY = 540f;

        private RocketReachedTargetCallback? targetReachedCallback;
        private RocketReturnedToBaseCallback? returnedToBaseCallback;

        private int launchStartMs = 0;
        private int returnTimeStartMs = 0;

        private int w = 50;
        private int h = 50;

        private Random rng = new Random();

        public Rocket()
        {
            LaunchCost = (float) Math.Exp(level);
        }

        public int GetLevel()
        {
            return level;
        }

        public void SetTarget(float x, float y)
        {
            this.targetX = x;
            this.targetY = y;

            this.CalculateDistanceToTarget();
        }

        public float GetLaunchCost()
        {
            return this.LaunchCost;
        }

        public float GetUpgradeCost()
        {
            return 100f * this.level;
        }

        public void Launch(RocketReachedTargetCallback targetReachedCallback,
                           RocketReturnedToBaseCallback returnedToBaseCallback)
        {
            initialV = new Vector(targetX - posX, targetY - posY).Normalize();

            this.state = "launching";
            this.initialSpeed = 0;
            this.currentSpeed = 0;

            this.targetReachedCallback = targetReachedCallback;
            this.returnedToBaseCallback = returnedToBaseCallback;

            this.launchStartMs = Environment.TickCount;
        }

        public void Update()
        {
            // If it runs out of fuel in the middle of a flight, it needs to crash
            if (IsInFlight() && GetFuelPercentLeft() == 0)
            {
                state = "Returning-to-base";
            }

            int secsSinceLaunch = (Environment.TickCount - launchStartMs) / 1000;
            if (IsLaunching())
            {
                currentSpeed = acceleration * secsSinceLaunch;
                v = initialV.Multiply(currentSpeed);
                
                fuelLevel = totalFuel - burnRate*secsSinceLaunch;
            }
            else if (IsReturningToBase())
            {
                int secsSinceReturnStart = (Environment.TickCount - returnTimeStartMs) / 1000;
                currentSpeed = (float)Math.Max(0.25, initialSpeed - acceleration * secsSinceReturnStart);

                v = initialV.Multiply(currentSpeed);
                
                fuelLevel = totalFuel - burnRate*secsSinceLaunch;
            }

            posX += v.x;
            posY += v.y;
            this.CalculateDistanceToTarget();

            if (distanceToTarget <= 8f && state.Equals("launching"))
            {
                initialV = new Vector(initialPosX - posX, initialPosY - posY).Normalize();
                targetReachedCallback();

                state = "Returning-to-base";
                returnTimeStartMs = Environment.TickCount;
                initialSpeed = currentSpeed;
            }
            else if (GetDistanceToBase() <= 8f && state.Equals("Returning-to-base"))
            {
                state = "idle";
                totalFuel = fuelLevel;
                initialV = new Vector();
                v = new Vector();

                if (returnedToBaseCallback != null)
                {
                    returnedToBaseCallback();
                }
            }
        }

        public bool IsIdle()
        {
            return state.Equals("idle");
        }

        public bool IsLaunching()
        {
            return state.Equals("launching");
        }

        public bool IsReturningToBase()
        {
            return state.Equals("Returning-to-base");
        }

        public bool IsInFlight()
        {
            return state == "launching" || state == "Returning-to-base";
        }

        public void Upgrade()
        {
            this.level += 1;
        }

        public float GetSpeed()
        {
            return currentSpeed;
        }

        public float GetDistanceToTarget()
        {
            return distanceToTarget;
        }

        public float GetFuelPercentLeft()
        {
            return (float) Math.Max(0, fuelLevel / fuelCapacity * 100);
        }

        private void CalculateDistanceToTarget()
        {
            this.distanceToTarget = (float) Math.Sqrt(Math.Pow(this.targetY - this.posY, 2) + Math.Pow(this.targetX - this.posX, 2));
        }

        private float GetDistanceToBase()
        {
            return (float) Math.Sqrt(Math.Pow(this.initialPosX - this.posX, 2) + Math.Pow(this.initialPosY - this.posY, 2));
        }

        public void Draw()
        {
            if ((IsLaunching() || IsReturningToBase()) && GetFuelPercentLeft() > 0)
            {
                DrawFlame();
            }

            if (this.targetX != 0f && this.targetY != 0f)
            {
                int tx = (int)this.targetX;
                int ty = (int)this.targetY;
                
                // Draw target crosshairs and circles
                Raylib.DrawCircleLines(tx, ty, 15, Color.Red);
                Raylib.DrawCircleLines(tx, ty, 5, Color.Red);
                Raylib.DrawLine(tx - 20, ty, tx + 20, ty, Color.Red);
                Raylib.DrawLine(tx, ty - 20, tx, ty + 20, Color.Red);
            }

            Vector2 v1 = new Vector2(posX, posY); // Top vertex
            Vector2 v2 = new Vector2(posX - w/2, posY + h); // Bottom-left vertex
            Vector2 v3 = new Vector2(posX + w/2, posY + h); // Bottom-right vertex

            Raylib.DrawTriangle(v1, v2, v3, Color.Red);

            DrawCockpit();
        }

        private void DrawCockpit()
        {
            TextSize ts = TextSize.GetInstance();

            if (IsLaunching()) {
                string distanceText = "Distance to target: " + Math.Round(GetDistanceToTarget(), 2) + "m";
                Raylib.DrawText(distanceText, 20, 500, ts.Normal, Color.Green);
            }

            if (IsLaunching() || IsReturningToBase())
            {
                string speedText = "Speed: " + Math.Round(GetSpeed(), 2) + "m/s";
                Raylib.DrawText(speedText, 20, 530, ts.Normal, Color.Green);
            }

            string fuelLeftText = "Fuel left: " + Math.Round(GetFuelPercentLeft(), 2) + "%";
            Raylib.DrawText(fuelLeftText, 20, 560, ts.Normal, Color.Green);
        }

        private void DrawFlame()
        {
            // Flame flickers by randomising the tip position slightly each frame
            float flickerX = posX + (float)(rng.NextDouble() - 0.5) * 10f;
            float flickerY = posY + 50f + 20f + (float)(rng.NextDouble() * 15f);

            Vector2 f1 = new Vector2(flickerX, flickerY);        // Tip (bottom, flickering)
            Vector2 f2 = new Vector2(posX - 15, posY + 50f);     // Top-left (base of rocket)
            Vector2 f3 = new Vector2(posX + 15, posY + 50f);     // Top-right (base of rocket)

            Raylib.DrawTriangle(f2, f1, f3, Color.Orange);
            Raylib.DrawTriangle(f2, new Vector2(flickerX - 5f, flickerY - 10f), f3, Color.Yellow);
        }
    }
}
