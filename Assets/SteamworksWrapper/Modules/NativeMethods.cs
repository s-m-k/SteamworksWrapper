using UnityEngine;
using System.Runtime.InteropServices;
using System;

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
            [return: MarshalAs(UnmanagedType.I1)]
            public extern static bool InitializeSteam();

            [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            public extern static bool IsRestartRequired(uint appId);

            [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            public extern static bool ShutdownSteam();

            [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            public extern static bool RunCallbacks();

            
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

            [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
            public extern static void Friends_ActivateGameOverlayToWebPage([MarshalAs(UnmanagedType.LPStr)] string url);


            //USER
            [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
            public extern static CSteamID User_GetSteamID();


            //USERSTATS
            [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            public extern static bool UserStats_GetStatInt([MarshalAs(UnmanagedType.LPStr)] string name, ref int stat);

            [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            public extern static bool UserStats_SetStatInt([MarshalAs(UnmanagedType.LPStr)] string name, int stat);

            [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            public extern static bool UserStats_GetStatFloat([MarshalAs(UnmanagedType.LPStr)] string name, ref float stat);

            [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            public extern static bool UserStats_SetStatFloat([MarshalAs(UnmanagedType.LPStr)] string name, float stat);

            [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            public extern static bool UserStats_SetAchievement([MarshalAs(UnmanagedType.LPStr)] string name);

            [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            public extern static bool UserStats_IsAchievementSet([MarshalAs(UnmanagedType.LPStr)] string name);
            
            [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            public extern static bool UserStats_ResetAllStatsAndRemoveAchievements();

            [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            public extern static bool UserStats_StoreStats();

            [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            public extern static bool UserStats_RequestCurrentStats();


            //UGC
            [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
            public extern static IntPtr Workshop_Create(uint appId);

            [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
            public extern static void Workshop_Destroy(IntPtr workshop);

            [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
            public extern static void Workshop_CreateItem(IntPtr workshop, WorkshopFileType fileType);

            [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
            public extern static void Workshop_StartItemUpdate(IntPtr workshop, ulong fileID);

            [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            public extern static bool Workshop_SetItemTitle(IntPtr workshop, [MarshalAs(UnmanagedType.LPStr)] string title);

            [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            public extern static bool Workshop_SetItemDescription(IntPtr workshop, [MarshalAs(UnmanagedType.LPStr)] string description);

            [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            public extern static bool Workshop_SetItemUpdateLanguage(IntPtr workshop, [MarshalAs(UnmanagedType.LPStr)] string lang);

            [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            public extern static bool Workshop_SetItemMetadata(IntPtr workshop, [MarshalAs(UnmanagedType.LPStr)] string meta);

            [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            public extern static bool Workshop_SetItemVisibility(IntPtr workshop, PublishedFileVisibility visibility);

            [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            public extern static bool Workshop_SetItemTags(IntPtr workshop,
                [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr, SizeParamIndex = 2)] string[] tags, int count);

            [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            public extern static bool Workshop_SetItemContent(IntPtr workshop, [MarshalAs(UnmanagedType.LPStr)] string path);

            [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            public extern static bool Workshop_SetItemPreview(IntPtr workshop, [MarshalAs(UnmanagedType.LPStr)] string path);
            
            [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
            public extern static void Workshop_SubmitItemUpdate(IntPtr workshop, [MarshalAs(UnmanagedType.LPStr)] string changeNotes);
            
            [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
            public extern static void Workshop_OnError(IntPtr workshop, OnWorkshopError error);

            [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
            public extern static void Workshop_OnCreateItem(IntPtr workshop, OnWorkshopCreateItem createItem);

            [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
            public extern static void Workshop_OnSubmitItem(IntPtr workshop, OnWorkshopSubmitItem submitItem);
            
            [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
            public extern static ItemUpdateStatus Workshop_TrackUploadProgress(IntPtr workshop, ref ulong uploaded, ref ulong total);
    
            [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            public extern static bool UGC_GetItemInstallInfo(ulong fileID, ref ulong sizeOnDisk, IntPtr folder, uint folderSize, ref uint timestamp);

            [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            public extern static bool UGC_TrackDownloadProgress(ulong fileID, ref ulong bytesDownloaded, ref ulong bytesTotal);

            [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
            public extern static uint UGC_GetSubscribedItemsCount();

            [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
            public extern static uint UGC_GetSubscribedItems([MarshalAs(UnmanagedType.LPArray,ArraySubType = UnmanagedType.U8,SizeParamIndex = 1)]ulong[] items, uint maxEntries);

            [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
            public extern static WorkshopItemState UGC_GetItemState(ulong fileID);

            [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            public extern static bool UGC_Download(ulong fileID, [MarshalAs(UnmanagedType.I1)] bool highPriority);
        }
    }
}