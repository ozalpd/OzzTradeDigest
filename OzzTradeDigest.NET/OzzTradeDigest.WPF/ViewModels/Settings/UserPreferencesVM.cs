using TD.AppInfra.ViewModels;
using TD.WPF.Models;

namespace TD.WPF.ViewModels.Settings;

public partial class UserPreferencesVM : AbstractCreateEditVM
{
    private readonly AppSettings _settings;

    public UserPreferencesVM() : this(AppSettings.GetAppSettings()) { }

    public UserPreferencesVM(AppSettings settings)
    {
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));

        // Initialize VM properties from current AppSettings state
        _uiCulture = _settings.UiCulture;
        _imagesFolder = _settings.ImagesFolder;
        _defaultRiskAmount = _settings.DefaultRiskAmount;

        AvailableCultures = new List<CultureOption>
        {
            new CultureOption { CultureCode = "", DisplayName = "System Default" },
            new CultureOption { CultureCode = "en-US", DisplayName = "English (en-US)" },
            new CultureOption { CultureCode = "tr-TR", DisplayName = "Türkçe (tr-TR)" }
        };

        OnInitialized();
    }

    partial void OnInitialized();

    public List<CultureOption> AvailableCultures { get; }

    public string UiCulture
    {
        get => _uiCulture;
        set
        {
            if (_uiCulture != value)
            {
                _uiCulture = value;
                RaisePropertyChanged(nameof(UiCulture));
                ValidateProperty(_settings, nameof(_settings.UiCulture));
            }
        }
    }
    private string _uiCulture;

    public string ImagesFolder
    {
        get => _imagesFolder;
        set
        {
            if (_imagesFolder != value)
            {
                _imagesFolder = value;
                RaisePropertyChanged(nameof(ImagesFolder));
                ValidateProperty(_settings, nameof(_settings.ImagesFolder));
            }
        }
    }
    private string _imagesFolder;

    public decimal DefaultRiskAmount
    {
        get => _defaultRiskAmount;
        set
        {
            if (_defaultRiskAmount != value)
            {
                _defaultRiskAmount = value >= 0 ? value : 0m;
                RaisePropertyChanged(nameof(DefaultRiskAmount));
                ValidateProperty(_settings, nameof(_settings.DefaultRiskAmount));
            }
        }
    }
    private decimal _defaultRiskAmount;

    /// <summary>
    /// Saves modified properties back to AppSettings singleton and serializes to disk.
    /// </summary>
    public bool SaveSettings()
    {
        bool cultureChanged = _settings.UiCulture != UiCulture;

        _settings.UiCulture = UiCulture;
        _settings.ImagesFolder = ImagesFolder;
        _settings.DefaultRiskAmount = DefaultRiskAmount;
        _settings.Save();

        return cultureChanged;
    }
}

public class CultureOption
{
    public string CultureCode { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
}