# Make a mod

A working example mod to start from. It makes your jumps floaty and has a slider in the overlay.

## Contents

- [You need](#you-need)
- [Steps](#steps)
- [What's inside](#whats-inside)
- [Finding game code](#finding-game-code)
- [Rules that save you a crash](#rules-that-save-you-a-crash)
- [Share your mod](#share-your-mod)

## You need

- [.NET SDK](https://dotnet.microsoft.com/download) 8 or newer
- The game with [Cosmic Mod Loader](https://github.com/ren-/CosmicModLoader/releases/latest) installed

## Steps

1. [Download this repo](https://github.com/ren-/CosmicModLoader/archive/refs/heads/main.zip) and copy the `template` folder somewhere.
2. Rename `MyMod` to your mod's name in all four places:
   - the file `MyMod.csproj`
   - `<AssemblyName>` and `<RootNamespace>` inside it
   - `namespace MyMod` in the `.cs` files
3. Set `<GameDir>` in the `.csproj` to your game folder.
4. Open a terminal in the folder and run `dotnet build`.
   Your DLL gets copied into the game's `Mods` folder.
5. Start the game, press **Insert**, open your section.

Then swap the example for your own idea.

## What's inside

| File | What it does |
|---|---|
| `ModEntry.cs` | Start point. The loader calls `Initialize()` once at startup. Your overlay section is here. |
| `GravityPatch.cs` | Example [Harmony](https://harmony.pardeike.net/articles/intro.html) patch on a game method. |
| `Settings.cs` | Saves and loads settings in a file next to your DLL. |
| `MyMod.csproj` | Build setup. Copies the DLL into `Mods` after each build. |

## Finding game code

- Open the game's DLLs in [dnSpy](https://github.com/dnSpyEx/dnSpy) or [ILSpy](https://github.com/icsharpcode/ILSpy).
  - Gameplay (players, enemies, bosses): `Game.exe`
  - Engine: `ParisEngine.dll`
- **Xbox app:** `Game.exe` is encrypted on disk, so tools can't open it. Use a Steam copy to read the code.
  You don't need it to build: find methods by name, like `GravityPatch.cs` does.
- The game folder's `ParisEngine.dll` is the patched one. The original is in `.cosmicloader-backup`.

## Rules that save you a crash

- Show text with `ImGui.TextUnformatted`. `Text`, `TextWrapped` and `SetTooltip` read the string as a printf format: a `%` misprints and some text crashes the game.
- Only call ImGui inside your draw function.
- Don't open your own ImGui window. Your section is already inside one.
- Levels load on another thread. Lock anything a loading hook and your draw function both touch.
- Don't ship `0Harmony.dll`, `CosmicLoader.dll` or `ImGui.NET.dll`. The loader provides them.
- Use your own Harmony ID (`new Harmony("yourname.yourmod")`).
- Need a helper library? Ship its DLL next to yours in `Mods`. Every DLL there loads before any mod starts.

If `Initialize()` throws, the loader skips your mod and the game keeps running.
If your draw function throws, your section shows the error in red. Either way it's in the loader log:
`%LOCALAPPDATA%\CosmicLoader\loader.log`.

## Share your mod

1. Zip your DLL.
2. Upload it. Players drag the zip onto the loader app.
3. List [Cosmic Mod Loader](https://github.com/ren-/CosmicModLoader/releases/latest) as a requirement.

The template code is free to use however you like, no credit needed.
