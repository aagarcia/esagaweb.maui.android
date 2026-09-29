# Changelog — Esagaweb.Maui.Android

Format based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).
Versions follow [SemVer](https://semver.org/).

## [Unreleased]

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
