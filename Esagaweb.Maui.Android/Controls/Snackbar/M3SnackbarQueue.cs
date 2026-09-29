namespace Esagaweb.Maui.Android.Controls.Snackbar;

/// <summary>Represents a snackbar request with message text, optional action text, and optional visible duration.</summary>
/// <param name="Message">The message text to display.</param>
/// <param name="ActionText">The action button text. Null means no action button. The default is null.</param>
/// <param name="Duration">The visible duration. Null means the queue default duration. The default is null.</param>
public sealed record M3SnackbarRequest(
    string Message,
    string? ActionText = null,
    TimeSpan? Duration = null);

/// <summary>First-in first-out queue of snackbars. One request is visible and the rest wait in line.</summary>
/// <remarks>The host observes <see cref="Changed"/> and displays <see cref="Current"/>.</remarks>
/// <example>
/// <code language="csharp">
/// var queue = new M3SnackbarQueue();
/// queue.Enqueue(new M3SnackbarRequest("Saved"));
/// queue.Enqueue(new M3SnackbarRequest("Photo deleted", "Undo"));
/// </code>
/// </example>
public sealed class M3SnackbarQueue
{
    /// <summary>Gets the default visible duration of each request, which is 4 seconds.</summary>
    public static readonly TimeSpan DefaultDuration = TimeSpan.FromSeconds(4);

    private readonly Queue<M3SnackbarRequest> _pending = new();

    /// <summary>Gets the currently visible request, or null when the queue is empty.</summary>
    public M3SnackbarRequest? Current { get; private set; }

    /// <summary>Gets the number of waiting requests, not counting the visible one.</summary>
    public int PendingCount => _pending.Count;

    /// <summary>Occurs when the queue changes, after an enqueue, a dismissal, or a clear.</summary>
    public event EventHandler? Changed;

    /// <summary>Adds a request to the queue. It is shown at once when nothing is visible.</summary>
    /// <param name="request">The request to add to the queue.</param>
    public void Enqueue(M3SnackbarRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        if (Current is null)
            Current = request;
        else
            _pending.Enqueue(request);
        Changed?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>Dismisses the visible request and advances to the next waiting request, if any.</summary>
    public void DismissCurrent()
    {
        Current = _pending.Count > 0 ? _pending.Dequeue() : null;
        Changed?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>Clears the whole queue, including the visible request.</summary>
    public void Clear()
    {
        _pending.Clear();
        Current = null;
        Changed?.Invoke(this, EventArgs.Empty);
    }
}
