namespace SpaceEngineers2D.Model.BlockBlueprints
{
    public class BlockComponentData
    {
        private int[] _components;

        public BlockComponentData(int componentCount)
        {
            _components = new int[componentCount];
        }

        public int Get(int index)
        {
            return _components[index];
        }

        public void Set(int index, int count)
        {
            _components[index] = count;
        }
    }
}
