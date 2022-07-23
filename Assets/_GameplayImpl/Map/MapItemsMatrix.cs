
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{


    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class MapItemMatrix : __IItem_Container__, ICreatable
    {
        [JsonProperty] private Vec2 size;

        public const int Margin = 1;
        public bool IsInside(Vec2 pos) => pos.x >= Margin && pos.y >= Margin && pos.x < size.x - Margin && pos.x < size.y - Margin;


        [JsonIgnore] private Item[,] Tiles;
        [JsonProperty] private Dictionary<int, Item> tiles;
        public void OnCreate() {

        }

        private void __OnSerializingTiles() {
            tiles = new Dictionary<int, Item>();
            A.Assert(size.x == Tiles.GetLength(0));
            A.Assert(size.y == Tiles.GetLength(1));
            for (int i = 0; i < size.x; i++) {
                for (int j = 0; j < size.y; j++) {
                    Item item = Tiles[i, j];
                    if (item != null) {
                        tiles.Add(ToInt(new Vec2(i, j)), item);
                    }
                }
            }
        }

        private void __OnDeserializingTiles() {
            A.Assert(Tiles == null);
            Tiles = new Item[size.x, size.y];
            foreach (var pair in tiles) {
                Vec2 pos = ToVec2(pair.Key);
                Tiles[pos.x, pos.y] = pair.Value;
            }
        }

        private int ToInt(Vec2 v) => v.x + v.y * size.x;
        private Vec2 ToVec2(int i) => new Vec2(i % size.x, i / size.x);

        /// <summary>
        /// 序列化前。把紧凑数组变成稀疏数组
        /// </summary>
        [System.Runtime.Serialization.OnSerializing]
        private void OnSerializing(System.Runtime.Serialization.StreamingContext sc) {
            // to data
            __OnSerializingTiles();
        }
        /// <summary>
        /// 序列化后。把稀疏数组变成紧凑数组(多空格)
        /// </summary>
        [System.Runtime.Serialization.OnDeserialized]
        private void OnDeserialized(System.Runtime.Serialization.StreamingContext sc) {
            // from data
            (this as __IItem_Container__).__Rebind_Items__();
            __OnDeserializingTiles();
        }




        public static MapItemMatrix Create(Vec2 size) {
            MapItemMatrix matrix = Creator.__CreateData<MapItemMatrix>();
            matrix.size = size;
            matrix.Tiles = new Item[size.x, size.y];
            return matrix;
        }

        void __IItem_Container__.__Rebind_Items__() {
            foreach (var pair in tiles) {
                Vec2 pos = ToVec2(pair.Key);
                __IItem__ _item = pair.Value;
                _item.__Container__ = this;
                _item.__Pos__ = pos;
            }
        }
        void __IItem_Container__.__Remove_Item__(Item item) {
            __IItem__ _item = item;
            int x = _item.__Pos__.x;
            int y = _item.__Pos__.y;
            A.Assert(_item.__Container__ == this && Tiles[x, y] == item);

            (item as __IItem__).__OnRemoveFromMap__();

            _item.__Container__ = null;
            _item.__Pos__ = new Vec2(-1, -1);
            Tiles[x, y] = null;

            Map.ReRender9At(new Vec2(x, y)); // rerender view
        }

        public Item this[int x, int y] {
            get {
                return Tiles[x, y];
            }
            set {
                Vec2 pos = new Vec2(x, y);
                A.Assert(IsInside(pos), () => $"not inside {pos}");

                Tiles[x, y]?.Detach();

                if (value != null) {

                    value.Detach();

                    __IItem__ _item = value;
                    _item.__Container__ = this;
                    _item.__Pos__ = pos;
                    Tiles[x, y] = value;

                    (value as __IItem__).__OnAddToMap__();
                }

                Map.ReRender9At(pos); // rerender view
            }
        }
    }

}
