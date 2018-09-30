using System;
using System.Windows;

namespace SpaceEngineers2D.Geometry
{
    public struct IntVector
    {
        public static readonly IntVector Zero = new IntVector(0, 0, 0);

        public static readonly IntVector Left = new IntVector(-1, 0, 0);

        public static readonly IntVector Right = new IntVector(1, 0, 0);

        public static readonly IntVector Up = new IntVector(0, -1, 0);

        public static readonly IntVector Down = new IntVector(0, 1, 0);

        public static readonly IntVector Front = new IntVector(0, 0, -1);

        public static readonly IntVector Back = new IntVector(0, 0, 1);

        public static readonly IntVector RightDown = new IntVector(1, 1, 0);

        public static readonly IntVector RightBack = new IntVector(1, 0, 1);

        public static readonly IntVector DownBack = new IntVector(1, 0, 1);

        public static IntVector FromWindowsPoint(Point point)
        {
            return new IntVector((int)point.X, (int)point.Y, 0);
        }

        public int X;

        public int Y;

        public int Z;

        public double SquareLength => Math.Pow(X, 2) + Math.Pow(Y, 2);

        public IntVector(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static IntVector operator +(IntVector a, IntVector b) => new IntVector(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

        public static IntVector operator -(IntVector a, IntVector b) => new IntVector(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

        public static IntVector operator -(IntVector a) => new IntVector(-a.X, -a.Y, -a.Z);

        public static IntVector operator *(IntVector a, IntVector b) => new IntVector(a.X * b.X, a.Y * b.Y, a.Z * b.Z);

        public static IntVector operator *(IntVector a, int b) => new IntVector(a.X * b, a.Y * b, a.Z * b);

        public static IntVector operator *(IntVector a, float b) => new IntVector((int)(a.X * b), (int)(a.Y * b), (int)(a.Z * b));

        public static IntVector operator *(IntVector a, double b) => new IntVector((int)(a.X * b), (int)(a.Y * b), (int)(a.Z * b));

        public static IntVector operator /(IntVector a, IntVector b) => new IntVector(a.X / b.X, a.Y / b.Y, a.Z / b.Z);

        public static IntVector operator /(IntVector a, int b) => new IntVector(a.X / b, a.Y / b, a.Z / b);

        public static IntVector operator /(IntVector a, float b) => new IntVector((int)(a.X / b), (int)(a.Y / b), (int)(a.Z / b));

        public static IntVector operator /(IntVector a, double b) => new IntVector((int)(a.X / b), (int)(a.Y / b), (int)(a.Z / b));

        public static IntVector operator %(IntVector a, IntVector b) => new IntVector(a.X % b.X, a.Y % b.Y, a.Z % b.Z);

        public static IntVector operator %(IntVector a, int b) => new IntVector(a.X % b, a.Y % b, a.Z % b);

        public IntVector MoveRight(int x)
        {
            return new IntVector(X + x, Y, Z);
        }

        public IntVector MoveLeft(int x)
        {
            return new IntVector(X - x, Y, Z);
        }

        public IntVector MoveDown(int y)
        {
            return new IntVector(X, Y + y, Z);
        }

        public IntVector MoveUp(int y)
        {
            return new IntVector(X, Y - y, Z);
        }

        public IntVector Floor(int step)
        {
            return new IntVector(
                (int)Math.Floor((double)X / step),
                (int)Math.Floor((double)Y / step),
                (int)Math.Floor((double)Z / step)) * step;
        }

        public IntVector DivideRoundDown(int step)
        {
            return new IntVector(
                (int)Math.Floor((double)X / step),
                (int)Math.Floor((double)Y / step),
                (int)Math.Floor((double)Z / step));
        }

        public IntVector DivideRoundUp(int step)
        {
            return new IntVector(
                (int)Math.Ceiling((double)X / step),
                (int)Math.Ceiling((double)Y / step),
                (int)Math.Ceiling((double)Z / step));
        }

        public override string ToString()
        {
            return X + "|" + Y + "|" + Z;
        }
    }
}
