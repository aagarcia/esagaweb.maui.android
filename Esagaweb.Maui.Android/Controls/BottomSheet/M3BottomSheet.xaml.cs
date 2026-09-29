using Esagaweb.Maui.Android.Controls.Common;
using Microsoft.Maui.Controls.Shapes;

namespace Esagaweb.Maui.Android.Controls.BottomSheet;

/// <summary>
/// Hoja inferior modal (como Flutter showModalBottomSheet): título + contenido,
/// se cierra con el scrim, el botón de la showcase o atrás. Arrastre completo
/// como mejora futura. Se muestra con <see cref="ShowAsync"/> o incrustada.
/// </summary>
[ContentProperty(nameof(SheetBody))]
public partial class M3BottomSheet : ContentView
{
    /// <summary>Propiedad enlazable para <see cref="Title"/>.</summary>
    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(nameof(Title), typeof(string), typeof(M3BottomSheet), string.Empty,
            propertyChanged: (b, _, n) =>
            {
                var self = (M3BottomSheet)b;
                self.TitleLabel.Text = (string?)n ?? string.Empty;
                self.TitleLabel.IsVisible = !string.IsNullOrEmpty((string?)n);
            });

    /// <summary>Título. Vacío = se oculta.</summary>
    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    /// <summary>Contenido de la hoja (hijos XAML van aquí).</summary>
    public View? SheetBody
    {
        get => SheetContent.Content;
        set => SheetContent.Content = value;
    }

    /// <summary>Se dispara al cerrarse (por cualquier vía).</summary>
    public event EventHandler? Dismissed;

    /// <summary>Crea una nueva instancia.</summary>
    public M3BottomSheet()
    {
        InitializeComponent();
        TitleLabel.FontFamily = "NunitoSemiBold";
        TitleLabel.IsVisible = !string.IsNullOrEmpty(Title);
        ApplyTheme();
    }

    private Page? _hostPage;

    /// <summary>Muestra la hoja como modal inferior a pantalla completa con scrim.</summary>
    public async Task ShowAsync(INavigation navigation)
    {
        var scrim = new BoxView { Color = Color.FromArgb("#52000000") };
        scrim.GestureRecognizers.Add(new TapGestureRecognizer
        {
            Command = new Command(async () => await HideAsync())
        });
        // Desvincular de un padre anterior ANTES de agregar al host:
        // hacerlo después lo arrancaría de su propia página host.
        (Parent as Layout)?.Children.Remove(this);
        _hostPage = new ContentPage
        {
            BackgroundColor = Colors.Transparent,
            Content = new Grid
            {
                RowDefinitions = new RowDefinitionCollection(
                    new RowDefinition(GridLength.Star),
                    new RowDefinition(GridLength.Auto)),
                Children = { scrim, this }
            }
        };
        Grid.SetRow(this, 1);
        await navigation.PushModalAsync(_hostPage);
    }

    /// <summary>Cierra la hoja modal (si está visible).</summary>
    public async Task HideAsync()
    {
        if (_hostPage?.Navigation is not null)
            await _hostPage.Navigation.PopModalAsync();
        _hostPage = null;
        Dismissed?.Invoke(this, EventArgs.Empty);
    }

    private void ApplyTheme()
    {
        M3ControlHelper.SetThemed(Sheet, Border.BackgroundColorProperty, "M3Surface", "M3DarkSurface");
        M3ControlHelper.SetThemed(TitleLabel, Label.TextColorProperty, "M3OnSurface", "M3DarkOnSurface");
        M3ControlHelper.SetThemed(Handle, BoxView.ColorProperty, "M3Outline", "M3DarkOutline");
    }
}
