namespace Esagaweb.Maui.Android.Controls.Common;

/// <summary>
/// Resolves shared Material 3 theme resources for controls.
/// </summary>
public static class M3ControlHelper
{
    /// <summary>
    /// Resolves a <see cref="Color"/> from the application resources by key.
    /// </summary>
    /// <param name="key">The resource key to look up in the application resources.</param>
    /// <returns>The resolved color, or <see cref="Colors.Transparent"/> when the key is missing or is not a color.</returns>
    public static Color Res(string key)
    {
        if (Application.Current?.Resources.TryGetValue(key, out var v) == true && v is Color c)
            return c;
        return Colors.Transparent;
    }

    /// <summary>
    /// Applies a light and dark color pair to a bindable property so it follows the system theme automatically.
    /// </summary>
    /// <param name="target">The element that owns the property.</param>
    /// <param name="property">The bindable property to theme.</param>
    /// <param name="lightKey">The resource key of the light theme color.</param>
    /// <param name="darkKey">The resource key of the dark theme color.</param>
    public static void SetThemed(Element target, BindableProperty property, string lightKey, string darkKey)
    {
        target.SetAppThemeColor(property, Res(lightKey), Res(darkKey));
    }

    /// <summary>
    /// Resolves a <see cref="double"/> from the application resources by key.
    /// </summary>
    /// <param name="key">The resource key to look up in the application resources.</param>
    /// <param name="fallback">The value to return when the key is missing or is not a <see cref="double"/>.</param>
    /// <returns>The resolved value, or <paramref name="fallback"/> when the key is missing or is not a <see cref="double"/>.</returns>
    public static double ResDouble(string key, double fallback)
    {
        if (Application.Current?.Resources.TryGetValue(key, out var v) is true && v is double d)
            return d;
        return fallback;
    }
}
