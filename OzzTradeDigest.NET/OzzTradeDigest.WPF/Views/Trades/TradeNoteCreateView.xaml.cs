using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TD.Models;
using TD.WPF.ViewModels.Trades;

namespace TD.WPF.Views.Trades
{
    // Codebehind for TradeNoteCreateView.xaml
    public partial class TradeNoteCreateView : Window
    {
        private readonly TradeNoteCreateVM _viewModel;
        private Trade? _preselectedTrade;

        public TradeNote TradeNote => _viewModel.TradeNote;

        /// <summary>
        /// This constructor should not be called, but we need it for the designer to work.
        /// </summary>
        /// <remarks>This constructor creates a dummy TradeNote for the designer.</remarks>
        public TradeNoteCreateView() : this(null)
        {

        }

        /// <summary>
        /// This constructor should be used at runtime to create the view with a real TradeNote.
        /// </summary>
        /// <param name="tradeNote">The real TradeNote to be used by the view.</param>
        public TradeNoteCreateView(Trade? preselectedTrade)
        {
            InitializeComponent();

            _viewModel = new TradeNoteCreateVM();
            DataContext = _viewModel;
            _preselectedTrade = preselectedTrade;
            SourceInitialized += TradeNoteCreateView_SourceInitialized;
        }

        private async void TradeNoteCreateView_SourceInitialized(object? sender, EventArgs e)
        {
            if (_preselectedTrade != null)
            {
                _viewModel.TradeId = _preselectedTrade.Id;
                _viewModel.Trade = _preselectedTrade;
            }
        }

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

            if (_viewModel.Category == TradeNoteCategory.SetupNote)
                _viewModel.Adherence = null;

            if (!string.IsNullOrWhiteSpace(_viewModel.ImageURL))
            {
                var img = new TradeImage
                {
                    ImageURL = _viewModel.ImageURL.Trim()
                };
                _viewModel.TradeNote.TradeImages.Add(img);
            }

            DialogResult = true;
        }
    }
}