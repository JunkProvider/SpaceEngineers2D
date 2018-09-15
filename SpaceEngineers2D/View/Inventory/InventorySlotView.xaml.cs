using System.Windows.Input;

namespace SpaceEngineers2D.View.Inventory
{
    public partial class InventorySlotView
    {
        public InventorySlotView()
        {
            InitializeComponent();
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (DataContext == null)
            {
                return;
            }

            var viewModel = (InventorySlotViewModel) DataContext;

            viewModel.TryTransfereWithInteractionSlot();
        }
    }
}
