using System;

namespace SpaceEngineers2D.Model.Blocks
{
    using System.Collections.Generic;
    using System.Windows.Media;

    using Items;

    public class StandardBlockType : BlockType
    {
        private readonly Func<IReadOnlyList<IItem>> _getDroppedItemsFunc;

        public ImageSource Image{ get; }

        public float MaxIntegrity { get; } = 10f;


        public StandardBlockType(ImageSource image, Func<IReadOnlyList<IItem>> getDroppedItemsFunc)
        {
            Image = image;
            _getDroppedItemsFunc = getDroppedItemsFunc;
        }

        public IReadOnlyList<IItem> GetDroppedItems()
        {
            return _getDroppedItemsFunc();
        }

        public StandardBlock InstantiateBlock()
        {
            return new StandardBlock(this);
        }
    }
}
