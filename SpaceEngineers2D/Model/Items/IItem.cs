using System.Windows.Media;
using SpaceEngineers2D.Chemistry.Quantities;

namespace SpaceEngineers2D.Model.Items
{
    public interface IItem
    {
        string Name { get; }

        string Tooltip { get; }

        ImageSource Icon { get; }

        Mass Mass { get; }

        Volume Volume { get; }

        bool ShouldBeAutoCombined(IItem other);

        bool CanBeCombinded(IItem other);

        IItem Combine(IItem other);
    }
}
