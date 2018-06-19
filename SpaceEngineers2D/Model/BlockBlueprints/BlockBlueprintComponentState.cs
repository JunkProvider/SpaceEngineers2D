namespace SpaceEngineers2D.Model.BlockBlueprints
{
    using SpaceEngineers2D.Model.Items;

    public class BlockBlueprintComponentState
    {
        private readonly BlockBlueprintComponent _component;

        public StandardItemType ItemType => _component.ItemType;

        public int RequiredCount => _component.Count;

        public int ActualCount { get; set; }

        public int RemainingCount => RequiredCount - ActualCount;

        public bool Complete => RemainingCount == 0;

        public float ActualIntegrityValue => ActualCount * _component.IntegrityValue;

        public BlockBlueprintComponentState(BlockBlueprintComponent component)
        {
            _component = component;
        }
    }
}