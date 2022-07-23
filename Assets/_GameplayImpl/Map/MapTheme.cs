
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{
    [JsonObject(MemberSerialization.OptIn)]
    public class MapTheme : ICreatable
    {
        public void OnCreate() { }

        [JsonProperty] public Color4 White => Color4.white;

        [JsonProperty] public Color4 SandColor { get; private set; } // 砂子颜色
        [JsonProperty] public Color4 ClayColor { get; private set; } // 泥土颜色
        [JsonProperty] public Color4 ClayBakedColor { get; private set; } // 泥土颜色

        [JsonProperty] public Color4 BedrockColor { get; private set; } // 泥土颜色

        public Color4 ConcreteColor { get; private set; } = Color4.From255(235, 235, 240); // 水泥颜色
        [JsonProperty] public Color4 StoneColor { get; private set; } // 石头颜色
        [JsonProperty] public Color4 MetalColor { get; private set; } // 金属颜色
        [JsonProperty] public Color4 LightMetalColor { get; private set; } // 金属颜色


        [JsonProperty] public Color4 WaterColor { get; private set; } // 水域颜色
        [JsonProperty] public Color4 FloraColor { get; private set; } // 草叶颜色
        [JsonProperty] public Color4 DryFloraColor { get; private set; } // 草叶颜色
        [JsonProperty] public Color4 FruitColor { get; private set; }

        [JsonProperty] public Color4 BackgroundColor { get; private set; } // 背景颜色
        [JsonProperty] public Color4 UIColor { get; private set; } // 背景颜色

        public Color4 BulbColor { get; } = Color4.From255(255, 200, 0);
        public Color4 GoldColor { get; } = Color4.From255(255, 200, 0);
        public Color4 WoodColor { get; } = Color4.From255(210, 120, 70); // 210, 150, 70

        public Color4 QICColor { get; } = Color4.From255(49, 176, 80); // 210, 150, 70

        public Color4 CopperColor { get; } = Color4.From255(184, 115, 51);
        public Color4 CopperOreColor { get; } = Color4.From255(84, 150, 136);
        public Color4 IronOreColor { get; } = Color4.From255(98, 42, 29);

        public void AfterCreate() {


            uint hashcode = G.map.hashcode;

            float lightness = H.HashedFloat(ref hashcode);

            (float, float, float) SandHSV = ( // 红黄色相，中饱和，中低亮度
                M.Lerp(25, 65, H.HashedFloat(ref hashcode)),
                M.Lerp(0.6f, 0.7f, H.HashedFloat(ref hashcode)),
                M.Lerp(0.7f, 0.9f, lightness)
            );
            SandColor = Color4.HSV(SandHSV);

            (float, float, float) ClayHSV = ( // 红黄色相，中饱和，中低亮度
                M.Lerp(10, 50, H.HashedFloat(ref hashcode)),
                M.Lerp(0.5f, 0.6f, H.HashedFloat(ref hashcode)),
                M.Lerp(0.7f, 0.8f, lightness)
            );
            ClayColor = Color4.HSV(ClayHSV);

            (float, float, float) ClayBakedHSV = ( // 红黄色相，中饱和，中低亮度
                ClayHSV.Item1 - 20,
                M.Lerp(ClayHSV.Item2, 1, 0.3f),
                M.Lerp(ClayHSV.Item3, 1, 0.8f)
            );
            ClayBakedColor = Color4.HSV(ClayBakedHSV);


            (float, float, float) StoneHSV = ( // 全色相，低饱和，中高亮度
                ClayHSV.Item1,
                0.2f * H.HashedFloat(ref hashcode),
                M.Lerp(0.7f, 0.8f, lightness)
            );
            StoneColor = Color4.HSV(StoneHSV);

            (float, float, float) MetalHSV = ( // 基于StoneHSV，降低饱和度 增大亮度，
                StoneHSV.Item1,
                0,
                M.Lerp(StoneHSV.Item2, 0.9f, 0.8f)
            );
            MetalColor = Color4.HSV(MetalHSV);

            (float, float, float) LightMetalHSV = ( // 基于StoneHSV，降低饱和度 增大亮度，
                MetalHSV.Item1,
                MetalHSV.Item2,
                M.Lerp(StoneHSV.Item2, 0.9f, 0.8f)
            );
            LightMetalColor = Color4.HSV(LightMetalHSV);




            float waterHue = H.HashedFloat(ref hashcode);
            (float, float, float) WaterHSV = ( // 蓝青色相，中饱和，高亮度
                M.Lerp(130, 230, waterHue),
                M.Lerp(0.4f, 0.5f, H.HashedFloat(ref hashcode)),
                M.Lerp(0.5f, 0.7f, lightness)
            );
            WaterColor = Color4.HSV(WaterHSV);

            (float, float, float) FloraHSV = ( // 黄绿青色相，中饱和，高亮度
                M.Lerp(60, 180, waterHue),
                M.Lerp(0.5f, 0.6f, H.HashedFloat(ref hashcode)),
                M.Lerp(0.7f, 0.9f, lightness)
            );
            FloraColor = Color4.HSV(FloraHSV);

            (float, float, float) DryFloraHSV = ( // 黄绿色相，中饱和，高亮度
                M.Lerp(FloraHSV.Item1, 60, 0.9f),
                M.Lerp(FloraHSV.Item2, 0, 0.2f),
                FloraHSV.Item3 + 0.1f
            );
            DryFloraColor = Color4.HSV(DryFloraHSV);

            (float, float, float) FruitHSV = (
                M.Lerp(-40, 80, H.HashedFloat(ref hashcode)),
                M.Lerp(0.8f, 1f, H.HashedFloat(ref hashcode)),
                M.Lerp(0.5f, 0.8f, lightness)
            );
            FruitColor = Color4.HSV(FruitHSV);



            Type bedrockType = MapTerrain.GetBaseMineral();
            if (bedrockType == typeof(sand)) {
                BedrockColor = SandColor;
            } else if (bedrockType == typeof(stone)) {
                BedrockColor = StoneColor;
            } else {
                BedrockColor = ClayColor;
            }


            BackgroundColor = G.map.type == MapType.planet ? WaterColor : White;
            UIColor = G.map.type == MapType.planet ? StoneColor : MetalColor;
        }
    }
}
