using System;
using System.Numerics;
using Raylib_cs;

namespace Game1
{
    public delegate void RocketReachedTargetCallback();

    class Rocket
    {
        private int level = 1;

        private float LaunchCost = 0f;

        public float fuelLevel { get; private set; } = 12;
        public float burnRate { get; private set; } = 0.34f; // Per m

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

        private RocketReachedTargetCallback? targetReachedCallback;

        private int launchStartMs = 0;

        private Random rng = new Random();

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

        public void Launch(RocketReachedTargetCallback targetReachedCallback)
        {
            initialV = new Vector(targetX - posX, targetY - posY).Normalize();

            this.state = "launching";
            this.currentSpeed = 0;

            this.targetReachedCallback = targetReachedCallback;

            this.launchStartMs = Environment.TickCount;
        }

        public void Update()
        {
            if (IsLaunching())
            {
                currentSpeed = acceleration * ((Environment.TickCount - launchStartMs) / 1000);
                v = new Vector(initialV.x * currentSpeed, initialV.y * currentSpeed);
            }

            posX += v.x;
            posY += v.y;
            this.CalculateDistanceToTarget();

            if (distanceToTarget <= 8f && state.Equals("launching"))
            {
                v = new Vector(0f, 0f);
                targetReachedCallback();

                state = "Returning-to-base";
            }
        }

        public void ReturnToBase()
        {

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

        private void CalculateDistanceToTarget()
        {
            this.distanceToTarget = (float) Math.Sqrt(Math.Pow(this.targetY - this.posY, 2) + Math.Pow(this.targetX - this.posX, 2));
        }

        public void Draw()
        {
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
            Vector2 v2 = new Vector2(posX - 25, posY + 50); // Bottom-left vertex
            Vector2 v3 = new Vector2(posX + 25, posY + 50); // Bottom-right vertex

            Raylib.DrawTriangle(v1, v2, v3, Color.Red);

            if (IsLaunching() || IsReturningToBase())
            {
                DrawFlame();
            }
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
