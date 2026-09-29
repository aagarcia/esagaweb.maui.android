using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Esagaweb.Maui.Android.Controls.Common;
using Esagaweb.Maui.Android.Helpers;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Layouts;

namespace Esagaweb.Maui.Android.Controls.NavigationBar;

/// <summary>
/// Navigation bar with an indicator pill, an icon, and a label for three to five destinations.
/// </summary>
/// <remarks>
/// Uses a horizontal bar in compact and medium widths and a vertical rail in expanded widths.
/// The parent control decides the position and calls <see cref="ApplyBreakpoint(double)"/> to update the orientation.
/// Uses theme resources for surface, secondary container, and on-surface colors.
/// </remarks>
/// <example>
/// <code lang="XAML">
/// <![CDATA[
/// <ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
///              xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
///              xmlns:nav="clr-namespace:Esagaweb.Maui.Android.Controls.NavigationBar;assembly=Esagaweb.Maui.Android"
///              xmlns:common="clr-namespace:Esagaweb.Maui.Android.Controls.Common;assembly=Esagaweb.Maui.Android">
///     <nav:M3NavigationBar SelectedIndex="0">
///         <nav:M3NavigationItem Label="Home"
///                               Glyph="{x:Static common:M3Icons.Home}"
///                               SelectedGlyph="{x:Static common:M3Icons.HomeFilled}" />
///         <nav:M3NavigationItem Label="Search"
///                               Glyph="{x:Static common:M3Icons.Search}" />
///         <nav:M3NavigationItem Label="Settings"
///                               Glyph="{x:Static common:M3Icons.Settings}" />
///     </nav:M3NavigationBar>
/// </ContentPage>
/// ]]>
/// </code>
/// </example>
[ContentProperty(nameof(Destinations))]
public partial class M3NavigationBar : ContentView
{
    /// <summary>
    /// Identifies the <see cref="SelectedIndex"/> bindable property.
    /// </summary>
    public static readonly BindableProperty SelectedIndexProperty =
        BindableProperty.Create(nameof(SelectedIndex), typeof(int), typeof(M3NavigationBar), 0,
            BindingMode.TwoWay,
            propertyChanged: (b, _, _) => ((M3NavigationBar)b).RebuildItems());

    /// <summary>
    /// Gets the navigation destinations.
    /// </summary>
    /// <remarks>
    /// Add three to five <see cref="M3NavigationItem"/> objects. XAML children go here.
    /// </remarks>
    public ObservableCollection<M3NavigationItem> Destinations { get; } = new();

    /// <summary>
    /// Gets or sets the selected index. The default is 0.
    /// </summary>
    /// <remarks>
    /// Bindable property. Supports two-way binding. Changing the value rebuilds the items.
    /// </remarks>
    public int SelectedIndex
    {
        get => (int)GetValue(SelectedIndexProperty);
        set => SetValue(SelectedIndexProperty, value);
    }

    /// <summary>
    /// Occurs when the selection changes, with the new index.
    /// </summary>
    public event EventHandler<int>? SelectionChanged;

    private bool _isRail;

    /// <summary>
    /// Initializes a new instance of the <see cref="M3NavigationBar"/> class.
    /// </summary>
    public M3NavigationBar()
    {
        InitializeComponent();
        Destinations.CollectionChanged += (_, _) => RebuildItems();
        M3ControlHelper.SetThemed(this, BackgroundColorProperty, "M3Surface", "M3DarkSurface");
        RebuildItems();
    }

    /// <summary>
    /// Applies the responsive layout for the specified width and rebuilds the items when the mode changes.
    /// </summary>
    /// <param name="widthDp">The available width in density-independent pixels. Values less than or equal to zero are ignored.</param>
    public void ApplyBreakpoint(double widthDp)
    {
        if (widthDp <= 0)
            return;
        var rail = ResponsiveHelper.GetBreakpoint(widthDp) == M3Breakpoint.Expanded;
        if (rail == _isRail && ItemsHost.Children.Count == Destinations.Count)
            return; // Without a mode change, rebuilding would recreate every cell.
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
