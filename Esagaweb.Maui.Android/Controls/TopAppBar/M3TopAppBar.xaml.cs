using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;
using Esagaweb.Maui.Android.Controls.Common;

namespace Esagaweb.Maui.Android.Controls.TopAppBar;

/// <summary>
/// TopAppBar M3: Small / CenterAligned (64dp) / Medium (112dp) / Large (152dp).
/// Navegación con glyph M3Icons + acciones (típicamente M3IconButton).
/// El colapso con scroll de Medium/Large se deja como mejora futura.
/// </summary>
[ContentProperty(nameof(Actions))]
public partial class M3TopAppBar : ContentView
{
    /// <summary>Propiedad enlazable para <see cref="Title"/>.</summary>
    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(nameof(Title), typeof(string), typeof(M3TopAppBar), string.Empty,
            propertyChanged: (b, _, n) =>
            {
                var self = (M3TopAppBar)b;
                self.SmallTitle.Text = (string?)n ?? string.Empty;
                self.BigTitle.Text = (string?)n ?? string.Empty;
            });

    /// <summary>Propiedad enlazable para <see cref="Type"/>.</summary>
    public static readonly BindableProperty TypeProperty =
        BindableProperty.Create(nameof(Type), typeof(M3TopAppBarType), typeof(M3TopAppBar),
            M3TopAppBarType.Small, propertyChanged: (b, _, _) => ((M3TopAppBar)b).UpdateAppearance());

    /// <summary>Propiedad enlazable para <see cref="NavigationGlyph"/>.</summary>
    public static readonly BindableProperty NavigationGlyphProperty =
        BindableProperty.Create(nameof(NavigationGlyph), typeof(string), typeof(M3TopAppBar), string.Empty,
            propertyChanged: (b, _, _) => ((M3TopAppBar)b).UpdateAppearance());

    /// <summary>Propiedad enlazable para <see cref="NavigationCommand"/>.</summary>
    public static readonly BindableProperty NavigationCommandProperty =
        BindableProperty.Create(nameof(NavigationCommand), typeof(ICommand), typeof(M3TopAppBar), null);

    /// <summary>Título (chico siempre, grande en Medium/Large).</summary>
    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    /// <summary>Tipo de barra. Default Small.</summary>
    public M3TopAppBarType Type
    {
        get => (M3TopAppBarType)GetValue(TypeProperty);
        set => SetValue(TypeProperty, value);
    }

    /// <summary>Glyph M3Icons de navegación (ej. Menu, ArrowBack). Vacío = sin botón.</summary>
    public string NavigationGlyph
    {
        get => (string)GetValue(NavigationGlyphProperty);
        set => SetValue(NavigationGlyphProperty, value);
    }

    /// <summary>Comando del botón de navegación (sin parámetros).</summary>
    public ICommand? NavigationCommand
    {
        get => (ICommand?)GetValue(NavigationCommandProperty);
        set => SetValue(NavigationCommandProperty, value);
    }

    /// <summary>Acciones a la derecha (típicamente M3IconButton). Hijos XAML van aquí.</summary>
    public ObservableCollection<View> Actions { get; } = new();

    /// <summary>Se dispara al tocar el botón de navegación.</summary>
    public event EventHandler? NavigationClicked;

    /// <summary>Crea una nueva instancia.</summary>
    public M3TopAppBar()
    {
        InitializeComponent();
        Actions.CollectionChanged += OnActionsChanged;
        NavButton.Clicked += (_, _) =>
        {
            NavigationCommand?.Execute(null);
            NavigationClicked?.Invoke(this, EventArgs.Empty);
        };
        SmallTitle.Text = Title;
        BigTitle.Text = Title;
        UpdateAppearance();
    }

    private void OnActionsChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        ActionsHost.Children.Clear();
        foreach (var a in Actions)
            ActionsHost.Children.Add(a);
    }

    private void UpdateAppearance()
    {
        M3ControlHelper.SetThemed(Root, BackgroundColorProperty, "M3Surface", "M3DarkSurface");
        M3ControlHelper.SetThemed(SmallTitle, Label.TextColorProperty, "M3OnSurface", "M3DarkOnSurface");
        M3ControlHelper.SetThemed(BigTitle, Label.TextColorProperty, "M3OnSurface", "M3DarkOnSurface");

        NavButton.Glyph = NavigationGlyph;
        NavButton.IsVisible = !string.IsNullOrEmpty(NavigationGlyph);

        Toolbar.HeightRequest = M3ControlHelper.ResDouble("M3AppBarSmallHeight", 64);
        switch (Type)
        {
            case M3TopAppBarType.CenterAligned:
                SmallTitle.HorizontalOptions = LayoutOptions.Center;
                SmallTitle.HorizontalTextAlignment = TextAlignment.Center;
                BigTitle.IsVisible = false;
                break;
            case M3TopAppBarType.Medium:
                SmallTitle.IsVisible = false;
                BigTitle.IsVisible = true;
                BigTitle.FontSize = M3ControlHelper.ResDouble("M3TitleLarge", 22);
                break;
            case M3TopAppBarType.Large:
                SmallTitle.IsVisible = false;
                BigTitle.IsVisible = true;
                BigTitle.FontSize = M3ControlHelper.ResDouble("M3HeadlineMedium", 28);
                break;
            default: // Small
                SmallTitle.IsVisible = true;
                SmallTitle.HorizontalOptions = LayoutOptions.Start;
                SmallTitle.HorizontalTextAlignment = TextAlignment.Start;
                BigTitle.IsVisible = false;
                break;
        }
    }
}
