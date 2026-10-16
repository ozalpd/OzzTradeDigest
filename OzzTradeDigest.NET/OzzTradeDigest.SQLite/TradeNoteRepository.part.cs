using TD.Models;

namespace TD.SQLite;

public partial class TradeNoteRepository
{
    partial void OnCreated(TradeNote tradeNote)
    {
        _ = SaveTradeImagesAsync(tradeNote);
    }

    partial void OnUpdated(TradeNote tradeNote)
    {
        _ = SaveTradeImagesAsync(tradeNote);
    }

    public async Task SaveTradeImagesAsync(TradeNote tradeNote)
    {
        if (updatingFromSaveImagesAsync || tradeNote == null)
            return;

        updatingFromSaveImagesAsync = true;
        bool anyChanges = false;
        var existingImages = await TradeImageRepository.GetByTradeNoteIdAsync(tradeNote.Id);
        var existingImageIds = existingImages.Select(i => i.Id).ToHashSet();
        foreach (var image in tradeNote.TradeImages)
        {
            if (image.Id == 0)
            {
                image.TradeNoteId = tradeNote.Id;
                image.TradeId = tradeNote.TradeId;
                await TradeImageRepository.CreateAsync(image);
                anyChanges = true;
            }
            else if (existingImageIds.Contains(image.Id))
            {
                anyChanges = await TradeImageRepository.UpdateAsync(image) || anyChanges;
                existingImageIds.Remove(image.Id);
            }
        }
        foreach (var id in existingImageIds)
        {
            await TradeImageRepository.DeleteAsync(id);
            anyChanges = true;
        }

        updatingFromSaveImagesAsync = false;
    }
    bool updatingFromSaveImagesAsync = false;
}
