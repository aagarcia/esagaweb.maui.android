# M3TextField

M3 text field guided by Flutter's `TextField` (namespace `Esagaweb.Maui.Android.Controls.TextField`). Example: **Checkout** page of the Sample (validated form).

## Parameters

| Parameter | Type | Default | Notes |
|---|---|---|---|
| `Text` | `string` | `""` | **TwoWay**: load and read (API forms). |
| `Label` | `string` | `""` | Top label. |
| `Placeholder` | `string` | `""` | Example text. |
| `HelperText` | `string` | `""` | Help (hidden when there is an error). |
| `ErrorText` | `string` | `""` | Requires `HasError`. |
| `HasError` | `bool` | `false` | Error state. |
| `Variant` | `M3TextFieldVariant` | `Filled` | `Filled` `Outlined`. |
| `LeadingGlyph` | `string` | `""` | Left icon (`M3Icons.*`). |
| `TrailingGlyph` | `string` | `""` | Right icon, tappable via `TrailingCommand`. |
| `TrailingCommand` | `ICommand` | `null` | E.g. clear, show password. |
| `IsPassword` | `bool` | `false` | Hides input. |
| `Keyboard` | `Keyboard` | `Default` | Keyboard to show. |
| `MaxLength` | `int` | No limit | Max characters. |
| `IsReadOnly` | `bool` | `false` | Read-only. |

## Example

```xml
<field:M3TextField Label="Email" Placeholder="name@mail.com" Keyboard="Email"
                   Variant="Outlined" Text="{Binding Email}"
                   TrailingGlyph="{x:Static common:M3Icons.Close}"
                   TrailingCommand="{Binding ClearCommand}" />
```

## Notes

- `TextChanged` and `Completed` events; `FocusField()` method.
