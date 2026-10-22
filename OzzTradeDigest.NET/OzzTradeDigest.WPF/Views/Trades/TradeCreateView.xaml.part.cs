using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TD.WPF.Views.Trades
{
    public partial class TradeCreateView
    {
        async Task OnSourceInitialized()
        {
            await _viewModel.SetAllSymbolsAsync();
            _viewModel.SetSymbols();

            PlannedPositionValueTextBox.PreviewTextInput += CalculatedTextBox_TextInput;
            PlannedEntryPriceTextBox.PreviewTextInput += CalculatedTextBox_TextInput;
            _viewModel.PropertyChanged += OnPropertyChanged;

            SetPositionValueReadOnly();
            _viewModel.Step = 1;
        }

        private void OnPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_viewModel.PlannedEntryPrice))
            {
                SetPositionValueReadOnly();
            }
            else if (e.PropertyName == nameof(_viewModel.Step))
            {
                switch (_viewModel.Step)
                {
                    case 1:
                        Step1Grid.Visibility = Visibility.Visible;
                        Step2Grid.Visibility = Visibility.Collapsed;
                        OkButton.IsEnabled = false;
                        break;

                    case 2:
                        Step1Grid.Visibility = Visibility.Collapsed;
                        Step2Grid.Visibility = Visibility.Visible;
                        OkButton.IsEnabled = true;
                        break;

                    default:
                        break;
                }

                PreviousButton.IsEnabled = _viewModel.Step > minStep;
                NextButton.IsEnabled = _viewModel.Step < maxStep;
            }
        }
        int minStep = 1;
        int maxStep = 2;

        private void CalculatedTextBox_TextInput(object sender, TextCompositionEventArgs e)
        {
            if (sender is not TextBox tb)
                return;

            if (tb == PlannedPositionValueTextBox)
                _viewModel.SetOrderQuantity(tb.Text + e.Text);

            if (tb == PlannedEntryPriceTextBox)
            {
                if (_viewModel.PlannedEntryPrice > 0 && e.Text == "\t")
                {
                    PlannedPositionValueTextBox.Focus();
                }

                if (_viewModel.PlannedEntryPrice > 0)
                    return;

                if (decimal.TryParse(tb.Text, out var price) && price > 0)
                {
                    istyping = true;
                    _viewModel.PlannedEntryPrice = price;
                    istyping = false;
                }
            }
        }
        bool istyping = false;

        public void SetPositionValueReadOnly()
        {
            if (_viewModel.PlannedEntryPrice > 0)
            {
                PlannedPositionValueTextBox.Style = (Style)FindResource("ValidationTextBoxStyle");
                if (!istyping)
                    PlannedPositionValueTextBox.Focus();
            }
            else
            {
                PlannedPositionValueTextBox.Style = (Style)FindResource("ReadOnlyTextBoxStyle");
            }
        }
    }
}
