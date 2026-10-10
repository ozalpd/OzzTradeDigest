using System.ComponentModel.DataAnnotations;
using TD.i18n;

namespace TD.Models
{
    public enum DatePeriod
    {
        [Display(ResourceType = typeof(LocalizedStrings), Name = "AllDates", Order = 0)]
        AllDates = 0,
        [Display(ResourceType = typeof(LocalizedStrings), Name = "ThisWeek", Order = 10)]
        ThisWeek = 10,
        [Display(ResourceType = typeof(LocalizedStrings), Name = "PreviousWeek", Order = 20)]
        PreviousWeek = 20,
        [Display(ResourceType = typeof(LocalizedStrings), Name = "ThisMonth", Order = 30)]
        ThisMonth = 30,
        [Display(ResourceType = typeof(LocalizedStrings), Name = "PreviousMonth", Order = 40)]
        PreviousMonth = 40,
        [Display(ResourceType = typeof(LocalizedStrings), Name = "ThisQuarter", Order = 50)]
        ThisQuarter = 50,
        [Display(ResourceType = typeof(LocalizedStrings), Name = "PreviousQuarter", Order = 60)]
        PreviousQuarter = 60,
        [Display(ResourceType = typeof(LocalizedStrings), Name = "ThisHalfYear", Order = 70)]
        ThisHalfYear = 70,
        [Display(ResourceType = typeof(LocalizedStrings), Name = "PreviousHalfYear", Order = 80)]
        PreviousHalfYear = 80,
        [Display(ResourceType = typeof(LocalizedStrings), Name = "ThisYear", Order = 90)]
        ThisYear = 90,
        [Display(ResourceType = typeof(LocalizedStrings), Name = "PreviousYear", Order = 100)]
        PreviousYear = 100
    }

    public enum EntryOrderType : int
    {
        [Display(ResourceType = typeof(LocalizedStrings), Name = "Market")]
        Market = 10,
        [Display(ResourceType = typeof(LocalizedStrings), Name = "Limit")]
        Limit = 20,
        [Display(ResourceType = typeof(LocalizedStrings), Name = "StopMarket")]
        StopMarket = 40,   // stop-market entry
        [Display(ResourceType = typeof(LocalizedStrings), Name = "StopLimit")]
        StopLimit = 50
    }

    public enum ExitOrderType : int
    {
        [Display(ResourceType = typeof(LocalizedStrings), Name = "Market")]
        Market = 10,
        [Display(ResourceType = typeof(LocalizedStrings), Name = "Limit")]
        Limit = 20,
        [Display(ResourceType = typeof(LocalizedStrings), Name = "TrailingStop")]
        TrailingStop = 30,
        [Display(ResourceType = typeof(LocalizedStrings), Name = "Stop")]
        Stop = 40,
        [Display(ResourceType = typeof(LocalizedStrings), Name = "StopLimit")]
        StopLimit = 50,
    }

    public enum ExitOrderMode : int
    {
        [Display(ResourceType = typeof(LocalizedStrings), Name = "PlannedTP")]
        PlannedTP = 10,
        [Display(ResourceType = typeof(LocalizedStrings), Name = "PlannedSL")]
        PlannedSL = 20,
        [Display(ResourceType = typeof(LocalizedStrings), Name = "ManualExit")]
        ManualExit = 30,
        [Display(ResourceType = typeof(LocalizedStrings), Name = "TimedExit")]
        TimedExit = 40,
        [Display(ResourceType = typeof(LocalizedStrings), Name = "Algorithm")]
        Algorithm = 50,
        [Display(ResourceType = typeof(LocalizedStrings), Name = "MarginCall")]
        MarginCall = 60
    }

