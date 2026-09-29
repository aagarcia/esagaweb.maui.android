# M3TopAppBar

M3 top app bar (namespace `Esagaweb.Maui.Android.Controls.TopAppBar`). Examples: every Sample page (**Catalog** uses Medium; **Detail**, **Checkout**, and **Settings** use Small).

## Parameters

| Parameter | Type | Default | Notes |
|---|---|---|---|
| `Title` | `string` | `""` | Title (small always, large on Medium/Large). |
| `Type` | `M3TopAppBarType` | `Small` | `Small` `CenterAligned` `Medium` `Large`. |
| `NavigationGlyph` | `string` | `""` | `M3Icons.*` icon (e.g. `Menu`). Empty = no button. |
| `NavigationCommand` | `ICommand` | `null` | No parameters. |
| `Actions` | `ObservableCollection<View>` | - | Typically `M3IconButton`. XAML children. |

## Example

```xml
<appbar:M3TopAppBar Title="Title" Type="Medium"
                    NavigationGlyph="{x:Static common:M3Icons.Menu}">
    <appbar:M3TopAppBar.Actions>
        <iconbtn:M3IconButton Glyph="{x:Static common:M3Icons.Search}" />
    </appbar:M3TopAppBar.Actions>
</appbar:M3TopAppBar>
```

## Notes

- Heights: Small/CenterAligned 64dp; Medium 112dp; Large 152dp.
- `NavigationClicked` event in addition to `NavigationCommand`.
- Scroll collapse on Medium/Large is a future improvement.
