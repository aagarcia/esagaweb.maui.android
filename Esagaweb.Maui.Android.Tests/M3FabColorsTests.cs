using Esagaweb.Maui.Android.Controls.Fab;
using Xunit;

namespace Esagaweb.Maui.Android.Tests;

public class M3FabColorsTests
{
    [Theory]
    [InlineData(M3FabVariant.Primary, "M3PrimaryContainer", "M3DarkPrimaryContainer",
        "M3OnPrimaryContainer", "M3DarkPrimary")]
    [InlineData(M3FabVariant.Surface, "M3SurfaceVariant", "M3DarkSurfaceVariant",
        "M3Primary", "M3DarkPrimary")]
    [InlineData(M3FabVariant.Secondary, "M3SecondaryContainer", "M3DarkSecondaryContainer",
        "M3OnSecondaryContainer", "M3DarkOnSecondaryContainer")]
    [InlineData(M3FabVariant.Tertiary, "M3TertiaryContainer", "M3DarkTertiaryContainer",
        "M3OnTertiaryContainer", "M3DarkOnTertiaryContainer")]
    public void Resolve_DevuelveClavesDeLaVariante(M3FabVariant variant,
        string bgLight, string bgDark, string iconLight, string iconDark)
    {
        var (bl, bd, il, id) = M3FabColors.Resolve(variant);
        Assert.Equal(bgLight, bl);
        Assert.Equal(bgDark, bd);
        Assert.Equal(iconLight, il);
        Assert.Equal(iconDark, id);
    }

    [Fact]
    public void Resolve_ValorFueraDeRango_CaeAPrimary()
    {
        var (bl, _, _, _) = M3FabColors.Resolve((M3FabVariant)99);
        Assert.Equal("M3PrimaryContainer", bl);
    }
}
