using System.Windows.Input;
using Esagaweb.Maui.Android.Controls.Common;
using Microsoft.Maui.Controls.Shapes;

namespace Esagaweb.Maui.Android.Controls.TextField;

/// <summary>
/// Campo de texto M3 (Filled / Outlined), guiado por Flutter TextField:
/// label, placeholder, helper/error, leading/trailing glyph (MaterialSymbols),
/// password, teclado, longitudes. Toda la fila es tocable para enfocar.
/// </summary>
public partial class M3TextField : ContentView
{
    /// <summary>Propiedad enlazable para <see cref="Text"/>.</summary>
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(M3TextField), string.Empty,
            BindingMode.TwoWay, propertyChanged: (b, _, n) =>
            {
                var self = (M3TextField)b;
                if (self.InnerEntry.Text != (string?)n)
                    self.InnerEntry.Text = (string?)n ?? string.Empty;
            });

    /// <summary>Propiedad enlazable para <see cref="Label"/>.</summary>
    public static readonly BindableProperty LabelProperty =
        BindableProperty.Create(nameof(Label), typeof(string), typeof(M3TextField), string.Empty,
            propertyChanged: (b, _, _) => ((M3TextField)b).UpdateLabels());

    /// <summary>Propiedad enlazable para <see cref="Placeholder"/>.</summary>
    public static readonly BindableProperty PlaceholderProperty =
        BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(M3TextField), string.Empty,
            propertyChanged: (b, _, n) => ((M3TextField)b).InnerEntry.Placeholder = (string?)n ?? string.Empty);

    /// <summary>Propiedad enlazable para <see cref="HelperText"/>.</summary>
    public static readonly BindableProperty HelperTextProperty =
        BindableProperty.Create(nameof(HelperText), typeof(string), typeof(M3TextField), string.Empty,
            propertyChanged: (b, _, _) => ((M3TextField)b).UpdateLabels());

    /// <summary>Propiedad enlazable para <see cref="ErrorText"/>.</summary>
    public static readonly BindableProperty ErrorTextProperty =
        BindableProperty.Create(nameof(ErrorText), typeof(string), typeof(M3TextField), string.Empty,
            propertyChanged: (b, _, _) => ((M3TextField)b).UpdateLabels());

    /// <summary>Propiedad enlazable para <see cref="HasError"/>.</summary>
    public static readonly BindableProperty HasErrorProperty =
        BindableProperty.Create(nameof(HasError), typeof(bool), typeof(M3TextField), false,
            propertyChanged: (b, _, _) => ((M3TextField)b).UpdateAppearance());

    /// <summary>Propiedad enlazable para <see cref="Variant"/>.</summary>
    public static readonly BindableProperty VariantProperty =
        BindableProperty.Create(nameof(Variant), typeof(M3TextFieldVariant), typeof(M3TextField),
            M3TextFieldVariant.Filled, propertyChanged: (b, _, _) => ((M3TextField)b).UpdateAppearance());

    /// <summary>Propiedad enlazable para <see cref="LeadingGlyph"/>.</summary>
    public static readonly BindableProperty LeadingGlyphProperty =
        BindableProperty.Create(nameof(LeadingGlyph), typeof(string), typeof(M3TextField), string.Empty,
            propertyChanged: (b, _, _) => ((M3TextField)b).UpdateGlyphs());

    /// <summary>Propiedad enlazable para <see cref="TrailingGlyph"/>.</summary>
    public static readonly BindableProperty TrailingGlyphProperty =
        BindableProperty.Create(nameof(TrailingGlyph), typeof(string), typeof(M3TextField), string.Empty,
            propertyChanged: (b, _, _) => ((M3TextField)b).UpdateGlyphs());

    /// <summary>Propiedad enlazable para <see cref="IsPassword"/>.</summary>
    public static readonly BindableProperty IsPasswordProperty =
        BindableProperty.Create(nameof(IsPassword), typeof(bool), typeof(M3TextField), false,
            propertyChanged: (b, _, n) => ((M3TextField)b).InnerEntry.IsPassword = (bool)n);

    /// <summary>Propiedad enlazable para <see cref="Keyboard"/>.</summary>
    public static readonly BindableProperty KeyboardProperty =
        BindableProperty.Create(nameof(Keyboard), typeof(Keyboard), typeof(M3TextField), Keyboard.Default,
            propertyChanged: (b, _, n) => ((M3TextField)b).InnerEntry.Keyboard = (Keyboard)n);

    /// <summary>Propiedad enlazable para <see cref="MaxLength"/>.</summary>
    public static readonly BindableProperty MaxLengthProperty =
        BindableProperty.Create(nameof(MaxLength), typeof(int), typeof(M3TextField), int.MaxValue,
            propertyChanged: (b, _, n) => ((M3TextField)b).InnerEntry.MaxLength = (int)n);

    /// <summary>Propiedad enlazable para <see cref="IsReadOnly"/>.</summary>
    public static readonly BindableProperty IsReadOnlyProperty =
        BindableProperty.Create(nameof(IsReadOnly), typeof(bool), typeof(M3TextField), false,
            propertyChanged: (b, _, n) => ((M3TextField)b).InnerEntry.IsReadOnly = (bool)n);

    /// <summary>Propiedad enlazable para <see cref="TrailingCommand"/>.</summary>
    public static readonly BindableProperty TrailingCommandProperty =
        BindableProperty.Create(nameof(TrailingCommand), typeof(ICommand), typeof(M3TextField), null);

    /// <summary>Texto escrito. TwoWay: sirve para cargar y leer de vuelta (ej. formularios API).</summary>
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    /// <summary>Etiqueta superior (12sp).</summary>
    public string Label
    {
        get => (string)GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }

    /// <summary>Texto de ejemplo cuando está vacío.</summary>
    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    /// <summary>Texto de ayuda bajo el campo (se oculta si hay error).</summary>
    public string HelperText
    {
        get => (string)GetValue(HelperTextProperty);
        set => SetValue(HelperTextProperty, value);
    }

    /// <summary>Texto de error (requiere <see cref="HasError"/>).</summary>
    public string ErrorText
    {
        get => (string)GetValue(ErrorTextProperty);
        set => SetValue(ErrorTextProperty, value);
    }

    /// <summary>Si muestra estado de error. Default false.</summary>
    public bool HasError
    {
        get => (bool)GetValue(HasErrorProperty);
        set => SetValue(HasErrorProperty, value);
    }

    /// <summary>Variante visual. Default Filled.</summary>
    public M3TextFieldVariant Variant
    {
        get => (M3TextFieldVariant)GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
    }

    /// <summary>Glyph MaterialSymbols a la izquierda (ej. M3Icons.Search).</summary>
    public string LeadingGlyph
    {
        get => (string)GetValue(LeadingGlyphProperty);
        set => SetValue(LeadingGlyphProperty, value);
    }

    /// <summary>Glyph MaterialSymbols a la derecha, tocable vía TrailingCommand.</summary>
    public string TrailingGlyph
    {
        get => (string)GetValue(TrailingGlyphProperty);
        set => SetValue(TrailingGlyphProperty, value);
    }

    /// <summary>Si oculta lo escrito (contraseñas). Default false.</summary>
    public bool IsPassword
    {
        get => (bool)GetValue(IsPasswordProperty);
        set => SetValue(IsPasswordProperty, value);
    }

    /// <summary>Teclado a mostrar. Default <see cref="Keyboard.Default"/>.</summary>
    public Keyboard Keyboard
    {
        get => (Keyboard)GetValue(KeyboardProperty);
        set => SetValue(KeyboardProperty, value);
    }

    /// <summary>Máximo de caracteres. Default sin límite.</summary>
    public int MaxLength
    {
        get => (int)GetValue(MaxLengthProperty);
        set => SetValue(MaxLengthProperty, value);
    }

    /// <summary>Si es solo lectura. Default false.</summary>
    public bool IsReadOnly
    {
        get => (bool)GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, value);
    }

    /// <summary>Comando al tocar el icono trailing (ej. limpiar, mostrar contraseña).</summary>
    public ICommand? TrailingCommand
    {
        get => (ICommand?)GetValue(TrailingCommandProperty);
        set => SetValue(TrailingCommandProperty, value);
    }

    /// <summary>Se dispara al cambiar el texto.</summary>
    public event EventHandler<TextChangedEventArgs>? TextChanged;
    /// <summary>Se dispara al confirmar (enter/listón).</summary>
    public event EventHandler? Completed;

    private bool _focused;

    /// <summary>Crea una nueva instancia.</summary>
    public M3TextField()
    {
        InitializeComponent();
        // Blindaje de fuentes: iconos SIEMPRE MaterialSymbols, texto Nunito.
        LeadingGlyphLabel.FontFamily = "MaterialSymbols";
        TrailingGlyphLabel.FontFamily = "MaterialSymbols";
        FieldLabel.FontFamily = "NunitoSemiBold";
        SupportLabel.FontFamily = "NunitoRegular";
        InnerEntry.FontFamily = "NunitoRegular";
        InnerEntry.Text = Text;
        InnerEntry.Placeholder = Placeholder;
        InnerEntry.IsPassword = IsPassword;
        InnerEntry.Keyboard = Keyboard;
        InnerEntry.MaxLength = MaxLength;
        InnerEntry.IsReadOnly = IsReadOnly;
        InnerEntry.TextChanged += (_, e) =>
        {
            if (Text != e.NewTextValue)
                Text = e.NewTextValue;
            TextChanged?.Invoke(this, e);
        };
        InnerEntry.Completed += (_, _) => Completed?.Invoke(this, EventArgs.Empty);
        InnerEntry.Focused += (_, _) => { _focused = true; UpdateAppearance(); };
        InnerEntry.Unfocused += (_, _) => { _focused = false; UpdateAppearance(); };
        UpdateGlyphs();
        UpdateLabels();
        UpdateAppearance();
    }

    /// <summary>Pone el foco en el campo.</summary>
    public void FocusField() => InnerEntry.Focus();

    /// <inheritdoc/>
    protected override void OnPropertyChanged(string? propertyName = null)
    {
        base.OnPropertyChanged(propertyName);
        if (propertyName == nameof(IsEnabled))
            UpdateAppearance();
    }

    private void OnTrailingTapped(object? sender, TappedEventArgs e)
    {
        if (TrailingCommand?.CanExecute(null) == true)
            TrailingCommand.Execute(null);
    }

    private void UpdateGlyphs()
    {
        // Reafirmar: el estilo implícito Nunito nunca debe pisar los glyphs.
        LeadingGlyphLabel.FontFamily = "MaterialSymbols";
        TrailingGlyphLabel.FontFamily = "MaterialSymbols";
        LeadingGlyphLabel.Text = LeadingGlyph;
        LeadingGlyphLabel.IsVisible = !string.IsNullOrEmpty(LeadingGlyph);
        TrailingGlyphLabel.Text = TrailingGlyph;
        TrailingGlyphLabel.IsVisible = !string.IsNullOrEmpty(TrailingGlyph);
    }

    private void UpdateLabels()
    {
        FieldLabel.Text = Label;
        FieldLabel.IsVisible = !string.IsNullOrEmpty(Label);
        var showError = HasError && !string.IsNullOrEmpty(ErrorText);
        SupportLabel.Text = showError ? ErrorText : HelperText;
        SupportLabel.IsVisible = !string.IsNullOrEmpty(SupportLabel.Text);
        UpdateAppearance();
    }

    private void UpdateAppearance()
    {
        var h = M3ControlHelper.ResDouble("M3TextFieldHeight", 56);
        var r = M3ControlHelper.ResDouble("M3TextFieldCornerRadius", 4);
        FieldBorder.HeightRequest = h;
        if (FieldBorder.StrokeShape is RoundRectangle rr)
            rr.CornerRadius = r;

        var enabled = IsEnabled;
        var error = HasError;
        Opacity = enabled ? 1 : 0.6;
        InnerEntry.IsEnabled = enabled;

        // Colores de contenido.
        if (error)
        {
            M3ControlHelper.SetThemed(FieldLabel, global::Microsoft.Maui.Controls.Label.TextColorProperty, "M3Error", "M3DarkError");
            M3ControlHelper.SetThemed(SupportLabel, global::Microsoft.Maui.Controls.Label.TextColorProperty, "M3Error", "M3DarkError");
            M3ControlHelper.SetThemed(LeadingGlyphLabel, global::Microsoft.Maui.Controls.Label.TextColorProperty, "M3OnSurfaceVariant", "M3DarkOnSurface");
            M3ControlHelper.SetThemed(TrailingGlyphLabel, global::Microsoft.Maui.Controls.Label.TextColorProperty, "M3OnSurfaceVariant", "M3DarkOnSurface");
            M3ControlHelper.SetThemed(InnerEntry, Entry.TextColorProperty, "M3OnSurface", "M3DarkOnSurface");
        }
        else
        {
            M3ControlHelper.SetThemed(FieldLabel, global::Microsoft.Maui.Controls.Label.TextColorProperty,
                _focused ? "M3Primary" : "M3OnSurfaceVariant",
                _focused ? "M3DarkPrimary" : "M3DarkOnSurface");
            M3ControlHelper.SetThemed(SupportLabel, global::Microsoft.Maui.Controls.Label.TextColorProperty, "M3OnSurfaceVariant", "M3DarkOnSurface");
            M3ControlHelper.SetThemed(LeadingGlyphLabel, global::Microsoft.Maui.Controls.Label.TextColorProperty, "M3OnSurfaceVariant", "M3DarkOnSurface");
            M3ControlHelper.SetThemed(TrailingGlyphLabel, global::Microsoft.Maui.Controls.Label.TextColorProperty, "M3OnSurfaceVariant", "M3DarkOnSurface");
            M3ControlHelper.SetThemed(InnerEntry, Entry.TextColorProperty, "M3OnSurface", "M3DarkOnSurface");
            M3ControlHelper.SetThemed(InnerEntry, Entry.PlaceholderColorProperty, "M3Outline", "M3DarkOutline");
        }

        if (Variant == M3TextFieldVariant.Filled)
        {
            M3ControlHelper.SetThemed(FieldBorder, Border.BackgroundColorProperty, "M3SurfaceVariant", "M3DarkSurfaceVariant");
            FieldBorder.StrokeThickness = 0;
            FieldBorder.Stroke = Colors.Transparent;
            // Indicador inferior.
            IndicatorLine.IsVisible = true;
            IndicatorLine.HeightRequest = _focused
                ? M3ControlHelper.ResDouble("M3TextFieldIndicatorActive", 2)
                : M3ControlHelper.ResDouble("M3TextFieldIndicatorInactive", 1);
            if (error)
                M3ControlHelper.SetThemed(IndicatorLine, BoxView.ColorProperty, "M3Error", "M3DarkError");
            else if (_focused)
                M3ControlHelper.SetThemed(IndicatorLine, BoxView.ColorProperty, "M3Primary", "M3DarkPrimary");
            else
                M3ControlHelper.SetThemed(IndicatorLine, BoxView.ColorProperty, "M3Outline", "M3DarkOutline");
        }
        else
        {
            FieldBorder.BackgroundColor = Colors.Transparent;
            IndicatorLine.IsVisible = false;
            FieldBorder.StrokeThickness = _focused ? 2 : 1;
            if (!enabled)
                M3ControlHelper.SetThemed(FieldBorder, Border.StrokeProperty, "M3Outline", "M3DarkOutline");
            else if (error)
                M3ControlHelper.SetThemed(FieldBorder, Border.StrokeProperty, "M3Error", "M3DarkError");
            else if (_focused)
                M3ControlHelper.SetThemed(FieldBorder, Border.StrokeProperty, "M3Primary", "M3DarkPrimary");
            else
                M3ControlHelper.SetThemed(FieldBorder, Border.StrokeProperty, "M3Outline", "M3DarkOutline");
        }
    }
}
