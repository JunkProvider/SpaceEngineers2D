using System;
using System.ComponentModel;
using SpaceEngineers2D.Model;
using SpaceEngineers2D.Model.Blocks;
using SpaceEngineers2D.View.Inventory;

namespace SpaceEngineers2D.View
{
    public class ApplicationViewModel : PropertyObservable
    {
        private object _interactingBlockViewModel;

        public InventoryViewModel PlayerInventoryViewModel { get; }

        public object InteractingBlockViewModel
        {
            get => _interactingBlockViewModel;
            private set => SetProperty(ref _interactingBlockViewModel, value);
        }

        public World World { get; }

        public ApplicationViewModel(World world)
        {
            if (world == null)
                throw new ArgumentNullException(nameof(world));

            PlayerInventoryViewModel = new InventoryViewModel(world.Player.Inventory, world.Player.HandInventorySlot);
            World = world;

            World.Player.PropertyChanged += OnPlayerPropertyChanged;

            UpdateInteractingBlockViewModel();
        }

        private void OnPlayerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(World.Player.InteractingBlock))
            {
                UpdateInteractingBlockViewModel();
            }
        }

        private void UpdateInteractingBlockViewModel()
        {
            switch (World.Player.InteractingBlock?.Object)
            {
                case BlastFurnaceBlock blastFurnaceBlock:
                    InteractingBlockViewModel = new BlastFurnaceViewModel(new InventoryViewModel(blastFurnaceBlock.Inventory, World.Player.HandInventorySlot));
                    break;
                default:
                    InteractingBlockViewModel = null;
                    break;
            }
        }
    }

    public class BlastFurnaceViewModel
    {
        public InventoryViewModel InventoryViewModel { get; }

        public BlastFurnaceViewModel(InventoryViewModel inventoryViewModel)
        {
            InventoryViewModel = inventoryViewModel;
        }
    }
}

