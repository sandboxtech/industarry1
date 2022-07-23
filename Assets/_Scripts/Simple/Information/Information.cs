
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{

    public class Information : MonoBehaviour
    {
        public static Information I { get; private set; }

        private void Awake() {
            A.Assert(I == null);
            I = this;

            AwakePlatform();
        }

        private void Start() {
            StartTimestamp();
        }

        public static bool MapEnteringFrame = false;
        private void Update() {
            MapEnteringFrame = false;

            // Timestamp = DateTime.UtcNow.Ticks;
            UpdateUniverseTime();

            FrameSeed = H.Hash((uint)G.now);
            FrameSeedFloat = H.HashFloat(FrameSeed, (uint)Salt.Frame);
            FrameCount++;
        }

        /// <summary>
        /// 平台判断
        /// </summary>
        public bool IsInStandalone { get; private set; }
        public bool IsInEditor { get; private set; }
        private void AwakePlatform() {
            IsInStandalone = !Application.isMobilePlatform;
            IsInEditor = Application.isEditor;

            if (IsInStandalone) Application.targetFrameRate = 120;
            else Application.targetFrameRate = 60;
        }


        private void UpdateUniverseTime() {
            long now = DateTime.UtcNow.Ticks;
            long dt = now - G.universe.current_time_unscaled;

            // G.universe.time_scaler = M.Sin(Time.time) + 1; // 变速测试

            long bonus = M.Min((long)(dt * G.universe.bonus_scale), G.universe.bonus_ticks);
            G.universe.bonus_ticks -= bonus;

            G.universe.current_time += dt + bonus;
            G.universe.current_time_unscaled = now;
        }
        public long Now => DateTime.UtcNow.Ticks;


        /// <summary>
        /// 当前时间戳
        /// </summary>
        // public long Timestamp { get; private set; }
        public uint FrameSeed { get; private set; }
        public float FrameSeedFloat { get; private set; }
        public long FrameCount { get; private set; } = 0;
        /// <summary>
        /// 帧缓存，若时间戳等于现在，则用旧值。若不等，则计算新内容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lastValue"></param>
        /// <param name="lastFrame"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static T FrameBuffer<T>(ref T lastValue, ref long lastFrame, Func<T> func) {
            if (lastFrame != I.FrameCount) {
                lastValue = func();
                lastFrame = I.FrameCount;
            }
            return lastValue;
        }

        private void StartTimestamp() {
            // Timestamp = DateTime.UtcNow.Ticks; // + extra time
            FrameSeed = 0;
            FrameCount = 0;
        }

        public T RandomOne<T>(T[] list) {
            return list[FrameSeed % list.Length];
        }
        public T RandomOne<T>(List<T> list) {
            return list[(int)(FrameSeed % list.Count)];
        }
    }
}
