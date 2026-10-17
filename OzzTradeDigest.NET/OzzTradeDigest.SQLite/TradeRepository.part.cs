using Microsoft.Data.Sqlite;
using TD.Helpers;
using TD.Models;
using TD.SQLite.Extensions;

namespace TD.SQLite
{
    public partial class TradeRepository
    {
        partial void OnAppendingWhere(TradeQueryParameters queryParameters, List<string> whereClauses, SqliteCommand command)
        {
            AppendTradeDatesToWhereClause(queryParameters, whereClauses, command);

            if (queryParameters.TradeStatus.HasValue && queryParameters.TradeStatus.Value is TradeStatusQuery stat && (int)stat != 0)
            {
                if ((int)stat < 1000)
                {
                    whereClauses.Add("TradeStatus = @tradeStatus");
                    command.AddParameter("@tradeStatus", (int)stat);
                }
                else if (stat == TradeStatusQuery.Waiting)
                {
                    whereClauses.Add("TradeStatus < @tradeStatusOpen");
                    command.AddParameter("@tradeStatusOpen", (int)TradeStatusQuery.Active);
                }
                else if (stat == TradeStatusQuery.ActiveOrWaiting)
                {
                    whereClauses.Add("TradeStatus < @tradeStatusClosed");
                    command.AddParameter("@tradeStatusClosed", (int)TradeStatusQuery.Closed);
                }
                else if (stat == TradeStatusQuery.MissedOrCancelled)
                {
                    whereClauses.Add("TradeStatus < @notMissedOrCancelled");
                    command.AddParameter("@notMissedOrCancelled", 0);
                }

                if (stat == TradeStatusQuery.Waiting || stat == TradeStatusQuery.ActiveOrWaiting)
                {
                    whereClauses.Add("TradeStatus > @notMissedOrCancelled");
                    command.AddParameter("@notMissedOrCancelled", 0);
                }
            }
        }

        partial void OnCreated(Trade trade)
        {
            _ = SaveNavigationCollectionsAsync(trade);
        }

        partial void OnLoaded(Trade trade)
        {
            _ = LoadNavigationCollectionsAsync(trade);
        }

        partial void OnUpdated(Trade trade)
        {
            _ = SaveNavigationCollectionsAsync(trade);
        }

        private void AppendTradeDatesToWhereClause(TradeQueryParameters queryParameters, List<string> whereClauses, SqliteCommand command)
        {
            if (queryParameters?.TradeDateType == null)
                return;

            //queryParameters.EntryTimeMin = null; queryParameters.EntryTimeMax = null;
            //queryParameters.ExitTimeMin = null; queryParameters.ExitTimeMax = null;
            //queryParameters.UpdatedAtMin = null; queryParameters.UpdatedAtMax = null;

            switch (queryParameters.TradeDateType.Value)
            {
                case TradeDateType.EntryTime:
                    queryParameters.EntryTimeMin = queryParameters.StartDate;
                    queryParameters.EntryTimeMax = queryParameters.EndDate;
                    break;

                case TradeDateType.ExitTime:
                    queryParameters.ExitTimeMin = queryParameters.StartDate;
                    queryParameters.ExitTimeMax = queryParameters.EndDate;
                    break;

                case TradeDateType.CancellationTime:
                    queryParameters.CancellationTimeMin = queryParameters.StartDate;
                    queryParameters.CancellationTimeMax = queryParameters.EndDate;
                    break;

                case TradeDateType.UpdateTime:
                    queryParameters.UpdatedAtMin = queryParameters.StartDate;
                    queryParameters.UpdatedAtMax = queryParameters.EndDate;
                    break;

                default:
                    break;
            }
        }

        public async Task LoadNavigationCollectionsAsync(Trade trade)
        {
            try
            {
                var images = await TradeImageRepository.GetByTradeIdAsync(trade.Id);
                trade.TradeImages = images.ToList();
                foreach (var image in trade.TradeImages)
                {
                    image.Trade = trade;
                }
            }
            catch (Exception)
            {
                // handle/log as needed
            }

            try
            {
                var notes = await TradeNoteRepository.GetByTradeIdAsync(trade.Id);
                trade.TradeNotes = notes.ToList();
                foreach (var note in trade.TradeNotes)
                {
                    note.Trade = trade;
                    note.TradeImages = trade.TradeImages
                                            .Where(i => i.TradeNoteId == note.Id)
                                            .ToList();
                    foreach (var image in note.TradeImages)
                    {
                        image.TradeNote = note;
                    }
                }
            }
            catch (Exception)
            {
                // handle/log as needed
            }

            try
            {
                var entryOrders = await EntryOrderRepository.GetByTradeIdAsync(trade.Id);
                trade.EntryOrders = entryOrders.ToList();
                foreach (var order in trade.EntryOrders)
                {
                    order.Trade = trade;
                }
            }
            catch (Exception)
            {
                // handle/log as needed
            }

            try
            {
                var exitOrders = await ExitOrderRepository.GetByTradeIdAsync(trade.Id);
                trade.TakeProfitOrders = exitOrders.Where(o => o.ExitOrderMode == ExitOrderMode.PlannedTP)
                                                   .ToList();
                trade.StopLossOrders = exitOrders.Where(o => o.ExitOrderMode == ExitOrderMode.PlannedSL)
                                                 .ToList();
                foreach (var order in trade.TakeProfitOrders)
                {
                    order.Trade = trade;
                }
                foreach (var order in trade.StopLossOrders)
                {
                    order.Trade = trade;
                }
            }
            catch (Exception)
            {
                // handle/log as needed
            }
        }

