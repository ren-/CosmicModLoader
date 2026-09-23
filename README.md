# Cosmic Mod Loader for MARVEL Cosmic Invasion

Made by claymore.

Loads code mods for MARVEL Cosmic Invasion. You install it once; after that, every mod is a DLL
you drop into the game's `Mods` folder. Works on the Xbox app (PC Game Pass) version and on Steam.

Mod settings live in an in-game overlay, REFramework style: press **Insert** while playing (or click
both sticks on a pad) and a window opens over the game with a section per mod. Changes apply
immediately and are saved when you close it. In single player, gameplay pauses while it is open.

## Download

Get `CosmicModLoader-<version>.zip` from [Releases](https://github.com/ren-/CosmicModLoader/releases/latest). This repository only
hosts the downloads and this page; the source is not published. Mods come separately (for example
Boss Hitstun Overhaul on Nexus Mods).

## What's in the zip

| File | What it is |
|---|---|
| `CosmicModLoader.exe` | the app: installs and removes the loader, adds mods (drag and drop), lists them |
| `CosmicLoader.dll` | the loader itself, copied into the game folder |
| `0Harmony.dll` | [Harmony](https://github.com/pardeike/Harmony), the patching library mods use (MIT) |
| `Mono.Cecil.dll` | [Mono.Cecil](https://github.com/jbevain/cecil), used by the app to patch the engine file (MIT) |
| `lib\` | the overlay: [Dear ImGui](https://github.com/ocornut/imgui) (`cimgui.dll`) and [ImGui.NET](https://github.com/ImGuiNET/ImGui.NET) with its .NET support DLLs (all MIT), installed to `CosmicLoader\` in the game folder |
| `*-LICENSE.txt` | the licences |

Extract the whole zip into one folder and run `CosmicModLoader.exe` from there.

## Supported game builds

The app identifies the game's `ParisEngine.dll` by its SHA-256 hash before touching anything and
refuses any build it doesn't know. After a game update it will say so and do nothing until the
loader is updated.

| Build | Game folder |
|---|---|
| Xbox app 26.6.8.0 | `C:\XboxGames\MARVEL Cosmic Invasion\Content` |
| Steam, July 2026 build (game 1.0.0.13466) | `...\steamapps\common\MARVEL Cosmic Invasion` |

## Install

1. Close the game.
2. Run `CosmicModLoader.exe`. It looks for the game in every drive's `XboxGames` folder and
   in all Steam libraries. If it picked the wrong copy or found nothing, use **Browse** and pick the
   folder that has `Game.exe` and `ParisEngine.dll` in it.
3. Press **Install loader**. It creates the game's `Mods` folder.
4. Drag a mod's `.zip` or `.dll` onto the app window (or onto `CosmicModLoader.exe`). The app copies
   the DLLs into `Mods`; dropping a mod you already have replaces the old copy. You can also
   copy DLLs into `Mods` by hand (**Open Mods folder**).
5. Start the game as usual.

If Windows refuses to write into the game folder, the app offers to retry as administrator.

## What it changes

- `ParisEngine.dll` is backed up to `.cosmicloader-backup\` and replaced by a copy with **one**
  added instruction: a call to `CosmicLoader.Loader.Initialize()` at the start of the engine's
  first startup method. The app checks that exactly one method changed and that removing the call
  gives back the original method's instructions unchanged.
- `CosmicLoader.dll` and `0Harmony.dll` are added next to it, the overlay files go into a
  `CosmicLoader\` subfolder, and an empty `Mods` folder is created.
- `Game.exe` is never opened or modified. Nothing from the game is downloaded or shipped: the patch
  is made on your PC, from your own copy.
- Nothing connects to the internet.

## Windows warnings

The app is an unsigned .NET exe, so Windows SmartScreen may say "Windows protected your PC" (press
**More info**, then **Run anyway**), and Defender's cloud heuristics sometimes flag a new release for a
day or two (names ending in `!ml` are machine-learning guesses, not matched signatures). The exe is
not packed or obfuscated: open it in ILSpy or dnSpy to read exactly what it does.

The patched engine needs `CosmicLoader.dll` next to it. If an antivirus quarantines that file (or it
gets deleted), the game will not start: restore the file, or press **Uninstall** in the app (or
verify game files) to go back to vanilla.

## Uninstall

Close the game, run the app, press **Uninstall**. The engine is put back byte for byte and the
loader files are removed. Your `Mods` folder is left alone; delete it by hand if you want.
Verifying game files in the Xbox app or Steam also puts the original engine back; the loader's
files then do nothing, and the app's **Uninstall** tidies them up.

## Logs

`%LOCALAPPDATA%\CosmicLoader\loader.log` (**Open loader log**) says which DLLs it found, which mods
it started and why any failed. Each game launch starts a fresh log and keeps the previous one as
`loader.old.log`. Mods write their own logs. The overlay keeps its window layout and options
(`imgui.ini`, `overlay.cfg`) in the same folder.

## The overlay

- **Insert**, or both sticks clicked on a pad, shows and hides it. Mouse and keyboard work as in any
  ImGui window; a pad can navigate too (d-pad, A, B).
- While it is open the game gets no input, and in single player gameplay is paused (music and
  menu animations keep going; switch this off in the window's **Loader** section). Online, the game keeps running, like its own pause menu.
- **Loader** section: pause option, font size (16 px by default at any resolution, like REFramework; raise it on a big or 4K screen), and what loaded this session.

## For mod authors

A mod is a .NET Framework 4.x class library (x64 or AnyCPU) placed in `Mods`. The loader looks for
a public static class named `<AssemblyName>.ModEntry` and calls its `Initialize()`. For settings,
reference `CosmicLoader.dll` and `ImGui.NET.dll` (both provided at runtime, don't ship them) and
register a section, the same pattern as REFramework's `re.on_draw_ui` / `re.on_config_save`:

```csharp
using CosmicLoader;
using ImGuiNET;

namespace MyMod                       // assembly name MyMod -> MyMod.ModEntry
{
    public static class ModEntry
    {
        static float speed = 1f;

        // Called once, very early in the game's startup (before the engine is set up).
        public static void Initialize()
        {
            new HarmonyLib.Harmony("my.mod").PatchAll();
            // Every frame the overlay is open and this section is expanded. Plain ImGui calls.
            Overlay.OnDrawUI("My Mod", () =>
            {
                ImGui.SliderFloat("Speed", ref speed, 0.5f, 2f);
                if (ImGui.IsItemHovered()) { ImGui.BeginTooltip(); ImGui.TextUnformatted("How fast things go."); ImGui.EndTooltip(); }
            });
            // When the overlay closes and when the game exits: write your settings file here.
            Overlay.OnConfigSave("My Mod", () => { /* save speed */ });
        }
    }
}
```

- Every `*.dll` directly in `Mods` is loaded (sorted by name) before any mod starts, so mods can
  ship helper libraries. A DLL without `ModEntry` is just a library. With two copies of the same
  assembly, only the highest version loads.
- An exception from `Initialize` is logged and that mod is skipped; the game keeps running. An
  exception in a draw callback is logged, and that mod's section shows the error in red instead of
  its UI for the rest of the session.
- Show text with `ImGui.TextUnformatted`, and tooltips with `BeginTooltip` / `TextUnformatted` /
  `EndTooltip`. `ImGui.Text`, `TextWrapped` and `SetTooltip` treat the string as a printf format: a
  `%` in it misprints, and some sequences crash the game.
- Call ImGui only inside your draw callback. Outside it there is no frame, and cimgui is built
  without checks, so a stray call crashes instead of throwing.
- Draw callbacks run on the game's main thread at the end of each frame, so they may read and change game
  state directly. Levels load on a separate thread, though, so lock anything a load-time hook of
  yours also touches. Don't open your own ImGui window inside them; the section is already inside one.
- Reference `0Harmony.dll` 2.4 from the game folder rather than shipping your own copy.
- Keep settings next to your DLL (`Path.GetDirectoryName(typeof(ModEntry).Assembly.Location)`).

## Reporting problems

Open an [issue](https://github.com/ren-/CosmicModLoader/issues) with the game build (Xbox app or Steam), what happened, and
`%LOCALAPPDATA%\CosmicLoader\loader.log` plus `loader.old.log` attached. For a problem with a
particular mod, attach that mod's log too.

## Licences

Cosmic Mod Loader is free to download and use. The third-party libraries in the zip keep their own
licences (Harmony, Mono.Cecil, Dear ImGui, ImGui.NET and the .NET support DLLs are all MIT); the
licence texts are included in the zip.
