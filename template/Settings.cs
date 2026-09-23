using System;
using System.Globalization;
using System.IO;

namespace MyMod
{
    // Settings in a small text file next to the DLL (Mods\MyMod.cfg).
    internal static class Settings
    {
        public static bool Enabled = true;
        public static float GravityScale = 0.6f;

        private static string FilePath =>
            Path.Combine(Path.GetDirectoryName(typeof(Settings).Assembly.Location), "MyMod.cfg");

        public static void Load()
        {
            try
            {
                if (!File.Exists(FilePath)) return;
                foreach (string line in File.ReadAllLines(FilePath))
                {
                    string[] parts = line.Split(new[] { '=' }, 2);
                    if (parts.Length != 2) continue;
                    string key = parts[0].Trim(), value = parts[1].Trim();
                    if (key == "Enabled") bool.TryParse(value, out Enabled);
                    else if (key == "GravityScale" && float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out float scale))
                        GravityScale = Math.Max(0.2f, Math.Min(1.5f, scale));
                }
            }
            catch (Exception) { /* a broken file just means defaults */ }
        }

        // Called by the loader when the overlay closes and when the game exits.
        public static void Save()
        {
            File.WriteAllLines(FilePath, new[]
            {
                "Enabled=" + Enabled,
                "GravityScale=" + GravityScale.ToString(CultureInfo.InvariantCulture)
            });
        }
    }
}
