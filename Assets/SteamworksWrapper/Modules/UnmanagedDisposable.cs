using UnityEngine;
using System.Runtime.InteropServices;
using System;

namespace SteamworksWrapper {
    public abstract class UnmanagedDisposable : IDisposable {
        bool disposed = false;
        protected IntPtr pointer;
        
        protected UnmanagedDisposable(IntPtr pointer) {
            this.pointer = pointer;
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void AssertDisposed() {
            if (disposed) {
                throw new ObjectDisposedException("Object has been disposed.");
            }
        }

        protected void Dispose(bool disposing) {
            if (!disposed) {
                if (IntPtr.Zero != pointer) {
                    Debug.Log("SteamworksWrapper: destroying native runtime object: " + GetType().Name + ".");
                    DestroyUnmanaged(pointer);
                    pointer = IntPtr.Zero;
                }

                DestroyManaged();

                disposed = true;
            }
        }

        protected abstract void DestroyUnmanaged(IntPtr pointer);
        protected abstract void DestroyManaged();
    }
}