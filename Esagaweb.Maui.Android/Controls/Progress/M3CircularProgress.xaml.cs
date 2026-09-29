using Esagaweb.Maui.Android.Controls.Common;

namespace Esagaweb.Maui.Android.Controls.Progress;

/// <summary>
/// Circular indeterminate progress indicator.
/// </summary>
/// <remarks>
/// Uses theme resources for the primary color.
/// </remarks>
/// <example>
/// <code lang="XAML">
/// <![CDATA[
/// <ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
///              xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
///              xmlns:prog="clr-namespace:Esagaweb.Maui.Android.Controls.Progress;assembly=Esagaweb.Maui.Android">
///     <prog:M3CircularProgress IsRunning="True" />
/// </ContentPage>
/// ]]>
/// </code>
/// </example>
public partial class M3CircularProgress : ContentView
{
    /// <summary>
    /// Identifies the <see cref="IsRunning"/> bindable property.
    /// </summary>
    public static readonly BindableProperty IsRunningProperty =
        BindableProperty.Create(nameof(IsRunning), typeof(bool), typeof(M3CircularProgress), true,
            propertyChanged: (b, _, n) => ((M3CircularProgress)b).Spinner.IsRunning = (bool)n);

    /// <summary>
    /// Gets or sets a value indicating whether the animation runs. The default is true.
    /// </summary>
    /// <remarks>
    /// Bindable property. Changing the value updates the inner indicator.
    /// </remarks>
    public bool IsRunning
    {
        get => (bool)GetValue(IsRunningProperty);
        set => SetValue(IsRunningProperty, value);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="M3CircularProgress"/> class.
    /// </summary>
    public M3CircularProgress()
    {
        InitializeComponent();
        Spinner.IsRunning = IsRunning;
        M3ControlHelper.SetThemed(Spinner, ActivityIndicator.ColorProperty, "M3Primary", "M3DarkPrimary");
    }
}
