using UnityEngine;

namespace SteamworksWrapper {
    public sealed partial class Steam : MonoBehaviour {
        public static class User {
            public static CSteamID GetSteamID() {
                if (!steamInitialized) {
                    return new CSteamID();
                }

                return NativeMethods.User_GetSteamID();
            }
        }
    }
}