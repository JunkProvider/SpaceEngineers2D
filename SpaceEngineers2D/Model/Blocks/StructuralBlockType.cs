using System.Windows.Media;

namespace SpaceEngineers2D.Model.Blocks
{
    using BlockBlueprints;

    public class StructuralBlockType : BlockType
    {
        public ImageSource Image { get; }

        public BlockBlueprint Blueprint { get; }
        
        public StructuralBlockType(ImageSource image, BlockBlueprint blueprint)
        {
            Image = image;
            Blueprint = blueprint;
        }

        public StructuralBlock InstantiateBlock()
        {
            return new StructuralBlock(this);
        }
    }
}
