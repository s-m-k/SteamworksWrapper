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
        private const int individualPublic = 0x01100001; //universe set to Public (1), account type set to Individual (1)
        
        [StructLayout(LayoutKind.Explicit)]
        //TODO support it properly?
        public struct CSteamID {
            [FieldOffset(0)]
            public ulong allBytes;

            [FieldOffset(0)]
            public uint accountID;
            [FieldOffset(4)]
            public uint universeAndAccountType;

            public static CSteamID FromIndividualID(uint individualID) {
                return new CSteamID() {
                    accountID = individualID,
                    universeAndAccountType = individualPublic
                };
            }

            public bool IsIndividualAndPublic() {
                return universeAndAccountType == individualPublic;
            }
        }

        public enum FriendFlags {
            None = 0x00,
            Blocked = 0x01,
            FriendshipRequested = 0x02,
            Regular = 0x04,
            ClanMember = 0x08,
            OnGameServer = 0x10,
            RequestingFriendship = 0x80,
            RequestingInfo = 0x100,
            Ignored = 0x200,
            IgnoredFriend = 0x400,
            ChatMember = 0x1000,
            All = 0xFFFF,
        };

        public enum SteamworksError {
            ERR_NO_ERROR = 0x0,
            ERR_UNKNOWN_ERROR = 0x1,
            ERR_LEADERBOARD_NOT_FOUND = 0x2,
            ERR_LEADERBOARD_NOT_READY = 0x3,
            ERR_CANT_DOWNLOAD_SCORES = 0x4,
            ERR_CANT_UPLOAD_SCORES = 0x5
        }

        public enum LeaderboardDataRequest {
            GLOBAL = 0,
            GLOBAL_AROUND_USER = 1,
            FRIENDS = 2,
            USERS = 3,
        };

        public enum LeaderboardUpdateMethod {
            NONE = 0,
            KEEP_BEST = 1,
            REPLACE_WITH_NEW = 2,
        };
    }
}