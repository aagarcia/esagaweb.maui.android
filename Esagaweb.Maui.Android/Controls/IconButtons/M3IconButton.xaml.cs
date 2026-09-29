using System.Windows.Input;
using Esagaweb.Maui.Android.Controls.Common;

namespace Esagaweb.Maui.Android.Controls.IconButtons;

/// <summary>Botón de solo icono M3 (Standard/Filled/Tonal/Outlined) con glyph MaterialSymbols blindado.</summary>
public partial class M3IconButton : ContentView
{
    /// <summary>Propiedad enlazable para <see cref="Glyph"/>.</summary>
    public static readonly BindableProperty GlyphProperty =
        BindableProperty.Create(nameof(Glyph), typeof(string), typeof(M3IconButton), string.Empty,
            propertyChanged: (b, _, _) => ((M3IconButton)b).UpdateIcon());

    /// <summary>Propiedad enlazable para <see cref="Icon"/>.</summary>
    public static readonly BindableProperty IconProperty =
        BindableProperty.Create(nameof(Icon), typeof(ImageSource), typeof(M3IconButton), null,
            propertyChanged: (b, _, n) =>
            {
                var self = (M3IconButton)b;
                self.ButtonImage.Source = (ImageSource?)n;
                self.UpdateIcon();
            });

    /// <summary>Propiedad enlazable para <see cref="Variant"/>.</summary>
    public static readonly BindableProperty VariantProperty =
        BindableProperty.Create(nameof(Variant), typeof(M3IconButtonVariant), typeof(M3IconButton),
            M3IconButtonVariant.Standard, propertyChanged: (b, _, _) => ((M3IconButton)b).UpdateAppearance());

    /// <summary>Propiedad enlazable para <see cref="Command"/>.</summary>
    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(M3IconButton), null);

    /// <summary>Glyph de fuente MaterialSymbols (estilo M3Icons.Favorite). Prioritario sobre Icon.</summary>
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

    /// <summary>Variante visual. Default Standard.</summary>
    public M3IconButtonVariant Variant
    {
        get => (M3IconButtonVariant)GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
    }

    /// <summary>Comando al tocar (sin parámetros).</summary>
    public ICommand? Command
    {
        get => (ICommand?)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    /// <summary>Crea una nueva instancia.</summary>
    public M3IconButton()
    {
        InitializeComponent();
        GlyphLabel.FontFamily = "MaterialSymbols";
        UpdateIcon();
        UpdateAppearance();
    }

    /// <inheritdoc/>
    protected override void OnPropertyChanged(string? propertyName = null)
    {
        base.OnPropertyChanged(propertyName);
        if (propertyName == nameof(IsEnabled))
            UpdateAppearance();
    }

    private void UpdateIcon()
    {
        GlyphLabel.FontFamily = "MaterialSymbols";
        var hasGlyph = !string.IsNullOrEmpty(Glyph);
        GlyphLabel.Text = Glyph;
        GlyphLabel.IsVisible = hasGlyph;
        ButtonImage.IsVisible = !hasGlyph && ButtonImage.Source is not null;
    }

    private void SetContentThemed(string lightKey, string darkKey)
        => M3ControlHelper.SetThemed(GlyphLabel, Label.TextColorProperty, lightKey, darkKey);

    private void UpdateAppearance()
    {
        if (!IsEnabled)
        {
            RootBorder.Opacity = 0.38;
            SetContentThemed("M3OnSurfaceVariant", "M3DarkOnSurface");
            return;
        }
        RootBorder.Opacity = 1;

        switch (Variant)
        {
            case M3IconButtonVariant.Filled:
                M3ControlHelper.SetThemed(RootBorder, Border.BackgroundColorProperty, "M3Primary", "M3DarkPrimary");
                RootBorder.Stroke = Colors.Transparent;
                SetContentThemed("M3OnPrimary", "M3DarkOnPrimary");
                break;
            case M3IconButtonVariant.Tonal:
                M3ControlHelper.SetThemed(RootBorder, Border.BackgroundColorProperty, "M3SecondaryContainer", "M3DarkPrimaryContainer");
                RootBorder.Stroke = Colors.Transparent;
                SetContentThemed("M3OnPrimaryContainer", "M3DarkPrimary");
                break;
            case M3IconButtonVariant.Outlined:
                RootBorder.BackgroundColor = Colors.Transparent;
                M3ControlHelper.SetThemed(RootBorder, Border.StrokeProperty, "M3Outline", "M3DarkOutline");
                SetContentThemed("M3Primary", "M3DarkPrimary");
                break;
            default: // Standard
                RootBorder.BackgroundColor = Colors.Transparent;
                RootBorder.Stroke = Colors.Transparent;
                SetContentThemed("M3Primary", "M3DarkPrimary");
                break;
        }
    }

    private void OnTapped(object? sender, TappedEventArgs e)
    {
        if (!IsEnabled) return;
        Command?.Execute(null);
        Clicked?.Invoke(this, EventArgs.Empty);
    }

    private void OnPointerPressed(object? sender, PointerEventArgs e) => RootBorder.Opacity = 0.7;
    private void OnPointerReleased(object? sender, PointerEventArgs e) => RootBorder.Opacity = IsEnabled ? 1 : 0.38;

    /// <summary>Se dispara al tocar (si está habilitado).</summary>
    public event EventHandler? Clicked;
}
