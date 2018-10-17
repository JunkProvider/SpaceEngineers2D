using System.Linq;
using System.Windows.Media.Animation;

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

        public void Initialize(IPhysicsEngineContext world)
        {
            foreach (var entity in world.GetMovableObjects())
            {
                _collisionEngine.DetectTouchedBlocks(world, entity);
            }
        }

        public void Update(IPhysicsEngineContext world, TimeSpan elapsedTime)
        {
            _collisionEngine.Move(world, elapsedTime);

            foreach (var movableObject in world.GetMovableObjects())
            {
                movableObject.Position = CoordinateSystem.Normalize(movableObject.Position);
            }

            foreach (var movableObject in world.GetMovableObjects())
            {
                _collisionEngine.DetectTouchedBlocks(world, movableObject);
            }

            foreach (var movableObject in world.GetMovableObjects())
            {
                ResetVelocityInBlockedDirections(movableObject);
            }

            foreach (var movableObject in world.GetMovableObjects())
            {
                ApplyGravityIfNotBlocked(movableObject, world.Gravity, elapsedTime);
                ApplyDrag(movableObject, elapsedTime);
            }
        }

        private void ApplyGravityIfNotBlocked(IMovableObject obj, IntVector gravity, TimeSpan elapsedTime)
        {
            if (!obj.TouchedObjects[Side.Bottom].Any() || obj.TouchedObjects[Side.Bottom].All(o => o is IMovableObject))
            {
                obj.Velocity = obj.Velocity + gravity * elapsedTime.TotalSeconds;
            }
        }

        private void ApplyDrag(IMovableObject obj, TimeSpan elapsedTime)
        {
            var dragFactor = Math.Min(1, 0.05f * elapsedTime.TotalSeconds);
            obj.Velocity = obj.Velocity - obj.Velocity * dragFactor;
        }

        private void ResetVelocityInBlockedDirections(IMovableObject obj)
        {
            var velocity = obj.Velocity;

            if (obj.Velocity.X < 0)
            {
                foreach (var touchedObj in obj.TouchedObjects[Side.Left])
                {
                    if (touchedObj is IMovableObject movableTouchedObj)
                    {
                        if (movableTouchedObj.Velocity.X > velocity.X)
                        {
                            velocity.X = (velocity.X + movableTouchedObj.Velocity.X) / 2;
                            movableTouchedObj.Velocity = movableTouchedObj.Velocity.WithX(velocity.X);
                        }
                    }
                    else
                    {
                        velocity.X = 0;
                    }
                }
            }

            if (obj.Velocity.X > 0)
            {
                foreach (var touchedObj in obj.TouchedObjects[Side.Right])
                {
                    if (touchedObj is IMovableObject movableTouchedObj)
                    {
                        if (movableTouchedObj.Velocity.X < velocity.X)
                        {
                            velocity.X = (velocity.X + movableTouchedObj.Velocity.X) / 2;
                            movableTouchedObj.Velocity = movableTouchedObj.Velocity.WithX(velocity.X);
                        }
                    }
                    else
                    {
                        velocity.X = 0;
                    }
                }
            }

            if (obj.Velocity.Y < 0)
            {
                foreach (var touchedObj in obj.TouchedObjects[Side.Top])
                {
                    if (touchedObj is IMovableObject movableTouchedObj)
                    {
                        if (movableTouchedObj.Velocity.Y > velocity.Y)
                        {
                            velocity.Y = (velocity.Y + movableTouchedObj.Velocity.Y) / 2;
                            movableTouchedObj.Velocity = movableTouchedObj.Velocity.WithY(velocity.Y);
                        }
                    }
                    else
                    {
                        velocity.Y = 0;
                    }
                }
            }

            if (obj.Velocity.Y > 0)
            {
                foreach (var touchedObj in obj.TouchedObjects[Side.Bottom])
                {
                    if (touchedObj is IMovableObject movableTouchedObj)
                    {
                        if (movableTouchedObj.Velocity.Y < velocity.Y)
                        {
                            velocity.Y = (velocity.Y + movableTouchedObj.Velocity.Y) / 2;
                            movableTouchedObj.Velocity = movableTouchedObj.Velocity.WithY(velocity.Y);
                        }
                    }
                    else
                    {
                        velocity.Y = 0;
                    }
                }
            }

            obj.Velocity = velocity;
        }
    }

    public class Collision
    {
        public IMovableObject ObjectA { get; }

        public IMovableObject ObjectB { get; }

        public Collision(IMovableObject objectA, IMovableObject objectB)
        {
            ObjectA = objectA;
            ObjectB = objectB;
        }
    }
}
