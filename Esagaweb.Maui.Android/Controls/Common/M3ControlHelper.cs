namespace Esagaweb.Maui.Android.Controls.Common;

/// <summary>
/// Utilidades compartidas de controles M3 (carpeta Common).
/// Resuelve colores Light/Dark de Themes/M3Colors.xaml por clave.
/// </summary>
public static class M3ControlHelper
{
    /// <summary>Resuelve un Color de Themes/M3Colors.xaml por clave (Transparent si falta).</summary>
    public static Color Res(string key)
    {
        if (Application.Current?.Resources.TryGetValue(key, out var v) == true && v is Color c)
            return c;
        return Colors.Transparent;
    }

    /// <summary>Aplica AppThemeBinding a una propiedad enlazable (para light/dark automático).</summary>
    public static void SetThemed(Element target, BindableProperty property, string lightKey, string darkKey)
    {
        target.SetAppThemeColor(property, Res(lightKey), Res(darkKey));
    }

    /// <summary>Resuelve un double de recursos por clave (fallback si falta).</summary>
    public static double ResDouble(string key, double fallback)
    {
        if (Application.Current?.Resources.TryGetValue(key, out var v) is true && v is double d)
            return d;
        return fallback;
    }
}
