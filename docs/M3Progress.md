# M3Progress

M3 indicators (namespace `Esagaweb.Maui.Android.Controls.Progress`). Examples: **Checkout** and **Settings** (linear, while saving/paying) and **Catalog** (circular, while filtering).

## Parameters

| Control | Parameter | Type | Default | Notes |
|---|---|---|---|---|
| `M3LinearProgress` | `Value` | `double` | `0` | 0 to 1. SurfaceVariant track, Primary bar. |
| `M3CircularProgress` | `IsRunning` | `bool` | `true` | Indeterminate, Primary color. |

## Example

```xml
<prog:M3LinearProgress Value="0.65" />
<prog:M3CircularProgress IsRunning="{Binding Loading}" />
```
