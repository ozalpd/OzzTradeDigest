using System.Collections.ObjectModel;
using TD.Extensions;
using TD.Models;
using TD.WPF.Commands.Trades;

namespace TD.WPF.ViewModels.Trades;

public partial class TradeHistoryVM
{
    public ObservableCollection<TradeNote> TradeNotes { get; }
    //public TradeNoteCreateCommand TradeNoteCreateCommand { get; }
    //public TradeNoteDeleteCommand TradeNoteDeleteCommand { get; }
    //public TradeNoteDetailCommand TradeNoteDetailCommand { get; }
    //public TradeNoteEditCommand TradeNoteEditCommand { get; }

    /// <summary>
    /// Gets the collection of available TradeNoteCategory enum members for selection or display.
    /// </summary>
    public IEnumerable<EnumValueItem<TradeNoteCategory>> TradeNoteCategoryValues { get; }

    public TradeNote? SelectedTradeNote
    {
        get { return _selectedTradeNote; }
        set
        {
            _selectedTradeNote = value;
            RaisePropertyChanged(nameof(SelectedTradeNote));
            RaiseTradeNoteCmdCanExecute();
        }
    }
    TradeNote? _selectedTradeNote;

    /// <summary>
    /// Removes <paramref name="tradeNote"/> from <see cref="Trade.TradeNotes"/> of
    /// <see cref="SelectedTrade"/>, then persists <em>all</em> remaining entry orders
    /// in a single repository call (<see cref="ITradeRepository.SaveTradeNotesAsync"/>).
    /// </summary>
    /// <remarks>
    /// The save call persists the entire <see cref="Trade.TradeNotes"/> collection after
    /// removal, so any other in-memory modifications to that collection are also committed.
    /// Returns <c>false</c> without saving if <see cref="SelectedTrade"/> is <c>null</c> or
    /// <paramref name="tradeNote"/> has not been persisted yet (<c>Id == 0</c>).
    /// </remarks>
    /// <param name="tradeNote">The entry order to delete.</param>
    /// <returns><c>true</c> if the order was found and removed; otherwise <c>false</c>.</returns>
    public async Task<bool> DeleteTradeNoteAsync(TradeNote tradeNote)
    {
        if (SelectedTrade == null)
            return false;

        ArgumentNullException.ThrowIfNull(tradeNote, nameof(tradeNote));
        bool isDeleted = false;
        if (tradeNote.Id > 0)
        {
            isDeleted = SelectedTrade.TradeNotes.Remove(tradeNote);
            await TradeRepository.SaveTradeNotesAsync(SelectedTrade);
            RefreshTrades();
        }

        return isDeleted;
    }

    private void ReplaceTradeNotes()
    {
        TradeNotes.Clear();
        if (SelectedTrade != null)
        {
            ReplaceCollection(TradeNotes, SelectedTrade.TradeNotes);
        }
        RaiseTradeNoteCmdCanExecute();
    }

    private void RaiseTradeNoteCmdCanExecute()
    {
        //TradeNoteCreateCommand.RaiseCanExecuteChanged();
        //TradeNoteDeleteCommand.RaiseCanExecuteChanged();
        //TradeNoteEditCommand.RaiseCanExecuteChanged();
        //TradeNoteDetailCommand.RaiseCanExecuteChanged();
    }

    /// <summary>
    /// Adds or updates <paramref name="TradeNote"/> within <see cref="Trade.TradeNotes"/> of
    /// <see cref="SelectedTrade"/>, then persists <em>all</em> entry orders of that trade
    /// in a single repository call (<see cref="ITradeRepository.SaveTradeNotesAsync"/>).
    /// </summary>
    /// <remarks>
    /// Despite its singular name, this method always saves the entire
    /// <see cref="Trade.TradeNotes"/> collection — not just the supplied order.
    /// If <paramref name="TradeNote"/> is new (<c>Id == 0</c>) it is appended to the collection
    /// before saving; if it already exists it is updated in place.
    /// </remarks>
    /// <param name="TradeNote">The entry order to add or update.</param>
    public async Task SaveTradeNoteAsync(TradeNote TradeNote)
    {
        if (SelectedTrade == null)
            return;
        if (TradeNote.Id == 0)
        {
            SelectedTrade.TradeNotes.Add(TradeNote);
            TradeNote.Trade = SelectedTrade;
        }
        await TradeRepository.SaveTradeNotesAsync(SelectedTrade);
        RefreshTrades();
    }
}
