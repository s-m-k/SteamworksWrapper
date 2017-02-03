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
        //TODO UTF-8 maybe
        static class NativeMethods {
#if UNITY_EDITOR_64 || UNITY_64
            const string dllName = "SteamworksWrapper64";
#else
            const string dllName = "SteamworksWrapper";
#endif

            [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
            public extern static bool InitializeSteam();

            [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
            public extern static bool IsRestartRequired(uint appId);

            [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
            public extern static void ShutdownSteam();

            [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
            public extern static void RunCallbacks();

            
            //LEADERBOARDS
            [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
            public extern static IntPtr Leaderboard_Create();

            [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
            public extern static void Leaderboard_Destroy(IntPtr leaderboard);


            [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
            public extern static void Leaderboard_OnError(IntPtr leaderboard, OnLeaderboardError onError);

            [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
            public extern static void Leaderboard_OnFind(IntPtr leaderboard, OnLeaderboardFind onFind);

            [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
            public extern static void Leaderboard_OnDownloadScores(IntPtr leaderboard, OnLeaderboardDownloadScores onDownload);
            
            [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
            public extern static void Leaderboard_OnUploadScore(IntPtr leaderboard, OnLeaderboardUploadScore onUpload);


            [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
            public extern static void Leaderboard_Find(IntPtr leaderboard, [MarshalAs(UnmanagedType.LPStr)] string name);

            [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
            public extern static void Leaderboard_DownloadScores(IntPtr leaderboard, LeaderboardDataRequest data, int from, int to);

            [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
            public extern static void Leaderboard_UploadScore(IntPtr leaderboard, LeaderboardUpdateMethod method, int score);


            //FRIENDS
            [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
            public extern static IntPtr Friends_GetPersonaName();

            [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
            public extern static IntPtr Friends_GetFriendPersonaName(CSteamID friendSteamID);

            [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
            public extern static int Friends_GetFriendCount(int flags);

            [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
            public extern static CSteamID Friends_GetFriendByIndex(int index, int flags);

            //USER
            [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
            public extern static CSteamID User_GetSteamID();

            //USERSTATS
            [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
            public extern static bool UserStats_GetStatInt([MarshalAs(UnmanagedType.LPStr)] string name, ref int stat);

            [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
            public extern static bool UserStats_SetStatInt([MarshalAs(UnmanagedType.LPStr)] string name, int stat);

            [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
            public extern static bool UserStats_GetStatFloat([MarshalAs(UnmanagedType.LPStr)] string name, ref float stat);

            [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
            public extern static bool UserStats_SetStatFloat([MarshalAs(UnmanagedType.LPStr)] string name, float stat);

            [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
            public extern static bool UserStats_SetAchievement([MarshalAs(UnmanagedType.LPStr)] string name, bool achievement);

            [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
            public extern static bool UserStats_IsAchievementSet([MarshalAs(UnmanagedType.LPStr)] string name);
            
            [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
            public extern static bool UserStats_ResetAllStatsAndRemoveAchievements();

            [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
            public extern static bool UserStats_StoreStats();

            [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
            public extern static bool UserStats_RequestCurrentStats();
        }
    }
}