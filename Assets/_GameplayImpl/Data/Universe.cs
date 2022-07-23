
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{
    // 本超星系团
    // 弹弓效应
    // x 无工质，看起来动量不守恒
    [JsonObject(MemberSerialization.OptIn)]
    public class Universe : ICreatable
    {

        [JsonProperty] public long biology_life;
        [JsonProperty] public long universe_start;
        [JsonProperty] public long universe_now;
        [JsonProperty] public Vec2 planet_pos;

        public static float NoiseOver(uint salt, long delta, long now) => ValueNoise.Get(salt, (uint)G.TimeOf(now, delta), G.ProgressOf(now, delta));


        private float anim0;
        private long anim1;
        public float Anim(long now) => Information.FrameBuffer(ref anim0, ref anim1, () => NoiseOver((uint)Salt.Anim, C.Second, now));
        public float Anim() => Anim(G.now);

        /// <summary>
        /// 时运
        /// </summary>
        public float Luck(long now) => NoiseOver((uint)Salt.Luck, C.Hour, now);
        public float Luck() => Anim(G.now);



        public const long plunk_time = 1;


        public const long endTime = 100000000000_000_000_0; // 100000000000

        public static string TimespanDescription(long t) {
            if (t <= 0) {
                return "完成";
            } else if (t < C.Second) {
                return $"{t / C.MilliSecond,3}毫秒";
            } else if (t < C.Minute) {
                return $"{t / C.Second,3}秒";
            } else if (t < C.Hour) {
                long min = t / C.Minute;
                return $"{min,3}分{(t - min * C.Minute) / C.Second,3}秒";
            } else if (t < C.Day) {
                long hour = t / C.Hour;
                return $"{hour,3}时{(t - hour * C.Hour) / C.Minute,3}分";
            } else if (t < C.Year) {
                long day = t / C.Day;
                return $"{day,3}天{(t - day * C.Day) / C.Hour,3}时";
            } else {
                long year = t / C.Year;
                return $"{year,4}年{(t - year * C.Year) / C.Day,4}天";
            }
        }

        public void OnCreate() {
            current_time_unscaled = Information.I.Now;
            current_time = Information.I.Now;

            creation_time = current_time;
        }

        [JsonProperty] public long creation_time;

        [JsonProperty] public long current_time_unscaled;
        [JsonProperty] public long current_time;
        [JsonProperty] public long bonus_ticks = 0;
        [JsonProperty] public float bonus_scale = 1f;
    }
}
