using HarmonyLib;

namespace CustomBoomboxTracks.Patches
{
    [HarmonyPatch(typeof(BoomboxItem), "StartMusic")]
    internal class BoomboxItem_StartMusic
    {
        static void Postfix(BoomboxItem __instance, bool startMusic)
        {
            if (startMusic) BoomboxPlugin.LogInfo($"Playing {__instance.boomboxAudio.clip.name}");
        }
    }
}
