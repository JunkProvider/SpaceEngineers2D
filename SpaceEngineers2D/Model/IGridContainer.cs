namespace SpaceEngineers2D.Model
{
    using System.Collections.Generic;

    public interface IGridContainer
    {
        ICollection<Grid> Grids { get; }
    }
}
