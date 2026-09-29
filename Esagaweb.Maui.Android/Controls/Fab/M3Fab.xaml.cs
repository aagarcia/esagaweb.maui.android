using System.Windows.Input;
using Esagaweb.Maui.Android.Controls.Common;
using Microsoft.Maui.Controls.Shapes;

namespace Esagaweb.Maui.Android.Controls.Fab;

/// <summary>Floating action button that promotes the primary action on a screen.</summary>
/// <remarks>Supports Primary, Surface, Secondary, and Tertiary color variants with automatic light and dark theme colors. Shows a circular button by default and switches to an extended pill layout when <see cref="Text"/> is set. Sizes and colors resolve from the shared theme resources. Register fonts and theme dictionaries with the setup method before use.</remarks>
/// <example>
/// <code><![CDATA[
/// <ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
///              xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
///              xmlns:fab="clr-namespace:Esagaweb.Maui.Android.Controls.Fab;assembly=Esagaweb.Maui.Android"
///              xmlns:common="clr-namespace:Esagaweb.Maui.Android.Controls.Common;assembly=Esagaweb.Maui.Android">
///     <fab:M3Fab Size="Regular"
///                Glyph="{x:Static common:M3Icons.Favorite}"
///                Clicked="OnFavClicked" />
/// </ContentPage>
/// ]]></code>
/// </example>
public partial class M3Fab : ContentView
{
    /// <summary>Identifies the <see cref="Glyph"/> bindable property.</summary>
    public static readonly BindableProperty GlyphProperty =
        BindableProperty.Create(nameof(Glyph), typeof(string), typeof(M3Fab), string.Empty,
            propertyChanged: (b, _, _) => ((M3Fab)b).UpdateIcon());

    /// <summary>Identifies the <see cref="Icon"/> bindable property.</summary>
    public static readonly BindableProperty IconProperty =
        BindableProperty.Create(nameof(Icon), typeof(ImageSource), typeof(M3Fab), null,
            propertyChanged: (b, _, n) =>
            {
                var self = (M3Fab)b;
                self.FabImage.Source = (ImageSource?)n;
                self.UpdateIcon();
            });

    /// <summary>Identifies the <see cref="Size"/> bindable property.</summary>
    public static readonly BindableProperty SizeProperty =
        BindableProperty.Create(nameof(Size), typeof(M3FabSize), typeof(M3Fab),
            M3FabSize.Regular, propertyChanged: (b, _, _) => ((M3Fab)b).UpdateAppearance());

