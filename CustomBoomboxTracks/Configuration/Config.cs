using System.IO;
using BepInEx;
using BepInEx.Configuration;
using CustomBoomboxTracks.Managers;

namespace CustomBoomboxTracks.Configuration
{
    internal static class Config
    {
        private const string CONFIG_FILE_NAME = "boombox.cfg";

        private static ConfigFile _config;
        private static ConfigEntry<bool> _useDefaultSongs;
        private static ConfigEntry<bool> _streamAudioFromDisk;

        public static void Init()
        {
            BoomboxPlugin.LogDebug("Initializing config...");
            var filePath = Path.Combine(Paths.ConfigPath, CONFIG_FILE_NAME);
            _config = new ConfigFile(filePath, true);
            _useDefaultSongs = _config.Bind("Config", "Use Default Songs", false, "Include the default songs in the rotation.");
            _streamAudioFromDisk = _config.Bind("Config", "Stream Audio From Disk", false, "Requires less memory and takes less time to load, but prevents playing the same song twice at once.");
            BoomboxPlugin.LogDebug("Config initialized!");
        }

        public static bool UseDefaultSongs => _useDefaultSongs.Value || AudioManager.HasNoSongs;
        public static bool StreamFromDisk => _streamAudioFromDisk.Value;
    }
}
