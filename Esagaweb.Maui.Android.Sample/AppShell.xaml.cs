using Esagaweb.Maui.Android.Sample.Pages;

namespace Esagaweb.Maui.Android.Sample;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		// Detalle fuera del flyout: se abre por push con ?id=.
		Routing.RegisterRoute("detail", typeof(DetailPage));
	}
}
