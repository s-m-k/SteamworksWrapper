using UnityEngine;
using System.Collections;
using System;

namespace SteamworksWrapper {
    public sealed partial class Steam : MonoBehaviour {

        static Steam instance;
        static bool steamInitialized = false;
        static bool needsStatsToStore = false;

        private Steam() {

        }

        public static Steam Instance {
            get {
                return instance;
            }
        }

        public static bool IsInitialized {
            get {
                return steamInitialized;
            }
        }

        public static void Init(uint appId, GameObject obj) {
            //TODO check if steam exists

            try {
                if (NativeMethods.IsRestartRequired(appId)) {
                    Application.Quit();
                    return;
                }

                if (!NativeMethods.InitializeSteam()) {
                    throw new Exception("SteamAPI_Init() failed. Refer to Valve's documentation or the comment above this line for more information.");
                }

                steamInitialized = true;

                instance = obj.AddComponent<Steam>();
            } catch (Exception e) {
                Debug.LogError("Error: " + e.Message);
                steamInitialized = false;
            }
        }

        void Awake() {
            Debug.Log("Steam API initialized: " + steamInitialized);
        }

        public static void Shutdown() {
            if (!steamInitialized) {
                return;
            }

            Debug.Log("Shutting down the Steam API.");
            NativeMethods.ShutdownSteam();
        }

        static void SteamDebug(int nSeverity, System.Text.StringBuilder pchDebugText) {
            Debug.LogWarning(pchDebugText);
        }

        IEnumerator TryStoringStatsAgain() {
            yield return new WaitForSeconds(5);
            needsStatsToStore = true;
        }

        private void Update() {
            if (!steamInitialized) {
                return;
            }

            if (needsStatsToStore) {
                needsStatsToStore = false;

                if (!NativeMethods.UserStats_StoreStats()) {
                    Debug.LogError("Failed to store Steam stats. Check your internet connection. Trying again in 5 seconds.");
                    StartCoroutine(TryStoringStatsAgain());
                }
            }

            NativeMethods.RunCallbacks();
        }
    }
}