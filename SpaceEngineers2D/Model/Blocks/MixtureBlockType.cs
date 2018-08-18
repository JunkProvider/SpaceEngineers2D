using SpaceEngineers2D.Chemistry;

namespace SpaceEngineers2D.Model.Blocks
{
    public class MixtureBlockType : BlockType
    {
        public MixtureBlock InstantiateBlock(Mixture mixture)
        {
            return new MixtureBlock(this, mixture);
        }
    }
}
