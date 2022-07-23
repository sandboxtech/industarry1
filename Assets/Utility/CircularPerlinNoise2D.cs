

using System;

namespace W
{
    public class CircularPerlinNoise2D
    {
        private (float, float)[,] vertex_buffer;
        private Vec2 noiseSize;

        private float[,] noise_buffer;
        private Vec2 mapSize;

        public CircularPerlinNoise2D(Vec2 noiseSize, Vec2 mapSize, uint offset) => Initialize(noiseSize, mapSize, offset);


        private void Initialize(Vec2 noiseSize, Vec2 mapSize, uint offset) {
            this.noiseSize = noiseSize;
            this.mapSize = mapSize;

            vertex_buffer = new (float, float)[noiseSize.x, noiseSize.y];
            for (int i = 0; i < noiseSize.x; i++) {
                for (int j = 0; j < noiseSize.y; j++) {
                    // vertex_buffer[i, j] = RandomVec2Simple16(i, j, noiseSize.x, noiseSize.y, offset);
                    vertex_buffer[i, j] = RandomVec2(i, j, noiseSize.x, noiseSize.y, offset);
                }
            }

            noise_buffer = new float[mapSize.x, mapSize.y];
            for (int i = 0; i < mapSize.x; i++) {
                for (int j = 0; j < mapSize.y; j++) {
                    noise_buffer[i, j] = __Evaluate(i, j, mapSize.x, mapSize.y);
                }
            }
        }

        public float On(Vec2 pos) {
            return noise_buffer[pos.x, pos.y];
        }

        private float __Evaluate(int i, int j, int width, int height) {
            return __Evaluate((i + 0.5f) / width, (j + 0.5f) / height);
        }

        private float __Evaluate(float tx, float ty) {
            float fx = tx * noiseSize.x;
            float fy = ty * noiseSize.y;
            int x = (int)fx;
            int y = (int)fy;
            float result = Interpolate(fx % 1, fy % 1,
                GetVector(x, y, vertex_buffer), GetVector(x, y + 1, vertex_buffer),
                GetVector(x + 1, y + 1, vertex_buffer), GetVector(x + 1, y, vertex_buffer)
            );
            return result;
        }
        private (float, float) GetVector(int x, int y, (float, float)[,] vectors) {
            return vectors[x == noiseSize.x ? 0 : x, y == noiseSize.y ? 0 : y];
        }


        private static float Interpolate(float x, float y,
            (float, float) p0, (float, float) p1,
            (float, float) p2, (float, float) p3) {

            if (x < 0 || x >= 1) throw new Exception();
            if (y < 0 || y >= 1) throw new Exception();


            float v0x = x - 0;  // P0点的方向向量 (0, 0)
            float v0y = y - 0;
            float v1x = x - 0;  // P1点的方向向量 (0, 1)
            float v1y = y - 1;
            float v2x = x - 1;  // P2点的方向向量 (1, 1)
            float v2y = y - 1;
            float v3x = x - 1;  // P3点的方向向量 (1, 0)
            float v3y = y - 0;


            float product0 = p0.Item1 * v0x + p0.Item2 * v0y;  // P0点梯度向量和方向向量的点乘
            float product1 = p1.Item1 * v1x + p1.Item2 * v1y;  // P1点梯度向量和方向向量的点乘
            float product2 = p2.Item1 * v2x + p2.Item2 * v2y;  // P2点梯度向量和方向向量的点乘
            float product3 = p3.Item1 * v3x + p3.Item2 * v3y;  // P3点梯度向量和方向向量的点乘

            // P1和P2的插值
            float d0 = x;
            d0 = d0 * d0 * d0 * (d0 * (d0 * 6 - 15) + 10);

            float n0 = product1 * (1.0f - d0) + product2 * d0;
            // P0和P3的插值
            float n1 = product0 * (1.0f - d0) + product3 * d0;

            // P点的插值
            float d1 = y;
            d1 = d1 * d1 * d1 * (d1 * (d1 * 6 - 15) + 10);

            float result = n1 * (1.0f - d1) + n0 * d1;

            return result;
        }


        private static (float, float) RandomVec2(int i, int j, int width, int height, uint offset = 0) {
            if (i < 0) i += width;
            if (j < 0) j += height;
            if (i > width) j -= width;
            if (j > height) j -= height;
            float angle = H.ToFloat(H.Hash(i, j, width, height, offset)) * M.PI * 2;
            return (
                Sqrt2 * M.Cos(angle),
                Sqrt2 * M.Sin(angle)
            );
        }

        private static (float, float) RandomVec2Simple4(int i, int j, int width, int height, uint offset = 0) {
            i = i % width;
            if (i < 0) i += width;
            j = j % height;
            if (j < 0) j += height;
            uint hash = H.Hash(i, j, width, height, offset);
            switch (hash % 4) {
                case 0:
                    return (-1, 1); //  Vector2.left + Vector2.up;
                case 1:
                    return (-1, -1); // Vector2.left + Vector2.down;
                case 2:
                    return (1, 1); //Vector2.right + Vector2.up;
                case 3:
                    return (1, -1); //Vector2.right + Vector2.down;
                default:
                    throw new Exception();
            }
        }

        private const float Sqrt2 = 1.4142135623730950488016f; // 1.4142135623730950488016


