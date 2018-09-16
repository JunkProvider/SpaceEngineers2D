using System.Collections.Generic;
using SpaceEngineers2D.Persistence;

namespace SpaceEngineers2D.Model.Items
{
    public abstract class ItemType<TItem> : ItemType
        where TItem : Item
    {
        protected ItemType(int id, string name) : base(id, name)
        {
        }

        public override void Save(Item item, DictionaryAccess data)
        {
            Save(data, (TItem) item);
        }

        public virtual void Save(DictionaryAccess data, TItem item)
        {

        }
    }

    public abstract class ItemType
    {
        public int Id { get; }

        public string Name { get; }

        protected ItemType(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public abstract Item Load(DictionaryAccess data);

        public abstract void Save(Item item, DictionaryAccess data);
    }
}
