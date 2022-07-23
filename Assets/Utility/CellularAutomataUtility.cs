
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{
    public static class CellularAutomataUtility
    {
        public static void Randomize(int seed, int[,] map) {
            System.Random random = new System.Random(seed);

            int width = map.GetLength(0);
            int height = map.GetLength(1);
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    map[i, j] = random.Next(0, 2);
                }
            }
        }

        public static void GameOfTerrain_IterateBackAndForth(bool[,] source, bool[,] buffer, int width, int height) {
            Interate(source, buffer, width, height);
            Interate(buffer, buffer, width, height);
        }
        public static void Interate(bool[,] source, bool[,] target, int width, int height) {
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    int count = NeighborCount(i, j, source, width, height);
                    target[i, j] = GameOfTerrain(source[i, j], count);
                }
            }
        }
        private static bool GameOfLife(bool source, int count) {
            if (count < 2 || count > 3) {
                return false;
            } else if (count == 3) {
                return true;
            } else {
                return source;
            }
        }
        private static bool GameOfTerrain(bool source, int count) {
            if (count < 2) {
                return false;
            } else if (count > 5) {
                return true;
            } else {
                return source;
            }
        }


        private static int NeighborCount(int i, int j, bool[,] map, int width, int height) {
            int result = 0;
            Vec2 pos = new Vec2(i, j);
            result += Count(Clamp(pos + Vec2.left, width, height), map);
            result += Count(Clamp(pos + Vec2.right, width, height), map);
            result += Count(Clamp(pos + Vec2.up, width, height), map);
            result += Count(Clamp(pos + Vec2.down, width, height), map);
            result += Count(Clamp(pos + Vec2.left + Vec2.up, width, height), map);
            result += Count(Clamp(pos + Vec2.left + Vec2.down, width, height), map);
            result += Count(Clamp(pos + Vec2.right + Vec2.up, width, height), map);
            result += Count(Clamp(pos + Vec2.right + Vec2.down, width, height), map);
            return result;
        }
        private static int Count(Vector2Int pos, bool[,] map) {
            return map[pos.x, pos.y] ? 1 : 0;
        }

        private static Vector2Int Repeat(Vector2Int pos, int width, int height) {
            Vector2Int result = pos;
            if (result.x < 0) result.x += width;
            if (result.y < 0) result.y += height;
            if (result.x >= width) result.x -= width;
            if (result.y >= height) result.y -= height;
            return result;
        }

        private static Vector2Int Clamp(Vector2Int pos, int width, int height) {
            Vector2Int result = pos;
            if (result.x < 0) result.x = 0;
            if (result.y < 0) result.y = 0;
            if (result.x >= width) result.x = width - 1;
            if (result.y >= height) result.y = height - 1;
            return result;
        }
    }
}

