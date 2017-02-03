#include "Leaderboard.h"

extern "C" {
	void Leaderboard::OnReady(LeaderboardFindResult_t* result, bool isFailure) {
		if (isFailure || result->m_bLeaderboardFound == 0) {
			ReportError(ERR_LEADERBOARD_NOT_FOUND);
			return;
		}

		handle = result->m_hSteamLeaderboard;

		if (onFind != NULL) {
			onFind();
		}
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
			outEntry.steamId = entry.m_steamIDUser;
			outEntry.globalRank = entry.m_nGlobalRank;

			downloadedEntries.push_back(outEntry);
		}

		if (onDownload != NULL) {
			if (downloadedEntries.size() > 0) {
				onDownload(&downloadedEntries[0], count);
			}
			else {
				onDownload(NULL, count);
			}
		}
	}

	void Leaderboard::OnUploaded(LeaderboardScoreUploaded_t *result, bool isFailure) {
		if (!result->m_bSuccess || isFailure) {
			ReportError(ERR_CANT_UPLOAD_SCORES);
			return;
		}

		if (onUpload != NULL) {
			onUpload();
		}
	}

	void Leaderboard::Find(const char* name) {
		SteamAPICall_t call = SteamUserStats()->FindLeaderboard(name);
		findCallResult.Set(call, this, &Leaderboard::OnReady);
	}

	void Leaderboard::DownloadScores(ELeaderboardDataRequest mode, int32 from, int32 to) {
		SteamAPICall_t call = SteamUserStats()->DownloadLeaderboardEntries(handle, mode, from, to);
		scoresDownloadedResult.Set(call, this, &Leaderboard::OnDownloaded);
	}

	void Leaderboard::UploadScore(ELeaderboardUploadScoreMethod method, int32 score) {
		SteamAPICall_t call = SteamUserStats()->UploadLeaderboardScore(handle, method, score, NULL, 0);
		scoreUploadedResult.Set(call, this, &Leaderboard::OnUploaded);
	}

	void Leaderboard::ReportError(SteamworksError error) {
		if (onError != NULL) {
			onError(error);
		}
	}

	Leaderboard::~Leaderboard() {
		findCallResult.Cancel();
		scoreUploadedResult.Cancel();
		scoreUploadedResult.Cancel();
	}

	API(Leaderboard*) Leaderboard_Create() {
		return new Leaderboard();
	}

	API(void) Leaderboard_Destroy(Leaderboard* leaderboard) {
		delete leaderboard;
	}

	API(void) Leaderboard_OnError(Leaderboard* leaderboard, LeaderboardErrorCallback errorCallback) {
		leaderboard->onError = errorCallback;
	}

	API(void) Leaderboard_OnFind(Leaderboard* leaderboard, LeaderboardFindCallback findCallback) {
		leaderboard->onFind = findCallback;
	}

	API(void) Leaderboard_OnDownloadScores(Leaderboard* leaderboard, LeaderboardDownloadCallback downloadCallback) {
		leaderboard->onDownload = downloadCallback;
	}

	API(void) Leaderboard_OnUploadScore(Leaderboard* leaderboard, LeaderboardUploadCallback uploadCallback) {
		leaderboard->onUpload = uploadCallback;
	}

	API(void) Leaderboard_Find(Leaderboard* leaderboard, const char* name) {
		leaderboard->Find(name);
	}

	API(void) Leaderboard_DownloadScores(Leaderboard* leaderboard, ELeaderboardDataRequest data, int32 from, int32 to) {
		leaderboard->DownloadScores(data, from, to);
	}

	API(void) Leaderboard_UploadScore(Leaderboard* leaderboard, ELeaderboardUploadScoreMethod method, int32 score) {
		leaderboard->UploadScore(method, score);
	}
}