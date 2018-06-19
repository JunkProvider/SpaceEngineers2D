using System.Linq;
using SpaceEngineers2D.Model.Chemicals;
using SpaceEngineers2D.Physics;

namespace SpaceEngineers2D.Model
{
    using System.Collections.Generic;

    using SpaceEngineers2D.Geometry;
    using SpaceEngineers2D.Model.Blocks;
    using SpaceEngineers2D.Model.Items;

    public class World : IGridContainer
    {
        public ElementList Elements { get; }

        public CompoundList Compounds { get; }

        public ItemTypes ItemTypes { get; }

        public BlockTypes BlockTypes { get; }

        public ICollection<Grid> Grids { get; set; } = new List<Grid>();

        public Player Player { get; set; }

        public ISet<MobileItem> Items { get; set; } = new HashSet<MobileItem>();

        public Camera Camera { get; set; }

        public IntVector Gravity { get; set; } = new IntVector(0, 9810);

        public World(Player player, Camera camera)
        {
            Player = player;
            Camera = camera;
            Elements = new ElementList();
            Compounds = new CompoundList(Elements);
            ItemTypes = new ItemTypes(Compounds);
            BlockTypes = new BlockTypes(ItemTypes);
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
            var areaAroundBlock = IntRectangle.FromPositionAndSize(positionOfBlockToRemove, IntVector.RightBottom * Constants.PhysicsUnit).Extend(Constants.PhysicsUnit);
            
            foreach (var grid in Grids)
            {
                var removedBlock = grid.SetBlock(positionOfBlockToRemove, null);

                if (removedBlock == null)
                {
                    continue;
                }

                grid.ForEachWithin(areaAroundBlock, (block, position) =>
                {
                    if (block != null && block != removedBlock)
                    {
                        block.OnNeighborChanged(this, IntRectangle.FromPositionAndSize(position, Constants.PhysicsUnitVector), new BlockInWorld<Block>(block, grid, positionOfBlockToRemove));
                    }
                });
            }
        }
    }
}
