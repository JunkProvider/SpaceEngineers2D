using System;
using System.Windows.Media;
using SpaceEngineers2D.Geometry;
using SpaceEngineers2D.Model;
using SpaceEngineers2D.Model.Blocks;

namespace SpaceEngineers2D.View.BlockRenderers
{
    public class StandardBlockRenderer : BlockRenderer<StandardBlock>
    {
        protected override void Render(DrawingContext context, Camera camera, StandardBlock block, IntRectangle rectangle)
        {
            context.PushOpacity(0.75f + 0.25f * block.IntegrityRatio);
            context.DrawImage(block.Image, camera.CastRectangle(rectangle).ToWindowsRect());
            context.Pop();
        }
    }
}
