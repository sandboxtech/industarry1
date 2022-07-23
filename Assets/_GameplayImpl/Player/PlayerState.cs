
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{
    public enum PlayerStateType
    {
        idle = 0,
        walking,
        eating,
        interacting,

    }
    public class PlayerState : ICreatable
    {
        public void OnCreate() {
            timespan_walking_through_tile = C.Second / 4;
            timespan_eating = C.Second / 4;
            timespan_interacting = C.Second / 2;
        }


        #region state

        [JsonProperty] private PlayerStateType last_type; // 上次状态
        [JsonProperty] private PlayerStateType type; // 当前状态
        [JsonIgnore]
        public PlayerStateType Type {
            get {
                return type;
            }
            set {
                if (type == value) return;
                last_type = type;
                type = value;
                ExitState();
                EnterState();
            }
        }

        // for general state
        [JsonProperty] public long timestart { get; set; } // 行动开始时间戳
        [JsonProperty] public long timespan { get; private set; } // 行动时长

        [JsonIgnore] public bool Finished => G.now >= timestart + timespan;
        [JsonIgnore] public float ActionProgress => M.Progress(G.now, timestart, timespan);

        private void DoActionForTicks(long ticks = C.Second) {
            timestart = G.now;
            timespan = ticks;
        }
        public void Finish() {
            timespan = 0;
        }

        #endregion





        #region transition
        /// <summary>
        /// 定义进入状态行为
        /// </summary>
        private void EnterState() {
            switch (type) {
                case PlayerStateType.walking:
                    timestamp_walk_start = G.now;
                    break;
                case PlayerStateType.interacting:
                    break;
                case PlayerStateType.eating:
                    eating_slot = G.player.HandIndex;
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 定义退出状态行为
        /// </summary>
        private void ExitState() {
            switch (last_type) {
                case PlayerStateType.walking:
                    timestamp_walk_start = 0;
                    step_count_since_walk_start = 0;
                    break;
                case PlayerStateType.interacting:
                    break;
                case PlayerStateType.eating:
                    eating_slot = -1;
                    break;
                default:
                    break;
            }
        }
        #endregion





        #region step
        // for walking state
        [JsonProperty] public long timespan_walking_through_tile { get; set; } // 走一格所需时间
        [JsonProperty] public long timestamp_walk_start { get; set; }  // 为走路动画服务

        [JsonIgnore] public float step_float_since_walk_start => M.Progress(G.now, timestamp_walk_start, timespan_walking_through_tile);  // 这次行走走过的总路程(float)
        [JsonProperty] public int step_count_since_walk_start { get; set; } // 


        /// <summary>
        /// 走一步，再走一步
        /// </summary>
        public void TakeAStep() {
            bool isHungery = G.player.attr.IsHungry;
            long hungeryFactor = isHungery ? 2 : 1;

            DoActionForTicks(timespan_walking_through_tile * hungeryFactor);
            Type = PlayerStateType.walking;

            step_count_since_walk_start++;
            G.achievement.step_count_in_life++;
        }
        #endregion







        #region misc actions

        [JsonProperty] public long timespan_interacting { get; set; }
        [JsonProperty] public long timespan_eating { get; set; }


        public void TryEat(float timeScale = 1) {
            Type = PlayerStateType.eating;
            DoActionForTicks((long)(timespan_eating * timeScale));
        }

        [JsonProperty] public int eating_count { get; set; }
        [JsonProperty] public int eating_slot { get; set; }
        public void TryIteract(float timeScale) {
            Type = PlayerStateType.interacting;
            DoActionForTicks((long)(timespan_interacting * timeScale));
        }

        #endregion


    }
}
