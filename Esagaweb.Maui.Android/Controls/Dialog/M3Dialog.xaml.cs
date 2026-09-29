using Esagaweb.Maui.Android.Controls.Common;
using Microsoft.Maui.Controls.Shapes;

namespace Esagaweb.Maui.Android.Controls.Dialog;

/// <summary>
/// Diálogo M3 (como Flutter AlertDialog / showDialog): título, mensaje o
/// contenido propio, acción confirmar + cancelar. Se muestra con
/// <see cref="ShowAsync"/> (modal, devuelve true/false/null) o incrustado.
/// </summary>
[ContentProperty(nameof(DialogContent))]
public partial class M3Dialog : ContentView
{
    /// <summary>Propiedad enlazable para <see cref="Title"/>.</summary>
    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(nameof(Title), typeof(string), typeof(M3Dialog), string.Empty,
            propertyChanged: (b, _, n) =>
            {
                var self = (M3Dialog)b;
                self.TitleLabel.Text = (string?)n ?? string.Empty;
                self.TitleLabel.IsVisible = !string.IsNullOrEmpty((string?)n);
            });

    /// <summary>Propiedad enlazable para <see cref="Message"/>.</summary>
    public static readonly BindableProperty MessageProperty =
        BindableProperty.Create(nameof(Message), typeof(string), typeof(M3Dialog), string.Empty,
            propertyChanged: (b, _, n) =>
            {
                var self = (M3Dialog)b;
                self.MessageLabel.Text = (string?)n ?? string.Empty;
                self.MessageLabel.IsVisible = !string.IsNullOrEmpty((string?)n);
            });

    /// <summary>Propiedad enlazable para <see cref="ConfirmText"/>.</summary>
    public static readonly BindableProperty ConfirmTextProperty =
        BindableProperty.Create(nameof(ConfirmText), typeof(string), typeof(M3Dialog), "OK",
            propertyChanged: (b, _, n) => ((M3Dialog)b).UpdateActions());

    /// <summary>Propiedad enlazable para <see cref="CancelText"/>.</summary>
    public static readonly BindableProperty CancelTextProperty =
        BindableProperty.Create(nameof(CancelText), typeof(string), typeof(M3Dialog), string.Empty,
            propertyChanged: (b, _, n) => ((M3Dialog)b).UpdateActions());

    /// <summary>Título. Vacío = se oculta.</summary>
    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    /// <summary>Mensaje. Vacío = se oculta.</summary>
    public string Message
    {
        get => (string)GetValue(MessageProperty);
        set => SetValue(MessageProperty, value);
    }

    /// <summary>Texto del botón de confirmar. Default "OK".</summary>
    public string ConfirmText
    {
        get => (string)GetValue(ConfirmTextProperty);
        set => SetValue(ConfirmTextProperty, value);
    }

    /// <summary>Texto del botón de cancelar. Vacío = sin botón. Default vacío.</summary>
    public string CancelText
    {
        get => (string)GetValue(CancelTextProperty);
        set => SetValue(CancelTextProperty, value);
    }

    /// <summary>Contenido propio en lugar del mensaje (ej. un M3TextField).</summary>
    public View? DialogContent
    {
        get => CustomContent.Content;
        set
        {
            CustomContent.Content = value;
            CustomContent.IsVisible = value is not null;
        }
    }

    /// <summary>Se dispara al confirmar.</summary>
    public event EventHandler? Confirmed;
    /// <summary>Se dispara al cancelar.</summary>
    public event EventHandler? Cancelled;

    /// <summary>Crea una nueva instancia.</summary>
    public M3Dialog()
    {
        InitializeComponent();
        TitleLabel.FontFamily = "NunitoSemiBold";
        MessageLabel.FontFamily = "NunitoRegular";
        TitleLabel.IsVisible = !string.IsNullOrEmpty(Title);
        MessageLabel.IsVisible = !string.IsNullOrEmpty(Message);
        CancelButton.Clicked += async (_, _) => await CloseAsync(false);
        ConfirmButton.Clicked += async (_, _) => await CloseAsync(true);
        UpdateActions();
        ApplyTheme();
    }

    private TaskCompletionSource<bool?>? _tcs;
    private Page? _hostPage;

    /// <summary>Muestra el diálogo como modal. true=confirmar, false=cancelar, null=cerrado por atrás.</summary>
    public async Task<bool?> ShowAsync(INavigation navigation)
    {
        _tcs = new TaskCompletionSource<bool?>();
        // Desvincular de un padre anterior ANTES de agregarlo al host:
        // hacerlo después lo arrancaría de su propia página host.
        ParentRefresh();
        _hostPage = new ContentPage
        {
            BackgroundColor = Color.FromArgb("#52000000"),
            Content = new Grid
            {
                Padding = 32,
                Children = { this }
            }
        };
        await navigation.PushModalAsync(_hostPage);
        return await _tcs.Task;
    }

    private async Task CloseAsync(bool? result)
    {
        if (_hostPage?.Navigation is not null)
            await _hostPage.Navigation.PopModalAsync();
        _hostPage = null;
        if (result == true)
            Confirmed?.Invoke(this, EventArgs.Empty);
        else if (result == false)
            Cancelled?.Invoke(this, EventArgs.Empty);
        _tcs?.TrySetResult(result);
        _tcs = null;
    }

    private void ParentRefresh()
    {
        // El ContentView debe desvincularse de su padre anterior antes del modal.
        (Parent as Layout)?.Children.Remove(this);
    }

    private void UpdateActions()
    {
        ConfirmButton.Text = ConfirmText;
        ConfirmButton.IsVisible = !string.IsNullOrEmpty(ConfirmText);
        CancelButton.Text = CancelText;
        CancelButton.IsVisible = !string.IsNullOrEmpty(CancelText);
        ActionsRow.IsVisible = ConfirmButton.IsVisible || CancelButton.IsVisible;
    }

    private void ApplyTheme()
    {
        Card.MaximumWidthRequest = M3ControlHelper.ResDouble("M3DialogMaxWidth", 280);
        if (Card.StrokeShape is RoundRectangle rr)
            rr.CornerRadius = M3ControlHelper.ResDouble("M3DialogCornerRadius", 28);
        M3ControlHelper.SetThemed(Card, Border.BackgroundColorProperty, "M3Surface", "M3DarkSurface");
        M3ControlHelper.SetThemed(TitleLabel, global::Microsoft.Maui.Controls.Label.TextColorProperty, "M3OnSurface", "M3DarkOnSurface");
        M3ControlHelper.SetThemed(MessageLabel, global::Microsoft.Maui.Controls.Label.TextColorProperty, "M3OnSurfaceVariant", "M3DarkOnSurface");
        M3ControlHelper.SetThemed(ConfirmButton, Button.BackgroundColorProperty, "M3Error", "M3DarkError");
        M3ControlHelper.SetThemed(ConfirmButton, Button.TextColorProperty, "M3OnError", "M3DarkOnError");
        M3ControlHelper.SetThemed(CancelButton, Button.TextColorProperty, "M3Primary", "M3DarkPrimary");
    }
}
