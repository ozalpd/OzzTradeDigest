using TD.WPF.Commands;
using TD.WPF.Helpers;

namespace TD.WPF.ViewModels.Trades
{
    public partial class TradeImageCreateVM : IOpenFileContext
    {
        partial void OnInitialized()
        {
            OpenFileCommand = new OpenFileCommand(this);
        }

        public OpenFileCommand OpenFileCommand { get; private set; }

        public string OpenFileDialogFilter => FileHelper.GetOpenImgFilter();

        public void SetFilePath(string filePath)
        {
            ImageURL = filePath;
        }
    }
}
