using Esagaweb.Maui.Android.Themes;

namespace Esagaweb.Maui.Android;

/// <summary>
/// Fusiona los 6 diccionarios M3 en la app consumidora.
/// Llamar una vez en el ctor de App (después de InitializeComponent):
/// <c>EsagawebThemes.Apply();</c>
/// </summary>
public static class EsagawebThemes
{
    /// <summary>Fusiona los 6 diccionarios M3 en <see cref="Application.Current"/> (si existe).</summary>
    public static void Apply()
    {
        var merged = Application.Current?.Resources.MergedDictionaries;
        if (merged is null)
            return;

        merged.Add(new M3Colors());
        merged.Add(new M3Typography());
        merged.Add(new M3Buttons());
        merged.Add(new M3Surfaces());
        merged.Add(new M3Inputs());
        merged.Add(new M3Overlays());
    }
}
