using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
using System;
using System.Text;

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