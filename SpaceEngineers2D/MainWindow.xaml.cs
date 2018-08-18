using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SpaceEngineers2D.Chemistry;
using SpaceEngineers2D.Chemistry.Quantities;
using SpaceEngineers2D.Controllers;
using SpaceEngineers2D.Geometry;
using SpaceEngineers2D.Model;
using SpaceEngineers2D.Model.Blocks;
using SpaceEngineers2D.Physics;
using SpaceEngineers2D.View;
using SpaceEngineers2D.View.Inventory;
using Grid = SpaceEngineers2D.Model.Grid;

namespace SpaceEngineers2D
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public World World { get; }

        public WorldRendererParameters WorldRendererParameters { get; }

        private Random _random = new Random();

        public MainWindow()
        {
            World = new World(new Player { Position = new IntVector(0, -1600) }, new Camera { Zoom = 0.05f });

            var coalMixture = Mixture.FromSingleCompound(World.Compounds.C, Volume.FromLiters(100), Temperature.FromKelvin(293));
            var oreMixture = Mixture.FromAbsoluteAmounts(
                new Dictionary<Compound, Volume>
                {
                    { World.Compounds.Fe2O3, Volume.FromLiters(30) },
                    { World.Compounds.Fe3O4, Volume.FromLiters(70) }
                }, 
                Temperature.FromKelvin(293));

            var grid = new Grid();
            for (var x = -100; x < 100; x++)
            {
                for (var y = 0; y < 50; y++)
                {
                    if (y == 0)
                    {
                        grid.SetBlock(new IntVector(x, y) * Constants.PhysicsUnit, World.BlockTypes.Grass.InstantiateBlock());
                        continue;
                    }

                    if (_random.Next(0, 100) < 5)
                    {
                        grid.SetBlock(new IntVector(x, y) * Constants.PhysicsUnit, World.BlockTypes.MixtureBlockType.InstantiateBlock(coalMixture));
                        continue;
                    }

                    grid.SetBlock(new IntVector(x, y) * Constants.PhysicsUnit, World.BlockTypes.MixtureBlockType.InstantiateBlock(oreMixture));
                }
            }

            World.Grids.Add(grid);
            
            WorldRendererParameters = new WorldRendererParameters(World, new WorldRendererController(World));

            ToolTipService.ShowDurationProperty.OverrideMetadata(
                typeof(DependencyObject), new FrameworkPropertyMetadata(Int32.MaxValue));

            InitializeComponent();
            DataContext = this;
        }

        private void MainWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    World.Player.MovementOrders[Side.Left] = true;
                    break;
                case Key.Right:
                    World.Player.MovementOrders[Side.Right] = true;
                    break;
                case Key.Up:
                    World.Player.MovementOrders[Side.Top] = true;
                    break;
                case Key.Down:
                    World.Player.MovementOrders[Side.Bottom] = true;
                    break;
            }
        }

        private void MainWindow_OnKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    World.Player.MovementOrders[Side.Left] = false;
                    break;
                case Key.Right:
                    World.Player.MovementOrders[Side.Right] = false;
                    break;
                case Key.Up:
                    World.Player.MovementOrders[Side.Top] = false;
                    break;
                case Key.Down:
                    World.Player.MovementOrders[Side.Bottom] = false;
                    break;
                case Key.Space:
                    PickUpClosestItem();
                    break;
            }
        }

        private void PickUpClosestItem()
        {
            var player = World.Player;
            var item = GetClosestItem(player.Bounds.Center);

            if (item == null)
                return;

            if (player.Inventory.Put(item.Item))
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
    }
}
