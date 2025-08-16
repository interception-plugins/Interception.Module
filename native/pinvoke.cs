using System;
using System.Runtime.InteropServices;

using interception.utils;

namespace interception.native {
    public static class pinvoke {
        [DllImport("kernel32.dll")]
        static extern IntPtr LoadLibrary(string filename);

        [DllImport("kernel32.dll")]
        static extern IntPtr GetProcAddress(IntPtr hModule, string procname);

        [DllImport("kernel32.dll")]
        static extern bool FreeLibrary(IntPtr hModule);

        [DllImport("libdl.so")]
        static extern IntPtr dlopen(string filename, int flags);

        [DllImport("libdl.so")]
        static extern int dlclose(IntPtr handle);

        [DllImport("libdl.so")]
        static extern IntPtr dlsym(IntPtr handle, string symbol);

        public static IntPtr load_library(string filename) {
            if (!os_util.is_unix()) {
                return LoadLibrary(filename);
            }
            else {
                return dlopen(filename, 2);
            }
        }

        public static int free_library(IntPtr handle) {
            if (!os_util.is_unix()) {
                bool result = FreeLibrary(handle);
                return Convert.ToInt32(result);
            }
            else {
                return dlclose(handle);
            }
        }

        public static IntPtr get_proc_addr(IntPtr handle, string funcname) {
            if (!os_util.is_unix())
                return GetProcAddress(handle, funcname);
            else
                return dlsym(handle, funcname);
        }

        public static T make_delegate<T>(IntPtr ptr) {
            return Marshal.GetDelegateForFunctionPointer<T>(ptr);
        }

        public static T make_delegate<T>(string filename, string funcname, out IntPtr loaded_lib_handle) {
            loaded_lib_handle = load_library(filename);
            var fn_ptr = get_proc_addr(loaded_lib_handle, funcname);
            var fn = Marshal.GetDelegateForFunctionPointer<T>(fn_ptr);
            return fn;
        }

        public static int get_last_error() {
            return Marshal.GetLastWin32Error(); // not sure if it works on unix system
        }
    }
}
