using Esagaweb.Maui.Android.Controls.Common;

namespace Esagaweb.Maui.Android.Controls.Selection;

/// <summary>
/// Row with a themed radio button and a label. Tapping the row selects it.
/// </summary>
/// <remarks>
/// Group options with <see cref="GroupName"/> so only one option can be selected.
/// Uses theme resources for on-surface text, outline, and disabled opacity.
/// </remarks>
/// <example>
/// <code lang="XAML">
/// <![CDATA[
/// <ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
///              xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
///              xmlns:sel="clr-namespace:Esagaweb.Maui.Android.Controls.Selection;assembly=Esagaweb.Maui.Android">
///     <sel:M3RadioRow Text="Light" GroupName="Theme" Value="L" IsSelected="True" />
///     <sel:M3RadioRow Text="Dark" GroupName="Theme" Value="D" />
/// </ContentPage>
/// ]]>
/// </code>
/// </example>
public partial class M3RadioRow : ContentView
{
    /// <summary>
    /// Identifies the <see cref="Text"/> bindable property.
    /// </summary>
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(M3RadioRow), string.Empty,
            propertyChanged: (b, _, n) => ((M3RadioRow)b).RowLabel.Text = (string?)n ?? string.Empty);

    /// <summary>
    /// Identifies the <see cref="IsSelected"/> bindable property.
    /// </summary>
    public static readonly BindableProperty IsSelectedProperty =
        BindableProperty.Create(nameof(IsSelected), typeof(bool), typeof(M3RadioRow), false,
            BindingMode.TwoWay, propertyChanged: (b, _, n) => ((M3RadioRow)b).InnerRadio.IsChecked = (bool)n);

    /// <summary>
    /// Identifies the <see cref="GroupName"/> bindable property.
    /// </summary>
    public static readonly BindableProperty GroupNameProperty =
        BindableProperty.Create(nameof(GroupName), typeof(string), typeof(M3RadioRow), string.Empty,
            propertyChanged: (b, _, n) => ((M3RadioRow)b).InnerRadio.GroupName = (string?)n ?? string.Empty);

    /// <summary>
    /// Identifies the <see cref="Value"/> bindable property.
    /// </summary>
    public static readonly BindableProperty ValueProperty =
        BindableProperty.Create(nameof(Value), typeof(object), typeof(M3RadioRow), null);

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
    /// Gets or sets a value indicating whether this option is selected. The default is false.
    /// </summary>
    /// <remarks>
    /// Bindable property. Supports two-way binding. Changing the value updates the inner radio button.
    /// </remarks>
    public bool IsSelected
    {
        get => (bool)GetValue(IsSelectedProperty);
        set => SetValue(IsSelectedProperty, value);
    }

    /// <summary>
    /// Gets or sets the group name. Only one option in the same group can be selected. The default is an empty string.
    /// </summary>
    /// <remarks>
    /// Bindable property. Changing the value updates the inner radio button.
    /// </remarks>
    public string GroupName
    {
        get => (string)GetValue(GroupNameProperty);
        set => SetValue(GroupNameProperty, value);
    }

    /// <summary>
    /// Gets or sets the value associated with this option. The value is used as provided. The default is null.
    /// </summary>
    /// <remarks>
    /// Bindable property.
    /// </remarks>
    public object? Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    /// <summary>
    /// Occurs when the selection changes, with the new value.
    /// </summary>
    public event EventHandler<bool>? SelectedChanged;

    /// <summary>
    /// Initializes a new instance of the <see cref="M3RadioRow"/> class.
    /// </summary>
    public M3RadioRow()
    {
        InitializeComponent();
        RowLabel.FontFamily = "NunitoRegular";
        RowLabel.Text = Text;
        InnerRadio.HandlerChanged += (_, _) => ApplyRadioTint();
        InnerRadio.GroupName = GroupName;
        InnerRadio.IsChecked = IsSelected;
        InnerRadio.CheckedChanged += (_, e) =>
        {
            IsSelected = e.Value;
            SelectedChanged?.Invoke(this, e.Value);
        };
        Root.GestureRecognizers.Add(new TapGestureRecognizer
        {
            Command = new Command(() => InnerRadio.IsChecked = true)
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
        M3ControlHelper.SetThemed(InnerRadio, RadioButton.TextColorProperty, "M3OnSurface", "M3DarkOnSurface");
        M3ControlHelper.SetThemed(InnerRadio, RadioButton.BorderColorProperty, "M3Outline", "M3DarkOutline");
        M3ControlHelper.SetThemed(this, CheckedTintProperty, "M3Primary", "M3DarkPrimary");
        M3ControlHelper.SetThemed(this, UncheckedTintProperty, "M3Outline", "M3DarkOutline");
        Opacity = IsEnabled ? 1 : 0.6;
    }

    // Theme-bound colors for the native radio circle. They are bindable so a light/dark switch
    // re-applies the tint; otherwise Android falls back to the app's colorAccent.
    private static readonly BindableProperty CheckedTintProperty =
        BindableProperty.Create("CheckedTint", typeof(Color), typeof(M3RadioRow), null,
            propertyChanged: (b, _, _) => ((M3RadioRow)b).ApplyRadioTint());

    private static readonly BindableProperty UncheckedTintProperty =
        BindableProperty.Create("UncheckedTint", typeof(Color), typeof(M3RadioRow), null,
            propertyChanged: (b, _, _) => ((M3RadioRow)b).ApplyRadioTint());

    private void ApplyRadioTint()
    {
#if ANDROID
        if (InnerRadio?.Handler?.PlatformView is not global::Android.Widget.CompoundButton radio)
            return;
        if (GetValue(CheckedTintProperty) is not Color on || GetValue(UncheckedTintProperty) is not Color off)
            return;
        radio.ButtonTintList = new global::Android.Content.Res.ColorStateList(
            new[] { new[] { global::Android.Resource.Attribute.StateChecked }, System.Array.Empty<int>() },
            new int[] { on.ToInt(), off.ToInt() });
#endif
    }
}
