namespace SpaceEngineers2D.Physics
{
    using System.Collections.Generic;

    using Geometry;
    using Model;

    public class CollisionEngine
    {
        public void DetectTouchedBlocks(ICollection<Grid> grids, IMobileObject obj)
        {
            foreach (var pair in obj.TouchedBlocks)
            {
                pair.Value.Clear();
            }

            foreach (var grid in grids)
            {
                var objBounds = IntRectangle.FromPositionAndSize(obj.Position, obj.Size);
                var extendedObjBounds = IntRectangle.FromPositionAndSize(obj.Position, obj.Size).Extend(Constants.PhysicsUnit);

                grid.ForEachWithin(extendedObjBounds, (block, blockPosition) =>
                {
                    var blockBounds = IntRectangle.FromPositionAndSize(
                        blockPosition,
                        IntVector.RightBottom * Constants.PhysicsUnit);

                    if (block.IsSolid && objBounds.TryGetTouchedSide(blockBounds, out var touchedSide))
                    {
                        obj.TouchedBlocks[touchedSide].Add(block);
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
                MoveLeft(grids, movingObj, distance);
            }
        }

        public void MoveRight(ICollection<Grid> grids, IMobileObject movingObj, int distance)
        {
            var offset = distance;
            // var touchedBlocks: BlockWithOffset[] = [];

            foreach (var grid in grids) {
                var movingObjPos = movingObj.Position;
                var rightOrig = movingObjPos.X + movingObj.Size.X;
                var rightDest = rightOrig + offset;
                var top = movingObjPos.Y;
                var bottom = movingObjPos.Y + movingObj.Size.Y;

                var areaToCheck = IntRectangle.FromXYWidthAndHeight(rightOrig, top, rightDest - rightOrig, bottom - top)
                    .Extend(Constants.PhysicsUnit);

                grid.ForEachWithin(areaToCheck, (block, blockPos) => {
                    if (block.IsSolid && top < blockPos.Y + Constants.PhysicsUnit && bottom > blockPos.Y)
                    {
                        if (rightDest >= blockPos.X && rightOrig <= blockPos.X)
                        {
                            rightDest = blockPos.X;
                            offset = rightDest - rightOrig;
                            // touchedBlocks.push(new BlockWithOffset(block, offset));
                        }
                    }
                });
            }

            if (offset != 0)
            {
                movingObj.Position = movingObj.Position.AddX(offset);
                // movingObj.touchedBlocks.items[Side.Left] = [];
                // movingObj.touchedBlocks.items[Side.Right] = touchedBlocks.filter(b => b.offset <= offset).map(b => b.block);
            }
        }

        public void MoveLeft(ICollection<Grid> grids, IMobileObject movingObj, int distance)
        {
            var offset = distance;
            // var touchedBlocks: BlockWithOffset[] = [];

            foreach (var grid in grids) {
                var movingObjPos = movingObj.Position;
                var leftOrig = movingObjPos.X;
                var leftDest = leftOrig + offset;
                var top = movingObjPos.Y;
                var bottom = movingObjPos.Y + movingObj.Size.Y;

                var areaToCheck = IntRectangle.FromXYWidthAndHeight(leftDest, top, leftOrig - leftDest, bottom - top)
                    .Extend(Constants.PhysicsUnit);

                grid.ForEachWithin(areaToCheck, (block, blockPos) => {
                    if (block.IsSolid && top < blockPos.Y + Constants.PhysicsUnit && bottom > blockPos.Y)
                    {
                        if (leftDest <= blockPos.X + Constants.PhysicsUnit && leftOrig >= blockPos.X + Constants.PhysicsUnit)
                        {
                            leftDest = blockPos.X + Constants.PhysicsUnit;
                            offset = leftDest - leftOrig;
                        }
                    }
                });
            }

            movingObj.Position = movingObj.Position.AddX(offset);
        }

        public void MoveVertical(ICollection<Grid> grids, IMobileObject movingObj, int offset)
        {
            if (offset > 0)
            {
                MoveDown(grids, movingObj, offset);
            }
            else if (offset < 0)
            {
                MoveUp(grids, movingObj, offset);
            }
        }

        public void MoveDown(ICollection<Grid> grids, IMobileObject movingObj, int distance)
        {
            var offset = distance;

            foreach (var grid in grids) {
                var movingObjPos = movingObj.Position;
                var bottomOrig = movingObjPos.Y + movingObj.Size.Y;
                var bottomDest = bottomOrig + offset;
                var left = movingObjPos.X;
                var right = movingObjPos.X + movingObj.Size.X;

                var areaToCheck = IntRectangle.FromXYWidthAndHeight(left, bottomOrig, right - left, bottomDest - bottomOrig)
                    .Extend(Constants.PhysicsUnit);

                grid.ForEachWithin(areaToCheck, (block, blockPos) => {
                    if (block.IsSolid && left < blockPos.X + Constants.PhysicsUnit && right > blockPos.X)
                    {
                        if (bottomDest > blockPos.Y && bottomOrig <= blockPos.Y)
                        {
                            bottomDest = blockPos.Y;
                            offset = bottomDest - bottomOrig;
                        }
                    }
                });
            }

            movingObj.Position = movingObj.Position.AddY(offset);
        }

        public void MoveUp(ICollection<Grid> grids, IMobileObject movingObj, int distance)
        {
            var offset = distance;

            foreach (var grid in grids) {
                var movingObjPos = movingObj.Position;
                var topOrig = movingObjPos.Y;
                var topDest = topOrig + offset;
                var left = movingObjPos.X;
                var right = movingObjPos.X + movingObj.Size.X;

                var areaToCheck = IntRectangle.FromXYWidthAndHeight(left, topOrig, right - left, topOrig - topDest)
                    .Extend(Constants.PhysicsUnit);

                grid.ForEachWithin(areaToCheck, (block, blockPos) => {
                    if (block.IsSolid && left < blockPos.X + Constants.PhysicsUnit && right > blockPos.X)
                    {
                        if (topDest < blockPos.Y + Constants.PhysicsUnit && topOrig >= blockPos.Y + Constants.PhysicsUnit)
                        {
                            topDest = blockPos.Y + Constants.PhysicsUnit;
                            offset = topDest - topOrig;
                        }
                    }
                });
            }

            movingObj.Position = movingObj.Position.AddY(offset);
        }
    }
}
