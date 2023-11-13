using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;

namespace CustomBoomboxTracks
{
    [BepInPlugin(GUID, NAME, VERSION)]
    public class BoomboxPlugin : BaseUnityPlugin
    {
        private const string GUID = "com.steven.lethalcompany.boomboxmusic";
        private const string NAME = "Custom Boombox Music";
        private const string VERSION = "1.0.0";

        private static BoomboxPlugin Instance;

        void Awake()
        {
            Instance = this;

            Configuration.Config.Init();

            var harmony = new Harmony(GUID);
            harmony.PatchAll();
        }


        #region logging
        internal static void LogDebug(string message) => Instance.Log(message, LogLevel.Debug);
        internal static void LogInfo(string message) => Instance.Log(message, LogLevel.Info);
        internal static void LogWarning(string message) => Instance.Log(message, LogLevel.Warning);
        internal static void LogError(string message) => Instance.Log(message, LogLevel.Error);
        internal static void LogError(Exception ex) => Instance.Log($"{ex.Message}\n{ex.StackTrace}", LogLevel.Error);
        private void Log(string message, LogLevel logLevel) => Logger.Log(logLevel, message);
        #endregion
    }
}
