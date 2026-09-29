using Microsoft.Maui.Devices;

namespace Esagaweb.Maui.Android.Helpers;

/// <summary>Responsive breakpoints for phone, foldable, and tablet widths.</summary>
public enum M3Breakpoint
{
    /// <summary>Phone width below 600 density pixels.</summary>
    Compact,
    /// <summary>Foldable or small tablet width from 600 to 840 density pixels.</summary>
    Medium,
    /// <summary>Tablet width above 840 density pixels.</summary>
    Expanded
}

/// <summary>Responsive helpers for breakpoints, spacing, and column counts.</summary>
/// <example>
/// <code language="csharp">
/// var breakpoint = ResponsiveHelper.GetBreakpoint(Width);
/// Root.Spacing = ResponsiveHelper.SpacingFor(breakpoint);
/// int columns = ResponsiveHelper.ColumnsFor(breakpoint);
/// </code>
/// </example>
public static class ResponsiveHelper
{
    /// <summary>Returns the breakpoint for a width in density pixels.</summary>
    /// <param name="widthDp">The available width in density pixels.</param>
    /// <returns>The matching breakpoint.</returns>
    public static M3Breakpoint GetBreakpoint(double widthDp)
        => widthDp < 600 ? M3Breakpoint.Compact
         : widthDp <= 840 ? M3Breakpoint.Medium
         : M3Breakpoint.Expanded;

    /// <summary>Gets a value indicating whether the device is a tablet.</summary>
    public static bool IsTablet => DeviceInfo.Idiom == DeviceIdiom.Tablet;

    /// <summary>Returns the suggested spacing for a breakpoint: 16, 24, or 32.</summary>
    /// <param name="bp">The breakpoint.</param>
    /// <returns>The spacing in density pixels.</returns>
    public static double SpacingFor(M3Breakpoint bp) => bp switch
    {
        M3Breakpoint.Compact => 16,
        M3Breakpoint.Medium => 24,
        _ => 32,
    };

    /// <summary>Returns the suggested column count for a breakpoint: 1, 2, or 3.</summary>
    /// <param name="bp">The breakpoint.</param>
    /// <returns>The number of columns.</returns>
    public static int ColumnsFor(M3Breakpoint bp) => bp switch
    {
        M3Breakpoint.Compact => 1,
        M3Breakpoint.Medium => 2,
        _ => 3,
    };
}
