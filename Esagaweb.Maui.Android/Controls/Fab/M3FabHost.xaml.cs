namespace Esagaweb.Maui.Android.Controls.Fab;

/// <summary>
/// Anfitrión de FAB flotante: contenido + botón anclado abajo (End o Center).
/// Sustituye el Grid overlay manual para usar M3Fab suelto en cualquier página.
/// </summary>
public partial class M3FabHost : ContentView
{
    /// <summary>Propiedad enlazable para <see cref="Body"/>.</summary>
    public static readonly BindableProperty BodyProperty =
        BindableProperty.Create(nameof(Body), typeof(View), typeof(M3FabHost), null,
            propertyChanged: (b, _, n) => ((M3FabHost)b).BodySlot.Content = (View?)n);

    /// <summary>Propiedad enlazable para <see cref="FabContent"/>.</summary>
    public static readonly BindableProperty FabContentProperty =
        BindableProperty.Create(nameof(FabContent), typeof(View), typeof(M3FabHost), null,
            propertyChanged: (b, _, n) => ((M3FabHost)b).FabSlot.Content = (View?)n);

    /// <summary>Propiedad enlazable para <see cref="Location"/>.</summary>
    public static readonly BindableProperty LocationProperty =
        BindableProperty.Create(nameof(Location), typeof(M3FabLocation), typeof(M3FabHost),
            M3FabLocation.End, propertyChanged: (b, _, _) => ((M3FabHost)b).UpdateFab());

    /// <summary>Contenido principal de la página.</summary>
    public View? Body
    {
        get => (View?)GetValue(BodyProperty);
        set => SetValue(BodyProperty, value);
    }

    /// <summary>FAB (normalmente M3Fab) que flota sobre el contenido.</summary>
    public View? FabContent
    {
        get => (View?)GetValue(FabContentProperty);
        set => SetValue(FabContentProperty, value);
    }

    /// <summary>Posición del FAB. Default End.</summary>
    public M3FabLocation Location
    {
        get => (M3FabLocation)GetValue(LocationProperty);
        set => SetValue(LocationProperty, value);
    }

    /// <summary>Crea una nueva instancia.</summary>
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
