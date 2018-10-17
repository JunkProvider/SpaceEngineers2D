using System;
using System.Linq;
using SpaceEngineers2D.Model.Entities;

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
        
        public void DetectTouchedBlocks(ICollisionEngineContext context, IMovableObject checkedObject)
        {
            checkedObject.TouchedObjects.Clear();

            var areaToCheck = IntRectangle.FromPositionAndSize(checkedObject.Position, checkedObject.Size).Extend(Constants.BlockSize);

            var checkedObjBounds = checkedObject.Bounds;

            foreach (var obj in context.GetCollidableObjectsWithin(areaToCheck))
            {
                if (obj == checkedObject)
                    continue;

                if (obj.IsSolid)
                {
                    var objBounds = CoordinateSystem.Denormalize(obj.Bounds, checkedObjBounds);

                    if (checkedObjBounds.TryGetTouchedSide(objBounds, out var touchedSide))
                    {
                        checkedObject.TouchedObjects[touchedSide].Add(obj);
                    }
                }
            }
        }

        public void Move(ICollisionEngineContext context, TimeSpan elspsedTime)
        {
            var leftMovingObjects = new List<IMovableObject>();
            var rightMovingObjects = new List<IMovableObject>();
            var upMovingObjects = new List<IMovableObject>();
            var downMovingObjects = new List<IMovableObject>();

            foreach (var entity in context.GetMovableObjects())
            {
                if (entity.Velocity.X < 0)
                {
                    leftMovingObjects.Add(entity);
                }
                else if (entity.Velocity.X > 0)
                {
                    rightMovingObjects.Add(entity);
                }

                if (entity.Velocity.Y < 0)
                {
                    upMovingObjects.Add(entity);
                }
                else if (entity.Velocity.Y > 0)
                {
                    downMovingObjects.Add(entity);
                }
            }

            leftMovingObjects = leftMovingObjects.OrderBy(entity => entity.Position.X).ToList();
            rightMovingObjects = rightMovingObjects.OrderByDescending(entity => entity.Position.X).ToList();
            upMovingObjects = upMovingObjects.OrderBy(entity => entity.Position.Y).ToList();
            downMovingObjects = downMovingObjects.OrderByDescending(entity => entity.Position.Y).ToList();

            foreach (var entity in leftMovingObjects)
            {
                MoveLeft(context, entity, (int)(-entity.Velocity.X * elspsedTime.TotalSeconds));
            }

            foreach (var entity in rightMovingObjects)
            {
                MoveRight(context, entity, (int)(entity.Velocity.X * elspsedTime.TotalSeconds));
            }

            foreach (var entity in upMovingObjects)
            {
                MoveUp(context, entity, (int)(-entity.Velocity.Y * elspsedTime.TotalSeconds));
            }

            foreach (var entity in downMovingObjects)
            {
                MoveDown(context, entity, (int)(entity.Velocity.Y * elspsedTime.TotalSeconds));
            }
        }

        public void Move(ICollisionEngineContext context, IMovableObject movingObj, IntVector offset)
        {
            MoveHorizontal(context, movingObj, offset.X);
            MoveVertical(context, movingObj, offset.Y);
        }

        public void MoveHorizontal(ICollisionEngineContext context, IMovableObject movingObj, int distance)
        {
            if (distance > 0)
            {
                MoveRight(context, movingObj, distance);
            }
            else if (distance < 0)
            {
                MoveLeft(context, movingObj, -distance);
            }
        }

        public void MoveRight(ICollisionEngineContext context, IMovableObject movingObj, int distance)
        {
            var movingObjBounds = movingObj.Bounds;

            var sweptArea = new IntRectangle(movingObjBounds.Right, movingObjBounds.Top, movingObjBounds.Front, distance, movingObjBounds.Height, movingObjBounds.Depth);

            var areaToCheck = sweptArea.Extend(Constants.BlockSize);

            foreach (var obj in context.GetCollidableObjectsWithin(areaToCheck))
            {
                if (obj == movingObj)
                    continue;

                var objBounds = CoordinateSystem.Denormalize(obj.Bounds, sweptArea);

                if (obj.IsSolid && sweptArea.Intersects(objBounds))
                    sweptArea = new IntRectangle(sweptArea.Left, sweptArea.Top, sweptArea.Front, objBounds.Left - sweptArea.Left, sweptArea.Height, sweptArea.Depth);
            }

            movingObj.Position = movingObj.Position.MoveRight(sweptArea.Width);
        }

        public void MoveLeft(ICollisionEngineContext context, IMovableObject movingObj, int distance)
        {
            var movingObjBounds = movingObj.Bounds;

            var sweptArea = new IntRectangle(movingObjBounds.Left - distance, movingObjBounds.Top, movingObjBounds.Front, distance, movingObjBounds.Height, movingObjBounds.Depth);

            var areaToCheck = sweptArea.Extend(Constants.BlockSize);

            foreach (var obj in context.GetCollidableObjectsWithin(areaToCheck))
            {
                if (obj == movingObj)
                    continue;

                var objBounds = CoordinateSystem.Denormalize(obj.Bounds, sweptArea);

                if (obj.IsSolid && sweptArea.Intersects(objBounds))
                    sweptArea = new IntRectangle(objBounds.Right, sweptArea.Top, sweptArea.Front, sweptArea.Right - objBounds.Right, sweptArea.Height, sweptArea.Depth);
            }

            movingObj.Position = movingObj.Position.MoveLeft(sweptArea.Width);
        }

        public void MoveVertical(ICollisionEngineContext context, IMovableObject movingObj, int offset)
        {
            if (offset > 0)
            {
                MoveDown(context, movingObj, offset);
            }
            else if (offset < 0)
            {
                MoveUp(context, movingObj, -offset);
            }
        }

        public void MoveDown(ICollisionEngineContext context, IMovableObject movingObj, int distance)
        {
            var movingObjBounds = movingObj.Bounds;
            
            var sweptArea = new IntRectangle(movingObjBounds.Left, movingObjBounds.Bottom, movingObjBounds.Front, movingObjBounds.Width, distance, movingObjBounds.Depth);
            
            var areaToCheck = sweptArea.Extend(Constants.BlockSize);

            foreach (var obj in context.GetCollidableObjectsWithin(areaToCheck))
            {
                if (obj == movingObj)
                    continue;

                var objBounds = CoordinateSystem.Denormalize(obj.Bounds, sweptArea);

                if (obj.IsSolid && sweptArea.Intersects(objBounds))
                    sweptArea = new IntRectangle(sweptArea.Left, sweptArea.Top, sweptArea.Front, sweptArea.Width, objBounds.Top - sweptArea.Top, sweptArea.Depth);
            }

            movingObj.Position = movingObj.Position.MoveDown(sweptArea.Height);
        }

        public void MoveUp(ICollisionEngineContext context, IMovableObject movingObj, int distance)
        {
            var movingObjBounds = movingObj.Bounds;

            var sweptArea = new IntRectangle(movingObjBounds.Left, movingObjBounds.Top - distance, movingObjBounds.Front, movingObjBounds.Width, distance, movingObjBounds.Depth);

            var areaToCheck = sweptArea.Extend(Constants.BlockSize);

            foreach (var obj in context.GetCollidableObjectsWithin(areaToCheck))
            {
                if (obj == movingObj)
                    continue;

                var objBounds = CoordinateSystem.Denormalize(obj.Bounds, sweptArea);

                if (obj.IsSolid && sweptArea.Intersects(objBounds))
                    sweptArea = new IntRectangle(sweptArea.Left, objBounds.Bottom, sweptArea.Front, sweptArea.Width, sweptArea.Bottom - objBounds.Bottom, sweptArea.Depth);
            }
            
            movingObj.Position = movingObj.Position.MoveUp(sweptArea.Height);
        }
    }
}
