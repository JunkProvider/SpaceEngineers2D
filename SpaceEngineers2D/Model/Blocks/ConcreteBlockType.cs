using System.Windows.Media;

namespace SpaceEngineers2D.Model.Blocks
{
    using BlockBlueprints;
    using Items;
    using System.Collections.Generic;

    public class ConcreteBlockType : StructuralBlockType
    {
        public override BlockBlueprint Blueprint { get; }

        public ConcreteBlockType(ImageSource image, ItemTypes itemTypes)
            : base(image)
        {
            var requiredComponents = new List<BlockBlueprintComponent>
            {
                new BlockBlueprintComponent(5, itemTypes.Rock, 10f),
                new BlockBlueprintComponent(1, itemTypes.Ore, 10f)
            };
            Blueprint  = new BlockBlueprint(requiredComponents);
        }

        public override StructuralBlock InstantiateBlock()
        {
            return new StructuralBlock(this);
        }
    }
}
