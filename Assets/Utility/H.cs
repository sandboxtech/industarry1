﻿
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{
    public static class H
    {

        public static uint Hash(string a) {
            uint result = 7;
            foreach (char c in a) {
                result += c;
                result = Hash(result);
            }
            return result;
        }

        /// 4-byte Integer Hashing

        /// The hashes on this page (with the possible exception of HashMap.java's) are all public domain. 
        /// So are the ones on Thomas Wang's page. Thomas recommends citing the author and page when using them.
        /// http://www.burtleburtle.net/bob/hash/integer.html

        /// Thomas Wang has an integer hash using multiplication that's faster than any of mine on my Core 2 duo using gcc -O3, 
        /// and it passes my favorite sanity tests well. 
        /// I've had reports it doesn't do well with integer sequences with a multiple of 34.
        public static uint Hash(uint a) {
            a = (a ^ 61) ^ (a >> 16);
            a = a + (a << 3);
            a = a ^ (a >> 4);
            a = a * 0x27d4eb2d;
            a = a ^ (a >> 15);
            return a;
        }

        public static uint Hash(uint a, uint salt) {
            return Hash(a + salt);
        }


        public static int Mod(uint hashcode, int divider) {
            if (divider <= 0) throw new Exception();
            int result = (int)hashcode % divider;
            if (result < 0) result += divider;
            return result;
        }


        public static float HashedLerp(float start, float end, ref uint x, uint salt = 0) {
            return start + (end - start) * HashedFloat(ref x, salt);
        }


        public static float ToFloat(uint hash) {
            return (float)hash / uint.MaxValue;
        }
        public static float HashFloat(uint a, uint salt) {
            return ToFloat(Hash(a, salt));
        }
        /// <summary>
        /// in-place hash
        /// </summary>
        public static uint Hashed(ref uint x, uint salt = 0) {
            x = Hash(x + salt);
            return x;
        }
        public static float HashedFloat(ref uint x, uint salt = 0) {
            return ToFloat(Hashed(ref x, salt));
        }


        public static uint Hash(int i, int j, int width, int height, uint offset = 0) {
            return unchecked(Hash((uint)(offset * width * height + i + j * width)));
        }


        //public static float SimpleValueNoise(float x) {
        //    float x0 = (float)(Hash((uint)(x))) / uint.MaxValue;
        //    float x1 = (float)(Hash((uint)(x + 1))) / uint.MaxValue;
        //    float result = M.Lerp(x0, x1, EaseFuncUtility.EaseInOutCubic(x - (uint)x));
        //    return result;
        //}
        //public static double SimpleValueNoise(double x) {
        //    double x0 = (double)(Hash((uint)(x))) / uint.MaxValue;
        //    double x1 = (double)(Hash((uint)(x + 1))) / uint.MaxValue;
        //    double t = EaseFuncUtility.EaseInOutCubic(x - (uint)x);
        //    double result = x0 + (x1 - x0) * t;
        //    // Debug.LogWarning($"{x} @ {x0} @ {x1} @ {t} @ {result}");
        //    return result;
        //}



        /// <summary>
        /// Thomas has 64-bit integer hashes too. I don't have any of those yet.
        /// Here's a way to do it in 6 shifts:
        /// </summary>
        public static uint Hash0(uint a) {
            a = (a + 0x7ed55d16) + (a << 12);
            a = (a ^ 0xc761c23c) ^ (a >> 19);
            a = (a + 0x165667b1) + (a << 5);
            a = (a + 0xd3a2646c) ^ (a << 9);
            a = (a + 0xfd7046c5) + (a << 3);
            a = (a ^ 0xb55a4f09) ^ (a >> 16);
            return a;
        }

        /// <summary>
        /// Or 7 shifts, if you don't like adding those big magic constants:
        /// </summary>
        public static uint Hash1(uint a) {
            a -= (a << 6);
            a ^= (a >> 17);
            a -= (a << 9);
            a ^= (a << 4);
            a -= (a << 3);
            a ^= (a << 10);
            a ^= (a >> 15);
            return a;
        }

        /// <summary>
        /// Thomas Wang has a function that does it in 6 shifts (provided you use the low bits, hash & (SIZE-1), 
        /// rather than the high bits if you can't use the whole value):
        /// </summary>
        public static uint Hash2(uint a) {
            a += ~(a << 15);
            a ^= (a >> 10);
            a += (a << 3);
            a ^= (a >> 6);
            a += ~(a << 11);
            a ^= (a >> 16);
            return a;
        }

        /// <summary>
        /// Here's a 5-shift one where you have to use the high bits, hash >> (32-logSize), because the low bits are hardly mixed at all:
        /// </summary>
        public static uint Hash3(uint a) {
            a = (a + 0x479ab41d) + (a << 8);
            a = (a ^ 0xe4aa10ce) ^ (a >> 5);
            a = (a + 0x9942f0a6) - (a << 14);
            a = (a ^ 0x5aedd67d) ^ (a >> 3);
            a = (a + 0x17bea992) + (a << 7);
            return a;
        }

        /// <summary>
        /// Here's one that takes 4 shifts. You need to use the bottom bits, and you need to use at least the bottom 11 bits. 
        /// It doesn't achieve avalanche at the high or the low end. It does pass my integer sequences tests, 
        /// and all settings of any set of 4 bits usually maps to 16 distinct values in bottom 11 bits.
        /// </summary>
        public static uint Hash4(uint a) {
            a = (a ^ 0xdeadbeef) + (a << 4);
            a = a ^ (a >> 10);
            a = a + (a << 7);
            a = a ^ (a >> 13);
            return a;
        }

        /// <summary>
        /// And this one isn't too bad, provided you promise to use at least the 17 lowest bits. Passes the integer sequence and 4-bit tests.
        /// </summary>
        public static uint Hash5(uint a) {
            a = a ^ (a >> 4);
            a = (a ^ 0xdeadbeef) + (a << 5);
            a = a ^ (a >> 11);
            return a;
        }

        public static uint Hash6(uint h) {
            // This function ensures that hashCodes that differ only by
            // constant multiples at each bit position have a bounded
            // number of collisions (approximately 8 at default load factor).
            h ^= (h >> 20) ^ (h >> 12);
            return h ^ (h >> 7) ^ (h >> 4);
        }
    }
}

