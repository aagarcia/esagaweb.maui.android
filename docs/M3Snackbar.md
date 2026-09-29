# M3Snackbar (host + queue)

M3 snackbars like Flutter's `ScaffoldMessenger` (namespace `Esagaweb.Maui.Android.Controls.Snackbar`). Examples: **Catalog**, **Detail**, **Checkout**, and **Settings** pages of the Sample.

## Usage

One instance at the bottom of the page + enqueue:

```xml
<snack:M3SnackbarHost x:Name="Snacks" Grid.Row="1" />
```

```csharp
Snacks.Show("Item archived", "Undo"); // optional action
Snacks.Show("Simple notice");         // 4s by default
```

## API

- `M3SnackbarHost.Queue` (`M3SnackbarQueue`): observable FIFO queue.
- `Show(message, actionText?, duration?)`: shortcut to enqueue.
- `ActionInvoked` event when the action is tapped.
- `OccupiedHeight` (readonly `double`): vertical space occupied by the visible snackbar bar (height + margin), or 0 when hidden or not yet measured.
- `OccupiedHeightChanged` event: raised when `OccupiedHeight` changes (e.g., after layout measures a multi-line message).
- `M3SnackbarQueue`: `Current`, `PendingCount`, `Enqueue()`, `DismissCurrent()`, `Clear()`, `Changed` event, `DefaultDuration` (4s).

## Notes

- The bar is always dark (like Flutter), auto-dismisses by duration, and advances the queue on its own.
