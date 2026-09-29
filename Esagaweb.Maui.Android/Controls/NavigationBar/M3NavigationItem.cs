using System.Windows.Input;

namespace Esagaweb.Maui.Android.Controls.NavigationBar;

/// <summary>Un destino del M3NavigationBar (icono + etiqueta + comando opcional).</summary>
public class M3NavigationItem
{
    /// <summary>Etiqueta bajo el icono.</summary>
    public string Label { get; set; } = string.Empty;

    /// <summary>Glyph MaterialSymbols inactivo (estilo M3Icons.Home).</summary>
    public string Glyph { get; set; } = string.Empty;

    /// <summary>Glyph activo (en M3 suele ser la versión filled). Vacío = usa Glyph.</summary>
    public string SelectedGlyph { get; set; } = string.Empty;

    /// <summary>Comando al seleccionar este destino (sin parámetros).</summary>
    public ICommand? Command { get; set; }
}
