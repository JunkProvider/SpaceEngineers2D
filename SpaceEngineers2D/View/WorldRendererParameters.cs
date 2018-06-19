using SpaceEngineers2D.Controllers;
using SpaceEngineers2D.Model;

namespace SpaceEngineers2D.View
{
    public class WorldRendererParameters
    {
        public WorldRendererParameters(World world, WorldRendererController controller)
        {
            World = world;
            Controller = controller;
        }

        public World World { get; }

        public WorldRendererController Controller { get; }
    }
}
