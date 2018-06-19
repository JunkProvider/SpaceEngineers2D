using System.Windows.Media;

namespace SpaceEngineers2D.Model.Items
{
    public abstract class Item
    {
        public ItemType ItemType { get; }

        public string Name => ItemType.Name;

        public abstract float Mass { get; }

        public abstract float Volume { get; }

        public abstract ImageSource Icon { get; }

        protected Item(ItemType itemType)
        {
            ItemType = itemType;
        }

        public abstract Item Clone();

        protected abstract bool Equals(Item other);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Item)obj);
        }
    }
}
