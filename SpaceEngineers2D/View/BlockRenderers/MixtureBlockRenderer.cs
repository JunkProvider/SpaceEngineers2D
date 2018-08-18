using System;
using System.Windows.Media;
using SpaceEngineers2D.Geometry;
using SpaceEngineers2D.Model;
using SpaceEngineers2D.Model.Blocks;

namespace SpaceEngineers2D.View.BlockRenderers
{
    public class MixtureBlockRenderer : BlockRenderer<MixtureBlock>
    {
        protected override void Render(DrawingContext context, Camera camera, MixtureBlock block, IntRectangle rectangle)
        {
            var brush = Brushes.BlueViolet;
            var pen = new Pen(brush, 2);

            context.PushOpacity(0.75f + 0.25f * block.IntegrityRatio);
            context.DrawRectangle(brush, pen, camera.CastRectangle(rectangle).ToWindowsRect());
            context.Pop();
        }
    }
}
