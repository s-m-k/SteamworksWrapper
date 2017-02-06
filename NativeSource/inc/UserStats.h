#ifndef _USER_STATS_H
#define _USER_STATS_H

#include "Common.h"

extern "C" {
	API(BOOL) UserStats_RequestCurrentStats();
	API(BOOL) UserStats_StoreStats();
	API(BOOL) UserStats_ResetAllStatsAndRemoveAchievements();
	API(BOOL) UserStats_SetAchievement(const char *name);
	API(BOOL) UserStats_IsAchievementSet(const char *name);
	API(BOOL) UserStats_GetStatInt(const char *name, int32 *stat);
	API(BOOL) UserStats_SetStatInt(const char *name, int32 stat);
	API(BOOL) UserStats_GetStatFloat(const char *name, float *stat);
	API(BOOL) UserStats_SetStatFloat(const char *name, float stat);
}

#endif
