using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace interception.hooks {
    public class hook {
        public MethodBase original_method { get; private set; }
        public IntPtr original_ptr { get; private set; }
        public IntPtr hook_ptr { get; private set; }
        public method_backup backup { get; private set; }
        public bool is_enabled { get; private set; }

        public hook(MethodBase original_method, IntPtr original_ptr, IntPtr hook_ptr) {
            this.original_method = original_method;
            this.original_ptr = original_ptr;
            this.hook_ptr = hook_ptr;
            this.backup = new method_backup(this.original_ptr);
            this.is_enabled = false;
        }

        public unsafe void enable() {
            if (is_enabled)
                throw new Exception($"hook 0x{hook_ptr.ToString("X2")} for method '{original_method.Name}' is already enabled");
            try {
                byte* ptr = (byte*)original_ptr.ToPointer();
                if (IntPtr.Size == 8) {
                    *ptr = 0x48;
                    ptr[1] = 0xB8;
                    *(long*)(ptr + 2) = hook_ptr.ToInt64();
                    ptr[10] = 0xFF;
                    ptr[11] = 0xE0;
                }
                else {
                    *ptr = 0x68;
                    *(int*)(ptr + 1) = hook_ptr.ToInt32();
                    ptr[5] = 0xC3;
                }
            }
            catch (Exception ex) {
                throw new Exception($"failed to enable hook 0x{hook_ptr.ToString("X2")} for method \"{original_method.Name}\"", ex);
            }
            is_enabled = true;
        }

        public unsafe void disable() {
            if (!is_enabled)
                throw new Exception($"hook 0x{hook_ptr.ToString("X2")} for method '{original_method.Name}' is not enabled");
            try {
                byte* ptr = (byte*)backup.ptr.ToPointer();
                *ptr = backup.a;
                ptr[1] = backup.b;
                ptr[10] = backup.c;
                ptr[11] = backup.d;
                ptr[12] = backup.e;
                if (IntPtr.Size == 8) {
                    *(long*)(ptr + 2) = (long)backup.f64;
                }
                else {
                    *(int*)(ptr + 1) = (int)backup.f32;
                    ptr[5] = backup.g;
                }
            }
            catch (Exception ex) {
                throw new Exception($"failed to disable hook 0x{hook_ptr.ToString("X2")} for method \"{original_method.Name}\"", ex);
            }
            is_enabled = false;
        }
    }
}
