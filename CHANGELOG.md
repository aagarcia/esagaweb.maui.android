# Changelog — Esagaweb.Maui.Android

Format based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).
Versions follow [SemVer](https://semver.org/).

## [Unreleased]

### Added
- `CommandParameter` property on `M3Fab` and `M3IconButton`.
- `M3FabHost.Snackbar` property: connects a `M3SnackbarHost` so the FAB translates upward while the snackbar is visible (Material 3 behavior).
- `M3SnackbarHost.OccupiedHeight` (readonly) and `OccupiedHeightChanged` event: expose the vertical space occupied by the visible snackbar bar.

### Fixed
- `M3Dialog.ShowAsync`: hangs when modal is dismissed with hardware back button (now handles back via `M3ModalHostPage`); idempotent `CloseAsync`; re-entrant `ShowAsync` returns existing task.
- `M3BottomSheet`: back button did not raise `Dismissed`; `HideAsync` could pop an unrelated page; double scrim tap caused double pop (now uses `M3ModalHostPage`, idempotent `HideAsync`).
- `M3Fab` / `M3IconButton`: `Command` now respects `CanExecute` and receives `CommandParameter` (previously ignored both).
- `M3Badge`: dark mode text color used `M3DarkOnPrimary` instead of `M3DarkOnError`.
- `M3Fab`: disabled state now shows reduced opacity (0.38) matching `M3IconButton`.
- `M3BottomSheet` / `M3TextField`: the scrim, the drag handle and the indicator line turned opaque dark gray in apps using the default MAUI template styles (implicit `BoxView` style); their `BackgroundColor` is now set explicitly.
- `M3TextField` (Android): the native `EditText` underline was drawn inside the field; it is now hidden.
- `M3RadioRow` (Android): the radio circle used the app's `colorAccent` instead of the Material 3 palette (nearly invisible in dark theme); it now uses `M3Primary` / `M3DarkPrimary` when checked and the outline color when unchecked, and follows live theme changes.
- FAB was covered by the snackbar; now it lifts while the snackbar is visible (Material 3).

### Changed
- XML documentation (IntelliSense) translated to English and expanded: defaults, parameters, return values and XAML examples.

- Public repository, English docs, Sample uses ProjectReference, CI.
- `M3Dialog.ConfirmText` default changed from "Aceptar" to "OK".

## [1.1.0] — 2026-09-23

### Added
- `M3Fab`: `M3FabVariant` variants (Primary/Surface/Secondary/Tertiary), extended
  mode (`Text` → icon + text pill) and `M3FabHost` host
  (`Body` + `FabContent`, `Location` End/Center).
- New baseline seed #6750A4 colors: `M3OnSecondaryContainer`,
  `M3DarkSecondaryContainer`, `M3DarkOnSecondaryContainer`,
  `M3TertiaryContainer`, `M3OnTertiaryContainer`, `M3DarkTertiaryContainer`,
  `M3DarkOnTertiaryContainer`, `M3OnTertiary`, `M3DarkOnPrimaryContainer`.

### Removed (breaking)
- `M3Scaffold`, `M3FabLocation` (Scaffold namespace), `M3ScaffoldLayout` and its test:
  screens are composed with TopAppBar + content + `M3FabHost` + Snackbar.
  The `ScaffoldDemoPage` demo and its `scaffold` route also leave the sample.
  (`M3Scaffold` existed in 1.0.0 and was withdrawn in 1.1.0.)

## [1.0.0] — 2026-09-22
First version for internal company use (local feed).

### Added
- 19 Material 3 Android-only controls: M3Button (5 variants), M3Fab (3 sizes),
  M3IconButton (4 variants), M3Card (3 variants), M3TopAppBar (4 types),
  M3NavigationBar (bar/rail), M3TextField (Filled/Outlined), M3SwitchRow,
  M3CheckBoxRow, M3RadioRow, M3SliderRow, M3Dialog, M3BottomSheet, M3SnackbarHost
  (+ FIFO queue), M3LinearProgress, M3CircularProgress, M3Chip (3 types), M3Badge,
  M3Scaffold (AppBar+Body+FAB+bar/rail+Snackbar).
- 4284 `M3Icons` icons Flutter-style over `MaterialSymbolsOutlined.ttf` (OFL).
- Nunito fonts + light/dark theming + Compact/Medium/Expanded breakpoints.
- `builder.UseEsagaweb()` + `EsagawebThemes.Apply()`: 2-line setup.
- Sample app (Esagaweb Store).
- Full XML documentation (IntelliSense) + per-component `docs/`.
