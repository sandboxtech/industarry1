

using UnityEngine;

namespace W
{
    public class ColorRampGenerator : MonoBehaviour
    {
        /// <summary>
        /// 生成色阶颜色
        /// </summary>
        /// <param name="brightness">明度，取值0-1</param>
        /// <param name="hue">色相基础。0为红色，1/3f为绿色，2/3f为蓝色，1为红色</param>
        /// <returns></returns>
        private Color ColorRamp(float brightness, float hue, float hueOffset = 1 / 6f) {
            float h = hue + 2 * hueOffset * brightness - hueOffset + 1;
            float s = -4 * (brightness - 0.5f) * (brightness - 0.5f) + 1;
            float v = brightness;
            Debug.LogWarning($"{h} {s} {v}");
            return Color.HSVToRGB(h % 1, s, v);
        }

        [Range(4, 16)]
        public int Count = 8;

        [Range(0, 1f)]
        public float Hue = 0f;

        [Range(0, 1f)]
        public float Offset = 1 / 6f;

        public Color[] Colors;

        [ContextMenu("Test")]
        private void TestColorRamp() {
            Colors = new Color[Count];
            for (int i = 0; i < Count; i++) {
                Colors[i] = ColorRamp((float)i / (Count - 1), Hue, Offset);
            }
        }
    }
}
