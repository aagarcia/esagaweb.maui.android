using Esagaweb.Maui.Android.Controls.Common;
using Xunit;

public class M3ControlHelperTests
{
    [Fact]
    public void Res_ClaveInexistente_DevuelveTransparente()
    {
        // Sin app MAUI viva, cualquier clave cae al fallback.
        Assert.Equal(Colors.Transparent, M3ControlHelper.Res("M3ClaveQueNoExiste"));
    }

    [Fact]
    public void ResDouble_ClaveInexistente_DevuelveFallback()
    {
        Assert.Equal(42.0, M3ControlHelper.ResDouble("M3ClaveQueNoExiste", 42.0));
    }

    [Fact]
    public void SetThemed_SinApp_NoRevienta()
    {
        var label = new Label();
        var ex = Record.Exception(() =>
            M3ControlHelper.SetThemed(label, Label.TextColorProperty, "M3Primary", "M3DarkPrimary"));
        Assert.Null(ex);
    }
}
