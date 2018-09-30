namespace SpaceEngineers2D.Model
{
    using System.Collections.Generic;
    using Physics;
    using Geometry;
    using Blocks;
    using Items;

    public class World : IGridContainer
    {
        public ItemTypes ItemTypes { get; }

        public BlockTypes BlockTypes { get; }

        public ICollection<Grid> Grids { get; set; } = new List<Grid>();

        public int Width => CoordinateSystem.MaxX - CoordinateSystem.MinX;

        public ICoordinateSystem CoordinateSystem { get; }

        public Player Player { get; set; }

        public ISet<MobileItem> Items { get; set; } = new HashSet<MobileItem>();

        public Camera Camera { get; set; }

        public IntVector Gravity { get; set; } = IntVector.Down * 9810;

        public World(Player player, Camera camera, int width)
        {
            Player = player;
            Camera = camera;
            ItemTypes = new ItemTypes();
            BlockTypes = new BlockTypes(ItemTypes);
            CoordinateSystem = Model.CoordinateSystem.CreateHorizontalCircular(0, width);
        }

        public bool IsBottomBlock(IntVector position, IntVector bottomBlockPosition)
        {
            CoordinateSystem.Denormalize(bottomBlockPosition, position);
            return bottomBlockPosition.X == position.X && bottomBlockPosition.Y == position.Y + Constants.BlockSize;
        }

        public IBlockInWorld GetBottomBlock(IntVector position)
        {
            return GetBlock(position + IntVector.Down * Constants.BlockSize);
        }

        public IBlockInWorld GetBlock(IntVector position)
        {
            foreach (var grid in Grids)
            {
                var block = grid.GetBlock(position);
                if (block != null)
                {
                    return block;
                }
            }

            return null;
        }

        public void RemoveBlock(IntVector positionOfBlockToRemove)
        {
            var areaAroundBlock = IntRectangle.FromPositionAndSize(positionOfBlockToRemove, IntVector.RightDown * Constants.BlockSize).Extend(Constants.BlockSize);
            
            foreach (var grid in Grids)
            {
                var removedBlock = grid.SetBlock(positionOfBlockToRemove, null);

                if (removedBlock == null)
                {
                    continue;
                }

                // var removedBlockBounds = IntRectangle.FromPositionAndSize(positionOfBlockToRemove, Constants.PhysicsUnitVector);

                grid.ForEachWithin(areaAroundBlock, (block, position) =>
                {
                    if (block != null && block != removedBlock)
                    {
                        // var blockBounds = IntRectangle.FromPositionAndSize(position, Constants.PhysicsUnitVector);
                        block.OnNeighborChanged(
                            this,
                            IntRectangle.FromPositionAndSize(position, Constants.BlockSizeVector),
                            new BlockInWorld<Block>(block, grid, positionOfBlockToRemove));
                    }
                });
            }
        }
    }
}
