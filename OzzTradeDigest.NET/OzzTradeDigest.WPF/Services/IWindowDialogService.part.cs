using System.Windows;
using TD.Models;

namespace TD.WPF.Services;

public partial interface IWindowDialogService
{
    /// <summary>
    /// Displays the Stop Loss Order creation dialog and returns the result indicating whether the user confirmed the dialog
    /// and the created Stop Loss Order, if any.
    /// </summary>
    /// <param name="owner">The window that will own the dialog. This determines the dialog's parent window for modality and positioning.</param>
    /// <param name="preselectedTrade">An optional preselected trade to initialize the dialog with.</param>
    /// <returns>A tuple containing a Boolean value that is <see langword="true"/> if the user confirmed the dialog; otherwise, <see
    /// langword="false"/>. The second item is the created Stop Loss Order if confirmed; otherwise, <see langword="null"/>.</returns>
    (bool IsConfirmed, ExitOrder? StopLossOrder) ShowStopLossOrderCreateDialog(Window owner, Trade? preselectedTrade);

    /// <summary>
    /// Displays a modal dialog for editing the specified Stop Loss Order and returns the result indicating whether the dialog
    /// was confirmed and if any changes were made.
    /// </summary>
    /// <param name="owner">The window that will own the dialog. This parameter determines the parent window for modal behavior and dialog
    /// positioning.</param>
    /// <param name="stopLossOrder">The Stop Loss Order entity to edit. The dialog will display and allow editing of this stopLossOrder's properties.</param>
    /// <returns>A tuple containing two values: IsConfirmed is <see langword="true"/> if the user confirmed the dialog;
    /// otherwise, <see langword="false"/>. IsDirty is <see langword="true"/> if any changes were made to the stopLossOrder
    /// during editing; otherwise, <see langword="false"/>.</returns>
    (bool IsConfirmed, bool IsDirty) ShowStopLossOrderEditDialog(Window owner, ExitOrder stopLossOrder);

    /// <summary>
    /// Displays the Take Profit Order creation dialog and returns the result indicating whether the user confirmed the dialog
    /// and the created Take Profit Order, if any.
    /// </summary>
    /// <param name="owner">The window that will own the dialog. This determines the dialog's parent window for modality and positioning.</param>
    /// <param name="preselectedTrade">An optional preselected trade to initialize the dialog with.</param>
    /// <returns>A tuple containing a Boolean value that is <see langword="true"/> if the user confirmed the dialog; otherwise, <see
    /// langword="false"/>. The second item is the created Take Profit Order if confirmed; otherwise, <see langword="null"/>.</returns>
    (bool IsConfirmed, ExitOrder? TakeProfitOrder) ShowTakeProfitOrderCreateDialog(Window owner, Trade? preselectedTrade);

    /// <summary>
    /// Displays a modal dialog for editing the specified Take Profit Order and returns the result indicating whether the dialog
    /// was confirmed and if any changes were made.
    /// </summary>
    /// <param name="owner">The window that will own the dialog. This parameter determines the parent window for modal behavior and dialog
    /// positioning.</param>
    /// <param name="takeProfitOrder">The Take Profit Order entity to edit. The dialog will display and allow editing of this takeProfitOrder's properties.</param>
    /// <returns>A tuple containing two values: IsConfirmed is <see langword="true"/> if the user confirmed the dialog;
    /// otherwise, <see langword="false"/>. IsDirty is <see langword="true"/> if any changes were made to the takeProfitOrder
    /// during editing; otherwise, <see langword="false"/>.</returns>
    (bool IsConfirmed, bool IsDirty) ShowTakeProfitOrderEditDialog(Window owner, ExitOrder takeProfitOrder);
}
