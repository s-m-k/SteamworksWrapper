#include "SteamworksWrapper.h"

static IUnityInterfaces* unityInterfaces = NULL;

extern "C" {
	void UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API UnityPluginLoad(IUnityInterfaces* ui) {
		unityInterfaces = ui;
	}

	void UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API UnityPluginUnload() {

	}

	API(BOOLRET) InitializeSteam() {
		return SteamAPI_Init();
	}

	API(BOOLRET) IsRestartRequired(uint32 appId) {
		return SteamAPI_RestartAppIfNecessary(appId);
	}

	API(void) ShutdownSteam() {
		SteamAPI_Shutdown();
	}

	API(void) RunCallbacks() {
		SteamAPI_RunCallbacks();
	}
}