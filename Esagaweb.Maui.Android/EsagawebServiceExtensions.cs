namespace Esagaweb.Maui.Android;

/// <summary>Registers the library fonts in the consuming app.</summary>
/// <remarks>Call <see cref="UseEsagaweb"/> in <c>MauiProgram</c> and merge the theme dictionaries once in the app.</remarks>
public static class EsagawebServiceExtensions
{
    /// <summary>Registers the Material Symbols and text fonts and returns the builder.</summary>
    /// <param name="builder">The app builder.</param>
    /// <returns>The same app builder for chaining.</returns>
    /// <example>
    /// <code language="csharp"><![CDATA[
    /// public static MauiApp CreateMauiApp()
    /// {
    ///     var builder = MauiApp.CreateBuilder();
    ///     builder.UseMauiApp<App>().UseEsagaweb();
    ///     return builder.Build();
    /// }
    /// ]]></code>
    /// </example>
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
