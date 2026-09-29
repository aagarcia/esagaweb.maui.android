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

	// En Android, atrás con el flyout abierto cerraba la app: primero se cierra el menú.
	protected override bool OnBackButtonPressed()
	{
		if (FlyoutIsPresented)
		{
			FlyoutIsPresented = false;
			return true;
		}
		return base.OnBackButtonPressed();
	}
}
