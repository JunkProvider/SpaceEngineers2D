using System.Windows.Media;

namespace SpaceEngineers2D.Model.Items
{
    public interface IItem
    {
        string Name { get; }

        ImageSource Icon { get; }

        double Amount { get; }

        double Mass { get; }

        bool ShouldBeAutoCombined(IItem other);

        bool CanBeCombinded(IItem other);

        IItem Combine(IItem other);
    }
}
