using System;

namespace Game1
{
    class Factory
    {
        public float Income { set; get; } = 0f;

        private float UpgradeCost { set; get; } = (float)Math.Exp(1f);
        private int UpgradeLevel = 1;

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

        public void LaunchRocket()
        {

        }

        public void UpgradeRocket()
        {
            
        }
    }
}
