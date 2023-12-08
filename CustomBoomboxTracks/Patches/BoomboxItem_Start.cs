using HarmonyLib;
using CustomBoomboxTracks.Managers;

namespace CustomBoomboxTracks.Patches
{
    [HarmonyPatch(typeof(BoomboxItem), "Start")]
    internal class BoomboxItem_Start
    {
        static bool Prefix(BoomboxItem __instance)
        {
            if (AudioManager.FinishedLoading)
                AudioManager.ApplyClips(__instance);
            else
                AudioManager.OnAllSongsLoaded += () => AudioManager.ApplyClips(__instance);
            return true;
        }
    }
}
