# M3Badge

M3 badge like Flutter's `Badge` (namespace `Esagaweb.Maui.Android.Controls.Badge`). Overlay it with a `Grid`. Example: **Catalog** page of the Sample (cart badge).

## Parameters

| Parameter | Type | Default | Notes |
|---|---|---|---|
| `Count` | `int` | `0` | `0` hides unless `ShowZero`. |
| `MaxCount` | `int` | `99` | Above shows `"{MaxCount}+"`. |
| `ShowZero` | `bool` | `false` | Shows 0 instead of hiding. |
| `IsDot` | `bool` | `false` | Dot without a number. |

## Example

```xml
<Grid>
    <iconbtn:M3IconButton Glyph="{x:Static common:M3Icons.Mail}" />
    <badge:M3Badge Count="{Binding Unread}" HorizontalOptions="End" VerticalOptions="Start" />
</Grid>
```
