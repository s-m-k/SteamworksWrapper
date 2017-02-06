#ifndef UGC_H
#define UGC_H

#include "Common.h"

extern "C" {
	typedef struct {
		BOOL needsToAcceptLegalAgreement;
		EResult result;
		PublishedFileId_t publishedFieldID;
	} CreateItemResult;

	typedef void(*WorkshopCreateItemCallback)(CreateItemResult);
	typedef void(*WorkshopSubmitItemCallback)(EResult);

	class Workshop {
	public:
		AppId_t appId;

		SteamParamStringArray_t tags;

		CCallResult<Workshop, CreateItemResult_t> createItemResult;
		CCallResult<Workshop, SubmitItemUpdateResult_t> submitItemResult;

		UGCUpdateHandle_t updateHandle;
		ErrorCallbackDetailed onError = NULL;
		WorkshopCreateItemCallback onCreateItem = NULL;
		WorkshopSubmitItemCallback onSubmitItem = NULL;

		void OnCreateItem(CreateItemResult_t *result, bool isFailure);
		void OnSubmitItem(SubmitItemUpdateResult_t *result, bool isFailure);

		Workshop(AppId_t appId);
		~Workshop();
	};

	API(Workshop*) Workshop_Create(AppId_t appId);
	API(void) Workshop_Destroy(Workshop *workshop);

	API(void) Workshop_OnError(Workshop *workshop, ErrorCallbackDetailed error);
	API(void) Workshop_OnCreateItem(Workshop *workshop, WorkshopCreateItemCallback callback);
	API(void) Workshop_OnSubmitItem(Workshop *workshop, WorkshopSubmitItemCallback callback);

	API(void) Workshop_CreateItem(Workshop *workshop, EWorkshopFileType type);
	API(void) Workshop_StartItemUpdate(Workshop *workshop, PublishedFileId_t fileID);
	API(BOOL) Workshop_SetItemTitle(Workshop *workshop, const char *title);
	API(BOOL) Workshop_SetItemDescription(Workshop *workshop, const char *description);
	API(BOOL) Workshop_SetItemUpdateLanguage(Workshop *workshop, const char *lang);
	API(BOOL) Workshop_SetItemMetadata(Workshop *workshop, const char *meta);
	API(BOOL) Workshop_SetItemVisibility(Workshop *workshop, ERemoteStoragePublishedFileVisibility visibility);
	API(BOOL) Workshop_SetItemTags(Workshop *workshop, const char **tags, int num);
	API(BOOL) Workshop_AddItemKeyValueTag(Workshop *workshop, const char *key, const char *value);
	API(BOOL) Workshop_RemoveItemKeyValueTags(Workshop *workshop, const char *key);
	API(BOOL) Workshop_SetItemContent(Workshop *workshop, const char *folder);
	API(BOOL) Workshop_SetItemPreview(Workshop *workshop, const char *file);
	API(void) Workshop_SubmitItemUpdate(Workshop *workshop, const char *changeNotes);
}

#endif
