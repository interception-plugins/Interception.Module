using System;

namespace interception.hooks {
    public struct method_backup {
        public IntPtr ptr;
        public byte a;
        public byte b;
        public byte c;
        public byte d;
        public byte e;
        public ulong f64;
        public uint f32;
        public byte g;

        internal unsafe method_backup(IntPtr ptr) {
            this.ptr = ptr;
            byte* _ptr = (byte*)ptr.ToPointer();
            this.a = *_ptr;
            this.b = _ptr[1];
            this.c = _ptr[10];
            this.d = _ptr[11];
            this.e = _ptr[12];
            if (IntPtr.Size == 8) {
                this.f64 = (ulong)(*(long*)(_ptr + 2));
                this.f32 = 0;
                this.g = 0;
            }
            else {
                this.f64 = 0;
                this.f32 = *(uint*)(_ptr + 1);
                this.g = _ptr[5];
            }
        }
    }
}
