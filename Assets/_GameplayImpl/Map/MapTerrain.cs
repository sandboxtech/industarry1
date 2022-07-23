
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace W
{
    public interface InterceptFootstepSound { bool Intercept { get; } }


    public enum GroundType : byte
    {
        none,

        star,

        sea,
        bedrock,

        //sea_sand,
        //bedrock_sand,

        grassland,

        hill_clay,
        hill_sand,
        hill_stone,


        farmland,

        // wall
        wall_min,

        claybaked_brick_wall,
        stone_brick_wall,
        wood_brick_wall,

        concrete_wall,
        metal_wall,
        wall_max,

        // floor
        floor_min,

        claybaked_tile_floor,
        stone_tile_floor,
        wood_tile_floor,

        garden_floor,
        floor_max,

        pipe,
        rail,
        road,
    }
    public static class GroundTypeUtil
    {
        public static bool IsWall(GroundType type) {
            return (byte)type < (byte)GroundType.wall_max && (byte)type > (byte)GroundType.wall_min;
        }
    }



    [JsonObject(MemberSerialization.OptIn)]
    public class MapTerrainLayer : ICreatable
    {

        public void OnCreate() { }

        public static MapTerrainLayer Create(Vec2 size) {
            MapTerrainLayer layer = Creator.__CreateData<MapTerrainLayer>();
            layer.Initialize(size);
            return layer;
        }
        private void Initialize(Vec2 size) {
            this.size = size;
            Layer = new byte[size.x * size.y];
        }

        [JsonProperty] private Vec2 size;
        [JsonIgnore] private byte[] Layer;
        [JsonProperty] private byte[] layer;

        [System.Runtime.Serialization.OnSerializing]
        private void OnSerializing(System.Runtime.Serialization.StreamingContext sc) {
            layer = Compression.Compress(Layer);
        }

        [System.Runtime.Serialization.OnDeserialized]
        private void OnDeserialized(System.Runtime.Serialization.StreamingContext sc) {
            Layer = Compression.Decompress(layer);
        }


        public GroundType this[int i, int j] {
            get {
                return (GroundType)Layer[i + j * size.y];
            }
            set {
                Layer[i + j * size.y] = (byte)value;
            }
        }
        public GroundType this[Vec2 pos] {
            get {
                return (GroundType)Layer[pos.x + pos.y * size.y];
            }
            set {
                Layer[pos.x + pos.y * size.y] = (byte)value;
            }
        }
    }


    [JsonObject(MemberSerialization.OptIn)]
    public partial class MapTerrain : ICreatable
    {

        public void OnCreate() { }

        public static  MapTerrain OnCreate(MapType type) {
            switch(type) {
                case MapType.planet:
                    return Creator.__CreateData<Planet>();
                case MapType.spaceship:
                    return Creator.__CreateData<SpaceStation>();
                case MapType.star_system:
                    return Creator.__CreateData<StarSystem>();
                default:
                    throw new Exception();
            }
        }

        public static bool[,] room;

        [JsonProperty] public MapTerrainLayer bedrock { get; private set; }
        [JsonProperty] public MapTerrainLayer floor { get; private set; }
        [JsonProperty] public MapTerrainLayer decor { get; private set; }





        public static float PlanetSolarPanelEfficiency(uint hashcode) => M.Lerp(0.25f, 3f, H.HashFloat(hashcode, 1));
        public static float PlanetTakeOffCostMultiplier(uint hashcode) => M.Lerp(0.25f, 2f, H.HashFloat(hashcode, 2));


        public static List<Type> SpecialBushes = new List<Type>() {
            typeof(cactus_plant),
            typeof(eggplant_plant),
            typeof(potato_plant),

            typeof(pumpkin_plant),
            typeof(turnip_plant),
        };
        public static Type SpecialBushOf(uint hashcode) {
            //if (G.universe.planet_pos == planet_.HomePlanetPosition) {
            //    return typeof(berry_plant);
            //}
            //if (MapSubType.sand == G.map.subtype) {
            //    return typeof(cactus_plant);
            //}
            return SpecialBushes[(int)(hashcode % SpecialBushes.Count)];
        }

        //public Type SpecialBush { get {
        //        return SpecialBushOf(G.map.hashcode); //  SpecialFloras[(int)(G.map.hashcode % SpecialFloras.Count)];
        //    }
        //}


        public static List<Type> SpecialTrees = new List<Type>() {
            typeof(tree_branch),
            typeof(tree_wood),
            typeof(tree_leaf),
        };
        public static Type SpecialTreeOf(uint hashcode) {
            return SpecialTrees[(int)(hashcode % SpecialTrees.Count)];
        }

        //public Type SpecialTree {
        //    get {
        //        //if (G.universe.planet_pos == planet_.HomePlanetPosition) {
        //        //    return typeof(tree_branch);
        //        //}
        //        //if (MapSubType.sand == G.map.subtype) {
        //        //    return null;
        //        //}
        //        return SpecialTreeOf(G.map.hashcode); //  SpecialFloras[(int)(G.map.hashcode % SpecialFloras.Count)];
        //    }
        //}




        public static List<Type> SpecialMinerals = new List<Type>() {
            // typeof(metal_ore),
            typeof(copper_ore),
            typeof(iron_ore),
            typeof(coal),
        };

        public static long MineralRichnessOf(uint hashcode) => 3 + (G.map.hashcode % 13);
        public static Type SpecialMineralOf(uint hashcode) {
            return SpecialMinerals[(int)(hashcode % SpecialMinerals.Count)];
        }

        public static Type GetBaseMineral() {
            return G.map.subtype == MapSubType.sand ? typeof(sand) : G.map.subtype == MapSubType.stone ? typeof(stone) : typeof(clay);
        }


        public static string Caculate_PlaceName(uint hashcode) {
            if (H.HashedFloat(ref hashcode, 0) < 0.5f) {
                string head = TextSystem.Ins.RandomLineOf("Map_Head", H.Hashed(ref hashcode, 0));
                string tail = TextSystem.Ins.RandomLineOf("Map_Tail", H.Hashed(ref hashcode, 0));
                return $"{head}{tail}";
            } else {
                string head = TextSystem.Ins.RandomLineOf("Map_Tail", H.Hashed(ref hashcode, 0));
                string tail = TextSystem.Ins.RandomLineOf("Map_Tail", H.Hashed(ref hashcode, 0));
                return $"{head}{tail}";
            }
        }



        public virtual bool NoPlayerAvatar => false;

        public virtual void AfterCreate() {
            Vec2 size = G.map.size;

            room = new bool[size.x, size.y];

            bedrock = MapTerrainLayer.Create(size);
            floor = MapTerrainLayer.Create(size);
            decor = MapTerrainLayer.Create(size);

            CreateLayers();

            CalcPortalDistance();
        }


        public const int Margin = 3;
        public virtual Vec2 Portal => G.map.Center;


        protected uint[,] hashcodes { get; set; }
        protected bool[,] cellular_buffer { get; set; }
        protected bool[,] rect { get; set; }
        protected int[,] distance_to_portal { get; set; }

        private void CreateLayers() {

            Map map = G.map;

            Vec2 size = map.size;
            int width = size.x;
            int height = size.y;

            hashcodes = new uint[width, height];
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    hashcodes[i, j] = map.HashcodeOfTile(new Vec2(i, j));
                }
            }

            cellular_buffer = new bool[width, height];
            rect = new bool[width, height];


            for (int i = Margin; i < width - Margin; i++) {
                for (int j = Margin; j < height - Margin; j++) {
                    rect[i, j] = true;
                }
            }
        }

        private Dictionary<GroundType, Item> prefabs;
        public Item FindPrefab(GroundType type) {
            if (type == GroundType.none) return null;
            prefabs.TryGetValue(type, out Item item);
            return item;
        }
        public bool CanEnter(GroundType type) {
            if (type == GroundType.none) return true;
            return FindPrefab(type).CanEnter;
        }

        protected void CalcPortalDistance() {
            Vec2 portal = Portal;
            int width = G.map.Width;
            int height = G.map.Height;
            distance_to_portal = new int[width, height];
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    Vec2 pos = new Vec2(i, j);
                    distance_to_portal[i, j] = Vec2.CornerDistanceBewtween(pos, portal);
                }
            }
        }
    }
}
