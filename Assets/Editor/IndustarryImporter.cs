
using UnityEditor;

namespace W
{
    public class IndustarryImporter : AssetPostprocessor
    {
        void OnPreprocessTexture() {
            TextureImporter importer = (TextureImporter)assetImporter;
            importer.textureType = TextureImporterType.Sprite;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.filterMode = UnityEngine.FilterMode.Point;
            importer.spritePixelsPerUnit = 16;
            // importer.spritePivot = new UnityEngine.Vector2(0, 0);
        }
    }
}
