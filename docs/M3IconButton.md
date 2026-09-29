# M3IconButton

Icon-only button (namespace `Esagaweb.Maui.Android.Controls.IconButtons`). Examples: top-app-bar actions across the Sample (**Catalog**, **Detail**, **Checkout**, **Settings**).

## Parameters

| Parameter | Type | Default | Notes |
|---|---|---|---|
| `Glyph` | `string` | `""` | `M3Icons.*` icon. Takes priority over `Icon`. |
| `Icon` | `ImageSource` | `null` | Image when there is no `Glyph`. |
| `Variant` | `M3IconButtonVariant` | `Standard` | `Standard` `Filled` `Tonal` `Outlined`. |
| `Command` | `ICommand` | `null` | No parameters. |

## Example

```xml
<iconbtn:M3IconButton Variant="Tonal" Glyph="{x:Static common:M3Icons.Settings}"
                      Command="{Binding OpenSettingsCommand}" />
```

## Notes

- `Clicked` event in addition to `Command`.
