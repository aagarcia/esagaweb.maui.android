using Esagaweb.Maui.Android.Controls.Common;

namespace Esagaweb.Maui.Android.Controls.Badge;

/// <summary>
/// Insignia M3 (como Flutter Badge): contador sobre iconos (se superpone con
/// Grid en uso) o punto. Count=0 la oculta salvo <see cref="ShowZero"/>.
/// </summary>
public partial class M3Badge : ContentView
{
    /// <summary>Propiedad enlazable para <see cref="Count"/>.</summary>
    public static readonly BindableProperty CountProperty =
        BindableProperty.Create(nameof(Count), typeof(int), typeof(M3Badge), 0,
            propertyChanged: (b, _, _) => ((M3Badge)b).UpdateAppearance());

    /// <summary>Propiedad enlazable para <see cref="MaxCount"/>.</summary>
    public static readonly BindableProperty MaxCountProperty =
        BindableProperty.Create(nameof(MaxCount), typeof(int), typeof(M3Badge), 99,
            propertyChanged: (b, _, _) => ((M3Badge)b).UpdateAppearance());

    /// <summary>Propiedad enlazable para <see cref="ShowZero"/>.</summary>
    public static readonly BindableProperty ShowZeroProperty =
        BindableProperty.Create(nameof(ShowZero), typeof(bool), typeof(M3Badge), false,
            propertyChanged: (b, _, _) => ((M3Badge)b).UpdateAppearance());

    /// <summary>Propiedad enlazable para <see cref="IsDot"/>.</summary>
    public static readonly BindableProperty IsDotProperty =
        BindableProperty.Create(nameof(IsDot), typeof(bool), typeof(M3Badge), false,
            propertyChanged: (b, _, _) => ((M3Badge)b).UpdateAppearance());

    /// <summary>Cantidad a mostrar. 0 oculta la insignia salvo <see cref="ShowZero"/>.</summary>
    public int Count
    {
        get => (int)GetValue(CountProperty);
        set => SetValue(CountProperty, value);
    }

    /// <summary>Tope: por encima muestra "{MaxCount}+". Default 99.</summary>
    public int MaxCount
    {
        get => (int)GetValue(MaxCountProperty);
        set => SetValue(MaxCountProperty, value);
    }

    /// <summary>Si muestra el 0 en vez de ocultarse. Default false.</summary>
    public bool ShowZero
    {
        get => (bool)GetValue(ShowZeroProperty);
        set => SetValue(ShowZeroProperty, value);
    }

    /// <summary>Modo punto (sin número). Default false.</summary>
    public bool IsDot
    {
        get => (bool)GetValue(IsDotProperty);
        set => SetValue(IsDotProperty, value);
    }

    /// <summary>Crea una nueva instancia.</summary>
    public M3Badge()
    {
        InitializeComponent();
        CountLabel.FontFamily = "NunitoSemiBold";
        UpdateAppearance();
    }

    private void UpdateAppearance()
    {
        IsVisible = IsDot || ShowZero || Count > 0;
        if (!IsVisible) return;
        CountLabel.Text = IsDot ? string.Empty : Count > MaxCount ? $"{MaxCount}+" : $"{Count}";
        CountLabel.IsVisible = !IsDot;
        Pill.Padding = IsDot ? new Thickness(0) : new Thickness(4, 0);
        Pill.WidthRequest = IsDot ? 8 : -1;
        Pill.HeightRequest = IsDot ? 8 : M3ControlHelper.ResDouble("M3BadgeSize", 16);
        M3ControlHelper.SetThemed(Pill, Border.BackgroundColorProperty, "M3Error", "M3DarkError");
        M3ControlHelper.SetThemed(CountLabel, Label.TextColorProperty, "M3OnError", "M3DarkOnPrimary");
    }
}
