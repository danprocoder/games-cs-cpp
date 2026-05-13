using System;
using System.Threading;

namespace Game1
{
    class Game1
    {
        private int LastTimeMs { set; get; } = 0;
        private Factory factory;

        private readonly float AutoEarnable = 0.02f;
        private readonly float ClickEarnable = 0.35f;

        public Game1()
        {
            this.LastTimeMs = DateTime.Now.Millisecond;
            this.factory = new Factory();
        }

        void AutoIncreaseIncome()
        {
            int currentMs = Environment.TickCount;

            if ((currentMs - this.LastTimeMs) > 1000)
            {
                this.factory.Earn((float) Math.Pow(this.AutoEarnable, this.factory.GetLevel()));
                this.LastTimeMs = currentMs;
            }
        }

        void Play()
        {
            string userInput = "";
            while (true)
            {
                this.AutoIncreaseIncome();

                Console.Clear();
                Console.WriteLine(
                    "Total income: [{0}], Lvl: [{1}], Upgrade Cost: [{2}]\n",
                    factory.Income,
                    factory.GetLevel(),
                    factory.GetTotalUpgradeCost()
                );
                Console.Write("Enter something: " + userInput);

                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.Enter)
                    {
                        string cmd = userInput.ToLower();
                        userInput = "";
                        if (cmd.Equals("exit")) break;
                        else if (cmd.Equals("click"))
                            this.factory.Earn(this.ClickEarnable);
                        else if (cmd.Equals("factory -upgrade"))
                            this.factory.Upgrade();
                        else if (cmd.Equals("rocket -launch"))
                            this.factory.LaunchRocket();
                        else if (cmd.Equals("rocket -upgrade"))
                            this.factory.UpgradeRocket();
                    }
                    else if (key.Key == ConsoleKey.Backspace)
                    {
                        if (userInput.Length > 0)
                            userInput = userInput[..^1];
                    }
                    else
                    {
                        userInput += key.KeyChar;
                    }
                }

                Thread.Sleep(100);
            }
        }

        static void Main(string[] args)
        {
            Game1 game = new Game1();
            game.Play();
        }
    }
}
