using System;
using System.Linq;
using SpaceEngineers2D.Geometry;
using SpaceEngineers2D.Model;
using SpaceEngineers2D.Model.Blocks;
using SpaceEngineers2D.Physics;

namespace SpaceEngineers2D.Controllers
{
    public class WorldController
    {
        private const double PlayerRange = 2;
        
        private IntVector _mousePosition;

        private int _activity = 0;

        public World World { get; }

        private Player Player => World.Player;

        public WorldController(World world)
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
            
            if (Player.TargetBlock == null)
            {
                var grid = World.Grids.First();
                var blockBounds = grid.GetBlockBounds(mousePosition);

                if (!Player.Bounds.Overlaps(blockBounds) && IsInPlayerRange(blockBounds))
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

        public void OnUpdate(TimeSpan elapsedTime)
        {
            foreach (var grid in World.Grids)
            {
                grid.ForEach((blockToUpdate, position) => blockToUpdate.OnUpdate(World, new IntRectangle(position, Constants.PhysicsUnitVector), elapsedTime));
            }

            var block = World.GetBlock(_mousePosition);

            Player.TargetPosition = _mousePosition;
            Player.TargetBlockCoords = block != null ? new BlockCoords(block.Grid, block.Bounds) : null;
            Player.TargetBlockCoordsInRange = Player.TargetBlockCoords != null && IsInPlayerRange(Player.TargetBlockCoords.Bounds);
            Player.TargetBlock = block;

            if (_activity == 2 && Player.TargetBlockCoordsInRange)
            {
                Player.TargetBlock.As<StructuralBlock>((structuralBlock) => WeldBlock(structuralBlock, elapsedTime));
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
                // player.WeldedBlock = new BlockInWorld<StructuralBlock>(block, coords.Grid, coords.Bounds.Position);
            }
        }

        private void WeldBlock(BlockInWorld<StructuralBlock> block, TimeSpan elapsedTime)
        {
            block.Object.BlueprintState.Weld(World.Player.Inventory, 10 * (float)elapsedTime.TotalSeconds);
        }

        private void GrindBlock(TimeSpan elapsedTime)
        {
            var block = World.Player.TargetBlock;

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
