namespace SpaceEngineers2D.Geometry.BinaryGrid
{
    public class BinaryGridLeaf<T> : IBinarySubGrid<T>
    {
        public const int Size = 2;

        private readonly T[,,] _items = new T[Size, Size, Size];

        public T Get(IntVector position)
        {
            return _items[position.X, position.Y, position.Z];
        }

        public SetItemResult<T> Set(IntVector position, T item, SetItemResult<T> result)
        {
            result.RemovedItem = _items[position.X, position.Y, position.Z];
            _items[position.X, position.Y, position.Z] = item;
            return result;
        }

        public void ForEachWithin(IntRectangle rectangle, EnumerateItemDelegate<T> func)
        {
            for (var x = rectangle.Left; x < rectangle.Right; x++)
            {
                for (var y = rectangle.Top; y < rectangle.Bottom; y++)
                {
                    for (var z = rectangle.Front; z < rectangle.Back; z++)
                    {
                        var item = _items[x, y, z];
                        if (item != null)
                        {
                            func(item, new IntVector(x, y, z));
                        }
                    }
                }
            }
        }
    }
}
