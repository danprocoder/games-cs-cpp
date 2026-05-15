using System;
using System.Numerics;
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

        private readonly float ClickEarnable = 0.35f;

        private List<Engineer> engineers = new List<Engineer>();

        public Factory()
        {
            rocket = new Rocket();

            engineers.Add(new Engineer());
            engineers.Add(new Engineer());
            engineers.Add(new Engineer());
        }

        public void Earn(float income = 1f)
        {
            this.Income += income;
        }

        public float CalculatePassiveIncome()
        {
            // Calculate average of skill level
            int totalSkillLevel = 0;
            int numberOfEngineers = this.engineers.Count;

            foreach (var engineer in engineers)
            {
                totalSkillLevel += engineer.SkillLevel;
            }
            float avg = (float)totalSkillLevel / (float)numberOfEngineers;

            return (float)Math.Pow(avg, 0.03) * this.GetLevel();
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

            int newHire = (int) Math.Pow(2, 0.4 * this.GetLevel());
            
            for (int i = 0; i < newHire; i++)
            {
                this.engineers.Add(new Engineer());
            }
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
                this.Earn(this.CalculatePassiveIncome());
                this.LastTimeMs = currentMs;
            }

            rocket.Update();
        }

        public void Draw()
        {
            DrawWarehouse();
            rocket.Draw();
        }

        // Draws a warehouse/hangar building around the rocket's base position
        private void DrawWarehouse()
        {
            // The rocket sits at (400, 540) with width=50, height=50.
            // We build a warehouse centred on x=400, with the floor at y=595.

            int cx = 400;          // Horizontal centre of the warehouse
            int floorY = 595;      // Ground / floor y-coordinate

            // --- Main building body ---
            int buildingW = 260;
            int buildingH = 130;
            int buildingX = cx - buildingW / 2;  // 270
            int buildingY = floorY - buildingH;  // 465

            // Filled wall (dark steel-grey)
            Color wallColor   = new Color(55, 65, 75, 255);
            Color accentColor = new Color(80, 95, 110, 255);
            Color roofColor   = new Color(40, 50, 60, 255);
            Color doorColor   = new Color(20, 25, 30, 255);
            Color windowColor = new Color(100, 160, 200, 180);
            Color lineColor   = new Color(120, 135, 150, 255);

            Raylib.DrawRectangle(buildingX, buildingY, buildingW, buildingH, wallColor);

            // --- Pitched roof ---
            // Triangle sitting on top of the building
            int roofPeak = buildingY - 45;
            Vector2 roofLeft  = new Vector2(buildingX - 10, buildingY);
            Vector2 roofRight = new Vector2(buildingX + buildingW + 10, buildingY);
            Vector2 roofTip   = new Vector2(cx, roofPeak);
            Raylib.DrawTriangle(roofTip, roofLeft, roofRight, roofColor);
            // Outline the roof
            Raylib.DrawLine((int)roofLeft.X, (int)roofLeft.Y, (int)roofTip.X, (int)roofTip.Y, lineColor);
            Raylib.DrawLine((int)roofRight.X, (int)roofRight.Y, (int)roofTip.X, (int)roofTip.Y, lineColor);
            Raylib.DrawLine((int)roofLeft.X, (int)roofLeft.Y, (int)roofRight.X, (int)roofRight.Y, lineColor);

            // --- Roof ridge vent strip ---
            Raylib.DrawRectangle(cx - 20, roofPeak - 8, 40, 10, accentColor);
            Raylib.DrawRectangleLines(cx - 20, roofPeak - 8, 40, 10, lineColor);

            // --- Central hangar door opening ---
            int doorW = 80;
            int doorH = buildingH;          // Full height of the wall
            int doorX = cx - doorW / 2;
            // Draw a dark rectangle — the open door
            Raylib.DrawRectangle(doorX, buildingY, doorW, doorH, doorColor);
            // Door frame lines
            Raylib.DrawLine(doorX,          buildingY, doorX,          floorY, lineColor);
            Raylib.DrawLine(doorX + doorW,  buildingY, doorX + doorW,  floorY, lineColor);
            Raylib.DrawLine(doorX,          buildingY, doorX + doorW,  buildingY, lineColor);

            // Horizontal panel lines across the door
            for (int py = buildingY + 30; py < floorY; py += 30)
            {
                Raylib.DrawLine(doorX, py, doorX + doorW, py, accentColor);
            }

            // --- Left side window ---
            int winW = 40;
            int winH = 28;
            int winLX = buildingX + 15;
            int winY  = buildingY + 30;
            Raylib.DrawRectangle(winLX, winY, winW, winH, windowColor);
            Raylib.DrawRectangleLines(winLX, winY, winW, winH, lineColor);
            // Window cross-bar
            Raylib.DrawLine(winLX + winW/2, winY, winLX + winW/2, winY + winH, lineColor);
            Raylib.DrawLine(winLX, winY + winH/2, winLX + winW, winY + winH/2, lineColor);

            // --- Right side window ---
            int winRX = buildingX + buildingW - 15 - winW;
            Raylib.DrawRectangle(winRX, winY, winW, winH, windowColor);
            Raylib.DrawRectangleLines(winRX, winY, winW, winH, lineColor);
            Raylib.DrawLine(winRX + winW/2, winY, winRX + winW/2, winY + winH, lineColor);
            Raylib.DrawLine(winRX, winY + winH/2, winRX + winW, winY + winH/2, lineColor);

            // --- Horizontal cladding lines across the main wall ---
            for (int hy = buildingY + 25; hy < floorY; hy += 25)
            {
                // Skip the door area
                Raylib.DrawLine(buildingX, hy, doorX, hy, accentColor);
                Raylib.DrawLine(doorX + doorW, hy, buildingX + buildingW, hy, accentColor);
            }

            // --- Building outline ---
            Raylib.DrawRectangleLines(buildingX, buildingY, buildingW, buildingH, lineColor);

            // --- Ground / floor line ---
            Raylib.DrawLine(buildingX - 20, floorY, buildingX + buildingW + 20, floorY, lineColor);

            // --- Small chimney / vent on the left side of roof ---
            Raylib.DrawRectangle(buildingX + 30, buildingY - 20, 14, 22, accentColor);
            Raylib.DrawRectangleLines(buildingX + 30, buildingY - 20, 14, 22, lineColor);

            // --- "FACTORY" label on the building ---
            int ts = TextSize.GetInstance().Small;
            string label = "FACTORY";
            int labelW = Raylib.MeasureText(label, ts);
            Raylib.DrawText(label, cx - labelW / 2, buildingY + buildingH - 20, ts, lineColor);
        }
    }
}
