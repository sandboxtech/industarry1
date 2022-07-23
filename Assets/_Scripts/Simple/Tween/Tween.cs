
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace W
{
    public class Tween : MonoBehaviour
    {
        public static Tween I { get; private set; }
        private void Awake() {
            A.Assert(I == null);
            I = this;
        }

        public static void Add(long duration, Func<float, bool> cb) => I._Add(duration, cb);

        public void _Add(long duration, Func<float, bool> cb) {

            A.Assert(duration < 10 * C.Second && duration > 0, () => duration.ToString());

            StartCoroutine(Coroutine(G.now, duration, cb));
        }

        IEnumerator Coroutine(long start, long duration, Func<float, bool> cb) {
            float t = 0;
            while (t < 1 && cb(t)) {
                yield return null;
                t = (float)(G.now - start) / duration;
            }
            cb(1);
        }
    }
}
