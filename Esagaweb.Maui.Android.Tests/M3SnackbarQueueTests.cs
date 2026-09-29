using Esagaweb.Maui.Android.Controls.Snackbar;
using Xunit;

public class M3SnackbarQueueTests
{
    [Fact]
    public void Enqueue_EnVacio_PasaACurrent()
    {
        var q = new M3SnackbarQueue();
        q.Enqueue(new M3SnackbarRequest("hola"));
        Assert.Equal("hola", q.Current?.Message);
        Assert.Equal(0, q.PendingCount);
    }

    [Fact]
    public void Enqueue_ConActual_EncolaEnEspera()
    {
        var q = new M3SnackbarQueue();
        q.Enqueue(new M3SnackbarRequest("uno"));
        q.Enqueue(new M3SnackbarRequest("dos"));
        Assert.Equal("uno", q.Current?.Message);
        Assert.Equal(1, q.PendingCount);
    }

    [Fact]
    public void DismissCurrent_AvanzaAlSiguiente()
    {
        var q = new M3SnackbarQueue();
        q.Enqueue(new M3SnackbarRequest("uno"));
        q.Enqueue(new M3SnackbarRequest("dos"));
        q.DismissCurrent();
        Assert.Equal("dos", q.Current?.Message);
        Assert.Equal(0, q.PendingCount);
        q.DismissCurrent();
        Assert.Null(q.Current);
    }

    [Fact]
    public void Clear_VaciaTodo()
    {
        var q = new M3SnackbarQueue();
        q.Enqueue(new M3SnackbarRequest("uno"));
        q.Enqueue(new M3SnackbarRequest("dos"));
        q.Clear();
        Assert.Null(q.Current);
        Assert.Equal(0, q.PendingCount);
    }

    [Fact]
    public void Cambios_DisparanEvento()
    {
        var q = new M3SnackbarQueue();
        var n = 0;
        q.Changed += (_, _) => n++;
        q.Enqueue(new M3SnackbarRequest("uno"));
        q.DismissCurrent();
        q.Clear();
        Assert.Equal(3, n);
    }
}
