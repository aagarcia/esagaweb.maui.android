# M3Fab

Standalone M3 floating action button (namespace `Esagaweb.Maui.Android.Controls.Fab`). Example: **Detail** page of the Sample (favorite FAB), and **Catalog** (extended "Checkout" FAB).

## Parameters

| Parameter | Type | Default | Notes |
|---|---|---|---|
| `Glyph` | `string` | `""` | `M3Icons.*` icon. Takes priority over `Icon`. |
| `Icon` | `ImageSource` | `null` | Image when there is no `Glyph`. |
| `Size` | `M3FabSize` | `Regular` | `Small` (48) `Regular` (56) `Large` (96). Circular mode only. |
| `Variant` | `M3FabVariant` | `Primary` | `Primary` `Surface` `Secondary` `Tertiary`. |
| `Text` | `string` | `""` | Non-empty = extended mode (icon + text pill). |
| `Command` | `ICommand` | `null` | Executes only when `CanExecute(CommandParameter)` is `true`. |
| `CommandParameter` | `object` | `null` | Parameter passed to `Command`. |

## Examples

Circular:

```xml
<fab:M3Fab Size="Regular" Glyph="{x:Static common:M3Icons.Add}"
           Command="{Binding AddCommand}" />
```

Extended secondary:

```xml
<fab:M3Fab Variant="Secondary" Text="Create"
           Glyph="{x:Static common:M3Icons.Add}" Clicked="OnCreate" />
```

## M3FabHost

Host that docks the FAB over any content (`Body` + `FabContent`, `Location` `End`/`Center`):

```xml
<fab:M3FabHost Snackbar="{x:Reference Snacks}">
    <fab:M3FabHost.Body>
        <Grid RowDefinitions="*,Auto">
            <ScrollView><!-- content --></ScrollView>
            <snack:M3SnackbarHost x:Name="Snacks" Grid.Row="1" />
        </Grid>
    </fab:M3FabHost.Body>
    <fab:M3FabHost.FabContent>
        <fab:M3Fab Glyph="{x:Static common:M3Icons.Favorite}" Clicked="OnFavClicked" />
    </fab:M3FabHost.FabContent>
</fab:M3FabHost>
```

| Parameter | Type | Default | Notes |
|---|---|---|---|
| `Snackbar` | `M3SnackbarHost` | `null` | When set, the FAB translates upward by the snackbar occupied height while the snackbar is visible (Material 3 behavior). |

## Notes

- Variants: `Primary` (PrimaryContainer), `Surface` (SurfaceVariant + Primary icon), `Secondary` (SecondaryContainer), `Tertiary` (TertiaryContainer). Automatic light/dark.
- Extended: 56-high pill, 16 radius, text in the icon's `LabelLarge` color.
- M3 shadow (elevation 3). `Clicked` event in addition to `Command`.
