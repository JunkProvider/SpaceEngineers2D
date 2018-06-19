using System.Windows.Media;

namespace SpaceEngineers2D.Model.Items
{
    public class StandardItemType : ItemType
    {
        public float Mass { get; }

        public float Volume { get; }

        public ImageSource Icon { get; }

        public StandardItemType(string name, float mass, float volume, ImageSource icon)
            : base(name)
        {
            Mass = mass;
            Volume = volume;
            Icon = icon;
        }

        public StandardItem InstantiateItem()
        {
            return new StandardItem(this);
        }
    }
}
