using System.Windows.Input;
using Esagaweb.Maui.Android.Controls.Common;

namespace Esagaweb.Maui.Android.Controls.Selection;

/// <summary>
/// Row with a themed check box and a label. Tapping the row toggles the value.
/// </summary>
/// <remarks>
/// Uses theme resources for on-surface text and primary check box colors.
/// </remarks>
/// <example>
/// <code lang="XAML">
/// <![CDATA[
/// <ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
///              xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
///              xmlns:sel="clr-namespace:Esagaweb.Maui.Android.Controls.Selection;assembly=Esagaweb.Maui.Android">
///     <sel:M3CheckBoxRow Text="Receive promotional email" IsChecked="True" />
/// </ContentPage>
/// ]]>
/// </code>
/// </example>
public partial class M3CheckBoxRow : ContentView
{
    /// <summary>
    /// Identifies the <see cref="Text"/> bindable property.
    /// </summary>
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(M3CheckBoxRow), string.Empty,
            propertyChanged: (b, _, n) => ((M3CheckBoxRow)b).RowLabel.Text = (string?)n ?? string.Empty);

    /// <summary>
    /// Identifies the <see cref="IsChecked"/> bindable property.
    /// </summary>
    public static readonly BindableProperty IsCheckedProperty =
        BindableProperty.Create(nameof(IsChecked), typeof(bool), typeof(M3CheckBoxRow), false,
            BindingMode.TwoWay, propertyChanged: (b, _, n) => ((M3CheckBoxRow)b).InnerCheck.IsChecked = (bool)n);

    /// <summary>
    /// Identifies the <see cref="Command"/> bindable property.
    /// </summary>
    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(M3CheckBoxRow), null);

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
    /// Gets or sets a value indicating whether the row is checked. The default is false.
    /// </summary>
    /// <remarks>
    /// Bindable property. Supports two-way binding. Changing the value updates the inner check box.
    /// </remarks>
    public bool IsChecked
    {
        get => (bool)GetValue(IsCheckedProperty);
        set => SetValue(IsCheckedProperty, value);
    }

    /// <summary>
    /// Gets or sets the command run on change, with the new boolean value. The default is null.
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
    /// Occurs when the checked state changes, with the new value.
    /// </summary>
    public event EventHandler<bool>? CheckedChanged;

    /// <summary>
    /// Initializes a new instance of the <see cref="M3CheckBoxRow"/> class.
    /// </summary>
    public M3CheckBoxRow()
    {
        InitializeComponent();
        RowLabel.FontFamily = "NunitoRegular";
        RowLabel.Text = Text;
        InnerCheck.IsChecked = IsChecked;
        InnerCheck.CheckedChanged += (_, e) =>
        {
            IsChecked = e.Value;
            if (Command?.CanExecute(e.Value) == true)
                Command.Execute(e.Value);
            CheckedChanged?.Invoke(this, e.Value);
        };
        Root.GestureRecognizers.Add(new TapGestureRecognizer
        {
            Command = new Command(() => InnerCheck.IsChecked = !InnerCheck.IsChecked)
        });
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
        M3ControlHelper.SetThemed(InnerCheck, CheckBox.ColorProperty, "M3Primary", "M3DarkPrimary");
        Opacity = IsEnabled ? 1 : 0.6;
    }
}
