
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{

//    2 3 5 7 11 13 17 19 23 29
//31 37 41 43 47 53 59 61 67 71
    public static class Prime
    {
        public const int _2 = 2;
        public const int _3 = 3;
        public const int _5 = 5;
        public const int _7 = 7;
        public const int _11 = 11;
        public const int _13 = 13;
        public const int _17 = 17;
        public const int _19 = 19;
        public const int _23 = 23;
        public const int _29 = 29;

        public const int _31 = 31;
        public const int _37 = 37;
        public const int _41 = 41;
        public const int _47 = 47;
        public const int _43 = 43;
        public const int _53 = 53;
        public const int _59 = 59;

        public const int _61 = 61;
        public const int _67 = 67;
        public const int _71 = 71;
        public const int _73 = 73;
        public const int _79 = 79;
        public const int _83 = 83;
        public const int _89 = 89;
        public const int _97 = 97;
    }


    /// <summary>
    /// M for math
    /// </summary>
    public static class M
    {
        public static float Progress(long now, long start, long span) => (float)((double)(now - start) / span);

        public static float Buffer(ref float v, Func<float> f) {
            if (v == 0) v = f();
            if (v == 0) return float.MaxValue;
            return v;
        }
        public static int Buffer(ref int v, Func<int> f) {
            if (v == 0) v = f();
            if (v == 0) return int.MaxValue;
            return v;
        }

        public static int ToPercent(float x) => (int)(x * 100);

        public static long Min(long x, long y) => x < y ? x : y;
        public static int Min(int x, int y) => x < y ? x : y;
        public static float Min(float x, float y) => x < y ? x : y;

        public static long Max(long x, long y) => x > y ? x : y;
        public static int Max(int x, int y) => x > y ? x : y;
        public static float Max(float x, float y) => x > y ? x : y;


        public static int Abs(int x) => x > 0 ? x : -x;
        public static float Abs(float x) => x > 0 ? x : -x;

        public static float PI => Mathf.PI;
        public static float PI2 => Mathf.PI * 2;
        public static float PI_Half => Mathf.PI / 2;
        public static float Cos(float x) => Mathf.Cos(x);
        public static float Sin(float x) => Mathf.Sin(x);

        public static float Pow(float x, float y) => Mathf.Pow(x, y);


        public static float Lerp(float min, float max, float x) => min + x * (max - min); // min - x * min + x * max // min * (1 - x) + max * x
        public static float InverseLerp(float min, float max, float x) => max == min ? 0 : (x - min) / (max - min);
        public static float Remap(float min, float max, float min2, float max2, float x) => Lerp(min, max, InverseLerp(min2, max2, x));

        public static long Clamp(long min, long max, long x) => x < min ? min : (x > max ? max : x);
        public static long Clamp01(long x) => Clamp(0, 1, x);
        public static float Clamp(float min, float max, float x) => x < min ? min : (x > max ? max : x);
        public static float Clamp01(float x) => Clamp(0, 1, x);
        public static int Clamp(int min, int max, int x) => x < min ? min : (x > max ? max : x);
        public static int Clamp01(int x) => Clamp(0, 1, x);

        public static int Floor(float x) => (int)x;

        public static float Square(float x) => x * x;
        public static int Square(int x) => x * x;

        public static float Sqrt(float x) => Mathf.Sqrt(x);

        public static float Div(long x, long y) => y == 0 ? float.MaxValue : (float)((double)x / y);

        public static long FakeCubicRoot(long x) {
            long result = 1;
            result *= (long)Math.Pow(10, x / 3);
            switch (x % 3) {
                case 2:
                    result *= 5;
                    break;
                case 1:
                    result *= 2;
                    break;
                default:
                    break;
            }
            return result;
        }
    }
}
