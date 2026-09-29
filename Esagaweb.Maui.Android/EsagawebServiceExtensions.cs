namespace Esagaweb.Maui.Android;

/// <summary>
/// Registro de Esagaweb.Maui.Android en la app consumidora (2 líneas):
/// <c>builder.UseEsagaweb();</c> en MauiProgram + <c>EsagawebThemes.Apply();</c>
/// en el ctor de App. Registra fuentes M3 y fusiona los 6 diccionarios.
/// </summary>
public static class EsagawebServiceExtensions
{
    /// <summary>Registra las fuentes MaterialSymbols/Nunito. Llamar en MauiProgram.</summary>
    public static MauiAppBuilder UseEsagaweb(this MauiAppBuilder builder)
    {
        builder.ConfigureFonts(fonts =>
        {
            fonts.AddFont("Nunito-Regular.ttf", "NunitoRegular");
            fonts.AddFont("Nunito-SemiBold.ttf", "NunitoSemiBold");
            fonts.AddFont("MaterialSymbolsOutlined.ttf", "MaterialSymbols");
        });

        return builder;
    }
}
