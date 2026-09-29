using System.Windows.Input;

namespace Esagaweb.Maui.Android.Controls.NavigationBar;

/// <summary>
/// A single destination of the navigation bar, with an icon, a label, and an optional command.
/// </summary>
/// <example>
/// <code lang="XAML">
/// <![CDATA[
/// <ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
///              xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
///              xmlns:nav="clr-namespace:Esagaweb.Maui.Android.Controls.NavigationBar;assembly=Esagaweb.Maui.Android"
///              xmlns:common="clr-namespace:Esagaweb.Maui.Android.Controls.Common;assembly=Esagaweb.Maui.Android">
///     <nav:M3NavigationBar SelectedIndex="0">
///         <nav:M3NavigationItem Label="Home"
///                               Glyph="{x:Static common:M3Icons.Home}"
///                               SelectedGlyph="{x:Static common:M3Icons.HomeFilled}" />
///         <nav:M3NavigationItem Label="Search"
///                               Glyph="{x:Static common:M3Icons.Search}" />
///     </nav:M3NavigationBar>
/// </ContentPage>
/// ]]>
/// </code>
/// </example>
public class M3NavigationItem
{
    /// <summary>
    /// Gets or sets the label shown below the icon. The default is an empty string.
    /// </summary>
    public string Label { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the inactive Material Symbols glyph. The default is an empty string.
    /// </summary>
    public string Glyph { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the active glyph. When empty, the value of <see cref="Glyph"/> is used. The default is an empty string.
    /// </summary>
    public string SelectedGlyph { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the command run when this destination is selected, without parameters. The default is null.
    /// </summary>
    public ICommand? Command { get; set; }
}
