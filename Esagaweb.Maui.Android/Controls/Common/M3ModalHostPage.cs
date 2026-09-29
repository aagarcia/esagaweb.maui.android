namespace Esagaweb.Maui.Android.Controls.Common;

/// <summary>
/// A modal host page that exposes a back button press callback for dialogs and bottom sheets.
/// </summary>
internal sealed class M3ModalHostPage : ContentPage
{
    /// <summary>
    /// Invoked when the hardware/software back button is pressed while this page is on top.
    /// </summary>
    public Action? BackRequested { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="M3ModalHostPage"/> class.
    /// </summary>
    public M3ModalHostPage()
    {
        BackgroundColor = Colors.Transparent;
    }

    /// <summary>
    /// Handles the back button press by invoking <see cref="BackRequested"/> and consuming the event.
    /// </summary>
    /// <returns><see langword="true"/> to indicate the back navigation was handled.</returns>
    protected override bool OnBackButtonPressed()
    {
        BackRequested?.Invoke();
        return true;
    }
}