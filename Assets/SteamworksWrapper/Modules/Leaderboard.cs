using UnityEngine;
using System.Runtime.InteropServices;
using System;

namespace SteamworksWrapper {
    public sealed partial class Steam : MonoBehaviour {
        delegate void OnLeaderboardError(SteamworksError error);
        delegate void OnLeaderboardFind();
        delegate void OnLeaderboardDownloadScores([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)]LeaderboardEntry[] entries, int count);
        delegate void OnLeaderboardUploadScore();

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct LeaderboardEntry {
            public CSteamID steamID;
            public int globalRank;
            public int score;
        }

        public sealed class Leaderboard : IDisposable {
            bool disposed = false;
            bool ready = false;
            IntPtr pointer;

            public event Action onFind;
            public event Action<LeaderboardEntry[]> onDownloadScores;
            public event Action onUploadScore;
            public event Action<SteamworksError> onError;

            OnLeaderboardError errorDelegate;
            OnLeaderboardDownloadScores downloadScoresDelegate;
            OnLeaderboardFind findDelegate;
            OnLeaderboardUploadScore uploadScoreDelegate;

            public static Leaderboard Create() {
                if (!steamInitialized) {
                    throw new Exception("Leaderboard error: Steam is not initialized.");
                }

                return new Leaderboard();
            }

            private Leaderboard() {
                pointer = NativeMethods.Leaderboard_Create();
                errorDelegate = new OnLeaderboardError(OnError);
                findDelegate = new OnLeaderboardFind(OnFind);
                downloadScoresDelegate = new OnLeaderboardDownloadScores(OnDownloadScores);
                uploadScoreDelegate = new OnLeaderboardUploadScore(OnUploadScore);

                NativeMethods.Leaderboard_OnError(pointer, errorDelegate);
                NativeMethods.Leaderboard_OnFind(pointer, findDelegate);
                NativeMethods.Leaderboard_OnDownloadScores(pointer, downloadScoresDelegate);
                NativeMethods.Leaderboard_OnUploadScore(pointer, uploadScoreDelegate);
            }

            void OnDownloadScores(LeaderboardEntry[] scores, int count) {
                if (onDownloadScores != null) {
                    onDownloadScores(scores);
                }
            }

            void OnUploadScore() {
                if (onUploadScore != null) {
                    onUploadScore();
                }
            }

            void OnError(SteamworksError error) {
                if (onError != null) {
                    onError(error);
                }
            }

            void OnFind() {
                ready = true;

                if (onFind != null) {
                    onFind();
                }
            }

            ~Leaderboard() {
                DisposeUnmanaged();
            }

            public void Find(string name) {
                NativeMethods.Leaderboard_Find(pointer, name);
            }

            public void DownloadScores(LeaderboardDataRequest dataRequest, int from, int to) {
                AssertReady();
                NativeMethods.Leaderboard_DownloadScores(pointer, dataRequest, from, to);
            }

            public void UploadScore(LeaderboardUpdateMethod method, int score) {
                AssertReady();
                NativeMethods.Leaderboard_UploadScore(pointer, method, score);
            }

            public void Dispose() {
                DisposeUnmanaged();
                GC.SuppressFinalize(this);
            }

            void AssertReady() {
                if (!ready) {
                    throw new Exception("The leaderboard is not ready. Call Find(name) first.");
                }
            }

            void DisposeUnmanaged() {
                if (!disposed) {
                    NativeMethods.Leaderboard_Destroy(pointer);
                    disposed = true;
                }
            }
        }
    }
}