    public enum TradeAdherence : int
    {
        [Display(ResourceType = typeof(LocalizedStrings), Name = "Excellent", Order = 10)]
        /// <summary>Flawless execution, followed the trade plan precisely</summary>
        Excellent = 100,
        [Display(ResourceType = typeof(LocalizedStrings), Name = "Good", Order = 20)]
        /// <summary>Minor deviations from the plan, but overall good execution</summary>
        Good = 75,
        [Display(ResourceType = typeof(LocalizedStrings), Name = "Moderate", Order = 30)]
        /// <summary>Moderate deviation (e.g., exited too early or bent rules noticeably)</summary>
        Moderate = 50,
        [Display(ResourceType = typeof(LocalizedStrings), Name = "Poor", Order = 40)]
        /// <summary>Severe rule violation, off-plan, or reactive/emotional trading</summary>
        Poor = 25,
        [Display(ResourceType = typeof(LocalizedStrings), Name = "VeryPoor", Order = 50)]
        /// <summary>Catastrophic execution, complete disregard for the plan</summary>
        VeryPoor = 0,
    }

    public enum TradeNoteCategory : int
    {
        [Display(ResourceType = typeof(LocalizedStrings), Name = "Setup", Order = 10)]
        ///<summary>Pre-trade thesis, market structure, entry conditions, trade plan</summary>
        Setup = 10,
        [Display(ResourceType = typeof(LocalizedStrings), Name = "EntryNote", Order = 20)]
        ///<summary>Execution context, entry price notes, slippage, initial reaction</summary>
        EntryNote = 20,
        [Display(ResourceType = typeof(LocalizedStrings), Name = "Management", Order = 30)]
        ///<summary>Active trade adjustments (scaling in/out, moving SL to BE, trailing)</summary>
        Management = 30,
        [Display(ResourceType = typeof(LocalizedStrings), Name = "ExitNote", Order = 40)]
        ///<summary>Exit rationale, target hit, stop-out notes, market condition changes</summary>
        ExitNote = 40,
        [Display(ResourceType = typeof(LocalizedStrings), Name = "Review", Order = 50)]
        ///<summary>Post-trade autopsy, emotional check, rule adherence, key lessons</summary>
        Review = 50,
        [Display(ResourceType = typeof(LocalizedStrings), Name = "Other", Order = 1010)]
        ///<summary>Uncategorized or general market observations</summary>
        Other = 1010
    }

    public enum MarketType : int
    {
        [Display(ResourceType = typeof(LocalizedStrings), Name = "Unspecified")]
        Unspecified = 0,
        [Display(ResourceType = typeof(LocalizedStrings), Name = "Stock")]
        Stock = 20,
        [Display(ResourceType = typeof(LocalizedStrings), Name = "Fund")]
        Fund = 30,
        [Display(ResourceType = typeof(LocalizedStrings), Name = "Futures")]
        Futures = 40,
        [Display(ResourceType = typeof(LocalizedStrings), Name = "Forex")]
        Forex = 50,
        [Display(ResourceType = typeof(LocalizedStrings), Name = "Option")]
        Option = 60,
        [Display(ResourceType = typeof(LocalizedStrings), Name = "Commodity")]
        Commodity = 70,
        [Display(ResourceType = typeof(LocalizedStrings), Name = "CryptoSpot")]
        CryptoSpot = 80,
        [Display(ResourceType = typeof(LocalizedStrings), Name = "CryptoPerpetual")]
        CryptoPerpetual = 90,
        [Display(ResourceType = typeof(LocalizedStrings), Name = "Index")]
        Index = 100
    }

    /// <summary>
    /// Specifies which side of the entry price a planned price level must be on for a Long position.
    /// The rule is automatically inverted for Short positions.
    /// </summary>
    public enum PriceSide
    {
        /// <summary>Price must be above the entry price (e.g. Take Profit for Long).</summary>
        Above,
        /// <summary>Price must be below the entry price (e.g. Stop Loss for Long).</summary>
        Below
    }

    public enum ReportType
    {
        Balance = 10,
        Performance = 20,
        NetResult = 30,
        TradeHistory = 40
    }

    public enum SessionType : int
    {
        [Display(ResourceType = typeof(LocalizedStrings), Name = "Break")]
        Break = 0,
        [Display(ResourceType = typeof(LocalizedStrings), Name = "Regular")]
        Regular = 1,
        [Display(ResourceType = typeof(LocalizedStrings), Name = "PreMarket")]
        PreMarket = 2,
        [Display(ResourceType = typeof(LocalizedStrings), Name = "AfterHours")]
        AfterHours = 3,
        [Display(ResourceType = typeof(LocalizedStrings), Name = "SundayOpen")]
        SundayOpen = 5,
        [Display(ResourceType = typeof(LocalizedStrings), Name = "OpeningAuction")]
        OpeningAuction = 6,
        [Display(ResourceType = typeof(LocalizedStrings), Name = "ClosingAuction")]
        ClosingAuction = 7,

