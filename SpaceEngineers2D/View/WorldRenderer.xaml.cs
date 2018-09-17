using System;
using System.IO;
using System.Reflection;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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
        private readonly PhysicsEngine _physicsEngine = new PhysicsEngine();

        private readonly BlockRendererRegistry _blockRendererRegistry = new BlockRendererRegistry();

        private readonly ImageSource _playerImage;

        private ApplicationViewModel ApplicationViewModel => DataContext as ApplicationViewModel;

        public WorldRenderer()
        {
            InitializeComponent();

            _playerImage = LoadImage("Player");

            _blockRendererRegistry.Add<IStandardRenderableBlock, IStandardRenderableBlock>(new StandardBlockRenderer());
            _blockRendererRegistry.Add<StructuralBlock, StructuralBlock>(new StructuralBlockRenderer());

            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(30);
            timer.Start();
            timer.Tick += (sender, args) =>
                {
                    if (ApplicationViewModel?.WorldController != null)
                    {
                        var mousePosition = ApplicationViewModel.World.Camera.UncastPosition(IntVector.FromWindowsPoint(Mouse.GetPosition(this)));
                        ApplicationViewModel.WorldController.OnMouseMove(mousePosition);
                        ApplicationViewModel.WorldController.OnUpdate(timer.Interval);
                        _physicsEngine.Update(ApplicationViewModel.World, timer.Interval);
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
            camera.Position = ApplicationViewModel.World.Player.Position + IntVector.Up * Constants.PhysicsUnit;

            var visibleArea = camera.UncastRectangle(IntRectangle.FromPositionAndSize(IntVector.Zero, viewport))
                .Extend(2 * Constants.PhysicsUnit);

            RenderPlayer(dc);
            RenderItems(dc);
            RenderGrids(dc, visibleArea, camera);
            RenderHoveredBlockEffect(dc);
        }

        private void RenderPlayer(DrawingContext dc)
        {
            dc.DrawImage(_playerImage, ApplicationViewModel.World.Camera.CastRectangle(ApplicationViewModel.World.Player.Bounds).ToWindowsRect());
        }

        private void RenderHoveredBlockEffect(DrawingContext dc)
        {
            var player = ApplicationViewModel.World.Player;

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
                grid.ForEachWithin(
                    IntRectangle.FromPositionAndSize(visibleArea.Position, visibleArea.Size),
                    (block, position) =>
                    {
                        var renderer = _blockRendererRegistry.Get(block.GetType());

                        var blockRect = IntRectangle.FromPositionAndSize(
                            position,
                            IntVector.RightBottom * Constants.PhysicsUnit);

                        renderer.Render(dc, camera, block, blockRect);
                    });
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
            return new IntVector((int)ActualWidth, (int)ActualHeight);
        }

        private static ImageSource LoadImage(string file)
        {
            var x = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return new BitmapImage(new Uri(x + "\\Assets\\Images\\" + file + ".png"));
        }
    }
}
