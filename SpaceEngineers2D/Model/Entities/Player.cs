using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using SpaceEngineers2D.Annotations;
using SpaceEngineers2D.Geometry;
using SpaceEngineers2D.Model.Blocks;
using SpaceEngineers2D.Model.Inventories;
using SpaceEngineers2D.Physics;

namespace SpaceEngineers2D.Model.Entities
{
    public class Player : Entity, INotifyPropertyChanged
    {
        public const int ItemPickupRange = 1000;

        public event PropertyChangedEventHandler PropertyChanged;

        private IBlockInWorld _interactingBlock;
        private ZLayer _blockPlacementLayer;

        public Inventory Inventory { get; } = new Inventory(9);

        public InventorySlot HandInventorySlot { get; } = new InventorySlot(0);

        public List<BlockBlueprintSlot> BlueprintSlots { get; } = new List<BlockBlueprintSlot> { new BlockBlueprintSlot(Key.NumPad1), new BlockBlueprintSlot(Key.NumPad2), new BlockBlueprintSlot(Key.NumPad3) };

        public BlockBlueprintSlot SelectedBlueprintSlot => BlueprintSlots.SingleOrDefault(blueprintSlot => blueprintSlot.Selected);

        public ZLayer BlockPlacementLayer
        {
            get => _blockPlacementLayer;
            set => SetProperty(ref _blockPlacementLayer, value);
        }
        
        public IntVector Coords => Position / Constants.BlockSizeVector;

        public override IntVector Size { get; } = new IntVector(800, 1800, 1000);

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

        public Player(PlayerType entityType)
            : base(entityType)
        {
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

        public void ToggleBlockPlacementLayer()
        {
            if (BlockPlacementLayer == ZLayer.Foreground)
            {
                BlockPlacementLayer = ZLayer.Background;
            }
            else
            {
                BlockPlacementLayer = ZLayer.Foreground;
            }
        }

        public override void Update(World world, TimeSpan elapsedTime)
        {
            base.Update(world, elapsedTime);

            ApplyMovementOrders();
        }

        private void ApplyMovementOrders()
        {
            var playerMoveSpeed = 4;

            if (TouchedBlocks[Side.Bottom].Count != 0)
            {
                var velocity = Velocity;

                if (MovementOrders[Side.Left])
                {
                    velocity.X = -playerMoveSpeed * Constants.BlockSize;
                }
                else if (MovementOrders[Side.Right])
                {
                    velocity.X = playerMoveSpeed * Constants.BlockSize;
                }
                else
                {
                    velocity.X = 0;
                }

                if (MovementOrders[Side.Top])
                {
                    velocity = velocity + IntVector.Up * Constants.BlockSize * 7;
                }

                Velocity = velocity;
            }
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
