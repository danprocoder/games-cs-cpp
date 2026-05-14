namespace Game1
{
    class Vector
    {
        public float x { set; get; }
        public float y { set; get; }

        public Vector(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public Vector()
        {
            this.x = 0f;
            this.y = 0f;
        }

        public Vector Normalize()
        {
            float mag = Magnitude();
            return new Vector(x / mag, y / mag);
        }

        public float Magnitude()
        {
            return (float) Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
        }

        public Vector Multiply(float scalar)
        {
            return new Vector(this.x * scalar, this.y * scalar);
        }
    }
}
