namespace Esagaweb.Maui.Android.Controls.Fab;

/// <summary>Host layout that docks a floating action button over page content.</summary>
/// <remarks>Supports End and Center positions anchored to the bottom edge. Use this host instead of a manual overlay grid when a floating button is needed on a page.</remarks>
/// <example>
/// <code><![CDATA[
/// <ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
///              xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
///              xmlns:fab="clr-namespace:Esagaweb.Maui.Android.Controls.Fab;assembly=Esagaweb.Maui.Android"
///              xmlns:common="clr-namespace:Esagaweb.Maui.Android.Controls.Common;assembly=Esagaweb.Maui.Android">
///     <fab:M3FabHost Location="End">
///         <fab:M3FabHost.Body>
///             <!-- Page content. -->
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

    /// <summary>Initializes a new instance of the <see cref="M3FabHost"/> class.</summary>
    public M3FabHost()
    {
        InitializeComponent();
        UpdateFab();
    }

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
