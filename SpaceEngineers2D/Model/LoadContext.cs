using SpaceEngineers2D.Model.Blocks;
using SpaceEngineers2D.Model.Entities;
using SpaceEngineers2D.Model.Items;
using SpaceEngineers2D.Persistence;

namespace SpaceEngineers2D.Model
{
    public class LoadContext
    {
        public Deserializer Deserializer { get; }

        public BlockTypes BlockTypes { get; }

        public EntityTypes EntityTypes { get; }

        public ItemTypes ItemTypes { get; }

        public LoadContext(Deserializer deserializer, BlockTypes blockTypes, EntityTypes entityTypes, ItemTypes itemTypes)
        {
            BlockTypes = blockTypes;
            EntityTypes = entityTypes;
            ItemTypes = itemTypes;
            Deserializer = deserializer;
        }
    }
}