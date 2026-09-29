using System.Reflection;
using Esagaweb.Maui.Android.Controls.Common;
using Xunit;

namespace Esagaweb.Maui.Android.Tests;

public class M3IconsTests
{
    [Theory]
    // Codepoints oficiales verificados (escapes exactos; si falla, regenerar desde la fuente).
    [InlineData("Add", "")]
    [InlineData("ArrowBack", "")]
    [InlineData("Delete", "")]
    [InlineData("Favorite", "")]
    [InlineData("Home", "")]
    [InlineData("Menu", "")]
    [InlineData("Settings", "")]
    public void IconoConocido_TieneCodepointEsperado(string name, string expected)
    {
        var field = typeof(M3Icons).GetField(name, BindingFlags.Public | BindingFlags.Static);
        Assert.NotNull(field);
        Assert.Equal(expected, field.GetValue(null));
    }

    [Fact]
    public void TodosLosGlyphs_SonUnSoloRuneDeUsoPrivado()
    {
        var fields = typeof(M3Icons).GetFields(BindingFlags.Public | BindingFlags.Static);
        Assert.True(fields.Length > 4000, $"Se esperaban >4000 iconos, hay {fields.Length}");
        foreach (var f in fields)
        {
            var v = (string)f.GetValue(null)!;
            var runes = v.EnumerateRunes().ToArray();
            Assert.True(runes.Length == 1, $"{f.Name} debe ser un solo rune");
            var n = runes[0].Value;
            var enPrivado = (n >= 0xE000 && n <= 0xF8FF)      // PUA BMP
                || (n >= 0xF0000 && n <= 0xFFFFD)             // Plano 15
                || (n >= 0x100000 && n <= 0x10FFFD);          // Plano 16
            Assert.True(enPrivado, $"{f.Name} fuera del área de uso privado");
        }
    }

    [Fact]
    public void Nombres_SonPascalCaseSinGuiones()
    {
        foreach (var f in typeof(M3Icons).GetFields(BindingFlags.Public | BindingFlags.Static))
        {
            Assert.DoesNotContain("_", f.Name);
            Assert.True(char.IsUpper(f.Name[0]) || f.Name[0] == 'N', $"{f.Name} debe ser PascalCase");
        }
    }
}
