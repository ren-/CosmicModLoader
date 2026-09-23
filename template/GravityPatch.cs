using System.Reflection;
using HarmonyLib;

namespace MyMod
{
    // Example patch: scales the gravity your character falls with, so jumps float.
    // The method is found by name, so this builds without the game's DLLs.
    [HarmonyPatch]
    internal static class GravityPatch
    {
        private static MethodBase TargetMethod() =>
            AccessTools.PropertyGetter(AccessTools.TypeByName("Paris.Game.Actor.Player"), "GravityAcceleration");

        // Runs after the game's getter. __result is what the game computed.
        private static void Postfix(ref float __result)
        {
            if (Settings.Enabled) __result *= Settings.GravityScale;
        }
    }
}
