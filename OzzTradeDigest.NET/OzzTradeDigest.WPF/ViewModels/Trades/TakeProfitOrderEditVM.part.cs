namespace TD.WPF.ViewModels.Trades
{
    public partial class TakeProfitOrderEditVM
    {
        partial void OnInitialized()
        {
            PropertyChanged += OnPropertyChanged;
        }


        /// <summary>
        /// Gets the planned profit amount for this specific take profit order,
        /// calculated from the trade's entry price, this order's price and quantity.
        /// Recalculates when OrderPrice, OrderQuantity or Trade changes.
        /// </summary>
        public decimal? OrderProfitAmount => _takeProfitOrder.OrderProfitAmount;

        private void OnPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName is nameof(OrderPrice) or nameof(OrderQuantity) or nameof(Trade))
            {
                RaisePropertyChanged(nameof(OrderValue));
                RaisePropertyChanged(nameof(OrderProfitAmount));
            }

            if (e.PropertyName is nameof(FilledPrice) or nameof(FilledQuantity) or nameof(Trade))
            {
                RaisePropertyChanged(nameof(FilledValue));
            }
        }
    }
}
