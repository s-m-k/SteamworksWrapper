using UnityEngine;
using System.Runtime.InteropServices;
using System;

namespace SteamworksWrapper {
    public abstract class UnmanagedDisposable : IDisposable {
        bool disposed = false;
        readonly protected IntPtr pointer;
        
        protected UnmanagedDisposable(IntPtr pointer) {
            this.pointer = pointer;
        }
        
        ~UnmanagedDisposable() {
            DisposeUnmanaged();
        }

        public void Dispose() {
            DisposeUnmanaged();
            GC.SuppressFinalize(this);
        }

        void DisposeUnmanaged() {
            if (!disposed) {
                DestroyUnmanaged(pointer);
                disposed = true;
            }
        }

        protected abstract void DestroyUnmanaged(IntPtr pointer);
    }
}