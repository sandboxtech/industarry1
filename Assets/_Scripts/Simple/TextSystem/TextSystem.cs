

using System.Collections.Generic;
using UnityEngine;

namespace W
{
    public class TextSystem : MonoBehaviour
    {
        public static TextSystem Ins { get; private set; }



        public int LineCountOf(string filename) => allFiles[filename].Count;
        public string LineOf(string filename, int index) => allFiles[filename][index];

        public string RandomLineOf(string filename, uint hashcode) {
            List<string> file = allFiles[filename];

            int count = file.Count;
            int index = H.Mod(hashcode, count);

            return file[index];
        }

        private Dictionary<string, List<string>> allFiles = new Dictionary<string, List<string>>();


        private HashSet<string> sameLineTest = new HashSet<string>();

        private void Awake() {
            if (Ins != null) throw new System.Exception();
            Ins = this;


        }

        private void Start() {
            ProcessTextAsset(Res.I.TextOf("Map_Head"));
            ProcessTextAsset(Res.I.TextOf("Map_Tail"));
        }

        private void ProcessTextAsset(TextAsset textAsset) {
            if (allFiles.ContainsKey(textAsset.name)) {
                throw new System.Exception(textAsset.name);
            }
            List<string> lines = new List<string>();
            allFiles.Add(textAsset.name, lines);

            string[] lines_ = textAsset.text.Split('\n', '\r');

            int i = 0;
            foreach (string line in lines_) {
                i++;

                if (line.Length == 0 || line[0] == '#') {
                    continue;
                } else {
                    //if (sameLineTest.Contains(line)) {
                    //    throw new System.Exception($"相同行：行数 {i} . 文件 {textAsset.name} . 内容 {line}");
                    //}
                    //sameLineTest.Add(line);

                    lines.Add(line);
                }
            }
            //sameLineTest.Clear();
        }
    }
}
