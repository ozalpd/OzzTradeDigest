# OzzTradeDigest — AI Assistant Guidelines

Welcome to OzzTradeDigest. When working on this repository, always align with the project guidelines, architecture, and domain rules established in `.github/`.

## Authoritative Documentation

1. **Architecture & Coding Conventions**: Read [.github/copilot-instructions.md](.github/copilot-instructions.md)
   - Mandatory layered MVVM pattern (`TD`, `TD.AppInfra`, `TD.RepositoryContracts`, `TD.SQLite`, `TD.WPF`).
   - Raw SQLite data access with `Microsoft.Data.Sqlite` (no ORM).
   - Entity-first naming conventions (`TradingAccountCreateVM`, `ExchangeDeleteCommand`).
   - Localization rules (`TD.i18n`).
   - Changelog discipline: Record any behavior change in `CHANGELOG.md`.

2. **UI & XAML Standards**: Read [.github/instructions/xaml-conventions.instructions.md](.github/instructions/xaml-conventions.instructions.md)
   - Consult before creating or editing any XAML views, styles, icon buttons, or attached behaviors (`AsyncImageLoader`, `BubbleScroll`).
   - Follow localization markup conventions (`{x:Static i18n:LocalizedStrings.*}`, `{x:Static i18n:ActionStrings.*}`).

3. **Database Scripts & Code Generation**: Read [.github/instructions/dbscripts-generated.instructions.md](.github/instructions/dbscripts-generated.instructions.md)
   - Scripts in `OzzTradeDigest.SQLite/DbScripts/` are code-generated via `OzzCodeGen`. Do not modify manually.
   - Schema changes must be made via `OzzCodeGen/SqliteScriptsGen.settings`.

4. **Domain & Financial Logic**: Read [.github/agents/trade-expert.agent.md](.github/agents/trade-expert.agent.md)
   - Details trade lifecycles, order types, risk/reward calculations, R-multiples, and fee math (`Trade.calc.cs`).

5. **Entity Scaffolding**: Follow [.github/prompts/scaffold-entity.prompt.md](.github/prompts/scaffold-entity.prompt.md)
   - Step-by-step workflow when adding new domain entities across models, repository contracts, SQLite repositories, and view models.

6. **Roadmap & Priorities**: Check [.github/development-roadmap.md](.github/development-roadmap.md)
   - Prioritized task cycles (from Cycle 1 blocking core workflow gaps through Polish).

