using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;
using Esagaweb.Maui.Android.Controls.Common;

namespace Esagaweb.Maui.Android.Controls.TopAppBar;

/// <summary>Material top app bar with navigation glyph, title, and action items.</summary>
/// <remarks>Medium and large types show a second large title row. Scroll collapse is a future improvement.</remarks>
/// <example>
/// <code language="xaml"><![CDATA[
/// <ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
///              xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
///              xmlns:appbar="clr-namespace:Esagaweb.Maui.Android.Controls.TopAppBar;assembly=Esagaweb.Maui.Android">
///     <appbar:M3TopAppBar Title="Store"
///                        Type="Medium" />
///     <appbar:M3TopAppBar Title="Cart"
///                        Type="Small" />
/// </ContentPage>
/// ]]></code>
/// </example>
[ContentProperty(nameof(Actions))]
public partial class M3TopAppBar : ContentView
{
    /// <summary>Identifies the <see cref="Title"/> bindable property.</summary>
    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(nameof(Title), typeof(string), typeof(M3TopAppBar), string.Empty,
            propertyChanged: (b, _, n) =>
            {
                var self = (M3TopAppBar)b;
                self.SmallTitle.Text = (string?)n ?? string.Empty;
                self.BigTitle.Text = (string?)n ?? string.Empty;
            });

    /// <summary>Identifies the <see cref="Type"/> bindable property.</summary>
    public static readonly BindableProperty TypeProperty =
        BindableProperty.Create(nameof(Type), typeof(M3TopAppBarType), typeof(M3TopAppBar),
            M3TopAppBarType.Small, propertyChanged: (b, _, _) => ((M3TopAppBar)b).UpdateAppearance());

    /// <summary>Identifies the <see cref="NavigationGlyph"/> bindable property.</summary>
    public static readonly BindableProperty NavigationGlyphProperty =
        BindableProperty.Create(nameof(NavigationGlyph), typeof(string), typeof(M3TopAppBar), string.Empty,
            propertyChanged: (b, _, _) => ((M3TopAppBar)b).UpdateAppearance());

    /// <summary>Identifies the <see cref="NavigationCommand"/> bindable property.</summary>
    public static readonly BindableProperty NavigationCommandProperty =
        BindableProperty.Create(nameof(NavigationCommand), typeof(ICommand), typeof(M3TopAppBar), null);

    /// <summary>Gets or sets the title. The default is empty.</summary>
    /// <remarks>Bindable property. Changes update the small and large title labels.</remarks>
    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    /// <summary>Gets or sets the bar type. The default is <see cref="M3TopAppBarType.Small"/>.</summary>
    /// <remarks>Bindable property. Changes update the bar appearance.</remarks>
    public M3TopAppBarType Type
    {
        get => (M3TopAppBarType)GetValue(TypeProperty);
        set => SetValue(TypeProperty, value);
    }

    /// <summary>Gets or sets the navigation glyph. Empty means no navigation button. The default is empty.</summary>
    /// <remarks>Bindable property. Changes update the bar appearance.</remarks>
    public string NavigationGlyph
    {
        get => (string)GetValue(NavigationGlyphProperty);
        set => SetValue(NavigationGlyphProperty, value);
    }

    /// <summary>Gets or sets the navigation button command. The default is null.</summary>
    /// <remarks>Bindable property.</remarks>
    public ICommand? NavigationCommand
    {
        get => (ICommand?)GetValue(NavigationCommandProperty);
        set => SetValue(NavigationCommandProperty, value);
    }

    /// <summary>Gets the actions shown on the right side. Child elements in XAML go here.</summary>
    public ObservableCollection<View> Actions { get; } = new();

    /// <summary>Occurs when the navigation button is tapped.</summary>
    public event EventHandler? NavigationClicked;

    /// <summary>Initializes a new instance of the <see cref="M3TopAppBar"/> class.</summary>
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
