using System.Windows.Input;
using Esagaweb.Maui.Android.Controls.Common;
using Microsoft.Maui.Controls.Shapes;

namespace Esagaweb.Maui.Android.Controls.Fab;

/// <summary>Botón flotante M3 (Small/Regular/Large) con glyph MaterialSymbols blindado.</summary>
public partial class M3Fab : ContentView
{
    /// <summary>Propiedad enlazable para <see cref="Glyph"/>.</summary>
    public static readonly BindableProperty GlyphProperty =
        BindableProperty.Create(nameof(Glyph), typeof(string), typeof(M3Fab), string.Empty,
            propertyChanged: (b, _, _) => ((M3Fab)b).UpdateIcon());

    /// <summary>Propiedad enlazable para <see cref="Icon"/>.</summary>
    public static readonly BindableProperty IconProperty =
        BindableProperty.Create(nameof(Icon), typeof(ImageSource), typeof(M3Fab), null,
            propertyChanged: (b, _, n) =>
            {
                var self = (M3Fab)b;
                self.FabImage.Source = (ImageSource?)n;
                self.UpdateIcon();
            });

    /// <summary>Propiedad enlazable para <see cref="Size"/>.</summary>
    public static readonly BindableProperty SizeProperty =
        BindableProperty.Create(nameof(Size), typeof(M3FabSize), typeof(M3Fab),
            M3FabSize.Regular, propertyChanged: (b, _, _) => ((M3Fab)b).UpdateAppearance());

    /// <summary>Propiedad enlazable para <see cref="Command"/>.</summary>
    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(M3Fab), null);

    /// <summary>Propiedad enlazable para <see cref="Variant"/>.</summary>
    public static readonly BindableProperty VariantProperty =
        BindableProperty.Create(nameof(Variant), typeof(M3FabVariant), typeof(M3Fab),
            M3FabVariant.Primary, propertyChanged: (b, _, _) => ((M3Fab)b).UpdateAppearance());

    /// <summary>Propiedad enlazable para <see cref="Text"/>.</summary>
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(M3Fab), string.Empty,
            propertyChanged: (b, _, _) => ((M3Fab)b).UpdateAppearance());

    /// <summary>Glyph de fuente MaterialSymbols (estilo M3Icons.Add). Prioritario sobre Icon.</summary>
    public string Glyph
    {
        get => (string)GetValue(GlyphProperty);
        set => SetValue(GlyphProperty, value);
    }

    /// <summary>Imagen fallback cuando Glyph está vacío.</summary>
    public ImageSource? Icon
    {
        get => (ImageSource?)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    /// <summary>Tamaño (48/56/96dp). Default Regular.</summary>
    public M3FabSize Size
    {
        get => (M3FabSize)GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }

    /// <summary>Comando al tocar (sin parámetros).</summary>
    public ICommand? Command
    {
        get => (ICommand?)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    /// <summary>Variante de color M3. Default Primary.</summary>
    public M3FabVariant Variant
    {
        get => (M3FabVariant)GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
    }

    /// <summary>Texto del modo extendido. Vacío = FAB circular. Default "".</summary>
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    /// <summary>True cuando <see cref="Text"/> no está vacío (píldora con icono + texto).</summary>
    public bool IsExtended => !string.IsNullOrEmpty(Text);

    /// <summary>Crea una nueva instancia.</summary>
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
            // Píldora M3: alto 56, radio 16, respiración lateral.
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

    /// <summary>Se dispara al tocar.</summary>
    public event EventHandler? Clicked;
}
