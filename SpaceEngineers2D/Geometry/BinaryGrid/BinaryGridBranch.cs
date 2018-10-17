using System.Collections.Generic;
using System.Linq;

namespace SpaceEngineers2D.Geometry.BinaryGrid
{
    using System;

    public class BinaryGridBranch<T> : IBinarySubGrid<T>
    {
        public int Size { get; private set; }

        public IntVector SizeVector { get; private set; }

        public int HalfSize { get; private set; }

        public IntVector HalfSizeVector { get; private set; }

        protected IBinarySubGrid<T>[] Children = new IBinarySubGrid<T>[8];

        public BinaryGridBranch(int size, IBinarySubGrid<T>[] children)
            :this(size)
        {
            Children = children;
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
            var z = position.Z;

            // 0|0|0
            if (x < halfSize && y < halfSize && z < halfSize)
            {
                return Children[0] != null ? Children[0].Get(position) : default(T);
            }

            // 1|0|0
            if (x >= halfSize && y < halfSize && z < halfSize)
            {
                return Children[1] != null ? Children[1].Get(new IntVector(x - halfSize, y, z)) : default(T);
            }

            // 0|1|0
            if (x < halfSize && y >= halfSize && z < halfSize)
            {
                return Children[2] != null ? Children[2].Get(new IntVector(x, y - halfSize, z)) : default(T);
            }

            // 1|1|0
            if (x >= halfSize && y >= halfSize && z < halfSize)
            {
                return Children[3] != null ? Children[3].Get(new IntVector(x - halfSize, y - halfSize, z)) : default(T);
            }

            // 0|0|1
            if (x < halfSize && y < halfSize && z >= halfSize)
            {
                return Children[4] != null ? Children[1].Get(new IntVector(x, y, z - halfSize)) : default(T);
            }

            // 1|0|1
            if (x >= halfSize && y < halfSize && z >= halfSize)
            {
                return Children[5] != null ? Children[1].Get(new IntVector(x - halfSize, y, z - halfSize)) : default(T);
            }

            // 0|1|1
            if (x < halfSize && y >= halfSize && z >= halfSize)
            {
                return Children[6] != null ? Children[2].Get(new IntVector(x, y - halfSize, z - halfSize)) : default(T);
            }

            // 1|1|1
            if (x >= halfSize && y >= halfSize && z >= halfSize)
            {
                return Children[7] != null ? Children[3].Get(new IntVector(x - halfSize, y - halfSize, z - halfSize)) : default(T);
            }

            throw new InvalidOperationException("Logic Error.");
        }

        public virtual SetItemResult<T> Set(IntVector position, T item, SetItemResult<T> result)
        {
            var halfSize = HalfSize;
            var x = position.X;
            var y = position.Y;
            var z = position.Z;

            // 0|0|0
            if (x < halfSize && y < halfSize && z < halfSize)
            {
                return GetOrCreateChild(0).Set(position, item, result);
            }

            // 1|0|0
            if (x >= halfSize && y < halfSize && z < halfSize)
            {
                return GetOrCreateChild(1).Set(new IntVector(x - halfSize, y, z), item, result);
            }

            // 0|1|0
            if (x < halfSize && y >= halfSize && z < halfSize)
            {
                return GetOrCreateChild(2).Set(new IntVector(x, y - halfSize, z), item, result);
            }

            // 1|1|0
            if (x >= halfSize && y >= halfSize && z < halfSize)
            {
                return GetOrCreateChild(3).Set(new IntVector(x - halfSize, y - halfSize, z), item, result);
            }

            // 0|0|1
            if (x < halfSize && y < halfSize && z >= halfSize)
            {
                return GetOrCreateChild(4).Set(new IntVector(x, y, z - halfSize), item, result);
            }

            // 1|0|1
            if (x >= halfSize && y < halfSize && z >= halfSize)
            {
                return GetOrCreateChild(5).Set(new IntVector(x - halfSize, y, z - halfSize), item, result);
            }

            // 0|1|1
            if (x < halfSize && y >= halfSize && z >= halfSize)
            {
                return GetOrCreateChild(6).Set(new IntVector(x, y - halfSize, z - halfSize), item, result);
            }

            // 1|1|1
            if (x >= halfSize && y >= halfSize && z >= halfSize)
            {
                return GetOrCreateChild(7).Set(new IntVector(x - halfSize, y - halfSize, z - halfSize), item, result);
            }

            throw new InvalidOperationException("Logic Error.");
        }

        public virtual IEnumerable<T> GetAllWithin(IntRectangle rectangle)
        {
            var halfSize = HalfSize;
            var minX = rectangle.Left;
            var maxX = rectangle.Right;
            var minY = rectangle.Top;
            var maxY = rectangle.Bottom;
            var minZ = rectangle.Front;
            var maxZ = rectangle.Back;

            var items = Enumerable.Empty<T>();

            items = GetFromChild(items, 0, minX, minY, minZ, Math.Min(halfSize, maxX), Math.Min(halfSize, maxY), Math.Min(halfSize, maxZ));
            items = GetFromChild(items, 1, Math.Max(0, minX - halfSize), minY, minZ, maxX - halfSize, Math.Min(halfSize, maxY), Math.Min(halfSize, maxZ));
            items = GetFromChild(items, 2, minX, Math.Max(0, minY - halfSize), minZ, Math.Min(halfSize, maxX), maxY - halfSize, Math.Min(halfSize, maxZ));
            items = GetFromChild(items, 3, Math.Max(0, minX - halfSize), Math.Max(0, minY - halfSize), minZ, maxX - halfSize, maxY - halfSize, Math.Min(halfSize, maxZ));
            items = GetFromChild(items, 4, minX, minY, Math.Max(0, minZ - halfSize), Math.Min(halfSize, maxX), Math.Min(halfSize, maxY), maxZ - halfSize);
            items = GetFromChild(items, 5, Math.Max(0, minX - halfSize), minY, Math.Max(0, minZ - halfSize), maxX - halfSize, Math.Min(halfSize, maxY), maxZ - halfSize);
            items = GetFromChild(items, 6, minX, Math.Max(0, minY - halfSize), Math.Max(0, minZ - halfSize), Math.Min(halfSize, maxX), maxY - halfSize, maxZ - halfSize);
            items = GetFromChild(items, 7, Math.Max(0, minX - halfSize), Math.Max(0, minY - halfSize), Math.Max(0, minZ - halfSize), maxX - halfSize, maxY - halfSize, maxZ - halfSize);

            return items;
        }

