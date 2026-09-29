using Esagaweb.Maui.Android.Controls.Common;
using Microsoft.Maui.Controls.Shapes;

namespace Esagaweb.Maui.Android.Controls.Dialog;

/// <summary>
/// Represents a Material 3 dialog with a title, a message or custom content, and confirm and cancel actions.
/// </summary>
/// <remarks>
/// The dialog can be shown modally with <see cref="ShowAsync"/> or embedded inline as content.
/// <see cref="Confirmed"/> is raised when the dialog is confirmed and <see cref="Cancelled"/> is raised
/// when the dialog is canceled. Empty action labels hide their buttons. Requires <c>UseEsagaweb</c>
/// font registration and the M3 theme resource dictionaries.
/// </remarks>
/// <example>
/// <code language="csharp"><![CDATA[
/// var dialog = new M3Dialog
/// {
///     Title = "Add to cart",
///     Message = "Confirm the selected size before continuing.",
///     ConfirmText = "Add",
///     CancelText = "Cancel",
/// };
/// bool? result = await dialog.ShowAsync(Navigation);
/// if (result == true)
/// {
///     // The dialog was confirmed.
/// }
/// ]]></code>
/// </example>
[ContentProperty(nameof(DialogContent))]
public partial class M3Dialog : ContentView
{
    /// <summary>
    /// Identifies the <see cref="Title"/> bindable property.
    /// </summary>
    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(nameof(Title), typeof(string), typeof(M3Dialog), string.Empty,
            propertyChanged: (b, _, n) =>
            {
                var self = (M3Dialog)b;
                self.TitleLabel.Text = (string?)n ?? string.Empty;
                self.TitleLabel.IsVisible = !string.IsNullOrEmpty((string?)n);
            });

    /// <summary>
    /// Identifies the <see cref="Message"/> bindable property.
    /// </summary>
    public static readonly BindableProperty MessageProperty =
        BindableProperty.Create(nameof(Message), typeof(string), typeof(M3Dialog), string.Empty,
            propertyChanged: (b, _, n) =>
            {
                var self = (M3Dialog)b;
                self.MessageLabel.Text = (string?)n ?? string.Empty;
                self.MessageLabel.IsVisible = !string.IsNullOrEmpty((string?)n);
            });

    /// <summary>
    /// Identifies the <see cref="ConfirmText"/> bindable property.
    /// </summary>
    public static readonly BindableProperty ConfirmTextProperty =
        BindableProperty.Create(nameof(ConfirmText), typeof(string), typeof(M3Dialog), "OK",
            propertyChanged: (b, _, n) => ((M3Dialog)b).UpdateActions());

    /// <summary>
    /// Identifies the <see cref="CancelText"/> bindable property.
    /// </summary>
    public static readonly BindableProperty CancelTextProperty =
        BindableProperty.Create(nameof(CancelText), typeof(string), typeof(M3Dialog), string.Empty,
            propertyChanged: (b, _, n) => ((M3Dialog)b).UpdateActions());

    /// <summary>
    /// Gets or sets the dialog title. An empty value hides the title. The default is <see cref="string.Empty"/>.
    /// </summary>
    /// <remarks>This is a bindable property.</remarks>
    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    /// <summary>
    /// Gets or sets the dialog message. An empty value hides the message. The default is <see cref="string.Empty"/>.
    /// </summary>
    /// <remarks>This is a bindable property.</remarks>
    public string Message
    {
        get => (string)GetValue(MessageProperty);
        set => SetValue(MessageProperty, value);
    }

    /// <summary>
    /// Gets or sets the confirm button text. An empty value hides the button. The default is <c>OK</c>.
    /// </summary>
    /// <remarks>This is a bindable property.</remarks>
    public string ConfirmText
    {
        get => (string)GetValue(ConfirmTextProperty);
        set => SetValue(ConfirmTextProperty, value);
    }

    /// <summary>
    /// Gets or sets the cancel button text. An empty value hides the button. The default is <see cref="string.Empty"/>.
    /// </summary>
    /// <remarks>This is a bindable property.</remarks>
    public string CancelText
    {
        get => (string)GetValue(CancelTextProperty);
        set => SetValue(CancelTextProperty, value);
    }

    /// <summary>
    /// Gets or sets custom content displayed in the dialog body. A <c>null</c> value hides the custom content area.
    /// </summary>
    public View? DialogContent
    {
        get => CustomContent.Content;
        set
        {
            CustomContent.Content = value;
            CustomContent.IsVisible = value is not null;
        }
    }

    /// <summary>
    /// Occurs when the dialog is confirmed.
    /// </summary>
    public event EventHandler? Confirmed;
    /// <summary>
    /// Occurs when the dialog is canceled.
    /// </summary>
    public event EventHandler? Cancelled;

    /// <summary>
    /// Initializes a new instance of the <see cref="M3Dialog"/> class.
    /// </summary>
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

    /// <summary>
    /// Displays the dialog as a modal page.
    /// </summary>
    /// <param name="navigation">The navigation service used to push the modal host page.</param>
    /// <returns>A task that completes when the dialog closes. Its result is <c>true</c> when the dialog is confirmed, <c>false</c> when the dialog is canceled, and <c>null</c> when the dialog is dismissed without a choice (e.g., hardware back button).</returns>
    /// <remarks>
    /// Calling <see cref="ShowAsync"/> while the dialog is already open returns the same pending task without presenting a second modal.
    /// </remarks>
    public async Task<bool?> ShowAsync(INavigation navigation)
    {
        if (_tcs is not null)
        {
            // Dialog already open: return the existing task.
            return await _tcs.Task;
        }

        _tcs = new TaskCompletionSource<bool?>();
        // Detach from a previous parent BEFORE adding it to the host:
        // doing it afterwards would remove it from its own host page.
        ParentRefresh();
        _hostPage = new M3ModalHostPage
        {
            BackgroundColor = Color.FromArgb("#52000000"),
            Content = new Grid
            {
                Padding = 32,
                Children = { this }
            },
            BackRequested = () => _ = CloseAsync(null)
        };
        await navigation.PushModalAsync(_hostPage);
        return await _tcs.Task;
    }

    private async Task CloseAsync(bool? result)
    {
        var tcs = _tcs;
        if (tcs is null)
        {
            return;
        }
        _tcs = null;

        var host = _hostPage;
        _hostPage = null;

        if (host?.Navigation is not null)
        {
            await host.Navigation.PopModalAsync();
        }

        if (result == true)
        {
            Confirmed?.Invoke(this, EventArgs.Empty);
        }
        else if (result == false)
        {
            Cancelled?.Invoke(this, EventArgs.Empty);
        }

        tcs.TrySetResult(result);
    }

    private void ParentRefresh()
    {
        // The ContentView must detach from its previous parent before the modal.
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
