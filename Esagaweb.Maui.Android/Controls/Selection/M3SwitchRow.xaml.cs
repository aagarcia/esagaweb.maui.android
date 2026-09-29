using System.Windows.Input;
using Esagaweb.Maui.Android.Controls.Common;

namespace Esagaweb.Maui.Android.Controls.Selection;

/// <summary>
/// Row with a label and a themed switch.
/// </summary>
/// <remarks>
/// Uses theme resources for on-surface text, primary active color, and disabled opacity.
/// </remarks>
/// <example>
/// <code lang="XAML">
/// <![CDATA[
/// <ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
///              xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
///              xmlns:sel="clr-namespace:Esagaweb.Maui.Android.Controls.Selection;assembly=Esagaweb.Maui.Android">
///     <sel:M3SwitchRow Text="News and offers" IsOn="True" />
/// </ContentPage>
/// ]]>
/// </code>
/// </example>
public partial class M3SwitchRow : ContentView
{
    /// <summary>
    /// Identifies the <see cref="Text"/> bindable property.
    /// </summary>
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(M3SwitchRow), string.Empty,
            propertyChanged: (b, _, n) => ((M3SwitchRow)b).RowLabel.Text = (string?)n ?? string.Empty);

    /// <summary>
    /// Identifies the <see cref="IsOn"/> bindable property.
    /// </summary>
    public static readonly BindableProperty IsOnProperty =
        BindableProperty.Create(nameof(IsOn), typeof(bool), typeof(M3SwitchRow), false,
            BindingMode.TwoWay, propertyChanged: (b, _, n) => ((M3SwitchRow)b).InnerSwitch.IsToggled = (bool)n);

    /// <summary>
    /// Identifies the <see cref="Command"/> bindable property.
    /// </summary>
    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(M3SwitchRow), null);

    /// <summary>
    /// Gets or sets the row label. The default is an empty string.
    /// </summary>
    /// <remarks>
    /// Bindable property. Changing the value updates the inner label.
    /// </remarks>
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the switch is on. The default is false.
    /// </summary>
    /// <remarks>
    /// Bindable property. Supports two-way binding. Changing the value updates the inner switch.
    /// </remarks>
    public bool IsOn
    {
        get => (bool)GetValue(IsOnProperty);
        set => SetValue(IsOnProperty, value);
    }

    /// <summary>
    /// Gets or sets the command run on toggle, with the new boolean value. The default is null.
    /// </summary>
    /// <remarks>
    /// Bindable property.
    /// </remarks>
    public ICommand? Command
    {
        get => (ICommand?)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    /// <summary>
    /// Occurs when the switch toggles, with the new value.
    /// </summary>
    public event EventHandler<bool>? Toggled;

    /// <summary>
    /// Initializes a new instance of the <see cref="M3SwitchRow"/> class.
    /// </summary>
    public M3SwitchRow()
    {
        InitializeComponent();
        RowLabel.FontFamily = "NunitoRegular";
        RowLabel.Text = Text;
        InnerSwitch.IsToggled = IsOn;
        InnerSwitch.Toggled += (_, e) =>
        {
            IsOn = e.Value;
            if (Command?.CanExecute(e.Value) == true)
                Command.Execute(e.Value);
            Toggled?.Invoke(this, e.Value);
        };
        ApplyTheme();
    }

    /// <inheritdoc/>
    protected override void OnPropertyChanged(string? propertyName = null)
    {
        base.OnPropertyChanged(propertyName);
        if (propertyName == nameof(IsEnabled))
            ApplyTheme();
    }

    private void ApplyTheme()
    {
        Root.HeightRequest = M3ControlHelper.ResDouble("M3SelectionRowHeight", 48);
        M3ControlHelper.SetThemed(RowLabel, Label.TextColorProperty, "M3OnSurface", "M3DarkOnSurface");
        M3ControlHelper.SetThemed(InnerSwitch, Switch.OnColorProperty, "M3Primary", "M3DarkPrimary");
        M3ControlHelper.SetThemed(InnerSwitch, Switch.ThumbColorProperty, "M3OnPrimary", "M3DarkOnPrimary");
        Opacity = IsEnabled ? 1 : 0.6;
    }
}
