using System.Collections.Generic;
using System.Linq;
using SpaceEngineers2D.Geometry;

namespace SpaceEngineers2D.Model
{
    public class TouchedObjectCollection
    {
        private readonly Dictionary<Side, HashSet<ICollidable>> _items = new Dictionary<Side, HashSet<ICollidable>>();

        public TouchedObjectCollection()
        {
            _items[Side.Left] = new HashSet<ICollidable>();
            _items[Side.Right] = new HashSet<ICollidable>();
            _items[Side.Top] = new HashSet<ICollidable>();
            _items[Side.Bottom] = new HashSet<ICollidable>();
            _items[Side.Front] = new HashSet<ICollidable>();
            _items[Side.Back] = new HashSet<ICollidable>();
        }

        public ISet<ICollidable> this[Side index] => _items[index];

        public int Count => _items.Sum(item => item.Value.Count);

        public bool Any()
        {
            return Count != 0;
        }

        public void Clear()
        {
            foreach (var item in _items)
            {
                item.Value.Clear();
            }
        }
    }
}
