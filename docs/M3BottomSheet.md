# M3BottomSheet

Modal bottom sheet like Flutter's `showModalBottomSheet` (namespace `Esagaweb.Maui.Android.Controls.BottomSheet`). Example: **Detail** page of the Sample (size picker).

<img src="images/size-sheet.gif" width="280" alt="Size picker bottom sheet">

## Parameters

| Parameter | Type | Default | Notes |
|---|---|---|---|
| `Title` | `string` | `""` | Empty = hidden. |
| `SheetBody` | `View` | `null` | Content. XAML children go here. |

## Example

```csharp
var sheet = new M3BottomSheet
{
    Title = "Options",
    SheetBody = new VerticalStackLayout { /* ... */ }
};
await sheet.ShowAsync(Navigation);
```

```xml
<sheet:M3BottomSheet Title="Filters">
    <VerticalStackLayout>
        <Label Text="Content" />
    </VerticalStackLayout>
</sheet:M3BottomSheet>
```

## Notes

- Dismiss with the scrim, `HideAsync()`, or the back button.
- `Dismissed` event. Full drag gesture = future improvement.
