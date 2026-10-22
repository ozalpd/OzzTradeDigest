using System.Windows;
using System.Windows.Input;
using TD.i18n;
using TD.WPF.ViewModels.Settings;

namespace TD.WPF.Views.Settings;

/// <summary>
/// Interaction logic for UserPreferencesView.xaml
/// </summary>
public partial class UserPreferencesView
{
    private readonly UserPreferencesVM _viewModel;

    public UserPreferencesView() : base(new UserPreferencesVM())
    {
        InitializeComponent();
        _viewModel = (UserPreferencesVM)DataContext;
    }

    private void DecimalTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        e.Handled = !char.IsDigit(e.Text, 0) && e.Text != ".";
    }

    private void BrowseFolder_Click(object sender, RoutedEventArgs e)
    {
        // WPF OpenFolderDialog (available in .NET 8 / .NET 10)
        var dialog = new Microsoft.Win32.OpenFolderDialog
        {
            InitialDirectory = _viewModel.ImagesFolder
        };

        if (dialog.ShowDialog() == true)
        {
            _viewModel.ImagesFolder = dialog.FolderName;
        }
    }

    private void OkButton_Click(object sender, RoutedEventArgs e)
    {
        bool requiresRestart = _viewModel.SaveSettings();

        if (requiresRestart)
        {
            MessageBox.Show(MessageStrings.LanguageSettingNeedsRestart,
                            CommonStrings.AppTitle,
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
        }

        DialogResult = true;
    }
}
