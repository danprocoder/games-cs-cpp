using System;
using Raylib_cs;

namespace Game1
{
    class Game
    {
        protected Factory Factory;

        private string? launchState { get; set; } = null;

        public Game()
        {
            Factory = new Factory();
        }

        public void Update()
        {
            Factory.UpdateEvent();
            Factory.Update();
        }

        public void HandleEvents()
        {
            if (Raylib.IsMouseButtonPressed(MouseButton.Left))
            {
                int x = Raylib.GetMouseX();
                int y = Raylib.GetMouseY();

                if (x > 670 && y > 550)
                {
                    if (this.launchState == null)
                    {
                        this.launchState = "SelectTarget";
                    }
                    else if (this.launchState.Equals("SelectTarget"))
                    {
                        this.launchState = "Launching";
                        Factory.GetRocket().Launch(this.onRocketReachedTarget);
                        Factory.Income -= Factory.GetRocket().GetLaunchCost();
                    }
                }
                else if (launchState != null && launchState.Equals("SelectTarget"))
                {
                    Factory.GetRocket().SetTarget(x, y);
                }
            }
        }

        public void onRocketReachedTarget()
        {
            Factory.Earn(1f);
            Factory.GetRocket().ReturnToBase();
        }

        public void DrawTexts()
        {
            // Texts at the top of the screen
            Raylib.DrawText("Total income: $" + Math.Round(this.Factory.Income, 2), 10, 10, 20, Color.White);
            Raylib.DrawText("Level: " + this.Factory.GetLevel(), 10, 40, 20, Color.White);
            Raylib.DrawText("Upgrade cost: $" + Math.Round(this.Factory.GetTotalUpgradeCost(), 2), 10, 70, 20, Color.White);
            string rocketText = "Rocket level: 0";
            Raylib.DrawText(rocketText, 800 - Raylib.MeasureText(rocketText, 20) - 10, 10, 20, Color.White);

            if (launchState != null)
            {
                if (launchState.Equals("SelectTarget"))
                {
                    int tw = Raylib.MeasureText("Click to select a target.", 20);
                    Raylib.DrawText("Click to select a target.", 400 - (tw/2), 300, 20, Color.Green);
                    Raylib.DrawRectangleLines(400 - (tw/2) - 10, 300 - 10, tw + 20, 40, Color.Green);
                }
                else if (launchState.Equals("Launching"))
                {
                    Rocket rocket = this.Factory.GetRocket();
                    Raylib.DrawText("Distance to target: " + Math.Round(rocket.GetDistanceToTarget(), 2) + "m", 400, 300, 12, Color.Green);
                    Raylib.DrawText("Speed: " + Math.Round(rocket.GetSpeed(), 2) + "m/s", 400, 340, 12, Color.Green);
                }
            }
        }

        public void DrawLaunchButton()
        {
            string text = "Launch rocket";
            if (this.launchState != null)
            {
                if (this.launchState.Equals("SelectTarget"))
                {
                    text = "Start";
                }
                else if (this.launchState.Equals("Launching"))
                {
                    Rocket rocket = Factory.GetRocket();
                    if (rocket.IsLaunching())
                    {
                        text = "Launching...";
                    }
                    else if (rocket.IsReturningToBase())
                    {
                        text = "Returning to base...";
                    }
                }
            }

            int tw = Raylib.MeasureText(text, 15);
            int btnWidth = tw + 20;
            int btnX = 800 - btnWidth - 20;
            Raylib.DrawRectangle(btnX, 550, btnWidth, 40, Color.Green);
            Raylib.DrawText(text, btnX + 10, 560, 15, Color.Black);
        }

        public void Draw()
        {
            Factory.Draw();

            DrawTexts();

            DrawLaunchButton();
        }

        public static void Main()
        {
            Raylib.InitWindow(800, 600, "Rocket Tycoon Calculator");
            Raylib.SetTargetFPS(60);

            Game game = new Game();

            while (!Raylib.WindowShouldClose())
            {
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.Black);

                game.HandleEvents();
                game.Update();
                game.Draw();
                
                Raylib.EndDrawing();
            }

            Raylib.CloseWindow();
        }
    }
}
