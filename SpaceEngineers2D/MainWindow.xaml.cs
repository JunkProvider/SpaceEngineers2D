using System;
using System.Windows;
using System.Windows.Input;
using SpaceEngineers2D.Controllers;
using SpaceEngineers2D.Geometry;
using SpaceEngineers2D.Model;
using SpaceEngineers2D.Physics;
using SpaceEngineers2D.View;

namespace SpaceEngineers2D
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public World World { get; }

        public WorldRendererParameters WorldRendererParameters { get; }

        public ApplicationViewModel ApplicationViewModel { get; }

        private readonly Random _random = new Random();

        const Key LeftKey = Key.A;
        const Key RightKey = Key.D;
        const Key UpKey = Key.W;
        const Key DownKey = Key.S;

        public MainWindow()
        {
            World = new World(new Player { Position = new IntVector(0, -1600) }, new Camera { Zoom = 0.05f });

            var grid = new Grid();
            for (var x = -100; x < 100; x++)
            {
                for (var y = 0; y < 50; y++)
                {
                    if (y == 0)
                    {
                        if (_random.Next(0, 100) < 80)
                            grid.SetBlock(new IntVector(x, y) * Constants.PhysicsUnit, World.BlockTypes.Grass.InstantiateBlock());

                        continue;
                    }

                    if (y == 1)
                    {
                        grid.SetBlock(new IntVector(x, y) * Constants.PhysicsUnit, World.BlockTypes.DirtWithGrass.InstantiateBlock());
                        continue;
                    }

                    if (y == 2 || (y == 3 && _random.Next(0, 100) < 50))
                    {
                        grid.SetBlock(new IntVector(x, y) * Constants.PhysicsUnit, World.BlockTypes.Dirt.InstantiateBlock());
                        continue;
                    }

                    if (y == 4  && _random.Next(0, 100) < 50)
                    {
                        var topBlock = grid.GetBlock(new IntVector(x, y - 1) * Constants.PhysicsUnit);
                        if (topBlock.Object.BlockType == World.BlockTypes.Dirt)
                        {
                            grid.SetBlock(new IntVector(x, y) * Constants.PhysicsUnit, World.BlockTypes.Dirt.InstantiateBlock());
                            continue;
                        }
                    }

                    if (_random.Next(0, 100) < 5)
                    {
                        grid.SetBlock(new IntVector(x, y) * Constants.PhysicsUnit, World.BlockTypes.IronOreDeposit.InstantiateBlock());
                        continue;
                    }

                    if (_random.Next(0, 100) < 5)
                    {
                        grid.SetBlock(new IntVector(x, y) * Constants.PhysicsUnit, World.BlockTypes.CoalDeposit.InstantiateBlock());
                        continue;
                    }

                    grid.SetBlock(new IntVector(x, y) * Constants.PhysicsUnit, World.BlockTypes.Rock.InstantiateBlock());
                }
            }

            var blastFurnace = World.BlockTypes.BlastFurnace.InstantiateBlock();
            blastFurnace.BlueprintState.FinishImmediately();
            grid.SetBlock(new IntVector(-2, 0) * Constants.PhysicsUnit, blastFurnace);

            World.Grids.Add(grid);

            World.Player.BlueprintSlots[0].BlueprintedBlock = World.BlockTypes.Concrete;
            World.Player.BlueprintSlots[1].BlueprintedBlock = World.BlockTypes.IronPlate;

            WorldRendererParameters = new WorldRendererParameters(World, new WorldRendererController(World));

            ApplicationViewModel = new ApplicationViewModel(World);

            DataContext = this;
            
            InitializeComponent();
        }

        private void MainWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            // If executed immediately nothing happens and debugged dies when setting a breakpoint.
            Dispatch.Exec(() => World.Player.TrySetSelectedBlueprintSlot(e.Key));

            switch (e.Key)
            {
                case LeftKey:
                    World.Player.MovementOrders[Side.Left] = true;
                    break;
                case RightKey:
                    World.Player.MovementOrders[Side.Right] = true;
                    break;
                case UpKey:
                    World.Player.MovementOrders[Side.Top] = true;
                    break;
                case DownKey:
                    World.Player.MovementOrders[Side.Bottom] = true;
                    break;
                case Key.Space:
                    PickUpClosestItem();
                    break;
                case Key.E:
                    Dispatch.Exec(WorldRendererParameters.Controller.OnInteraction);
                    break;
            }
        }

        private void MainWindow_OnKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case LeftKey:
                    World.Player.MovementOrders[Side.Left] = false;
                    break;
                case RightKey:
                    World.Player.MovementOrders[Side.Right] = false;
                    break;
                case UpKey:
                    World.Player.MovementOrders[Side.Top] = false;
                    break;
                case DownKey:
                    World.Player.MovementOrders[Side.Bottom] = false;
                    break;
            }
        }

        private void PickUpClosestItem()
        {
            var player = World.Player;
            var item = GetClosestItem(player.Bounds.Center);

            if (item == null)
                return;

            player.Inventory.Put(item.ItemStack);

            if (item.ItemStack.Size == 0)
            {
                World.Items.Remove(item);
            }
        }

        private MobileItem GetClosestItem(IntVector position)
        {
            var closestDistance = double.PositiveInfinity;
            MobileItem closestItem = null;

            foreach (var item in World.Items)
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
