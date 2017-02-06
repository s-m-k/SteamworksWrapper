using UnityEngine;
using System.Runtime.InteropServices;

namespace SteamworksWrapper {
    public sealed partial class Steam : MonoBehaviour {
        public static class Friends {
            public static string GetPersonaName() {
                if (!steamInitialized) {
                    return "";
                }

                return Marshal.PtrToStringAnsi(NativeMethods.Friends_GetPersonaName());
            }

            public static string GetFriendPersonaName(CSteamID steamID) {
                if (!steamInitialized) {
                    return "";
                }

                return Marshal.PtrToStringAnsi(NativeMethods.Friends_GetFriendPersonaName(steamID));
            }

            public static CSteamID[] GetFriendList(FriendFlags flags) {
                if (!steamInitialized) {
                    return new CSteamID[0];
                }

                int intFlags = (int)flags;
                int count = NativeMethods.Friends_GetFriendCount(intFlags);
                CSteamID[] friends = new CSteamID[count];

                for (int i = 0; i < count; i++) {
                    friends[i] = NativeMethods.Friends_GetFriendByIndex(i, intFlags);
                }
                
                return friends;
            }
        }
    }
}