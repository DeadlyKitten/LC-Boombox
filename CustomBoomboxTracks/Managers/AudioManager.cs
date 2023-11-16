using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using BepInEx;
using CustomBoomboxTracks.Configuration;
using CustomBoomboxTracks.Utilities;
using UnityEngine;
using UnityEngine.Networking;

namespace CustomBoomboxTracks.Managers
{
    internal static class AudioManager
    {
        public static event Action OnAllSongsLoaded;
        public static bool FinishedLoading => finishedLoading;

        static string[] allSongPaths;
        static List<AudioClip> clips = new List<AudioClip>();
        static bool firstRun = true;
        static bool finishedLoading = false;

        static readonly string directory = Path.Combine(Paths.BepInExRootPath, "Custom Songs", "Boombox Music");

        public static bool HasNoSongs => allSongPaths.Length == 0;

        public static void GenerateFolders() => Directory.CreateDirectory(directory);

        public static void Load()
        {
            if (firstRun)
            {
                firstRun = false;
                allSongPaths = Directory.GetFiles(directory);

                BoomboxPlugin.LogInfo("Preparing to load AudioClips...");

                var coroutines = new List<Coroutine>();
                foreach (var track in allSongPaths)
                {
                    var coroutine = SharedCoroutineStarter.StartCoroutine(LoadAudioClip(track));
                    coroutines.Add(coroutine);
                }

                SharedCoroutineStarter.StartCoroutine(WaitForAllClips(coroutines));
            }
        }

        private static IEnumerator LoadAudioClip(string filePath)
        {
            BoomboxPlugin.LogInfo($"Loading {filePath}!");

            var audioType = GetAudioType(filePath);
            if (audioType == AudioType.UNKNOWN)
            {
                BoomboxPlugin.LogError($"Failed to load AudioClip from {filePath}\nUnsupported file extension!");
                yield break;
            }

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

            var clip = DownloadHandlerAudioClip.GetContent(loader);
            if (clip && clip.loadState == AudioDataLoadState.Loaded)
            {
                BoomboxPlugin.LogInfo($"Loaded {filePath}");
                clip.name = Path.GetFileName(filePath);
                clips.Add(clip);
                yield break;
            }

            // Failed to load.
            BoomboxPlugin.LogError($"Failed to load clip at: {filePath}\nThis might be due to an mismatch between the audio codec and the file extension!");
        }

        private static IEnumerator WaitForAllClips(List<Coroutine> coroutines)
        {
            foreach(var coroutine in coroutines)
            {
                yield return coroutine;
            }

            finishedLoading = true;
            OnAllSongsLoaded?.Invoke();
            OnAllSongsLoaded = null;
        }

        public static void ApplyClips(BoomboxItem __instance)
        {
            BoomboxPlugin.LogInfo($"Applying clips!");

            if (Config.UseDefaultSongs)
                __instance.musicAudios = __instance.musicAudios.Concat(clips).ToArray();
            else
                __instance.musicAudios = clips.ToArray();

            BoomboxPlugin.LogInfo($"Total Clip Count: {__instance.musicAudios.Length}");
        }

        private static AudioType GetAudioType(string path)
        {
            var extension = Path.GetExtension(path).ToLower();   

            if (extension == ".wav")
                return AudioType.WAV;
            if (extension == ".ogg")
                return AudioType.OGGVORBIS;
            if (extension == ".mp3")
                return AudioType.MPEG;

            BoomboxPlugin.LogError($"Unsupported extension type: {extension}");
            return AudioType.UNKNOWN;
        }
    }
}
