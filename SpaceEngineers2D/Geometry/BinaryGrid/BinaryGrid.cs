using System;
using System.Collections.Generic;

namespace SpaceEngineers2D.Geometry.BinaryGrid
{
    public class BinaryGrid<T> : BinaryGridBranch<T>
    {
        private IntVector Offset { get; set; }

        public BinaryGrid()
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

            if (Extend(position.X, position.Y, position.Z, result))
            {
                return Set(absolutePosition, item, result);
            }

            return base.Set(position, item, result);
        }

        public void ForEach(EnumerateItemDelegate<T> func)
        {
            base.ForEachWithin(new IntRectangle(0, 0, 0, Size, Size, Size), (block, coords) => func(block, coords + Offset));
        }

        public IEnumerable<T> GetAll()
        {
            return base.GetAllWithin(IntRectangle.FromPositionAndSize(IntVector.Zero, SizeVector));
        }

        public override IEnumerable<T> GetAllWithin(IntRectangle rectangle)
        {
            rectangle = IntRectangle.FromPoints(
                IntVectorMath.MinMax(rectangle.LeftTopFront - Offset, IntVector.Zero, SizeVector),
                IntVectorMath.MinMax(rectangle.RightBottomBack - Offset, IntVector.Zero, SizeVector)
            );

            return base.GetAllWithin(rectangle);
        }

        public override void ForEachWithin(IntRectangle rectangle, EnumerateItemDelegate<T> func)
        {
            rectangle = IntRectangle.FromPoints(
                IntVectorMath.MinMax(rectangle.LeftTopFront - Offset, IntVector.Zero, SizeVector),
                IntVectorMath.MinMax(rectangle.RightBottomBack - Offset, IntVector.Zero, SizeVector)
            );

            base.ForEachWithin(rectangle, (block, coords) => func(block, coords + Offset));
        }

        private bool Extend(int x, int y, int z, SetItemResult<T> result)
        {
            var xShift = x < 0 ? -1 : (x >= Size ? 1 : 0);
            var yShift = y < 0 ? -1 : (y >= Size ? 1 : 0);
            var zShift = z < 0 ? -1 : (z >= Size ? 1 : 0);

            if (xShift == 0 && yShift == 0 && zShift == 0)
                return false;

            DoExtend(Math.Min(0, xShift), Math.Min(0, yShift), Math.Min(0, zShift), result);

            return true;
        }

        private void DoExtend(int shiftX, int shiftY, int shiftZ, SetItemResult<T> result)
        {
            var prevSize = Size;
            var prevChildren = Children;

            var offsetShift = new IntVector(shiftX, shiftY, shiftZ) * prevSize;
            result.OffsetShift = result.OffsetShift + offsetShift;
            Offset += offsetShift;

            SetSize(prevSize * 2);

            var child = new BinaryGridBranch<T>(prevSize, prevChildren);
            var childIndex = -shiftX + 2 * -shiftY + 4 * -shiftZ;

            Children = new IBinarySubGrid<T>[8];
            Children[childIndex] = child;
        }
    }
}
