using System;
using System.Linq;
using SpaceEngineers2D.Geometry;
using SpaceEngineers2D.Physics;

namespace SpaceEngineers2D.Model.Entities
{
    public class Frog : Entity
    {
        public override IntVector Size { get; } = new IntVector(400, 400, 400);

        public Frog(FrogType entityType)
            : base(entityType)
        {
        }

        public override void Update(World world, TimeSpan elapsedTime)
        {
            base.Update(world, elapsedTime);

            ApplyMovementOrders();
        }

        private void ApplyMovementOrders()
        {
            if (TouchedObjects[Side.Bottom].Any())
            {
                var velocity = Velocity;

                velocity = velocity + IntVector.Up * Constants.BlockSize * 7;

                Velocity = velocity;
            }
        }
    }
}
