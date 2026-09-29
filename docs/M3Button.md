# M3Button

Material 3 button (namespace `Esagaweb.Maui.Android.Controls.Buttons`). Examples: **Checkout**, **Detail**, and **Settings** pages of the Sample.

## Parameters

| Parameter | Type | Default | Notes |
|---|---|---|---|
| `Text` | `string` | `""` | Button text. |
| `Variant` | `M3ButtonVariant` | `Filled` | `Elevated` `Filled` `Tonal` `Outlined` `Text`. |
| `Glyph` | `string` | `""` | `M3Icons.*` icon. Takes priority over `Icon`. |
| `Icon` | `ImageSource` | `null` | Image when there is no `Glyph`. |
| `Command` | `ICommand` | `null` | Receives `CommandParameter`. |
| `CommandParameter` | `object` | `null` | Command parameter. |
| `IsEnabled` | `bool` | `true` | Disabled dims to 38/60%. |

## Example

```xml
<buttons:M3Button Text="Save" Variant="Filled"
                  Glyph="{x:Static common:M3Icons.Check}"
                  Command="{Binding SaveCommand}" />
```

## Notes

- Responsive: 40dp on Compact, 48dp on Medium/Expanded (call `ApplyBreakpoint(width)` from `SizeChanged`).
- `Clicked` event in addition to `Command`.
