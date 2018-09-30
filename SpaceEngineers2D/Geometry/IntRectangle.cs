namespace SpaceEngineers2D.Geometry
{
    using System.Windows;

    public struct IntRectangle
    {
        public static IntRectangle FromPositionAndSize(IntVector position, IntVector size)
        {
            return new IntRectangle(position, size);
        }

        public static IntRectangle FromPoints(IntVector leftTop, IntVector rightBottom)
        {
            return new IntRectangle(leftTop, rightBottom - leftTop);
        }

        public static IntRectangle FromXYWidthHeight(int x, int y, int width, int height)
        {
            return new IntRectangle(new IntVector(x, y, 0), new IntVector(width, height, 0));
        }

        public static IntRectangle FromXYZWidthHeightDepth(int x, int y, int z, int width, int height, int depth)
        {
            return new IntRectangle(new IntVector(x, y, z), new IntVector(width, height, depth));
        }

        public static IntRectangle FromLeftTopFrontRightBottomBack(int left, int top, int front, int right, int bottom, int back)
        {
            return new IntRectangle(new IntVector(left, top, front), new IntVector(right - left, bottom - top, back - front));
        }

        public IntVector Position;

        public IntVector Size;

        public int Left => Position.X;

        public int Right => Position.X + Size.X;

        public int Top => Position.Y;

        public int Bottom => Position.Y + Size.Y;

        public int Front => Position.Z;

        public int Back => Position.Z + Size.Z;

        public int Width => Size.X;

        public int Height => Size.Y;

        public int Depth => Size.Z;

        public IntVector LeftTopFront => Position;

        public IntVector RightBottomBack => Position + Size;

        public IntVector Center => Position + Size / 2;

        public IntVector CenterBottom => new IntVector(Left + Width / 2, Bottom, Front + Depth / 2);

        public IntRange XRange => new IntRange(Left, Right);

        public IntRange YRange => new IntRange(Top, Bottom);

        public IntRange ZRange => new IntRange(Front, Back);

        public IntRectangle(IntVector position, IntVector size)
        {
            Position = position;
            Size = size;
        }

        public IntRectangle(int x, int y, int z, int width, int height, int depth)
        {
            Position = new IntVector(x, y, z);
            Size = new IntVector(width, height, depth);
        }

        public IntRectangle Extend(int extend)
        {
            return Extend(new IntVector(extend, extend, extend));
        }

        public IntRectangle Extend(IntVector extend)
        {
            return new IntRectangle(Position - extend, Size + (extend * 2));
        }

        public IntRectangle Move(IntVector offset)
        {
            return new IntRectangle(Position + offset, Size);
        }

        public bool Contains(IntVector point)
        {
            return point.X >= Left && point.X < Right && point.Y >= Top && point.Y < Bottom && point.Z >= Front && point.Z < Back;
        }

        public bool Intersects(IntRectangle other)
        {
            return other.Right > Left && other.Left < Right && other.Bottom > Top && other.Top < Bottom && other.Back > Front && other.Front < Back;
        }

        public bool TryGetTouchedSide(IntRectangle other, out Side touchedSide)
        {
            if (Left < other.Right && Right > other.Left)
            {
                if (Top == other.Bottom)
                {
                    touchedSide = Side.Top;
                    return true;
                }
                if (Bottom == other.Top)
                {
                    touchedSide = Side.Bottom;
                    return true;
                }
            }
            if (Top < other.Bottom && Bottom > other.Top)
            {
                if (Left == other.Right)
                {
                    touchedSide = Side.Left;
                    return true;
                }
                if (Right == other.Left)
                {
                    touchedSide = Side.Right;
                    return true;
                }
            }
            touchedSide = Side.Left;
            return false;
        }

        public Rect ToWindowsRect()
        {
            return new Rect(Position.X, Position.Y, Size.X, Size.Y);
        }

        public override string ToString()
        {
            return Position.X + "|" + Position.Y + "|" + Position.Z + " " + Size.X + "x" + Size.Y + "x" + Size.Z;
        }
    }
}
