using TD.Extensions;
using TD.Models;
using static TD.Extensions.EnumExtension;

namespace TD.WPF.ViewModels.Trades
{
    public partial class TradeCreateVM
    {
        IReadOnlyList<Symbol> _allSymbols = null;

        partial void OnInitialized()
        {
            EntryOrderTypeValues = GetValues<EntryOrderType>();
            ExitOrderTypeValues = GetValues<ExitOrderType>();
            ExitOrderForTpValues = ExitOrderTypeValues.Where(v => v.Value <= ExitOrderType.TrailingStop).ToList();
            PropertyChanged += OnPropertyChanged;

            EntryOrderType = EntryOrderType.Limit;
            StopLossOrderType = ExitOrderType.Market;
            TakeProfitOrderType = ExitOrderType.Limit;
        }

        public IEnumerable<EnumValueItem<EntryOrderType>> EntryOrderTypeValues { get; private set; } = Array.Empty<EnumValueItem<EntryOrderType>>();
        public IEnumerable<EnumValueItem<ExitOrderType>> ExitOrderTypeValues { get; private set; } = Array.Empty<EnumValueItem<ExitOrderType>>();
        public IEnumerable<EnumValueItem<ExitOrderType>> ExitOrderForTpValues { get; private set; } = Array.Empty<EnumValueItem<ExitOrderType>>();


        private void OnPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(OrderQuantity) || e.PropertyName == nameof(PlannedEntryPrice))
            {
                if (!_calculatingOrderQuantity)
                    RaisePropertyChanged(nameof(PlannedPositionValue));
            }

            if (e.PropertyName == nameof(PlannedTP) || e.PropertyName == nameof(OrderQuantity))
            {
                RaisePropertyChanged(nameof(PlannedRiskRewardRatio));
                RaisePropertyChanged(nameof(PlannedProfit));
            }

            if (e.PropertyName == nameof(PlannedSL) || e.PropertyName == nameof(OrderQuantity))
            {
                RaisePropertyChanged(nameof(PlannedRiskRewardRatio));
                RaisePropertyChanged(nameof(PlannedRiskAmount));
            }

            if (e.PropertyName == nameof(TradingAccountId))
            {
                if (!_accountSetBySymbol)
                {
                    _accountSetByUser = true;
                    SetSymbols();
                }
            }

            if (e.PropertyName == nameof(SymbolId))
            {
                SetAccountAndMarketType();
                ValidateProperty(Trade, nameof(MarketType));
            }

            if (e.PropertyName == nameof(TradeDirection) || e.PropertyName == nameof(TradeStatus) || e.PropertyName == nameof(PlannedEntryPrice))
            {
                RaisePropertyChanged(nameof(PlannedRiskRewardRatio));
                RaisePropertyChanged(nameof(PlannedRiskAmount));
                RaisePropertyChanged(nameof(PlannedProfit));
                ValidateProperty(Trade, nameof(PlannedTP));
                ValidateProperty(Trade, nameof(PlannedSL));
            }

            if (e.PropertyName == nameof(TradeStatus))
            {
                ValidateProperty(_trade, nameof(EntryTime));
            }

            if (e.PropertyName == nameof(ExitTime))
            {
                ValidateProperty(_trade, nameof(EntryTime));
            }

            if (e.PropertyName == nameof(EntryTime))
            {
                ValidateProperty(_trade, nameof(ExitTime));
                ValidateProperty(_trade, nameof(TradeStatus));
            }

