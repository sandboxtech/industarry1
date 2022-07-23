
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{
    /// <summary>
    /// T for Transformation
    /// </summary>
    public static class Transformation
    {
        public static Matrix4x4 Identity => Matrix4x4.identity;
        public static Matrix4x4 Translate(Vec2 v) => Matrix4x4.Translate(new Vector3(v.x, v.y));

        public static Matrix4x4 Scale05(float x, float y) =>
            Matrix4x4.Translate(new Vector3(0.5f, 0, 0))
            * Matrix4x4.Scale(new Vector3(x, y, 1))
            * Matrix4x4.Translate(new Vector3(-0.5f, 0, 0));
        public static Matrix4x4 Scale(float x, float y, float translation) =>
            Matrix4x4.Translate(new Vector3(translation, 0, 0))
            * Matrix4x4.Scale(new Vector3(x, y, 1))
            * Matrix4x4.Translate(new Vector3(-translation, 0, 0));

        public static float ease_revserse(float x, Func<float, float> ease) => 1 - ease(1 - x);

        public static float ease_in_out(float x,
            Func<float, float> ease_in, Func<float, float> ease_out) {
            return x < 0.5f ? ease_in(x * 2) * 0.5f : (ease_out(x * 2 - 1) + 1) * 0.5f;
        }

        public static float ease_in_sine(float x) => 1 - M.Cos(x * M.PI_Half);
        public static float ease_out_sine(float x) => M.Sin(x * M.PI_Half);
        public static float ease_in_out_sine(float x) => (1 - M.Cos(x * M.PI)) / 2;


        public static float ease_in_out_cubic(float x) => x < 0.5f ? 2 * x * x : (-2 * x + 4) * x - 1;


        public static float ease_in_quad(float x) => x * x;
        public static float ease_out_quad(float x) => -x * (x + 2);

        public static float ease_in_elastic(float x) {
            if (x <= 0) return 0;
            if (x >= 1) return 1;
            return -M.Pow(2, 10 * (x - 1)) * M.Sin((x * 10 - 10.75f) * (2 * M.PI / 3));
        }
    }
}
