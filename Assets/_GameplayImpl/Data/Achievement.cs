
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Achievement : ICreatable
    {
        [JsonProperty] public int step_count_in_life { get; set; } // 一辈子走过的总步数
        [JsonProperty] public int food_consumed_count_in_life { get; set; } // 一辈子完成过的交互
        [JsonProperty] public int drink_consumed_count_in_life { get; set; } // 一辈子完成过的交互
        [JsonProperty] public int interaction_completion_count_in_life { get; set; } // 一辈子完成过的交互

        [JsonProperty] public int planet_visited_count { get; set; } // 一辈子访问过的星球数目

        [JsonProperty] public int transaction_count { get; set; }

        public void OnCreate() {
        }


        //[System.Runtime.Serialization.OnSerializing]
        //private void OnSerializing(System.Runtime.Serialization.StreamingContext sc) {
        //}
        //[System.Runtime.Serialization.OnDeserialized]
        //private void OnDeserialized(System.Runtime.Serialization.StreamingContext sc) {
        //}
    }
}
