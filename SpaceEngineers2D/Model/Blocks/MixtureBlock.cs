using System.Collections.Generic;
using SpaceEngineers2D.Chemistry;
using SpaceEngineers2D.Model.Items;

namespace SpaceEngineers2D.Model.Blocks
{
    public class MixtureBlock : Block
    {
        private double _integrity = 100;

        public Mixture Mixture { get; }

        public override bool IsSolid => Mixture.AggregationState == AggregationState.Solid;

        public override bool IsDestoryed => _integrity <= 0;

        public double IntegrityRatio => _integrity / 100;

        public MixtureBlock(MixtureBlockType blockType, Mixture mixture) : base(blockType)
        {
            Mixture = mixture;
        }

        public override IReadOnlyList<IItem> GetDroppedItems()
        {
            return new IItem[] {new MixtureItem(Mixture)};
        }

        public override void Damage(float damage)
        {
            _integrity -= damage;
        }
    }
}
