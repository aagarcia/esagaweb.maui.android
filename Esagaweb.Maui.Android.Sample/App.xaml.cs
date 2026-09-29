using Microsoft.Extensions.DependencyInjection;

namespace Esagaweb.Maui.Android.Sample;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
		EsagawebThemes.Apply();
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new AppShell());
	}
}