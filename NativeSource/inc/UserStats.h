#ifndef _USER_STATS_H
#define _USER_STATS_H

#include "Common.h"

extern "C" {
	API(bool) UserStats_RequestCurrentStats();
	API(bool) UserStats_StoreStats();
	API(bool) UserStats_ResetAllStatsAndRemoveAchievements();
	API(bool) UserStats_SetAchievement(const char *name);
	API(bool) UserStats_IsAchievementSet(const char *name);
	API(bool) UserStats_GetStatInt(const char *name, int32 *stat);
	API(bool) UserStats_SetStatInt(const char *name, int32 stat);
	API(bool) UserStats_GetStatFloat(const char *name, float *stat);
	API(bool) UserStats_SetStatFloat(const char *name, float stat);
}

#endif
