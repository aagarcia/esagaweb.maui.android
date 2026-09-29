using Esagaweb.Maui.Android.Controls.Common;

namespace Esagaweb.Maui.Android.Controls.Progress;

/// <summary>Progreso lineal M3 (track SurfaceVariant, barra Primary). Value 0..1.</summary>
public partial class M3LinearProgress : ContentView
{
    /// <summary>Propiedad enlazable para <see cref="Value"/>.</summary>
    public static readonly BindableProperty ValueProperty =
        BindableProperty.Create(nameof(Value), typeof(double), typeof(M3LinearProgress), 0.0,
            propertyChanged: (b, _, n) => ((M3LinearProgress)b).Bar.Progress = (double)n);

    /// <summary>Progreso de 0 a 1. Default 0.</summary>
    public double Value
    {
        get => (double)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    /// <summary>Crea una nueva instancia.</summary>
    public M3LinearProgress()
    {
        InitializeComponent();
        Bar.Progress = Value;
        M3ControlHelper.SetThemed(Bar, ProgressBar.ProgressColorProperty, "M3Primary", "M3DarkPrimary");
        M3ControlHelper.SetThemed(this, BackgroundColorProperty, "M3SurfaceVariant", "M3DarkSurfaceVariant");
    }
}
