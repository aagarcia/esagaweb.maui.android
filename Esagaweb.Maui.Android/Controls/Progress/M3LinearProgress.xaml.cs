using Esagaweb.Maui.Android.Controls.Common;

namespace Esagaweb.Maui.Android.Controls.Progress;

/// <summary>
/// Linear determinate progress indicator with values from 0 to 1.
/// </summary>
/// <remarks>
/// Uses theme resources for the track and bar colors.
/// </remarks>
/// <example>
/// <code lang="XAML">
/// <![CDATA[
/// <ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
///              xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
///              xmlns:prog="clr-namespace:Esagaweb.Maui.Android.Controls.Progress;assembly=Esagaweb.Maui.Android">
///     <prog:M3LinearProgress Value="0.5" />
/// </ContentPage>
/// ]]>
/// </code>
/// </example>
public partial class M3LinearProgress : ContentView
{
    /// <summary>
    /// Identifies the <see cref="Value"/> bindable property.
    /// </summary>
    public static readonly BindableProperty ValueProperty =
        BindableProperty.Create(nameof(Value), typeof(double), typeof(M3LinearProgress), 0.0,
            propertyChanged: (b, _, n) => ((M3LinearProgress)b).Bar.Progress = (double)n);

    /// <summary>
    /// Gets or sets the progress from 0 to 1. The default is 0.0.
    /// </summary>
    /// <remarks>
    /// Bindable property. Changing the value updates the inner bar.
    /// </remarks>
    public double Value
    {
        get => (double)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="M3LinearProgress"/> class.
    /// </summary>
    public M3LinearProgress()
    {
        InitializeComponent();
        Bar.Progress = Value;
        M3ControlHelper.SetThemed(Bar, ProgressBar.ProgressColorProperty, "M3Primary", "M3DarkPrimary");
        M3ControlHelper.SetThemed(this, BackgroundColorProperty, "M3SurfaceVariant", "M3DarkSurfaceVariant");
    }
}
