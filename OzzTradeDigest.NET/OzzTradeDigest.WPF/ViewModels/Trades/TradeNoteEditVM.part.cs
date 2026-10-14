using System.ComponentModel;
using TD.Models;

namespace TD.WPF.ViewModels.Trades
{
    public partial class TradeNoteEditVM
    {
        partial void OnInitialized()
        {
            PropertyChanged += TradeNoteEditVM_PropertyChanged;
        }

        public bool ShowAdherence => Category > TradeNoteCategory.SetupNote;

        private void TradeNoteEditVM_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Category))
            {
                RaisePropertyChanged(nameof(ShowAdherence));
            }
        }
    }
}
