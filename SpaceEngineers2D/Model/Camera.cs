namespace SpaceEngineers2D.Model
{
    using SpaceEngineers2D.Geometry;

    public class Camera
    {
        public IntVector Position { get; set; }

        public float Zoom { get; set; } = 1;

        public IntVector Viewport { get; set; } = IntVector.RightDown;

        public IntRectangle CastRectangle(IntRectangle rectangle)
        {
            return IntRectangle.FromPositionAndSize(CastPosition(rectangle.Position), CastSize(rectangle.Size));
        }

        public IntVector CastPosition(IntVector position)
        {
            var casted = position * Zoom;
            casted -= Position * Zoom;
            casted += Viewport / 2;
            return casted;
        }

        public IntVector CastSize(IntVector size)
        {
            return size * Zoom;
        }

        public IntRectangle UncastRectangle(IntRectangle rectangle)
        {
            return IntRectangle.FromPositionAndSize(UncastPosition(rectangle.Position), UncastSize(rectangle.Size));
        }

        public IntVector UncastPosition(IntVector position)
        {
            var uncasted = position;
            uncasted -= Viewport / 2;
            uncasted += Position * Zoom;
            uncasted /= Zoom;
            return uncasted;
            // return (position + Position * Zoom - Viewport / 2) / Zoom;
        }

        public IntVector UncastSize(IntVector size)
        {
            return size / Zoom;
        }
    }
}
