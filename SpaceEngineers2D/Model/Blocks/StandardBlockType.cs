namespace SpaceEngineers2D.Model.Blocks
{
    using System.Collections.Generic;
    using System.Windows.Media;

    using Items;

    public class StandardBlockType : BlockType
    {
        public ImageSource Image{ get; }

        public IDictionary<StandardItemType, int> DroppedItems { get; }

        public float MaxIntegrity { get; } = 10f;

        public StandardBlockType(ImageSource image, IDictionary<StandardItemType, int> droppedItems)
        {
            Image = image;
            DroppedItems = droppedItems;
        }

        public StandardBlock InstantiateBlock()
        {
            return new StandardBlock(this);
        }
    }
}
