#include "UserStats.h"

extern "C" {
	API(bool) UserStats_RequestCurrentStats() {
		return SteamUserStats()->RequestCurrentStats();
	}

	API(bool) UserStats_StoreStats() {
		return SteamUserStats()->StoreStats();
	}

	API(bool) UserStats_ResetAllStatsAndRemoveAchievements() {
		return SteamUserStats()->ResetAllStats(true);
	}

	API(bool) UserStats_SetAchievement(const char *name) {
		return SteamUserStats()->SetAchievement(name);
	}

	API(bool) UserStats_IsAchievementSet(const char *name) {
		bool isSet;

		if (SteamUserStats()->GetAchievement(name, &isSet)) {
			return isSet;
		}

		return false;
	}

	API(bool) UserStats_GetStatInt(const char *name, int32 *stat) {
		return SteamUserStats()->GetStat(name, stat);
	}

	API(bool) UserStats_SetStatInt(const char *name, int32 stat) {
		return SteamUserStats()->SetStat(name, stat);
	}

	API(bool) UserStats_GetStatFloat(const char *name, float *stat) {
		return SteamUserStats()->GetStat(name, stat);
	}

	API(bool) UserStats_SetStatFloat(const char *name, float stat) {
		return SteamUserStats()->SetStat(name, stat);
	}
}