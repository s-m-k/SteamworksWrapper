#ifndef _LEADERBOARD_H
#define _LEADERBOARD_H

#include <vector>
#include "Common.h"
#include "steam\steam_api.h"

extern "C" {
	typedef struct {
		CSteamID steamId;
		int32 globalRank;
		int32 score;
	} LeaderboardEntry;

	typedef void(*LeaderboardFindCallback)();
	typedef void(*LeaderboardErrorCallback)(SteamworksError error);
	typedef void(*LeaderboardDownloadCallback)(LeaderboardEntry* scores, int count);
	typedef void(*LeaderboardUploadCallback)();

	class Leaderboard {
	private:
		SteamLeaderboard_t handle = -1;
		std::vector<LeaderboardEntry> downloadedEntries;

		void ReportError(SteamworksError error);
	public:
		LeaderboardErrorCallback onError = NULL;
		LeaderboardFindCallback onFind = NULL;
		LeaderboardDownloadCallback onDownload = NULL;
		LeaderboardUploadCallback onUpload = NULL;

		CCallResult<Leaderboard, LeaderboardFindResult_t> findCallResult;
		CCallResult<Leaderboard, LeaderboardScoresDownloaded_t> scoresDownloadedResult;
		CCallResult<Leaderboard, LeaderboardScoreUploaded_t> scoreUploadedResult;

		void OnReady(LeaderboardFindResult_t *result, bool isFailure);
		void OnDownloaded(LeaderboardScoresDownloaded_t *result, bool isFailure);
		void OnUploaded(LeaderboardScoreUploaded_t *result, bool isFailure);

		void Find(const char* name);
		void DownloadScores(ELeaderboardDataRequest mode, int32 from, int32 to);
		void UploadScore(ELeaderboardUploadScoreMethod, int32 score);

		~Leaderboard();
	};

	API(Leaderboard*) Leaderboard_Create();
	API(void) Leaderboard_Destroy(Leaderboard* leaderboard);
	API(void) Leaderboard_OnError(Leaderboard* leaderboard, LeaderboardErrorCallback errorCallback);
	API(void) Leaderboard_OnFind(Leaderboard* leaderboard, LeaderboardFindCallback findCallback);
	API(void) Leaderboard_OnDownloadScores(Leaderboard* leaderboard, LeaderboardDownloadCallback downloadCallback);
	API(void) Leaderboard_OnUploadScore(Leaderboard* leaderboard, LeaderboardUploadCallback uploadCallback);
	API(void) Leaderboard_Find(Leaderboard* leaderboard, const char* name);
	API(void) Leaderboard_DownloadScores(Leaderboard* leaderboard, ELeaderboardDataRequest data, int32 from, int32 to);
	API(void) Leaderboard_UploadScore(Leaderboard* leaderboard, ELeaderboardUploadScoreMethod method, int32 score);
}

#endif