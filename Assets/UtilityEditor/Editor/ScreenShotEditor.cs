

//using System.IO;
//using UnityEditor;
//using UnityEngine;

//namespace Weathering
//{
//    [CustomEditor(typeof(ScreenShot))]
//    public class ScreenShotEditor : Editor
//    {
//        public override void OnInspectorGUI() {

//            DrawDefaultInspector();


//            if (GUILayout.Button("截图并且保存到桌面")) {
//                ScreenShot myScript = (ScreenShot)target;
//                Debug.Log($"保存到 {myScript.TakeScreenShot()}");

//            }

//            if (GUILayout.Button("删除存档")) {

//                string file = $"{Application.persistentDataPath}/globals.json";
//                if (File.Exists(file)) {
//                    File.Delete(file);

//                    Debug.Log($"已经删除 {file}");
//                }
//                else {
//                    Debug.LogWarning($"没有存档 {file}");
//                }

//            }
//        }
//    }
//}