        private IEnumerable<T> GetFromChild(IEnumerable<T> items, int index, int left, int top, int front, int right, int bottom, int back)
        {
            if (Children[index] == null)
                return items;

            return items.Concat(Children[index].GetAllWithin(IntRectangle.FromLeftTopFrontRightBottomBack(left, top, front, right, bottom, back)));
        }

        public virtual void ForEachWithin(IntRectangle rectangle, EnumerateItemDelegate<T> func)
        {
            var halfSize = HalfSize;
            var minX = rectangle.Left;
            var maxX = rectangle.Right;
            var minY = rectangle.Top;
            var maxY = rectangle.Bottom;
            var minZ = rectangle.Front;
            var maxZ = rectangle.Back;

            var children = Children;

            // 0|0|0
            if (children[0] != null && minX < halfSize && minY < halfSize)
            {
                children[0].ForEachWithin(
                    IntRectangle.FromLeftTopFrontRightBottomBack(
                        minX, minY, minZ,
                        Math.Min(halfSize, maxX), Math.Min(halfSize, maxY), Math.Min(halfSize, maxZ)), 
                    (block, coords) => func(block, coords)
                );
            }

            // 1|0|0
            if (children[1] != null && minY < halfSize && maxX >= halfSize)
            {
                children[1].ForEachWithin(
                    IntRectangle.FromLeftTopFrontRightBottomBack(
                        Math.Max(0, minX - halfSize), minY, minZ,
                        maxX - halfSize, Math.Min(halfSize, maxY), Math.Min(halfSize, maxZ)),
                    (block, coords) => func(block, coords + IntVector.Right * halfSize)
                );
            }

            // 0|1|0
            if (children[2] != null && minX < halfSize && maxY >= halfSize)
            {
                children[2].ForEachWithin(
                    IntRectangle.FromLeftTopFrontRightBottomBack(
                        minX, Math.Max(0, minY - halfSize), minZ,
                        Math.Min(halfSize, maxX), maxY - halfSize, Math.Min(halfSize, maxZ)),
                    (block, coords) => func(block, coords + IntVector.Down * halfSize)
                );
            }

            // 1|1|0
            if (children[3] != null && maxX >= halfSize && maxY >= halfSize)
            {
                children[3].ForEachWithin(
                    IntRectangle.FromLeftTopFrontRightBottomBack(
                        Math.Max(0, minX - halfSize), Math.Max(0, minY - halfSize), minZ,
                        maxX - halfSize, maxY - halfSize, Math.Min(halfSize, maxZ)),
                    (block, coords) => func(block, coords + IntVector.RightDown * halfSize)
                );
            }

            // 0|0|1
            if (children[4] != null && minX < halfSize && minY < halfSize)
            {
                children[4].ForEachWithin(
                    IntRectangle.FromLeftTopFrontRightBottomBack(
                        minX, minY, Math.Max(0, minZ - halfSize),
                        Math.Min(halfSize, maxX), Math.Min(halfSize, maxY), maxZ - halfSize),
                    (block, coords) => func(block, coords + IntVector.Back * halfSize)
                );
            }

            // 1|0|1
            if (children[5] != null && minY < halfSize && maxX >= halfSize)
            {
                children[5].ForEachWithin(
                    IntRectangle.FromLeftTopFrontRightBottomBack(
                        Math.Max(0, minX - halfSize), minY, Math.Max(0, minZ - halfSize),
                        maxX - halfSize, Math.Min(halfSize, maxY), maxZ - halfSize),
                    (block, coords) => func(block, coords + IntVector.RightBack * halfSize)
                );
            }

            // 0|1|1
            if (children[6] != null && minX < halfSize && maxY >= halfSize)
            {
                children[6].ForEachWithin(
                    IntRectangle.FromLeftTopFrontRightBottomBack(
                        minX, Math.Max(0, minY - halfSize), Math.Max(0, minZ - halfSize),
                        Math.Min(halfSize, maxX), maxY - halfSize, maxZ - halfSize),
                    (block, coords) => func(block, coords + IntVector.DownBack * halfSize)
                );
            }

            // 1|1|1
            if (children[7] != null && maxX >= halfSize && maxY >= halfSize)
            {
                children[7].ForEachWithin(
                    IntRectangle.FromLeftTopFrontRightBottomBack(
                        Math.Max(0, minX - halfSize), Math.Max(0, minY - halfSize), Math.Max(0, minZ - halfSize),
                        maxX - halfSize, maxY - halfSize, maxZ - halfSize),
                    (block, coords) => func(block, coords + HalfSizeVector)
                );
            }
        }

        protected void SetSize(int size)
        {
            Size = size;
            SizeVector = new IntVector(Size, Size, Size);
            HalfSize = size / 2;
            HalfSizeVector = new IntVector(HalfSize, HalfSize, HalfSize);
        }

        private IBinarySubGrid<T> GetOrCreateChild(int index)
        {
            var halfSize = HalfSize;
            var child = Children[index];
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
                Children[index] = child;
            }
            return child;
        }
    }
}
