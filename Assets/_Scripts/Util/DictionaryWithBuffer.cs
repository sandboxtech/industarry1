
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{
    public class DictionaryWithBuffer<TKey, TValue> where TValue : class
    {
        private Dictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>();

        private Func<TKey, TValue> getter;
        public DictionaryWithBuffer(Func<TKey, TValue> getter) {
            A.Assert(getter != null);
            this.getter = getter;
        }

        public TValue this[TKey type] {
            get {
                if (!dict.TryGetValue(type, out TValue result)) {
                    result = getter(type);
                    dict.Add(type, result);
                }
                return result;
            }
        }
    }
}
