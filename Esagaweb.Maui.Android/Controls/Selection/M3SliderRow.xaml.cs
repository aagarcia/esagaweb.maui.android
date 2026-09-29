using Esagaweb.Maui.Android.Controls.Common;

namespace Esagaweb.Maui.Android.Controls.Selection;

/// <summary>Fila M3: etiqueta + valor + Slider nativo themado (como Flutter Slider con label).</summary>
public partial class M3SliderRow : ContentView
{
    /// <summary>Propiedad enlazable para <see cref="Text"/>.</summary>
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(M3SliderRow), string.Empty,
            propertyChanged: (b, _, n) => ((M3SliderRow)b).RowLabel.Text = (string?)n ?? string.Empty);

    /// <summary>Propiedad enlazable para <see cref="Value"/>.</summary>
    public static readonly BindableProperty ValueProperty =
        BindableProperty.Create(nameof(Value), typeof(double), typeof(M3SliderRow), 0.0,
            BindingMode.TwoWay, propertyChanged: (b, _, n) =>
            {
                var self = (M3SliderRow)b;
                if (Math.Abs(self.InnerSlider.Value - (double)n) > double.Epsilon)
                    self.InnerSlider.Value = (double)n;
                self.UpdateValueLabel();
            });

    /// <summary>Propiedad enlazable para <see cref="Minimum"/>.</summary>
    public static readonly BindableProperty MinimumProperty =
        BindableProperty.Create(nameof(Minimum), typeof(double), typeof(M3SliderRow), 0.0,
            propertyChanged: (b, _, n) => ((M3SliderRow)b).InnerSlider.Minimum = (double)n);

    /// <summary>Propiedad enlazable para <see cref="Maximum"/>.</summary>
    public static readonly BindableProperty MaximumProperty =
        BindableProperty.Create(nameof(Maximum), typeof(double), typeof(M3SliderRow), 100.0,
            propertyChanged: (b, _, n) => ((M3SliderRow)b).InnerSlider.Maximum = (double)n);

    /// <summary>Propiedad enlazable para <see cref="ShowValue"/>.</summary>
    public static readonly BindableProperty ShowValueProperty =
        BindableProperty.Create(nameof(ShowValue), typeof(bool), typeof(M3SliderRow), true,
            propertyChanged: (b, _, _) => ((M3SliderRow)b).UpdateValueLabel());

    /// <summary>Etiqueta de la fila.</summary>
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    /// <summary>Valor actual. TwoWay: sirve para cargar y leer de vuelta.</summary>
    public double Value
    {
        get => (double)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    /// <summary>Valor mínimo. Default 0.</summary>
    public double Minimum
    {
        get => (double)GetValue(MinimumProperty);
        set => SetValue(MinimumProperty, value);
    }

    /// <summary>Valor máximo. Default 100.</summary>
    public double Maximum
    {
        get => (double)GetValue(MaximumProperty);
        set => SetValue(MaximumProperty, value);
    }

    /// <summary>Si muestra el valor numérico a la derecha. Default true.</summary>
    public bool ShowValue
    {
        get => (bool)GetValue(ShowValueProperty);
        set => SetValue(ShowValueProperty, value);
    }

    /// <summary>Se dispara al cambiar el valor (con el nuevo valor).</summary>
    public event EventHandler<double>? ValueChanged;

    /// <summary>Crea una nueva instancia.</summary>
    public M3SliderRow()
    {
        InitializeComponent();
        RowLabel.FontFamily = "NunitoRegular";
        ValueLabel.FontFamily = "NunitoSemiBold";
        RowLabel.Text = Text;
        InnerSlider.Minimum = Minimum;
        InnerSlider.Maximum = Maximum;
        InnerSlider.Value = Value;
        InnerSlider.ValueChanged += (_, e) =>
        {
            Value = e.NewValue;
            UpdateValueLabel();
            ValueChanged?.Invoke(this, e.NewValue);
        };
        M3ControlHelper.SetThemed(RowLabel, Label.TextColorProperty, "M3OnSurface", "M3DarkOnSurface");
        M3ControlHelper.SetThemed(ValueLabel, Label.TextColorProperty, "M3Primary", "M3DarkPrimary");
        UpdateValueLabel();
    }

    private void UpdateValueLabel()
    {
        ValueLabel.Text = $"{Value:F0}";
        ValueLabel.IsVisible = ShowValue;
    }
}
