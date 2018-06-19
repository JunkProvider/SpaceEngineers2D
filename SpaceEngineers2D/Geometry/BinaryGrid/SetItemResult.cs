namespace SpaceEngineers2D.Geometry.BinaryGrid
{
    public class SetItemResult<T>
    {
        public T RemovedItem { get; set; }

        public IntVector OffsetShift { get; set; }
    }
}
