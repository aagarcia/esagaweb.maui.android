using System.Windows.Input;
using Esagaweb.Maui.Android.Controls.Common;
using Microsoft.Maui.Controls.Shapes;

namespace Esagaweb.Maui.Android.Controls.Cards;

/// <summary>
/// Represents a Material 3 card that hosts arbitrary content with configurable color, elevation and shape.
/// </summary>
/// <remarks>
/// The appearance is selected with <see cref="Variant"/>. A <see langword="null"/> <see cref="CardColor"/> uses
/// the default color of the variant, and an <see cref="Elevation"/> of -1 uses the default elevation of the variant.
/// The corner shape follows <see cref="CornerRadius"/>, and the inner spacing follows <see cref="ContentPadding"/>.
/// Tapping the card runs <see cref="Command"/> when it can execute, then raises <see cref="Clicked"/>.
/// Background, border and shadow follow the current theme through <c>M3Surface</c>, <c>M3DarkSurface</c>,
/// <c>M3SurfaceVariant</c>, <c>M3DarkSurfaceVariant</c>, <c>M3Outline</c>, <c>M3DarkOutline</c> and <c>M3Elevation1</c>.
/// </remarks>
/// <example>
/// <code language="xaml">
/// <![CDATA[
/// <cards:M3Card xmlns:cards="clr-namespace:Esagaweb.Maui.Android.Controls.Cards;assembly=Esagaweb.Maui.Android"
///               Variant="Elevated">
///     <VerticalStackLayout Spacing="8">
///         <Label Text="Product" />
///         <Label Text="Shipping in 24/48h." />
///     </VerticalStackLayout>
/// </cards:M3Card>
/// ]]>
/// </code>
/// </example>
public partial class M3Card : Border
{
    /// <summary>
    /// Identifies the <see cref="Variant"/> bindable property.
    /// </summary>
    public static readonly BindableProperty VariantProperty =
        BindableProperty.Create(nameof(Variant), typeof(M3CardVariant), typeof(M3Card),
            M3CardVariant.Elevated, propertyChanged: (b, _, _) => ((M3Card)b).UpdateAppearance());

    /// <summary>
    /// Identifies the <see cref="CardColor"/> bindable property.
    /// </summary>
    public static readonly BindableProperty CardColorProperty =
        BindableProperty.Create(nameof(CardColor), typeof(Color), typeof(M3Card), null,
            propertyChanged: (b, _, _) => ((M3Card)b).UpdateAppearance());

    /// <summary>
    /// Identifies the <see cref="Elevation"/> bindable property.
    /// </summary>
    public static readonly BindableProperty ElevationProperty =
        BindableProperty.Create(nameof(Elevation), typeof(double), typeof(M3Card), -1.0,
            propertyChanged: (b, _, _) => ((M3Card)b).UpdateAppearance());

    /// <summary>
    /// Identifies the <see cref="CornerRadius"/> bindable property.
    /// </summary>
    public static readonly BindableProperty CornerRadiusProperty =
        BindableProperty.Create(nameof(CornerRadius), typeof(double), typeof(M3Card), 12.0,
            propertyChanged: (b, _, _) => ((M3Card)b).UpdateAppearance());

    /// <summary>
    /// Identifies the <see cref="ContentPadding"/> bindable property.
    /// </summary>
    public static readonly BindableProperty ContentPaddingProperty =
        BindableProperty.Create(nameof(ContentPadding), typeof(Thickness), typeof(M3Card), new Thickness(16),
            propertyChanged: (b, _, n) => ((M3Card)b).Padding = (Thickness)n);

    /// <summary>
    /// Identifies the <see cref="Command"/> bindable property.
    /// </summary>
    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(M3Card), null);

    /// <summary>
    /// Identifies the <see cref="CommandParameter"/> bindable property.
    /// </summary>
    public static readonly BindableProperty CommandParameterProperty =
        BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(M3Card), null);

    /// <summary>
    /// Gets or sets the visual variant of the card. The default is <see cref="M3CardVariant.Elevated"/>.
    /// </summary>
    /// <remarks>
    /// This is a bindable property.
    /// </remarks>
    public M3CardVariant Variant
    {
        get => (M3CardVariant)GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
    }

    /// <summary>
    /// Gets or sets the background color of the card. A <see langword="null"/> value uses the default color of the variant. The default is <see langword="null"/>.
    /// </summary>
    /// <remarks>
    /// This is a bindable property.
    /// </remarks>
    public Color? CardColor
    {
        get => (Color?)GetValue(CardColorProperty);
        set => SetValue(CardColorProperty, value);
    }

    /// <summary>
    /// Gets or sets the elevation of the card. A value of -1 uses the default elevation of the variant. The default is -1.
    /// </summary>
    /// <remarks>
    /// This is a bindable property.
    /// </remarks>
    public double Elevation
    {
        get => (double)GetValue(ElevationProperty);
        set => SetValue(ElevationProperty, value);
    }

    /// <summary>
    /// Gets or sets the corner radius of the card in density-independent pixels. The default is 12.
    /// </summary>
    /// <remarks>
    /// This is a bindable property.
    /// </remarks>
    public double CornerRadius
    {
        get => (double)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    /// <summary>
    /// Gets or sets the inner padding of the card content. The default is 16 on all sides.
    /// </summary>
    /// <remarks>
    /// This is a bindable property.
    /// </remarks>
    public Thickness ContentPadding
    {
        get => (Thickness)GetValue(ContentPaddingProperty);
        set => SetValue(ContentPaddingProperty, value);
    }

    /// <summary>
    /// Gets or sets the command executed when the card is tapped. It receives <see cref="CommandParameter"/>. The default is <see langword="null"/>.
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

    /// <summary>
    /// Initializes a new instance of the <see cref="M3Card"/> class.
    /// </summary>
    public M3Card()
    {
        InitializeComponent();
        Margin = new Thickness(4); // Use the Flutter default margin.
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

    /// <summary>
    /// Occurs when the card is tapped.
    /// </summary>
    public event EventHandler? Clicked;
}
