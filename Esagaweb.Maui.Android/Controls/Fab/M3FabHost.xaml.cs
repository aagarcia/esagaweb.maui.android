namespace Esagaweb.Maui.Android.Controls.Fab;

/// <summary>Host layout that docks a floating action button over page content.</summary>
/// <remarks>Supports End and Center positions anchored to the bottom edge. Use this host instead of a manual overlay grid when a floating button is needed on a page. Set <see cref="Snackbar"/> to a <see cref="Controls.Snackbar.M3SnackbarHost"/> to automatically lift the FAB while the snackbar is visible (Material 3 behavior).</remarks>
/// <example>
/// <code><![CDATA[
/// <ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
///              xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
///              xmlns:fab="clr-namespace:Esagaweb.Maui.Android.Controls.Fab;assembly=Esagaweb.Maui.Android"
///              xmlns:common="clr-namespace:Esagaweb.Maui.Android.Controls.Common;assembly=Esagaweb.Maui.Android"
///              xmlns:snack="clr-namespace:Esagaweb.Maui.Android.Controls.Snackbar;assembly=Esagaweb.Maui.Android">
///     <fab:M3FabHost Location="End" Snackbar="{x:Reference Snacks}">
///         <fab:M3FabHost.Body>
///             <Grid RowDefinitions="*,Auto">
///                 <!-- Page content. -->
///                 <snack:M3SnackbarHost x:Name="Snacks" Grid.Row="1" />
///             </Grid>
///         </fab:M3FabHost.Body>
///         <fab:M3FabHost.FabContent>
///             <fab:M3Fab Variant="Primary"
///                        Text="Pagar"
///                        Glyph="{x:Static common:M3Icons.ShoppingCart}"
///                        Clicked="OnCartClicked" />
///         </fab:M3FabHost.FabContent>
///     </fab:M3FabHost>
/// </ContentPage>
/// ]]></code>
/// </example>
public partial class M3FabHost : ContentView
{
    /// <summary>Identifies the <see cref="Body"/> bindable property.</summary>
    public static readonly BindableProperty BodyProperty =
        BindableProperty.Create(nameof(Body), typeof(View), typeof(M3FabHost), null,
            propertyChanged: (b, _, n) => ((M3FabHost)b).BodySlot.Content = (View?)n);

    /// <summary>Identifies the <see cref="FabContent"/> bindable property.</summary>
    public static readonly BindableProperty FabContentProperty =
        BindableProperty.Create(nameof(FabContent), typeof(View), typeof(M3FabHost), null,
            propertyChanged: (b, _, n) => ((M3FabHost)b).FabSlot.Content = (View?)n);

    /// <summary>Identifies the <see cref="Location"/> bindable property.</summary>
    public static readonly BindableProperty LocationProperty =
        BindableProperty.Create(nameof(Location), typeof(M3FabLocation), typeof(M3FabHost),
            M3FabLocation.End, propertyChanged: (b, _, _) => ((M3FabHost)b).UpdateFab());

    /// <summary>Identifies the <see cref="Snackbar"/> bindable property.</summary>
    public static readonly BindableProperty SnackbarProperty =
        BindableProperty.Create(nameof(Snackbar), typeof(Controls.Snackbar.M3SnackbarHost), typeof(M3FabHost), null,
            propertyChanged: (b, _, n) => ((M3FabHost)b).OnSnackbarChanged((Controls.Snackbar.M3SnackbarHost?)n));

    /// <summary>Gets or sets the main page content displayed under the floating button. The default is null.</summary>
    /// <remarks>This is a bindable property.</remarks>
    public View? Body
    {
        get => (View?)GetValue(BodyProperty);
        set => SetValue(BodyProperty, value);
    }

    /// <summary>Gets or sets the floating content displayed over the page, usually a floating action button. The default is null.</summary>
    /// <remarks>This is a bindable property.</remarks>
    public View? FabContent
    {
        get => (View?)GetValue(FabContentProperty);
        set => SetValue(FabContentProperty, value);
    }

    /// <summary>Gets or sets the position of the floating content. The default is End.</summary>
    /// <remarks>This is a bindable property.</remarks>
    public M3FabLocation Location
    {
        get => (M3FabLocation)GetValue(LocationProperty);
        set => SetValue(LocationProperty, value);
    }

    /// <summary>
    /// Gets or sets the snackbar host whose visibility controls the FAB vertical offset.
    /// When the snackbar is visible, the FAB translates upward by the snackbar occupied height (Material 3 behavior).
    /// The default is <see langword="null"/>.
    /// </summary>
    /// <remarks>This is a bindable property.</remarks>
    public Controls.Snackbar.M3SnackbarHost? Snackbar
    {
        get => (Controls.Snackbar.M3SnackbarHost?)GetValue(SnackbarProperty);
        set => SetValue(SnackbarProperty, value);
    }

    /// <summary>Initializes a new instance of the <see cref="M3FabHost"/> class.</summary>
    public M3FabHost()
    {
        InitializeComponent();
        UpdateFab();
    }

    private Controls.Snackbar.M3SnackbarHost? _snackbar;

    private void OnSnackbarChanged(Controls.Snackbar.M3SnackbarHost? newSnackbar)
    {
        if (_snackbar is not null)
        {
            _snackbar.OccupiedHeightChanged -= OnSnackbarOccupiedHeightChanged;
        }
        _snackbar = newSnackbar;
        if (_snackbar is not null)
        {
            _snackbar.OccupiedHeightChanged += OnSnackbarOccupiedHeightChanged;
        }
        ApplySnackbarOffset();
    }

    private void OnSnackbarOccupiedHeightChanged(object? sender, EventArgs e)
    {
        ApplySnackbarOffset();
    }

    private void ApplySnackbarOffset()
    {
        var offset = _snackbar?.OccupiedHeight ?? 0;
        var translation = ComputeFabTranslation(offset);
        if (FabSlot.IsLoaded && FabSlot.Handler is not null)
        {
            _ = FabSlot.TranslateToAsync(0, translation, 150, Easing.CubicOut);
        }
        else
        {
            FabSlot.TranslationY = translation;
        }
    }

    /// <summary>
    /// Computes the vertical translation for the FAB slot based on the snackbar occupied height.
    /// </summary>
    /// <param name="occupiedHeight">The vertical space occupied by the snackbar.</param>
    /// <returns>The translation value (negative to move up, zero when no offset needed).</returns>
    internal static double ComputeFabTranslation(double occupiedHeight)
        => -Math.Max(0, occupiedHeight);

    private void UpdateFab()
    {
        if (Location == M3FabLocation.Center)
        {
            FabSlot.HorizontalOptions = LayoutOptions.Center;
            FabSlot.Margin = new Thickness(0, 0, 0, 16);
        }
        else
        {
            FabSlot.HorizontalOptions = LayoutOptions.End;
            FabSlot.Margin = new Thickness(0, 0, 16, 16);
        }
    }
}
