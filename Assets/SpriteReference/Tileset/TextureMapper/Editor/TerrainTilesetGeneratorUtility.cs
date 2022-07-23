

using System.IO;
using UnityEngine;

namespace Weathering
{
    public class TerrainTilesetGeneratorUtility : MonoBehaviour
    {
        private const string toReverseData = @"
0 0 8 8
0 8 24 8
0 16 24 16
0 24 8 16
8 0 8 24
8 8 0 0
8 16 0 24
8 24 8 0
16 0 16 24
16 8 24 0
16 16 24 24
16 24 16 0
24 0 16 8
24 8 0 8
24 16 0 16
24 24 16 16
32 0 32 16
32 8 32 24
32 16 32 0
32 24 32 8
40 0 40 16
40 8 40 24
40 16 40 0
40 24 40 8
";



        private const string toStandardData = @"
0 0 32 16
0 8 32 24
0 16 32 16
0 24 32 24
0 32 32 16
0 40 32 24
0 48 0 0
0 56 0 8
0 64 0 16
0 72 0 8
0 80 0 16
0 88 0 24
8 0 40 16
8 8 8 8
8 16 8 16
8 24 8 8
8 32 8 16
8 40 40 24
8 48 8 0
8 56 40 24
8 64 40 16
8 72 40 24
8 80 40 16
8 88 8 24
16 0 32 16
16 8 16 8
16 16 32 0
16 24 32 8
16 32 16 16
16 40 32 24
16 48 16 0
16 56 32 24
16 64 32 16
16 72 32 24
16 80 32 16
16 88 16 24
24 0 40 16
24 8 8 8
24 16 40 0
24 24 40 8
24 32 8 16
24 40 40 24
24 48 8 0
24 56 40 24
24 64 40 16
24 72 40 24
24 80 40 16
24 88 8 24
32 0 32 16
32 8 16 8
32 16 16 16
32 24 16 8
32 32 16 16
32 40 32 24
32 48 16 0
32 56 32 24
32 64 32 16
32 72 32 24
32 80 32 16
32 88 16 24
40 0 40 16
40 8 40 24
40 16 40 16
40 24 40 24
40 32 40 16
40 40 40 24
40 48 24 0
40 56 24 8
40 64 24 16
40 72 24 8
40 80 24 16
40 88 24 24
48 0 0 0
48 8 0 8
48 16 0 16
48 24 0 8
48 32 0 16
48 40 0 24
48 48 0 0
48 56 0 8
48 64 0 16
48 72 0 8
48 80 0 16
48 88 0 24
56 0 24 0
56 8 24 8
56 16 24 16
56 24 24 8
56 32 24 16
56 40 24 24
56 48 8 0
56 56 8 8
56 64 8 16
56 72 8 8
56 80 8 16
56 88 8 24
64 0 0 0
64 8 0 24
64 16 16 0
64 24 16 8
64 32 16 16
64 40 16 24
64 48 16 0
64 56 16 8
64 64 16 16
64 72 16 8
64 80 16 16
64 88 16 24
72 0 8 0
72 8 8 24
72 16 8 0
72 24 40 24
72 32 40 16
72 40 8 24
72 48 8 0
72 56 8 8
72 64 8 16
72 72 8 8
72 80 8 16
72 88 8 24
80 0 16 0
80 8 16 24
80 16 16 0
80 24 32 24
80 32 32 16
80 40 16 24
80 48 16 0
80 56 16 8
80 64 16 16
80 72 16 8
80 80 16 16
80 88 16 24
88 0 8 0
88 8 8 24
88 16 8 0
88 24 8 8
88 32 8 16
88 40 8 24
88 48 24 0
88 56 24 8
88 64 24 16
88 72 24 8
88 80 24 16
88 88 24 24
96 0 16 0
96 8 16 24
96 16 32 16
96 24 16 8
96 32 16 16
96 40 32 24
96 48 0 16
96 56 0 8
96 64 0 16
96 72 0 8
96 80 32 16
96 88 16 8
104 0 24 0
104 8 24 24
104 16 8 16
104 24 8 8
104 32 8 16
104 40 8 8
104 48 8 16
104 56 40 24
104 64 40 16
104 72 8 8
104 80 8 16
104 88 40 24
112 0 0 0
112 8 0 24
112 16 16 16
112 24 16 8
112 32 16 16
112 40 16 8
112 48 16 16
112 56 32 24
112 64 32 16
112 72 16 8
112 80 16 16
112 88 32 24
120 0 24 0
120 8 24 24
120 16 40 16
120 24 8 8
120 32 8 16
120 40 40 24
120 48 24 16
120 56 24 8
120 64 24 16
120 72 24 8
120 80 40 16
120 88 8 8
";

        [Tooltip("标准地图内部转换，暂未实现")]
        public bool ToReverse_Standard;
        [Tooltip("小图变大图。仅支持8分地块")]
        public bool ToStandard_SmallToStandard;
        [Tooltip("小图反向。仅支持8分地块")]
        public bool ToReverse_Small;

        public Sprite[] Sources;
        // public Sprite Source;

        [ContextMenu("Generate")]
        private void Ge() {

            foreach (Sprite Source in Sources) {

                textureFrom = Source.texture;

                string path = UnityEditor.AssetDatabase.GetAssetPath(Source);


                int size = 8;
                string[] lines = null;
                if (ToStandard_SmallToStandard) {
                    size = 16;

                    if (textureFrom.width != 48 && textureFrom.height != 32) throw new System.Exception();

                    lines = toStandardData.Split('\n', '\r', '\f');
                    textureTo = new Texture2D(128, 96);
                } else if (ToReverse_Small) {
                    if (textureFrom.width != 48 && textureFrom.height != 32) throw new System.Exception();

                    lines = toReverseData.Split('\n', '\r', '\f');
                    textureTo = new Texture2D(48, 32);
                } else {
                    throw new System.Exception();
                }
                foreach (string line in lines) {
                    if (string.IsNullOrEmpty(line)) {
                        continue;
                    }
                    string[] datas = line.Split(' ');
                    if (datas.Length != 4) throw new System.Exception(datas.Length.ToString());
                    int destX = int.Parse(datas[0]);
                    int destY = int.Parse(datas[1]);
                    int sourceX = int.Parse(datas[2]);
                    int sourceY = int.Parse(datas[3]);

                    for (int i = 0; i < size; i++) {
                        for (int j = 0; j < size; j++) {
                            textureTo.SetPixel(destX + i, destY + j, textureFrom.GetPixel(sourceX + i, sourceY + j));
                        }
                    }
                }

                textureTo.Apply();

                File.WriteAllBytes(path.Substring(0, path.Length - 1 - 4) + "_result.png", textureTo.EncodeToPNG());

                // File.WriteAllBytes(Application.persistentDataPath + $"/{Source.name}.png", textureTo.EncodeToPNG());
            }
            Debug.Log($"OK");
        }

        private Texture2D textureFrom;
        private Texture2D textureTo;
    }
}
