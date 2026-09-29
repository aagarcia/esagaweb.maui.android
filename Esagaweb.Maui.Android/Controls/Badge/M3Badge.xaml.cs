using Esagaweb.Maui.Android.Controls.Common;

namespace Esagaweb.Maui.Android.Controls.Badge;

/// <summary>
/// Represents a Material 3 badge that displays a notification count or a small dot over an icon or other content.
/// </summary>
/// <remarks>
/// Set <see cref="Count"/> to update the displayed value. The badge is hidden when the count is zero unless
/// <see cref="ShowZero"/> is <see langword="true"/>, and values above <see cref="MaxCount"/> are shown as
/// "{MaxCount}+". Set <see cref="IsDot"/> to <see langword="true"/> to show a dot without a number.
/// The background and text colors follow the current theme through <c>M3Error</c>, <c>M3DarkError</c>,
/// <c>M3OnError</c> and <c>M3DarkOnPrimary</c>, and the pill height uses <c>M3BadgeSize</c>.
/// The host application must register fonts with <c>UseEsagaweb</c> so the NunitoSemiBold font resolves.
/// The badge does not position itself. Overlay it on other content with a <c>Grid</c>.
/// </remarks>
/// <example>
/// <code language="xaml">
/// <![CDATA[
/// <Grid xmlns:badge="clr-namespace:Esagaweb.Maui.Android.Controls.Badge;assembly=Esagaweb.Maui.Android">
///     <badge:M3Badge Count="3"
///                    MaxCount="99"
///                    HorizontalOptions="End"
///                    VerticalOptions="Start" />
/// </Grid>
/// ]]>
/// </code>
/// </example>
public partial class M3Badge : ContentView
{
    /// <summary>
    /// Identifies the <see cref="Count"/> bindable property.
    /// </summary>
    public static readonly BindableProperty CountProperty =
        BindableProperty.Create(nameof(Count), typeof(int), typeof(M3Badge), 0,
            propertyChanged: (b, _, _) => ((M3Badge)b).UpdateAppearance());

    /// <summary>
    /// Identifies the <see cref="MaxCount"/> bindable property.
    /// </summary>
    public static readonly BindableProperty MaxCountProperty =
        BindableProperty.Create(nameof(MaxCount), typeof(int), typeof(M3Badge), 99,
            propertyChanged: (b, _, _) => ((M3Badge)b).UpdateAppearance());

    /// <summary>
    /// Identifies the <see cref="ShowZero"/> bindable property.
    /// </summary>
    public static readonly BindableProperty ShowZeroProperty =
        BindableProperty.Create(nameof(ShowZero), typeof(bool), typeof(M3Badge), false,
            propertyChanged: (b, _, _) => ((M3Badge)b).UpdateAppearance());

    /// <summary>
    /// Identifies the <see cref="IsDot"/> bindable property.
    /// </summary>
    public static readonly BindableProperty IsDotProperty =
        BindableProperty.Create(nameof(IsDot), typeof(bool), typeof(M3Badge), false,
            propertyChanged: (b, _, _) => ((M3Badge)b).UpdateAppearance());

    /// <summary>
    /// Gets or sets the count displayed inside the badge. The badge is hidden when the value is zero unless <see cref="ShowZero"/> is <see langword="true"/>. The default is 0.
    /// </summary>
    /// <remarks>
    /// This is a bindable property.
    /// </remarks>
    public int Count
    {
        get => (int)GetValue(CountProperty);
        set => SetValue(CountProperty, value);
    }

    /// <summary>
    /// Gets or sets the maximum count displayed in full. Larger values are displayed as "{MaxCount}+". The default is 99.
    /// </summary>
    /// <remarks>
    /// This is a bindable property.
    /// </remarks>
    public int MaxCount
    {
        get => (int)GetValue(MaxCountProperty);
        set => SetValue(MaxCountProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the badge remains visible when <see cref="Count"/> is zero. The default is <see langword="false"/>.
    /// </summary>
    /// <remarks>
    /// This is a bindable property.
    /// </remarks>
    public bool ShowZero
    {
        get => (bool)GetValue(ShowZeroProperty);
        set => SetValue(ShowZeroProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the badge is displayed as a dot without a number. The default is <see langword="false"/>.
    /// </summary>
    /// <remarks>
    /// This is a bindable property.
    /// </remarks>
    public bool IsDot
    {
        get => (bool)GetValue(IsDotProperty);
        set => SetValue(IsDotProperty, value);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="M3Badge"/> class.
    /// </summary>
    public M3Badge()
    {
        InitializeComponent();
        CountLabel.FontFamily = "NunitoSemiBold";
        UpdateAppearance();
    }

    private void UpdateAppearance()
    {
        IsVisible = IsDot || ShowZero || Count > 0;
        if (!IsVisible) return;
        CountLabel.Text = IsDot ? string.Empty : Count > MaxCount ? $"{MaxCount}+" : $"{Count}";
        CountLabel.IsVisible = !IsDot;
        Pill.Padding = IsDot ? new Thickness(0) : new Thickness(4, 0);
        Pill.WidthRequest = IsDot ? 8 : -1;
        Pill.HeightRequest = IsDot ? 8 : M3ControlHelper.ResDouble("M3BadgeSize", 16);
        M3ControlHelper.SetThemed(Pill, Border.BackgroundColorProperty, "M3Error", "M3DarkError");
        M3ControlHelper.SetThemed(CountLabel, Label.TextColorProperty, "M3OnError", "M3DarkOnPrimary");
    }
}
