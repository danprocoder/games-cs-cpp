namespace Game1.UI
{
    class TextSize
    {
        public int XLarge { get; private set; } = 1;
        public int Large { get; private set; } = 2;
        public int Normal { get; private set; } = 3;
        public int Small { get; private set; } = 4;
        public int XSmall { get; private set; } = 5;

        private readonly float TextScale = 1.25f;

        private static TextSize? Instance = null;

        private TextSize(int baseSize)
        {
            Large = (int) (baseSize * TextScale);
            XLarge = (int) (Large * TextScale);
            Normal = baseSize;
            Small = (int) (Normal / TextScale);
            XSmall = (int) (Small / TextScale);
        }

        public static TextSize GetInstance()
        {
            Instance ??= new TextSize(13);
            return Instance;
        }
    }
}
