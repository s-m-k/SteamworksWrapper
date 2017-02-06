#include "UserStats.h"

extern "C" {
	API(BOOL) UserStats_RequestCurrentStats() {
		return SteamUserStats()->RequestCurrentStats();
	}

	API(BOOL) UserStats_StoreStats() {
		return SteamUserStats()->StoreStats();
	}

	API(BOOL) UserStats_ResetAllStatsAndRemoveAchievements() {
		return SteamUserStats()->ResetAllStats(true);
	}

	API(BOOL) UserStats_SetAchievement(const char *name) {
		return SteamUserStats()->SetAchievement(name);
	}

	API(BOOL) UserStats_IsAchievementSet(const char *name) {
		bool isSet;

		if (SteamUserStats()->GetAchievement(name, &isSet)) {
			return isSet;
		}

		return false;
	}

	API(BOOL) UserStats_GetStatInt(const char *name, int32 *stat) {
		return SteamUserStats()->GetStat(name, stat);
	}

	API(BOOL) UserStats_SetStatInt(const char *name, int32 stat) {
		return SteamUserStats()->SetStat(name, stat);
	}

	API(BOOL) UserStats_GetStatFloat(const char *name, float *stat) {
		return SteamUserStats()->GetStat(name, stat);
	}

	API(BOOL) UserStats_SetStatFloat(const char *name, float stat) {
		return SteamUserStats()->SetStat(name, stat);
	}
}