using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace interception.hooks {
    public static class hook_manager {
        internal static readonly Dictionary<IntPtr, hook> hooks = new Dictionary<IntPtr, hook>();

        internal static void unhook_all() {
            var len = hooks.Count;
            for (int i = 0; i < len; i++) {
                var hk = hooks.ElementAt(i).Value;
                if (hk.is_enabled) 
                    hk.disable();
            }
            hooks.Clear();
        }

        public static void create_hook(MethodInfo method, MethodInfo hook_callback) {
            var original_handle = method.MethodHandle;
            var hook_handle = hook_callback.MethodHandle;
            RuntimeHelpers.PrepareMethod(original_handle);
            RuntimeHelpers.PrepareMethod(hook_handle);
            
            if (hooks.ContainsKey(hook_handle.GetFunctionPointer()))
                throw new ArgumentException($"method \"{hook_callback.Name}\" is already a hook");
            hooks.Add(hook_handle.GetFunctionPointer(), new hook(method, original_handle.GetFunctionPointer(), hook_handle.GetFunctionPointer()));
        }

        public static void create_hook<T>(T method, T hook_callback) where T : Delegate {
            create_hook(method.GetMethodInfo(), hook_callback.GetMethodInfo());
        }

        public static void create_hook<T>(Type type, string method_name, BindingFlags? flags, T hook_callback, int method_index = 0) where T : Delegate {
            var methods = type.GetMethods(flags == null ? BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance : (BindingFlags)flags);
            var len = methods.Length;
            int found = -1;
            for (int i = 0; i < len; i++) {
                if (method_name == methods[i].Name) {
                    found++;
                    if (method_index == found) {
                        create_hook(methods[i], hook_callback.GetMethodInfo());
                        return;
                    }
                }
            }
            throw new Exception($"method \"{method_name}\" was not found in type \"{type.FullName}\" (index == {method_index})");
        }

        public static void enable_hook<T>(T hook_callback) where T : Delegate {
            if (!hooks.ContainsKey(hook_callback.GetMethodInfo().MethodHandle.GetFunctionPointer()))
                throw new ArgumentException($"method \"{hook_callback.GetMethodInfo().Name}\" is not a hook");
            if (hooks[hook_callback.GetMethodInfo().MethodHandle.GetFunctionPointer()].is_enabled)
                throw new Exception($"hook '{hook_callback.GetMethodInfo().Name}' is already enabled");
            hooks[hook_callback.GetMethodInfo().MethodHandle.GetFunctionPointer()].enable();
        }

        public static void disable_hook<T>(T hook_callback) where T : Delegate {
            if (!hooks.ContainsKey(hook_callback.GetMethodInfo().MethodHandle.GetFunctionPointer()))
                throw new ArgumentException($"method \"{hook_callback.GetMethodInfo().Name}\" is not a hook");
            if (!hooks[hook_callback.GetMethodInfo().MethodHandle.GetFunctionPointer()].is_enabled)
                throw new Exception($"hook '{hook_callback.GetMethodInfo().Name}' is not enabled");
            hooks[hook_callback.GetMethodInfo().MethodHandle.GetFunctionPointer()].disable();
        }

        public static void remove_hook<T>(T hook_callback) where T : Delegate {
            if (!hooks.ContainsKey(hook_callback.GetMethodInfo().MethodHandle.GetFunctionPointer()))
                throw new ArgumentException($"method \"{hook_callback.GetMethodInfo().Name}\" is not a hook");
            if (hooks[hook_callback.GetMethodInfo().MethodHandle.GetFunctionPointer()].is_enabled)
                hooks[hook_callback.GetMethodInfo().MethodHandle.GetFunctionPointer()].disable();
            hooks.Remove(hook_callback.GetMethodInfo().MethodHandle.GetFunctionPointer());
        }

        public static void call_original(object instance, params object[] args) {
            StackTrace trace = new StackTrace(false);
            if (trace.FrameCount < 2) 
                throw new Exception("invalid traceback to the original method");
            MethodBase hook_callback = trace.GetFrame(1).GetMethod();
            if (!hooks.ContainsKey(hook_callback.MethodHandle.GetFunctionPointer()))
                throw new Exception("this method can only be called from hook");
            if (!hooks[hook_callback.MethodHandle.GetFunctionPointer()].is_enabled)
                throw new Exception($"hook '{hook_callback.Name}' is not enabled");
            hooks[hook_callback.MethodHandle.GetFunctionPointer()].disable();
            hooks[hook_callback.MethodHandle.GetFunctionPointer()].original_method.Invoke(instance, args);
            hooks[hook_callback.MethodHandle.GetFunctionPointer()].enable();
        }

        public static unsafe bool is_method_hooked(MethodInfo method) {
            var func_ptr = method.MethodHandle.GetFunctionPointer();
            try {
                byte* ptr = (byte*)func_ptr.ToPointer();
                if (IntPtr.Size == 8) {
                    return (*ptr == 0x48 && ptr[1] == 0xB8 && ptr[10] == 0xFF && ptr[11] == 0xE0);
                }
                else {
                    return (*ptr == 0x68 && ptr[5] == 0xC3);
                }
            }
            catch (Exception ex) {
                throw new Exception($"failed to check if method \"{method.Name}\" was hooked", ex);
            }
        }

        public static unsafe bool is_method_hooked<T>(T method) where T : Delegate {
            return is_method_hooked(method.GetMethodInfo());
        }
    }
}
