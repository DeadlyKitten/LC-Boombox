using CustomBoomboxTracks.Managers;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomBoomboxTracks.Patches {

    [HarmonyPatch(typeof(StartOfRound), "OnPlayerConnectedClientRpc")]
    public class StartOfRound_OnPlayerConnectedClientRpc {
        static void Postfix(StartOfRound __instance) {
            RandomSyncManager.ResyncRandomSeed(__instance.randomMapSeed);
        }
    }
}
