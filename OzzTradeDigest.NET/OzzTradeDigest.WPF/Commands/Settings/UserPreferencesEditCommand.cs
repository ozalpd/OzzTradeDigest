using System.Windows;
using TD.AppInfra.Commands;
using TD.i18n;
using TD.WPF.Services;
using TD.WPF.ViewModels.Settings;

namespace TD.WPF.Commands.Settings;

public class UserPreferencesEditCommand : AbstractCommand
{
    protected readonly UserPreferencesVM _viewModel;
    private readonly IWindowDialogService _windowDialogService;

    public UserPreferencesEditCommand(UserPreferencesVM viewModel, IWindowDialogService windowDialogService)
    {
        _viewModel = viewModel;
        _windowDialogService = windowDialogService;
    }

    public override void Execute(object? parameter)
    {
        if (parameter is not Window owner)
            return;

        try
        {
            var dialogResult = _windowDialogService.ShowUserPreferencesDialog(owner);
        }
        catch (Exception ex)
        {
            string innerExceptionMsg = ex.InnerException != null ? $"\n{ex.InnerException.Message}" : "";
            MessageBox.Show($"{MessageStrings.SaveOperationFailed}\n{ex.Message}{innerExceptionMsg}",
                               CommonStrings.AppTitle,
                               MessageBoxButton.OK,
                               MessageBoxImage.Error);
        }
    }
}
