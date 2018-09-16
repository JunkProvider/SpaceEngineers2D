namespace SpaceEngineers2D.Model.BlockBlueprints
{
    using Items;

    public class BlockBlueprintComponentState
    {
        private readonly BlockBlueprintComponent _component;

        public int ActualCount { get; set; }

        public StandardItemType ItemType => _component.ItemType;

        public int RequiredCount => _component.Count;
        
        public int RemainingCount => RequiredCount - ActualCount;

        public bool Complete => RemainingCount == 0;

        public double ActualIntegrityValue => ActualCount * _component.IntegrityValue;

        public BlockBlueprintComponentState(BlockBlueprintComponent component)
        {
            _component = component;
        }
    }
}