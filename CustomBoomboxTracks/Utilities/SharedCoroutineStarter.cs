using System.Collections;
using UnityEngine;

namespace CustomBoomboxTracks.Utilities
{
    /// <summary>
    /// Utility class for running coroutines from non-Monobehaviours.
    /// </summary>
    public class SharedCoroutineStarter : MonoBehaviour
    {
        private static SharedCoroutineStarter _instance;

        /// <summary>
        /// Starts a coroutine, without the need for the calling class to inherit from Monobehaviour.
        /// </summary>
        /// <param name="routine">The Coroutine to start.</param>
        /// <returns>The started Coroutine.</returns>
        public static new Coroutine StartCoroutine(IEnumerator routine)
        {
            if (_instance == null)
            {
                _instance = new GameObject("Shared Coroutine Starter").AddComponent<SharedCoroutineStarter>();
                DontDestroyOnLoad(_instance);
            }

            return ((MonoBehaviour)_instance).StartCoroutine(routine);
        }
    }
}
