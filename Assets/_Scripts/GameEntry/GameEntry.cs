


using Newtonsoft.Json;
using System.IO;
using UnityEngine;

namespace W
{
    /// <summary>
    /// 所有会序列化的东西，替代构造函数
    /// </summary>
    public interface ICreatable
    {
        void OnCreate();
    }

    /// <summary>
    /// 大部分MonoBehaviour对执行顺序无要求，除了以下几个
    /// GameEntry(save) 存读档
    /// Information(frame process) 每帧信息
    /// UI(input) 玩家输入
    /// </summary>
    public class GameEntry : MonoBehaviour
    {
        /// 单例
        public static GameEntry I { get; private set; }

        /// <summary>
        /// 必须有类似Saves这样的根文件夹，不然在Application.persistentDataPath底下会出现SharingViolation文件共享错误
        /// </summary>
        private string SaveFolder => $"{Application.persistentDataPath}/Saves/{Globals.VERSION}/";


        /// <summary>
        /// 全局存档文件的地址
        /// </summary>
        private string GlobalsPath => JsonPathOf("globals");
        private string JsonPathOf(string s) => $"{SaveFolder}{s}.bin";

        /// <summary>
        /// 开发时使用。是否自动删除存档
        /// </summary>
        [SerializeField]
        private bool DeleteSaveOnStart = false;

        private void Start() {
            if (I != null) throw new System.Exception();
            I = this;

            Application.targetFrameRate = -1;

            if (DeleteSaveOnStart && !__TestLogic__.debug) DeleteSave();

            StartSave();
        }

        private float autosaveTimeAccumulation = 0;
        private void Update() {
            autosaveTimeAccumulation += Time.deltaTime;
            if (autosaveTimeAccumulation >= PlayerSettings.AutosaveSeconds) {
                Save();
                autosaveTimeAccumulation -= PlayerSettings.AutosaveSeconds;
            }
        }

        /// <summary>
        /// 地图存档文件是否存在
        /// </summary>
        public bool ExistMap(string name) {
            return File.Exists(JsonPathOf(name));
        }



        /// <summary>
        /// 是否加密和压缩存档。因为AES对大小有要求，加密已失效
        /// </summary>
        [SerializeField] private bool encryptAndCompress;

        /// <summary>
        /// 创建顺序
        /// Globals
        ///     G.I 有效
        ///     G. settings universe achievement player ... etc 有效
        ///     G.I.map_key 有效
        ///     initial Map, G.map 有效
        /// </summary>
        private void StartSave() {
            // 尝试创建存档文件夹
            if (!Directory.Exists(SaveFolder)) {
                Directory.CreateDirectory(SaveFolder);
            }

            Globals globals;
            if (!File.Exists(GlobalsPath)) {
                // 新建存档
                Creator.__CreateData<Globals>(); // 创建全局数据
                Maps.GotoInitialMap(); // 创建地图

                __TestLogic__.OnCreate();
            } else {
                // 读取存档
                string globalsJson = Creator.Read(GlobalsPath, textEncoding, encryptAndCompress);
                globals = Creator.DeserializeFromJson<Globals>(globalsJson);

                // 读取地图
                string mapfile_path = JsonPathOf(globals.map_key);
                if (File.Exists(mapfile_path)) {
                    string mapJson = Creator.Read(mapfile_path, textEncoding, encryptAndCompress);

                    Creator.DeserializeFromJson<Map>(mapJson);

                } else {
                    // 特例，记录在globals里的地图不存在，一般说明玩家进入新地图后没保存游戏
                    PlayerEnterMap(globals.map_key, globals.map_type);
                }
            }
        }


        /// <summary>
        /// 玩家进入新地图。需要提供此地图的key。第一次进入，必须提供地图类型以初始化
        /// </summary>
        public void PlayerEnterMap(string new_map_key, MapType subtype = MapType.none) {
            Information.MapEnteringFrame = true;

            // 保存前，map_key记录为新地图
            Globals.I.map_key = new_map_key;
            Globals.I.map_type = subtype;

            Map oldMap = G.map;
            if (oldMap != null) {
                // 离开旧地图
                BeforeExitMap();
                Save();
            }

            string mapfile_path = JsonPathOf(new_map_key);
            if (!File.Exists(mapfile_path)) {
                A.Assert(subtype != MapType.none); // 创建地图，则必须提供type参数。以后可以省略

                Map.Create(new_map_key, subtype); // *** 入口 ***
            } else {
                string mapJson = Creator.Read(mapfile_path, textEncoding, encryptAndCompress);
                Creator.DeserializeFromJson<Map>(mapJson);
            }

            AfterEnterMap();
        }


        private void AfterEnterMap() {
            G.map.OnPlayerEnter(G.player); // 移动玩家位置至地图入口
            (G.player as __Player__).OnEnterMap(G.map); // 无事发生

            UI.I?.RerenderUIColor(); // 重绘UI颜色
            UI_Hand.I?.RenderEntireHand(); // 重绘工具栏物品
            MapView.I?.OnEnterNewMap_Rerender(); // 重绘地图
            PlayerView.I?.RerenderPlayer(); // 重绘玩家
        }

        private void BeforeExitMap() {
            (G.player as __Player__).OnExitMap(G.map); // 无事发生
            G.map.OnPlayerExit(G.player); // 移动地图入口至玩家位置

            MapView.I.OnExitOldMap_Rerender();
        }


        private System.Text.Encoding textEncoding => System.Text.Encoding.UTF8;

        /// <summary>
        /// 保存存档，包括globals和地图
        /// </summary>
        public void Save() {
            Globals globals = Globals.I;
            Map map = G.map;

            string globalsJson = Creator.SerializeToJson(globals);
            string mapJson = Creator.SerializeToJson(map);
            string pathOfMapJson = JsonPathOf(map.key);

            //Write(GlobalsPath, globalsJson, textEncoding);
            //Write(pathOfMapJson, mapJson, textEncoding);
            Creator.WriteSafer(new string[] { GlobalsPath, pathOfMapJson }, new string[] { globalsJson, mapJson }, textEncoding, encryptAndCompress);

            if (Information.I.IsInEditor) Debug.Log($"Game saved at {SaveFolder}");
        }


        /// <summary>
        /// 删除存档
        /// </summary>
        public void DeleteSave() {
            DirectoryInfo dir = new DirectoryInfo(SaveFolder);
            if (dir.Exists) {
                FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //返回目录中所有文件和子目录
                bool anyDeleted = false;
                foreach (FileSystemInfo i in fileinfo) {
                    if (i is DirectoryInfo) {
                        DirectoryInfo subdir = new DirectoryInfo(i.FullName);
                        subdir.Delete(true);
                    } else {
                        File.Delete(i.FullName);
                        anyDeleted = true;
                    }
                }
                if (anyDeleted) Debug.Log($"Game save deleted at {SaveFolder}");
            }
        }

        /// <summary>
        /// 退出游戏
        /// </summary>
        public void ExitGame() {
            if (Information.I.IsInEditor) {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            } else {
                Application.Quit();
            }
        }
    }
}
