# OzzTradeDigest.WPF — Development Roadmap

This file is an AI-readable planning document. It tracks implemented features and
prioritized upcoming work. Update status markers as features are completed.

## Status Markers
- ✅ Done
- 🔴 Cycle 1 — Blocking (do first)
- 🟠 Cycle 2 — Usability
- 🟡 Cycle 3 — Analytics
- 🟢 Cycle 4 — Import/Export
- ⚪ Cycle 5 — Polish

---

## ✅ Implemented

- Maintenance CRUD: Currency, Exchange, Symbol, TradingAccount (full create/edit/delete with views)
- MakerFeeRate / TakerFeeRate in TradingAccount create/edit views
- Trade CRUD: create, edit, delete dialogs with full field coverage
- TotalFeesCorrected and FundingFeeTotal entry in TradeEditView
- Trade list: master pane with paging, date-range + account + symbol + status filters
- P&L color coding in trade list (`NetProfitLoss` and `RealizedProfitLoss` colored via `ProfitLossToColor`)
- Trade detail pane: inline in MainWindow — all trade properties, order grids with add/edit/delete buttons, image thumbnails
- TradeDetailView: standalone popup window with full trade + orders + notes
- Order CRUD: EntryOrder, StopLossOrder, TakeProfitOrder — full create/edit/delete
- Notes field in order create/edit views (EntryOrder, StopLossOrder, TakeProfitOrder)
- Order calculated display props: `OrderRiskAmount` (SL VMs), `OrderProfitAmount` (TP VMs)
- TradeImage CRUD: full create/edit/delete/detail views, dialogs, image picker, thumbnail strip
- Converters: TradeStatusToColor, TradeDirectionToColor, BoolToColor, ProfitLossToColor
- Database backup service: AutoBackupHelper, DatabaseBackupService (logic only, no UI trigger yet)
- About dialog
- App infrastructure: AppSettings, AppVersion, WindowPosition, composition root in App.OnStartup

---

## 🔴 Cycle 1 — Core Workflow Gaps (blocking daily use)

### 1. TradeDirection + MarketType filters in toolbar
- TradeStatus filter is implemented; TradeDirection and MarketType filters are still missing from the toolbar
- Add ComboBox filters for TradeDirection and MarketType to the filter toolbar in `MainWindow.xaml`
- Both have corresponding `*Values` collections already in `TradeListVM` and properties on `QueryVM`

### 2. Fee auto-calculation wiring
- `Trade.TotalFeesCalculated` is persisted but never automatically populated
- `CalculateFromOrders()` in `Trade.calc.cs` / `Trade.part.cs` should sum `FeeCalculated` from all EntryOrders + TakeProfitOrders + StopLossOrders
- `FeeCalculated` on order entities is a calculated-only property: `FilledValue * rate` (maker/taker from TradingAccount)
- `NetProfitLoss` must be computed after `TotalFeesCalculated` is set

---

## 🟠 Cycle 2 — Usability & Completeness

### 3. Tags filter in toolbar
- `Trade.Tags` is a persisted free-text field (255 chars)
- Add a text search filter to the filter toolbar bound to `QueryVM.ByTags` or similar
- `TradeQueryParameters` may need a `Tags` search property added

### 4. Trade detail pane tabs
- The detail pane is getting tall and hard to scan
- Split into tabs: **Details** | **Orders** | **Images** | **Notes**
- Keep the trade header (account, symbol, status, direction) always visible above the tabs

### 5. CancellationTime auto-sync in TradeEditView
- When `TradeStatus` is set to `Cancelled`, auto-fill `CancellationTime` with current UTC time
- When `CancellationTime` is cleared, reset `TradeStatus` away from `Cancelled`
- Bidirectional sync; implement in `TradeEditVM.part.cs`

---

## 🟡 Cycle 3 — Analytics & Reporting

### 6. Summary view
- `ShowSummaryCommand` is in the menu but not implemented (command is null / no-op)
- Show: total trades, win rate, average R, average NetProfitLoss, grouped by account / symbol / period
- New `SummaryWindow` + `SummaryWindowVM`; open from `ShowSummaryCommand`

### 7. RealizedR display
- `RealizedR` is calculated and persisted on `Trade`
- Add a column to the trade list DataGrid
- Show in trade detail pane alongside `PlannedRiskRewardRatio`

### 8. PlannedRiskRewardRatio visual indicator
- Add color or icon to the R:R column in the trade list: < 1.0 = red, 1.0–1.9 = yellow, ≥ 2.0 = green
- Use a CellStyle with DataTriggers or a converter

---

## 🟢 Cycle 4 — Import / Export

### 9. CSV export
- File → Export menu item exists but command is not implemented
- Export the currently filtered trade list to CSV
- Include all visible columns; let user choose file path via SaveFileDialog

### 10. ByBit CSV import
- File → Import menu item exists but command is not implemented
- Map ByBit trade history export format to Trade + Order entities
- Show a preview/mapping dialog before committing

### 11. Database backup UI
- `DatabaseBackupService` is fully implemented with ZIP + timestamp
- No UI exposes it — add a menu item or toolbar button to trigger manual backup
- Show last backup timestamp somewhere (status bar or About dialog)

---

## ⚪ Cycle 5 — Polish & Low Priority

### 12. TradeStatus color fix — Missed / Cancelled
- Current pink tones (#FFCEE3, #FFB0D2) for Missed/Cancelled are misleading (reads as "loss")
- Change to neutral tones: Missed → soft lavender (#E8E0F0), Cancelled → light grey (#D8D8D8)
- Edit `TradeStatusToColor.cs`

### 13. App settings dialog
- `AppSettings` + `UiCulture` exist with no UI to change them
- New `SettingsDialog` with: UI language/culture picker, default page size, database path override
- Add menu item under File or new Settings menu

### 14. TradeDetail popup improvements
- Add print/export button to `TradeDetailView`
- Add keyboard shortcut (e.g. `F4` or `Ctrl+D`) to open it from the main window

### 15. Keyboard shortcuts
- `Ctrl+N` — create trade
- `Delete` — delete selected trade (with confirmation)
- `Enter` — open trade detail
- `F5` — reload / apply filters
- Add via `InputBindings` on `MainWindow`

### 16. Charts view
- `ShowChartsCommand` in menu but not implemented
- Equity curve, win/loss pie, P&L by symbol bar chart
- Low priority until Summary view (Cycle 3) is stable
