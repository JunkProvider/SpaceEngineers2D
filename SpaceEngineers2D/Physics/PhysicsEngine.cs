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

        public void Update(World world, TimeSpan elapsedTime)
        {
            var player = world.Player;

            ApplyDrag(player);
            ApplyGravityIfNotBlocked(player, world.Gravity, elapsedTime);
            ResetVelocityInBlockedDirections(player);
            ApplyPlayerMovementOrders(player);
            _collisionEngine.Move(world.Grids, player, player.Velocity * elapsedTime.TotalSeconds);
            _collisionEngine.DetectTouchedBlocks(world.Grids, player);

            foreach (var item in world.Items)
            {
                ApplyDrag(item);
                ApplyGravityIfNotBlocked(item, world.Gravity, elapsedTime);
                ResetVelocityInBlockedDirections(item);
                _collisionEngine.Move(world.Grids, item, item.Velocity * elapsedTime.TotalSeconds);
                _collisionEngine.DetectTouchedBlocks(world.Grids, item);
            }

            player.Position = CoordinateSystem.Normalize(player.Position);
        }

        private static void ApplyPlayerMovementOrders(Player player)
        {
            var playerMoveSpeed = 4;

            if (player.TouchedBlocks[Side.Bottom].Count != 0)
            {
                var velocity = player.Velocity;

                if (player.MovementOrders[Side.Left])
                {
                    velocity.X = -playerMoveSpeed * Constants.BlockSize;
                }
                else if (player.MovementOrders[Side.Right])
                {
                    velocity.X = playerMoveSpeed * Constants.BlockSize;
                }
                else
                {
                    velocity.X = 0;
                }

                if (player.MovementOrders[Side.Top])
                {
                    velocity = velocity + IntVector.Up * Constants.BlockSize * 7;
                }

                player.Velocity = velocity;
            }
        }

        private void ApplyGravityIfNotBlocked(IMobileObject obj, IntVector gravity, TimeSpan elapsedTime)
        {
            if (obj.TouchedBlocks[Side.Bottom].Count == 0)
            {
                obj.Velocity = obj.Velocity + gravity * elapsedTime.TotalSeconds;
            }
        }

        private void ApplyDrag(IMobileObject obj)
        {
            obj.Velocity = obj.Velocity * 0.99f;
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
