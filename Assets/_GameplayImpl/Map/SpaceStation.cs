
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{
    public class SpaceStation : MapTerrain
    {
        private bool[,] metal_wall;
        private bool[,] tile_floor;
        private bool[,] space_station_airlock;

        private const int roomCount = 7;
        private const int roomCountHalf = roomCount / 2;

        private const int roomSize = 9;
        private const int roomSizeHalf = roomSize / 2;
        private const int connectionSize = 3;
        private const int unitSize = roomSize + connectionSize;


        public override Vec2 Portal => FindRoomBottomLeft(roomCountHalf, roomCountHalf) + new Vec2(roomSizeHalf, roomSizeHalf);

        public override void AfterCreate() {
            base.AfterCreate();

            CreateBuffer();
            CreateGround();
            PlaceMisc();
        }
        private void CreateGround() {
            for (int j = 0; j < G.map.Height; j++) {
                for (int i = 0; i < G.map.Width; i++) {
                    if (metal_wall[i, j]) {
                        floor[i, j] = GroundType.metal_wall;
                    } else if (tile_floor[i, j]) {
                        floor[i, j] = GroundType.stone_tile_floor;
                    }
                }
            }
        }
        private void PlaceMisc() {

            // 左买右卖 上飞下存
            // 其他是扩展


            // 上边，放置飞船
            G.map[FindRoomBottomLeft(roomCountHalf, roomCountHalf + 1) + new Vec2(2, 2)] = Item.Of<spaceship1>();

            G.map[FindRoomBottomLeft(roomCountHalf, roomCountHalf + 2) + new Vec2(1, 5)] = Item.Of<spaceship0>();

            // 中间，
            Vec2 centerRoom = FindRoomBottomLeft(roomCountHalf, roomCountHalf);
            G.map[centerRoom + new Vec2(2, 5)] = Item.Of<space_console_qic>();
            // G.map[centerRoom + new Vec2(6, 5)] = Item.Of<spacestation_radar>();
            G.map[centerRoom + new Vec2(5, 5)] = Item.Of<space_console_qic>();
            G.map[centerRoom + new Vec2(2, 2)] = Item.Of<billboard_spacestation>();

            // 左边
            Vec2 leftRoom = FindRoomBottomLeft(roomCountHalf-1, roomCountHalf);
            G.map[leftRoom + new Vec2(2, 2)] = Item.Of<space_console_seller>();
            G.map[leftRoom + new Vec2(2, 5)] = Item.Of<space_console_seller>();
            G.map[leftRoom + new Vec2(5, 2)] = Item.Of<space_console_seller>();
            G.map[leftRoom + new Vec2(5, 5)] = Item.Of<space_console_seller>();

            // 左边
            Vec2 rightRoom = FindRoomBottomLeft(roomCountHalf + 1, roomCountHalf);
            G.map[rightRoom + new Vec2(2, 2)] = Item.Of<space_console_buyer>();
            G.map[rightRoom + new Vec2(2, 5)] = Item.Of<space_console_buyer>();
            G.map[rightRoom + new Vec2(5, 2)] = Item.Of<space_console_buyer>();
            G.map[rightRoom + new Vec2(5, 5)] = Item.Of<space_console_buyer>();

            // 下边
            Vec2 downRoom = FindRoomBottomLeft(roomCountHalf, roomCountHalf - 1);
            G.map[downRoom + new Vec2(2, 2)] = Item.Of<spacestation_engine>();


            //// 闸门
            //for (int j = 0; j < G.map.Height; j++) {
            //    for (int i = 0; i < G.map.Width; i++) {
            //        if (space_station_airlock[i, j]) {
            //            G.map[i, j] = Item.Of<airlock>();
            //        }
            //    }
            //}
        }



        private Vec2 FindRoomBottomLeft(int x, int y)
            => new Vec2((G.map.Width - roomCount * unitSize + connectionSize) / 2 + x * unitSize,
                (G.map.Height - roomCount * unitSize + connectionSize) / 2 + y * unitSize);
        private Vec2 FindRightAirlock(int x, int y) => FindRoomBottomLeft(x, y) + new Vec2(roomSize - 1, roomSizeHalf);
        private Vec2 FindTopAirlock(int x, int y) => FindRoomBottomLeft(x, y) + new Vec2(roomSizeHalf, roomSize - 1);



        private int DistanceToCenterRoom(int i, int j) => M.Abs(i - roomCountHalf) + M.Abs(j - roomCountHalf);
        private bool ValidRoom(int i, int j) => DistanceToCenterRoom(i, j) <= 2;

        private void CreateBuffer() {

            Vec2 size = G.map.size;
            int width = size.x;
            int height = size.y;

            metal_wall = new bool[width, height];
            tile_floor = new bool[width, height];
            space_station_airlock = new bool[width, height];

            A.Assert(unitSize * roomCount + 2 * Margin < width);
            A.Assert(unitSize * roomCount + 2 * Margin < height);

            // 建造房间
            for (int i = 0; i < roomCount; i++) {
                for (int j = 0; j < roomCount; j++) {
                    if (!ValidRoom(i, j)) continue;

                    Vec2 pos = FindRoomBottomLeft(i, j);

                    for (int ii = 0; ii < roomSize; ii++) { // 横向
                        metal_wall[pos.x + ii, pos.y] = true; // 下方
                        metal_wall[pos.x + ii, pos.y + roomSize - 1] = true; // 上方
                    }
                    for (int jj = 1; jj < roomSize - 1; jj++) { // 纵向
                        metal_wall[pos.x, pos.y + jj] = true; // 左方
                        metal_wall[pos.x + roomSize - 1, pos.y + jj] = true; // 右方
                    }

                    for (int ii = 1; ii < roomSize - 1; ii++) {
                        for (int jj = 1; jj < roomSize - 1; jj++) {
                            tile_floor[pos.x + ii, pos.y + jj] = true; // 地板
                        }
                    }
                }
            }

            // 建造走廊
            for (int i = 0; i < roomCount - 1; i++) {
                for (int j = 0; j < roomCount; j++) {
                    if (!ValidRoom(i, j)) continue;

                    if (ValidRoom(i + 1, j)) {
                        Vec2 posRight = FindRightAirlock(i, j);
                        for (int k = 0; k < connectionSize + 2; k++) {
                            tile_floor[posRight.x + k, posRight.y] = true; // 地板

                            metal_wall[posRight.x + k, posRight.y + 1] = true; // 上墙
                            metal_wall[posRight.x + k, posRight.y - 1] = true; // 下墙
                        }
                        if (j == roomCountHalf && (i == roomCountHalf || i == roomCountHalf - 1)) {

                        } else {
                            space_station_airlock[posRight.x, posRight.y] = true;
                            space_station_airlock[posRight.x + connectionSize + 1, posRight.y] = true;
                        }
                        metal_wall[posRight.x, posRight.y] = false;
                        metal_wall[posRight.x + connectionSize + 1, posRight.y] = false;
                    }

                    if (ValidRoom(i, j + 1)) {
                        Vec2 posTop = FindTopAirlock(i, j);
                        for (int k = 0; k < connectionSize + 2; k++) {
                            tile_floor[posTop.x, posTop.y + k] = true; // 地板

                            metal_wall[posTop.x + 1, posTop.y + k] = true;  // 右墙
                            metal_wall[posTop.x - 1, posTop.y + k] = true; // 左墙
                        }
                        if (i == roomCountHalf && (j == roomCountHalf || j == roomCountHalf - 1)) {

                        } else {
                            space_station_airlock[posTop.x, posTop.y] = true;
                            space_station_airlock[posTop.x, posTop.y + connectionSize + 1] = true;
                        }
                        metal_wall[posTop.x, posTop.y] = false;
                        metal_wall[posTop.x, posTop.y + connectionSize + 1] = false;
                    }
                }
            }
        }
    }
}
