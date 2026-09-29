namespace Esagaweb.Maui.Android.Controls.Fab;

/// <summary>Provides the theme resource keys for each floating action button color variant.</summary>
/// <remarks>Contains no user interface logic. The keys resolve against the shared theme dictionaries for light and dark modes.</remarks>
public static class M3FabColors
{
    /// <summary>Resolves the background and icon theme resource keys for the specified variant.</summary>
    /// <param name="variant">The color variant to resolve.</param>
    /// <returns>The light and dark resource keys for the background and the icon.</returns>
    public static (string BgLight, string BgDark, string IconLight, string IconDark) Resolve(M3FabVariant variant) =>
        variant switch
        {
            M3FabVariant.Surface => ("M3SurfaceVariant", "M3DarkSurfaceVariant", "M3Primary", "M3DarkPrimary"),
            M3FabVariant.Secondary => ("M3SecondaryContainer", "M3DarkSecondaryContainer",
                "M3OnSecondaryContainer", "M3DarkOnSecondaryContainer"),
            M3FabVariant.Tertiary => ("M3TertiaryContainer", "M3DarkTertiaryContainer",
                "M3OnTertiaryContainer", "M3DarkOnTertiaryContainer"),
            _ => ("M3PrimaryContainer", "M3DarkPrimaryContainer", "M3OnPrimaryContainer", "M3DarkPrimary"),
        };
}
