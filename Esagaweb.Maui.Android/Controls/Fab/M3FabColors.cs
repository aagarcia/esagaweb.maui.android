namespace Esagaweb.Maui.Android.Controls.Fab;

/// <summary>
/// Mapeo puro variante → claves de color de Themes/M3Colors.xaml (sin UI, testeable).
/// Cada variante devuelve (fondoLight, fondoDark, iconoLight, iconoDark).
/// </summary>
public static class M3FabColors
{
    /// <summary>Resuelve las 4 claves de recurso para la variante indicada.</summary>
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
