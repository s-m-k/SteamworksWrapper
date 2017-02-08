#ifndef _USER_STATS_H
#define _USER_STATS_H

#include "Common.h"

extern "C" {
	API(BOOLRET) UserStats_RequestCurrentStats();
	API(BOOLRET) UserStats_StoreStats();
	API(BOOLRET) UserStats_ResetAllStatsAndRemoveAchievements();
	API(BOOLRET) UserStats_SetAchievement(const char *name);
	API(BOOLRET) UserStats_IsAchievementSet(const char *name);
	API(BOOLRET) UserStats_GetStatInt(const char *name, int32 *stat);
	API(BOOLRET) UserStats_SetStatInt(const char *name, int32 stat);
	API(BOOLRET) UserStats_GetStatFloat(const char *name, float *stat);
	API(BOOLRET) UserStats_SetStatFloat(const char *name, float stat);
}

#endif
