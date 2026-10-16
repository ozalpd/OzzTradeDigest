using TD.i18n;

namespace TD.WPF.Helpers;

public static class FileHelper
{
    public static string GetOpenImgFilter()
    {
        return $"{CommonStrings.AllSupportedGraphics}|*.jpg;*.jpeg;*.png;|" +
               "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
               $"{CommonStrings.PortableNetworkGraphic} (*.png)|*.png";
    }
}