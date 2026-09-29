using Esagaweb.Maui.Android.Themes;

namespace Esagaweb.Maui.Android;

/// <summary>Merges the six Material theme dictionaries into the consuming app.</summary>
/// <remarks>Call <see cref="Apply"/> once in the app constructor, after bootstrapping the resources.</remarks>
public static class EsagawebThemes
{
    /// <summary>Merges the six Material dictionaries into the current application resources, when available.</summary>
    /// <example>
    /// <code language="csharp">
    /// public App()
    /// {
    ///     InitializeComponent();
    ///     EsagawebThemes.Apply();
    /// }
    /// </code>
    /// </example>
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
