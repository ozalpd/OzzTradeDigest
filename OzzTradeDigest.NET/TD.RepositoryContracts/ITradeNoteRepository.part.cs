using TD.Models;

namespace TD.RepositoryContracts
{
    public partial interface ITradeNoteRepository
    {
        Task SaveTradeImagesAsync(TradeNote tradeNote);
    }
}
