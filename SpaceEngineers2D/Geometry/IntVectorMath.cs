﻿namespace SpaceEngineers2D.Geometry
{
    using System;

    public static class IntVectorMath
    {
        public static IntVector MinMax(IntVector a, IntVector max, IntVector min)
        {
            return Min(min, Max(max, a));
        }

        public static IntVector Min(IntVector a, IntVector b)
        {
            return new IntVector(Math.Min(a.X, b.X), Math.Min(a.Y, b.Y));
        }

        public static IntVector Max(IntVector a, IntVector b)
        {
            return new IntVector(Math.Max(a.X, b.X), Math.Max(a.Y, b.Y));
        }
    }
}
