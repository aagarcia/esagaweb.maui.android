namespace Esagaweb.Maui.Android.Controls.Snackbar;

/// <summary>Solicitud de aviso (lógica pura, testeable). La vista es <see cref="M3SnackbarHost"/>.</summary>
/// <param name="Message">Texto del aviso.</param>
/// <param name="ActionText">Texto del botón de acción. Null = sin botón.</param>
/// <param name="Duration">Duración visible. Null = default de la cola (4s).</param>
public sealed record M3SnackbarRequest(
    string Message,
    string? ActionText = null,
    TimeSpan? Duration = null);

/// <summary>
/// Cola FIFO de snackbars estilo Flutter (uno visible, resto en espera).
/// El host observa <see cref="Changed"/> y muestra <see cref="Current"/>.
/// </summary>
public sealed class M3SnackbarQueue
{
    /// <summary>Duración default de cada aviso (4 segundos).</summary>
    public static readonly TimeSpan DefaultDuration = TimeSpan.FromSeconds(4);

    private readonly Queue<M3SnackbarRequest> _pending = new();

    /// <summary>Aviso visible actualmente. Null = cola vacía.</summary>
    public M3SnackbarRequest? Current { get; private set; }

    /// <summary>Cantidad de avisos en espera (sin contar el visible).</summary>
    public int PendingCount => _pending.Count;

    /// <summary>Se dispara en cada cambio (encolar, cerrar, limpiar).</summary>
    public event EventHandler? Changed;

    /// <summary>Agrega un aviso: si no hay visible, se muestra de inmediato.</summary>
    public void Enqueue(M3SnackbarRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        if (Current is null)
            Current = request;
        else
            _pending.Enqueue(request);
        Changed?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>Cierra el visible y avanza al siguiente en espera (si hay).</summary>
    public void DismissCurrent()
    {
        Current = _pending.Count > 0 ? _pending.Dequeue() : null;
        Changed?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>Vacía la cola por completo (incluido el visible).</summary>
    public void Clear()
    {
        _pending.Clear();
        Current = null;
        Changed?.Invoke(this, EventArgs.Empty);
    }
}
