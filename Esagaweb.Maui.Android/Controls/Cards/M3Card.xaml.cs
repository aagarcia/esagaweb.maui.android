using System.Windows.Input;
using Esagaweb.Maui.Android.Controls.Common;
using Microsoft.Maui.Controls.Shapes;

namespace Esagaweb.Maui.Android.Controls.Cards;

/// <summary>
/// Card M3 guiada por Flutter (Card / Card.filled / Card.outlined):
/// child = Content del Border, color = CardColor, elevation = Elevation,
/// shape = CornerRadius, margin = Margin (default 4 como Flutter).
/// Toda la card puede ser tocable (Command + Clicked).
/// </summary>
public partial class M3Card : Border
{
    /// <summary>Propiedad enlazable para <see cref="Variant"/>.</summary>
    public static readonly BindableProperty VariantProperty =
        BindableProperty.Create(nameof(Variant), typeof(M3CardVariant), typeof(M3Card),
            M3CardVariant.Elevated, propertyChanged: (b, _, _) => ((M3Card)b).UpdateAppearance());

    /// <summary>Propiedad enlazable para <see cref="CardColor"/>.</summary>
    public static readonly BindableProperty CardColorProperty =
        BindableProperty.Create(nameof(CardColor), typeof(Color), typeof(M3Card), null,
            propertyChanged: (b, _, _) => ((M3Card)b).UpdateAppearance());

    /// <summary>Propiedad enlazable para <see cref="Elevation"/>.</summary>
    public static readonly BindableProperty ElevationProperty =
        BindableProperty.Create(nameof(Elevation), typeof(double), typeof(M3Card), -1.0,
            propertyChanged: (b, _, _) => ((M3Card)b).UpdateAppearance());

    /// <summary>Propiedad enlazable para <see cref="CornerRadius"/>.</summary>
    public static readonly BindableProperty CornerRadiusProperty =
        BindableProperty.Create(nameof(CornerRadius), typeof(double), typeof(M3Card), 12.0,
            propertyChanged: (b, _, _) => ((M3Card)b).UpdateAppearance());

    /// <summary>Propiedad enlazable para <see cref="ContentPadding"/>.</summary>
    public static readonly BindableProperty ContentPaddingProperty =
        BindableProperty.Create(nameof(ContentPadding), typeof(Thickness), typeof(M3Card), new Thickness(16),
            propertyChanged: (b, _, n) => ((M3Card)b).Padding = (Thickness)n);

    /// <summary>Propiedad enlazable para <see cref="Command"/>.</summary>
    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(M3Card), null);

    /// <summary>Propiedad enlazable para <see cref="CommandParameter"/>.</summary>
    public static readonly BindableProperty CommandParameterProperty =
        BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(M3Card), null);

    /// <summary>Variante visual. Default Elevated.</summary>
    public M3CardVariant Variant
    {
        get => (M3CardVariant)GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
    }

    /// <summary>Null = color default de la variante (igual que Flutter).</summary>
    public Color? CardColor
    {
        get => (Color?)GetValue(CardColorProperty);
        set => SetValue(CardColorProperty, value);
    }

    /// <summary>-1 = elevación default de la variante (1 elevated, 0 resto).</summary>
    public double Elevation
    {
        get => (double)GetValue(ElevationProperty);
        set => SetValue(ElevationProperty, value);
    }

    /// <summary>Radio de esquina en dp. Default 12.</summary>
    public double CornerRadius
    {
        get => (double)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    /// <summary>Relleno interior. Default 16 en los 4 lados.</summary>
    public Thickness ContentPadding
    {
        get => (Thickness)GetValue(ContentPaddingProperty);
        set => SetValue(ContentPaddingProperty, value);
    }

    /// <summary>Comando al tocar la card (recibe <see cref="CommandParameter"/>).</summary>
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

    /// <summary>Crea una nueva instancia.</summary>
    public M3Card()
    {
        InitializeComponent();
        Margin = new Thickness(4); // default Flutter
        Padding = ContentPadding;
        UpdateAppearance();
    }

    private void UpdateAppearance()
    {
        Padding = ContentPadding;
        StrokeShape = new RoundRectangle { CornerRadius = CornerRadius };

        if (CardColor is not null)
        {
            BackgroundColor = CardColor;
        }
        else switch (Variant)
        {
            case M3CardVariant.Filled:
                M3ControlHelper.SetThemed(this, BackgroundColorProperty, "M3SurfaceVariant", "M3DarkSurfaceVariant");
                break;
            default:
                M3ControlHelper.SetThemed(this, BackgroundColorProperty, "M3Surface", "M3DarkSurface");
                break;
        }

        if (Variant == M3CardVariant.Outlined)
            M3ControlHelper.SetThemed(this, StrokeProperty, "M3Outline", "M3DarkOutline");
        else
            Stroke = Colors.Transparent;

        var elev = Elevation >= 0 ? Elevation
            : Variant == M3CardVariant.Elevated ? M3ControlHelper.ResDouble("M3Elevation1", 1) : 0;
        Shadow = elev > 0
            ? new Shadow { Offset = new Point(0, elev), Opacity = 0.25f, Radius = (float)elev }
            : null!;
    }

    private void OnTapped(object? sender, TappedEventArgs e)
    {
        if (Command?.CanExecute(CommandParameter) == true)
            Command.Execute(CommandParameter);
        Clicked?.Invoke(this, EventArgs.Empty);
    }

    private void OnPointerPressed(object? sender, PointerEventArgs e) => Opacity = 0.93;
    private void OnPointerReleased(object? sender, PointerEventArgs e) => Opacity = 1;

    /// <summary>Se dispara al tocar la card.</summary>
    public event EventHandler? Clicked;
}
