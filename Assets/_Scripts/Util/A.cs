
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{
    /// <summary>
    /// A for Assertion
    /// </summary>
    public static class A
    {
        public static void Log(string message) {
            if (Information.I.IsInEditor) {
                Debug.Log(message);
            } else {
                // todo
            }
        }
        public static void Assert(bool b, Func<string> s = null) {
            return;
            // if (!b) Throw(s == null ? null : s.Invoke());
        }
        public static void Throw(string s = null) {
            throw new Exception($"Assertion Failed: “{s}”");
        }
    }

}
