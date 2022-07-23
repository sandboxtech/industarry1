

using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace W
{
    public class TerrainMappingFileGenerator : MonoBehaviour
    {
        public int size = 8;
        public Sprite Source;
        public Sprite Target;

        [System.NonSerialized]
        private Dictionary<Vec2, Vec2> mappings = new Dictionary<Vec2, Vec2>();
        [System.NonSerialized]
        private Dictionary<Color, Vec2> colorToBottomLeft = new Dictionary<Color, Vec2>();

        [ContextMenu("Generate")]
        private void Generate() {
            Texture2D smallT = Source.texture;
            Texture2D bigT = Target.texture;

            mappings.Clear();
            colorToBottomLeft.Clear();
            

            if (smallT.width / size != 0) throw new System.Exception();
            if (smallT.height / size != 0) throw new System.Exception();
            if (bigT.width / size != 0) throw new System.Exception();
            if (bigT.width / size != 0) throw new System.Exception();

            for (int i = 0; i < smallT.width / size; i++) {
                for (int j = 0; j < smallT.height / size; j++) {
                    Vec2 index = new Vec2(i * size, j * size);
                    Color color = smallT.GetPixel(index.x, index.y);
                    colorToBottomLeft.Add(color, index);
                }
            }

            for (int i = 0; i < bigT.width / size; i++) {
                for (int j = 0; j < bigT.height / size; j++) {
                    Vec2 index = new Vec2(i * size, j * size);
                    Color color = bigT.GetPixel(index.x, index.y);
                    if (!colorToBottomLeft.ContainsKey(color)) {
                        Debug.LogWarning($"{color} : {i}_{j}");
                    }
                    mappings.Add(index, colorToBottomLeft[color]);
                }
            }

            StringBuilder sb = new StringBuilder();
            foreach (var item in mappings) {
                sb.Append($"{item.Key.x} {item.Key.y} {item.Value.x} {item.Value.y}\n");
            }
            Debug.Log(sb.ToString());
            // Debug.Log(JsonConvert.SerializeObject(mappings, Formatting.Indented));


            mappings.Clear();
            colorToBottomLeft.Clear();
        }
    }
}
