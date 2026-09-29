using Esagaweb.Maui.Android.Controls.Common;
using Microsoft.Maui.Controls.Shapes;

namespace Esagaweb.Maui.Android.Controls.BottomSheet;

/// <summary>
/// Represents a Material 3 modal bottom sheet that displays a title with custom content anchored to the bottom of the screen.
/// </summary>
/// <remarks>
/// The sheet can be shown as a full-screen transparent modal with a scrim through <see cref="ShowAsync"/>,
/// or embedded inline like any other view. Child elements declared in XAML are assigned to <see cref="SheetBody"/>.
/// Tapping the scrim calls <see cref="HideAsync"/>, which closes the modal and raises <see cref="Dismissed"/>.
/// An empty <see cref="Title"/> hides the title label. The surface, title and handle colors follow the current
/// theme through <c>M3Surface</c>, <c>M3DarkSurface</c>, <c>M3OnSurface</c>, <c>M3DarkOnSurface</c>,
/// <c>M3Outline</c> and <c>M3DarkOutline</c>. The host application must register fonts with <c>UseEsagaweb</c>
/// so the NunitoSemiBold font resolves.
/// </remarks>
/// <example>
/// <code language="csharp">
/// <![CDATA[
/// var options = new VerticalStackLayout();
/// options.Children.Add(new Label { Text = "Small" });
/// var sheet = new M3BottomSheet { Title = "Choose a size", SheetBody = options };
/// await sheet.ShowAsync(Navigation);
/// ]]>
/// </code>
/// </example>
[ContentProperty(nameof(SheetBody))]
public partial class M3BottomSheet : ContentView
{
    /// <summary>
    /// Identifies the <see cref="Title"/> bindable property.
    /// </summary>
    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(nameof(Title), typeof(string), typeof(M3BottomSheet), string.Empty,
            propertyChanged: (b, _, n) =>
            {
                var self = (M3BottomSheet)b;
                self.TitleLabel.Text = (string?)n ?? string.Empty;
                self.TitleLabel.IsVisible = !string.IsNullOrEmpty((string?)n);
            });

    /// <summary>
    /// Gets or sets the title displayed above the sheet content. An empty value hides the title. The default is an empty string.
    /// </summary>
    /// <remarks>
    /// This is a bindable property.
    /// </remarks>
    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    /// <summary>
    /// Gets or sets the content displayed inside the sheet. Child elements declared in XAML are assigned here. The default is <see langword="null"/>.
    /// </summary>
    public View? SheetBody
    {
        get => SheetContent.Content;
        set => SheetContent.Content = value;
    }

    /// <summary>
    /// Occurs when the sheet is closed by any means.
    /// </summary>
    public event EventHandler? Dismissed;

    /// <summary>
    /// Initializes a new instance of the <see cref="M3BottomSheet"/> class.
    /// </summary>
    public M3BottomSheet()
    {
        InitializeComponent();
        TitleLabel.FontFamily = "NunitoSemiBold";
        TitleLabel.IsVisible = !string.IsNullOrEmpty(Title);
        ApplyTheme();
    }

    private Page? _hostPage;

    /// <summary>
    /// Shows the sheet as a bottom-anchored modal with a scrim. A task that completes when the modal page has been presented.
    /// </summary>
    /// <param name="navigation">The navigation service used to present the modal page.</param>
    public async Task ShowAsync(INavigation navigation)
    {
        var scrim = new BoxView { Color = Color.FromArgb("#52000000") };
        scrim.GestureRecognizers.Add(new TapGestureRecognizer
        {
            Command = new Command(async () => await HideAsync())
        });
        // Detach from a previous parent before adding to the host, or it would be removed from its own host page.
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

    /// <summary>
    /// Hides the modal sheet if it is visible and raises <see cref="Dismissed"/>. A task that completes when the modal page has been dismissed.
    /// </summary>
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
