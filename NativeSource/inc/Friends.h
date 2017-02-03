#ifndef _FRIENDS_H
#define _FRIENDS_H

#include "Common.h"

extern "C" {
	API(const char*) Friends_GetPersonaName();
	API(const char*) Friends_GetFriendPersonaName(CSteamID friendSteamID);
	API(int32) Friends_GetFriendCount(int32 friendFlags);
	API(CSteamIDRet) Friends_GetFriendByIndex(int32 index, int32 friendFlags);
}

#endif