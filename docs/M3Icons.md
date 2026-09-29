# M3Icons

4284 Material Symbols Outlined icons with Flutter-style names
(`Icons.favorite` → `M3Icons.Favorite`), in `Esagaweb.Maui.Android.Controls.Common`.
The font ships with the package and is registered via `builder.UseEsagaweb()`.

## Usage

```xml
<buttons:M3Button Text="Like" Glyph="{x:Static common:M3Icons.Favorite}" />
```

```csharp
using Esagaweb.Maui.Android.Controls.Common;
string glyph = M3Icons.Search;
```

## Naming convention

- `snake_case` → `PascalCase` (`favorite_border` → `FavoriteBorder`).
- Leading digit gets an `N` prefix (`123` → `N123`).

## Notes

- Glyphs go in their own `Label` with `FontFamily="MaterialSymbols"`;
  never mix text and icon in the same `Label` (the implicit text style would break it).
- Font: `MaterialSymbolsOutlined.ttf` (variable, default outlined), OFL license.
