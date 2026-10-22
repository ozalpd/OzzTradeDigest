using System.Windows;
using TD.Models;
using TD.WPF.Views.Settings;
using TD.WPF.Views.Trades;

namespace TD.WPF.Services;

public partial class WindowDialogService : IWindowDialogService
{

    /// <inheritdoc />
    public (bool IsConfirmed, ExitOrder? StopLossOrder) ShowStopLossOrderCreateDialog(Window owner, Trade? preselectedTrade)
    {
        var dialog = new StopLossOrderCreateView(preselectedTrade)
        {
            Owner = owner
        };

        var result = dialog.ShowDialog() == true;
        return (result, result ? dialog.StopLossOrder : null);
    }

    /// <inheritdoc />
    public (bool IsConfirmed, bool IsDirty) ShowStopLossOrderEditDialog(Window owner, ExitOrder stopLossOrder)
    {
        var dialog = new StopLossOrderEditView(stopLossOrder)
        {
            Owner = owner
        };

        var result = dialog.ShowDialog() == true;
        return (result, dialog.IsDirty);
    }

    /// <inheritdoc />
    public (bool IsConfirmed, ExitOrder? TakeProfitOrder) ShowTakeProfitOrderCreateDialog(Window owner, Trade? preselectedTrade)
    {
        var dialog = new TakeProfitOrderCreateView(preselectedTrade)
        {
            Owner = owner
        };

        var result = dialog.ShowDialog() == true;
        return (result, result ? dialog.TakeProfitOrder : null);
    }

    /// <inheritdoc />
    public (bool IsConfirmed, bool IsDirty) ShowTakeProfitOrderEditDialog(Window owner, ExitOrder takeProfitOrder)
    {
        var dialog = new TakeProfitOrderEditView(takeProfitOrder)
        {
            Owner = owner
        };

        var result = dialog.ShowDialog() == true;
        return (result, dialog.IsDirty);
    }

    /// <inheritdoc />
    public bool ShowUserPreferencesDialog(Window owner)
    {
        var dialog = new UserPreferencesView
        {
            Owner = owner
        };
        return dialog.ShowDialog() == true;
    }
}
