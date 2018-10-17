using System.Collections.Generic;

namespace SpaceEngineers2D.Geometry.BinaryGrid
{
    public interface IBinarySubGrid<T>
    {
        T Get(IntVector position);

        SetItemResult<T> Set(IntVector position, T item, SetItemResult<T> result);

        void ForEachWithin(IntRectangle rectangle, EnumerateItemDelegate<T> func);

        IEnumerable<T> GetAllWithin(IntRectangle rectangle);
    }
}
