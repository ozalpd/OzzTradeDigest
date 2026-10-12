using System.Windows;
using TD.AppInfra.Commands;
using TD.i18n;
using TD.WPF.ViewModels.Trades;

namespace TD.WPF.Commands.Trades;

public partial class TradeImageDeleteCommand : AbstractCommand
{
    protected readonly TradeHistoryVM _viewModel;

    public TradeImageDeleteCommand(TradeHistoryVM viewModel)
    {
        _viewModel = viewModel;
    }

    public override bool CanExecute(object? parameter)
    {
        var tradeImage = _viewModel.SelectedTradeImage;
        return tradeImage != null && tradeImage.Id > 0;
    }

    public override async void Execute(object? parameter)
    {
        var tradeImage = _viewModel.SelectedTradeImage;
        if (tradeImage is null || tradeImage.Id < 1)
            return;

        var result = MessageBox.Show(
            MessageStrings.AreYouSureToDelete,
            CommonStrings.AppTitle,
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (result != MessageBoxResult.Yes)
            return;

        await _viewModel.DeleteTradeImageAsync(tradeImage);
    }
}
