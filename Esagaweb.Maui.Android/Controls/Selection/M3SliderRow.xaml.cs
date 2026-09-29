using Esagaweb.Maui.Android.Controls.Common;

namespace Esagaweb.Maui.Android.Controls.Selection;

/// <summary>
/// Row with a label, a numeric value, and a themed slider.
/// </summary>
/// <remarks>
/// Uses theme resources for on-surface text, primary value text, and slider track colors.
/// </remarks>
/// <example>
/// <code lang="XAML">
/// <![CDATA[
/// <ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
///              xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
///              xmlns:sel="clr-namespace:Esagaweb.Maui.Android.Controls.Selection;assembly=Esagaweb.Maui.Android">
///     <sel:M3SliderRow Text="Brightness" Minimum="0" Maximum="100" Value="70" />
/// </ContentPage>
/// ]]>
/// </code>
/// </example>
public partial class M3SliderRow : ContentView
{
    /// <summary>
    /// Identifies the <see cref="Text"/> bindable property.
    /// </summary>
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(M3SliderRow), string.Empty,
            propertyChanged: (b, _, n) => ((M3SliderRow)b).RowLabel.Text = (string?)n ?? string.Empty);

    /// <summary>
    /// Identifies the <see cref="Value"/> bindable property.
    /// </summary>
    public static readonly BindableProperty ValueProperty =
        BindableProperty.Create(nameof(Value), typeof(double), typeof(M3SliderRow), 0.0,
            BindingMode.TwoWay, propertyChanged: (b, _, n) =>
            {
                var self = (M3SliderRow)b;
                if (Math.Abs(self.InnerSlider.Value - (double)n) > double.Epsilon)
                    self.InnerSlider.Value = (double)n;
                self.UpdateValueLabel();
            });

    /// <summary>
    /// Identifies the <see cref="Minimum"/> bindable property.
    /// </summary>
    public static readonly BindableProperty MinimumProperty =
        BindableProperty.Create(nameof(Minimum), typeof(double), typeof(M3SliderRow), 0.0,
            propertyChanged: (b, _, n) => ((M3SliderRow)b).InnerSlider.Minimum = (double)n);

    /// <summary>
    /// Identifies the <see cref="Maximum"/> bindable property.
    /// </summary>
    public static readonly BindableProperty MaximumProperty =
        BindableProperty.Create(nameof(Maximum), typeof(double), typeof(M3SliderRow), 100.0,
            propertyChanged: (b, _, n) => ((M3SliderRow)b).InnerSlider.Maximum = (double)n);

    /// <summary>
    /// Identifies the <see cref="ShowValue"/> bindable property.
    /// </summary>
    public static readonly BindableProperty ShowValueProperty =
        BindableProperty.Create(nameof(ShowValue), typeof(bool), typeof(M3SliderRow), true,
            propertyChanged: (b, _, _) => ((M3SliderRow)b).UpdateValueLabel());

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
    /// Gets or sets the current value. The default is 0.0.
    /// </summary>
    /// <remarks>
    /// Bindable property. Supports two-way binding. Changing the value updates the inner slider and the value label.
    /// </remarks>
    public double Value
    {
        get => (double)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    /// <summary>
    /// Gets or sets the minimum value. The default is 0.0.
    /// </summary>
    /// <remarks>
    /// Bindable property. Changing the value updates the inner slider.
    /// </remarks>
    public double Minimum
    {
        get => (double)GetValue(MinimumProperty);
        set => SetValue(MinimumProperty, value);
    }

    /// <summary>
    /// Gets or sets the maximum value. The default is 100.0.
    /// </summary>
    /// <remarks>
    /// Bindable property. Changing the value updates the inner slider.
    /// </remarks>
    public double Maximum
    {
        get => (double)GetValue(MaximumProperty);
        set => SetValue(MaximumProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the numeric value is shown. The default is true.
    /// </summary>
    /// <remarks>
    /// Bindable property. Changing the value updates the value label.
    /// </remarks>
    public bool ShowValue
    {
        get => (bool)GetValue(ShowValueProperty);
        set => SetValue(ShowValueProperty, value);
    }

    /// <summary>
    /// Occurs when the value changes, with the new value.
    /// </summary>
    public event EventHandler<double>? ValueChanged;

    /// <summary>
    /// Initializes a new instance of the <see cref="M3SliderRow"/> class.
    /// </summary>
    public M3SliderRow()
    {
        InitializeComponent();
        RowLabel.FontFamily = "NunitoRegular";
        ValueLabel.FontFamily = "NunitoSemiBold";
        RowLabel.Text = Text;
        InnerSlider.Minimum = Minimum;
        InnerSlider.Maximum = Maximum;
        InnerSlider.Value = Value;
        InnerSlider.ValueChanged += (_, e) =>
        {
            Value = e.NewValue;
            UpdateValueLabel();
            ValueChanged?.Invoke(this, e.NewValue);
        };
        M3ControlHelper.SetThemed(RowLabel, Label.TextColorProperty, "M3OnSurface", "M3DarkOnSurface");
        M3ControlHelper.SetThemed(ValueLabel, Label.TextColorProperty, "M3Primary", "M3DarkPrimary");
        UpdateValueLabel();
    }

    private void UpdateValueLabel()
    {
        ValueLabel.Text = $"{Value:F0}";
        ValueLabel.IsVisible = ShowValue;
    }
}
