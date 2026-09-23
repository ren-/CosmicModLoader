# Cosmic Mod Loader for MARVEL Cosmic Invasion

Made by claymore.

Loads code mods for MARVEL Cosmic Invasion. You install it once; after that, every mod is a DLL
in the game's `Mods` folder. Works on the Xbox app (PC Game Pass) version and on Steam.

Mod settings live in an in-game overlay, REFramework style: press **Insert** while playing (or click
both sticks on a pad) and a window opens over the game with a section per mod.

**[Download the latest version](https://github.com/ren-/CosmicModLoader/releases/latest)**

This repository only hosts the downloads and this guide; the source is not published. Mods come
separately (for example Boss Hitstun Overhaul on Nexus Mods).

## Contents

- [Quick start](#quick-start)
- [Step by step](#step-by-step)
  - [1. Open the app](#1-open-the-app)
  - [2. Install the loader](#2-install-the-loader)
  - [3. Add mods](#3-add-mods)
  - [4. Play and change mod settings](#4-play-and-change-mod-settings)
  - [5. Remove a mod or the whole loader](#5-remove-a-mod-or-the-whole-loader)
- [After a game update](#after-a-game-update)
- [Troubleshooting](#troubleshooting)
- [Supported game builds](#supported-game-builds)
- [What it changes in the game folder](#what-it-changes-in-the-game-folder)
- [Logs](#logs)
- [For mod authors](#for-mod-authors)
- [Reporting problems](#reporting-problems)
- [Licences](#licences)

## Quick start

1. Download `CosmicModLoader-<version>.zip` from [Releases](https://github.com/ren-/CosmicModLoader/releases/latest) and extract the **whole** zip into one folder.
2. Close the game, run `CosmicModLoader.exe`, press **Install loader**.
3. Drag a mod's `.zip` or `.dll` onto the app window.
4. Start the game. Press **Insert** (or click both sticks) to change mod settings.

## Step by step

### 1. Open the app

Extract the whole zip into one folder (the app needs the `lib` folder and the DLLs next to it),
close the game, and run `CosmicModLoader.exe`.

![The app on first start](images/1-first-start.png)

1. **Game folder.** The app looks for the game in every drive's `XboxGames` folder and in all
   Steam libraries and fills this in. If it picked the wrong copy or found nothing, press
   **Browse** and pick the folder that has `Game.exe` and `ParisEngine.dll` in it:
   - Xbox app: the `Content` folder, usually `C:\XboxGames\MARVEL Cosmic Invasion\Content`
   - Steam: the game folder itself, `...\steamapps\common\MARVEL Cosmic Invasion`
2. **Status.** Which game build it found and whether the loader is installed. Grey dot: vanilla
   game. Green: loader installed. Yellow: an older loader or an old Boss Hitstun install that
   **Update loader** will replace. Red: no game in that folder (use **Browse**), or a build it
   doesn't know (see [After a game update](#after-a-game-update)).
3. **Install loader.** Greyed out while the game is running, when the folder has no game in it,
   or when the build is unknown.
4. **Output.** What the app found and did. When there is more than one copy of the game on your
   PC, the others are listed here too.

Windows may show "Windows protected your PC" the first time: press **More info**, then
**Run anyway**. See [Troubleshooting](#troubleshooting) for why.

### 2. Install the loader

Press **Install loader**. It takes a second.

![After installing](images/2-installed.png)

1. The status turns green: the loader is installed and the `Mods` folder is empty.
2. What it did: it checked your game build, backed up the original `ParisEngine.dll`, and added
   the one call that starts the loader. The details are in
   [What it changes](#what-it-changes-in-the-game-folder).
3. The **Mods** list. Drag mods here (next step).

If Windows won't let the app write into the game folder, it asks to retry as administrator.

### 3. Add mods

Drag a mod's `.zip` or `.dll` from Explorer anywhere onto the app window (or onto
`CosmicModLoader.exe` itself). The app copies the mod's DLLs into `Mods`; dropping a mod you
already have replaces the old copy with the one you dropped. You can also copy DLLs into `Mods`
by hand.

![A mod added](images/3-mod-added.png)

1. The mod shows up with its version. "library (used by other mods)" is a helper DLL a mod
   shipped with, not a mod on its own. A red note means the file can't be loaded; the note says why.
2. **Launch game** starts the game the normal way (through the Xbox app or Steam).
3. **Open Mods folder** opens `Mods` in Explorer.
4. **Open loader log** shows what loaded in your last session. **Share loader log** copies the
   log (this session and the one before) to the clipboard and selects the file in Explorer, ready
   to attach to a bug report.
5. **Uninstall** removes the loader (step 5).

### 4. Play and change mod settings

Start the game as usual. Mods load at startup; nothing extra to do.

Press **Insert** on the keyboard, or click **both sticks** on a pad, to open the mod overlay.
Press it again to close it.

![The overlay](images/5-overlay.png)

*Screenshots of the overlay come from a preview window that runs the real overlay code over a
stand-in background; in the game it sits over the game screen.*

1. The overlay window. Drag the title bar to move it, drag the bottom-right corner to resize it,
   the arrow on the left folds it, and the **X** closes it. It remembers its position.
2. **Loader**: the loader's own options.
3. One section per mod. Click a section to open it. Mods without settings don't add one.

![Settings open](images/6-overlay-settings.png)

1. **Pause the game while this window is open.** On by default. In single player the game
   pauses while the overlay is open (music and menu animations keep going). Online it never pauses, like the game's
   own pause menu.
2. **Font size.** 16 px at any resolution by default; raise it on a big or 4K screen (10 to 48).
3. A mod's settings (here Boss Hitstun Overhaul). Changes apply right away and are saved when you
   close the overlay.

While the overlay is open the game gets no input, so your character won't move while you click
around. Mouse and keyboard work as in any window; a pad can navigate too (d-pad to move, A to
press, B to go back).

### 5. Remove a mod or the whole loader

- **One mod:** delete its DLL from `Mods` (**Open Mods folder**). The app's list updates when you
  switch back to it.
- **The whole loader:** close the game, run the app, press **Uninstall**.

![After uninstalling](images/4-uninstalled.png)

1. The status is back to vanilla: `ParisEngine.dll` is restored byte for byte and the loader's
   files are removed.
2. Your `Mods` folder is left alone, so reinstalling brings your mods back. Delete it by hand if
   you want it gone.

Verifying game files in the Xbox app or Steam also puts the original engine back; the loader's
files then do nothing, and **Uninstall** tidies them up.

## After a game update

A game update replaces `ParisEngine.dll`, which switches the loader off: the game starts vanilla
and your mods don't load. The app checks the engine file's exact version before touching
anything, so on a build it doesn't know it shows a red status and does nothing. Wait for a
loader update, then run the new app and press **Install loader** again. Your `Mods` folder and
mod settings stay.

## Troubleshooting

**"Windows protected your PC" or an antivirus warning.** The app is an unsigned .NET exe, so
SmartScreen warns about it (press **More info**, then **Run anyway**), and Defender's cloud
heuristics sometimes flag a new release for a day or two (names ending in `!ml` are
machine-learning guesses, not matched signatures). The exe is not packed or obfuscated: open it
in ILSpy or dnSpy to read exactly what it does.

**The game doesn't start after installing.** The patched engine needs `CosmicLoader.dll` next to
it. If an antivirus quarantined that file (or it got deleted), restore it, or press **Uninstall**
in the app (or verify game files) to go back to vanilla.

**"GAME IS RUNNING, close it first".** The app won't change files the game has open. Close the
game fully and switch back to the app.

**The app can't find the game.** Press **Browse** and pick the folder with `Game.exe` and
`ParisEngine.dll` in it (Xbox: the `Content` folder; Steam: the game folder itself).

**"...is already in the game folder but I didn't put it there".** Something else left a file with
the same name in the game folder. Delete the file it names, then install again.

**Insert does nothing in game.** Open the loader log (**Open loader log**). If it's missing or
empty, the loader didn't start: check the app's status says the loader is installed and that the
game build is supported. If the log says `Overlay ready`, try clicking the game window once so it
has keyboard focus.

**A mod doesn't seem to do anything.** Check the loader log: it lists every DLL it found, every
mod it started and why any failed. Then check the mod's own log (each mod's page says where).

## Supported game builds

| Build | Game folder |
|---|---|
| Xbox app 26.6.8.0 | `C:\XboxGames\MARVEL Cosmic Invasion\Content` |
| Steam, July 2026 build (game 1.0.0.13466) | `...\steamapps\common\MARVEL Cosmic Invasion` |

## What it changes in the game folder

- `ParisEngine.dll` is backed up to `.cosmicloader-backup\` and replaced by a copy with **one**
  added instruction: a call to `CosmicLoader.Loader.Initialize()` at the start of the engine's
  first startup method. The app checks that exactly one method changed and that removing the call
  gives back the original method's instructions unchanged.
- `CosmicLoader.dll` and `0Harmony.dll` are added next to it, the overlay files go into a
  `CosmicLoader\` subfolder, and an empty `Mods` folder is created.
- `Game.exe` is never opened or modified. Nothing from the game is downloaded or shipped: the patch
  is made on your PC, from your own copy.
- Save files are never touched, and nothing connects to the internet.

### What's in the zip

| File | What it is |
|---|---|
| `CosmicModLoader.exe` | the app: installs and removes the loader, adds mods (drag and drop), lists them |
| `CosmicLoader.dll` | the loader itself, copied into the game folder |
| `0Harmony.dll` | [Harmony](https://github.com/pardeike/Harmony), the patching library mods use (MIT) |
| `Mono.Cecil.dll` | [Mono.Cecil](https://github.com/jbevain/cecil), used by the app to patch the engine file (MIT) |
| `lib\` | the overlay: [Dear ImGui](https://github.com/ocornut/imgui) (`cimgui.dll`) and [ImGui.NET](https://github.com/ImGuiNET/ImGui.NET) with its .NET support DLLs (all MIT), installed to `CosmicLoader\` in the game folder |
| `*-LICENSE.txt` | the licences |

## Logs

`%LOCALAPPDATA%\CosmicLoader\loader.log` (**Open loader log** in the app) says which DLLs it
found, which mods it started and why any failed. Each game launch starts a fresh log and keeps the
previous one as `loader.old.log`. Mods write their own logs. The overlay keeps its window layout and
options (`imgui.ini`, `overlay.cfg`) in the same folder.

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
