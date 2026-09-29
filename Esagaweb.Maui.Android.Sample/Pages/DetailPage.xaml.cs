using Esagaweb.Maui.Android.Controls.BottomSheet;
using Esagaweb.Maui.Android.Controls.Dialog;
using Esagaweb.Maui.Android.Controls.Selection;
using Esagaweb.Maui.Android.Helpers;

namespace Esagaweb.Maui.Android.Sample.Pages;

/// <summary>Detalle de producto: hero, confirmación con diálogo, tallas en bottom sheet.</summary>
[QueryProperty(nameof(ProductId), "id")]
public partial class DetailPage : ContentPage
{
    private string _productId = "1";
    private string _size = "M";
    private bool _fav;
    private M3Breakpoint? _lastBp;

    public string ProductId
    {
        get => _productId;
        set
        {
            _productId = value;
            LoadProduct();
        }
    }

    public DetailPage()
    {
        InitializeComponent();
        CartState.Changed += (_, _) => RefreshBadge();
        SizeChanged += OnSizeChanged;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadProduct();
        RefreshBadge();
    }

    private void LoadProduct()
    {
        if (NameLabel is null)
            return;
        var product = StoreData.Products.FirstOrDefault(p => p.Id.ToString() == _productId)
            ?? StoreData.Products[0];
        NameLabel.Text = product.Name;
        PriceLabel.Text = $"{product.Price:C} · {product.Category}";
    }

    private void RefreshBadge()
    {
        if (CartBadge is not null)
            CartBadge.Count = CartState.Count;
    }

    private async void OnAddClicked(object? sender, EventArgs e)
    {
        var dlg = new M3Dialog
        {
            Title = "Añadir al carrito",
            Message = $"{NameLabel.Text} (talla {_size}) por {PriceLabel.Text}. ¿Confirmar?",
            ConfirmText = "Añadir",
            CancelText = "Cancelar",
        };
        var result = await dlg.ShowAsync(Navigation);
        if (result == true)
        {
            CartState.Add();
            Snacks.ActionInvoked += OnUndoOnce;
            Snacks.Show($"{NameLabel.Text} añadido", "Deshacer");
        }
    }

    private void OnUndoOnce(object? sender, EventArgs e)
    {
        Snacks.ActionInvoked -= OnUndoOnce;
        if (CartState.Count > 0)
            CartState.Add(-1);
        Snacks.Show("Añadido cancelado");
    }

    private async void OnSizesClicked(object? sender, EventArgs e)
    {
        var bodyStyle = (Style)Application.Current!.Resources["M3BodyMediumStyle"];
        var options = new VerticalStackLayout { Spacing = 4 };
        var sheet = new M3BottomSheet { Title = "Elige talla", SheetBody = options };
        foreach (var s in new[] { "S", "M", "L", "XL" })
        {
            var row = new M3RadioRow { Text = $"Talla {s}", GroupName = "Size", IsSelected = s == _size };
            var captured = s;
            row.SelectedChanged += async (_, selected) =>
            {
                if (!selected)
                    return;
                _size = captured;
                SizeLabel.Text = $"Talla: {_size}";
                await sheet.HideAsync();
                Snacks.Show($"Talla {_size} seleccionada");
            };
            options.Children.Add(row);
        }
        options.Children.Add(new Label { Text = "Guía: la M equivale a la talla habitual.", Style = bodyStyle });
        await sheet.ShowAsync(Navigation);
    }

    private void OnFavClicked(object? sender, EventArgs e)
    {
        _fav = !_fav;
        Snacks.Show(_fav ? "Guardado en favoritos" : "Quitado de favoritos");
    }

    private void OnSearchClicked(object? sender, EventArgs e)
        => Snacks.Show("Búsqueda (demo): vuelve al catálogo para filtrar");

    private async void OnCartClicked(object? sender, EventArgs e)
        => await Shell.Current.GoToAsync("///checkout");

    private async void OnBackClicked(object? sender, EventArgs e)
        => await Shell.Current.GoToAsync("..");

    private void OnSizeChanged(object? sender, EventArgs e)
    {
        // Guarda anti-bucle: solo actuar si el breakpoint cambió.
        if (Width <= 0)
            return;
        var bp = ResponsiveHelper.GetBreakpoint(Width);
        if (bp == _lastBp)
            return;
        _lastBp = bp;
        Root.Spacing = ResponsiveHelper.SpacingFor(bp);
    }
}
