namespace SpaceEngineers2D.Geometry.BinaryGrid
{
    public class BinaryGridLeaf<T> : IBinarySubGrid<T>
    {
        public const int Size = 2;

        private readonly T[] _items = new T[Size * Size];

        public T Get(IntVector position)
        {
            return _items[PositionToIndex(position)];
        }

        public SetItemResult<T> Set(IntVector position, T item, SetItemResult<T> result)
        {
            var index = PositionToIndex(position);
            result.RemovedItem = _items[index];
            _items[index] = item;
            return result;
        }

        public void ForEachWithin(IntRectangle rectangle, EnumerateItemDelegate<T> func)
        {
            for (var x = rectangle.Left; x < rectangle.Right; x++)
            {
                for (var y = rectangle.Top; y < rectangle.Bottom; y++)
                {
                    var itemPosition = new IntVector(x, y);
                    var itemIndex = PositionToIndex(itemPosition);

                    var item = _items[itemIndex];
                    if (item != null)
                    {
                        func(item, itemPosition);
                    }
                }
            }
        }

        private int PositionToIndex(IntVector position)
        {
            return PositionToIndex(position.X, position.Y);
        }

        private int PositionToIndex(int x, int y)
        {
            return x + (y * Size);
        }
    }
}
