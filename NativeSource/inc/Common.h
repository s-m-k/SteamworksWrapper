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
	typedef int8 BOOLRET;

	enum SteamworksError {
		ERR_NO_ERROR = 0x0,
		ERR_UNKNOWN_ERROR = 0x1,
		ERR_LEADERBOARD_NOT_FOUND = 0x2,
		ERR_LEADERBOARD_NOT_READY = 0x3,
		ERR_CANT_DOWNLOAD_SCORES = 0x4,
		ERR_CANT_UPLOAD_SCORES = 0x5,
		ERR_CANT_CREATE_WORKSHOP_ITEM = 0x6,
		ERR_CANT_SUBMIT_WORKSHOP_ITEM = 0x7,
		ERR_CANT_SUBSCRIBE_ITEM = 0x8,
		ERR_CANT_UNSUBSCRIBE_ITEM = 0x9,
	};

	class PollEntity {
	public:
		SteamworksError error = ERR_NO_ERROR;
		EResult errorDetails = k_EResultOK;
		bool isDone = true;

		void ReportError(SteamworksError error);
		void ReportErrorDetailed(SteamworksError error, EResult result);
		void ReportDone();
	};
}

#endif
