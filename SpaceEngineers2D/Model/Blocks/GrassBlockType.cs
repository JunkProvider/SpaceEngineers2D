namespace SpaceEngineers2D.Model.Blocks
{
    public class GrassBlockType : BlockType
    {
        public GrassBlock InstantiateBlock()
        {
            return new GrassBlock(this);
        }
    }
}
