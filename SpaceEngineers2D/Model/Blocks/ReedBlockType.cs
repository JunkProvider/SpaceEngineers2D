using System.Collections.Generic;
using System.Windows.Media;
using SpaceEngineers2D.Persistence;

namespace SpaceEngineers2D.Model.Blocks
{
    public sealed class ReedBlockType : BlockType<ReedBlock>
    {
        public ImageSource Image { get; }

        public ReedBlockType(int id, string name, ImageSource image)
            : base(id, name)
        {
            Image = image;
        }

        public ReedBlock InstantiateBlock()
        {
            return new ReedBlock(this);
        }

        public override void Save(Serializer serializer, ReedBlock block, DictionaryAccess data)
        {
            base.Save(serializer, block, data);
        }

        public override Block Load(Deserializer deserializer, DictionaryAccess data)
        {
            return new ReedBlock(this);
        }
    }
}
