using System;
using Raylib_cs;

namespace Game1
{
    class TextSize
    {
        public int Large { get; private set; } = 1;
        public int Normal { get; private set; } = 2;
        public int Small { get; private set; } = 3;
        public int XSmall { get; private set; } = 4;

        private readonly float TextScale = 1.25f;

        private static TextSize? Instance = null;

        private TextSize(int baseSize)
        {
            Large = (int) (baseSize * TextScale);
            Normal = baseSize;
        }

        public static TextSize GetInstance()
        {
            Instance ??= new TextSize(13);
            return Instance;
        }
    }

    class PopupMsg
    {
        private string Msg;
        private int CreatedAt;
        private int Level;

        public PopupMsg(string msg, int level)
        {
            Msg = msg;
            CreatedAt = Environment.TickCount;
            Level = level;
        }

        public bool IsExpired()
        {
            return ((Environment.TickCount - CreatedAt) / 1000) > 3;
        }
        
        public void Draw()
        {
            int ts = TextSize.GetInstance().Normal;
            int tw = Raylib.MeasureText(Msg, ts);
            Raylib.DrawText(Msg, 400 - tw/2, 300, ts, Level == 0 ? Color.Red : Color.White);
        }
    }

    class Button
    {
        private int X;   
        private int Y;   
        private int W;   
        private int H;
        private string Label;

        public Button(int x, int y, string label)
        {
            this.X = x;
            this.Y = y;
            this.Label = label;

            int tw = Raylib.MeasureText(label, TextSize.GetInstance().Normal);
            this.W = tw + 20;
            this.H = 40;
        }

        public bool IsPressed()
        {
            return Raylib.IsMouseButtonPressed(MouseButton.Left) && 
                   Raylib.GetMouseX() > this.X && 
                   Raylib.GetMouseX() < this.X + this.W && 
                   Raylib.GetMouseY() > this.Y && 
                   Raylib.GetMouseY() < this.Y + this.H;
        }

        public void Draw()
        {
            Raylib.DrawRectangleLines(this.X, this.Y, this.W, this.H, Color.White);
            Raylib.DrawText(this.Label, this.X + 10, this.Y + 10, TextSize.GetInstance().Normal, Color.White);
        }
    }

    class Game
    {
        protected Factory Factory;

        private string? launchState { get; set; } = null;

        private int[][] stars = new int[100][];

        private PopupMsg? msg;

        private Button upgradeFactoryBtn = new Button(670, 450, "Upgrade factory");
        private Button upgradeRocketBtn = new Button(670, 500, "Upgrade rocket");

        public Game()
        {
            Factory = new Factory();

            Random rnd = new Random();
            for (int i = 0; i < 100; i++)
            {
                stars[i] = new int[] { rnd.Next(0, 800), rnd.Next(0, 600) };
            }
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
                        Rocket rocket = Factory.GetRocket();
                        float launchCost = rocket.GetLaunchCost();
                        if (launchCost > Factory.Income)
                        {
                            msg = new PopupMsg("Income is not enough to launch", 0);
                        }
                        else
                        {
                            this.launchState = "Launching";
                            Factory.Income -= rocket.GetLaunchCost();
                            rocket.Launch(onRocketReachedTarget, onRocketReturnedToBase);
                        }
                    }
                }
                else if (upgradeFactoryBtn.IsPressed() && Factory.GetRocket().IsIdle())
                {
                    if (Factory.Income < Factory.GetTotalUpgradeCost())
                    {
                        msg = new PopupMsg("Income is not enough to upgrade factory", 0);
                    }
                    else
                    {
                        Factory.Upgrade();
                    }
                }
                else if (upgradeRocketBtn.IsPressed() && Factory.GetRocket().IsIdle())
                {
                    if (Factory.Income < Factory.GetRocket().GetUpgradeCost())
                    {
                        msg = new PopupMsg("Income is not enough to upgrade rocket", 0);
                    }
                    else
                    {
                        Factory.UpgradeRocket();
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
            Factory.Earn(1f); // TODO: Earning needs to be exponential based on rocket level.
        }

        public void onRocketReturnedToBase()
        {
            this.launchState = null;
        }

        public void DrawTexts()
        {
            Rocket rocket = this.Factory.GetRocket();

            TextSize ts = TextSize.GetInstance();
            
            // Texts at the top of the screen
            Raylib.DrawText("Total income: $" + Math.Round(this.Factory.Income, 2), 10, 10, ts.Large, Color.White);
            Raylib.DrawText("Level: " + this.Factory.GetLevel(), 10, 40, ts.Large, Color.White);
            Raylib.DrawText("Upgrade cost: $" + Math.Round(this.Factory.GetTotalUpgradeCost(), 2), 10, 70, ts.Large, Color.White);
            
            // Top right
            string rocketText = "Rocket level: " + rocket.GetLevel();
            Raylib.DrawText(rocketText, 800 - Raylib.MeasureText(rocketText, ts.Large) - 10, 10, ts.Large, Color.White);
            
            string launchCost = "Launch cost: $" + Math.Round(rocket.GetLaunchCost(), 2);
            Raylib.DrawText(launchCost, 800 - Raylib.MeasureText(launchCost, ts.Large), 35, ts.Large, Color.White);


            if (launchState != null)
            {
                if (launchState.Equals("SelectTarget"))
                {
                    int tw = Raylib.MeasureText("Click to select a target.", 20);
                    Raylib.DrawText("Click to select a target.", 400 - (tw/2), 300, 20, Color.Green);
                    Raylib.DrawRectangleLines(400 - (tw/2) - 10, 300 - 10, tw + 20, 40, Color.Green);
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
            DrawStars();

            Factory.Draw();

            DrawTexts();

            this.upgradeFactoryBtn.Draw();
            this.upgradeRocketBtn.Draw();
            DrawLaunchButton();

            if (msg != null)
            {
                msg.Draw();

                if (msg.IsExpired())
                {
                    msg = null;
                }
            }
        }

        public void DrawStars()
        {
            for (int i = 0; i < stars.Length; i++)
            {
                Raylib.DrawPixel(stars[i][0], stars[i][1], Color.White);
            }
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
