

using System;
using System.Collections.Generic;

namespace W
{
    public static class StrPool
    {
        private static Dictionary<Type, string> typename_buffer = new Dictionary<Type, string>();
        public static string GetTypeName(Type type) {
            if (!typename_buffer.TryGetValue(type, out string result)) {
                result = type.Name;
                typename_buffer.Add(type, result);
            }
            return result;
        }


        private static Dictionary<string, Dictionary<int, string>> pool = new Dictionary<string, Dictionary<int, string>>();
        private static Dictionary<string, Dictionary<string, string>> pool2 = new Dictionary<string, Dictionary<string, string>>();

        /// <summary>
        /// 有缓存优化，避免频繁拼接字符串
        /// </summary>
        public static string Concat(string key, int index) {
            if (!pool.TryGetValue(key, out Dictionary<int, string> fullKeys)) {
                fullKeys = new Dictionary<int, string>();
                pool.Add(key, fullKeys);
            }
            if (!fullKeys.TryGetValue(index, out string fullKey)) {
                // fullKey = string.Format(key, index);
                fullKey = $"{key}_{index}";
                fullKeys.Add(index, fullKey);
            }
            return fullKey;
        }
        public static string Format(string key, int index) {
            if (!pool.TryGetValue(key, out Dictionary<int, string> fullKeys)) {
                fullKeys = new Dictionary<int, string>();
                pool.Add(key, fullKeys);
            }
            if (!fullKeys.TryGetValue(index, out string fullKey)) {
                // fullKey = string.Format(key, index);
                fullKey = string.Format(key, index);
                fullKeys.Add(index, fullKey);
            }
            return fullKey;
        }

        /// <summary>
        /// 有缓存优化，避免频繁拼接字符串
        /// </summary>
        public static string Concat(string key, string index) {
            if (!pool2.TryGetValue(key, out Dictionary<string, string> fullKeys)) {
                fullKeys = new Dictionary<string, string>();
                pool2.Add(key, fullKeys);
            }
            if (!fullKeys.TryGetValue(index, out string fullKey)) {
                fullKey = $"{key}{index}";
                fullKeys.Add(index, fullKey);
            }
            return fullKey;
        }
    }
}
