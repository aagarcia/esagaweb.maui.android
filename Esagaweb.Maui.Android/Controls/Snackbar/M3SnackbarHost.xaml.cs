using Esagaweb.Maui.Android.Controls.Common;

namespace Esagaweb.Maui.Android.Controls.Snackbar;

/// <summary>Visual host for a <see cref="M3SnackbarQueue"/>. It shows the current request and advances the queue.</summary>
/// <remarks>Place one instance at the bottom of the page and add notices with <see cref="Show"/>.</remarks>
/// <example>
/// <code language="csharp">
/// snackbarHost.Show("Saved");
/// snackbarHost.Show("Photo deleted", "Undo", TimeSpan.FromSeconds(5));
/// </code>
/// </example>
public partial class M3SnackbarHost : ContentView
{
    /// <summary>Gets the first-in first-out queue that feeds the host.</summary>
    public M3SnackbarQueue Queue { get; } = new();

    private IDispatcherTimer? _timer;
    private M3SnackbarRequest? _shown;

    /// <summary>Initializes a new instance of the <see cref="M3SnackbarHost"/> class.</summary>
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

    /// <summary>Occurs when the notice action button is tapped.</summary>
    public event EventHandler? ActionInvoked;

    /// <summary>Enqueues a notice with message text, optional action text, and optional duration.</summary>
    /// <param name="message">The message text to display.</param>
    /// <param name="actionText">The action button text. Null means no action button. The default is null.</param>
    /// <param name="duration">The visible duration. Null means the theme default duration. The default is null.</param>
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
        // Bar always uses the dark tone (fixed light keys, not themed).
        Bar.BackgroundColor = M3ControlHelper.Res("M3OnSurface");
        MessageLabel.TextColor = M3ControlHelper.Res("M3Surface");
        ActionButton.TextColor = M3ControlHelper.Res("M3PrimaryContainer");
        Bar.MinimumHeightRequest = M3ControlHelper.ResDouble("M3SnackbarMinHeight", 48);
    }
}
