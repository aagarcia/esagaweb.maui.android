using Esagaweb.Maui.Android.Controls.Common;

namespace Esagaweb.Maui.Android.Controls.Progress;

/// <summary>Progreso circular M3 (indeterminado, color Primary).</summary>
public partial class M3CircularProgress : ContentView
{
    /// <summary>Propiedad enlazable para <see cref="IsRunning"/>.</summary>
    public static readonly BindableProperty IsRunningProperty =
        BindableProperty.Create(nameof(IsRunning), typeof(bool), typeof(M3CircularProgress), true,
            propertyChanged: (b, _, n) => ((M3CircularProgress)b).Spinner.IsRunning = (bool)n);

    /// <summary>Si la animación está activa. Default true.</summary>
    public bool IsRunning
    {
        get => (bool)GetValue(IsRunningProperty);
        set => SetValue(IsRunningProperty, value);
    }

    /// <summary>Crea una nueva instancia.</summary>
    public M3CircularProgress()
    {
        InitializeComponent();
        Spinner.IsRunning = IsRunning;
        M3ControlHelper.SetThemed(Spinner, ActivityIndicator.ColorProperty, "M3Primary", "M3DarkPrimary");
    }
}
