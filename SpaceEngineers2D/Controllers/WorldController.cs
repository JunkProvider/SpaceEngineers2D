using System;
using System.Linq;
using SpaceEngineers2D.Geometry;
using SpaceEngineers2D.Model;
using SpaceEngineers2D.Model.Blocks;
using SpaceEngineers2D.Model.Entities;
using SpaceEngineers2D.Physics;

namespace SpaceEngineers2D.Controllers
{
    public class WorldController
    {
        private const double PlayerRange = 2;
        
        private IntVector _mousePosition;

        private int _activity = 0;

        public World World { get; }

        private Player Player { get; }

        public WorldController(World world)
        {
            World = world;
            Player = world.Entities.OfType<Player>().First();
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

            if (Player.BlockPlacementLayer == ZLayer.Background)
                mousePosition.Z = Constants.BlockSize;
            
            if (Player.TargetBlock == null)
            {
                var grid = World.Grids.First();
                var blockBounds = grid.GetBlockBounds(mousePosition);

                if (!Player.Bounds.Intersects(blockBounds) && IsInPlayerRange(blockBounds))
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

        public void OnInteraction()
        {
            var oldInteractingBlock = Player.InteractingBlock;

            if (oldInteractingBlock != null)
            {
                oldInteractingBlock.Object.OnInteractionEnded();
                Player.InteractingBlock = null;
            }

            if (Player.TargetBlock == null || oldInteractingBlock?.Object == Player.TargetBlock.Object)
            {
                return;
            }

            var interactionResult = Player.TargetBlock.Object.OnInteraction(new Block.OnInteractionContext(World, Player.TargetBlock.Bounds));

            if (interactionResult == Block.InteractionResult.Continuing)
            {
                Player.InteractingBlock = Player.TargetBlock;
            }
        }

        public void OnToggleBlockPlacementLayer()
        {
            Player.ToggleBlockPlacementLayer();
        }

        public void OnUpdate(TimeSpan elapsedTime)
        {
            foreach (var grid in World.Grids)
            {
                foreach (var blockToUpdate in grid.GetAll())
                {
                    blockToUpdate.OnUpdate(World, elapsedTime);
                }
            }

            foreach (var entity in World.Entities)
            {
                entity.Update(World, elapsedTime);
            }

            var mousePosition = _mousePosition;
            if (Player.BlockPlacementLayer == ZLayer.Background)
                mousePosition += IntVector.Back * Constants.BlockSize;

            var block = World.GetBlock(mousePosition);
            
            Player.TargetPosition = mousePosition;
            Player.TargetBlockCoords = block != null ? new BlockCoords(block.Grid, block.Bounds) : null;
            Player.TargetBlockCoordsInRange = Player.TargetBlockCoords != null && IsInPlayerRange(Player.TargetBlockCoords.Bounds);
            Player.TargetBlock = block;

            if (_activity == 2 && Player.TargetBlockCoordsInRange)
            {
                Player.TargetBlock.As<StructuralBlock>(structuralBlock => WeldBlock(structuralBlock, elapsedTime));
            }

            if (_activity == 1 && Player.TargetBlockCoordsInRange)
            {
                GrindBlock(elapsedTime);
            }
        }

        private void PlaceBlock(BlockCoords coords)
        {
            var blockType = Player.SelectedBlueprintSlot?.BlueprintedBlock;

            if (blockType != null && Player.Inventory.TryTakeNOfType(blockType.Blueprint.Components.First().ItemType, 1, out var itemStack))
            {
                var block = blockType.InstantiateBlock();
                block.BlueprintState.PutItem(itemStack.Item);
                coords.Grid.SetBlock(coords.Bounds.Position, block);
            }
        }

        private void WeldBlock(BlockInWorld<StructuralBlock> block, TimeSpan elapsedTime)
        {
            block.Object.BlueprintState.Weld(Player.Inventory, 10 * (float)elapsedTime.TotalSeconds);
        }

        private void GrindBlock(TimeSpan elapsedTime)
        {
            var block = Player.TargetBlock;

            block.Object.Damage(10 * (float)elapsedTime.TotalSeconds);

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

                Player.Inventory.Put(itemStack);
            }
        }

        private bool IsInPlayerRange(IntRectangle rectangle)
        {
            var playerCenter = Player.Bounds.Center;
            var x = rectangle.Left > playerCenter.X ? rectangle.Left : rectangle.Right;
            var y = rectangle.Top > playerCenter.Y ? rectangle.Top : rectangle.Bottom;
            return IsInPlayerRange(new IntVector(x, y, Player.Position.Z));
        }

        private bool IsInPlayerRange(IntVector point)
        {
            return (point - Player.Bounds.Center).SquareLength <= Math.Pow(PlayerRange * Constants.BlockSize, 2);
        }
    }
}
