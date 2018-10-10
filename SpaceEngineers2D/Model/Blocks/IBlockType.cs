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
}
