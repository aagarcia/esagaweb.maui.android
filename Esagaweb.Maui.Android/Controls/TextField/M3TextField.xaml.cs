using System.Windows.Input;
using Esagaweb.Maui.Android.Controls.Common;
using Microsoft.Maui.Controls.Shapes;

namespace Esagaweb.Maui.Android.Controls.TextField;

/// <summary>Material text field with filled and outlined variants, label, placeholder, and helper or error text.</summary>
/// <remarks>The whole row can be tapped to focus the field. Leading and trailing glyphs use Material Symbols.</remarks>
/// <example>
/// <code language="xaml"><![CDATA[
/// <ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
///              xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
///              xmlns:field="clr-namespace:Esagaweb.Maui.Android.Controls.TextField;assembly=Esagaweb.Maui.Android">
///     <field:M3TextField Label="Name"
///                        Placeholder="Type your name"
///                        HelperText="Filled with a leading glyph" />
///     <field:M3TextField Label="Mail"
///                        Placeholder="name@mail.com"
///                        Variant="Outlined"
///                        IsPassword="False"
///                        MaxLength="100" />
/// </ContentPage>
/// ]]></code>
/// </example>
public partial class M3TextField : ContentView
{
    /// <summary>Identifies the <see cref="Text"/> bindable property.</summary>
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(M3TextField), string.Empty,
            BindingMode.TwoWay, propertyChanged: (b, _, n) =>
            {
                var self = (M3TextField)b;
                if (self.InnerEntry.Text != (string?)n)
                    self.InnerEntry.Text = (string?)n ?? string.Empty;
            });

    /// <summary>Identifies the <see cref="Label"/> bindable property.</summary>
    public static readonly BindableProperty LabelProperty =
        BindableProperty.Create(nameof(Label), typeof(string), typeof(M3TextField), string.Empty,
            propertyChanged: (b, _, _) => ((M3TextField)b).UpdateLabels());

    /// <summary>Identifies the <see cref="Placeholder"/> bindable property.</summary>
    public static readonly BindableProperty PlaceholderProperty =
        BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(M3TextField), string.Empty,
            propertyChanged: (b, _, n) => ((M3TextField)b).InnerEntry.Placeholder = (string?)n ?? string.Empty);

    /// <summary>Identifies the <see cref="HelperText"/> bindable property.</summary>
    public static readonly BindableProperty HelperTextProperty =
        BindableProperty.Create(nameof(HelperText), typeof(string), typeof(M3TextField), string.Empty,
            propertyChanged: (b, _, _) => ((M3TextField)b).UpdateLabels());

    /// <summary>Identifies the <see cref="ErrorText"/> bindable property.</summary>
    public static readonly BindableProperty ErrorTextProperty =
        BindableProperty.Create(nameof(ErrorText), typeof(string), typeof(M3TextField), string.Empty,
            propertyChanged: (b, _, _) => ((M3TextField)b).UpdateLabels());

    /// <summary>Identifies the <see cref="HasError"/> bindable property.</summary>
    public static readonly BindableProperty HasErrorProperty =
        BindableProperty.Create(nameof(HasError), typeof(bool), typeof(M3TextField), false,
            propertyChanged: (b, _, _) => ((M3TextField)b).UpdateAppearance());

    /// <summary>Identifies the <see cref="Variant"/> bindable property.</summary>
    public static readonly BindableProperty VariantProperty =
        BindableProperty.Create(nameof(Variant), typeof(M3TextFieldVariant), typeof(M3TextField),
            M3TextFieldVariant.Filled, propertyChanged: (b, _, _) => ((M3TextField)b).UpdateAppearance());

    /// <summary>Identifies the <see cref="LeadingGlyph"/> bindable property.</summary>
    public static readonly BindableProperty LeadingGlyphProperty =
        BindableProperty.Create(nameof(LeadingGlyph), typeof(string), typeof(M3TextField), string.Empty,
            propertyChanged: (b, _, _) => ((M3TextField)b).UpdateGlyphs());

    /// <summary>Identifies the <see cref="TrailingGlyph"/> bindable property.</summary>
    public static readonly BindableProperty TrailingGlyphProperty =
        BindableProperty.Create(nameof(TrailingGlyph), typeof(string), typeof(M3TextField), string.Empty,
            propertyChanged: (b, _, _) => ((M3TextField)b).UpdateGlyphs());

    /// <summary>Identifies the <see cref="IsPassword"/> bindable property.</summary>
    public static readonly BindableProperty IsPasswordProperty =
        BindableProperty.Create(nameof(IsPassword), typeof(bool), typeof(M3TextField), false,
            propertyChanged: (b, _, n) => ((M3TextField)b).InnerEntry.IsPassword = (bool)n);

    /// <summary>Identifies the <see cref="Keyboard"/> bindable property.</summary>
    public static readonly BindableProperty KeyboardProperty =
        BindableProperty.Create(nameof(Keyboard), typeof(Keyboard), typeof(M3TextField), Keyboard.Default,
            propertyChanged: (b, _, n) => ((M3TextField)b).InnerEntry.Keyboard = (Keyboard)n);

    /// <summary>Identifies the <see cref="MaxLength"/> bindable property.</summary>
    public static readonly BindableProperty MaxLengthProperty =
        BindableProperty.Create(nameof(MaxLength), typeof(int), typeof(M3TextField), int.MaxValue,
            propertyChanged: (b, _, n) => ((M3TextField)b).InnerEntry.MaxLength = (int)n);

    /// <summary>Identifies the <see cref="IsReadOnly"/> bindable property.</summary>
    public static readonly BindableProperty IsReadOnlyProperty =
        BindableProperty.Create(nameof(IsReadOnly), typeof(bool), typeof(M3TextField), false,
            propertyChanged: (b, _, n) => ((M3TextField)b).InnerEntry.IsReadOnly = (bool)n);

    /// <summary>Identifies the <see cref="TrailingCommand"/> bindable property.</summary>
    public static readonly BindableProperty TrailingCommandProperty =
        BindableProperty.Create(nameof(TrailingCommand), typeof(ICommand), typeof(M3TextField), null);

    /// <summary>Gets or sets the entered text. The default is empty.</summary>
    /// <remarks>Bindable property. Two-way binding supports loading and reading a value back.</remarks>
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    /// <summary>Gets or sets the top label. The default is empty.</summary>
    /// <remarks>Bindable property. Changes update the visible labels.</remarks>
    public string Label
    {
        get => (string)GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }

    /// <summary>Gets or sets the sample text shown when the field is empty. The default is empty.</summary>
    /// <remarks>Bindable property. Changes update the inner entry placeholder.</remarks>
    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    /// <summary>Gets or sets the help text below the field. The default is empty.</summary>
    /// <remarks>Bindable property. It is hidden when an error is shown.</remarks>
    public string HelperText
    {
        get => (string)GetValue(HelperTextProperty);
        set => SetValue(HelperTextProperty, value);
    }

    /// <summary>Gets or sets the error text. The default is empty.</summary>
    /// <remarks>Bindable property. It is shown when <see cref="HasError"/> is true.</remarks>
    public string ErrorText
    {
        get => (string)GetValue(ErrorTextProperty);
        set => SetValue(ErrorTextProperty, value);
    }

    /// <summary>Gets or sets a value indicating whether the error state is shown. The default is false.</summary>
    /// <remarks>Bindable property. Changes update the field appearance.</remarks>
    public bool HasError
    {
        get => (bool)GetValue(HasErrorProperty);
        set => SetValue(HasErrorProperty, value);
    }

    /// <summary>Gets or sets the visual variant. The default is <see cref="M3TextFieldVariant.Filled"/>.</summary>
    /// <remarks>Bindable property. Changes update the field appearance.</remarks>
    public M3TextFieldVariant Variant
    {
        get => (M3TextFieldVariant)GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
    }

    /// <summary>Gets or sets the Material Symbols glyph on the left side. The default is empty.</summary>
    /// <remarks>Bindable property. Changes update the visible glyphs.</remarks>
    public string LeadingGlyph
    {
        get => (string)GetValue(LeadingGlyphProperty);
        set => SetValue(LeadingGlyphProperty, value);
    }

    /// <summary>Gets or sets the Material Symbols glyph on the right side. The default is empty.</summary>
    /// <remarks>Bindable property. Tapping it runs <see cref="TrailingCommand"/>.</remarks>
    public string TrailingGlyph
    {
        get => (string)GetValue(TrailingGlyphProperty);
        set => SetValue(TrailingGlyphProperty, value);
    }

    /// <summary>Gets or sets a value indicating whether the entered text is hidden. The default is false.</summary>
    /// <remarks>Bindable property. Changes update the inner entry.</remarks>
    public bool IsPassword
    {
        get => (bool)GetValue(IsPasswordProperty);
        set => SetValue(IsPasswordProperty, value);
    }

    /// <summary>Gets or sets the keyboard to display. The default is <see cref="Keyboard.Default"/>.</summary>
    /// <remarks>Bindable property. Changes update the inner entry.</remarks>
    public Keyboard Keyboard
    {
        get => (Keyboard)GetValue(KeyboardProperty);
        set => SetValue(KeyboardProperty, value);
    }

    /// <summary>Gets or sets the maximum number of characters. The default is no limit.</summary>
    /// <remarks>Bindable property. Changes update the inner entry.</remarks>
    public int MaxLength
    {
        get => (int)GetValue(MaxLengthProperty);
        set => SetValue(MaxLengthProperty, value);
    }

    /// <summary>Gets or sets a value indicating whether the field is read only. The default is false.</summary>
    /// <remarks>Bindable property. Changes update the inner entry.</remarks>
    public bool IsReadOnly
    {
        get => (bool)GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, value);
    }

    /// <summary>Gets or sets the command run when the trailing glyph is tapped. The default is null.</summary>
    /// <remarks>Bindable property.</remarks>
    public ICommand? TrailingCommand
    {
        get => (ICommand?)GetValue(TrailingCommandProperty);
        set => SetValue(TrailingCommandProperty, value);
    }

    /// <summary>Occurs when the text changes.</summary>
    public event EventHandler<TextChangedEventArgs>? TextChanged;
    /// <summary>Occurs when the user confirms the entry.</summary>
    public event EventHandler? Completed;

    private bool _focused;

    /// <summary>Initializes a new instance of the <see cref="M3TextField"/> class.</summary>
    public M3TextField()
    {
        InitializeComponent();
        // Font shielding: glyphs always use MaterialSymbols, text uses Nunito.
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

    /// <summary>Sets the focus to the field.</summary>
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
        // Reassert: the implicit Nunito style must never override the glyphs.
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

        // Content colors.
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
            // Bottom indicator.
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
