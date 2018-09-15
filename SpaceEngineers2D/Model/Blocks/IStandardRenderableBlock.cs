using System.Windows.Media;

namespace SpaceEngineers2D.Model.Blocks
{
    public interface IStandardRenderableBlock
    {
        ImageSource Image { get; }

        double IntegrityRatio { get; }
    }
}
