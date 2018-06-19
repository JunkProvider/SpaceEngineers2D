namespace SpaceEngineers2D.Geometry.BinaryGrid
{
    using System.Windows;

    public class BinaryGridRoot<T> : BinaryGridBranch<T>
    {
        public IntVector Offset { get; set; }

        public BinaryGridRoot()
            :base(2 * BinaryGridLeaf<T>.Size)
        {
        }

        public override T Get(IntVector position)
        {
            position = position - Offset;

            if (position.X < 0 || position.X >= Size || position.Y < 0 || position.Y >= Size)
            {
                return default(T);
            }

            return base.Get(position);
        }

        public SetItemResult<T> Set(IntVector position, T item)
        {
            return Set(position, item, new SetItemResult<T>());
        }

        public override SetItemResult<T> Set(IntVector position, T item, SetItemResult<T> result)
        {
            var absolutePosition = position;
            position -= Offset;

            if (Extend(position.X, position.Y, result))
            {
                return Set(absolutePosition, item, result);
            }

            return base.Set(position, item, result);
        }

        public override void ForEachWithin(IntRectangle rectangle, EnumerateItemDelegate<T> func)
        {
            rectangle = IntRectangle.FromPoints(
                IntVectorMath.MinMax(rectangle.LeftTop - Offset, IntVector.Zero, SizeVector),
                IntVectorMath.MinMax(rectangle.RightBottom - Offset, IntVector.Zero, SizeVector)
            );

            base.ForEachWithin(rectangle, (block, coords) => func(block, coords + Offset));
        }

        private bool Extend(int x, int y, SetItemResult<T> result)
        {
            if (x < 0)
            {
                DoExtend(-1, y < 0 ? -1 : 0, result);
                return true;
            }
            if (y < 0)
            {
                DoExtend(0, -1, result);
                return true;
            }
            if (x >= Size)
            {
                DoExtend(0, y < 0 ? -1 : 0, result);
                return true;
            }
            if (y >= Size)
            {
                DoExtend(-1, 0, result);
                return true;
            }
            return false;
        }

        private void DoExtend(int shiftX, int shiftY, SetItemResult<T> result)
        {
            var prevSize = Size;
            var prevChildren = _children;

            var offsetShift = new IntVector(shiftX, shiftY) * prevSize;
            result.OffsetShift = result.OffsetShift + offsetShift;
            Offset += offsetShift;

            SetSize(prevSize * 2);

            var child = new BinaryGridBranch<T>(prevSize, prevChildren);
            var childIndex = -shiftX + 2 * -shiftY;

            _children = new IBinarySubGrid<T>[4];
            _children[childIndex] = child;
        }
    }
}
