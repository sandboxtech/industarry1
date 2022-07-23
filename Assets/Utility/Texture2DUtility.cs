

namespace W
{
    //private void DebugMapNoise() {
    //    // 生成查看噪声贴图
    //    MapOfTerrain map = ActiveMap as MapOfTerrain;
    //    Texture2D result = new Texture2D(map.Width, map.Height);
    //    for (int i = 0; i < map.Width; i++) {
    //        for (int j = 0; j < map.Height; j++) {
    //            float altitude = map.AltitudeOf(map.GetBaseTile(i, j));
    //            if (altitude > max) {
    //                max = altitude;
    //            }
    //            result.SetPixel(i, j, Color.Lerp(Color.black, Color.white, (altitude + 1) / 2));
    //        }
    //    }
    //    result.Apply();
    //    string path = Application.persistentDataPath + "/debug_map.png";
    //    System.IO.File.WriteAllBytes(path, result.EncodeToPNG());

    //    Debug.Log($"OK. file generated at {path}");
    //}
}
