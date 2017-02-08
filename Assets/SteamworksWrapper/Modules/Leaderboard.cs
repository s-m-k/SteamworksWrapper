using UnityEngine;
using System.Runtime.InteropServices;
using System;

namespace SteamworksWrapper {
    public sealed partial class Steam : MonoBehaviour {
        [StructLayout(LayoutKind.Sequential)]
        public struct LeaderboardEntry {
            public CSteamID steamID;
            public int globalRank;
            public int score;
        }

        public sealed class Leaderboard : UnmanagedDisposable, IPollEntity {
            bool ready = false;
            
            public event Action onFind;
            public event Action<LeaderboardEntry[]> onDownloadScores;
            public event Action onUploadScore;
            public event Action<SteamworksError> onError;

            PolledCallback callback = null;

            public bool IsDone {
                get {
                    AssertDisposed();
                    return NativeMethods.Leaderboard_PollIsDone(pointer);
                }
            }

            public static Leaderboard Create() {
                if (!steamInitialized) {
                    throw new Exception("Leaderboard error: Steam is not initialized.");
                }

                return new Leaderboard();
            }

            private Leaderboard(): base(NativeMethods.Leaderboard_Create()) {
            }

            protected override void DestroyUnmanaged(IntPtr pointer) {
                NativeMethods.Leaderboard_Destroy(pointer);
            }

            protected override void DestroyManaged() {
                CancelCallback(ref callback);
            }

            void OnDownloadScores() {
                AssertDisposed();

                if (AssertError()) {
                    return;
                }

                ulong count = NativeMethods.Leaderboard_GetEntriesCount(pointer);
                LeaderboardEntry[] scores = new LeaderboardEntry[count];

                for (ulong i = 0; i < count; i++) {
                    NativeMethods.Leaderboard_GetEntry(pointer, i, ref scores[i]);
                }

                if (onDownloadScores != null) {
                    onDownloadScores(scores);
                }
            }

            void OnUploadScore() {
                AssertDisposed();

                if (AssertError()) {
                    return;
                }

                if (onUploadScore != null) {
                    onUploadScore();
                }
            }

            void OnFind() {
                AssertDisposed();

                if (AssertError()) {
                    ready = false;
                    return;
                }

                ready = true;

                if (onFind != null) {
                    onFind();
                }
            }

            bool AssertError() {
                SteamworksError error = NativeMethods.Leaderboard_GetError(pointer);

                if (error != SteamworksError.ERR_NO_ERROR && onError != null) {
                    onError(error);
                    return true;
                } else {
                    return false;
                }
            }

            public void Find(string name) {
                AssertDisposed();

                NativeMethods.Leaderboard_Find(pointer, name);
                CancelCallback(ref callback);
                WaitForDone(this, OnFind);
            }

            public void DownloadScores(LeaderboardDataRequest dataRequest, int from, int to) {
                AssertDisposed();
                AssertReady();

                NativeMethods.Leaderboard_DownloadScores(pointer, dataRequest, from, to);
                CancelCallback(ref callback);
                WaitForDone(this, OnDownloadScores);
            }

            public void UploadScore(LeaderboardUpdateMethod method, int score) {
                AssertDisposed();
                AssertReady();

                NativeMethods.Leaderboard_UploadScore(pointer, method, score);
                CancelCallback(ref callback);
                WaitForDone(this, OnUploadScore);
            }

            void AssertReady() {
                if (!ready) {
                    throw new Exception("The leaderboard is not ready. Call Find(name) first.");
                }
            }
        }
    }
}