using System;
using System.Windows.Media;

namespace SpaceEngineers2D.Model.Entities
{
    public interface IEntity : IMobileObject
    {
        IEntityType EntityType { get; }

        ImageSource Image { get; }

        void Update(World world, TimeSpan elapsedTime);
    }
}