        // CME-specific
        [Display(ResourceType = typeof(LocalizedStrings), Name = "RegularTradingHours")]
        RegularTradingHours = 8,      // RTH
        [Display(ResourceType = typeof(LocalizedStrings), Name = "ElectronicTradingHours")]
        ElectronicTradingHours = 9    // ETH / Globex
    }

    public enum SettingType : int
    {
        String = 1010,
        Email = 1020,
        StringArray = 1101,
        Boolean = 2000,
        Integer = 2010,
        IntegerArray = 2011,
        Decimal = 2021,
        DecimalArray = 2022,
        Date = 3010,
        DateArray = 3011,
        DateTime = 3020,
        DateTimeArray = 3021
    }

    public enum TradeDateType : int
    {
        [Display(ResourceType = typeof(LocalizedStrings), Name = "EntryTime", Order = 10)]
        EntryTime = 10,
        [Display(ResourceType = typeof(LocalizedStrings), Name = "ExitTime", Order = 20)]
        ExitTime = 20,
        [Display(ResourceType = typeof(LocalizedStrings), Name = "CancellationTime", Order = 30)]
        CancellationTime = 30,
        [Display(ResourceType = typeof(LocalizedStrings), Name = "UpdateTime", Order = 40)]
        UpdateTime = 40
    }

    public enum TradeDirection : int
    {
        [Display(ResourceType = typeof(LocalizedStrings), Name = "Long")]
        Long = 200,
        [Display(ResourceType = typeof(LocalizedStrings), Name = "Short")]
        Short = 100
    }

    public enum TradeStatus
    {
        [Display(ResourceType = typeof(LocalizedStrings), Name = "Planned", Order = 10)]
        Planned = 10,
        [Display(ResourceType = typeof(LocalizedStrings), Name = "Pending", Order = 20)]
        Pending = 20,
        [Display(ResourceType = typeof(LocalizedStrings), Name = "Active", Order = 30)]
        Active = 30,
        [Display(ResourceType = typeof(LocalizedStrings), Name = "Closed", Order = 40)]
        Closed = 40,
        [Display(ResourceType = typeof(LocalizedStrings), Name = "Missed", Order = 50)]
        Missed = -10,
        [Display(ResourceType = typeof(LocalizedStrings), Name = "Cancelled", Order = 60)]
        Cancelled = -20
    }

    public enum TradeStatusQuery
    {
        [Display(ResourceType = typeof(LocalizedStrings), Name = "All", Order = 0)]
        All = 0,
        [Display(ResourceType = typeof(LocalizedStrings), Name = "ActiveOrWaiting", Order = 100)]
        ActiveOrWaiting = 1010,
        [Display(ResourceType = typeof(LocalizedStrings), Name = "Waiting", Order = 300)]
        Waiting = 1020,
        [Display(ResourceType = typeof(LocalizedStrings), Name = "MissedOrCancelled", Order = 700)]
        MissedOrCancelled = 1030,

        [Display(ResourceType = typeof(LocalizedStrings), Name = "Planned", Order = 400)]
        Planned = 10,
        [Display(ResourceType = typeof(LocalizedStrings), Name = "Pending", Order = 500)]
        Pending = 20,
        [Display(ResourceType = typeof(LocalizedStrings), Name = "Active", Order = 200)]
        Active = 30,
        [Display(ResourceType = typeof(LocalizedStrings), Name = "Closed", Order = 600)]
        Closed = 40,
        [Display(ResourceType = typeof(LocalizedStrings), Name = "Missed", Order = 800)]
        Missed = -10,
        [Display(ResourceType = typeof(LocalizedStrings), Name = "Cancelled", Order = 900)]
        Cancelled = -20
    }
}