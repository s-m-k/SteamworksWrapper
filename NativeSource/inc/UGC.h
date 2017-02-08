#ifndef UGC_H
#define UGC_H

#include "Common.h"

extern "C" {
	typedef struct {
		BOOLRET needsToAcceptLegalAgreement;
		EResult result;
		PublishedFileId_t publishedFieldID;
	} CreateItemResult;

	typedef struct {
		BOOLRET needsToAcceptLegalAgreement;
		EResult result;
	} SubmitItemResult;

	typedef void(*WorkshopCreateItemCallback)(CreateItemResult);
	typedef void(*WorkshopSubmitItemCallback)(SubmitItemResult);

	class Workshop : public PollEntity {
	public:
		AppId_t appId;

		SteamParamStringArray_t tags;

		CCallResult<Workshop, CreateItemResult_t> createItemCallResult;
		CCallResult<Workshop, SubmitItemUpdateResult_t> submitItemCallResult;

		CreateItemResult createItemResult;
		SubmitItemResult submitItemResult;

		UGCUpdateHandle_t updateHandle;

		void OnCreateItem(CreateItemResult_t *result, bool isFailure);
		void OnSubmitItem(SubmitItemUpdateResult_t *result, bool isFailure);

		Workshop(AppId_t appId);
		~Workshop();
	};

	API(Workshop*) Workshop_Create(AppId_t appId);
	API(void) Workshop_Destroy(Workshop *workshop);

	API(SteamworksError) Workshop_GetError(Workshop *workshop);
	API(EResult) Workshop_GetErrorDetails(Workshop *workshop);
	API(BOOLRET) Workshop_PollIsDone(Workshop *workshop);

	API(void) Workshop_GetCreateItemResult(Workshop *workshop, CreateItemResult *result);
	API(void) Workshop_GetSubmitItemResult(Workshop *workshop, SubmitItemResult *result);

	API(void) Workshop_CreateItem(Workshop *workshop, EWorkshopFileType type);
	API(void) Workshop_StartItemUpdate(Workshop *workshop, PublishedFileId_t fileID);
	API(BOOLRET) Workshop_SetItemTitle(Workshop *workshop, const char *title);
	API(BOOLRET) Workshop_SetItemDescription(Workshop *workshop, const char *description);
	API(BOOLRET) Workshop_SetItemUpdateLanguage(Workshop *workshop, const char *lang);
	API(BOOLRET) Workshop_SetItemMetadata(Workshop *workshop, const char *meta);
	API(BOOLRET) Workshop_SetItemVisibility(Workshop *workshop, ERemoteStoragePublishedFileVisibility visibility);
	API(BOOLRET) Workshop_SetItemTags(Workshop *workshop, const char **tags, int num);
	API(BOOLRET) Workshop_AddItemKeyValueTag(Workshop *workshop, const char *key, const char *value);
	API(BOOLRET) Workshop_RemoveItemKeyValueTags(Workshop *workshop, const char *key);
	API(BOOLRET) Workshop_SetItemContent(Workshop *workshop, const char *folder);
	API(BOOLRET) Workshop_SetItemPreview(Workshop *workshop, const char *file);
	API(void) Workshop_SubmitItemUpdate(Workshop *workshop, const char *changeNotes);
	API(EItemUpdateStatus) Workshop_TrackUploadProgress(Workshop *workshop, uint64 *uploaded, uint64 *total);

	API(BOOLRET) UGC_GetItemInstallInfo(PublishedFileId_t fileID, uint64 *sizeOnDisk, char *folder, uint32 folderSize, uint32 *timeStamp);
	API(BOOLRET) UGC_TrackDownloadProgress(PublishedFileId_t fileID, uint64 *bytesDownloaded, uint64 *bytesTotal);
	API(uint32) UGC_GetSubscribedItemsCount();
	API(uint32) UGC_GetSubscribedItems(PublishedFileId_t *items, uint32 maxEntries);
	API(uint32) UGC_GetItemState(PublishedFileId_t fileID);
	API(BOOLRET) UGC_Download(PublishedFileId_t fileID, BOOLRET highPriority);
}

#endif
