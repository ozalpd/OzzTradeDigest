using System.Windows;
using TD.AppInfra.Commands;
using TD.i18n;
using TD.WPF.ViewModels.Trades;

namespace TD.WPF.Commands.Trades;

public partial class TradeNoteDeleteCommand : AbstractCommand
{
    protected readonly TradeHistoryVM _viewModel;

    public TradeNoteDeleteCommand(TradeHistoryVM viewModel)
    {
        _viewModel = viewModel;
    }

    public override bool CanExecute(object? parameter)
    {
        var tradeNote = _viewModel.SelectedTradeNote;
        if (tradeNote is null || tradeNote.Id < 1)
            return false;

        var result = _viewModel.TradeNoteRepository
                               .CanDeleteAsync(tradeNote.Id)
                               .GetAwaiter()
                               .GetResult();

        return result;
    }

    public override async void Execute(object? parameter)
    {
        var tradeNote = _viewModel.SelectedTradeNote;
        if (tradeNote is null || tradeNote.Id < 1)
            return;

        var result = MessageBox.Show(
            MessageStrings.AreYouSureToDelete,
            CommonStrings.AppTitle,
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (result != MessageBoxResult.Yes)
            return;

        await _viewModel.DeleteTradeNoteAsync(tradeNote);
    }
}
