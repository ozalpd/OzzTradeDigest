using System.Collections.Generic;
using System.ComponentModel;
using TD.Extensions;
using TD.Models;

namespace TD.WPF.ViewModels.Trades;

public partial class TradeNoteCreateVM
{
    partial void OnInitialized()
    {
        SetCategoryValues();
        PropertyChanged += TradeNoteCreateVM_PropertyChanged;
    }

    public bool ShowAdherence => Category > TradeNoteCategory.SetupNote;

    public IEnumerable<EnumValueItem<TradeNoteCategory>> FilteredCategoryValues
    {
        get => _filteredCategoryValues;
        set
        {
            if (_filteredCategoryValues != value)
            {
                _filteredCategoryValues = value;
                RaisePropertyChanged(nameof(FilteredCategoryValues));
            }
        }
    }
    IEnumerable<EnumValueItem<TradeNoteCategory>> _filteredCategoryValues;

    private void TradeNoteCreateVM_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Category))
        {
            RaisePropertyChanged(nameof(ShowAdherence));
        }
        else if (e.PropertyName == nameof(Trade))
        {
            TradeId = Trade?.Id ?? 0;
            SetCategoryValues();
        }
    }

    private void SetCategoryValues()
    {
        if (Trade == null || Trade.Id == 0)
        {
            FilteredCategoryValues = TradeNoteCategoryValues;
            return;
        }

        var tmpList = new List<EnumValueItem<TradeNoteCategory>>();
        bool hasSetupNote = Trade.TradeNotes
                                 .Any(n => n.Category == TradeNoteCategory.SetupNote);
        if (hasSetupNote)
        {
            tmpList.AddRange(TradeNoteCategoryValues.Where(c => c.Value != TradeNoteCategory.SetupNote));
        }
        else
        {
            tmpList.AddRange(TradeNoteCategoryValues);
        }

        if (Trade.TradeStatus == TradeStatus.Cancelled || Trade.TradeStatus == TradeStatus.Missed)
        {
            FilteredCategoryValues = tmpList.Where(c => c.Value > TradeNoteCategory.ExitNote);
        }
        else if (Trade.TradeStatus < TradeStatus.Active)
        {
            FilteredCategoryValues = tmpList.Where(c => c.Value < TradeNoteCategory.ExitNote
                                                     || c.Value == TradeNoteCategory.Other);
        }
        else if (Trade.TradeStatus < TradeStatus.Closed)
        {
            FilteredCategoryValues = tmpList.Where(c => c.Value < TradeNoteCategory.ReviewNote
                                                    || c.Value == TradeNoteCategory.Other);
        }
        else
        {
            FilteredCategoryValues = tmpList;
        }
    }
}
