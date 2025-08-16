using System;
using System.Reflection;

namespace interception.extensions {
    public static class Type_ex {
        public static object get_field_value(this Type t, string field_name, object obj = null, BindingFlags? flags = null) {
            return t.GetField(field_name, flags == null ? BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic : (BindingFlags)flags).GetValue(obj);
        }

        public static void set_field_value(this Type t, string field_name, object val = null, object obj = null, BindingFlags? flags = null) {
            t.GetField(field_name, flags == null ? BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic : (BindingFlags)flags).SetValue(obj, val);
        }

        public static object get_property_value(this Type t, string prop_name, object obj = null, BindingFlags ? flags = null) {
            return t.GetProperty(prop_name, flags == null ? BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic : (BindingFlags)flags).GetValue(obj);
        }

        public static void set_property_value(this Type t, string prop_name, object val = null, object obj = null, BindingFlags? flags = null) {
            t.GetProperty(prop_name, flags == null ? BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic : (BindingFlags)flags).SetValue(obj, val);
        }

        public static void invoke_method(this Type t, string method_name, BindingFlags? flags = null, object obj = null, params object[] args) {
            t.GetMethod(method_name, flags == null ? BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic : (BindingFlags)flags).Invoke(obj, args);
        }
    }
}
