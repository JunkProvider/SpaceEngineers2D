using System;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media;
using SpaceEngineers2D.Geometry;
using SpaceEngineers2D.Model;
using SpaceEngineers2D.Model.Entities;
using SpaceEngineers2D.Persistence;
using SpaceEngineers2D.View;

namespace SpaceEngineers2D
{
    public partial class MainWindow
    {
        private const string SaveFileDirectoryPath = "C:\\Users\\junkP\\Desktop\\tmp\\SpaceEngineers2DSaveGames\\";
        
        public ApplicationViewModel ApplicationViewModel { get; }

        private PersistenceService PersistenceService { get; }

        const Key LeftKey = Key.A;
        const Key RightKey = Key.D;
        const Key UpKey = Key.W;
        const Key DownKey = Key.S;

        public MainWindow()
        {
            PersistenceService = new PersistenceService(SaveFileDirectoryPath);

            ApplicationViewModel = new ApplicationViewModel(PersistenceService);

            DataContext = this;
            InitializeComponent();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (ApplicationViewModel.CanSaveGame())
            {
                ApplicationViewModel.SaveGame();
            }
        }

        private void MainWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (ApplicationViewModel.World == null)
                return;

            // If executed immediately nothing happens and debugged dies when setting a breakpoint.
            Dispatch.Exec(() => ApplicationViewModel.Player.TrySetSelectedBlueprintSlot(e.Key));

            switch (e.Key)
            {
                case LeftKey:
                    ApplicationViewModel.Player.MovementOrders[Side.Left] = true;
                    break;
                case RightKey:
                    ApplicationViewModel.Player.MovementOrders[Side.Right] = true;
                    break;
                case UpKey:
                    ApplicationViewModel.Player.MovementOrders[Side.Top] = true;
                    break;
                case DownKey:
                    ApplicationViewModel.Player.MovementOrders[Side.Bottom] = true;
                    break;
                case Key.Space:
                    PickUpClosestItem();
                    break;
                case Key.E:
                    Dispatch.Exec(ApplicationViewModel.WorldController.OnInteraction);
                    break;
                case Key.Z:
                    Dispatch.Exec(ApplicationViewModel.WorldController.OnToggleBlockPlacementLayer);
                    break;
            }
        }

        private void MainWindow_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (ApplicationViewModel.World == null)
                return;

            switch (e.Key)
            {
                case LeftKey:
                    ApplicationViewModel.Player.MovementOrders[Side.Left] = false;
                    break;
                case RightKey:
                    ApplicationViewModel.Player.MovementOrders[Side.Right] = false;
                    break;
                case UpKey:
                    ApplicationViewModel.Player.MovementOrders[Side.Top] = false;
                    break;
                case DownKey:
                    ApplicationViewModel.Player.MovementOrders[Side.Bottom] = false;
                    break;
            }
        }

        private void PickUpClosestItem()
        {
            if (ApplicationViewModel.World == null)
                return;

            var player = ApplicationViewModel.Player;
            var item = GetClosestItem(player.Bounds.Center);

            if (item == null)
                return;

            player.Inventory.Put(item.ItemStack);

            if (item.ItemStack.Size == 0)
            {
                ApplicationViewModel.World.Items.Remove(item);
            }
        }

        private MovableItem GetClosestItem(IntVector position)
        {
            var closestDistance = double.PositiveInfinity;
            MovableItem closestItem = null;

            foreach (var item in ApplicationViewModel.World.Items)
            {
                var itemPosition = item.Bounds.Center - position;
                var itemDistance = itemPosition.X * itemPosition.X + itemPosition.Y * itemPosition.Y;
                if (itemDistance <= Math.Pow(Player.ItemPickupRange, 2) && itemDistance < closestDistance)
                {
                    closestDistance = itemDistance;
                    closestItem = item;
                }
            }

            return closestItem;
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            MousePositionProvider.Instance.AbsoluteMousePosition = Mouse.GetPosition(this);
        }
    }
}
