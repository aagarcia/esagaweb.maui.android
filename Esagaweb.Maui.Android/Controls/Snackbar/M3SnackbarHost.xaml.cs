using Esagaweb.Maui.Android.Controls.Common;

namespace Esagaweb.Maui.Android.Controls.Snackbar;

/// <summary>
/// Anfitrión visual de <see cref="M3SnackbarQueue"/> (como Flutter ScaffoldMessenger):
/// muestra <c>Current</c>, auto-cierra por duración y avanza la cola.
/// Uso: una instancia al pie de la página + <c>Queue.Enqueue(...)</c>.
/// </summary>
public partial class M3SnackbarHost : ContentView
{
    /// <summary>Cola FIFO que alimenta al host.</summary>
    public M3SnackbarQueue Queue { get; } = new();

    private IDispatcherTimer? _timer;
    private M3SnackbarRequest? _shown;

    /// <summary>Crea una nueva instancia.</summary>
    public M3SnackbarHost()
    {
        InitializeComponent();
        MessageLabel.FontFamily = "NunitoRegular";
        ActionButton.FontFamily = "NunitoSemiBold";
        Queue.Changed += (_, _) => Refresh();
        ActionButton.Clicked += (_, _) =>
        {
            ActionInvoked?.Invoke(this, EventArgs.Empty);
            Queue.DismissCurrent();
        };
        ApplyTheme();
        Refresh();
    }

    /// <summary>Se dispara al tocar el botón de acción del aviso.</summary>
    public event EventHandler? ActionInvoked;

    /// <summary>Encola un aviso (texto + acción opcional + duración opcional).</summary>
    public void Show(string message, string? actionText = null, TimeSpan? duration = null)
        => Queue.Enqueue(new M3SnackbarRequest(message, actionText, duration));

    private void Refresh()
    {
        _timer?.Stop();
        _timer = null;
        _shown = Queue.Current;
        if (_shown is null)
        {
            Bar.IsVisible = false;
            return;
        }
        MessageLabel.Text = _shown.Message;
        ActionButton.Text = _shown.ActionText ?? string.Empty;
        ActionButton.IsVisible = !string.IsNullOrEmpty(_shown.ActionText);
        Bar.IsVisible = true;
        var seconds = _shown.Duration?.TotalSeconds
            ?? M3ControlHelper.ResDouble("M3SnackbarDurationSeconds", 4);
        _timer = Dispatcher.CreateTimer();
        _timer.Interval = TimeSpan.FromSeconds(Math.Max(1, seconds));
        _timer.Tick += (_, _) => Queue.DismissCurrent();
        _timer.Start();
    }

    private void ApplyTheme()
    {
        // Barra siempre oscura como Flutter (claves light fijas, no themed).
        Bar.BackgroundColor = M3ControlHelper.Res("M3OnSurface");
        MessageLabel.TextColor = M3ControlHelper.Res("M3Surface");
        ActionButton.TextColor = M3ControlHelper.Res("M3PrimaryContainer");
        Bar.MinimumHeightRequest = M3ControlHelper.ResDouble("M3SnackbarMinHeight", 48);
    }
}
