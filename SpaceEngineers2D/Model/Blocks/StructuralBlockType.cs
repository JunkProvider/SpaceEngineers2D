using System.Windows.Media;

namespace SpaceEngineers2D.Model.Blocks
{
    using BlockBlueprints;

    public abstract class StructuralBlockType : BlockType
    {
        public ImageSource Image { get; }

        public abstract BlockBlueprint Blueprint { get; }

        public abstract StructuralBlock InstantiateBlock();

        protected StructuralBlockType(ImageSource image)
        {
            Image = image;
        }
    }
}
