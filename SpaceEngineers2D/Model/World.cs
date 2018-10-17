using System.Linq;
using SpaceEngineers2D.Model.Entities;
using SpaceEngineers2D.View.Inventory;

namespace SpaceEngineers2D.Model
{
    using System.Collections.Generic;
    using Physics;
    using Geometry;
    using Blocks;
    using Items;

    public class World : PropertyObservable, IGridContainer, IPhysicsEngineContext
    {
        public BlockTypes BlockTypes { get; }

        public EntityTypes EntityTypes { get; }

        public ItemTypes ItemTypes { get; }

        public ICollection<Grid> Grids { get; set; } = new List<Grid>();

        public ISet<IEntity> Entities { get; set; } = new HashSet<IEntity>();

        public int Width => CoordinateSystem.MaxX - CoordinateSystem.MinX;

        public ICoordinateSystem CoordinateSystem { get; }

        public ISet<MovableItem> Items { get; set; } = new HashSet<MovableItem>();

        public Camera Camera { get; set; }

        public IntVector Gravity { get; set; } = IntVector.Down * 9810;

        public World(BlockTypes blockTypes, EntityTypes entityTypes, ItemTypes itemTypes, Camera camera, int width)
        {
            Camera = camera;
            BlockTypes = blockTypes;
            EntityTypes = entityTypes;
            ItemTypes = itemTypes;
            
            CoordinateSystem = Model.CoordinateSystem.CreateHorizontalCircular(0, width);
        }

        public IEnumerable<ICollidable> GetCollidableObjectsWithin(IntRectangle rectangle)
        {
            return Grids.SelectMany(grid => grid.GetAllWithin(rectangle)).Cast<ICollidable>().Concat(Entities);
        }

        public IEnumerable<IMovableObject> GetMovableObjects()
        {
            return Entities;
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

                foreach (var block in grid.GetAllWithin(areaAroundBlock))
                {
                    if (block != null && block != removedBlock)
                    {
                        // var blockBounds = IntRectangle.FromPositionAndSize(position, Constants.PhysicsUnitVector);
                        block.OnNeighborChanged(
                            this,
                            new BlockInWorld<Block>(block, grid, positionOfBlockToRemove));
                    }
                }
            }
        }
    }
}
