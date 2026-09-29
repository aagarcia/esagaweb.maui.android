using System.Windows.Input;
using Esagaweb.Maui.Android.Controls.Common;

namespace Esagaweb.Maui.Android.Controls.IconButtons;

/// <summary>Icon-only button for compact toolbar and card actions.</summary>
/// <remarks>Supports Standard, Filled, Tonal, and Outlined variants with automatic light and dark theme colors. Shows a reduced opacity treatment when disabled. Register fonts and theme dictionaries with the setup method before use.</remarks>
/// <example>
/// <code><![CDATA[
/// <ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
///              xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
///              xmlns:iconbtn="clr-namespace:Esagaweb.Maui.Android.Controls.IconButtons;assembly=Esagaweb.Maui.Android"
///              xmlns:common="clr-namespace:Esagaweb.Maui.Android.Controls.Common;assembly=Esagaweb.Maui.Android">
///     <iconbtn:M3IconButton Variant="Standard"
///                           Glyph="{x:Static common:M3Icons.Search}"
///                           Clicked="OnSearchClicked" />
/// </ContentPage>
/// ]]></code>
/// </example>
public partial class M3IconButton : ContentView
{
    /// <summary>Identifies the <see cref="Glyph"/> bindable property.</summary>
    public static readonly BindableProperty GlyphProperty =
        BindableProperty.Create(nameof(Glyph), typeof(string), typeof(M3IconButton), string.Empty,
            propertyChanged: (b, _, _) => ((M3IconButton)b).UpdateIcon());

    /// <summary>Identifies the <see cref="Icon"/> bindable property.</summary>
    public static readonly BindableProperty IconProperty =
        BindableProperty.Create(nameof(Icon), typeof(ImageSource), typeof(M3IconButton), null,
            propertyChanged: (b, _, n) =>
            {
                var self = (M3IconButton)b;
                self.ButtonImage.Source = (ImageSource?)n;
                self.UpdateIcon();
            });

    /// <summary>Identifies the <see cref="Variant"/> bindable property.</summary>
    public static readonly BindableProperty VariantProperty =
        BindableProperty.Create(nameof(Variant), typeof(M3IconButtonVariant), typeof(M3IconButton),
            M3IconButtonVariant.Standard, propertyChanged: (b, _, _) => ((M3IconButton)b).UpdateAppearance());

    /// <summary>Identifies the <see cref="Command"/> bindable property.</summary>
    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(M3IconButton), null);

    /// <summary>Identifies the <see cref="CommandParameter"/> bindable property.</summary>
    public static readonly BindableProperty CommandParameterProperty =
        BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(M3IconButton), null);

    /// <summary>Gets or sets the Material Symbols glyph shown on the button. Takes priority over <see cref="Icon"/>. The default is an empty string.</summary>
    /// <remarks>This is a bindable property.</remarks>
    public string Glyph
    {
        get => (string)GetValue(GlyphProperty);
        set => SetValue(GlyphProperty, value);
    }

    /// <summary>Gets or sets the image shown when <see cref="Glyph"/> is empty. The default is null.</summary>
    /// <remarks>This is a bindable property.</remarks>
    public ImageSource? Icon
    {
        get => (ImageSource?)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    /// <summary>Gets or sets the visual variant. The default is Standard.</summary>
    /// <remarks>This is a bindable property.</remarks>
    public M3IconButtonVariant Variant
    {
        get => (M3IconButtonVariant)GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
    }

    /// <summary>Gets or sets the command invoked when the button is tapped. It receives <see cref="CommandParameter"/> and executes only when <see cref="ICommand.CanExecute"/> returns <see langword="true"/> for that parameter. The default is <see langword="null"/>.</summary>
    /// <remarks>This is a bindable property.</remarks>
    public ICommand? Command
    {
        get => (ICommand?)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    /// <summary>Gets or sets the parameter passed to <see cref="Command"/>. The default is <see langword="null"/>.</summary>
    /// <remarks>This is a bindable property.</remarks>
    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    /// <summary>Initializes a new instance of the <see cref="M3IconButton"/> class.</summary>
    public M3IconButton()
    {
        InitializeComponent();
        GlyphLabel.FontFamily = "MaterialSymbols";
        UpdateIcon();
        UpdateAppearance();
    }

    /// <inheritdoc/>
    protected override void OnPropertyChanged(string? propertyName = null)
    {
        base.OnPropertyChanged(propertyName);
        if (propertyName == nameof(IsEnabled))
            UpdateAppearance();
    }

    private void UpdateIcon()
    {
        GlyphLabel.FontFamily = "MaterialSymbols";
        var hasGlyph = !string.IsNullOrEmpty(Glyph);
        GlyphLabel.Text = Glyph;
        GlyphLabel.IsVisible = hasGlyph;
        ButtonImage.IsVisible = !hasGlyph && ButtonImage.Source is not null;
    }

    private void SetContentThemed(string lightKey, string darkKey)
        => M3ControlHelper.SetThemed(GlyphLabel, Label.TextColorProperty, lightKey, darkKey);

    private void UpdateAppearance()
    {
        if (!IsEnabled)
        {
            RootBorder.Opacity = 0.38;
            SetContentThemed("M3OnSurfaceVariant", "M3DarkOnSurface");
            return;
        }
        RootBorder.Opacity = 1;

        switch (Variant)
        {
            case M3IconButtonVariant.Filled:
                M3ControlHelper.SetThemed(RootBorder, Border.BackgroundColorProperty, "M3Primary", "M3DarkPrimary");
                RootBorder.Stroke = Colors.Transparent;
                SetContentThemed("M3OnPrimary", "M3DarkOnPrimary");
                break;
            case M3IconButtonVariant.Tonal:
                M3ControlHelper.SetThemed(RootBorder, Border.BackgroundColorProperty, "M3SecondaryContainer", "M3DarkPrimaryContainer");
                RootBorder.Stroke = Colors.Transparent;
                SetContentThemed("M3OnPrimaryContainer", "M3DarkPrimary");
                break;
            case M3IconButtonVariant.Outlined:
                RootBorder.BackgroundColor = Colors.Transparent;
                M3ControlHelper.SetThemed(RootBorder, Border.StrokeProperty, "M3Outline", "M3DarkOutline");
                SetContentThemed("M3Primary", "M3DarkPrimary");
                break;
            default: // Standard
                RootBorder.BackgroundColor = Colors.Transparent;
                RootBorder.Stroke = Colors.Transparent;
                SetContentThemed("M3Primary", "M3DarkPrimary");
                break;
        }
    }

    private void OnTapped(object? sender, TappedEventArgs e)
    {
        if (!IsEnabled) return;
        if (Command?.CanExecute(CommandParameter) == true)
            Command.Execute(CommandParameter);
        Clicked?.Invoke(this, EventArgs.Empty);
    }

    private void OnPointerPressed(object? sender, PointerEventArgs e) => RootBorder.Opacity = 0.7;
    private void OnPointerReleased(object? sender, PointerEventArgs e) => RootBorder.Opacity = IsEnabled ? 1 : 0.38;

    /// <summary>Occurs when the button is tapped and the button is enabled.</summary>
    public event EventHandler? Clicked;
}