        private static (float, float) RandomVec2Simple8(int i, int j, int width, int height, uint offset = 0) {
            i = i % width;
            if (i < 0) i += width;
            j = j % height;
            if (j < 0) j += height;
            uint hash = H.Hash(i, j, width, height, offset);
            switch (hash % 8) {
                case 0:
                    return (-1, 1);
                case 1:
                    return (-1, -1);
                case 2:
                    return (1, 1);
                case 3:
                    return (1, -1);

                case 4:
                    return (Sqrt2, 0);
                case 5:
                    return (Sqrt2, 0);
                case 6:
                    return (0, Sqrt2);
                case 7:
                    return (0, Sqrt2);


                default:
                    throw new Exception();
            }
        }

        private const float Sqrt2MulSinR8PI = 0.5411961f; // Mathf.Sin(Mathf.PI / 8) * Mathf.Sqrt(2)
        private const float Sqrt2MulSin3R8PI = 1.306563f; // Mathf.Sin(3*Mathf.PI / 8) * Mathf.Sqrt(2)

        private static (float, float) RandomVec2Simple16(int i, int j, int width, int height, uint offset = 0) {
            i = i % width;
            if (i < 0) i += width;
            j = j % height;
            if (j < 0) j += height;

            uint hash = H.Hash(i, j, width, height, offset);
            switch (hash % 16) {
                case 0:
                    return (-1, 1);
                case 1:
                    return (-1, -1);
                case 2:
                    return (1, 1);
                case 3:
                    return (1, -1);

                case 4:
                    return (Sqrt2, 0);
                case 5:
                    return (Sqrt2, 0);
                case 6:
                    return (0, Sqrt2);
                case 7:
                    return (0, Sqrt2);

                case 8:
                    return (Sqrt2MulSinR8PI, Sqrt2MulSin3R8PI);
                case 9:
                    return (Sqrt2MulSin3R8PI, Sqrt2MulSinR8PI);
                case 10:
                    return (-Sqrt2MulSinR8PI, Sqrt2MulSin3R8PI);
                case 11:
                    return (-Sqrt2MulSin3R8PI, Sqrt2MulSinR8PI);

                case 12:
                    return (Sqrt2MulSinR8PI, -Sqrt2MulSin3R8PI);
                case 13:
                    return (Sqrt2MulSin3R8PI, -Sqrt2MulSinR8PI);
                case 14:
                    return (-Sqrt2MulSinR8PI, Sqrt2MulSin3R8PI);
                case 15:
                    return (-Sqrt2MulSin3R8PI, -Sqrt2MulSinR8PI);

                default:
                    throw new Exception();
            }
        }

        private static float PerlinNoise(float x, float y, int width, int height, int layer = 0) {
            int p0x = (int)(x); // P0坐标
            int p0y = (int)(y);
            int p1x = p0x;      // P1坐标
            int p1y = p0y + 1;
            int p2x = p0x + 1;  // P2坐标
            int p2y = p0y + 1;
            int p3x = p0x + 1;  // P3坐标
            int p3y = p0y;

            (float, float) g0 = RandomVec2Simple4(p0x, p0y, width, height, (uint)layer);
            float g0x = g0.Item1;  // P0点的梯度
            float g0y = g0.Item2;
            (float, float) g1 = RandomVec2Simple4(p1x, p1y, width, height, (uint)layer);
            float g1x = g1.Item1;  // P1点的梯度
            float g1y = g1.Item2;
            (float, float) g2 = RandomVec2Simple4(p2x, p2y, width, height, (uint)layer);
            float g2x = g2.Item1;  // P2点的梯度
            float g2y = g2.Item2;
            (float, float) g3 = RandomVec2Simple4(p3x, p3y, width, height, (uint)layer);
            float g3x = g3.Item1;  // P3点的梯度
            float g3y = g3.Item2;

            float v0x = x - p0x;  // P0点的方向向量
            float v0y = y - p0y;
            float v1x = x - p1x;  // P1点的方向向量
            float v1y = y - p1y;
            float v2x = x - p2x;  // P2点的方向向量
            float v2y = y - p2y;
            float v3x = x - p3x;  // P3点的方向向量
            float v3y = y - p3y;

            float product0 = g0x * v0x + g0y * v0y;  // P0点梯度向量和方向向量的点乘
            float product1 = g1x * v1x + g1y * v1y;  // P1点梯度向量和方向向量的点乘
            float product2 = g2x * v2x + g2y * v2y;  // P2点梯度向量和方向向量的点乘
            float product3 = g3x * v3x + g3y * v3y;  // P3点梯度向量和方向向量的点乘

            // P1和P2的插值
            float d0 = x - p0x;
            d0 = d0 * d0 * d0 * (d0 * (d0 * 6 - 15) + 10);

            float n0 = product1 * (1.0f - d0) + product2 * d0;
            // P0和P3的插值
            float n1 = product0 * (1.0f - d0) + product3 * d0;

            // P点的插值
            float d1 = y - p0y;
            d1 = d1 * d1 * d1 * (d1 * (d1 * 6 - 15) + 10);

            float result = n1 * (1.0f - d1) + n0 * d1;

            return result;
        }


        public const float F_0 = 0f;
        public const float F_5 = 0.03f;
        public const float F_10 = 0.07f;
        public const float F_15 = 0.11f;
        public const float F_20 = 0.16f;
        public const float F_25 = 0.22f;
        public const float F_30 = 0.26f;
        public const float F_35 = 0.32f;
        public const float F_40 = 0.4f;
        public const float F_45 = 0.5f;
        public const float F_50 = 1f;


    }

}
