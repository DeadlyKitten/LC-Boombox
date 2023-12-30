using CustomBoomboxTracks.Managers;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomBoomboxTracks.Patches {

    [HarmonyPatch(typeof(RoundManager), "GenerateNewLevelClientRpc")]
    internal class RoundManager_GenerateNewLevelClientRpc {
        static void Postfix(RoundManager __instance) {
            RandomSyncManager.ResyncRandomSeed(__instance.playersManager.randomMapSeed);
        }
    }
}
