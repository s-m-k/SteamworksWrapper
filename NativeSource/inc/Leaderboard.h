#ifndef _LEADERBOARD_H
#define _LEADERBOARD_H

#include <vector>
#include "Common.h"

extern "C" {
	typedef struct {
		CSteamIDRet steamId;
		int32 globalRank;
		int32 score;
	} LeaderboardEntry;

	/*typedef void(*LeaderboardFindCallback)();
	typedef void(*LeaderboardDownloadCallback)(LeaderboardEntry* scores, int count);
	typedef void(*LeaderboardUploadCallback)();*/

	class Leaderboard : public PollEntity {
	public:
		SteamLeaderboard_t handle = -1;
		std::vector<LeaderboardEntry> downloadedEntries;

		CCallResult<Leaderboard, LeaderboardFindResult_t> findCallResult;
		CCallResult<Leaderboard, LeaderboardScoresDownloaded_t> scoresDownloadedResult;
		CCallResult<Leaderboard, LeaderboardScoreUploaded_t> scoreUploadedResult;

		void OnReady(LeaderboardFindResult_t *result, bool isFailure);
		void OnDownloaded(LeaderboardScoresDownloaded_t *result, bool isFailure);
		void OnUploaded(LeaderboardScoreUploaded_t *result, bool isFailure);

		~Leaderboard();
	};

	API(Leaderboard*) Leaderboard_Create();
	API(void) Leaderboard_Destroy(Leaderboard* leaderboard);

	API(SteamworksError) Leaderboard_GetError(Leaderboard* leaderboard);
	API(BOOLRET) Leaderboard_PollIsDone(Leaderboard* leaderboard);

	API(uint64) Leaderboard_GetEntriesCount(Leaderboard* leaderboard);
	API(void) Leaderboard_GetEntry(Leaderboard* leaderboard, uint64 index, LeaderboardEntry *entry);

	API(void) Leaderboard_Find(Leaderboard* leaderboard, const char* name);
	API(void) Leaderboard_DownloadScores(Leaderboard* leaderboard, ELeaderboardDataRequest data, int32 from, int32 to);
	API(void) Leaderboard_UploadScore(Leaderboard* leaderboard, ELeaderboardUploadScoreMethod method, int32 score);
}

#endif
