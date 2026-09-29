using System.Windows.Input;
using Esagaweb.Maui.Android.Controls.Common;
using Esagaweb.Maui.Android.Helpers;

namespace Esagaweb.Maui.Android.Controls.Buttons;

/// <summary>Botón M3 (Elevated/Filled/Tonal/Outlined/Text) con glyph MaterialSymbols blindado.</summary>
public partial class M3Button : ContentView
{
    /// <summary>Propiedad enlazable para <see cref="Text"/>.</summary>
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(M3Button), string.Empty,
            propertyChanged: (b, _, n) => ((M3Button)b).TitleLabel.Text = (string?)n ?? string.Empty);

    /// <summary>Propiedad enlazable para <see cref="Icon"/>.</summary>
    public static readonly BindableProperty IconProperty =
        BindableProperty.Create(nameof(Icon), typeof(ImageSource), typeof(M3Button), null,
            propertyChanged: (b, _, n) =>
            {
                var self = (M3Button)b;
                self.IconImage.Source = (ImageSource?)n;
                self.UpdateGlyph();
            });

    /// <summary>Propiedad enlazable para <see cref="Glyph"/>.</summary>
    public static readonly BindableProperty GlyphProperty =
        BindableProperty.Create(nameof(Glyph), typeof(string), typeof(M3Button), string.Empty,
            propertyChanged: (b, _, _) => ((M3Button)b).UpdateGlyph());

    /// <summary>Propiedad enlazable para <see cref="Variant"/>.</summary>
    public static readonly BindableProperty VariantProperty =
        BindableProperty.Create(nameof(Variant), typeof(M3ButtonVariant), typeof(M3Button),
            M3ButtonVariant.Filled, propertyChanged: (b, _, _) => ((M3Button)b).UpdateAppearance());

    /// <summary>Propiedad enlazable para <see cref="Command"/>.</summary>
    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(M3Button), null);

    /// <summary>Propiedad enlazable para <see cref="CommandParameter"/>.</summary>
    public static readonly BindableProperty CommandParameterProperty =
        BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(M3Button), null);

    /// <summary>Texto del botón.</summary>
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    /// <summary>Imagen fallback cuando Glyph está vacío.</summary>
    public ImageSource? Icon
    {
        get => (ImageSource?)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    /// <summary>Glyph de fuente MaterialSymbols (estilo M3Icons.Favorite). Prioritario sobre Icon.</summary>
    public string Glyph
    {
        get => (string)GetValue(GlyphProperty);
        set => SetValue(GlyphProperty, value);
    }

    /// <summary>Variante visual. Default Filled.</summary>
    public M3ButtonVariant Variant
    {
        get => (M3ButtonVariant)GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
    }

    /// <summary>Comando al tocar (recibe <see cref="CommandParameter"/>).</summary>
    public ICommand? Command
    {
        get => (ICommand?)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    /// <summary>Parámetro del <see cref="Command"/>.</summary>
    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    private M3Breakpoint? _lastBp;

    /// <summary>Crea una nueva instancia.</summary>
    public M3Button()
    {
        InitializeComponent();
        // Blindaje: el estilo implícito de Label (Nunito) nunca debe pisar el glyph.
        GlyphLabel.FontFamily = "MaterialSymbols";
        TitleLabel.FontFamily = "NunitoSemiBold";
        TitleLabel.Text = Text;
        UpdateGlyph();
        UpdateAppearance();
        ApplyBreakpoint(400); // Compact por defecto (teléfono)
    }

    /// <summary>Responsive: 40dp Compact, 48dp Medium/Expanded. Llamar desde SizeChanged de la página.</summary>
    public void ApplyBreakpoint(double widthDp)
    {
        if (widthDp <= 0)
            return;
        var bp = ResponsiveHelper.GetBreakpoint(widthDp);
        if (bp == _lastBp)
            return; // Mismo breakpoint: reasignar HeightRequest realimenta SizeChanged.
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
        // Reafirmar fuentes en cada cambio: protege frente a estilos implícitos aplicados después.
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

    /// <summary>Se dispara al tocar (si está habilitado).</summary>
    public event EventHandler? Clicked;
}
