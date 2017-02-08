#include "Common.h"

extern "C" {
	void PollEntity::ReportError(SteamworksError error) {
		this->error = error;
		isDone = true;
	}

	void PollEntity::ReportErrorDetailed(SteamworksError error, EResult result) {
		this->error = error;
		this->errorDetails = result;
		isDone = true;
	}

	void PollEntity::ReportDone() {
		error = ERR_NO_ERROR;
		errorDetails = k_EResultOK;
		isDone = true;
	}
}