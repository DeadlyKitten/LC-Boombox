using System;
using System.Collections.Generic;
using System.Text;

namespace CustomBoomboxTracks.Managers {
    internal class RandomSyncManager {
        private static List<WeakReference<BoomboxItem>> boomboxes = new List<WeakReference<BoomboxItem>>();

        public static void AddBoombox(BoomboxItem boombox) {
            BoomboxPlugin.LogInfo($"Adding boombox {boombox.GetHashCode()}");
            boomboxes.Add(new WeakReference<BoomboxItem>(boombox));
        }

        public static void ResyncRandomSeed(int mapSeed) {
            // go over all boomboxes and remove them if they were GC'd
            boomboxes.RemoveAll(x => {
                if (x.TryGetTarget(out BoomboxItem boombox)) {
                    BoomboxPlugin.LogInfo($"Resyncing boombox {boombox.GetHashCode()}");
                    // mapSeed - 10 because that's what the game does
                    boombox.musicRandomizer = new Random(mapSeed - 10);
                    return false;
                } else {
                    // boombox was GC'd, remove it
                    BoomboxPlugin.LogInfo($"A boombox was GC'd");
                    return true;
                }
            });
        }
    }
}
