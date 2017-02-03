#ifndef _STEAMWORKS_WRAPPER_H
#define _STEAMWORKS_WRAPPER_H

#ifdef _WIN64
#undef _WIN32
#endif

#include "IUnityInterface.h"
#include "Common.h"
#include "Leaderboard.h"
#include "UserStats.h"
#include "Friends.h"

extern "C" {
	void UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API UnityPluginLoad(IUnityInterfaces* unityInterfaces);
	void UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API UnityPluginUnload();

	API(bool) InitializeSteam();
	API(bool) IsRestartRequired(uint32 appId);
	API(void) ShutdownSteam();
	API(void) RunCallbacks();
}

#endif
