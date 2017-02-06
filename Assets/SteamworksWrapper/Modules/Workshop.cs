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
        delegate void OnWorkshopSubmitItem(DetailedResult result);

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

        [Serializable]
        public class WorkshopItemInfo {
            public ulong fileID = 0;

            public string title = "";
            public string description = "";

            public string meta = "";

            public string contentFolderPath = "";
            public string previewImagePath = "";

            public List<string> tags = new List<string>();

            public string ToXML() {
                using (StringWriter writer = new StringWriter()) {
                    XmlSerializer xml = new XmlSerializer(typeof(WorkshopItemInfo));
                    xml.Serialize(writer, this);
                    return writer.ToString();
                }
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

        public sealed class Workshop : UnmanagedDisposable {
            bool ready = false;

            public event Action<SteamworksError, DetailedResult> onError;
            public event Action onNeedsToAcceptLegalAgreement;
            public event Action<WorkshopItemInfo> onCreateItem;
            public event Action onSubmitItem;

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

            public void UpdateItem(WorkshopItemInfo item, string changeNotes = "", string updateLanguage = "english") {
                var fileID = item.fileID;
                NativeMethods.Workshop_StartItemUpdate(pointer, fileID);

                NativeMethods.Workshop_SetItemTitle(pointer, item.title);
                NativeMethods.Workshop_SetItemDescription(pointer, item.description);
                NativeMethods.Workshop_SetItemUpdateLanguage(pointer, updateLanguage);
                NativeMethods.Workshop_SetItemMetadata(pointer, item.meta);
                NativeMethods.Workshop_SetItemTags(pointer, item.tags.ToArray(), item.tags.Count);
                NativeMethods.Workshop_SetItemContent(pointer, item.contentFolderPath);
                NativeMethods.Workshop_SetItemPreview(pointer, item.previewImagePath);
                NativeMethods.Workshop_SubmitItemUpdate(pointer, changeNotes);
            }

            void OnWorkshopError(SteamworksError error, DetailedResult result) {
                if (onError != null) {
                    onError(error, result);
                }
            }

            void OnCreateItem(WorkshopCreateItemResult result) {
                if (result.needsToAcceptLegalAgreement) {
                    if (onNeedsToAcceptLegalAgreement != null) {
                        onNeedsToAcceptLegalAgreement();
                    }

                    return;
                }

                if (onCreateItem != null) {
                    onCreateItem(new WorkshopItemInfo() {
                        fileID = result.publishedFieldID
                    });
                }
            }

            void OnSubmitItem(DetailedResult result) {
                if (onSubmitItem != null) {
                    onSubmitItem();
                }
            }

            protected override void DestroyUnmanaged(IntPtr pointer) {
                NativeMethods.Workshop_Destroy(pointer);
            }
        }
    }
}