namespace Esagaweb.Maui.Android.Sample.Pages;

/// <summary>Producto ficticio de la tienda demo (solo para ejercitar los controles).</summary>
public sealed record StoreProduct(int Id, string Name, string Category, decimal Price, string Variant);

/// <summary>Catálogo fijo de la demo.</summary>
public static class StoreData
{
    public static IReadOnlyList<StoreProduct> Products { get; } = new List<StoreProduct>
    {
        new(1, "Auriculares Norte", "Audio", 59.99m, "Elevated"),
        new(2, "Altavoz Puerto", "Audio", 89.99m, "Filled"),
        new(3, "Lámpara Faro", "Casa", 34.50m, "Outlined"),
        new(4, "Taza Bahía", "Casa", 12.99m, "Elevated"),
        new(5, "Mochila Sendero", "Fuera", 74.00m, "Filled"),
        new(6, "Botella Cumbre", "Fuera", 19.99m, "Outlined"),
    };

    public static IReadOnlyList<string> Categories { get; } = new List<string> { "Todos", "Audio", "Casa", "Fuera" };
}

/// <summary>Carrito compartido entre páginas (contador + aviso de cambios).</summary>
public static class CartState
{
    private static int _count;

    public static int Count
    {
        get => _count;
        private set
        {
            if (_count == value)
                return;
            _count = value;
            Changed?.Invoke(null, EventArgs.Empty);
        }
    }

    public static event EventHandler? Changed;

    public static void Add(int qty = 1) => Count += qty;

    public static void Clear() => Count = 0;
}
