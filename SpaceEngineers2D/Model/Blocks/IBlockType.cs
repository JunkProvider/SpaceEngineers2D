using SpaceEngineers2D.Persistence;

namespace SpaceEngineers2D.Model.Blocks
{
    public interface IBlockType
    {
        int Id { get; }

        string Name { get; }
        
        Block Load(Deserializer deserializer, DictionaryAccess data);

        void Save(Serializer serializer, Block block, DictionaryAccess data);
    }

    public abstract class BlockType<TBlock> : IBlockType
        where TBlock : Block
    {
        public int Id { get; }

        public string Name { get; }

        protected BlockType(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public abstract Block Load(Deserializer deserializer, DictionaryAccess data);

        public void Save(Serializer serializer, Block block, DictionaryAccess data)
        {
            Save(serializer, (TBlock) block, data);
        }

        public virtual void Save(Serializer serializer, TBlock block, DictionaryAccess data)
        {

        }
    }
}
