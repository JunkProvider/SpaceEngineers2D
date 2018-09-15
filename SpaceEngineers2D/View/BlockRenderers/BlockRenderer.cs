using System.Windows.Media;
using SpaceEngineers2D.Geometry;
using SpaceEngineers2D.Model;

namespace SpaceEngineers2D.View.BlockRenderers
{
    public interface IBlockRenderer
    {
        void Render(DrawingContext context, Camera camera, object block, IntRectangle rectangle);
    }

    public abstract class BlockRenderer<TBlock> : IBlockRenderer
    {
        public void Render(DrawingContext context, Camera camera, object block, IntRectangle rectangle)
        {
            Render(context, camera, (TBlock)block, rectangle);
        }

        protected abstract void Render(DrawingContext context, Camera camera, TBlock block, IntRectangle rectangle);
    }
}
