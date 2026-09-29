using Esagaweb.Maui.Android.Helpers;

namespace Esagaweb.Maui.Android.Sample.Pages;

/// <summary>Ajustes de la tienda: switches, tema, brillo y guardado con progreso.</summary>
public partial class SettingsPage : ContentPage
{
    private M3Breakpoint? _lastBp;
    private bool _saving;

    public SettingsPage()
    {
        InitializeComponent();
        NewsSwitch.Toggled += (_, v) => RefreshState();
        PromoCheck.CheckedChanged += (_, v) => RefreshState();
        BrightSlider.ValueChanged += (_, _) => RefreshState();
        RefreshState();
        SizeChanged += OnSizeChanged;
    }

    private void RefreshState()
        => StateLabel.Text = $"Estado: novedades={(NewsSwitch.IsOn ? "sí" : "no")}"
            + $" · promos={(PromoCheck.IsChecked ? "sí" : "no")}"
            + $" · brillo={BrightSlider.Value:F0}";

    private async void OnSaveClicked(object? sender, EventArgs e)
    {
        if (_saving)
            return;
        _saving = true;
        SaveProgress.IsVisible = true;
        await Task.Delay(900);
        SaveProgress.IsVisible = false;
        _saving = false;
        Snacks.Show("Ajustes guardados");
    }

    private void OnMenuClicked(object? sender, EventArgs e)
        => Shell.Current.FlyoutIsPresented = true;

    private void OnBellClicked(object? sender, EventArgs e)
        => Snacks.Show(NewsSwitch.IsOn ? "Tienes 2 novedades" : "Notificaciones desactivadas");

    private void OnSizeChanged(object? sender, EventArgs e)
    {
        // Guarda anti-bucle: solo actuar si el breakpoint cambió.
        if (Width <= 0)
            return;
        var bp = ResponsiveHelper.GetBreakpoint(Width);
        if (bp == _lastBp)
            return;
        _lastBp = bp;
        Root.Spacing = ResponsiveHelper.SpacingFor(bp);
    }
}
