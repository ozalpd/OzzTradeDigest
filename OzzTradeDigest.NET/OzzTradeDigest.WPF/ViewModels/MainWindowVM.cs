using TD.AppInfra.Models;
using TD.AppInfra.Services;
using TD.AppInfra.ViewModels;
using TD.WPF.Commands;
using TD.WPF.Commands.Settings;
using TD.WPF.Services;
using TD.WPF.ViewModels.Settings;
using TD.WPF.ViewModels.Trades;

namespace TD.WPF.ViewModels
{
    internal class MainWindowVM : AbstractDataErrorInfoVM
    {
        private readonly IWindowDialogService _windowDialogService;

        public MainWindowVM(AppDataSources dataSources) : this(dataSources, new WindowDialogService()) { }

        public MainWindowVM(AppDataSources dataSources, IWindowDialogService windowDialogService)
        {
            _windowDialogService = windowDialogService;
            var symbolLookupService = new SymbolLookupService(dataSources.SymbolRepository);
            var tradingAccountLookupService = new TradingAccountLookupService(dataSources.TradingAccountRepository);

            TradeHistory = new TradeHistoryVM(dataSources.TradeRepository, dataSources.TradeNoteRepository, _windowDialogService,
                                              symbolLookupService, tradingAccountLookupService);

            UserPreferencesEditCommand = new UserPreferencesEditCommand(new UserPreferencesVM(), _windowDialogService);
            ShowMaintenanceCommand = new ShowMaintenanceCommand(dataSources);
            ShowAboutCommand = new ShowAboutCommand();
            ExitCommand = new ExitCommand();
        }

        public UserPreferencesEditCommand UserPreferencesEditCommand { get; }

        public TradeHistoryVM TradeHistory { get; }
        public ShowMaintenanceCommand ShowMaintenanceCommand { get; }
        public ShowAboutCommand ShowAboutCommand { get; }
        public ExitCommand ExitCommand { get; }
    }
}
