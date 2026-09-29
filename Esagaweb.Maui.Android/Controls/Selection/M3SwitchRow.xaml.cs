using System.Windows.Input;
using Esagaweb.Maui.Android.Controls.Common;

namespace Esagaweb.Maui.Android.Controls.Selection;

/// <summary>Fila M3: etiqueta Nunito + Switch nativo themado (OnColor Primary).</summary>
public partial class M3SwitchRow : ContentView
{
    /// <summary>Propiedad enlazable para <see cref="Text"/>.</summary>
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(M3SwitchRow), string.Empty,
            propertyChanged: (b, _, n) => ((M3SwitchRow)b).RowLabel.Text = (string?)n ?? string.Empty);

    /// <summary>Propiedad enlazable para <see cref="IsOn"/>.</summary>
    public static readonly BindableProperty IsOnProperty =
        BindableProperty.Create(nameof(IsOn), typeof(bool), typeof(M3SwitchRow), false,
            BindingMode.TwoWay, propertyChanged: (b, _, n) => ((M3SwitchRow)b).InnerSwitch.IsToggled = (bool)n);

    /// <summary>Propiedad enlazable para <see cref="Command"/>.</summary>
    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(M3SwitchRow), null);

    /// <summary>Etiqueta de la fila.</summary>
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    /// <summary>Estado del interruptor. TwoWay: sirve para cargar y leer de vuelta (ej. formularios API).</summary>
    public bool IsOn
    {
        get => (bool)GetValue(IsOnProperty);
        set => SetValue(IsOnProperty, value);
    }

    /// <summary>Comando al alternar (recibe el nuevo valor bool).</summary>
    public ICommand? Command
    {
        get => (ICommand?)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    /// <summary>Se dispara al alternar (con el nuevo valor).</summary>
    public event EventHandler<bool>? Toggled;

    /// <summary>Crea una nueva instancia.</summary>
    public M3SwitchRow()
    {
        InitializeComponent();
        RowLabel.FontFamily = "NunitoRegular";
        RowLabel.Text = Text;
        InnerSwitch.IsToggled = IsOn;
        InnerSwitch.Toggled += (_, e) =>
        {
            IsOn = e.Value;
            if (Command?.CanExecute(e.Value) == true)
                Command.Execute(e.Value);
            Toggled?.Invoke(this, e.Value);
        };
        ApplyTheme();
    }

    /// <inheritdoc/>
    protected override void OnPropertyChanged(string? propertyName = null)
    {
        base.OnPropertyChanged(propertyName);
        if (propertyName == nameof(IsEnabled))
            ApplyTheme();
    }

    private void ApplyTheme()
    {
        Root.HeightRequest = M3ControlHelper.ResDouble("M3SelectionRowHeight", 48);
        M3ControlHelper.SetThemed(RowLabel, Label.TextColorProperty, "M3OnSurface", "M3DarkOnSurface");
        M3ControlHelper.SetThemed(InnerSwitch, Switch.OnColorProperty, "M3Primary", "M3DarkPrimary");
        M3ControlHelper.SetThemed(InnerSwitch, Switch.ThumbColorProperty, "M3OnPrimary", "M3DarkOnPrimary");
        Opacity = IsEnabled ? 1 : 0.6;
    }
}
