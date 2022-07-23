
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{


    public static class PrefabBuilding
    {
        public static void MakeWoodRoom(Vec2 pos, Vec2 size, Vec2 door_direction) {
            MakeRoom(pos, size, GroundType.wood_tile_floor, GroundType.wood_brick_wall, typeof(wood_door), door_direction);
        }
        public static void MakeStoneRoom(Vec2 pos, Vec2 size, Vec2 door_direction) {
            MakeRoom(pos, size, GroundType.stone_tile_floor, GroundType.stone_brick_wall, typeof(stone_door), door_direction);
        }
        public static void MakeBrickRoom(Vec2 pos, Vec2 size, Vec2 door_direction) {
            MakeRoom(pos, size, GroundType.claybaked_tile_floor, GroundType.claybaked_brick_wall, typeof(wood_door), door_direction);
        }

        public static void MakeRoom(Vec2 pos, Vec2 size, GroundType floor, GroundType wall, Type door, Vec2 door_direction) {
            // 初始地点
            MakeFloorWall(pos, size, floor, wall);
            Vec2 door_pos;
            if (door_direction.x == 0) {
                door_pos = door_direction.y > 0 ? DoorUp(pos, size) : DoorDown(pos, size);
            } else {
                door_pos = door_direction.x > 0 ? DoorRight(pos, size) : DoorLeft(pos, size);
            }
            MakeFloorDoor(door_pos, floor, door);
        }
        public static Vec2 DoorDown(Vec2 pos, Vec2 size) {
            return pos + new Vec2(size.x / 2, 0);
        }
        public static Vec2 DoorUp(Vec2 pos, Vec2 size) {
            return pos + new Vec2(size.x / 2, size.y - 1);
        }
        public static Vec2 DoorLeft(Vec2 pos, Vec2 size) {
            return pos + new Vec2(0, size.y / 2);
        }
        public static Vec2 DoorRight(Vec2 pos, Vec2 size) {
            return pos + new Vec2(size.x - 1, size.y / 2);
        }

        private static void MakeFloorDoor(Vec2 pos, GroundType floor, Type door) {
            G.terrain.floor[pos] = floor;
            G.map[pos] = Item.Of(door);
        }

        private static void MakeFloorWall(Vec2 pos, Vec2 size, GroundType floor, GroundType wall) {
            for (int i = 0; i < size.x; i++) {
                for (int j = 0; j < size.y; j++) {
                    Vec2 p = new Vec2(pos.x + i, pos.y + j);
                    MapTerrain.room[p.x, p.y] = true;

                    G.terrain.bedrock[p] = GroundType.bedrock;
                    if ((i == 0 || i == size.x - 1 || j == 0 || j == size.y - 1) && G.map[p] == null) {
                        G.terrain.floor[p] = wall; // 墙壁
                    } else {
                        G.terrain.floor[p] = floor; // 地板
                    }
                }
            }
        }
    }

    /// <summary>
    /// 为什么一个门代码这么多
    /// </summary>
    public abstract class door : Item
    {

        public override bool Destructable => true;
        public override void Destruct() {
            IdleData.Value = Close;
            base.Destruct();
        }

        protected enum DoorSound
        {
            None,
            Electric,
            Metal, Wood, Stone, Sand,
        }
        private string CloseDoorSound(DoorSound sound) {
            switch (sound) {
                case DoorSound.Metal: return "close_door_metal";
                case DoorSound.Wood: return "close_door_wood";
                case DoorSound.Stone: return "close_door_stone";
                case DoorSound.Sand: return "close_door_sand";
                case DoorSound.Electric: return "close_door_electric";
                default: return null;
            }
        }
        private string OpenDoorSound(DoorSound sound) {
            switch (sound) {
                case DoorSound.Metal: return "open_door_metal";
                case DoorSound.Wood: return "open_door_wood";
                case DoorSound.Stone: return "open_door_stone";
                case DoorSound.Sand: return "open_door_sand";
                case DoorSound.Electric: return "open_door_electric";
                default: return null;
            }
        }

        private const int Close = 1; // 0 for Open
        private const int Open = 0;
        public override void OnCreate() {
            base.OnCreate();
            lastID = Close;
            IdleData = Idle.Create(Close, Close, 180 * C.Second, Close);
        }


        private int lastID;
        protected int ID => lastID;

        protected virtual DoorSound Sound => DoorSound.Electric;
        private int sameID(ref int lastID, int thisID) {
            if (thisID != lastID && OnMap && !Information.MapEnteringFrame) {
                if (thisID == Close) {
                    Audio.I.PlaySoundClip(CloseDoorSound(Sound));
                } else {
                    Audio.I.PlaySoundClip(OpenDoorSound(Sound));
                }
                Particle.CreateDust(_Pos, ContentColor);
            }
            lastID = thisID;
            return thisID;
        }

        public override bool CanTap => true;
        public override void OnTap() {
            if (G.hold_tap && G.player.hand.TryAbsorbOrAdd(this)) {

            }
            else {
                if (IdleData.Value == Open) IdleData.Value = Close;
                else IdleData.Value = Open;
            }
            ReRender();
        }


        protected virtual int TheIndex => _Pos == G.player.position ? lastID : sameID(ref lastID, (int)IdleData.Value);
        public override string Content {
            get {
                if (OnMap && G.map.IsInside(_Pos + Vec2.up) && GroundTypeUtil.IsWall(G.terrain.floor[_Pos + Vec2.up])) {
                    return TheIndex == Open ? $"door_open_horizontal" : $"door_close_horizontal";
                }
                return TheIndex == Open ? $"door_open" : $"door_close";
            }
        }
        // private bool _Normal => !OnMap || Normal;
        // protected virtual bool Normal => G.map.IsInside(_Pos + Vec2.up) && G.terrain.decor[_Pos + Vec2.up] == GroundType.None;

        protected virtual float DoorOpenTime => 0.25f;
        public override bool ShakeOnEnter => false;
        public override bool TryPickOnEnter => false;
        public override void TryEnter(Vec2 pos) {
            base.TryEnter(pos);
            if (lastID == Close) {
                G.player.TryInteract(DoorOpenTime, () => {
                    IdleData.Clear();
                    int _ = TheIndex; // refresh index
                });
            }
        }
        public override bool CanEnter => lastID == Open;
    }



    [impl(typeof(electric_door_))]
    [cn("电动门")] public interface electric_door { }
    public class electric_door_ : door
    {
        public override Color4 ContentColor => G.theme.MetalColor;
        protected override DoorSound Sound => DoorSound.Electric;
    }

    [impl(typeof(wood_door_))]

    [recipe(typeof(wood_tile), 1, false, typeof(wood_door), 1)]
    [cn("木门")] public interface wood_door { }
    public class wood_door_ : door
    {
        public override Color4 ContentColor => G.theme.WoodColor;
        protected override DoorSound Sound => DoorSound.Wood;
    }

    [impl(typeof(stone_door_))]
    [recipe(typeof(stone_tile), 1, false, typeof(stone_door), 1)]
    [cn("石门")] public interface stone_door { }
    public class stone_door_ : door
    {
        public override Color4 ContentColor => G.theme.StoneColor;
        protected override DoorSound Sound => DoorSound.Stone;
    }

    [impl(typeof(metal_door_))]
    [cn("金属门")] public interface metal_door { }
    public class metal_door_ : door
    {
        public override Color4 ContentColor => G.theme.MetalColor;
        protected override DoorSound Sound => DoorSound.Metal;
    }

    [impl(typeof(iron_door_))]
    [recipe(typeof(iron), 1, false, typeof(iron_door), 1)]
    [cn("铁门")] public interface iron_door { }
    public class iron_door_ : door
    {
        public override Color4 ContentColor => G.theme.MetalColor;
        protected override DoorSound Sound => DoorSound.Metal;
    }
}
