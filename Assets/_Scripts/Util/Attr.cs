
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{
    public static class Attr
    {
        public static T Get<T>(Type type, bool inherit = false) where T : Attribute {

            A.Assert(type != null);

            Type at = typeof(T);

            Attribute[] aa = Attribute.GetCustomAttributes(type, at, inherit);

            for (int i = 0; i < aa.Length; i++) {
                Attribute aaa = aa[i];
                if (aaa is T) {
                    T r = aaa as T;
                    return r;
                }
            }
            return null;
        }



        private static Dictionary<Type, string> cnBuffer = new Dictionary<Type, string>();
        public static string CN<T>() => CN(typeof(T));
        public static string CN(Type type) {
            if (type == null) return null;

            if (cnBuffer.TryGetValue(type, out string result)) {
                return result;
            }

            cnAttribute cn = Get<cnAttribute>(type);
            result = cn == null ? null : cn.content;

            cnBuffer.Add(type, result);
            return result;
        }
    }


}
