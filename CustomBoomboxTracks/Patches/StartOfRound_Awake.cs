using CustomBoomboxTracks.Managers;
using HarmonyLib;

namespace CustomBoomboxTracks.Patches
{
    [HarmonyPatch(typeof(StartOfRound), "Awake")]
    internal class StartOfRound_Awake
    {
        static void Prefix() => AudioManager.Load();
    }
}
