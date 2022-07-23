
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{
    [JsonObject(MemberSerialization = MemberSerialization.Fields)]
    public class Idle
    {
        private Idle() {

        }
        public static Idle Create(long value, long max = long.MaxValue, long del = C.Second, long inc = 1) {
            Idle result = new Idle();
            result.time = G.now;
            result.Max = max;
            result.Del = del;
            result.Inc = inc;
            result.val = value; // dont set Value. 
            return result;
        }


        [JsonProperty] private long del = C.Second;
        [JsonProperty] private long inc = 1;
        [JsonProperty] private long val = 0;
        [JsonProperty] private long time = 0;
        [JsonProperty] private long max = long.MaxValue;

        public string DebugMessageOf(long value) => $"set {value} : val({val}) Value({Value}) max({Max}) inc({Inc}) del({Del})  time({time})    {(G.now - time) / Del} ";


        private void Sync() {
            if (del == 0) return;
            long now = G.now;
            long turn = (now - time) / Del;
            val += turn * Inc;
            val = M.Clamp(0, Max, val);
            time += turn * Del;
        }

        public long Max {
            get => max;
            set {
                A.Assert(value >= 0, () => DebugMessageOf(value));
                Sync(); max = value;
            }
        }
        public long Inc {
            get => inc;
            set { Sync(); inc = value; }
        }
        public long Del {
            get => del;
            set { Sync(); del = value; }
        }

        public long Value {
            get {
                long turn = (G.now - time) / Del;
                long result = turn * Inc + val;
                return M.Clamp(0, max, result);
            }
            set {
                A.Assert(value >= 0, () => DebugMessageOf(value));
                if (value >= Max) {
                    val = Max;
                    time = G.now;
                } 
                else {
                    // time += (Value - value) * Del; // largechange: overflow
                    Sync();
                    if (val == Max) {
                        time = G.now;
                    }
                    val = value;
                }
            }
        }
        public bool Empty => Value <= 0;
        public bool Maxed => Value >= Max;

        public void Clear() {
            time = G.now;
            val = 0;
        }

        //public void RandomizeProgress(float x) {
        //    A.Assert(x >= 0 && x <= 1);
        //    time -= (long)(x * Del);
        //}
        public void RandomizeAllProgress(uint hash) {
            Value = hash % (Max + 1); // value
            time -= (long)(H.HashFloat(hash, (uint)Salt.Idle) * Del); // progress
        }

        public long MaxSubValue => Max - Value;

        public float Progress => Value == Max ? 1 : (Value == 0 && Inc == 0) ? 0 : M.Progress(G.now, time, Del) % 1;

        public float TotalProgress { 
            get {
                return Max == 0 ? 0 : M.Clamp01((float)((double)(Value + Progress) / Max));
            }
        }
        public float OneSubTotalProgress => 1 - TotalProgress;
        public float OneSubProgress => 1 - Progress;

        public string ProgressTimeLeftDescription {
            get {
                if (Value >= Max) return "完成";
                long ticks = Del - (G.now - time) % Del;
                return Universe.TimespanDescription(ticks);
            }
        }
    }
}
