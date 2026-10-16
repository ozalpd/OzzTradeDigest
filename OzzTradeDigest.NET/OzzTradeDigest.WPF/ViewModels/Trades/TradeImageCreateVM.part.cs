using System.ComponentModel;
using TD.Models;
using TD.WPF.Commands;
using TD.WPF.Helpers;

namespace TD.WPF.ViewModels.Trades
{
    public partial class TradeImageCreateVM : IOpenFileContext
    {
        partial void OnInitialized()
        {
            OpenFileCommand = new OpenFileCommand(this);
            PropertyChanged += TradeImageCreateVM_PropertyChanged;
        }

        public OpenFileCommand OpenFileCommand { get; private set; }

        public string OpenFileDialogFilter => FileHelper.GetOpenImgFilter();


        public TradeNoteCategory? Category => _tradeImage?.TradeNote?.Category;
        
        public string? Notes => _tradeImage?.TradeNote?.Content;

        public void SetFilePath(string filePath)
        {
            ImageURL = filePath;
        }

        private void TradeImageCreateVM_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(TradeNote))
            {
                RaisePropertyChanged(nameof(Category));
                RaisePropertyChanged(nameof(Notes));
            }
        }
    }
}
