using Android.App;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
using AndroidX.Core.View;

namespace Esagaweb.Maui.Android.Sample;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        ApplySystemBarColors();
    }

    public override void OnConfigurationChanged(Configuration newConfig)
    {
        base.OnConfigurationChanged(newConfig);
        ApplySystemBarColors();
    }

    /// <summary>
    /// Tiñe la status bar con M3Surface para que no se vea el hueco blanco
    /// sobre el TopAppBar: con edge-to-edge (API 35+) la barra es transparente
    /// y deja ver el fondo de ventana. Se reaplica al cambiar light/dark.
    /// </summary>
    private void ApplySystemBarColors()
    {
        if (Window is null)
            return;
        var night = (Resources?.Configuration?.UiMode & UiMode.NightMask) == UiMode.NightYes;
        var surface = night
            ? new global::Android.Graphics.Color(0x14, 0x12, 0x18)   // M3DarkSurface
            : new global::Android.Graphics.Color(0xFE, 0xF7, 0xFF);  // M3Surface
        if (!OperatingSystem.IsAndroidVersionAtLeast(35))
            Window.SetStatusBarColor(surface); // API 21-34: barra opaca
        // API 35+: edge-to-edge, la barra es transparente y deja ver el fondo de ventana.
        Window.SetBackgroundDrawable(new global::Android.Graphics.Drawables.ColorDrawable(surface));
        new WindowInsetsControllerCompat(Window, Window.DecorView).AppearanceLightStatusBars = !night;
    }
}
