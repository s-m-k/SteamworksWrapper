#include "Leaderboard.h"
	void Leaderboard::OnReady(LeaderboardFindResult_t* result, bool isFailure) {
		if (isFailure || result->m_bLeaderboardFound == 0) {
			ReportError(ERR_LEADERBOARD_NOT_FOUND);
			return;
		}

		handle = result->m_hSteamLeaderboard;
		ReportDone();
	}

	void Leaderboard::OnDownloaded(LeaderboardScoresDownloaded_t *result, bool isFailure) {
		if (isFailure) {
			ReportError(ERR_CANT_DOWNLOAD_SCORES);
			return;
		}

		downloadedEntries.clear();

		int32 count = result->m_cEntryCount;
		SteamLeaderboardEntries_t entriesHandle = result->m_hSteamLeaderboardEntries;

		for (int32 i = 0; i < count; i++) {
			LeaderboardEntry_t entry;
			LeaderboardEntry outEntry;

			SteamUserStats()->GetDownloadedLeaderboardEntry(entriesHandle, i, &entry, NULL, 0);
			outEntry.score = entry.m_nScore;
			outEntry.steamId = entry.m_steamIDUser.ConvertToUint64();
			outEntry.globalRank = entry.m_nGlobalRank;

			downloadedEntries.push_back(outEntry);
		}

		ReportDone();
	}

	void Leaderboard::OnUploaded(LeaderboardScoreUploaded_t *result, bool isFailure) {
		if (!result->m_bSuccess || isFailure) {
			ReportError(ERR_CANT_UPLOAD_SCORES);
			return;
		}

		ReportDone();
	}

	Leaderboard::~Leaderboard() {
		findCallResult.Cancel();
		scoresDownloadedResult.Cancel();
		scoreUploadedResult.Cancel();
	}

extern "C" {

	API(Leaderboard*) Leaderboard_Create() {
		return new Leaderboard();
	}

	API(void) Leaderboard_Destroy(Leaderboard* leaderboard) {
		delete leaderboard;
	}

	API(void) Leaderboard_Find(Leaderboard* leaderboard, const char* name) {
		SteamAPICall_t call = SteamUserStats()->FindLeaderboard(name);
		leaderboard->isDone = false;
		leaderboard->findCallResult.Set(call, leaderboard, &Leaderboard::OnReady);
	}

	API(SteamworksError) Leaderboard_GetError(Leaderboard* leaderboard) {
		return leaderboard->error;
	}

	API(BOOLRET) Leaderboard_PollIsDone(Leaderboard* leaderboard) {
		return leaderboard->isDone;
	}

	API(uint64) Leaderboard_GetEntriesCount(Leaderboard* leaderboard) {
		return leaderboard->downloadedEntries.size();
	}

	API(void) Leaderboard_GetEntry(Leaderboard* leaderboard, uint64 index, LeaderboardEntry *entry) {
		*entry = leaderboard->downloadedEntries[index];
	}

	API(void) Leaderboard_DownloadScores(Leaderboard* leaderboard, ELeaderboardDataRequest data, int32 from, int32 to) {
		SteamAPICall_t call = SteamUserStats()->DownloadLeaderboardEntries(leaderboard->handle, data, from, to);
		leaderboard->isDone = false;
		leaderboard->scoresDownloadedResult.Set(call, leaderboard, &Leaderboard::OnDownloaded);
	}

	API(void) Leaderboard_UploadScore(Leaderboard* leaderboard, ELeaderboardUploadScoreMethod method, int32 score) {
		SteamAPICall_t call = SteamUserStats()->UploadLeaderboardScore(leaderboard->handle, method, score, NULL, 0);
		leaderboard->isDone = false;
		leaderboard->scoreUploadedResult.Set(call, leaderboard, &Leaderboard::OnUploaded);
	}
}