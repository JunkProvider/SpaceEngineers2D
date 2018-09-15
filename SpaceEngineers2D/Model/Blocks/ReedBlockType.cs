using System.Windows.Media;

namespace SpaceEngineers2D.Model.Blocks
{
    public class ReedBlockType : BlockType
    {
        public override string Name { get; }

        public ImageSource Image { get; }

        public ReedBlockType(string name, ImageSource image)
        {
            Name = name;
            Image = image;
        }

        public ReedBlock InstantiateBlock()
        {
            return new ReedBlock(this);
        }
    }
}
