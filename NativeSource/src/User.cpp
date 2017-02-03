#include "User.h"

extern "C" {
	API(CSteamIDRet) User_GetSteamID() {
		//copy the 64-bit data
		return SteamUser()->GetSteamID().ConvertToUint64();
	}
}