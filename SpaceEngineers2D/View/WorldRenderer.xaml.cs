using System;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using SpaceEngineers2D.Geometry;
using SpaceEngineers2D.Model;
using SpaceEngineers2D.Model.Blocks;
using SpaceEngineers2D.Physics;
using SpaceEngineers2D.View.BlockRenderers;

namespace SpaceEngineers2D.View
{
    public partial class WorldRenderer
    {
        private readonly BlockRendererRegistry _blockRendererRegistry = new BlockRendererRegistry();

        private ApplicationViewModel ApplicationViewModel => DataContext as ApplicationViewModel;

        private DateTime LastUpdateTime { get; set; }

        public WorldRenderer()
        {
            InitializeComponent();

            _blockRendererRegistry.Add<IStandardRenderableBlock, IStandardRenderableBlock>(new StandardBlockRenderer());
            _blockRendererRegistry.Add<StructuralBlock, StructuralBlock>(new StructuralBlockRenderer());

            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(30);
            LastUpdateTime = DateTime.Now;
            timer.Start();
            timer.Tick += (sender, args) =>
                {
                    if (ApplicationViewModel?.WorldController != null)
                    {
                        var time = DateTime.Now;
                        var elapsedTime = time - LastUpdateTime;
                        LastUpdateTime = time;

                        ApplicationViewModel.Physics.Update(ApplicationViewModel.World, elapsedTime);

                        var mousePosition = ApplicationViewModel.World.Camera.UncastPosition(IntVector.FromWindowsPoint(Mouse.GetPosition(this)));
                        ApplicationViewModel.WorldController.OnMouseMove(mousePosition);
                        ApplicationViewModel.WorldController.OnUpdate(elapsedTime);
                        
                        InvalidateVisual();
                    }
                };
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            if (ApplicationViewModel?.WorldController == null)
                return;

            var camera = ApplicationViewModel.World.Camera;
            var viewport = GetViewport();

            camera.Viewport = viewport;
            camera.Position = ApplicationViewModel.Player.Position + IntVector.Up * Constants.BlockSize;

            var visibleArea = camera.UncastRectangle(IntRectangle.FromPositionAndSize(IntVector.Zero, viewport))
                .Extend(new IntVector(2 * Constants.BlockSize, Constants.BlockSize, 0));

            RenderBackgroundBlocks(dc, visibleArea, camera);
            RenderEntities(dc, visibleArea, camera);
            RenderItems(dc);
            RenderForegroundBlocks(dc, visibleArea, camera);
            RenderHoveredBlockEffect(dc);
        }

        private void RenderForegroundBlocks(DrawingContext dc, IntRectangle visibleArea, Camera camera)
        {
            var visibleAreaPosition = visibleArea.Position;
            visibleAreaPosition.Z = 0;
            var visibleAreaSize = visibleArea.Size;
            visibleAreaSize.Z = Constants.BlockSize;
            RenderGrids(dc, IntRectangle.FromPositionAndSize(visibleAreaPosition, visibleAreaSize), camera);
        }

        private void RenderBackgroundBlocks(DrawingContext dc, IntRectangle visibleArea, Camera camera)
        {
            var visibleAreaPosition = visibleArea.Position;
            visibleAreaPosition.Z = Constants.BlockSize;
            var visibleAreaSize = visibleArea.Size;
            visibleAreaSize.Z = Constants.BlockSize;
            RenderGrids(dc, IntRectangle.FromPositionAndSize(visibleAreaPosition, visibleAreaSize), camera);
        }

        private void RenderEntities(DrawingContext dc, IntRectangle visibleArea, Camera camera)
        {
            foreach (var entity in ApplicationViewModel.World.Entities)
            {
                var bounds = ApplicationViewModel.World.CoordinateSystem.Denormalize(entity.Bounds, visibleArea);
                dc.DrawImage(entity.Image, ApplicationViewModel.World.Camera.CastRectangle(bounds).ToWindowsRect());
            }
        }

        private void RenderHoveredBlockEffect(DrawingContext dc)
        {
            var player = ApplicationViewModel.Player;

            var brush = new SolidColorBrush();
            brush.Color = Colors.Transparent;

            var strokeColor = player.TargetBlockCoordsInRange ? Colors.SteelBlue : Colors.AliceBlue;
            strokeColor.A = 128;
            Pen stroke = new Pen(new SolidColorBrush(strokeColor), 2);

            if (player.TargetBlockCoords != null)
            {
                dc.DrawRectangle(brush, stroke, ApplicationViewModel.World.Camera.CastRectangle(player.TargetBlockCoords.Bounds).ToWindowsRect());
            }
        }

        private void RenderItems(DrawingContext dc)
        {
            SolidColorBrush brush = new SolidColorBrush();
            brush.Color = Colors.Yellow;
            Pen pen = new Pen(Brushes.Black, 2);

            foreach (var item in ApplicationViewModel.World.Items)
            {
                dc.DrawRectangle(brush, pen, ApplicationViewModel.World.Camera.CastRectangle(item.Bounds).ToWindowsRect());
            }
        }

        private void RenderGrids(DrawingContext dc, IntRectangle visibleArea, Camera camera)
        {
            foreach (var grid in ApplicationViewModel.World.Grids)
            {
                foreach (var block in grid.GetAllWithin(visibleArea))
                {
                    var renderer = _blockRendererRegistry.Get(block.GetType());
                    var blockRect = ApplicationViewModel.World.CoordinateSystem.Denormalize(block.Bounds, visibleArea);
                    renderer.Render(dc, camera, block, blockRect);
                }
            }
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (ApplicationViewModel?.WorldController == null)
                return;

            var mousePosition = ApplicationViewModel.World.Camera.UncastPosition(IntVector.FromWindowsPoint(Mouse.GetPosition(this)));

            if (e.ChangedButton == MouseButton.Left)
            {
                ApplicationViewModel.WorldController.OnLeftMouseButtonDown(mousePosition);
            }

            if (e.ChangedButton == MouseButton.Right)
            {
                ApplicationViewModel.WorldController.OnRightMouseButtonDown(mousePosition);
            }
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (ApplicationViewModel?.WorldController == null)
                return;

            var mousePosition = ApplicationViewModel.World.Camera.UncastPosition(IntVector.FromWindowsPoint(Mouse.GetPosition(this)));

            if (e.ChangedButton == MouseButton.Left)
            {
                ApplicationViewModel.WorldController.OnLeftMouseButtonUp(mousePosition);
            }

            if (e.ChangedButton == MouseButton.Right)
            {
                ApplicationViewModel.WorldController.OnRightMouseButtonUp(mousePosition);
            }
        }

        private IntVector GetViewport()
        {
            return new IntVector((int)ActualWidth, (int)ActualHeight, 0);
        }
    }
}
