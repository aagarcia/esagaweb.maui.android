using System.Windows.Input;
using Esagaweb.Maui.Android.Controls.Common;

namespace Esagaweb.Maui.Android.Controls.Chip;

/// <summary>
/// Chip M3 compacto: Assist (tap), Filter (tap alterna Selected),
/// Input (muestra cierre). Icono leading con MaterialSymbols blindado.
/// </summary>
public partial class M3Chip : ContentView
{
    /// <summary>Propiedad enlazable para <see cref="Text"/>.</summary>
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(M3Chip), string.Empty,
            propertyChanged: (b, _, n) => ((M3Chip)b).TextLabel.Text = (string?)n ?? string.Empty);

    /// <summary>Propiedad enlazable para <see cref="Type"/>.</summary>
    public static readonly BindableProperty TypeProperty =
        BindableProperty.Create(nameof(Type), typeof(M3ChipType), typeof(M3Chip), M3ChipType.Assist,
            propertyChanged: (b, _, _) => ((M3Chip)b).UpdateAppearance());

    /// <summary>Propiedad enlazable para <see cref="Selected"/>.</summary>
    public static readonly BindableProperty SelectedProperty =
        BindableProperty.Create(nameof(Selected), typeof(bool), typeof(M3Chip), false,
            BindingMode.TwoWay, propertyChanged: (b, _, _) => ((M3Chip)b).UpdateAppearance());

    /// <summary>Propiedad enlazable para <see cref="Glyph"/>.</summary>
    public static readonly BindableProperty GlyphProperty =
        BindableProperty.Create(nameof(Glyph), typeof(string), typeof(M3Chip), string.Empty,
            propertyChanged: (b, _, _) => ((M3Chip)b).UpdateGlyph());

    /// <summary>Propiedad enlazable para <see cref="Command"/>.</summary>
    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(M3Chip), null);

    /// <summary>Texto del chip.</summary>
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    /// <summary>Comportamiento: Assist, Filter o Input. Default Assist.</summary>
    public M3ChipType Type
    {
        get => (M3ChipType)GetValue(TypeProperty);
        set => SetValue(TypeProperty, value);
    }

    /// <summary>Seleccionado (relevante en Filter). TwoWay.</summary>
    public bool Selected
    {
        get => (bool)GetValue(SelectedProperty);
        set => SetValue(SelectedProperty, value);
    }

    /// <summary>Glyph MaterialSymbols opcional (ej. M3Icons.Check, M3Icons.Close lo pone Input solo).</summary>
    public string Glyph
    {
        get => (string)GetValue(GlyphProperty);
        set => SetValue(GlyphProperty, value);
    }

    /// <summary>Comando al tocar (recibe Selected en Filter).</summary>
    public ICommand? Command
    {
        get => (ICommand?)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    /// <summary>Se dispara al tocar el chip.</summary>
    public event EventHandler? Tapped;
    /// <summary>Se dispara al tocar el cierre (solo Input).</summary>
    public event EventHandler? Closed;

    /// <summary>Crea una nueva instancia.</summary>
    public M3Chip()
    {
        InitializeComponent();
        GlyphLabel.FontFamily = "MaterialSymbols";
        CloseLabel.FontFamily = "MaterialSymbols";
        CloseLabel.Text = M3Icons.Close;
        TextLabel.FontFamily = "NunitoRegular";
        TextLabel.Text = Text;
        UpdateGlyph();
        UpdateAppearance();
    }

    private void OnTapped(object? sender, TappedEventArgs e)
    {
        if (!IsEnabled) return;
        if (Type == M3ChipType.Filter)
            Selected = !Selected;
        if (Command?.CanExecute(Selected) == true)
            Command.Execute(Selected);
        Tapped?.Invoke(this, EventArgs.Empty);
    }

    private void OnCloseTapped(object? sender, TappedEventArgs e)
    {
        if (!IsEnabled) return;
        Closed?.Invoke(this, EventArgs.Empty);
    }

    private void UpdateGlyph()
    {
        GlyphLabel.FontFamily = "MaterialSymbols";
        GlyphLabel.Text = Glyph;
        GlyphLabel.IsVisible = !string.IsNullOrEmpty(Glyph);
    }

    private void UpdateAppearance()
    {
        CloseLabel.IsVisible = Type == M3ChipType.Input;
        var on = Selected && Type == M3ChipType.Filter;
        if (!IsEnabled)
        {
            RootBorder.Opacity = 0.6;
            M3ControlHelper.SetThemed(RootBorder, Border.BackgroundColorProperty, "M3SurfaceVariant", "M3DarkSurfaceVariant");
            M3ControlHelper.SetThemed(RootBorder, Border.StrokeProperty, "M3Outline", "M3DarkOutline");
            M3ControlHelper.SetThemed(TextLabel, Label.TextColorProperty, "M3OnSurfaceVariant", "M3DarkOnSurface");
            return;
        }
        RootBorder.Opacity = 1;
        if (on)
        {
            M3ControlHelper.SetThemed(RootBorder, Border.BackgroundColorProperty, "M3SecondaryContainer", "M3DarkPrimaryContainer");
            RootBorder.Stroke = Colors.Transparent;
            M3ControlHelper.SetThemed(TextLabel, Label.TextColorProperty, "M3OnPrimaryContainer", "M3DarkPrimary");
            M3ControlHelper.SetThemed(GlyphLabel, Label.TextColorProperty, "M3OnPrimaryContainer", "M3DarkPrimary");
        }
        else
        {
            RootBorder.BackgroundColor = Colors.Transparent;
            M3ControlHelper.SetThemed(RootBorder, Border.StrokeProperty, "M3Outline", "M3DarkOutline");
            M3ControlHelper.SetThemed(TextLabel, Label.TextColorProperty, "M3OnSurfaceVariant", "M3DarkOnSurface");
            M3ControlHelper.SetThemed(GlyphLabel, Label.TextColorProperty, "M3Primary", "M3DarkPrimary");
        }
        M3ControlHelper.SetThemed(CloseLabel, Label.TextColorProperty, "M3OnSurfaceVariant", "M3DarkOnSurface");
    }
}
