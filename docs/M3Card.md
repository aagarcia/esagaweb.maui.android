# M3Card

M3 card like Flutter (`Card`/`Card.filled`/`Card.outlined`, namespace `Esagaweb.Maui.Android.Controls.Cards`).
The child XAML content is the `child`. Example: **Catalog** page of the Sample (product cards).

## Parameters

| Parameter | Type | Default | Notes |
|---|---|---|---|
| `Variant` | `M3CardVariant` | `Elevated` | `Elevated` `Filled` `Outlined`. |
| `CardColor` | `Color` | `null` | `null` = variant color. |
| `Elevation` | `double` | `-1` | `-1` = variant default (1 elevated, 0 others). |
| `CornerRadius` | `double` | `12` | Radius in dp. |
| `ContentPadding` | `Thickness` | `16` | Inner padding. |
| `Command` | `ICommand` | `null` | The whole card is tappable. Receives `CommandParameter`. |
| `CommandParameter` | `object` | `null` | Command parameter. |
| `Margin` | `Thickness` | `4` | Flutter default. |

## Example

```xml
<cards:M3Card Variant="Elevated" Command="{Binding OpenCommand}">
    <VerticalStackLayout>
        <Label Text="Title" />
    </VerticalStackLayout>
</cards:M3Card>
```

## Notes

- No `shadowColor` (MAUI `Shadow` has no color).
- `Clicked` event in addition to `Command`.
