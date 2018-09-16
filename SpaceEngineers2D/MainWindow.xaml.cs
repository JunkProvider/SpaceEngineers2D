using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;
using Newtonsoft.Json;
using SpaceEngineers2D.Controllers;
using SpaceEngineers2D.Geometry;
using SpaceEngineers2D.Model;
using SpaceEngineers2D.Persistence;
using SpaceEngineers2D.Persistence.DataModel;
using SpaceEngineers2D.Physics;
using SpaceEngineers2D.View;

namespace SpaceEngineers2D
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string SaveFilePath = "C:\\Users\\junkP\\Desktop\\tmp\\SpaceEngineers2D.json";

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
            var dict = new Dictionary<string, object>();

            //return;
            World = new World(new Player { Position = new IntVector(0, -1600) }, new Camera { Zoom = 0.05f });

            var grid = new Grid(0);
            World.Grids.Add(grid);

            if (!TryLoad())
            {
                CreateEnvironment(grid);
            }

            World.Player.BlueprintSlots[0].BlueprintedBlock = World.BlockTypes.Concrete;
            World.Player.BlueprintSlots[1].BlueprintedBlock = World.BlockTypes.IronPlate;

            WorldRendererParameters = new WorldRendererParameters(World, new WorldRendererController(World));

            ApplicationViewModel = new ApplicationViewModel(World);

            DataContext = this;
            
            InitializeComponent();
        }

        private void CreateEnvironment(Grid grid)
        {
            for (var x = -100; x < 100; x++)
            {
                for (var y = 0; y < 50; y++)
                {
                    if (y == 0)
                    {
                        if (_random.Next(0, 100) < 80)
                            grid.SetBlock(new IntVector(x, y) * Constants.PhysicsUnit,
                                World.BlockTypes.Grass.InstantiateBlock());

                        continue;
                    }

                    if (y == 1)
                    {
                        grid.SetBlock(new IntVector(x, y) * Constants.PhysicsUnit,
                            World.BlockTypes.DirtWithGrass.Instantiate());
                        continue;
                    }

                    if (y == 2 || (y == 3 && _random.Next(0, 100) < 50))
                    {
                        grid.SetBlock(new IntVector(x, y) * Constants.PhysicsUnit, World.BlockTypes.Dirt.Instantiate());
                        continue;
                    }

                    if (y == 4 && _random.Next(0, 100) < 50)
                    {
                        var topBlock = grid.GetBlock(new IntVector(x, y - 1) * Constants.PhysicsUnit);
                        if (topBlock.Object.BlockType == World.BlockTypes.Dirt)
                        {
                            grid.SetBlock(new IntVector(x, y) * Constants.PhysicsUnit, World.BlockTypes.Dirt.Instantiate());
                            continue;
                        }
                    }

                    if (_random.Next(0, 100) < 5)
                    {
                        grid.SetBlock(new IntVector(x, y) * Constants.PhysicsUnit,
                            World.BlockTypes.IronOreDeposit.Instantiate());
                        continue;
                    }

                    if (_random.Next(0, 100) < 5)
                    {
                        grid.SetBlock(new IntVector(x, y) * Constants.PhysicsUnit, World.BlockTypes.CoalDeposit.Instantiate());
                        continue;
                    }

                    grid.SetBlock(new IntVector(x, y) * Constants.PhysicsUnit, World.BlockTypes.Rock.Instantiate());
                }
            }

            for (var x = -100; x < 100; x++)
            {
                if (_random.Next(0, 100) > 10)
                    continue;

                var height = _random.Next(2, 4);

                for (var y = -height; y <= 0; y++)
                {
                    grid.SetBlock(new IntVector(x, y) * Constants.PhysicsUnit, World.BlockTypes.Reed.InstantiateBlock());
                }
            }

            var blastFurnace = World.BlockTypes.BlastFurnace.Instantiate();
            blastFurnace.BlueprintState.FinishImmediately();
            grid.SetBlock(new IntVector(-2, 0) * Constants.PhysicsUnit, blastFurnace);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            Save();
        }

        private bool TryLoad()
        {
            if (!File.Exists(SaveFilePath))
            {
                return false;
            }

            var json = File.ReadAllText(SaveFilePath);

            var data = JsonConvert.DeserializeObject<WorldData>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });

            var deserializer = new Deserializer(World.BlockTypes, World.ItemTypes);
            deserializer.MapWorld(World, data);

            return true;
        }

        private void Save()
        {
            var serializer = new Serializer();
            var data = serializer.MapWorld(World);

            var json = JsonConvert.SerializeObject(data, new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.All
            });

            File.WriteAllText(SaveFilePath, json);
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
