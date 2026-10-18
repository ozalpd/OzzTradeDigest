---
applyTo: "**/*.xaml"
description: "Use when: editing XAML views, creating new WPF pages, adding controls, styling UI elements"
---

# WPF XAML Conventions

## Resource Dictionaries

- **Icons**: Use `BootstrapIcons.xaml` geometry resources — reference via `{StaticResource icon-name}` (Bootstrap Icons v1.13.1 naming)
- **Styles**: Use `Styles.xaml` for shared styles — `DisabledWhenNullTextBoxStyle`, `ValidationTextBoxStyle`, `ValidationComboBoxStyle`, `IconButtonStyle-22x18`, `IconButtonStyle-28x24`
- Merge resource dictionaries in `App.xaml`, not in individual views

## Patterns

- Follow MVVM — **no code-behind logic** except `InitializeComponent()` and window lifecycle
- Bind to ViewModels using `{Binding}` markup — never set data directly in XAML code-behind
- Use `DataTrigger` for null-based disable patterns (see `DisabledWhenNullTextBoxStyle`)
- Validation errors display below controls using `Validation.ErrorTemplate` with `AdornedElementPlaceholder`
- When a `TextBox` uses `ReadOnlyTextBoxStyle`, set `Text` binding `Mode=OneWay`

## Attached Behaviors

Reusable attached behaviors live in `TD.WPF.Behaviors`. Declare the namespace at the top of the XAML file when needed: `xmlns:behaviors="clr-namespace:TD.WPF.Behaviors"`.

- **`AsyncImageLoader`** (`behaviors:AsyncImageLoader.*`) — asynchronously loads an `Image.Source` from a local file path or web URL, avoiding the `BitmapImage.UriSource` HTTP race/freeze.
  - `SourceUrl` (writable string) — bind to the image URL/path; triggers the async load.
  - `ThumbMaxSize` (writable double, default `120`) — the longest edge of the thumbnail in pixels; preserves aspect ratio.
  - `ThumbWidth` / `ThumbHeight` (read-only doubles) — populated after load (or the placeholder square while loading); bind an `Image`'s `Width`/`Height` to these via `RelativeSource Self` for fixed-aspect thumbnails.
  - `ImagePixelWidth` / `ImagePixelHeight` (read-only ints) — the loaded image's pixel dimensions, available after a successful load.
  - The VS XAML designer may report `ThumbWidth`/`ThumbHeight` as "not attachable" inside a `RelativeSource Self` binding — this is a false positive; the runtime resolves them correctly. Do not restructure working XAML to silence it.
- **`BubbleScroll`** (`behaviors:BubbleScroll.Enabled="True"`) — set on a nested `DataGrid`/`ListBox` inside an outer `ScrollViewer` so mouse-wheel input over the child scrolls the outer viewer instead of being swallowed by the child's own (non-scrolling) internal `ScrollViewer`. Use this whenever a content-sized child grid/list sits inside a parent `ScrollViewer`.

## Localization

- Always declare the `i18n` namespace at the top of every XAML file that needs localized strings: `xmlns:i18n="clr-namespace:TD.i18n;assembly=OzzTradeDigest.i18n"`
- **Menu item `Header`** values must use `{x:Static i18n:ActionStrings.Key}` or `{x:Static i18n:LocalizedStrings.Key}` — never hardcode visible menu text unless no matching resource key exists
- **Property label `TextBlock.Text`** must bind to `{x:Static i18n:LocalizedStrings.PropertyName}` — never use hardcoded strings for property labels
- **Button `ToolTip`** values in maintenance toolbars must bind to a localized string from `ActionStrings` or `LocalizedStrings`
- Use `ActionStrings` for verbs and general actions (e.g., `Import`, `Export`, `Save`, `Edit`, `Delete`, `ManageData`)
- Use `LocalizedStrings` for entity names and entity-scoped actions (e.g., `EditTrade`, `RemoveTrade`, `EditExchange`)

## Naming

- View files: `{EntityName}View.xaml` or `{Feature}Window.xaml`
- Style keys: `{Description}Style` or `{Description}Style-{size}` (e.g., `IconButtonStyle-28x24`)
- DataContext: set to corresponding ViewModel class from `TD.WPF.ViewModels`

## Layout

- Use `Grid` with explicit row/column definitions for structured layouts
- Use `StackPanel` only for simple linear arrangements
- Icon buttons use `Path` with `Data="{StaticResource icon-key}"` inside a `Viewbox`

## Maintenance Toolbar Buttons

- For maintenance tab action bars (Add, Save, Refresh), use a horizontal `StackPanel` docked to top with `Margin="0,0,0,8"`
- Use `Button` style `IconButtonStyle-28x24`
- Set localized `ToolTip` values from `TD.i18n` resource classes (for example `LocalizedStrings.*` and `ActionStrings.*`)
- Render icons as:
  - `Viewbox Width="14" Height="14"`
  - child `Path Fill="#0044D7" Data="{StaticResource <icon-key>}"`
- Use icon keys consistent with `MaintenanceWindow.xaml` actions:
  - Add: `plus-circle`
  - Save: `floppy-fill`
  - Refresh: `arrow-clockwise`
- Keep action button click handlers named by feature + action (for example `AddCurrency_Click`, `SaveSymbols_Click`, `RefreshExchanges_Click`)

## Maintenance Delete Button Standard

- Destructive actions (for example Delete in maintenance toolbars) must use a command binding (`Command="{Binding <DeleteCommand>}"`) instead of click handlers.
- Command `CanExecute` must control enabled/disabled state based on selection and domain safety checks.
- Destructive command execution must ask for user confirmation with a `MessageBox` before performing deletion (for example text: `Are you sure to delete?`, with `Yes/No` options).
- Use `IconButtonStyle-28x24` with red destructive background (`Background="#C80000"`).
- Use a localized delete tooltip from `LocalizedStrings` (for example `LocalizedStrings.DeleteExchange`).
- Render the delete icon with `trash3` inside `Viewbox Width="14" Height="14"`.
- Use hover inversion for icon color via `Path.Style` trigger:
  - normal: `Fill="#F8F8F8"`
  - hover: `Fill="#C80000"`.
- Keep this visual/behavior pattern consistent across all delete buttons in the WPF app.
