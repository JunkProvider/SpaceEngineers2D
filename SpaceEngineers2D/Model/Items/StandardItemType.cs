using System.Collections.Generic;
using System.Windows.Media;
using SpaceEngineers2D.Persistence;

namespace SpaceEngineers2D.Model.Items
{
    public class StandardItemType : ItemType<StandardItem>
    {
        public float Mass { get; }

        public float Volume { get; }

        public ImageSource Icon { get; }

        public StandardItemType(int id, string name, float mass, float volume, ImageSource icon)
            : base(id, name)
        {
            Mass = mass;
            Volume = volume;
            Icon = icon;
        }

        public StandardItem Instantiate()
        {
            return new StandardItem(this);
        }

        public override Item Load(DictionaryAccess data)
        {
            return new StandardItem(this);
        }
    }
}
