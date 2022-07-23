
#if UNITY_EDITOR

using System;
using UnityEditor;
using UnityEngine;

namespace W
{
    public class RenameUtility : MonoBehaviour
    {

        public Sprite[] Assets;

        [ContextMenu("Rename")]
        public void Rename() {
            Debug.LogWarning($"开始处理{Assets.Length}个文件");
            foreach (Sprite sprite in Assets) {
                string path = AssetDatabase.GetAssetPath(sprite);

                // Func<string, string> func = SplitOrReplace ? FirstPartBySpace : ReplaceByUnderSlash;
                Func<string, string> func;
                if (Split) {
                    func = FirstPartBySpace;
                } else if (Replace) {
                    func = ReplaceByUnderSlash;
                } else if (Change) {
                    func = Change23;
                } else if (SetList) {
                    func = SetListtt;
                } else {
                    Debug.LogError("func not selected");
                    break;
                }

                string source = sprite.name;
                string result = func(sprite.name);
                Debug.Log($"{source} renamed as {result}");

                AssetDatabase.RenameAsset(path, result);
            }
        }

        public bool Split = false;

        private string FirstPartBySpace(string s) {
            string[] ss = s.Split(' ');

            return ss[0];
        }

        public bool Replace = false;

        public string before;
        public string after;
        private string ReplaceByUnderSlash(string s) {
            string[] ss = s.Split('_');

            for (int i = 0; i < ss.Length; i++) {
                if (ss[i].Equals(before)) {
                    ss[i] = after;
                }
            }
            return string.Join("_", ss);
        }

        public bool Change;
        private string Change23(string s) {
            string[] ss = s.Split('_');
            return ss[0] + "_" + ss[1];
            //for (int i = 0; i < ss.Length; i++) {
            //    string t = ss[1];
            //    ss[1] = ss[2];
            //    ss[2] = t;
            //}
            //return string.Join("_", ss);
        }
        
        [NonSerialized]
        private int i = 0;
        public bool SetList = false;
        public string ListName = "default_";
        private string SetListtt(string s) {
            return $"{ListName}{i++}";
        }
    }
}

#endif
