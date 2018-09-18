namespace SpaceEngineers2D.Physics
{
    using System;
    using System.Windows;

    using SpaceEngineers2D.Geometry;
    using SpaceEngineers2D.Model;

    public class PhysicsEngine
    {
        private CollisionEngine _collisionEngine = new CollisionEngine();

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
        }

        private static void ApplyPlayerMovementOrders(Player player)
        {
            var playerMoveSpeed = 4;

            if (player.TouchedBlocks[Side.Bottom].Count != 0)
            {
                if (player.MovementOrders[Side.Left])
                {
                    player.Velocity = new IntVector(-playerMoveSpeed * Constants.PhysicsUnit, player.Velocity.Y);
                }
                else if (player.MovementOrders[Side.Right])
                {
                    player.Velocity = new IntVector(playerMoveSpeed * Constants.PhysicsUnit, player.Velocity.Y);
                }
                else
                {
                    player.Velocity = new IntVector(0, player.Velocity.Y);
                }

                if (player.MovementOrders[Side.Top])
                {
                    player.Velocity = player.Velocity + IntVector.Up * Constants.PhysicsUnit * 7;
                }
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
            if (obj.TouchedBlocks[Side.Left].Count != 0 && obj.Velocity.X < 0)
            {
                obj.Velocity = new IntVector(0, obj.Velocity.Y);
            }

            if (obj.TouchedBlocks[Side.Right].Count != 0 && obj.Velocity.X > 0)
            {
                obj.Velocity = new IntVector(0, obj.Velocity.Y);
            }

            if (obj.TouchedBlocks[Side.Top].Count != 0 && obj.Velocity.Y < 0)
            {
                obj.Velocity = new IntVector(obj.Velocity.X, 0);
            }

            if (obj.TouchedBlocks[Side.Bottom].Count != 0 && obj.Velocity.Y > 0)
            {
                obj.Velocity = new IntVector(obj.Velocity.X, 0);
            }
        }
    }
}
