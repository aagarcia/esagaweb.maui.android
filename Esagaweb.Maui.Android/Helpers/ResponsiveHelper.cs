using Microsoft.Maui.Devices;

namespace Esagaweb.Maui.Android.Helpers;

/// <summary>
/// Breakpoints M3 adaptados a MAUI Android.
/// Compact &lt;600dp (teléfono), Medium 600-840 (plegable/tablet chica), Expanded &gt;840 (tablet).
/// Se usa con VisualStateManager + SizeChanged de página.
/// </summary>
public enum M3Breakpoint
{
    /// <summary>Teléfono (&lt;600dp).</summary>
    Compact,
    /// <summary>Plegable/tablet chica (600-840dp).</summary>
    Medium,
    /// <summary>Tablet (&gt;840dp).</summary>
    Expanded
}

/// <summary>Helpers de responsive M3 (breakpoints, espaciados, columnas).</summary>
public static class ResponsiveHelper
{
    /// <summary>Breakpoint para un ancho en dp.</summary>
    public static M3Breakpoint GetBreakpoint(double widthDp)
        => widthDp < 600 ? M3Breakpoint.Compact
         : widthDp <= 840 ? M3Breakpoint.Medium
         : M3Breakpoint.Expanded;

    /// <summary>True si el dispositivo es tablet.</summary>
    public static bool IsTablet => DeviceInfo.Idiom == DeviceIdiom.Tablet;

    /// <summary>Espaciado sugerido por breakpoint (16/24/32).</summary>
    public static double SpacingFor(M3Breakpoint bp) => bp switch
    {
        M3Breakpoint.Compact => 16,
        M3Breakpoint.Medium => 24,
        _ => 32,
    };

    /// <summary>Columnas sugeridas por breakpoint (1/2/3).</summary>
    public static int ColumnsFor(M3Breakpoint bp) => bp switch
    {
        M3Breakpoint.Compact => 1,
        M3Breakpoint.Medium => 2,
        _ => 3,
    };
}
