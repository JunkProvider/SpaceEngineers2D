using System.ComponentModel.Design;
using SpaceEngineers2D.Model.Entities;

namespace SpaceEngineers2D.Physics
{
    using System;
    using Geometry;
    using Model;

    public class PhysicsEngine
    {
        private readonly CollisionEngine _collisionEngine;

        public ICoordinateSystem CoordinateSystem { get; }

        public PhysicsEngine(ICoordinateSystem coordinateSystem)
        {
            CoordinateSystem = coordinateSystem;
            _collisionEngine = new CollisionEngine(coordinateSystem);
        }

        public void Initialize(World world)
        {
            foreach (var entity in world.Entities)
            {
                _collisionEngine.DetectTouchedBlocks(world.Grids, entity);
            }
        }

        public void Update(World world, TimeSpan elapsedTime)
        {
            foreach (var entity in world.Entities)
            {
                UpdateEntity(world, entity, elapsedTime);
            }

            foreach (var item in world.Items)
            {
                UpdateItem(world, item, elapsedTime);
            }
        }

        private void UpdateItem(World world, MobileItem item, TimeSpan elapsedTime)
        {
            ApplyDrag(item, elapsedTime);
            ApplyGravityIfNotBlocked(item, world.Gravity, elapsedTime);
            ResetVelocityInBlockedDirections(item);
            _collisionEngine.Move(world.Grids, item, item.Velocity * elapsedTime.TotalSeconds);
            _collisionEngine.DetectTouchedBlocks(world.Grids, item);
        }

        private void UpdateEntity(World world, IEntity entity, TimeSpan elapsedTime)
        {
            ApplyGravityIfNotBlocked(entity, world.Gravity, elapsedTime);

            ApplyDrag(entity, elapsedTime);

            _collisionEngine.Move(world.Grids, entity, entity.Velocity * elapsedTime.TotalSeconds);

            entity.Position = CoordinateSystem.Normalize(entity.Position);

            _collisionEngine.DetectTouchedBlocks(world.Grids, entity);

            ResetVelocityInBlockedDirections(entity);
        }

        private void ApplyGravityIfNotBlocked(IMobileObject obj, IntVector gravity, TimeSpan elapsedTime)
        {
            if (obj.TouchedBlocks[Side.Bottom].Count == 0)
            {
                obj.Velocity = obj.Velocity + gravity * elapsedTime.TotalSeconds;
            }
        }

        private void ApplyDrag(IMobileObject obj, TimeSpan elapsedTime)
        {
            var dragFactor = Math.Min(1, 0.05f * elapsedTime.TotalSeconds);
            obj.Velocity = obj.Velocity - obj.Velocity * dragFactor;
        }

        private void ResetVelocityInBlockedDirections(IMobileObject obj)
        {
            var velocity = obj.Velocity;

            if (obj.TouchedBlocks[Side.Left].Count != 0 && obj.Velocity.X < 0)
            {
                velocity.X = 0;
            }

            if (obj.TouchedBlocks[Side.Right].Count != 0 && obj.Velocity.X > 0)
            {
                velocity.X = 0;
            }

            if (obj.TouchedBlocks[Side.Top].Count != 0 && obj.Velocity.Y < 0)
            {
                velocity.Y = 0;
            }

            if (obj.TouchedBlocks[Side.Bottom].Count != 0 && obj.Velocity.Y > 0)
            {
                velocity.Y = 0;
            }

            obj.Velocity = velocity;
        }
    }
}
