#ifndef _COMMON_H
#define _COMMON_H

#include "steam/steam_api.h"
#ifdef __linux__
#define API(x) x __cdecl
#elif __MACH__
#define API(x) x __cdecl
#else
#define API(x) __declspec(dllexport) x __cdecl
#endif

extern "C" {
	typedef int64 CSteamIDRet;

	//ensure we use exactly 1 byte for bool on the C++ <-> C# boundary
	typedef int8 BOOL;

	enum SteamworksError {
		ERR_NO_ERROR = 0x0,
		ERR_UNKNOWN_ERROR = 0x1,
		ERR_LEADERBOARD_NOT_FOUND = 0x2,
		ERR_LEADERBOARD_NOT_READY = 0x3,
		ERR_CANT_DOWNLOAD_SCORES = 0x4,
		ERR_CANT_UPLOAD_SCORES = 0x5,
		ERR_CANT_CREATE_ITEM = 0x6
	};

	typedef void(*ErrorCallback)(SteamworksError error);

	void ReportError(ErrorCallback onError, SteamworksError error) {
		if (onError != NULL) {
			onError(error);
		}
	}
}


#endif
