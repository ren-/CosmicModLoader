using CosmicLoader;
using HarmonyLib;
using ImGuiNET;

namespace MyMod
{
    // The loader calls MyMod.ModEntry.Initialize() once, early in game startup.
    // The namespace must match the AssemblyName in MyMod.csproj.
    public static class ModEntry
    {
        // The title of your section in the overlay.
        public const string Name = "My Mod";

        public static void Initialize()
        {
            Settings.Load();
            // Applies every [HarmonyPatch] class in this DLL (here: GravityPatch).
            new Harmony("yourname.mymod").PatchAll(typeof(ModEntry).Assembly);
            Overlay.OnDrawUI(Name, DrawUI);
            Overlay.OnConfigSave(Name, Settings.Save);
        }

        // Runs every frame while the overlay is open and your section is expanded.
        private static void DrawUI()
        {
            ImGui.Checkbox("Low gravity jumps", ref Settings.Enabled);
            ImGui.SliderFloat("Gravity", ref Settings.GravityScale, 0.2f, 1.5f, "%.2fx");
            if (ImGui.IsItemHovered())
            {
                ImGui.BeginTooltip();
                ImGui.TextUnformatted("1.00x is the normal game. Lower floats more.");
                ImGui.EndTooltip();
            }
        }
    }
}
