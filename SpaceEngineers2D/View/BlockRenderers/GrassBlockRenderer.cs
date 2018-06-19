using System;
using System.Collections.Generic;
using System.Windows.Media;
using SpaceEngineers2D.Geometry;
using SpaceEngineers2D.Model;
using SpaceEngineers2D.Model.Blocks;
using SpaceEngineers2D.Physics;

namespace SpaceEngineers2D.View.BlockRenderers
{
    public class GrassBlockRenderer : BlockRenderer<GrassBlock>
    {
        private const int StalkThickness = 80;

        private readonly GrassVersion[] _versions = {
            new GrassVersion(new List<GrassVersionStalk>
            {
                new GrassVersionStalk(100, 200, Colors.Green),
                new GrassVersionStalk(500, 800, Colors.ForestGreen),
                new GrassVersionStalk(900, 600, Colors.Green)
            }),
            new GrassVersion(new List<GrassVersionStalk>
            {
                new GrassVersionStalk(100, 200, Colors.Green),
                new GrassVersionStalk(300, 800, Colors.DarkGreen),
                new GrassVersionStalk(500, 600, Colors.DarkGreen),
                new GrassVersionStalk(700, 1000, Colors.DarkGreen),
                new GrassVersionStalk(900, 400, Colors.Green)
            }),
            new GrassVersion(new List<GrassVersionStalk>
            {
                new GrassVersionStalk(200, 200, Colors.Green),
                new GrassVersionStalk(300, 400, Colors.Green),
                new GrassVersionStalk(600, 200, Colors.ForestGreen),
                new GrassVersionStalk(800, 400, Colors.Green)
            })
        };

        protected override void Render(DrawingContext context, Camera camera, GrassBlock block, IntRectangle rectangle)
        {
            var version = GetVersion(Math.Abs(rectangle.Left));

            foreach (var stalk in version.Stalks)
            {
                RenderStalk(context, camera, block, stalk, rectangle);
            }
        }

        private void RenderStalk(DrawingContext context, Camera camera, GrassBlock block, GrassVersionStalk stalk, IntRectangle rectangle)
        {
            var stroke = new Pen(Brushes.Transparent, 0);

            context.DrawRectangle(stalk.Fill, stroke, camera.CastRectangle(IntRectangle.FromLeftTopRightAndBottom(
                rectangle.Left + stalk.XOffset - StalkThickness / 2,
                rectangle.Top + (rectangle.Size.Y - stalk.Height),
                rectangle.Left + stalk.XOffset + StalkThickness / 2,
                rectangle.Bottom)).ToWindowsRect());
        }

        private GrassVersion GetVersion(int x)
        {
            return _versions[GetVersionIndex(x)];
        }

        private int GetVersionIndex(int x)
        {
            x /= Constants.PhysicsUnit;

            if (x % 3 == 0)
            {
                return 0;
            }

            if (x % 2 == 0)
            {
                return 1;
            }

            return 2;
        }

        private class GrassVersion
        {
            public List<GrassVersionStalk> Stalks { get; }

            public GrassVersion(List<GrassVersionStalk> stalks)
            {
                Stalks = stalks;
            }
        }

        private class GrassVersionStalk
        {
            public int XOffset { get; }

            public int Height { get; }

            public Brush Fill { get; }

            public GrassVersionStalk(int xOffset, int height, Color fillColor)
            {
                XOffset = xOffset;
                Height = height;
                Fill = new SolidColorBrush(fillColor);
            }
        }
    }
}
