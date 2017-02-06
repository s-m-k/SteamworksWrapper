#include "UGC.h"

extern "C" {
	Workshop::Workshop(AppId_t appId) {
		this->appId = appId;
	}

	Workshop::~Workshop() {
		createItemResult.Cancel();
		submitItemResult.Cancel();
	}

	void Workshop::OnSubmitItem(SubmitItemUpdateResult_t *result, bool isFailure) {
		if (isFailure) {
			ReportErrorDetailed(onError, ERR_CANT_SUBMIT_WORKSHOP_ITEM, result->m_eResult);
			return;
		}

		if (NULL != onSubmitItem) {
			onSubmitItem(result->m_eResult);
		}
	}

	void Workshop::OnCreateItem(CreateItemResult_t *result, bool isFailure) {
		if (isFailure) {
			ReportErrorDetailed(onError, ERR_CANT_CREATE_WORKSHOP_ITEM, result->m_eResult);
			return;
		}

		CreateItemResult c;
		c.needsToAcceptLegalAgreement = result->m_bUserNeedsToAcceptWorkshopLegalAgreement;
		c.publishedFieldID = result->m_nPublishedFileId;
		c.result = result->m_eResult;

		if (NULL != onCreateItem) {
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

	API(void) Workshop_OnSubmitItem(Workshop *workshop, WorkshopSubmitItemCallback callback) {
		workshop->onSubmitItem = callback;
	}

	API(void) Workshop_OnError(Workshop *workshop, ErrorCallbackDetailed callback) {
		workshop->onError = callback;
	}

	API(void) Workshop_CreateItem(Workshop *workshop, EWorkshopFileType fileType) {
		SteamAPICall_t call = SteamUGC()->CreateItem(workshop->appId, fileType);
		workshop->createItemResult.Set(call, workshop, &Workshop::OnCreateItem);
	}

	API(void) Workshop_StartItemUpdate(Workshop *workshop, PublishedFileId_t fileID) {
		workshop->updateHandle = SteamUGC()->StartItemUpdate(workshop->appId, fileID);
	}

	API(BOOL) Workshop_SetItemTitle(Workshop *workshop, const char *title) {
		return SteamUGC()->SetItemTitle(workshop->updateHandle, title);
	}

	API(BOOL) Workshop_SetItemDescription(Workshop *workshop, const char *description) {
		return SteamUGC()->SetItemDescription(workshop->updateHandle, description);
	}

	API(BOOL) Workshop_SetItemUpdateLanguage(Workshop *workshop, const char *lang) {
		return SteamUGC()->SetItemUpdateLanguage(workshop->updateHandle, lang);
	}

	API(BOOL) Workshop_SetItemMetadata(Workshop *workshop, const char *meta) {
		return SteamUGC()->SetItemMetadata(workshop->updateHandle, meta);
	}

	API(BOOL) Workshop_SetItemVisibility(Workshop *workshop, ERemoteStoragePublishedFileVisibility visibility) {
		return SteamUGC()->SetItemVisibility(workshop->updateHandle, visibility);
	}

	API(BOOL) Workshop_SetItemTags(Workshop *workshop, const char **tags, int num) {
		workshop->tags.m_nNumStrings = num;
		workshop->tags.m_ppStrings = tags;

		return SteamUGC()->SetItemTags(workshop->updateHandle, &(workshop->tags));
	}

	API(BOOL) Workshop_AddItemKeyValueTag(Workshop *workshop, const char *key, const char *value) {
		return SteamUGC()->AddItemKeyValueTag(workshop->updateHandle, key, value);
	}

	API(BOOL) Workshop_RemoveItemKeyValueTags(Workshop *workshop, const char *key) {
		return SteamUGC()->RemoveItemKeyValueTags(workshop->updateHandle, key);
	}

	API(BOOL) Workshop_SetItemContent(Workshop *workshop, const char *folder) {
		return SteamUGC()->SetItemContent(workshop->updateHandle, folder);
	}

	API(BOOL) Workshop_SetItemPreview(Workshop *workshop, const char *file) {
		return SteamUGC()->SetItemPreview(workshop->updateHandle, file);
	}

	API(void) Workshop_SubmitItemUpdate(Workshop *workshop, const char *changeNote) {
		SteamAPICall_t call = SteamUGC()->SubmitItemUpdate(workshop->updateHandle, changeNote);
		workshop->submitItemResult.Set(call, workshop, &Workshop::OnSubmitItem);
	}
}