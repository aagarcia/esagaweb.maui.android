using System.Windows.Input;
using Esagaweb.Maui.Android.Controls.Common;

namespace Esagaweb.Maui.Android.Controls.Chip;

/// <summary>
/// Represents a compact Material 3 chip with Assist, Filter, and Input variants.
/// </summary>
/// <remarks>
/// Assist raises <see cref="Tapped"/> when tapped. Filter toggles <see cref="Selected"/> when tapped
/// and passes the current value to <see cref="Command"/>. Input shows a close affordance that raises
/// <see cref="Closed"/> when tapped. The leading glyph uses the MaterialSymbols font and is hidden
/// when <see cref="Glyph"/> is empty. A disabled chip renders with reduced opacity and muted colors.
/// Requires <c>UseEsagaweb</c> font registration and the M3 theme resource dictionaries.
/// </remarks>
/// <example>
/// <code language="xaml"><![CDATA[
/// <ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
///              xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
///              xmlns:chip="clr-namespace:Esagaweb.Maui.Android.Controls.Chip;assembly=Esagaweb.Maui.Android"
///              xmlns:common="clr-namespace:Esagaweb.Maui.Android.Controls.Common;assembly=Esagaweb.Maui.Android">
///     <chip:M3Chip Type="Assist"
///                  Text="In stock"
///                  Glyph="{x:Static common:M3Icons.Check}" />
/// </ContentPage>
/// ]]></code>
/// </example>
public partial class M3Chip : ContentView
{
    /// <summary>
    /// Identifies the <see cref="Text"/> bindable property.
    /// </summary>
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(M3Chip), string.Empty,
            propertyChanged: (b, _, n) => ((M3Chip)b).TextLabel.Text = (string?)n ?? string.Empty);

    /// <summary>
    /// Identifies the <see cref="Type"/> bindable property.
    /// </summary>
    public static readonly BindableProperty TypeProperty =
        BindableProperty.Create(nameof(Type), typeof(M3ChipType), typeof(M3Chip), M3ChipType.Assist,
            propertyChanged: (b, _, _) => ((M3Chip)b).UpdateAppearance());

    /// <summary>
    /// Identifies the <see cref="Selected"/> bindable property.
    /// </summary>
    public static readonly BindableProperty SelectedProperty =
        BindableProperty.Create(nameof(Selected), typeof(bool), typeof(M3Chip), false,
            BindingMode.TwoWay, propertyChanged: (b, _, _) => ((M3Chip)b).UpdateAppearance());

    /// <summary>
    /// Identifies the <see cref="Glyph"/> bindable property.
    /// </summary>
    public static readonly BindableProperty GlyphProperty =
        BindableProperty.Create(nameof(Glyph), typeof(string), typeof(M3Chip), string.Empty,
            propertyChanged: (b, _, _) => ((M3Chip)b).UpdateGlyph());

    /// <summary>
    /// Identifies the <see cref="Command"/> bindable property.
    /// </summary>
    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(M3Chip), null);

    /// <summary>
    /// Gets or sets the chip text. The default is <see cref="string.Empty"/>.
    /// </summary>
    /// <remarks>This is a bindable property.</remarks>
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    /// <summary>
    /// Gets or sets the chip behavior variant. The default is <see cref="M3ChipType.Assist"/>.
    /// </summary>
    /// <remarks>This is a bindable property.</remarks>
    public M3ChipType Type
    {
        get => (M3ChipType)GetValue(TypeProperty);
        set => SetValue(TypeProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the chip is selected. This is relevant to the Filter variant. The default is <c>false</c>.
    /// </summary>
    /// <remarks>This is a bindable property.</remarks>
    public bool Selected
    {
        get => (bool)GetValue(SelectedProperty);
        set => SetValue(SelectedProperty, value);
    }

    /// <summary>
    /// Gets or sets the optional Material Symbols leading glyph. The default is <see cref="string.Empty"/>, which hides the glyph.
    /// </summary>
    /// <remarks>This is a bindable property.</remarks>
    public string Glyph
    {
        get => (string)GetValue(GlyphProperty);
        set => SetValue(GlyphProperty, value);
    }

    /// <summary>
    /// Gets or sets the command invoked when the chip is tapped. It receives the current <see cref="Selected"/> value. The default is <c>null</c>.
    /// </summary>
    /// <remarks>This is a bindable property.</remarks>
    public ICommand? Command
    {
        get => (ICommand?)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    /// <summary>
    /// Occurs when the chip is tapped.
    /// </summary>
    public event EventHandler? Tapped;
    /// <summary>
    /// Occurs when the close affordance is tapped. Only the Input variant shows it.
    /// </summary>
    public event EventHandler? Closed;

    /// <summary>
    /// Initializes a new instance of the <see cref="M3Chip"/> class.
    /// </summary>
    public M3Chip()
    {
        InitializeComponent();
        GlyphLabel.FontFamily = "MaterialSymbols";
        CloseLabel.FontFamily = "MaterialSymbols";
        CloseLabel.Text = M3Icons.Close;
        TextLabel.FontFamily = "NunitoRegular";
        TextLabel.Text = Text;
        UpdateGlyph();
        UpdateAppearance();
    }

    private void OnTapped(object? sender, TappedEventArgs e)
    {
        if (!IsEnabled) return;
        if (Type == M3ChipType.Filter)
            Selected = !Selected;
        if (Command?.CanExecute(Selected) == true)
            Command.Execute(Selected);
        Tapped?.Invoke(this, EventArgs.Empty);
    }

    private void OnCloseTapped(object? sender, TappedEventArgs e)
    {
        if (!IsEnabled) return;
        Closed?.Invoke(this, EventArgs.Empty);
    }

    private void UpdateGlyph()
    {
        GlyphLabel.FontFamily = "MaterialSymbols";
        GlyphLabel.Text = Glyph;
        GlyphLabel.IsVisible = !string.IsNullOrEmpty(Glyph);
    }

    private void UpdateAppearance()
    {
        CloseLabel.IsVisible = Type == M3ChipType.Input;
        var on = Selected && Type == M3ChipType.Filter;
        if (!IsEnabled)
        {
            RootBorder.Opacity = 0.6;
            M3ControlHelper.SetThemed(RootBorder, Border.BackgroundColorProperty, "M3SurfaceVariant", "M3DarkSurfaceVariant");
            M3ControlHelper.SetThemed(RootBorder, Border.StrokeProperty, "M3Outline", "M3DarkOutline");
            M3ControlHelper.SetThemed(TextLabel, Label.TextColorProperty, "M3OnSurfaceVariant", "M3DarkOnSurface");
            return;
        }
        RootBorder.Opacity = 1;
        if (on)
        {
            M3ControlHelper.SetThemed(RootBorder, Border.BackgroundColorProperty, "M3SecondaryContainer", "M3DarkPrimaryContainer");
            RootBorder.Stroke = Colors.Transparent;
            M3ControlHelper.SetThemed(TextLabel, Label.TextColorProperty, "M3OnPrimaryContainer", "M3DarkPrimary");
            M3ControlHelper.SetThemed(GlyphLabel, Label.TextColorProperty, "M3OnPrimaryContainer", "M3DarkPrimary");
        }
        else
        {
            RootBorder.BackgroundColor = Colors.Transparent;
            M3ControlHelper.SetThemed(RootBorder, Border.StrokeProperty, "M3Outline", "M3DarkOutline");
            M3ControlHelper.SetThemed(TextLabel, Label.TextColorProperty, "M3OnSurfaceVariant", "M3DarkOnSurface");
            M3ControlHelper.SetThemed(GlyphLabel, Label.TextColorProperty, "M3Primary", "M3DarkPrimary");
        }
        M3ControlHelper.SetThemed(CloseLabel, Label.TextColorProperty, "M3OnSurfaceVariant", "M3DarkOnSurface");
    }
}
