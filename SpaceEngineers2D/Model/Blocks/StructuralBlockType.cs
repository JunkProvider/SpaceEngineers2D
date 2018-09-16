using System.Windows.Media;
using SpaceEngineers2D.Persistence;
using SpaceEngineers2D.Persistence.DataModel;

namespace SpaceEngineers2D.Model.Blocks
{
    using BlockBlueprints;

    public interface IStructuralBlockType : IBlockType
    {
        ImageSource Image { get; }

        BlockBlueprint Blueprint { get; }
    }

    public abstract class StructuralBlockType<TBlock> : BlockType<TBlock>, IStructuralBlockType
        where TBlock : StructuralBlock
    {
        public ImageSource Image { get; }

        public BlockBlueprint Blueprint { get; }

        protected StructuralBlockType(int id, string name, ImageSource image, BlockBlueprint blueprint)
            : base(id, name)
        {
            Image = image;
            Blueprint = blueprint;
        }
    }

    public sealed class StructuralBlockType : StructuralBlockType<StructuralBlock>
    {
        public StructuralBlockType(int id, string name, ImageSource image, BlockBlueprint blueprint)
            : base(id, name, image, blueprint)
        {
        }

        public StructuralBlock InstantiateBlock()
        {
            return new StructuralBlock(this);
        }

        public override Block Load(Deserializer deserializer, DictionaryAccess data)
        {
            var block = new StructuralBlock(this);

            if (data.TryGetClassNotNull("blueprintState", out BlockBlueprintStateData blueprintStateData))
            {
                deserializer.MapBlockBlueprintState(block.BlueprintState, blueprintStateData);
            }
            else
            {
                block.BlueprintState.FinishImmediately();
            }
            
            return block;
        }

        public override void Save(Serializer serializer, StructuralBlock block, DictionaryAccess data)
        {
            base.Save(serializer, block, data);

            data.Set("blueprintState", serializer.MapBlockBlueprintState(block.BlueprintState));
        }
    }
}
