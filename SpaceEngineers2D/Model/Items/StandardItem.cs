using System.Windows.Media;

namespace SpaceEngineers2D.Model.Items
{
    public sealed class StandardItem : Item
    {
        public StandardItemType StandardItemType => (StandardItemType)ItemType;

        public StandardItem(StandardItemType itemType)
            : base(itemType)
        {
        }

        public override float Mass => StandardItemType.Mass;

        public override float Volume => StandardItemType.Volume;

        public override ImageSource Icon => StandardItemType.Icon;

        public override Item Clone()
        {
            return new StandardItem(StandardItemType);
        }

        protected override bool Equals(Item other)
        {
            return other.ItemType == ItemType;
        }
    }
}
