
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace W
{
    public enum MapType
    {
        none = 0,
        spaceship,
        planet,
        star_system,
    }

    /// <summary>
    /// todo: remove this
    /// </summary>
    public enum MapSubType
    {
        none = 0,
        clay,
        stone,
        sand,
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class Map : ICreatable
    {
        #region property
        /// <summary>
        /// 玩家进入
        /// </summary>
        public void OnPlayerEnter(Player player) {

            player.position = portal_position;
            player.direction = portal_direction;

            UI.I.CameraPosition = portal_position;
        }
        /// <summary>
        /// 玩家退出
        /// </summary>
        public void OnPlayerExit(Player player) {
            portal_position = player.position;
            portal_direction = player.direction;
        }


        public static void ReRender9At(Vec2 pos) {
            MapView.I?.ReRenderNeighbor9(pos);
        }
        #endregion



        #region initialization or enable
        public void OnCreate() { }

        public static Map Create(string name, MapType type) {
            A.Assert(!GameEntry.I.ExistMap(name));

            Map map = Creator.__CreateData<Map>();
            (Globals.I as __IGlobals__).__Set__Map__(map); // 要么在initialize里绑定G.map
            map.OnCreate(name, type);
            return map;
        }

        [System.Runtime.Serialization.OnSerializing]
        private void OnSerializing(System.Runtime.Serialization.StreamingContext sc) {
        }
        [System.Runtime.Serialization.OnDeserialized]
        private void OnDeserialized(System.Runtime.Serialization.StreamingContext sc) {
            (Globals.I as __IGlobals__).__Set__Map__(this); // 要么在deserialize里绑定G.map
            OnEnable();
        }

        public static uint HashcodeOfMapKey(string s) => H.Hash(s);

        private void OnCreate(string name, MapType type) {
            key = name;
            hashcode = HashcodeOfMapKey(key);
            this.type = type;

            subtype = GetSubtype();

            CreateSizeGroundTile();

            CreateTerrainAndTheme();

            OnEnable();

            AfterCreate();
        }

        private MapSubType GetSubtype() {
            uint hash = hashcode;
            switch (type) {
                case MapType.none:
                    return MapSubType.none;
                case MapType.planet:
                    if (hash % 16 == 0) {
                        return MapSubType.sand;
                    }
                    else if (H.Hashed(ref hash) % 2 == 0) {
                        return MapSubType.stone;
                    }
                    return MapSubType.clay;
                default:
                    return MapSubType.none;
            }
        }

        #endregion



        #region initialization terrain theme

        [JsonProperty] public string key { get; private set; } // 地图名
        [JsonProperty] public uint hashcode { get; private set; } // 地图名 hashcode
        public uint HashcodeOfTile(Vec2 vec) => H.Hash(vec.x, vec.y, size.x, size.y, hashcode);
        [JsonProperty] public MapType type { get; private set; } // 地图枚举类型
        [JsonProperty] public MapSubType subtype { get; private set; } // 地图枚举类型

        [JsonProperty] public Vec2 size { get; private set; } // 地图大小
        [JsonIgnore] public Vec2 Center { get => size / 2; }
        [JsonIgnore] public int Width => size.x;
        [JsonIgnore] public int Height => size.y;

        public const int Margin = 1;
        public bool IsInside(Vec2 pos) => TileUtility.IsInRect(pos, InsideBottomLeft, InsideTopRight);
        public Vec2 InsideBottomLeft => new Vec2(Margin, Margin);
        public Vec2 InsideTopRight => new Vec2(size.x - Margin, size.y - Margin);




        [JsonProperty] public MapTheme theme { get; private set; }
        [JsonProperty] public MapTerrain terrain { get; private set; }
        private void CreateTerrainAndTheme() {
            terrain = MapTerrain.OnCreate(type);

            theme = Creator.__CreateData<MapTheme>();
        }
        #endregion




        #region initialization matrix
        // [JsonProperty] public MapItemMatrix ground { get; private set; }
        [JsonProperty] public MapItemMatrix _tiles { get; private set; }
        public Item this[Vec2 pos] { get => this[pos.x, pos.y]; set => this[pos.x, pos.y] = value; }
        public Item this[int x, int y] {
            get {
                Vec2 pos = new Vec2(x, y);
                Item front = _tiles[x, y];
                if (front is ItemRedirect redirect) {
                    Vec2 p = redirect.RedirectPos;
                    front = _tiles[p.x, p.y];
                }
                return front;
            }
            set => _tiles[x, y] = value;
        }

        public static Vec2 PlanetPosotion;
        [JsonProperty] protected Vec2 planet_position { get; private set; }
        [JsonProperty] private Vec2 portal_position;
        [JsonProperty] private Vec2 portal_direction;

        private void CreateSizeGroundTile() {
            switch (type) {
                case MapType.planet:
                    size = new Vec2(80, 80);
                    break;
                case MapType.spaceship:
                    size = new Vec2(80, 80);
                    break;
                case MapType.star_system:
                    size = new Vec2(50, 50);
                    break;
                default:
                    throw new Exception();
            }

            A.Assert(size.x > 0 && size.x < 200 && size.x % 2 == 0);
            A.Assert(size.y > 0 && size.y < 200 && size.y % 2 == 0);

            _tiles = MapItemMatrix.Create(size);
        }
        #endregion


        #region after enable
        /// <summary>
        /// 内存中创建时
        /// </summary>
        public void OnEnable() {
            A.Assert(hashcode == H.Hash(key));
        }

        public void AfterCreate() {
            theme.AfterCreate();
            terrain.AfterCreate(); // require G.map

            planet_position = PlanetPosotion;

            portal_position = terrain.Portal;
            portal_direction = Vec2.down;
        }

        #endregion




        #region hash




        /// <summary>
        /// 展示目标地面的信息
        /// </summary>
        public void NoticeGrounds(Vec2 pos) {
            bool _ = NoticeGround(terrain.decor[pos]) 
                || NoticeGround(terrain.floor[pos]) 
                || NoticeGround(terrain.bedrock[pos]);
        }
        private static bool NoticeGround(GroundType type) {
            if (type == GroundType.none) return false;
            Item prefeb = FindPrefab(type);
            UI_Notice.I.Send(prefeb.Description, prefeb);
            return true;
        }

        public bool CanPlace(Vec2 pos, Vec2 size) {
            if (!IsInside(pos)) return false;
            if (!IsInside(pos + size)) return false;
            for (int i = 0; i < size.x; i++) {
                for (int j = 0; j < size.y; j++) {
                    if (!CanEnter(pos + new Vec2(i, j))) {
                        return false;
                    }
                }
            }
            return true;
        }

        public bool CanEnter(Vec2 pos) {
            if (!IsInside(pos)) return false;

            Item item = _tiles[pos.x, pos.y];
            if (item != null) return item.CanEnter;

            return _CanEnter(terrain.bedrock[pos])
                && _CanEnter(terrain.floor[pos])
                && _CanEnter(terrain.decor[pos]);
        }
        private static bool _CanEnter(GroundType type) {
            if (type == GroundType.none) return true;
            return FindPrefab(type).CanEnter;
        }
        public static Item FindPrefab(GroundType type) {
            if (type == GroundType.none) return null;
            return prefabs[type];
        }
        [JsonIgnore]
        private static Dictionary<GroundType, Item> prefabs = new Dictionary<GroundType, Item>() {
            { GroundType.star, ItemPool.GetImpl<star_map_>() },

            { GroundType.sea, ItemPool.GetImpl<sea_>() },
            { GroundType.bedrock, ItemPool.GetImpl<bedrock_>() },

            //{ GroundType.sea_sand, ItemPool.GetImpl<sea_sand_>() },
            //{ GroundType.bedrock_sand, ItemPool.GetImpl<bedrock_sand_>() },

            { GroundType.grassland, ItemPool.GetImpl<grassland_>()},

            { GroundType.hill_sand, ItemPool.GetImpl<hill_sand_>()},
            { GroundType.hill_stone, ItemPool.GetImpl<hill_stone_>()},
            { GroundType.hill_clay, ItemPool.GetImpl<hill_clay_>()},

            { GroundType.farmland, ItemPool.GetImpl<farmland_>()},

            { GroundType.metal_wall, ItemPool.GetImpl<metal_wall_>()},
            { GroundType.concrete_wall, ItemPool.GetImpl<concrete_wall_>()},


            { GroundType.stone_brick_wall, ItemPool.GetImpl<stonebrick_wall_>()},
            { GroundType.wood_brick_wall, ItemPool.GetImpl<plank_wall_>()},
            { GroundType.claybaked_brick_wall, ItemPool.GetImpl<brick_wall_>()},

            { GroundType.stone_tile_floor, ItemPool.GetImpl<stone_tile_floor>()},
            { GroundType.wood_tile_floor, ItemPool.GetImpl<wood_tile_floor>()},
            { GroundType.claybaked_tile_floor, ItemPool.GetImpl<claybaked_tile_floor>()},
        };

        /// <summary>
        /// to redo
        /// </summary>
        public void PlayFootstepSoundOn(Vec2 pos) {
            GroundType ground = G.terrain.floor[pos];
            if (ground == GroundType.none) {
                Audio.I.PlayRandomSoundClip(Audio.I.Stone, 0.5f);
            } else if (ground == GroundType.grassland) {
                Audio.I.PlayRandomSoundClip(Audio.I.Grass, 0.4f);
            } else if (ground == GroundType.wood_tile_floor) {
                Audio.I.PlayRandomSoundClip(Audio.I.Wood, 0.6f);
            } else {
                Audio.I.PlayRandomSoundClip(Audio.I.Stone, 0.6f);
            }
        }

        #endregion


        #region effect

        public static void ShakeAt(Vec2 pos) { // 不用arg了
            MapView.I.Bounce(pos,
                0.5f, // time
                0.5f); // translation
        }

        #endregion
    }
}
