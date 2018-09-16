using SpaceEngineers2D.Persistence;

namespace SpaceEngineers2D.Model.Blocks
{
    using System.Collections.Generic;
    using System.Windows.Media;

    using Items;

    public sealed class StandardBlockType : BlockType<StandardBlock>
    {
        public ImageSource Image{ get; }
        
        public bool IsSolid { get; }

        public IDictionary<StandardItemType, int> DroppedItems { get; }

        public double MaxIntegrity { get; } = 10f;

        public StandardBlockType(int id, string name, ImageSource image, IDictionary<StandardItemType, int> droppedItems, bool isSolid = true)
            : base(id, name)
        {
            Image = image;
            DroppedItems = droppedItems;
            IsSolid = isSolid;
        }

        public StandardBlock Instantiate()
        {
            return new StandardBlock(this);
        }

        public override Block Load(Deserializer deserializer, DictionaryAccess data)
        {
            return new StandardBlock(this, data.GetOrDefault("integrity", MaxIntegrity));
        }

        public override void Save(Serializer serializer, StandardBlock block, DictionaryAccess data)
        {
            base.Save(serializer, block, data);
            data.Set("integrity", block.Integrity);
        }
    }
}
