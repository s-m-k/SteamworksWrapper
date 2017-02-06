#include "Common.h"

void ReportError(ErrorCallback onError, SteamworksError error) {
	if (onError != NULL) {
		onError(error);
	}
}

void ReportErrorDetailed(ErrorCallbackDetailed onError, SteamworksError error, EResult result) {
	if (onError != NULL) {
		onError(error, result);
	}
}