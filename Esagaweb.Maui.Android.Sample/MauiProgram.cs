using Microsoft.Extensions.Logging;

namespace Esagaweb.Maui.Android.Sample;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseEsagaweb();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
