using Esagaweb.Maui.Android.Controls.Cards;
using Esagaweb.Maui.Android.Controls.Chip;
using Esagaweb.Maui.Android.Controls.Common;
using Esagaweb.Maui.Android.Helpers;

namespace Esagaweb.Maui.Android.Sample.Pages;

/// <summary>Catálogo de la tienda: chips de filtro, cards por producto, badge de carrito.</summary>
public partial class CatalogPage : ContentPage
{
    private readonly List<M3Chip> _chips = new();
    private readonly List<M3Card> _cards = new();
    private string _filter = "Todos";
    private M3Breakpoint? _lastBp;
    private bool _filtering;

    public CatalogPage()
    {
        InitializeComponent();
        BuildChips();
        BuildCards();
        CartState.Changed += (_, _) => RefreshBadge();
        RefreshBadge();
        SizeChanged += OnSizeChanged;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        RefreshBadge();
        // Primera medición: colocar columnas aunque SizeChanged ya haya pasado.
        LayoutProducts();
    }

    private void BuildChips()
    {
        foreach (var cat in StoreData.Categories)
        {
            var chip = new M3Chip
            {
                Text = cat,
                Type = M3ChipType.Filter,
                Selected = cat == _filter,
                Glyph = cat == "Todos" ? M3Icons.Check : string.Empty,
            };
            chip.Tapped += (_, _) => OnChipTapped(chip, cat);
            _chips.Add(chip);
            ChipsRow.Children.Add(chip);
        }
    }

    private void BuildCards()
    {
        foreach (var p in StoreData.Products)
        {
            var name = new Label { Text = p.Name, Style = (Style)Application.Current!.Resources["M3TitleLargeStyle"] };
            var price = new Label { Text = $"{p.Price:C}", Style = (Style)Application.Current!.Resources["M3BodyMediumStyle"] };
            var cat = new Label { Text = p.Category, Style = (Style)Application.Current!.Resources["M3BodyMediumStyle"] };
            var body = new VerticalStackLayout { Spacing = 4, Children = { name, price, cat } };
            var card = new M3Card
            {
                Variant = Enum.Parse<M3CardVariant>(p.Variant),
                Content = body,
                VerticalOptions = LayoutOptions.Start,
                Command = new Command(() => Shell.Current.GoToAsync($"detail?id={p.Id}")),
            };
            _cards.Add(card);
        }
        LayoutProducts();
    }

    private async void OnChipTapped(M3Chip tapped, string category)
    {
        if (_filtering)
            return;
        // Grupo exclusivo: solo la tocada queda seleccionada.
        foreach (var c in _chips)
            c.Selected = c == tapped;
        if (!tapped.Selected)
        {
            // Si se deseleccionó la activa, volver a "Todos".
            _filter = "Todos";
            _chips[0].Selected = true;
        }
        else
        {
            _filter = category;
        }

        _filtering = true;
        Loading.IsVisible = true;
        await Task.Delay(350);
        LayoutProducts();
        Loading.IsVisible = false;
        _filtering = false;
    }

    private void LayoutProducts()
    {
        if (Width > 0)
            ApplyColumns(Width);
        else
            ApplyColumns(400);

        ProductsGrid.Children.Clear();
        var cols = ProductsGrid.ColumnDefinitions.Count;
        var row = 0;
        for (var i = 0; i < StoreData.Products.Count; i++)
        {
            var product = StoreData.Products[i];
            if (_filter != "Todos" && product.Category != _filter)
                continue;
            var card = _cards[i];
            (card.Parent as Layout)?.Children.Remove(card);
            ProductsGrid.Children.Add(card);
            Grid.SetRow(card, row / cols);
            Grid.SetColumn(card, row % cols);
            row++;
        }

        // Filas explícitas Auto: sin esto todas las cards caen en la fila 0,
        // se solapan y solo se ve la última estirada al alto del viewport.
        var needRows = row == 0 ? 1 : (row + cols - 1) / cols;
        if (ProductsGrid.RowDefinitions.Count != needRows)
        {
            ProductsGrid.RowDefinitions = new RowDefinitionCollection(
                Enumerable.Range(0, needRows).Select(_ => new RowDefinition(GridLength.Auto)).ToArray());
        }
    }

    private void ApplyColumns(double widthDp)
    {
        var cols = ResponsiveHelper.ColumnsFor(ResponsiveHelper.GetBreakpoint(widthDp));
        if (ProductsGrid.ColumnDefinitions.Count == cols)
            return;
        ProductsGrid.ColumnDefinitions = new ColumnDefinitionCollection(
            Enumerable.Range(0, cols).Select(_ => new ColumnDefinition(GridLength.Star)).ToArray());
    }

    private void RefreshBadge()
    {
        CartBadge.Count = CartState.Count;
        // El FAB extendido "Pagar" solo aparece cuando hay algo que pagar.
        CartFab.IsVisible = CartState.Count > 0;
    }

    private void OnMenuClicked(object? sender, EventArgs e)
        => Shell.Current.FlyoutIsPresented = true;

    private void OnSearchClicked(object? sender, EventArgs e)
        => Snacks.Show("Búsqueda (demo): prueba los filtros por categoría");

    private async void OnCartClicked(object? sender, EventArgs e)
        => await Shell.Current.GoToAsync("///checkout");

    private void OnSizeChanged(object? sender, EventArgs e)
    {
        // Guarda anti-bucle: rearmar el Grid dispara otra pasada de layout.
        if (Width <= 0)
            return;
        var bp = ResponsiveHelper.GetBreakpoint(Width);
        if (bp == _lastBp)
            return;
        _lastBp = bp;
        LayoutProducts();
    }
}
