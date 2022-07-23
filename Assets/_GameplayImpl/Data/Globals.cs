
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace W
{
    /// <summary>
    /// g for global
    /// </summary>
    public static class G
    {
        public static AdSelection AdSelection;

        public static long now_real => G.universe.current_time_unscaled;
        public static long now => G.universe.current_time;
        public static bool hold_tap => UI_Joystick.I.HoldTap;

        public static bool at_night => Light.I.DayLightnessCosine > 0.125f;

        public static long second => G.now / C.Second;
        public static float secondT => M.Progress(G.now, second * C.Second, C.Second); //  (float)(G.now - second * C.Second) / C.Second;
        public static long minute => G.now / C.Minute;
        public static float minuteT => M.Progress(G.now, minute * C.Minute, C.Minute);

        public static long TimeOf(long now, long delta) => now / delta; // 以delta为单位，从至今的时间
        public static float ProgressOf(long now, long delta) => M.Progress(now, (now / delta) * delta, delta); // 


        public static long frame => Information.I.FrameCount;
        public static uint frame_seed => Information.I.FrameSeed;
        public static float frame_seed_float => Information.I.FrameSeedFloat;

        public static Player player => Globals.I.player;
        public static Item selected { get => player.HandItem; set => player.HandItem = value; }
        public static Map map => Globals.I.map;
        public static MapTerrain terrain => Globals.I.map.terrain;
        public static MapTheme theme => Globals.I.map.theme;
        public static Settings settings => Globals.I.settings;
        public static Universe universe => Globals.I.universe;
        public static Achievement achievement => Globals.I.achievement;
    }




    /// <summary>
    /// used to dosomething after deserialization/enable
    /// </summary>
    public interface __IGlobals__
    {
        void __Set__Map__(Map map);
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class Globals : ICreatable, __IGlobals__
    {
        public const string VERSION = "2077";

        public static Globals I { get; private set; }

        [JsonProperty] public string version { get; private set; }

        [JsonProperty] public Player player { get; private set; }

        [JsonProperty] public Universe universe { get; private set; }

        [JsonProperty] public Settings settings { get; private set; }

        [JsonProperty] public Achievement achievement { get; private set; }

        [JsonProperty] public string map_key; // key
        [JsonProperty] public MapType map_type; // key

        /// <summary>
        /// 存入单独文件
        /// </summary>
        [JsonIgnore]
        public Map map { get; private set; }

        void __IGlobals__.__Set__Map__(Map map) {
            A.Assert(map != null);
            this.map = map;
        }

        /// 首次创建时
        /// </summary>
        void ICreatable.OnCreate() {
            // assert singleton
            A.Assert(I == null);
            I = this;

            version = VERSION;

            settings = Creator.__CreateData<Settings>();
            achievement = Creator.__CreateData<Achievement>();
            universe = Creator.__CreateData<Universe>();

            player = Creator.__CreateData<Player>();
        }

        /// <summary>
        /// 序列化前
        /// </summary>
        [System.Runtime.Serialization.OnSerializing]
        private void OnSerializing(System.Runtime.Serialization.StreamingContext sc) {
            // to data
            A.Assert(VERSION.Equals(version));
        }

        /// <summary>
        /// 序列化后
        /// </summary>
        [System.Runtime.Serialization.OnDeserialized]
        private void OnDeserialized(System.Runtime.Serialization.StreamingContext sc) {
            // from data
            A.Assert(VERSION.Equals(version));

            OnEnable();
        }

        /// <summary>
        /// 内存中创建时
        /// </summary>
        private void OnEnable() {
            A.Assert(I == null || I == this);
            I = this;
        }
    }

}
