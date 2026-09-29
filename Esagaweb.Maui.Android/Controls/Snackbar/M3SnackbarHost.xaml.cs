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
    private double _lastOccupiedHeight;

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
        Bar.SizeChanged += (_, _) => RaiseOccupiedHeightChanged();
        ApplyTheme();
        Refresh();
    }

    /// <summary>Occurs when the notice action button is tapped.</summary>
    public event EventHandler? ActionInvoked;

    /// <summary>
    /// Occurs when <see cref="OccupiedHeight"/> changes.
    /// </summary>
    public event EventHandler? OccupiedHeightChanged;

    /// <summary>
    /// Gets the vertical space occupied by the visible snackbar bar, including its margin.
    /// Returns 0 when the bar is hidden or has not been measured yet.
    /// </summary>
    public double OccupiedHeight => ComputeOccupiedHeight(Bar.IsVisible, Bar.Height, Bar.Margin);

    /// <summary>
    /// Computes the vertical space occupied by the snackbar bar.
    /// </summary>
    /// <param name="visible">Whether the bar is currently visible.</param>
    /// <param name="barHeight">The measured height of the bar. Values less than or equal to zero mean not yet measured.</param>
    /// <param name="margin">The margin applied to the bar.</param>
    /// <returns>The occupied vertical space (bar height plus vertical margin), or 0 when hidden or not yet measured.</returns>
    internal static double ComputeOccupiedHeight(bool visible, double barHeight, Thickness margin)
        => visible && barHeight > 0 ? barHeight + margin.VerticalThickness : 0;

    private void RaiseOccupiedHeightChanged()
    {
        var height = OccupiedHeight;
        if (Math.Abs(height - _lastOccupiedHeight) < 0.01)
        {
            return;
        }
        _lastOccupiedHeight = height;
        OccupiedHeightChanged?.Invoke(this, EventArgs.Empty);
    }

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
            RaiseOccupiedHeightChanged();
            return;
        }
        MessageLabel.Text = _shown.Message;
        ActionButton.Text = _shown.ActionText ?? string.Empty;
        ActionButton.IsVisible = !string.IsNullOrEmpty(_shown.ActionText);
        Bar.IsVisible = true;
        RaiseOccupiedHeightChanged();
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
