
#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace W
{
    public class CPUFilterUtility : MonoBehaviour
    {

        public Texture2D Source;


        [ContextMenu("map")]
        private void Map() {
            if (Source == null) throw new System.Exception();
            string path = AssetDatabase.GetAssetPath(Source);

            // down sampling
            Texture2D result = new Texture2D(Source.width, Source.height);

            const float ccc = 0.1f;
            for (int i = 0; i < Source.width; i++) {
                for (int j = 0; j < Source.height; j++) {
                    Color c1 = Source.GetPixel(M.Clamp(0, Source.width - 1, i + 1), j);
                    Color c2 = Source.GetPixel(M.Clamp(0, Source.width - 1, i - 1), j);
                    Color c3 = Source.GetPixel(i, M.Clamp(0, Source.height - 1, j + 1));
                    Color c4 = Source.GetPixel(i, M.Clamp(0, Source.height - 1, j - 1));
                    Color c5 = Source.GetPixel(i, j);
                    if (c5.a < ccc) {
                        if ((c1.a > ccc || c2.a > ccc || c3.a > ccc || c4.a > ccc)) {
                            result.SetPixel(i, j, Color.black);
                        } else {
                            result.SetPixel(i, j, Color.clear);
                        }
                    } else {
                        result.SetPixel(i, j, c5);
                    }
                }
            }

            result.Apply();

            Debug.Log($"OK at  {path + "_result.png"}");

            File.WriteAllBytes(path + "_result.png", result.EncodeToPNG());
        }


        private void Repeat(Texture2D result) {
            for (int i = 0; i < 10; i++) {
                for (int j = 0; j < 10; j++) {

                    for (int ii = 0; ii < 16; ii++) {
                        for (int jj = 0; jj < 16; jj++) {
                            result.SetPixel(i * 16 + ii, j * 16 + jj, Source.GetPixel(ii, jj));
                        }
                    }

                }
            }
        }

    }
}
#endif