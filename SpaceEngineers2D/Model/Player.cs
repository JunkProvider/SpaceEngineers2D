using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using SpaceEngineers2D.Annotations;
using SpaceEngineers2D.Physics;
using SpaceEngineers2D.View.Inventory;

namespace SpaceEngineers2D.Model
{
    using System.Collections.Generic;

    using Geometry;
    using Blocks;
    using Inventories;

    public class Player : IMobileObject, INotifyPropertyChanged
    {
        public const int ItemPickupRange = 1000;

        public event PropertyChangedEventHandler PropertyChanged;

        private IBlockInWorld _interactingBlock;
        private IntVector _position;

        public Inventory Inventory { get; } = new Inventory(9);

        public InventorySlot HandInventorySlot { get; } = new InventorySlot(0);

        public List<BlockBlueprintSlot> BlueprintSlots { get; } = new List<BlockBlueprintSlot> { new BlockBlueprintSlot(Key.NumPad1), new BlockBlueprintSlot(Key.NumPad2), new BlockBlueprintSlot(Key.NumPad3) };

        public BlockBlueprintSlot SelectedBlueprintSlot => BlueprintSlots.SingleOrDefault(blueprintSlot => blueprintSlot.Selected);

        public IntVector Position
        {
            get => _position;
            set
            {
                if (SetProperty(ref _position, value))
                    RaisePropertyChanged(nameof(Coords));
            }
        }

        public IntVector Coords => Position / Constants.PhysicsUnitVector;

        public IntVector Size { get; set; } = new IntVector(800, 1800);

        public IntRectangle Bounds => IntRectangle.FromPositionAndSize(Position, Size);

        public IntVector Velocity { get; set; }

        public Dictionary<Side, bool> MovementOrders { get; set; } = new Dictionary<Side, bool>();

        public IntVector TargetPosition { get; set; }

        public BlockCoords TargetBlockCoords { get; set; }

        public bool TargetBlockCoordsInRange { get; set; }

        public IBlockInWorld TargetBlock { get; set; }

        public IBlockInWorld InteractingBlock
        {
            get => _interactingBlock;
            set => SetProperty(ref _interactingBlock, value);
        }

        public Dictionary<Side, List<Block>> TouchedBlocks { get; set; } = new Dictionary<Side, List<Block>>();

        public Player()
        {
            TouchedBlocks[Side.Left] = new List<Block>();
            TouchedBlocks[Side.Right] = new List<Block>();
            TouchedBlocks[Side.Top] = new List<Block>();
            TouchedBlocks[Side.Bottom] = new List<Block>();

            MovementOrders[Side.Left] = false;
            MovementOrders[Side.Right] = false;
            MovementOrders[Side.Top] = false;
            MovementOrders[Side.Bottom] = false;
        }

        public void TrySetSelectedBlueprintSlot(Key hotkey)
        {
            foreach (var blueprintSlot in BlueprintSlots)
            {
                if (blueprintSlot.BlueprintedBlock != null && blueprintSlot.Hotkey == hotkey)
                {
                    SelectBlueprintSlot(blueprintSlot);
                    break;
                }
            }
        }

        public void SelectBlueprintSlot(BlockBlueprintSlot blueprintSlotToSelect)
        {
            foreach (var blueprintSlot in BlueprintSlots)
            {
                blueprintSlot.Selected = false;
            }

            blueprintSlotToSelect.Selected = true;

            this.RaisePropertyChanged(nameof(SelectedBlueprintSlot));
        }

        [NotifyPropertyChangedInvocator]
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (!object.Equals(field, value))
            {
                field = value;
                RaisePropertyChanged(propertyName);
                return true;
            }

            return false;
        }

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
