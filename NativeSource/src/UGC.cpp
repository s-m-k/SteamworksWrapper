#include "UGC.h"

extern "C" {
	Workshop::Workshop(AppId_t appId) {
		this->appId = appId;
	}

	Workshop::~Workshop() {
		createItemCallResult.Cancel();
		submitItemCallResult.Cancel();
	}

	void Workshop::OnSubmitItem(SubmitItemUpdateResult_t *result, bool isFailure) {
		if (isFailure || result->m_eResult != k_EResultOK) {
			ReportErrorDetailed(ERR_CANT_SUBMIT_WORKSHOP_ITEM, result->m_eResult);
			return;
		}

		submitItemResult.needsToAcceptLegalAgreement = result->m_bUserNeedsToAcceptWorkshopLegalAgreement;
		submitItemResult.result = result->m_eResult;

		ReportDone();
	}

	void Workshop::OnCreateItem(CreateItemResult_t *result, bool isFailure) {
		if (isFailure || result->m_eResult != k_EResultOK) {
			ReportErrorDetailed(ERR_CANT_CREATE_WORKSHOP_ITEM, result->m_eResult);
			return;
		}

		createItemResult.needsToAcceptLegalAgreement = result->m_bUserNeedsToAcceptWorkshopLegalAgreement;
		createItemResult.publishedFieldID = result->m_nPublishedFileId;
		createItemResult.result = result->m_eResult;
		
		ReportDone();
	}

	API(Workshop*) Workshop_Create(AppId_t appId) {
		return new Workshop(appId);
	}

	API(void) Workshop_Destroy(Workshop *workshop) {
		delete workshop;
	}

	API(SteamworksError) Workshop_GetError(Workshop *workshop) {
		return workshop->error;
	}

	API(EResult) Workshop_GetErrorDetails(Workshop *workshop) {
		return workshop->errorDetails;
	}

	API(BOOLRET) Workshop_PollIsDone(Workshop *workshop) {
		return workshop->isDone;
	}

	API(void) Workshop_GetCreateItemResult(Workshop *workshop, CreateItemResult *result) {
		*result = workshop->createItemResult;
	}

	API(void) Workshop_GetSubmitItemResult(Workshop *workshop, SubmitItemResult *result) {
		*result = workshop->submitItemResult;
	}

	API(void) Workshop_CreateItem(Workshop *workshop, EWorkshopFileType fileType) {
		SteamAPICall_t call = SteamUGC()->CreateItem(workshop->appId, fileType);
		workshop->isDone = false;
		workshop->createItemCallResult.Set(call, workshop, &Workshop::OnCreateItem);
	}

	API(void) Workshop_StartItemUpdate(Workshop *workshop, PublishedFileId_t fileID) {
		workshop->updateHandle = SteamUGC()->StartItemUpdate(workshop->appId, fileID);
	}

	API(BOOLRET) Workshop_SetItemTitle(Workshop *workshop, const char *title) {
		return SteamUGC()->SetItemTitle(workshop->updateHandle, title);
	}

	API(BOOLRET) Workshop_SetItemDescription(Workshop *workshop, const char *description) {
		return SteamUGC()->SetItemDescription(workshop->updateHandle, description);
	}

	API(BOOLRET) Workshop_SetItemUpdateLanguage(Workshop *workshop, const char *lang) {
		return SteamUGC()->SetItemUpdateLanguage(workshop->updateHandle, lang);
	}

	API(BOOLRET) Workshop_SetItemMetadata(Workshop *workshop, const char *meta) {
		return SteamUGC()->SetItemMetadata(workshop->updateHandle, meta);
	}

	API(BOOLRET) Workshop_SetItemVisibility(Workshop *workshop, ERemoteStoragePublishedFileVisibility visibility) {
		return SteamUGC()->SetItemVisibility(workshop->updateHandle, visibility);
	}

	API(BOOLRET) Workshop_SetItemTags(Workshop *workshop, const char **tags, int num) {
		workshop->tags.m_nNumStrings = num;
		workshop->tags.m_ppStrings = tags;

		return SteamUGC()->SetItemTags(workshop->updateHandle, &(workshop->tags));
	}

	API(BOOLRET) Workshop_AddItemKeyValueTag(Workshop *workshop, const char *key, const char *value) {
		return SteamUGC()->AddItemKeyValueTag(workshop->updateHandle, key, value);
	}

	API(BOOLRET) Workshop_RemoveItemKeyValueTags(Workshop *workshop, const char *key) {
		return SteamUGC()->RemoveItemKeyValueTags(workshop->updateHandle, key);
	}

	API(BOOLRET) Workshop_SetItemContent(Workshop *workshop, const char *folder) {
		return SteamUGC()->SetItemContent(workshop->updateHandle, folder);
	}

	API(BOOLRET) Workshop_SetItemPreview(Workshop *workshop, const char *file) {
		return SteamUGC()->SetItemPreview(workshop->updateHandle, file);
	}

	API(void) Workshop_SubmitItemUpdate(Workshop *workshop, const char *changeNote) {
		SteamAPICall_t call = SteamUGC()->SubmitItemUpdate(workshop->updateHandle, changeNote);
		workshop->isDone = false;
		workshop->submitItemCallResult.Set(call, workshop, &Workshop::OnSubmitItem);
	}

	API(EItemUpdateStatus) Workshop_TrackUploadProgress(Workshop *workshop, uint64 *uploaded, uint64 *total) {
		return SteamUGC()->GetItemUpdateProgress(workshop->updateHandle, uploaded, total);
	}

	API(BOOLRET) UGC_GetItemInstallInfo(PublishedFileId_t fileID, uint64 *sizeOnDisk, char *folder, uint32 folderSize, uint32 *timeStamp) {
		return SteamUGC()->GetItemInstallInfo(fileID, sizeOnDisk, folder, folderSize, timeStamp);
	}

	API(BOOLRET) UGC_TrackDownloadProgress(PublishedFileId_t fileID, uint64 *bytesDownloaded, uint64 *bytesTotal) {
		return SteamUGC()->GetItemDownloadInfo(fileID, bytesDownloaded, bytesTotal);
	}

	API(uint32) UGC_GetSubscribedItemsCount() {
		return SteamUGC()->GetNumSubscribedItems();
	}

	API(uint32) UGC_GetSubscribedItems(PublishedFileId_t *items, uint32 maxEntries) {
		return SteamUGC()->GetSubscribedItems(items, maxEntries);
	}

	API(uint32) UGC_GetItemState(PublishedFileId_t fileID) {
		return SteamUGC()->GetItemState(fileID);
	}

	API(BOOLRET) UGC_Download(PublishedFileId_t fileID, BOOLRET highPriority) {
		return SteamUGC()->DownloadItem(fileID, highPriority);
	}
}