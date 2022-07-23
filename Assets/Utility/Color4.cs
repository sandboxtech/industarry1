
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{
    [JsonObject(MemberSerialization.OptIn)]
    public struct Color4
    {
        public static Color4 wood => From255(255, 138, 47);
        public static Color4 copperMetal => From255(255, 81, 62);

        public static Color4 white => new Color4(1, 1, 1, 1);
        public static Color4 grey => new Color4(0.5f, 0.5f, 0.5f, 1);
        public static Color4 black => new Color4(0, 0, 0, 1);

        public static Color4 red => new Color4(1, 0, 0, 1);
        public static Color4 green => new Color4(0, 1, 0, 1);
        public static Color4 blue => new Color4(0, 0, 1, 1);

        public static Color4 cyan => new Color4(0, 1, 1, 1);
        public static Color4 magenta => new Color4(1, 0, 1, 1);
        public static Color4 yellow => new Color4(1, 1, 0, 1);

        public static Color4 clear => new Color4(1, 0, 0, 0);
        public static Color4 semi => new Color4(1, 1, 1, 0.5f);

        public static Color4 red_light => new Color4(1, 0.5f, 0.5f, 1);
        public static Color4 green_light => new Color4(0.5f, 1, 0.5f, 1);
        public static Color4 blue_light => new Color4(0.5f, 0.5f, 1, 1);

        public static Color4 cyan_light => new Color4(0.5f, 1, 1, 1);
        public static Color4 magenta_light => new Color4(1, 0.5f, 1, 1);
        public static Color4 yellow_light => new Color4(1, 1, 0.5f, 1);

        public static Color4 red_dark => new Color4(0.5f, 0, 0, 1);
        public static Color4 green_dark => new Color4(0, 0.5f, 0, 1);
        public static Color4 blue_dark => new Color4(0, 0, 0.5f, 1);

        public static Color4 cyan_dark => new Color4(0, 0.5f, 0.5f, 1);
        public static Color4 magenta_dark => new Color4(0.5f, 0, 0.5f, 1);
        public static Color4 yellow_dark => new Color4(0.5f, 0.5f, 0, 1);

        public static Color4 orange_light => new Color4(1, 0.75f, 0.5f, 1);
        public static Color4 orange => new Color4(1, 0.5f, 0, 1);
        public static Color4 orange_dark => new Color4(0.5f, 0.25f, 0, 1);


        private static float Clamp01(float x) => x < 0 ? 0 : (x > 1 ? 1 : x);
        public static Color4 Lerp(Color4 left, Color4 right, float t) {
            return new Color4(
                Clamp01(left.r + (right.r - left.r) * t),
                Clamp01(left.g + (right.g - left.g) * t),
                Clamp01(left.b + (right.b - left.b) * t),
                Clamp01(left.a + (right.a - left.a) * t)
            );
        }

        public static Color4 HueshiftHSV_CPU(float h, float s, float v) {
            h += (0.5f - v) * 0.2f; // 色相偏移
            float center = 0.35f;
            s = (1 - 1.5f * (v - center) * (v - center)) * s; // 饱和度映射
            return HSV(h, s, v);
        }
        public static Color4 HSV((float, float, float) hsv) => HSV(hsv.Item1, hsv.Item2, hsv.Item3);
        public static Color4 HSV(float h, float s, float v) {

            h %= 360;
            if (h < 0) h += 360;

            float r, g, b;
            if (v <= 0) { r = g = b = 0; } else if (s <= 0) {
                r = g = b = v;
            } else {
                float hf = h / 60.0f;
                int i = (int)Mathf.Floor(hf);
                float f = hf - i;
                float pv = v * (1 - s);
                float qv = v * (1 - s * f);
                float tv = v * (1 - s * (1 - f));
                switch (i) {

                    case 0:
                        r = v;
                        g = tv;
                        b = pv;
                        break;

                    case 1:
                        r = qv;
                        g = v;
                        b = pv;
                        break;
                    case 2:
                        r = pv;
                        g = v;
                        b = tv;
                        break;

                    case 3:
                        r = pv;
                        g = qv;
                        b = v;
                        break;
                    case 4:
                        r = tv;
                        g = pv;
                        b = v;
                        break;

                    case 5:
                        r = v;
                        g = pv;
                        b = qv;
                        break;

                    case 6:
                        r = v;
                        g = tv;
                        b = pv;
                        break;

                    case -1:
                        r = v;
                        g = pv;
                        b = qv;
                        break;

                    default:
                        r = g = b = v;
                        break;
                }
            }
            return new Color4(r, g, b, 1);
        }



        public Color4(float r, float g, float b, float a = 0) {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        public const float C255 = 255f;

        public static Color4 From255(float r, float g, float b, float a = C255) {
            const float divider = 255f;
            return new Color4(r / divider, g / divider, b / divider, a / divider);
        }
        public (int, int, int, int) To255 => ((int)(r * C255), (int)(g * C255), (int)(b * C255), (int)(a * C255));

        public string Dye(string s) {
            var c255 = To255;
            return $"<color=#{c255.Item1:x2}{c255.Item2:x2}{c255.Item3:x2}{c255.Item4:x2}>{s}</color>";
        }

        [JsonProperty]
        public float r;
        [JsonProperty]
        public float g;
        [JsonProperty]
        public float b;
        [JsonProperty]
        public float a;

        public override string ToString() {
            return $"({r},{g},{b},{a})";
        }

        public static implicit operator Color4(Color vec) {
            return new Color4(vec.r, vec.g, vec.b, vec.a);
        }

        public static implicit operator Color(Color4 pos) {
            return new Color(pos.r, pos.g, pos.b, pos.a);
        }
        public static implicit operator Color4((float, float, float) vec) {
            return new Color4(vec.Item1, vec.Item2, vec.Item3, 1);
        }

        public static implicit operator (float, float, float)(Color4 pos) {
            return (pos.r, pos.g, pos.b);
        }


        public Color4 SetA(float a) {
            this.a = a;
            return this;
        }

    }
}
