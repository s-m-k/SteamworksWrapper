using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace SteamworksWrapper {
    public sealed partial class Steam : MonoBehaviour {
        delegate void OnWorkshopError(SteamworksError error, DetailedResult result);
        delegate void OnWorkshopCreateItem(WorkshopCreateItemResult itemResult);
        delegate void OnWorkshopSubmitItem(WorkshopSubmitItemResult result);

        public enum UpdateParamErrors {
            TITLE = 0x1,
            DESCRIPTION = 0x2,
            LANGUAGE = 0x3,
            META = 0x4,
            VISIBILITY = 0x5,
            TAGS = 0x7,
            CONTENT_PATH = 0x8,
            PREVIEW_PATH = 0x9
        }

        public enum WorkshopFileType {
            First = 0,

            Community = 0,       // normal Workshop item that can be subscribed to
            Microtransaction = 1,        // Workshop item that is meant to be voted on for the purpose of selling in-game
            Collection = 2,      // a collection of Workshop or Greenlight items
            Art = 3,     // artwork
            Video = 4,       // external video
            Screenshot = 5,      // screenshot
            Game = 6,        // Greenlight game entry
            Software = 7,        // Greenlight software entry
            Concept = 8,     // Greenlight concept
            WebGuide = 9,        // Steam web guide
            IntegratedGuide = 10,        // application integrated guide
            Merch = 11,      // Workshop merchandise meant to be voted on for the purpose of being sold
            ControllerBinding = 12,      // Steam Controller bindings
            SteamworksAccessInvite = 13,     // internal
            SteamVideo = 14,     // Steam video
            GameManagedItem = 15,        // managed completely by the game, not the user, and not shown on the web

            // Update Max if you add values.
            Max = 16

        };

        public enum ItemUpdateStatus {
            Invalid = 0, // The item update handle was invalid, job might be finished, listen to SubmitItemUpdateResult_t
            Config = 1, // The item update is processing configuration data
            PreparingContent = 2, // The item update is reading and processing content files
            UploadingContent = 3, // The item update is uploading content changes to Steam
            UploadingPreviewFile = 4, // The item update is uploading new preview file image
            CommittingChanges = 5  // The item update is committing all changes
        };

        [Serializable]
        public class WorkshopItemInfo {
            public ulong fileID = 0;

            public string title = "";
            public string description = "";

            public string meta = "";

            [NonSerialized]
            [XmlIgnore]
            public string contentFolderPath = "";

            [NonSerialized]
            [XmlIgnore]
            public string previewImagePath = "";

            public string updateLanguage = "";

            public PublishedFileVisibility visibility = PublishedFileVisibility.Private;

            public List<string> tags = new List<string>();

            public string ToXML() {
                using (StringWriter writer = new StringWriter()) {
                    XmlSerializer xml = new XmlSerializer(typeof(WorkshopItemInfo));
                    xml.Serialize(writer, this);
                    return writer.ToString();
                }
            }

            public bool IsInvalid() {
                return fileID == 0;
            }

            public static WorkshopItemInfo FromXML(string input) {
                using (StringReader reader = new StringReader(input)) {
                    XmlSerializer xml = new XmlSerializer(typeof(WorkshopItemInfo));
                    return (WorkshopItemInfo)xml.Deserialize(reader);
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct WorkshopCreateItemResult {
            [MarshalAs(UnmanagedType.I1)]
            public bool needsToAcceptLegalAgreement;
            public DetailedResult result;
            public ulong publishedFieldID;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct WorkshopSubmitItemResult {
            [MarshalAs(UnmanagedType.I1)]
            public bool needsToAcceptLegalAgreement;
            public DetailedResult result;
        }

        public sealed class Workshop : UnmanagedDisposable {
            public event Action<SteamworksError, DetailedResult> onError;
            public event Action<WorkshopItemInfo, bool> onCreateItem;
            public event Action<bool> onSubmitItem;

            OnWorkshopError workshopErrorDelegate;
            OnWorkshopCreateItem createItemDelegate;
            OnWorkshopSubmitItem submitItemDelegate;
            
            public static Workshop Create() {
                if (!steamInitialized) {
                    throw new Exception("Leaderboard error: Steam is not initialized.");
                }

                return new Workshop();
            }

            private Workshop(): base(NativeMethods.Workshop_Create(appId)) {
                workshopErrorDelegate = new OnWorkshopError(OnWorkshopError);
                createItemDelegate = new OnWorkshopCreateItem(OnCreateItem);
                submitItemDelegate = new OnWorkshopSubmitItem(OnSubmitItem);

                NativeMethods.Workshop_OnError(pointer, workshopErrorDelegate);
                NativeMethods.Workshop_OnCreateItem(pointer, createItemDelegate);
                NativeMethods.Workshop_OnSubmitItem(pointer, submitItemDelegate);
            }

            public void CreateItem(WorkshopFileType fileType) {
                NativeMethods.Workshop_CreateItem(pointer, fileType);
            }

            public UpdateParamErrors[] UpdateItem(WorkshopItemInfo item, string changeNotes = "") {
                var fileID = item.fileID;
                NativeMethods.Workshop_StartItemUpdate(pointer, fileID);
                List<UpdateParamErrors> errors = new List<UpdateParamErrors>();

                if (!NativeMethods.Workshop_SetItemTitle(pointer, item.title)) {
                    errors.Add(UpdateParamErrors.TITLE);
                }

                if (!NativeMethods.Workshop_SetItemDescription(pointer, item.description)) {
                    errors.Add(UpdateParamErrors.DESCRIPTION);
                }

                if (!NativeMethods.Workshop_SetItemUpdateLanguage(pointer, item.updateLanguage)) {
                    errors.Add(UpdateParamErrors.LANGUAGE);
                }

                if (!NativeMethods.Workshop_SetItemMetadata(pointer, item.meta)) {
                    errors.Add(UpdateParamErrors.META);
                }

                if (!NativeMethods.Workshop_SetItemVisibility(pointer, item.visibility)) {
                    errors.Add(UpdateParamErrors.VISIBILITY);
                }

                if (!NativeMethods.Workshop_SetItemTags(pointer, item.tags.ToArray(), item.tags.Count)) {
                    errors.Add(UpdateParamErrors.TAGS);
                }

                if (!NativeMethods.Workshop_SetItemContent(pointer, item.contentFolderPath)) {
                    errors.Add(UpdateParamErrors.CONTENT_PATH);
                }

                if (!NativeMethods.Workshop_SetItemPreview(pointer, item.previewImagePath)) {
                    errors.Add(UpdateParamErrors.PREVIEW_PATH);
                }

                if (errors.Count == 0) {
                    NativeMethods.Workshop_SubmitItemUpdate(pointer, changeNotes);
                }

                return errors.ToArray();
            }

            void OnWorkshopError(SteamworksError error, DetailedResult result) {
                if (onError != null) {
                    onError(error, result);
                }
            }

            void OnCreateItem(WorkshopCreateItemResult result) {
                if (result.result != DetailedResult.OK) {
                    OnWorkshopError(SteamworksError.ERR_CANT_CREATE_WORKSHOP_ITEM, result.result);
                    return;
                }

                if (onCreateItem != null) {
                    onCreateItem(new WorkshopItemInfo() {
                        fileID = result.publishedFieldID
                    }, result.needsToAcceptLegalAgreement);
                }
            }

            void OnSubmitItem(WorkshopSubmitItemResult result) {
                if (result.result != DetailedResult.OK) {
                    OnWorkshopError(SteamworksError.ERR_CANT_SUBMIT_WORKSHOP_ITEM, result.result);
                    return;
                }

                if (onSubmitItem != null) {
                    onSubmitItem(result.needsToAcceptLegalAgreement);
                }
            }

            public ItemUpdateStatus TrackUploadProgress(out ulong uploaded, out ulong total) {
                uploaded = 0;
                total = 0;

                return NativeMethods.Workshop_TrackUploadProgress(pointer, ref uploaded, ref total);
            }

            protected override void DestroyUnmanaged(IntPtr pointer) {
                NativeMethods.Workshop_Destroy(pointer);
            }
        }
    }
}