namespace SpaceEngineers2D.Model.Blocks
{
    public class GrassBlockType : BlockType
    {
        public override string Name => "Grass";

        public GrassBlock InstantiateBlock()
        {
            return new GrassBlock(this);
        }
    }
}
