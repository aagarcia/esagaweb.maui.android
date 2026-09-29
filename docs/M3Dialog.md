# M3Dialog

M3 dialog like Flutter's `AlertDialog` (namespace `Esagaweb.Maui.Android.Controls.Dialog`). Example: **Detail** page of the Sample (confirm dialog).

<img src="images/add-to-cart.gif" width="280" alt="Confirm dialog, then a snackbar with undo">

## Parameters

| Parameter | Type | Default | Notes |
|---|---|---|---|
| `Title` | `string` | `""` | Empty = hidden. |
| `Message` | `string` | `""` | Empty = hidden. |
| `DialogContent` | `View` | `null` | Custom content instead of the message. |
| `ConfirmText` | `string` | `"OK"` | Filled Error button + white text. |
| `CancelText` | `string` | `""` | Empty = no button (transparent + Primary text). |

## Example

```csharp
var dlg = new M3Dialog
{
    Title = "Delete",
    Message = "Delete this item?",
    ConfirmText = "Delete",
    CancelText = "Cancel"
};
bool? result = await dlg.ShowAsync(Navigation);
// true = confirmed, false = cancelled, null = closed via back
```

## Notes

- `ShowAsync(INavigation)` is modal and returns `bool?`.
- `Confirmed` / `Cancelled` events.
- Can also be embedded as a regular view.
