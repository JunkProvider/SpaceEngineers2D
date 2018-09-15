using System.Windows.Media;
using SpaceEngineers2D.Model.BlockBlueprints;

namespace SpaceEngineers2D.Model.Blocks
{
    public class BlastFurnaceBlockType : StructuralBlockType
    {
        public BlastFurnaceBlockType(string name, ImageSource image, BlockBlueprint blueprint)
            : base(name, image, blueprint)
        {

        }

        public override StructuralBlock InstantiateBlock()
        {
            return new BlastFurnaceBlock(this);
        }
    }
}
