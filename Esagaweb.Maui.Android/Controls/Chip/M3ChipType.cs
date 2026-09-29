namespace Esagaweb.Maui.Android.Controls.Chip;

/// <summary>
/// Defines the behavior variants of an <see cref="M3Chip"/>.
/// </summary>
public enum M3ChipType
{
    /// <summary>
    /// Performs a single action when tapped. This is the default value.
    /// </summary>
    Assist,
    /// <summary>
    /// Toggles the selected state when tapped.
    /// </summary>
    Filter,
    /// <summary>
    /// Shows a close affordance that raises a closed notification when tapped.
    /// </summary>
    Input
}
