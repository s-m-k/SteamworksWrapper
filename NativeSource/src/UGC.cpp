#include "UGC.h"

extern "C" {
	Workshop::Workshop(AppId_t appId) {
		this->appId = appId;
	}

	Workshop::~Workshop() {
		createItemResult.Cancel();
	}

	API(void) Workshop_CreateItem(Workshop *workshop, EWorkshopFileType fileType) {
		SteamAPICall_t call = SteamUGC()->CreateItem(workshop->appId, fileType);
		workshop->createItemResult.Set(call, workshop, &Workshop::OnCreateItem);
	}

	void Workshop::OnCreateItem(CreateItemResult_t *result, bool isFailure) {
		if (isFailure) {
			ReportError(ERR_CANT_CREATE_ITEM);
			return;
		}

		CreateItemResult c;
		c.needsToAcceptLegalAgreement = result->m_bUserNeedsToAcceptWorkshopLegalAgreement;
		c.publishedFieldID = result->m_nPublishedFileId;
		c.result = result->m_eResult;

		if (onCreateItem != NULL) {
			onCreateItem(c);
		}
	}

	API(Workshop*) Workshop_Create(AppId_t appId) {
		return new Workshop(appId);
	}

	API(void) Workshop_Destroy(Workshop *workshop) {
		delete workshop;
	}

	API(void) Workshop_OnCreateItem(Workshop *workshop, WorkshopCreateItemCallback callback) {
		workshop->onCreateItem = callback;
	}

	API(void) Workshop_OnError(Workshop *workshop, ErrorCallback callback) {
		workshop->onError = callback;
	}
}