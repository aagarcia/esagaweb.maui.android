# Selection (Switch / CheckBox / Radio / Slider)

48dp M3 rows with label + native control (namespace `Esagaweb.Maui.Android.Controls.Selection`). Examples: **Settings** (switches, theme radios, brightness slider) and **Checkout** (shipping radios, terms check) pages of the Sample.

<img src="images/settings-theme.gif" width="280" alt="Switches, checkbox, slider and the light/dark radios">

## Shared parameters

| Control | Text | Value (**TwoWay**) | Extra |
|---|---|---|---|
| `M3SwitchRow` | `Text` | `IsOn` (`bool`) | `Command` (receives the value) |
| `M3CheckBoxRow` | `Text` | `IsChecked` (`bool`) | `Command`; the whole row toggles |
| `M3RadioRow` | `Text` | `IsSelected` (`bool`) | `GroupName`, `Value` |
| `M3SliderRow` | `Text` | `Value` (`double`) | `Minimum` (0), `Maximum` (100), `ShowValue` (true) |

## Example

```xml
<sel:M3SwitchRow Text="Notifications" IsOn="{Binding Notify}" />
<sel:M3RadioRow Text="Option A" GroupName="G" Value="A" IsSelected="True" />
<sel:M3SliderRow Text="Volume" Minimum="0" Maximum="100" Value="{Binding Volume}" />
```

## Notes

- All values are `TwoWay`: ideal for forms against APIs.
- Events: `Toggled(bool)`, `CheckedChanged(bool)`, `SelectedChanged(bool)`, `ValueChanged(double)`.
