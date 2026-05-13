using System;
using System.Numerics;
using Raylib_cs;

namespace Game1
{
    class Rocket
    {
        private int level = 1;

        private float LaunchCost = 0f;
        public float FuelLevel { get; private set; } = 100f;
        private float speed = 8f;

        private float distanceToTarget = 0f;

        public String State { get; private set; } = "idle";

        private int LaunchStart = 0;

        private float targetX = 0f;
        private float targetY = 0f;

        private Vector v = new Vector(0f, 0f);

        private float posX = 400f;
        private float posY = 540f;

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

        public void Launch()
        {
            Vector n = new Vector(targetX - posX, targetY - posY).Normalize();
            v = new Vector(n.x * speed, n.y * speed);
            Console.WriteLine("Launching rocket at {0}, {1}", v.x, v.y);
        }

        public void Update()
        {
            posX += v.x;
            posY += v.y;

            this.CalculateDistanceToTarget();
        }

        public void Upgrade()
        {
            this.level += 1;
            this.speed = (float)Math.Pow(13f, 0.4f*level);
        }

        public float GetSpeed()
        {
            return speed;
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
        }
    }
}
