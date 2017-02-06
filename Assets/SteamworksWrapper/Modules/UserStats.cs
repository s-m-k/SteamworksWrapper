using UnityEngine;

namespace SteamworksWrapper {
    public sealed partial class Steam : MonoBehaviour {
        public static class UserStats {
            public static bool SetIntStat(string name, int stat) {
                if (!steamInitialized) {
                    return false;
                }

                if (NativeMethods.UserStats_SetStatInt(name, stat)) {
                    needsStatsToStore = true;
                    return true;
                }

                return false;
            }

            public static bool SetFloatStat(string name, float stat) {
                if (!steamInitialized) {
                    return false;
                }

                if (NativeMethods.UserStats_SetStatFloat(name, stat)) {
                    needsStatsToStore = true;
                    return true;
                }

                return false;
            }

            public static bool GetIntStat(string name, out int stat) {
                stat = 0;
                if (!steamInitialized) {
                    return false;
                }

                return NativeMethods.UserStats_GetStatInt(name, ref stat);
            }

            public static bool GetFloatStat(string name, out float stat) {
                stat = 0;
                if (!steamInitialized) {
                    return false;
                }

                return NativeMethods.UserStats_GetStatFloat(name, ref stat);
            }

            public static bool GrowIntStatBy(string name, int val) {
                if (!steamInitialized) {
                    return false;
                }

                int s = 0;

                if (NativeMethods.UserStats_GetStatInt(name, ref s) && NativeMethods.UserStats_SetStatInt(name, s + val)) {
                    needsStatsToStore = true;
                    return true;
                }

                return false;
            }

            public static bool GrowFloatStatBy(string name, float val) {
                if (!steamInitialized) {
                    return false;
                }

                float s = 0;

                if (NativeMethods.UserStats_GetStatFloat(name, ref s) && NativeMethods.UserStats_SetStatFloat(name, s + val)) {
                    needsStatsToStore = true;
                    return true;
                }

                return false;
            }

            public static bool ResetAllStatsAndRemoveAchievements() {
                if (!steamInitialized) {
                    return false;
                }

                if (NativeMethods.UserStats_ResetAllStatsAndRemoveAchievements()) {
                    NativeMethods.UserStats_RequestCurrentStats();
                    needsStatsToStore = true;

                    return true;
                }

                return false;
            }

            public static bool RequestCurrentStats() {
                if (!steamInitialized) {
                    return false;
                }

                return NativeMethods.UserStats_RequestCurrentStats();
            }

            public static bool UnlockAchievement(string name) {
                if (!steamInitialized) {
                    return false;
                }

                if (NativeMethods.UserStats_IsAchievementSet(name)) {
                    Debug.Log("Unlocked an achievement: " + name + " [already unlocked]");
                    return false;
                }

                if (NativeMethods.UserStats_SetAchievement(name)) {
                    Debug.Log("Unlocked an achievement: " + name);
                    needsStatsToStore = true;
                    return true;
                }

                return false;
            }

            public static bool IsAchievementSet(string name) {
                if (!steamInitialized) {
                    return false;
                }

                return NativeMethods.UserStats_IsAchievementSet(name);
            }
        }
    }
}