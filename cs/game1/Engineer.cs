namespace Game1
{
    class Engineer
    {
        public int SkillLevel { private set; get; }

        public Engineer()
        {
            Random random = new Random();
            this.SkillLevel = random.Next(1, 5);
        }
    }
}
