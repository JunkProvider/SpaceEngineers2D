using System;
using System.Windows.Media;
using SpaceEngineers2D.Geometry;
using SpaceEngineers2D.Model;
using SpaceEngineers2D.Model.Blocks;

namespace SpaceEngineers2D.View.BlockRenderers
{
    public class StructuralBlockRenderer : BlockRenderer<StructuralBlock>
    {
        private const float BarThickness = 0.2f;
        private const float BarMinHeight = 0.05f;
        private readonly Brush _integrityCapBrush = new SolidColorBrush(Color.FromArgb(128, 64, 128, 255));
        private readonly Brush _integrityBrush = new SolidColorBrush(Color.FromArgb(255, 64, 128, 255));
        private readonly Pen _blueprintFramePen = new Pen(new SolidColorBrush(Color.FromArgb(255, 64, 128, 255)), 2);
        private readonly Pen _integrityPen = new Pen(Brushes.Transparent, 0);

        protected override void Render(DrawingContext context, Camera camera, StructuralBlock block, IntRectangle rectangle)
        {
            if (block.Finished)
            {
                RenderFinished(context, camera, block, rectangle);
            }
            else
            {
                RenderUnfinished(context, camera, block, rectangle);
            }
        }

        private void RenderFinished(DrawingContext context, Camera camera, StructuralBlock block, IntRectangle rectangle)
        {
            /*var fill = new SolidColorBrush(block.Color);
            var stroke = new Pen(Brushes.Transparent, 0);*/

            context.DrawImage(block.Image, camera.CastRectangle(rectangle).ToWindowsRect());            
        }

        private void RenderUnfinished(DrawingContext context, Camera camera, StructuralBlock block, IntRectangle rectangle)
        {
            /* var fillColor = block.Color;
            fillColor.A = (byte)(fillColor.A * block.BlueprintState.IntegrityRatio);
            var fill = new SolidColorBrush(fillColor);
            var stroke = new Pen(new SolidColorBrush(block.Color), 1);*/
            context.PushOpacity(block.BlueprintState.IntegrityRatio);
            context.DrawImage(block.Image, camera.CastRectangle(rectangle).ToWindowsRect());
            context.Pop();
            // context.DrawRectangle(Brushes.Transparent, _blueprintFramePen, camera.CastRectangle(rectangle).ToWindowsRect());

            var integrityCap = block.BlueprintState.IntegrityCapRatio;
            var integrityCapRect = new IntRectangle(
                rectangle.Left,
                (int)(rectangle.Top + (1 - integrityCap) * rectangle.Size.Y),
                (int)(rectangle.Size.X * BarThickness),
                (int)(integrityCap * rectangle.Size.Y));

            context.DrawRectangle(_integrityCapBrush, _integrityPen, camera.CastRectangle(integrityCapRect).ToWindowsRect());

            var integrity = block.BlueprintState.IntegrityRatio;
            var integrityRectHeight = (int)Math.Max(integrity * rectangle.Size.Y, BarMinHeight * rectangle.Size.Y);
            var integrityRect = new IntRectangle(
                rectangle.Left,
                rectangle.Top + rectangle.Size.Y - integrityRectHeight,
                (int)(rectangle.Size.X * BarThickness),
                integrityRectHeight);

            context.DrawRectangle(_integrityBrush, _integrityPen, camera.CastRectangle(integrityRect).ToWindowsRect());
        }
    }
}
