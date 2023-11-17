using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;

namespace CustomBoomboxTracks.Patches
{
    [HarmonyPatch(typeof(BoomboxItem), nameof(BoomboxItem.PocketItem))]
    internal class BoomboxItem_PocketItem
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var patchedInstructions = instructions.ToList();

            var skippedFirstCall = false;

            for (int i = 0; i < patchedInstructions.Count; i++)
            {
                if (skippedFirstCall)
                {
                    if (patchedInstructions[i].opcode == OpCodes.Call)
                        skippedFirstCall = true;

                    continue;
                }

                if (patchedInstructions[i].opcode == OpCodes.Ret) break;

                patchedInstructions[i].opcode = OpCodes.Nop;
            }

            return patchedInstructions;
        }
    }
}