            void SetAccountAndMarketType()
            {
                var symbol = _allSymbols.FirstOrDefault(s => s.Id == SymbolId);
                if (symbol != null)
                {
                    Trade.MarketType = symbol.MarketType;
                    RaisePropertyChanged(nameof(MarketType));
                    if (!_accountSetByUser)
                    {
                        _accountSetBySymbol = true;
                        var account = TradingAccounts?.FirstOrDefault(a => a.ExchangeId == symbol.ExchangeId && a.MarketType == symbol.MarketType);
                        if (account != null)
                        {
                            TradingAccountId = account.Id;
                        }
                        _accountSetBySymbol = false;
                    }
                }
            }
        }
        private bool _accountSetBySymbol = false;
        private bool _accountSetByUser = false;

        public EntryOrderType EntryOrderType
        {
            get => _entryOrderType;
            set
            {
                if (_entryOrderType != value)
                {
                    _entryOrderType = value;
                    RaisePropertyChanged(nameof(EntryOrderType));
                }
            }
        }
        EntryOrderType _entryOrderType;

        public ExitOrderType StopLossOrderType
        {
            get => _stopLossOrderType;
            set
            {
                if (_stopLossOrderType != value)
                {
                    _stopLossOrderType = value;
                    RaisePropertyChanged(nameof(StopLossOrderType));
                }
            }
        }
        ExitOrderType _stopLossOrderType;

        public ExitOrderType TakeProfitOrderType
        {
            get => _takeProfitOrderType;
            set
            {
                if (_takeProfitOrderType != value)
                {
                    _takeProfitOrderType = value;
                    RaisePropertyChanged(nameof(TakeProfitOrderType));
                }
            }
        }
        ExitOrderType _takeProfitOrderType;



        public void AddRelatedEntities()
        {
            AddEntryOrder();
            AddStopLossOrder();
            AddTakeProfitOrder();

            if (!string.IsNullOrWhiteSpace(SetupNotes))
            {
                var note = new TradeNote
                {
                    Content = SetupNotes,
                    TradeId = Trade.Id,
                    Category = TradeNoteCategory.SetupNote,
                    UpdatedAt = DateTime.UtcNow
                };
                Trade.TradeNotes.Add(note);
            }
        }

        private void AddEntryOrder()
        {
            if (PlannedEntryPrice == null || OrderQuantity == null)
                return;

            var entryOrder = new EntryOrder
            {
                Id = 0,
                TradeId = Trade.Id,
                OrderType = EntryOrderType,
                OrderQuantity = OrderQuantity,
                OrderPrice = PlannedEntryPrice.Value
            };
            Trade.EntryOrders.Add(entryOrder);
        }

        private void AddStopLossOrder()
        {
            if (PlannedSL == null || OrderQuantity == null)
                return;
            var slOrder = new ExitOrder
            {
                Id = 0,
                TradeId = Trade.Id,
                OrderType = StopLossOrderType,
                OrderQuantity = OrderQuantity,
                OrderPrice = PlannedSL.Value
            };
            Trade.StopLossOrders.Add(slOrder);
        }

        private void AddTakeProfitOrder()
        {
            if (PlannedTP == null || OrderQuantity == null)
                return;
            var tpOrder = new ExitOrder
            {
                Id = 0,
                TradeId = Trade.Id,
                OrderType = TakeProfitOrderType,
                OrderQuantity = OrderQuantity,
                OrderPrice = PlannedTP.Value
            };
            Trade.TakeProfitOrders.Add(tpOrder);
        }

        public void SetOrderQuantity(decimal positionValue)
        {
            Trade.SetQuantity(positionValue, planned: true);
            RaisePropertyChanged(nameof(OrderQuantity));
        }

        public void SetOrderQuantity(string positionValue)
        {
            decimal posValue = 0;
            if (decimal.TryParse(positionValue, out posValue))
            {
                _calculatingOrderQuantity = true;
                SetOrderQuantity(posValue);
                _calculatingOrderQuantity = false;
            }
        }
        bool _calculatingOrderQuantity = false;

        public int Step
        {
            get => _step;
            set
            {
                if (_step != value)
                {
                    _step = value;
                    RaisePropertyChanged(nameof(Step));
                }
            }
        }
        int _step = 0;

        public string SymbolFilter
        {
            get => _symbolFilter;
            set
            {
                if (_symbolFilter != value)
                {
                    _symbolFilter = value;
                    RaisePropertyChanged(nameof(SymbolFilter));
                    SetSymbols();
                }
            }
        }
        private string _symbolFilter = string.Empty;

        public void SetSymbols()
        {
            if (_allSymbols == null)
                _ = SetAllSymbolsAsync();

            if (Symbols == null || _allSymbols == null)
                return;

            TradingAccount? selectedAccount = TradingAccountId > 0 && TradingAccounts != null
                                            ? TradingAccounts.FirstOrDefault(a => a.Id == TradingAccountId)
                                            : null;
            var exchangeIds = new List<int>();
            var marketTypes = new List<MarketType>();
            if (selectedAccount == null)
            {
                exchangeIds = TradingAccounts?.Select(a => a.ExchangeId).Distinct().ToList() ?? new List<int>();
                marketTypes = TradingAccounts?.Select(a => a.MarketType).Distinct().ToList() ?? new List<MarketType>();
            }
            else
            {
                exchangeIds.Add(selectedAccount.ExchangeId);
                marketTypes.Add(selectedAccount.MarketType);
            }

            var filteredSymbols = _allSymbols.Where(s => exchangeIds.Contains(s.ExchangeId)
                                                     && marketTypes.Contains(s.MarketType));

            if (!string.IsNullOrEmpty(SymbolFilter))
            {
                filteredSymbols = filteredSymbols.Where(s => s.TickerFull.Contains(SymbolFilter, StringComparison.OrdinalIgnoreCase));
            }

            Symbols.Clear();
            foreach (var item in filteredSymbols.OrderBy(s => s.Ticker))
            {
                Symbols.Add(item);
            }
        }

        public async Task SetAllSymbolsAsync()
        {
            if (_allSymbols == null)
                _allSymbols = await _symbolLookupService.GetSymbolsAsync(isActive: true);
        }

        public string? SetupNotes
        {
            get { return _setupNotes; }
            set
            {
                _setupNotes = value;
                RaisePropertyChanged(nameof(SetupNotes));
            }
        }
        string? _setupNotes;
    }
}
