using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TD.Models;
using TD.WPF.ViewModels.Trades;

namespace TD.WPF.Views.Trades
{
    // Codebehind for TradeNoteEditView.xaml
    public partial class TradeNoteEditView : AbstractEditView
    {
        private readonly TradeNoteEditVM _viewModel;

        public TradeNote TradeNote => _viewModel.TradeNote;

        /// <summary>
        /// This constructor should not be called, but we need it for the designer to work.
        /// </summary>
        /// <remarks>This constructor creates a dummy TradeNote for the designer.</remarks>
        public TradeNoteEditView()
        {
            InitializeComponent();

            _viewModel = new TradeNoteEditVM(new TradeNote());
            _isDirty = _viewModel;
            DataContext = _viewModel;
            SourceInitialized += TradeNoteEditView_SourceInitialized;
        }

        /// <summary>
        /// This constructor should be used at runtime to create the view with a real TradeNote.
        /// </summary>
        /// <param name="tradeNote">The real TradeNote to be used by the view.</param>
        public TradeNoteEditView(TradeNote tradeNote) : base(new TradeNoteEditVM(tradeNote))
        {
            InitializeComponent();
            _viewModel = (TradeNoteEditVM)DataContext;
        }

        private async void TradeNoteEditView_SourceInitialized(object? sender, EventArgs e)
        {
            OnSourceInitialized();
        }
        partial void OnSourceInitialized();

        private void IntegerTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (sender is not TextBox tb)
                return;

            e.Handled = !char.IsDigit(e.Text, 0);
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_viewModel.ValidateModel())
                return;

            DialogResult = true;
        }
    }
}