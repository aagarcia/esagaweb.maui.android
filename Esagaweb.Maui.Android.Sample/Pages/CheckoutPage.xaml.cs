using System.Windows.Input;
using Esagaweb.Maui.Android.Helpers;

namespace Esagaweb.Maui.Android.Sample.Pages;

/// <summary>Formulario de pago: text fields, envío, términos y progreso.</summary>
public partial class CheckoutPage : ContentPage
{
    public ICommand ClearMailCommand { get; }
    public ICommand TogglePasswordCommand { get; }
    private M3Breakpoint? _lastBp;
    private bool _paying;

    public CheckoutPage()
    {
        ClearMailCommand = new Command(() => MailField.Text = string.Empty);
        TogglePasswordCommand = new Command(() => PasswordField.IsPassword = !PasswordField.IsPassword);
        InitializeComponent();
        SizeChanged += OnSizeChanged;
    }

    private async void OnPayClicked(object? sender, EventArgs e)
    {
        if (_paying)
            return;
        NameField.HasError = string.IsNullOrWhiteSpace(NameField.Text);
        MailField.HasError = string.IsNullOrWhiteSpace(MailField.Text) || !MailField.Text.Contains('@');
        if (NameField.HasError || MailField.HasError)
        {
            Snacks.Show("Revisa nombre y correo");
            return;
        }
        if (!TermsCheck.IsChecked)
        {
            Snacks.Show("Acepta los términos para continuar");
            return;
        }

        _paying = true;
        PayProgress.IsVisible = true;
        await Task.Delay(1200);
        PayProgress.IsVisible = false;
        _paying = false;
        CartState.Clear();
        Snacks.Show("Pedido confirmado. ¡Gracias por tu compra!");
    }

    private void OnMenuClicked(object? sender, EventArgs e)
        => Shell.Current.FlyoutIsPresented = true;

    private void OnInfoClicked(object? sender, EventArgs e)
        => Snacks.Show($"Llevas {CartState.Count} artículos en el carrito");

    private void OnSizeChanged(object? sender, EventArgs e)
    {
        // Guarda anti-bucle: solo actuar si el breakpoint cambió.
        if (Width <= 0)
            return;
        var bp = ResponsiveHelper.GetBreakpoint(Width);
        if (bp == _lastBp)
            return;
        _lastBp = bp;
        Root.Spacing = ResponsiveHelper.SpacingFor(bp);
    }
}
