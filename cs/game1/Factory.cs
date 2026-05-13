using System;
using Raylib_cs;

namespace Game1
{
    class Factory
    {
        public float Income { set; get; } = 0f;

        private float UpgradeCost { set; get; } = (float)Math.Exp(1f);
        private int UpgradeLevel = 1;

        private Rocket rocket;

        private int LastTimeMs = Environment.TickCount;
        
        private readonly float AutoEarnable = 0.02f;
        private readonly float ClickEarnable = 0.35f;

        public Factory()
        {
            rocket = new Rocket();
        }

        public void Earn(float income = 1f)
        {
            this.Income += income;
        }

        public int GetLevel()
        {
            return this.UpgradeLevel;
        }

        public float GetTotalUpgradeCost()
        {
            return this.UpgradeCost;
        }

        public void Upgrade()
        {
            this.Income -= this.UpgradeCost;
            this.UpgradeLevel++;
            this.UpgradeCost = (float)Math.Exp(this.UpgradeLevel);
        }

        public Rocket GetRocket()
        {
            return rocket;
        }


        public void UpgradeRocket()
        {
            this.Income -= this.rocket.GetUpgradeCost();
            rocket.Upgrade();
        }

        public void UpdateEvent()
        {
            if (Raylib.IsMouseButtonPressed(MouseButton.Left) && rocket.IsIdle())
            {
                this.Earn((float) ClickEarnable);
            }
        }

        public void Update()
        {
            int currentMs = Environment.TickCount;
            if (currentMs - this.LastTimeMs > 1000)
            {
                this.Earn((float) Math.Pow(this.AutoEarnable, this.GetLevel()));
                this.LastTimeMs = currentMs;
            }

            rocket.Update();
        }

        public void Draw()
        {
            rocket.Draw();
        }
    }
}
