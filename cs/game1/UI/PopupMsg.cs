using Raylib_cs;

namespace Game1.UI
{
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
}
