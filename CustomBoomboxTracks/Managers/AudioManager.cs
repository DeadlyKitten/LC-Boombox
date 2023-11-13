using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BepInEx;
using CustomBoomboxTracks.Configuration;
using CustomBoomboxTracks.Utilities;
using UnityEngine;
using UnityEngine.Networking;

namespace CustomBoomboxTracks.Managers
{
    internal static class AudioManager
    {
        static string[] allSongPaths;
        static List<AudioClip> clips = new List<AudioClip>();
        static bool firstRun = true;

        public static bool HasNoSongs => allSongPaths.Length == 0;

        public static void Load()
        {
            if (firstRun)
            {
                var directory = Path.Combine(Paths.BepInExRootPath, "Custom Songs", "Boombox Music");
                Directory.CreateDirectory(directory);

                allSongPaths = Directory.GetFiles(directory);

                BoomboxPlugin.LogDebug("Preparing to load AudioClips...");
                firstRun = false;

                foreach (var track in allSongPaths)
                {
                    SharedCoroutineStarter.StartCoroutine(LoadAudioClip(track));
                }
            }
        }

        private static IEnumerator LoadAudioClip(string filePath)
        {
            BoomboxPlugin.LogDebug($"Loading {filePath}!");
            var loader = UnityWebRequestMultimedia.GetAudioClip(filePath, GetAudioType(filePath));

            if (Config.StreamFromDisk) (loader.downloadHandler as DownloadHandlerAudioClip).streamAudio = true;

            loader.SendWebRequest();

            while (true)
            {
                if (loader.isDone) break;
                yield return null;
            }

            if (loader.error != null)
            {
                BoomboxPlugin.LogError($"Error loading clip from path: {filePath}\n{loader.error}");
                BoomboxPlugin.LogError(loader.error);
                yield break;
            }

            BoomboxPlugin.LogDebug($"Loaded {filePath}!");
            clips.Add(DownloadHandlerAudioClip.GetContent(loader));
        }

        public static void ApplyClips(BoomboxItem __instance)
        {
            BoomboxPlugin.LogDebug($"Applying clips!");

            if (Config.UseDefaultSongs)
                __instance.musicAudios = __instance.musicAudios.Concat(clips).ToArray();
            else
                __instance.musicAudios = clips.ToArray();

            BoomboxPlugin.LogDebug($"Total Clip Count: {__instance.musicAudios.Length}");
        }

        private static AudioType GetAudioType(string path)
        {
            if (Path.GetExtension(path).ToLower() == ".wav")
                return AudioType.WAV;
            if (Path.GetExtension(path).ToLower() == ".ogg")
                return AudioType.OGGVORBIS;
            return AudioType.MPEG;
        }
    }
}
