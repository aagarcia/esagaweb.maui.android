# M3Chip

Compact M3 chip (namespace `Esagaweb.Maui.Android.Controls.Chip`). Example: **Catalog** page of the Sample (exclusive filter chips).

<img src="images/chips.gif" width="280" alt="Exclusive filter chips">

## Parameters

| Parameter | Type | Default | Notes |
|---|---|---|---|
| `Text` | `string` | `""` | Chip text. |
| `Type` | `M3ChipType` | `Assist` | `Assist` (tap) `Filter` (toggles `Selected`) `Input` (shows close). |
| `Selected` | `bool` | `false` | **TwoWay** (matters for Filter). |
| `Glyph` | `string` | `""` | Optional `M3Icons.*` icon. |
| `Command` | `ICommand` | `null` | Receives `Selected` on Filter. |

## Example

```xml
<chip:M3Chip Text="Filter" Type="Filter" Selected="{Binding OnlyMine}"
             Glyph="{x:Static common:M3Icons.Check}" />
```

## Notes

- `Tapped` and `Closed` events (close, Input only).
