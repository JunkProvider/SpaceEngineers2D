namespace SpaceEngineers2D.Geometry
{
    using System;
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

        public static IntRectangle FromXYWidthAndHeight(int x, int y, int width, int height)
        {
            return new IntRectangle(new IntVector(x, y), new IntVector(width, height));
        }

        public static IntRectangle FromLeftTopRightAndBottom(int left, int top, int right, int bottom)
        {
            return new IntRectangle(new IntVector(left, top), new IntVector(right - left, bottom - top));
        }

        public IntVector Position;

        public IntVector Size;

        public int Left => Position.X;

        public int Right => Position.X + Size.X;

        public int Top => Position.Y;

        public int Bottom => Position.Y + Size.Y;

        public IntVector LeftTop => Position;

        public IntVector RightBottom => Position + Size;

        public IntVector Center => Position + Size / 2;

        public IntVector CenterBottom => new IntVector(Left + Size.X / 2, Bottom);

        private IntRectangle(IntVector position, IntVector size)
        {
            Position = position;
            Size = size;
        }

        public IntRectangle(int x, int y, int width, int height)
        {
            Position = new IntVector(x, y);
            Size = new IntVector(width, height);
        }

        public IntRectangle Extend(int extend)
        {
            var extendVector = new IntVector(extend, extend);
            return new IntRectangle(Position - extendVector, Size + (extendVector * 2));
        }

        public IntRectangle Move(IntVector offset)
        {
            return new IntRectangle(this.Position + offset, this.Size);
        }

        public bool Contains(IntVector point)
        {
            return point.X >= Left && point.X < Right && point.Y >= Top && point.Y < Bottom;
        }

        public bool Overlaps(IntRectangle other)
        {
            return other.Right > Left && other.Left < Right && other.Bottom > Top && other.Top < Bottom;
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
            return Position.X + "|" + Position.Y + " " + Size.X + "x" + Size.Y;
        }
    }
}
