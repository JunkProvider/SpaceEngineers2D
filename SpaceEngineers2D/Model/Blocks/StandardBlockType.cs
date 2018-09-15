namespace SpaceEngineers2D.Model.Blocks
{
    using System.Collections.Generic;
    using System.Windows.Media;

    using Items;

    public class StandardBlockType : BlockType
    {
        public ImageSource Image{ get; }

        public override string Name { get; }

        public bool IsSolid { get; }

        public IDictionary<StandardItemType, int> DroppedItems { get; }

        public float MaxIntegrity { get; } = 10f;

        public StandardBlockType(string name, ImageSource image, IDictionary<StandardItemType, int> droppedItems, bool isSolid = true)
        {
            Name = name;
            Image = image;
            DroppedItems = droppedItems;
            IsSolid = isSolid;
        }

        public StandardBlock InstantiateBlock()
        {
            return new StandardBlock(this);
        }
    }
}
