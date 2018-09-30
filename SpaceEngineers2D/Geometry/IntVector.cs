using System;

namespace SpaceEngineers2D.Geometry
{
    using System.Windows;

    public struct IntVector
    {
        public static readonly IntVector Zero = new IntVector(0, 0);

        public static readonly IntVector RightBottom = new IntVector(1, 1);

        public static readonly IntVector Left = new IntVector(-1, 0);

        public static readonly IntVector Right = new IntVector(1, 0);

        public static readonly IntVector Up = new IntVector(0, -1);

        public static readonly IntVector Down = new IntVector(0, 1);

        public static IntVector FromWindowsPoint(Point point)
        {
            return new IntVector((int)point.X, (int)point.Y);
        }

        public int X;

        public int Y;

        public double SquareLength => Math.Pow(X, 2) + Math.Pow(Y, 2);

        public IntVector(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public static IntVector operator +(IntVector a, IntVector b) => new IntVector(a.X + b.X, a.Y + b.Y);

        public static IntVector operator -(IntVector a, IntVector b) => new IntVector(a.X - b.X, a.Y - b.Y);

        public static IntVector operator -(IntVector a) => new IntVector(-a.X, -a.Y);

        public static IntVector operator *(IntVector a, IntVector b) => new IntVector(a.X * b.X, a.Y * b.Y);

        public static IntVector operator *(IntVector a, int b) => new IntVector(a.X * b, a.Y * b);

        public static IntVector operator *(IntVector a, float b) => new IntVector((int)(a.X * b), (int)(a.Y * b));

        public static IntVector operator *(IntVector a, double b) => new IntVector((int)(a.X * b), (int)(a.Y * b));

        public static IntVector operator /(IntVector a, IntVector b) => new IntVector(a.X / b.X, a.Y / b.Y);

        public static IntVector operator /(IntVector a, int b) => new IntVector(a.X / b, a.Y / b);

        public static IntVector operator /(IntVector a, float b) => new IntVector((int)(a.X / b), (int)(a.Y / b));

        public static IntVector operator /(IntVector a, double b) => new IntVector((int)(a.X / b), (int)(a.Y / b));

        public static IntVector operator %(IntVector a, IntVector b) => new IntVector(a.X % b.X, a.Y % b.Y);

        public static IntVector operator %(IntVector a, int b) => new IntVector(a.X % b, a.Y % b);

        public IntVector AddX(int x)
        {
            return new IntVector(X + x, Y);
        }

        public IntVector SubX(int x)
        {
            return new IntVector(X - x, Y);
        }

        public IntVector AddY(int y)
        {
            return new IntVector(X, Y + y);
        }

        public IntVector SubY(int y)
        {
            return new IntVector(X, Y - y);
        }

        public override string ToString()
        {
            return X + "|" + Y;
        }
    }
}
