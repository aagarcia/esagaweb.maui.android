# Contributing to Esagaweb.Maui.Android

Thanks for considering a contribution. This is a Material 3 component kit for .NET MAUI Android.

## Requirements

- .NET 10 SDK
- `maui-android` workload: `dotnet workload install maui-android`
- JDK 21 (Android builds must use JDK 21; if your `JAVA_HOME` points elsewhere,
  pass `-p:JavaSdkDirectory="<path-to-jdk-21>"` on every Android build)
- Android SDK + emulator (e.g. Pixel 7, API 36)

## Clone and build

```sh
git clone https://github.com/aagarcia/esagaweb.maui.android.git
cd esagaweb.maui.android
dotnet build Esagaweb.Maui.Android.Sample/Esagaweb.Maui.Android.Sample.csproj -f net10.0-android
```

On Windows with JDK 21 at the default path:

```sh
dotnet build Esagaweb.Maui.Android.Sample/Esagaweb.Maui.Android.Sample.csproj -f net10.0-android -p:JavaSdkDirectory="C:\Program Files\Java\jdk-21.0.12.1"
```

## Tests

```sh
dotnet test --project Esagaweb.Maui.Android.Tests
```

Pure logic must be testable on `net10.0` via the library's testable surface, using xunit.v3.
All public API must carry XML-doc comments.

## Branch flow

1. Fork the repo.
2. Create a branch: `feat/<name>` for features, `fix/<name>` for fixes.
3. Open a PR against `main` (fill in the PR template checklist).
4. Use [Conventional Commits](https://www.conventionalcommits.org/): `feat:`, `fix:`, `docs:`, `test:`, `chore:`.

## Project technical rules

- Pin `<MauiVersion>10.0.101</MauiVersion>`; the MAUI family (Controls/Essentials) always moves together.
- Every `ContentView` with its own `[ContentProperty]` declares its root in explicit
  `<ContentView.Content>` (avoids layout cycles / ANR).
- Controls carry no internal `x:DataType` (they respect the consumer's BindingContext).
- Glyph labels set `FontFamily=MaterialSymbols` explicitly.
- `SizeChanged` handlers use a breakpoint guard (avoids layout loops).
- Pure logic → testable on net10.0 with xunit.v3 tests; every public API with XML-doc.

## Adding a new component

1. Create `Esagaweb.Maui.Android/Controls/<Name>/`.
2. Document it in `docs/<Name>.md` (English, with example).
3. Show it in the Sample (a screen or usage in Catalog / Detail / Checkout / Settings).
4. Add a `CHANGELOG.md` entry under `[Unreleased]`.
