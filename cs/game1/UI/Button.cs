using Raylib_cs;

namespace Game1.UI
{
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

        public void SetLabel(string label)
        {
            Label = label;
        }

        public void Draw()
        {
            Raylib.DrawRectangleLines(this.X, this.Y, this.W, this.H, Color.White);
            Raylib.DrawText(this.Label, this.X + 10, this.Y + 10, TextSize.GetInstance().Normal, Color.White);
        }
    }
}