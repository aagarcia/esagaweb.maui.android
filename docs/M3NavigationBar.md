# M3NavigationBar

M3 navigation bar with indicator pill (namespace `Esagaweb.Maui.Android.Controls.NavigationBar`).
Responsive: bottom bar on Compact/Medium, side rail on Expanded.
Not currently showcased in the Sample pages; compose it with `M3TopAppBar` + content the same way `CatalogPage` does.

## Parameters

| Parameter | Type | Default | Notes |
|---|---|---|---|
| `Destinations` | `ObservableCollection<M3NavigationItem>` | - | 3–5 destinations. XAML children. |
| `SelectedIndex` | `int` | `0` | **TwoWay**: bindable to a ViewModel. |

`M3NavigationItem`: `Label` (`string`), `Glyph` (`string`), `SelectedGlyph` (`string`, empty = uses `Glyph`), `Command` (`ICommand`).

## Example

```xml
<navbar:M3NavigationBar SelectedIndex="{Binding TabIndex}">
    <navbar:M3NavigationItem Label="Home" Glyph="{x:Static common:M3Icons.Home}" />
    <navbar:M3NavigationItem Label="Search" Glyph="{x:Static common:M3Icons.Search}" />
</navbar:M3NavigationBar>
```

## Notes

- Call `ApplyBreakpoint(width)` from `SizeChanged`; the parent decides the placement.
- `SelectionChanged(int)` event.
