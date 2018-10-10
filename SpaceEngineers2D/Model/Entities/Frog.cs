using System;
using System.Collections.Generic;
using SpaceEngineers2D.Geometry;
using SpaceEngineers2D.Model.Blocks;
using SpaceEngineers2D.Physics;

namespace SpaceEngineers2D.Model.Entities
{
    public class Frog : Entity
    {
        public override IntVector Size { get; } = new IntVector(400, 400, 400);

        public Frog(FrogType entityType)
            : base(entityType)
        {
            TouchedBlocks[Side.Left] = new List<Block>();
            TouchedBlocks[Side.Right] = new List<Block>();
            TouchedBlocks[Side.Top] = new List<Block>();
            TouchedBlocks[Side.Bottom] = new List<Block>();
            TouchedBlocks[Side.Front] = new List<Block>();
            TouchedBlocks[Side.Back] = new List<Block>();
        }

        public override void Update(World world, TimeSpan elapsedTime)
        {
            base.Update(world, elapsedTime);

            ApplyMovementOrders();
        }

        private void ApplyMovementOrders()
        {
            if (TouchedBlocks[Side.Bottom].Count != 0)
            {
                var velocity = Velocity;

                velocity = velocity + IntVector.Up * Constants.BlockSize * 7;

                Velocity = velocity;
            }
        }
    }
}
