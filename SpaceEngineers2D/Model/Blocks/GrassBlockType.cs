using System.Collections.Generic;
using System.Windows.Media;
using SpaceEngineers2D.Persistence;

namespace SpaceEngineers2D.Model.Blocks
{
    public sealed class GrassBlockType : BlockType<GrassBlock>
    {
        public ImageSource Image { get; }

        public GrassBlockType(int id, string name, ImageSource image)
            : base(id, name)
        {
            Image = image;
        }

        public GrassBlock InstantiateBlock()
        {
            return new GrassBlock(this);
        }

        public override void Save(Serializer serializer, GrassBlock block, DictionaryAccess data)
        {
            base.Save(serializer, block, data);
        }

        public override Block Load(Deserializer deserializer, DictionaryAccess data)
        {
            return new GrassBlock(this);
        }
    }
}
