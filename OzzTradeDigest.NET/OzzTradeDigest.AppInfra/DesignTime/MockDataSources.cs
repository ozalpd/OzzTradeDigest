using TD.AppInfra.Models;

namespace TD.AppInfra.DesignTime
{
    public class MockDataSources : AppDataSources
    {
        public MockDataSources() : base(new CurrencyMockRepository(), new EntryOrderMockRepository(),
                                        new ExchangeMockRepository(), new ExitOrderMockRepository(),
                                        new SymbolMockRepository(),
                                        new TradingAccountMockRepository(), new TradeMockRepository())
        {

        }
    }
}
