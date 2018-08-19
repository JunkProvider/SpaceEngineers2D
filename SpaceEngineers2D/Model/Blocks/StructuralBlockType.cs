using System.Windows.Media;

namespace SpaceEngineers2D.Model.Blocks
{
    using BlockBlueprints;

    public class StructuralBlockType : BlockType
    {
        public override string Name { get; }

        public ImageSource Image { get; }

        public BlockBlueprint Blueprint { get; }
        
        public StructuralBlockType(string name, ImageSource image, BlockBlueprint blueprint)
        {
            Name = name;
            Image = image;
            Blueprint = blueprint;
        }

        public StructuralBlock InstantiateBlock()
        {
            return new StructuralBlock(this);
        }
    }
}
