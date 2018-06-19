namespace SpaceEngineers2D.Geometry.BinaryGrid
{
    using System;

    public class BinaryGridBranch<T> : IBinarySubGrid<T>
    {
        public int Size { get; private set; }

        public IntVector SizeVector { get; private set; }

        public int HalfSize { get; private set; }

        public IntVector HalfSizeVector { get; private set; }

        protected IBinarySubGrid<T>[] _children = new IBinarySubGrid<T>[4];

        public BinaryGridBranch(int size, IBinarySubGrid<T>[] children)
            :this(size)
        {
            _children = children;
        }

        public BinaryGridBranch(int size)
        {
            SetSize(size);
        }

        public virtual T Get(IntVector position)
        {
            var halfSize = HalfSize;
            var x = position.X;
            var y = position.Y;

            // 0|0
            if (x < halfSize && y < halfSize)
            {
                return _children[0] != null ? _children[0].Get(position) : default(T);
            }

            // 1|0
            if (x >= halfSize && y < halfSize)
            {
                return _children[1] != null ? _children[1].Get(new IntVector(x - halfSize, y)) : default(T);
            }

            // 0|1
            if (x < halfSize && y >= halfSize)
            {
                return _children[2] != null ? _children[2].Get(new IntVector(x, y - halfSize)) : default(T);
            }

            // 1|1
            if (x >= halfSize && y >= halfSize)
            {
                return _children[3] != null ? _children[3].Get(position - HalfSizeVector) : default(T);
            }

            throw new InvalidOperationException("Logic Error.");
        }

        public virtual SetItemResult<T> Set(IntVector position, T item, SetItemResult<T> result)
        {
            var halfSize = HalfSize;
            var x = position.X;
            var y = position.Y;

            // 0|0
            if (x < halfSize && y < halfSize)
            {
                return GetOrCreateChild(0).Set(position, item, result);
            }

            // 1|0
            if (x >= halfSize && y < halfSize)
            {
                return GetOrCreateChild(1).Set(new IntVector(x - halfSize, y), item, result);
            }

            // 0|1
            if (x < halfSize && y >= halfSize)
            {
                return GetOrCreateChild(2).Set(new IntVector(x, y - halfSize), item, result);
            }

            // 1|1
            if (x >= halfSize && y >= halfSize)
            {
                return GetOrCreateChild(3).Set(new IntVector(x - halfSize, y - halfSize), item, result);
            }

            throw new InvalidOperationException("Logic Error.");
        }

        public virtual void ForEachWithin(IntRectangle rectangle, EnumerateItemDelegate<T> func)
        {
            var halfSize = HalfSize;
            var minX = rectangle.Left;
            var minY = rectangle.Top;
            var maxX = rectangle.Right;
            var maxY = rectangle.Bottom;
            var children = _children;

            // 0|0
            if (children[0] != null && minX < halfSize && minY < halfSize)
            {
                children[0].ForEachWithin(
                    IntRectangle.FromLeftTopRightAndBottom(minX, minY, Math.Min(halfSize, maxX), Math.Min(halfSize, maxY)), 
                    (block, coords) => func(block, coords)
                );
            }

            // 1|0
            if (children[1] != null && minY < halfSize && maxX >= halfSize)
            {
                children[1].ForEachWithin(
                    IntRectangle.FromLeftTopRightAndBottom(Math.Max(0, minX - halfSize), minY, maxX - halfSize, Math.Min(halfSize, maxY)),
                    (block, coords) => func(block, coords.AddX(halfSize))
                );
            }

            // 0|1
            if (children[2] != null && minX < halfSize && maxY >= halfSize)
            {
                children[2].ForEachWithin(
                    IntRectangle.FromLeftTopRightAndBottom(minX, Math.Max(0, minY - halfSize), Math.Min(halfSize, maxX), maxY - halfSize),
                    (block, coords) => func(block, coords.AddY(halfSize))
                );
            }

            // 1|1
            if (children[3] != null && maxX >= halfSize && maxY >= halfSize)
            {
                children[3].ForEachWithin(
                    IntRectangle.FromLeftTopRightAndBottom(Math.Max(0, minX - halfSize), Math.Max(0, minY - halfSize), maxX - halfSize, maxY - halfSize),
                    (block, coords) => func(block, coords + HalfSizeVector)
                );
            }
        }

        protected void SetSize(int size)
        {
            Size = size;
            SizeVector = new IntVector(Size, Size);
            HalfSize = size / 2;
            HalfSizeVector = new IntVector(HalfSize, HalfSize);
        }

        private IBinarySubGrid<T> GetOrCreateChild(int index)
        {
            var halfSize = this.HalfSize;
            var child = this._children[index];
            if (child == null)
            {
                if (halfSize == BinaryGridLeaf<T>.Size)
                {
                    child = new BinaryGridLeaf<T>();
                }
                else
                {
                    child = new BinaryGridBranch<T>(halfSize);
                }
                _children[index] = child;
            }
            return child;
        }
    }
}
