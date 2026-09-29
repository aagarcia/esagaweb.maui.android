using Esagaweb.Maui.Android.Controls.Common;

namespace Esagaweb.Maui.Android.Controls.Selection;

/// <summary>Fila M3: RadioButton nativo themado + etiqueta Nunito (agrupar con GroupName como en Flutter RadioListTile).</summary>
public partial class M3RadioRow : ContentView
{
    /// <summary>Propiedad enlazable para <see cref="Text"/>.</summary>
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(M3RadioRow), string.Empty,
            propertyChanged: (b, _, n) => ((M3RadioRow)b).RowLabel.Text = (string?)n ?? string.Empty);

    /// <summary>Propiedad enlazable para <see cref="IsSelected"/>.</summary>
    public static readonly BindableProperty IsSelectedProperty =
        BindableProperty.Create(nameof(IsSelected), typeof(bool), typeof(M3RadioRow), false,
            BindingMode.TwoWay, propertyChanged: (b, _, n) => ((M3RadioRow)b).InnerRadio.IsChecked = (bool)n);

    /// <summary>Propiedad enlazable para <see cref="GroupName"/>.</summary>
    public static readonly BindableProperty GroupNameProperty =
        BindableProperty.Create(nameof(GroupName), typeof(string), typeof(M3RadioRow), string.Empty,
            propertyChanged: (b, _, n) => ((M3RadioRow)b).InnerRadio.GroupName = (string?)n ?? string.Empty);

    /// <summary>Propiedad enlazable para <see cref="Value"/>.</summary>
    public static readonly BindableProperty ValueProperty =
        BindableProperty.Create(nameof(Value), typeof(object), typeof(M3RadioRow), null);

    /// <summary>Etiqueta de la fila.</summary>
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    /// <summary>Si está seleccionada. TwoWay: sirve para cargar y leer de vuelta.</summary>
    public bool IsSelected
    {
        get => (bool)GetValue(IsSelectedProperty);
        set => SetValue(IsSelectedProperty, value);
    }

    /// <summary>Nombre del grupo: solo una opción del mismo grupo puede estar seleccionada.</summary>
    public string GroupName
    {
        get => (string)GetValue(GroupNameProperty);
        set => SetValue(GroupNameProperty, value);
    }

    /// <summary>Valor asociado a esta opción (se usa tal cual, sin interpretar).</summary>
    public object? Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    /// <summary>Se dispara al cambiar la selección (con el nuevo valor).</summary>
    public event EventHandler<bool>? SelectedChanged;

    /// <summary>Crea una nueva instancia.</summary>
    public M3RadioRow()
    {
        InitializeComponent();
        RowLabel.FontFamily = "NunitoRegular";
        RowLabel.Text = Text;
        InnerRadio.GroupName = GroupName;
        InnerRadio.IsChecked = IsSelected;
        InnerRadio.CheckedChanged += (_, e) =>
        {
            IsSelected = e.Value;
            SelectedChanged?.Invoke(this, e.Value);
        };
        Root.GestureRecognizers.Add(new TapGestureRecognizer
        {
            Command = new Command(() => InnerRadio.IsChecked = true)
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
        M3ControlHelper.SetThemed(InnerRadio, RadioButton.TextColorProperty, "M3OnSurface", "M3DarkOnSurface");
        M3ControlHelper.SetThemed(InnerRadio, RadioButton.BorderColorProperty, "M3Outline", "M3DarkOutline");
        Opacity = IsEnabled ? 1 : 0.6;
    }
}
