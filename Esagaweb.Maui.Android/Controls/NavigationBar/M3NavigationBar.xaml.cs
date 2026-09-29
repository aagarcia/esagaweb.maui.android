using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Esagaweb.Maui.Android.Controls.Common;
using Esagaweb.Maui.Android.Helpers;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Layouts;

namespace Esagaweb.Maui.Android.Controls.NavigationBar;

/// <summary>
/// NavigationBar M3 con 3-5 destinos: pastilla indicadora + icono + etiqueta.
/// Responsive: Compact/Medium = barra inferior (80dp); Expanded = rail lateral (80dp).
/// El padre decide la posición; la barra ajusta orientación con ApplyBreakpoint.
/// </summary>
[ContentProperty(nameof(Destinations))]
public partial class M3NavigationBar : ContentView
{
    /// <summary>Propiedad enlazable para <see cref="SelectedIndex"/>.</summary>
    public static readonly BindableProperty SelectedIndexProperty =
        BindableProperty.Create(nameof(SelectedIndex), typeof(int), typeof(M3NavigationBar), 0,
            BindingMode.TwoWay,
            propertyChanged: (b, _, _) => ((M3NavigationBar)b).RebuildItems());

    /// <summary>Destinos (3-5 M3NavigationItem). Hijos XAML van aquí.</summary>
    public ObservableCollection<M3NavigationItem> Destinations { get; } = new();

    /// <summary>Índice seleccionado. TwoWay: sirve para ViewModels (ej. tabs desde API).</summary>
    public int SelectedIndex
    {
        get => (int)GetValue(SelectedIndexProperty);
        set => SetValue(SelectedIndexProperty, value);
    }

    /// <summary>Se dispara al cambiar la selección (con el nuevo índice).</summary>
    public event EventHandler<int>? SelectionChanged;

    private bool _isRail;

    /// <summary>Crea una nueva instancia.</summary>
    public M3NavigationBar()
    {
        InitializeComponent();
        Destinations.CollectionChanged += (_, _) => RebuildItems();
        M3ControlHelper.SetThemed(this, BackgroundColorProperty, "M3Surface", "M3DarkSurface");
        RebuildItems();
    }

    /// <summary>Responsive: Expanded = rail vertical, resto = barra horizontal.</summary>
    public void ApplyBreakpoint(double widthDp)
    {
        if (widthDp <= 0)
            return;
        var rail = ResponsiveHelper.GetBreakpoint(widthDp) == M3Breakpoint.Expanded;
        if (rail == _isRail && ItemsHost.Children.Count == Destinations.Count)
            return; // Sin cambio de modo: reconstruir recrearía todas las celdas y realimenta SizeChanged.
        _isRail = rail;
        RebuildItems();
    }

    private void RebuildItems()
    {
        ItemsHost.Children.Clear();
        ItemsHost.Direction = _isRail ? FlexDirection.Column : FlexDirection.Row;
        ItemsHost.JustifyContent = _isRail ? FlexJustify.Start : FlexJustify.SpaceAround;
        ItemsHost.AlignItems = FlexAlignItems.Center;
        ItemsHost.Padding = _isRail ? new Thickness(0, 12) : new Thickness(0);
        if (!_isRail)
            ItemsHost.HeightRequest = M3ControlHelper.ResDouble("M3NavBarHeight", 80);
        else
            ItemsHost.HeightRequest = -1;

        var selected = Math.Clamp(SelectedIndex, 0, Math.Max(0, Destinations.Count - 1));
        for (var i = 0; i < Destinations.Count; i++)
        {
            var dest = Destinations[i];
            var index = i;
            var active = i == selected;

            var glyph = new Label
            {
                FontFamily = "MaterialSymbols",
                FontSize = 24,
                Text = active && !string.IsNullOrEmpty(dest.SelectedGlyph) ? dest.SelectedGlyph : dest.Glyph,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };
            var pill = new Border
            {
                HeightRequest = 32,
                WidthRequest = 64,
                StrokeThickness = 0,
                StrokeShape = new RoundRectangle { CornerRadius = 16 },
                Content = glyph,
                HorizontalOptions = LayoutOptions.Center
            };
            var label = new Label
            {
                Text = dest.Label,
                FontFamily = "NunitoSemiBold",
                FontSize = 12,
                HorizontalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center
            };

            if (active)
            {
                M3ControlHelper.SetThemed(pill, BackgroundColorProperty, "M3SecondaryContainer", "M3DarkPrimaryContainer");
                M3ControlHelper.SetThemed(glyph, Label.TextColorProperty, "M3OnPrimaryContainer", "M3DarkPrimary");
                M3ControlHelper.SetThemed(label, Label.TextColorProperty, "M3OnSurface", "M3DarkOnSurface");
            }
            else
            {
                pill.BackgroundColor = Colors.Transparent;
                M3ControlHelper.SetThemed(glyph, Label.TextColorProperty, "M3OnSurfaceVariant", "M3DarkOnSurface");
                M3ControlHelper.SetThemed(label, Label.TextColorProperty, "M3OnSurfaceVariant", "M3DarkOnSurface");
            }

            var cell = new VerticalStackLayout
            {
                Spacing = 4,
                WidthRequest = 80,
                Margin = _isRail ? new Thickness(0, 6) : new Thickness(0),
                Children = { pill, label }
            };
            cell.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    SelectedIndex = index;
                    dest.Command?.Execute(null);
                    SelectionChanged?.Invoke(this, index);
                })
            });
            ItemsHost.Children.Add(cell);
        }
    }
}
