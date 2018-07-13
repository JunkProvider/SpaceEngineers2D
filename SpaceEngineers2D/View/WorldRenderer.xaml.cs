using System;
using System.Windows;
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
    /// <summary>
    /// Interaction logic for Canvas.xaml
    /// </summary>
    public partial class WorldRenderer
    {

        public static readonly DependencyProperty ParametersProperty = DependencyProperty.Register(
            "Parameters", typeof(WorldRendererParameters),
            typeof(WorldRenderer)
        );

        public WorldRendererParameters Parameters
        {
            get => (WorldRendererParameters)GetValue(ParametersProperty);
            set => SetValue(ParametersProperty, value);
        }

        private readonly PhysicsEngine _physicsEngine = new PhysicsEngine();

        private readonly BlockRendererRegistry _blockRendererRegistry = new BlockRendererRegistry();

        public WorldRenderer()
        {
            InitializeComponent();

            _blockRendererRegistry.Add<StandardBlock, StandardBlock>(new StandardBlockRenderer());
            _blockRendererRegistry.Add<StructuralBlock, StructuralBlock>(new StructuralBlockRenderer());
            _blockRendererRegistry.Add<GrassBlock, GrassBlock>(new GrassBlockRenderer());

            var lastTime = DateTime.Now;

            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(30);
            timer.Start();
            timer.Tick += (sender, args) =>
                {
                    if (Parameters == null)
                    {
                        return;
                    }

                    var currentTime = DateTime.Now;
                    var elapsedTime = currentTime - lastTime;
                    lastTime = currentTime;

                    var mousePosition = Parameters.World.Camera.UncastPosition(IntVector.FromWindowsPoint(Mouse.GetPosition(this)));
                    Parameters.Controller.OnMouseMove(mousePosition);
                    Parameters.Controller.OnUpdate(timer.Interval);
                    _physicsEngine.Update(Parameters.World, elapsedTime);
                    InvalidateVisual();
                };
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            if (Parameters == null)
                return;

            var camera = Parameters.World.Camera;
            var viewport = GetViewport();

            camera.Viewport = viewport;
            camera.Position = Parameters.World.Player.Position + IntVector.Up * Constants.PhysicsUnit;

            var visibleArea = camera.UncastRectangle(IntRectangle.FromPositionAndSize(IntVector.Zero, viewport))
                .Extend(2 * Constants.PhysicsUnit);

            RenderPlayer(dc);
            RenderItems(dc);
            RenderGrids(dc, visibleArea, camera);
            RenderHoveredBlockEffect(dc);
        }

        private void RenderPlayer(DrawingContext dc)
        {
            SolidColorBrush brush = new SolidColorBrush();
            brush.Color = Colors.OrangeRed;
            Pen pen = new Pen(Brushes.DarkRed, 0);

            dc.DrawRectangle(brush, pen, Parameters.World.Camera.CastRectangle(Parameters.World.Player.Bounds).ToWindowsRect());
        }

        private void RenderHoveredBlockEffect(DrawingContext dc)
        {
            var player = Parameters.World.Player;

            SolidColorBrush brush = new SolidColorBrush();
            brush.Color = Colors.Transparent;

            var strokeColor = player.TargetBlockCoordsInRange ? Colors.SteelBlue : Colors.AliceBlue;
            strokeColor.A = 128;
            Pen stroke = new Pen(new SolidColorBrush(strokeColor), 2);

            if (player.TargetBlockCoords != null)
            {
                dc.DrawRectangle(brush, stroke, Parameters.World.Camera.CastRectangle(player.TargetBlockCoords.Bounds).ToWindowsRect());
            }
        }

        private void RenderItems(DrawingContext dc)
        {
            SolidColorBrush brush = new SolidColorBrush();
            brush.Color = Colors.Yellow;
            Pen pen = new Pen(Brushes.Black, 2);

            foreach (var item in Parameters.World.Items)
            {
                dc.DrawRectangle(brush, pen, Parameters.World.Camera.CastRectangle(item.Bounds).ToWindowsRect());
            }
        }

        private void RenderGrids(DrawingContext dc, IntRectangle visibleArea, Camera camera)
        {
            foreach (var grid in Parameters.World.Grids)
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
            var mousePosition = Parameters.World.Camera.UncastPosition(IntVector.FromWindowsPoint(Mouse.GetPosition(this)));

            if (e.ChangedButton == MouseButton.Left)
            {
                Parameters.Controller.OnLeftMouseButtonDown(mousePosition);
            }

            if (e.ChangedButton == MouseButton.Right)
            {
                Parameters.Controller.OnRightMouseButtonDown(mousePosition);
            }
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            var mousePosition = Parameters.World.Camera.UncastPosition(IntVector.FromWindowsPoint(Mouse.GetPosition(this)));

            if (e.ChangedButton == MouseButton.Left)
            {
                Parameters.Controller.OnLeftMouseButtonUp(mousePosition);
            }

            if (e.ChangedButton == MouseButton.Right)
            {
                Parameters.Controller.OnRightMouseButtonUp(mousePosition);
            }
        }

        private IntVector GetViewport()
        {
            return new IntVector((int)ActualWidth, (int)ActualHeight);
        }
    }
}
