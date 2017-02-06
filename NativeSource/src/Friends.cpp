#include "Friends.h"

extern "C" {
	API(const char*) Friends_GetPersonaName() {
		return SteamFriends()->GetPersonaName();
	}

	API(const char*) Friends_GetFriendPersonaName(CSteamID friendSteamID) {
		return SteamFriends()->GetFriendPersonaName(friendSteamID);
	}

	API(int32) Friends_GetFriendCount(int32 friendFlags) {
		return SteamFriends()->GetFriendCount(friendFlags);
	}

	API(CSteamIDRet) Friends_GetFriendByIndex(int32 index, int32 friendFlags) {
		//copy the 64-bit data
		return SteamFriends()->GetFriendByIndex(index, friendFlags).ConvertToUint64();
	}

	API(void) Friends_ActivateGameOverlayToWebPage(const char *url) {
		SteamFriends()->ActivateGameOverlayToWebPage(url);
	}
}