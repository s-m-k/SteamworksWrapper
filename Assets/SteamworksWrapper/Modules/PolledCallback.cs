using UnityEngine;
using System.Runtime.InteropServices;
using System.Collections;
using System;

namespace SteamworksWrapper {
    public sealed partial class Steam : MonoBehaviour {
        interface IPollEntity {
            bool IsDone {
                get;
            }
        }

        //Normal callbacks crashed, "leaked" other native plugins like FMOD etc.,
        //especially on 32-bit platform. Had that problem with Steamworks.NET too.
        //Polling is ugly, but at least seem to work. Fuck marshalling.
        class PolledCallback {
            static WaitForSeconds interval = new WaitForSeconds(0.1f);

            IPollEntity pollEntity;
            Action callback;
            Coroutine processCoroutine;
            Steam instance;

            public PolledCallback(Steam instance, IPollEntity entity, Action callback) {
                pollEntity = entity;
                this.callback = callback;
                this.instance = instance;

                processCoroutine = instance.StartCoroutine(Process());
            }

            public void Cancel() {
                instance.StopCoroutine(processCoroutine);
            }

            IEnumerator Process() {
                while (!pollEntity.IsDone) {
                    yield return interval;
                }
                
                callback();
            }
        }
    }
}