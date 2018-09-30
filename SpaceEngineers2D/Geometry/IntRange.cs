namespace SpaceEngineers2D.Geometry
{
    public struct IntRange
    {
        public int Min { get; }

        public int Max { get; }

        public IntRange(int min, int max)
        {
            Min = min;
            Max = max;
        }

        public bool Touches(int value)
        {
            return value >= Min && value <= Max;
        }

        public bool Contains(int value)
        {
            return value > Min && value < Max;
        }

        public bool Intersects(IntRange other)
        {
            if (other.Min >= Max && other.Max >= Max)
            {
                return false;
            }

            if (other.Max <= Min && other.Min <= Min)
            {
                return false;
            }

            return true;
        }
    }
}