    /// <summary>Identifies the <see cref="Command"/> bindable property.</summary>
    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(M3Fab), null);

    /// <summary>Identifies the <see cref="Variant"/> bindable property.</summary>
    public static readonly BindableProperty VariantProperty =
        BindableProperty.Create(nameof(Variant), typeof(M3FabVariant), typeof(M3Fab),
            M3FabVariant.Primary, propertyChanged: (b, _, _) => ((M3Fab)b).UpdateAppearance());

    /// <summary>Identifies the <see cref="Text"/> bindable property.</summary>
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(M3Fab), string.Empty,
            propertyChanged: (b, _, _) => ((M3Fab)b).UpdateAppearance());

    /// <summary>Gets or sets the Material Symbols glyph shown on the button. Takes priority over <see cref="Icon"/>. The default is an empty string.</summary>
    /// <remarks>This is a bindable property.</remarks>
    public string Glyph
    {
        get => (string)GetValue(GlyphProperty);
        set => SetValue(GlyphProperty, value);
    }

    /// <summary>Gets or sets the image shown when <see cref="Glyph"/> is empty. The default is null.</summary>
    /// <remarks>This is a bindable property.</remarks>
    public ImageSource? Icon
    {
        get => (ImageSource?)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    /// <summary>Gets or sets the button size. The default is Regular.</summary>
    /// <remarks>This is a bindable property.</remarks>
    public M3FabSize Size
    {
        get => (M3FabSize)GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }

    /// <summary>Gets or sets the command invoked when the button is tapped. The command receives no parameter. The default is null.</summary>
    /// <remarks>This is a bindable property.</remarks>
    public ICommand? Command
    {
        get => (ICommand?)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    /// <summary>Gets or sets the color variant. The default is Primary.</summary>
    /// <remarks>This is a bindable property.</remarks>
    public M3FabVariant Variant
    {
        get => (M3FabVariant)GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
    }

    /// <summary>Gets or sets the label shown in extended mode. When empty, the button renders as a circle. The default is an empty string.</summary>
    /// <remarks>This is a bindable property.</remarks>
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    /// <summary>Gets a value indicating whether the button uses the extended pill layout with icon and text. Returns true when <see cref="Text"/> is not empty.</summary>
    public bool IsExtended => !string.IsNullOrEmpty(Text);

    /// <summary>Initializes a new instance of the <see cref="M3Fab"/> class.</summary>
    public M3Fab()
    {
        InitializeComponent();
        GlyphLabel.FontFamily = "MaterialSymbols";
        UpdateIcon();
        UpdateAppearance();
    }

    private void UpdateIcon()
    {
        GlyphLabel.FontFamily = "MaterialSymbols";
        var hasGlyph = !string.IsNullOrEmpty(Glyph);
        GlyphLabel.Text = Glyph;
        GlyphLabel.IsVisible = hasGlyph;
        FabImage.IsVisible = !hasGlyph && FabImage.Source is not null;
    }

    private void UpdateAppearance()
    {
        var (bgLight, bgDark, iconLight, iconDark) = M3FabColors.Resolve(Variant);

        if (IsExtended)
        {
            // M3 pill: height 56, radius 16, horizontal padding.
            RootBorder.HeightRequest = 56;
            RootBorder.WidthRequest = -1;
            RootBorder.MinimumWidthRequest = 80;
            RootBorder.StrokeShape = new RoundRectangle { CornerRadius = 16 };
            RootBorder.Padding = new Thickness(20, 0);
            GlyphLabel.FontSize = 24;
            FabImage.HeightRequest = 24;
            FabImage.WidthRequest = 24;
            FabText.Text = Text;
            FabText.IsVisible = true;
            FabText.FontSize = M3ControlHelper.ResDouble("M3LabelLarge", 14);
        }
        else
        {
            var dim = Size switch
            {
                M3FabSize.Small => M3ControlHelper.ResDouble("M3FabSmall", 48),
                M3FabSize.Large => M3ControlHelper.ResDouble("M3FabLarge", 96),
                _ => M3ControlHelper.ResDouble("M3FabRegular", 56),
            };
            var iconDim = Size == M3FabSize.Large ? 36 : 24;

            RootBorder.HeightRequest = dim;
            RootBorder.WidthRequest = dim;
            RootBorder.MinimumWidthRequest = 0;
            RootBorder.StrokeShape = new RoundRectangle { CornerRadius = dim / 2 };
            RootBorder.Padding = 0;
            GlyphLabel.FontSize = iconDim;
            FabImage.HeightRequest = iconDim;
            FabImage.WidthRequest = iconDim;
            FabText.IsVisible = false;
        }

        M3ControlHelper.SetThemed(RootBorder, Border.BackgroundColorProperty, bgLight, bgDark);
        M3ControlHelper.SetThemed(GlyphLabel, Label.TextColorProperty, iconLight, iconDark);
        M3ControlHelper.SetThemed(FabText, Label.TextColorProperty, iconLight, iconDark);
        RootBorder.Shadow = new Shadow
        {
            Offset = new Point(0, 4),
            Opacity = 0.3f,
            Radius = (float)M3ControlHelper.ResDouble("M3Elevation3", 6)
        };
    }

    private void OnTapped(object? sender, TappedEventArgs e)
    {
        if (!IsEnabled) return;
        Command?.Execute(null);
        Clicked?.Invoke(this, EventArgs.Empty);
    }

    private void OnPointerPressed(object? sender, PointerEventArgs e) => RootBorder.Opacity = 0.85;
    private void OnPointerReleased(object? sender, PointerEventArgs e) => RootBorder.Opacity = 1;

    /// <summary>Occurs when the button is tapped.</summary>
    public event EventHandler? Clicked;
}
