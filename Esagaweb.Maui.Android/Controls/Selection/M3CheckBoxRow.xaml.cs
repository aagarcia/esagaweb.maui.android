using System.Windows.Input;
using Esagaweb.Maui.Android.Controls.Common;

namespace Esagaweb.Maui.Android.Controls.Selection;

/// <summary>Fila M3: CheckBox nativo themado + etiqueta Nunito. Toda la fila alterna el valor.</summary>
public partial class M3CheckBoxRow : ContentView
{
    /// <summary>Propiedad enlazable para <see cref="Text"/>.</summary>
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(M3CheckBoxRow), string.Empty,
            propertyChanged: (b, _, n) => ((M3CheckBoxRow)b).RowLabel.Text = (string?)n ?? string.Empty);

    /// <summary>Propiedad enlazable para <see cref="IsChecked"/>.</summary>
    public static readonly BindableProperty IsCheckedProperty =
        BindableProperty.Create(nameof(IsChecked), typeof(bool), typeof(M3CheckBoxRow), false,
            BindingMode.TwoWay, propertyChanged: (b, _, n) => ((M3CheckBoxRow)b).InnerCheck.IsChecked = (bool)n);

    /// <summary>Propiedad enlazable para <see cref="Command"/>.</summary>
    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(M3CheckBoxRow), null);

    /// <summary>Etiqueta de la fila.</summary>
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    /// <summary>Si está marcada. TwoWay: sirve para cargar y leer de vuelta (ej. formularios API).</summary>
    public bool IsChecked
    {
        get => (bool)GetValue(IsCheckedProperty);
        set => SetValue(IsCheckedProperty, value);
    }

    /// <summary>Comando al cambiar (recibe el nuevo valor bool).</summary>
    public ICommand? Command
    {
        get => (ICommand?)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    /// <summary>Se dispara al cambiar (con el nuevo valor).</summary>
    public event EventHandler<bool>? CheckedChanged;

    /// <summary>Crea una nueva instancia.</summary>
    public M3CheckBoxRow()
    {
        InitializeComponent();
        RowLabel.FontFamily = "NunitoRegular";
        RowLabel.Text = Text;
        InnerCheck.IsChecked = IsChecked;
        InnerCheck.CheckedChanged += (_, e) =>
        {
            IsChecked = e.Value;
            if (Command?.CanExecute(e.Value) == true)
                Command.Execute(e.Value);
            CheckedChanged?.Invoke(this, e.Value);
        };
        Root.GestureRecognizers.Add(new TapGestureRecognizer
        {
            Command = new Command(() => InnerCheck.IsChecked = !InnerCheck.IsChecked)
        });
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
        M3ControlHelper.SetThemed(InnerCheck, CheckBox.ColorProperty, "M3Primary", "M3DarkPrimary");
        Opacity = IsEnabled ? 1 : 0.6;
    }
}
