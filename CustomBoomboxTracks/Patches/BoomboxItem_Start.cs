using HarmonyLib;
using CustomBoomboxTracks.Managers;

namespace CustomBoomboxTracks.Patches
{
    [HarmonyPatch(typeof(BoomboxItem), "Start")]
    internal class BoomboxItem_Start
    {
        static void Postfix(BoomboxItem __instance) => AudioManager.ApplyClips(__instance);
    }
}
