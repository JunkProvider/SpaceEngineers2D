using System.Windows.Media;

namespace SpaceEngineers2D.Model.Blocks
{
    public class GrassBlockType : BlockType
    {
        public override string Name { get; }

        public ImageSource Image { get; }

        public GrassBlockType(string name, ImageSource image)
        {
            Name = name;
            Image = image;
        }

        public GrassBlock InstantiateBlock()
        {
            return new GrassBlock(this);
        }
    }
}
