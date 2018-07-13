using System.Windows.Media;

namespace SpaceEngineers2D.Model.Blocks
{
    using BlockBlueprints;

    public class ConcreteBlockType : StructuralBlockType
    {
        public override BlockBlueprint Blueprint { get; }

        public ConcreteBlockType(ImageSource image, BlockBlueprint blueprint)
            : base(image)
        {
            Blueprint  = blueprint;
        }

        public override StructuralBlock InstantiateBlock()
        {
            return new StructuralBlock(this);
        }
    }
}
