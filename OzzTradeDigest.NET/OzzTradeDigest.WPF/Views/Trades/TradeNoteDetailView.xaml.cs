using System.Windows;
using TD.Models;
using TD.WPF.ViewModels.Trades;

namespace TD.WPF.Views.Trades
{
    // Codebehind for TradeNoteDetailView.xaml
    public partial class TradeNoteDetailView : Window
    {
        private readonly TradeNote _viewModel;


        /// <summary>
        /// This constructor should not be called, but we need it for the designer to work.
        /// </summary>
        /// <remarks>This constructor creates a dummy TradeNote for the designer.</remarks>
        public TradeNoteDetailView()
        {
            InitializeComponent();

            _viewModel = new TradeNote();
            DataContext = _viewModel;
            SourceInitialized += TradeNoteDetailView_SourceInitialized;
        }

        /// <summary>
        /// This constructor should be used at runtime to create the view with a real TradeNote.
        /// </summary>
        /// <param name="tradeNote">The real TradeNote to be used by the view.</param>
        public TradeNoteDetailView(TradeNote tradeNote)
        {
            InitializeComponent();

            _viewModel = tradeNote;
            DataContext = _viewModel;
            SourceInitialized += TradeNoteDetailView_SourceInitialized;
        }

        private async void TradeNoteDetailView_SourceInitialized(object? sender, EventArgs e)
        {
            OnSourceInitialized();
        }
        partial void OnSourceInitialized();
    }
}