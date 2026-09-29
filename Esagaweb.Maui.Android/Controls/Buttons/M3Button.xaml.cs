using System.Windows.Input;
using Esagaweb.Maui.Android.Controls.Common;
using Esagaweb.Maui.Android.Helpers;

namespace Esagaweb.Maui.Android.Controls.Buttons;

/// <summary>
/// Represents a Material 3 button with five visual variants and optional leading icon content.
/// </summary>
/// <remarks>
/// The appearance is selected with <see cref="Variant"/>. The button uses a MaterialSymbols glyph from
/// <see cref="Glyph"/> when it is not empty, and falls back to the <see cref="Icon"/> image otherwise.
/// Tapping the button runs <see cref="Command"/> when it can execute, then raises <see cref="Clicked"/>.
/// The height is responsive through <see cref="ApplyBreakpoint"/>: compact widths use <c>M3ButtonHeightCompact</c>
/// and larger widths use <c>M3ButtonHeightExpanded</c>. Background, content and border colors follow the current
/// theme, including a reduced-opacity disabled state. The host application must register fonts with
/// <c>UseEsagaweb</c> so the MaterialSymbols and NunitoSemiBold fonts resolve.
/// </remarks>
/// <example>
/// <code language="xaml">
/// <![CDATA[
/// <buttons:M3Button xmlns:buttons="clr-namespace:Esagaweb.Maui.Android.Controls.Buttons;assembly=Esagaweb.Maui.Android"
///                   xmlns:common="clr-namespace:Esagaweb.Maui.Android.Controls.Common;assembly=Esagaweb.Maui.Android"
///                   Text="Add to cart"
///                   Variant="Filled"
///                   Glyph="{x:Static common:M3Icons.ShoppingCart}"
///                   Clicked="OnAddClicked" />
/// ]]>
/// </code>
/// </example>
public partial class M3Button : ContentView
{
    /// <summary>
    /// Identifies the <see cref="Text"/> bindable property.
    /// </summary>
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(M3Button), string.Empty,
            propertyChanged: (b, _, n) => ((M3Button)b).TitleLabel.Text = (string?)n ?? string.Empty);

    /// <summary>
    /// Identifies the <see cref="Icon"/> bindable property.
    /// </summary>
    public static readonly BindableProperty IconProperty =
        BindableProperty.Create(nameof(Icon), typeof(ImageSource), typeof(M3Button), null,
            propertyChanged: (b, _, n) =>
            {
                var self = (M3Button)b;
                self.IconImage.Source = (ImageSource?)n;
                self.UpdateGlyph();
            });

    /// <summary>
    /// Identifies the <see cref="Glyph"/> bindable property.
    /// </summary>
    public static readonly BindableProperty GlyphProperty =
        BindableProperty.Create(nameof(Glyph), typeof(string), typeof(M3Button), string.Empty,
            propertyChanged: (b, _, _) => ((M3Button)b).UpdateGlyph());

    /// <summary>
    /// Identifies the <see cref="Variant"/> bindable property.
    /// </summary>
    public static readonly BindableProperty VariantProperty =
        BindableProperty.Create(nameof(Variant), typeof(M3ButtonVariant), typeof(M3Button),
            M3ButtonVariant.Filled, propertyChanged: (b, _, _) => ((M3Button)b).UpdateAppearance());

    /// <summary>
    /// Identifies the <see cref="Command"/> bindable property.
    /// </summary>
    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(M3Button), null);

    /// <summary>
    /// Identifies the <see cref="CommandParameter"/> bindable property.
    /// </summary>
    public static readonly BindableProperty CommandParameterProperty =
        BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(M3Button), null);

    /// <summary>
    /// Gets or sets the text displayed on the button. The default is an empty string.
    /// </summary>
    /// <remarks>
    /// This is a bindable property.
    /// </remarks>
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    /// <summary>
    /// Gets or sets the image displayed when <see cref="Glyph"/> is empty. The default is <see langword="null"/>.
    /// </summary>
    /// <remarks>
    /// This is a bindable property.
    /// </remarks>
    public ImageSource? Icon
    {
        get => (ImageSource?)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    /// <summary>
    /// Gets or sets the MaterialSymbols glyph displayed on the button. A non-empty value takes precedence over <see cref="Icon"/>. The default is an empty string.
    /// </summary>
    /// <remarks>
    /// This is a bindable property.
    /// </remarks>
    public string Glyph
    {
        get => (string)GetValue(GlyphProperty);
        set => SetValue(GlyphProperty, value);
    }

    /// <summary>
    /// Gets or sets the visual variant of the button. The default is <see cref="M3ButtonVariant.Filled"/>.
    /// </summary>
    /// <remarks>
    /// This is a bindable property.
    /// </remarks>
    public M3ButtonVariant Variant
    {
        get => (M3ButtonVariant)GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
    }

    /// <summary>
    /// Gets or sets the command executed when the button is tapped. It receives <see cref="CommandParameter"/>. The default is <see langword="null"/>.
    /// </summary>
    /// <remarks>
    /// This is a bindable property.
    /// </remarks>
    public ICommand? Command
    {
        get => (ICommand?)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    /// <summary>
    /// Gets or sets the parameter passed to <see cref="Command"/>. The default is <see langword="null"/>.
    /// </summary>
    /// <remarks>
    /// This is a bindable property.
    /// </remarks>
    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    private M3Breakpoint? _lastBp;

    /// <summary>
    /// Initializes a new instance of the <see cref="M3Button"/> class.
    /// </summary>
    public M3Button()
    {
        InitializeComponent();
        // Keep the glyph font fixed so an implicit Label style never replaces it with Nunito.
        GlyphLabel.FontFamily = "MaterialSymbols";
        TitleLabel.FontFamily = "NunitoSemiBold";
        TitleLabel.Text = Text;
        UpdateGlyph();
        UpdateAppearance();
        ApplyBreakpoint(400); // Use the compact size by default for phones.
    }

    /// <summary>
    /// Applies the responsive size for the given width. Compact widths use a 40dp height, and medium and expanded widths use a 48dp height.
    /// </summary>
    /// <param name="widthDp">The available width in density-independent pixels. Values less than or equal to zero are ignored.</param>
    public void ApplyBreakpoint(double widthDp)
    {
        if (widthDp <= 0)
            return;
        var bp = ResponsiveHelper.GetBreakpoint(widthDp);
        if (bp == _lastBp)
            return; // Ignore a repeated breakpoint so reassigning HeightRequest does not feed back into SizeChanged.
        _lastBp = bp;
        RootBorder.HeightRequest = bp == M3Breakpoint.Compact
            ? M3ControlHelper.ResDouble("M3ButtonHeightCompact", 40)
            : M3ControlHelper.ResDouble("M3ButtonHeightExpanded", 48);
        HorizontalOptions = bp == M3Breakpoint.Expanded
            ? LayoutOptions.Center
            : LayoutOptions.Fill;
    }

    /// <inheritdoc/>
    protected override void OnPropertyChanged(string? propertyName = null)
    {
        base.OnPropertyChanged(propertyName);
        if (propertyName == nameof(IsEnabled))
            UpdateAppearance();
    }

    private void UpdateGlyph()
    {
        // Reapply the fonts on every change so later implicit styles keep the expected typefaces.
        GlyphLabel.FontFamily = "MaterialSymbols";
        TitleLabel.FontFamily = "NunitoSemiBold";
        var hasGlyph = !string.IsNullOrEmpty(Glyph);
        GlyphLabel.Text = Glyph;
        GlyphLabel.IsVisible = hasGlyph;
        IconImage.IsVisible = !hasGlyph && IconImage.Source is not null;
    }

    private void SetContentThemed(string lightKey, string darkKey)
    {
        M3ControlHelper.SetThemed(TitleLabel, Label.TextColorProperty, lightKey, darkKey);
        M3ControlHelper.SetThemed(GlyphLabel, Label.TextColorProperty, lightKey, darkKey);
    }

    private void UpdateAppearance()
    {
        RootBorder.Shadow = null!;
        if (!IsEnabled)
        {
            M3ControlHelper.SetThemed(RootBorder, Border.BackgroundColorProperty, "M3SurfaceVariant", "M3DarkSurfaceVariant");
            SetContentThemed("M3OnSurfaceVariant", "M3DarkOnSurface");
            RootBorder.Stroke = Colors.Transparent;
            TitleLabel.Opacity = 0.38;
            RootBorder.Opacity = 0.6;
            return;
        }

        RootBorder.Opacity = 1;
        TitleLabel.Opacity = 1;

        switch (Variant)
        {
            case M3ButtonVariant.Filled:
                M3ControlHelper.SetThemed(RootBorder, Border.BackgroundColorProperty, "M3Primary", "M3DarkPrimary");
                SetContentThemed("M3OnPrimary", "M3DarkOnPrimary");
                RootBorder.Stroke = Colors.Transparent;
                break;
            case M3ButtonVariant.Tonal:
                M3ControlHelper.SetThemed(RootBorder, Border.BackgroundColorProperty, "M3SecondaryContainer", "M3DarkPrimaryContainer");
                SetContentThemed("M3OnPrimaryContainer", "M3DarkPrimary");
                RootBorder.Stroke = Colors.Transparent;
                break;
            case M3ButtonVariant.Elevated:
                M3ControlHelper.SetThemed(RootBorder, Border.BackgroundColorProperty, "M3Surface", "M3DarkSurface");
                SetContentThemed("M3Primary", "M3DarkPrimary");
                RootBorder.Stroke = Colors.Transparent;
                RootBorder.Shadow = new Shadow
                {
                    Offset = new Point(0, 2),
                    Opacity = 0.25f,
                    Radius = (float)M3ControlHelper.ResDouble("M3Elevation2", 3)
                };
                break;
            case M3ButtonVariant.Outlined:
                RootBorder.BackgroundColor = Colors.Transparent;
                M3ControlHelper.SetThemed(RootBorder, Border.StrokeProperty, "M3Outline", "M3DarkOutline");
                SetContentThemed("M3Primary", "M3DarkPrimary");
                break;
            case M3ButtonVariant.Text:
                RootBorder.BackgroundColor = Colors.Transparent;
                RootBorder.Stroke = Colors.Transparent;
                SetContentThemed("M3Primary", "M3DarkPrimary");
                break;
        }
    }

    private void OnTapped(object? sender, TappedEventArgs e)
    {
        if (!IsEnabled) return;
        if (Command?.CanExecute(CommandParameter) == true)
            Command.Execute(CommandParameter);
        Clicked?.Invoke(this, EventArgs.Empty);
    }

    private void OnPointerPressed(object? sender, PointerEventArgs e) => RootBorder.Opacity = 0.85;
    private void OnPointerReleased(object? sender, PointerEventArgs e) => RootBorder.Opacity = IsEnabled ? 1 : 0.6;

    /// <summary>
    /// Occurs when the button is tapped while it is enabled.
    /// </summary>
    public event EventHandler? Clicked;
}
