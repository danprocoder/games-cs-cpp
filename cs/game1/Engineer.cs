namespace Game1
{
    class Engineer
    {
        public int SkillLevel { private set; get; }

        private static Random random = new Random();

        public Engineer()
        {
            this.SkillLevel = random.Next(1, 5);
        }
    }
}