        public async Task SaveNavigationCollectionsAsync(Trade trade)
        {
            if (trade == null || updatingFromSaveNavigationCollectionsAsync)
                return;

            updatingFromSaveNavigationCollectionsAsync = true;
            try
            {
                await SaveTradeNotesAsync(trade, updateTrade: false);
            }
            catch (Exception)
            {
                throw;
            }

            try
            {
                await SaveEntryOrdersAsync(trade, updateTrade: false);
                await SaveTakeProfitOrdersAsync(trade, updateTrade: false);
                await SaveStopLossOrdersAsync(trade, updateTrade: false);

                trade.CalculateFromOrders();
                await UpdateAsync(trade);
            }
            finally
            {
                updatingFromSaveNavigationCollectionsAsync = false;
            }
        }
        bool updatingFromSaveNavigationCollectionsAsync = false;

        public async Task SaveEntryOrdersAsync(Trade trade, bool updateTrade = true)
        {
            bool anyChanges = false;
            var existingOrders = await EntryOrderRepository.GetByTradeIdAsync(trade.Id);
            var existingOrderIds = existingOrders.Select(o => o.Id).ToHashSet();
            foreach (var order in trade.EntryOrders)
            {
                if (order.Id == 0)
                {
                    order.TradeId = trade.Id;
                    await EntryOrderRepository.CreateAsync(order);
                    anyChanges = true;
                }
                else if (existingOrderIds.Contains(order.Id))
                {
                    anyChanges = await EntryOrderRepository.UpdateAsync(order) || anyChanges;
                    existingOrderIds.Remove(order.Id);
                }
            }
            foreach (var id in existingOrderIds)
            {
                await EntryOrderRepository.DeleteAsync(id);
                anyChanges = true;
            }

            if (anyChanges && updateTrade)
            {
                trade.CalculateFromOrders();
                await UpdateAsync(trade);
            }
        }

        public async Task SaveStopLossOrdersAsync(Trade trade, bool updateTrade = true)
        {
            bool anyChanges = false;
            var existingOrders = await ExitOrderRepository.GetByTradeIdAsync(trade.Id);
            var existingOrderIds = existingOrders.Where(o => o.ExitOrderMode == ExitOrderMode.PlannedSL)
                                                 .Select(o => o.Id).ToHashSet();
            foreach (var order in trade.StopLossOrders)
            {
                if (order.Id == 0)
                {
                    order.TradeId = trade.Id;
                    order.ExitOrderMode = ExitOrderMode.PlannedSL;
                    await ExitOrderRepository.CreateAsync(order);
                    anyChanges = true;
                }
                else if (existingOrderIds.Contains(order.Id))
                {
                    order.TradeId = trade.Id;
                    order.ExitOrderMode = ExitOrderMode.PlannedSL;
                    anyChanges = await ExitOrderRepository.UpdateAsync(order) || anyChanges;
                    existingOrderIds.Remove(order.Id);
                }
            }
            foreach (var id in existingOrderIds)
            {
                await ExitOrderRepository.DeleteAsync(id);
                anyChanges = true;
            }

            if (anyChanges && updateTrade)
            {
                trade.CalculateFromOrders();
                await UpdateAsync(trade);
            }
        }

        public async Task SaveTakeProfitOrdersAsync(Trade trade, bool updateTrade = true)
        {
            bool anyChanges = false;
            var existingOrders = await ExitOrderRepository.GetByTradeIdAsync(trade.Id);
            var existingOrderIds = existingOrders.Where(o => o.ExitOrderMode == ExitOrderMode.PlannedTP).Select(o => o.Id).ToHashSet();
            foreach (var order in trade.TakeProfitOrders)
            {
                if (order.Id == 0)
                {
                    order.TradeId = trade.Id;
                    order.ExitOrderMode = ExitOrderMode.PlannedTP;
                    await ExitOrderRepository.CreateAsync(order);
                    anyChanges = true;
                }
                else if (existingOrderIds.Contains(order.Id))
                {
                    order.ExitOrderMode = ExitOrderMode.PlannedTP;
                    anyChanges = await ExitOrderRepository.UpdateAsync(order) || anyChanges;
                    existingOrderIds.Remove(order.Id);
                }
            }
            foreach (var id in existingOrderIds)
            {
                await ExitOrderRepository.DeleteAsync(id);
                anyChanges = true;
            }

            if (anyChanges && updateTrade)
            {
                trade.CalculateFromOrders();
                await UpdateAsync(trade);
            }
        }

        public async Task SaveTradeNotesAsync(Trade trade, bool updateTrade = true)
        {
            bool anyChanges = false;
            var existingNotes = await TradeNoteRepository.GetByTradeIdAsync(trade.Id);
            var existingNoteIds = existingNotes.Select(i => i.Id).ToHashSet();
            foreach (var Note in trade.TradeNotes)
            {
                if (Note.Id == 0)
                {
                    Note.TradeId = trade.Id;
                    await TradeNoteRepository.CreateAsync(Note);
                    anyChanges = true;
                }
                else if (existingNoteIds.Contains(Note.Id))
                {
                    anyChanges = await TradeNoteRepository.UpdateAsync(Note) || anyChanges;
                    existingNoteIds.Remove(Note.Id);
                }
            }
            foreach (var id in existingNoteIds)
            {
                await TradeNoteRepository.DeleteAsync(id);
                anyChanges = true;
            }

            if (anyChanges && updateTrade)
            {
                await UpdateAsync(trade);
            }
        }
    }
}