using System;
using System.Linq;
using SpaceEngineers2D.Chemistry.Quantities;
using SpaceEngineers2D.Geometry;
using SpaceEngineers2D.Model;
using SpaceEngineers2D.Model.Blocks;
using SpaceEngineers2D.Model.Items;
using SpaceEngineers2D.Physics;

namespace SpaceEngineers2D.Controllers
{
    public class WorldRendererController
    {
        private const double PlayerRange = 2;

        private IntVector _mousePosition;

        private int _activity = 0;

        public World World { get; }

        public WorldRendererController(World world)
        {
            World = world;
        }

        public void OnMouseMove(IntVector mousePosition)
        {
            _mousePosition = mousePosition;
        }

        public void OnLeftMouseButtonDown(IntVector mousePosition)
        {
            _activity = 1;
        }

        public void OnRightMouseButtonDown(IntVector mousePosition)
        {
            _activity = 2;

            var player = World.Player;

            if (player.TargetBlock == null)
            {
                var grid = World.Grids.First();
                var blockBounds = grid.GetBlockBounds(mousePosition);

                if (!player.Bounds.Overlaps(blockBounds) && IsInPlayerRange(blockBounds))
                {
                    PlaceBlock(new BlockCoords(grid, blockBounds));
                }
            }
        }

        public void OnLeftMouseButtonUp(IntVector mousePosition)
        {
            if (_activity == 1)
            {
                _activity = 0;
            }
        }

        public void OnRightMouseButtonUp(IntVector mousePosition)
        {
            if (_activity == 2)
            {
                _activity = 0;
            }
        }

        public void OnUpdate(TimeSpan elapsedTime)
        {
            var player = World.Player;

            var block = World.GetBlock(_mousePosition);

            this.UpdateItemsInPlayerInventory(elapsedTime);

            player.TargetPosition = _mousePosition;
            player.TargetBlockCoords = block != null ? new BlockCoords(block.Grid, block.Bounds) : null;
            player.TargetBlockCoordsInRange = player.TargetBlockCoords != null && IsInPlayerRange(player.TargetBlockCoords.Bounds);
            player.TargetBlock = block;

            if (_activity == 2 && player.TargetBlockCoordsInRange)
            {
                player.TargetBlock.As<StructuralBlock>((structuralBlock) => WeldBlock(structuralBlock, elapsedTime));
            }

            if (_activity == 1 && player.TargetBlockCoordsInRange)
            {
                GrindBlock(elapsedTime);
            }
        }

        private void UpdateItemsInPlayerInventory(TimeSpan elapsedTime)
        {
            var player = World.Player;

            foreach (var inventorySlot in player.Inventory.Slots)
            {
                if (inventorySlot.Item is MixtureItem mixtureItem)
                {
                    var reactedMixture = World.ReactionService.Check(mixtureItem.Mixture, Temperature.FromKelvin(2000), elapsedTime);

                    if (!reactedMixture.Equals(mixtureItem.Mixture))
                    {
                        inventorySlot.Item = new MixtureItem(reactedMixture);
                    }
                }
            }
        }

        private void PlaceBlock(BlockCoords coords)
        {
            var player = World.Player;
            var blockType = World.BlockTypes.Concrete;

            var block = blockType.InstantiateBlock();
            block.BlueprintState.Weld(player.Inventory, 1);

            if (!block.IsDestoryed)
            {
                coords.Grid.SetBlock(coords.Bounds.Position, block);
            }
        }

        private void WeldBlock(BlockInWorld<StructuralBlock> block, TimeSpan elapsedTime)
        {
            block.Object.BlueprintState.Weld(World.Player.Inventory, 50 * (float)elapsedTime.TotalSeconds);
        }

        private void GrindBlock(TimeSpan elapsedTime)
        {
            var block = World.Player.TargetBlock;

            block.Object.Damage(20 * (float)elapsedTime.TotalSeconds);

            if (block.Object.IsDestoryed)
            {
                OnDestroyBlock(block);
            }
        }

        private void OnDestroyBlock(IBlockInWorld block)
        {
            World.RemoveBlock(block.Bounds.Position);

            foreach (var itemStack in block.Object.GetDroppedItems())
            {
                /* var itemObj = new MobileItem(itemStack) { Position = command.Block.AbsolutePosition };
                command.World.Items.Add(itemObj); */

                World.Player.Inventory.Put(itemStack);
            }
        }

        private bool IsInPlayerRange(IntRectangle rectangle)
        {
            var playerCenter = World.Player.Bounds.Center;
            var x = rectangle.Left > playerCenter.X ? rectangle.Left : rectangle.Right;
            var y = rectangle.Top > playerCenter.Y ? rectangle.Top : rectangle.Bottom;
            return IsInPlayerRange(new IntVector(x, y));
        }

        private bool IsInPlayerRange(IntVector point)
        {
            return (point - World.Player.Bounds.Center).SquareLength <= Math.Pow(PlayerRange * Constants.PhysicsUnit, 2);
        }
    }
}
