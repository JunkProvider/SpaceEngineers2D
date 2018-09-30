namespace SpaceEngineers2D.Physics
{
    using System.Collections.Generic;

    using Geometry;
    using Model;

    public class CollisionEngine
    {
        private ICoordinateSystem CoordinateSystem { get; }

        public CollisionEngine(ICoordinateSystem coordinateSystem)
        {
            CoordinateSystem = coordinateSystem;
        }
        
        public void DetectTouchedBlocks(ICollection<Grid> grids, IMobileObject obj)
        {
            foreach (var pair in obj.TouchedBlocks)
            {
                pair.Value.Clear();
            }
            
            var areaToCheck = IntRectangle.FromPositionAndSize(obj.Position, obj.Size).Extend(Constants.BlockSize);

            var objBounds = obj.Bounds;

            /* var objLeft = NormalizeX(obj.Position.X);
            var objRight = NormalizeX(obj.Position.X + obj.Size.X);
            var objTop = NormalizeX(obj.Position.Y);
            var objBottom = NormalizeX(obj.Position.Y + obj.Size.Y); */

            foreach (var grid in grids)
            {
                grid.ForEachWithin(areaToCheck, (block, blockPosition) =>
                {
                    if (block.IsSolid)
                    {
                        var blockBounds = CoordinateSystem.Denormalize(blockPosition, Constants.BlockSizeVector, objBounds.Position, objBounds.Size);

                        if (objBounds.TryGetTouchedSide(blockBounds, out var touchedSide))
                        {
                            obj.TouchedBlocks[touchedSide].Add(block);
                        }
                    }
                });
            }
        }

        public void Move(ICollection<Grid> grids, IMobileObject movingObj, IntVector offset)
        {
            MoveHorizontal(grids, movingObj, offset.X);
            MoveVertical(grids, movingObj, offset.Y);
        }

        public void MoveHorizontal(ICollection<Grid> grids, IMobileObject movingObj, int distance)
        {
            if (distance > 0)
            {
                MoveRight(grids, movingObj, distance);
            }
            else if (distance < 0)
            {
                MoveLeft(grids, movingObj, -distance);
            }
        }

        public void MoveRight(ICollection<Grid> grids, IMobileObject movingObj, int distance)
        {
            var movingObjBounds = movingObj.Bounds;

            var sweptArea = new IntRectangle(movingObjBounds.Right, movingObjBounds.Top, movingObjBounds.Front, distance, movingObjBounds.Height, movingObjBounds.Depth);

            var areaToCheck = sweptArea.Extend(Constants.BlockSize);

            foreach (var grid in grids)
            {
                grid.ForEachWithin(areaToCheck, (block, blockPos) =>
                {
                    var blockBounds = CoordinateSystem.Denormalize(blockPos, Constants.BlockSizeVector, sweptArea.Position, sweptArea.Size);

                    if (block.IsSolid && sweptArea.Intersects(blockBounds))
                    {
                        sweptArea = new IntRectangle(sweptArea.Left, sweptArea.Top, sweptArea.Front, blockBounds.Left - sweptArea.Left, sweptArea.Height, sweptArea.Depth);
                    }
                });
            }

            movingObj.Position = movingObj.Position.MoveRight(sweptArea.Width);
        }

        public void MoveLeft(ICollection<Grid> grids, IMobileObject movingObj, int distance)
        {
            var movingObjBounds = movingObj.Bounds;

            var sweptArea = new IntRectangle(movingObjBounds.Left - distance, movingObjBounds.Top, movingObjBounds.Front, distance, movingObjBounds.Height, movingObjBounds.Depth);

            var areaToCheck = sweptArea.Extend(Constants.BlockSize);

            foreach (var grid in grids)
            {
                grid.ForEachWithin(areaToCheck, (block, blockPos) =>
                {
                    var blockBounds = CoordinateSystem.Denormalize(blockPos, Constants.BlockSizeVector, sweptArea.Position, sweptArea.Size);

                    if (block.IsSolid && sweptArea.Intersects(blockBounds))
                    {
                        sweptArea = new IntRectangle(blockBounds.Right, sweptArea.Top, sweptArea.Front, sweptArea.Right - blockBounds.Right, sweptArea.Height, sweptArea.Depth);
                    }
                });
            }

            movingObj.Position = movingObj.Position.MoveLeft(sweptArea.Width);
        }

        public void MoveVertical(ICollection<Grid> grids, IMobileObject movingObj, int offset)
        {
            if (offset > 0)
            {
                MoveDown(grids, movingObj, offset);
            }
            else if (offset < 0)
            {
                MoveUp(grids, movingObj, -offset);
            }
        }

        public void MoveDown(ICollection<Grid> grids, IMobileObject movingObj, int distance)
        {
            var movingObjBounds = movingObj.Bounds;
            
            var sweptArea = new IntRectangle(movingObjBounds.Left, movingObjBounds.Bottom, movingObjBounds.Front, movingObjBounds.Width, distance, movingObjBounds.Depth);
            
            var areaToCheck = sweptArea.Extend(Constants.BlockSize);

            foreach (var grid in grids) {
                grid.ForEachWithin(areaToCheck, (block, blockPos) =>
                {
                    var blockBounds = CoordinateSystem.Denormalize(blockPos, Constants.BlockSizeVector, sweptArea.Position, sweptArea.Size);

                    if (block.IsSolid && sweptArea.Intersects(blockBounds))
                    {
                        sweptArea = new IntRectangle(sweptArea.Left, sweptArea.Top, sweptArea.Front, sweptArea.Width, blockBounds.Top - sweptArea.Top, sweptArea.Depth);
                    }
                });
            }

            movingObj.Position = movingObj.Position.MoveDown(sweptArea.Height);
        }

        public void MoveUp(ICollection<Grid> grids, IMobileObject movingObj, int distance)
        {
            var movingObjBounds = movingObj.Bounds;

            var sweptArea = new IntRectangle(movingObjBounds.Left, movingObjBounds.Top - distance, movingObjBounds.Front, movingObjBounds.Width, distance, movingObjBounds.Depth);

            var areaToCheck = sweptArea.Extend(Constants.BlockSize);

            foreach (var grid in grids)
            {
                grid.ForEachWithin(areaToCheck, (block, blockPos) =>
                {
                    var blockBounds = CoordinateSystem.Denormalize(blockPos, Constants.BlockSizeVector, sweptArea.Position, sweptArea.Size);

                    if (block.IsSolid && sweptArea.Intersects(blockBounds))
                    {
                        sweptArea = new IntRectangle(sweptArea.Left, blockBounds.Bottom, sweptArea.Front, sweptArea.Width, sweptArea.Bottom - blockBounds.Bottom, sweptArea.Depth);
                    }
                });
            }

            movingObj.Position = movingObj.Position.MoveUp(sweptArea.Height);
        }
    }
}
