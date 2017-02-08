#include "UserStats.h"

extern "C" {
	API(BOOLRET) UserStats_RequestCurrentStats() {
		return SteamUserStats()->RequestCurrentStats();
	}

	API(BOOLRET) UserStats_StoreStats() {
		return SteamUserStats()->StoreStats();
	}

	API(BOOLRET) UserStats_ResetAllStatsAndRemoveAchievements() {
		return SteamUserStats()->ResetAllStats(true);
	}

	API(BOOLRET) UserStats_SetAchievement(const char *name) {
		return SteamUserStats()->SetAchievement(name);
	}

	API(BOOLRET) UserStats_IsAchievementSet(const char *name) {
		bool isSet;

		if (SteamUserStats()->GetAchievement(name, &isSet)) {
			return isSet;
		}

		return false;
	}

	API(BOOLRET) UserStats_GetStatInt(const char *name, int32 *stat) {
		return SteamUserStats()->GetStat(name, stat);
	}

	API(BOOLRET) UserStats_SetStatInt(const char *name, int32 stat) {
		return SteamUserStats()->SetStat(name, stat);
	}

	API(BOOLRET) UserStats_GetStatFloat(const char *name, float *stat) {
		return SteamUserStats()->GetStat(name, stat);
	}

	API(BOOLRET) UserStats_SetStatFloat(const char *name, float stat) {
		return SteamUserStats()->SetStat(name, stat);
	}
}