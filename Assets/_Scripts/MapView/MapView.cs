
using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace W
{
    public interface IIndicatorAhead { bool Show { get; } } // for indicator



    public class MapView : MonoBehaviour
    {
        public static MapView I { get; private set; }

        private void Awake() {
            A.Assert(I == null);
            I = this;

            BackgroundTransform = Background.transform;
        }

        private void Start() {
            StartBelt(); // 四向传送
            StartIndicator(); // 指示器

            RenderEntireMap(false);
        }

        private void Update() {
            UpdateBelt();
            UpdateIndicator();
            UpdateRenderTheme();

            RandomRenderANearbyItem();
        }

        #region random render

        private void RandomRenderANearbyItem() {
            uint seed = G.frame_seed;
            uint radius = ((seed >> 4) % 7) * 2 + 1;
            uint diameter = radius * 2 + 1;
            uint x = (seed >> 8) % diameter;
            uint y = (seed >> 12) % diameter;
            Vec2 pos = G.player.position + new Vec2((int)x - (int)radius, (int)y - (int)radius);
            if (G.map.IsInside(pos)) {
                ReRenderTile(G.map, pos);
            }
        }
        #endregion



        #region indicator
        [SerializeField]
        public Transform IndicatorTransform;

        [SerializeField]
        public Transform IndicatorActionTransform;

        [SerializeField]
        public BuildingBlueprint BuildingBlueprint;

        private bool IsInStandalone = false;
        private void StartIndicator() {
            IsInStandalone = Information.I.IsInStandalone;
            IndicatorTransform.gameObject.SetActive(IsInStandalone);
            IndicatorTransform.gameObject.SetActive(IsInStandalone);
        }

        private Vec2 lastPos;
        private Vector3 blueprintVelocity;
        private Vector3 blueprintTarget;
        private void UpdateIndicator() {

            Item hand = G.player.HandItem;

            if (IsInStandalone) {
                Vec2 pos = UI.I.FloorPosInt;
                Vector3 pos3 = new Vector3(pos.x, pos.y, 0);
                IndicatorTransform.position = pos3;

                float opacity = G.settings.blueprint_opacity;
                if (opacity > 0) {
                    float damping = G.settings.blueprint_damping * 0.2f;
                    if (pos != lastPos) {
                        lastPos = pos;

                        if (pos == G.player.position) {
                            // Particle.CreateSteamExplosion(pos);
                            BuildingBlueprint.RenderItem(null, Color.white);
                        } else {
                            blueprintTarget = pos3;

                            BuildingBlueprint.RenderItem(hand, hand == null || hand._DroppableAt(pos) ? Color.white : new Color(1, 0, 0, 1));
                        }
                    }
                    if (hand != null) {
                        if (damping > 0) {
                            BuildingBlueprint.Transform.position = Vector3.SmoothDamp(BuildingBlueprint.Transform.position, blueprintTarget, ref blueprintVelocity, damping);
                        } else {
                            BuildingBlueprint.Transform.position = blueprintTarget;
                        }
                    }
                    if (hand == null) {
                        BuildingBlueprint.RenderItem(null, Color.white);
                    }
                } else {
                    BuildingBlueprint.RenderItem(null, Color.white);
                }
            }

            if (hand is IIndicatorAhead) {
                IndicatorActionTransform.gameObject.SetActive(true);
                IndicatorActionTransform.position = G.player.front;
            } else {
                IndicatorActionTransform.gameObject.SetActive(false);
            }
        }

        #endregion



        #region bounce

        public void Bounce(Vec2 pos, float second, float translation) {
            if (G.settings.anim_shake_amplitude <= 0) return;
            if (G.settings.anim_shake_duration <= 0) return;

            A.Assert(second < 10f);
            Tween.Add((long)(C.Second * second * G.settings.anim_shake_duration), (float t) => {
                float offset = M.Sin(t * 2 * M.PI) * (G.universe.Anim() + 0.25f) * 0.5f * G.settings.anim_shake_amplitude;
                if (t < 1) {
                    Main.BounceContentTransform(new Vector3Int(pos.x, pos.y, 0), 1 + offset, 1 - offset, translation);
                } else {
                    Main.BounceContentTransformClear(new Vector3Int(pos.x, pos.y, 0));
                }
                return true;
            });
        }

        #endregion







        #region tilemaps

        [Header("Shader")]
        [SerializeField]
        private Material standardMaterial;
        public Material StandardMaterial => standardMaterial;
        [SerializeField]
        private Material hueshiftMaterial;
        public Material HueshiftMaterial => hueshiftMaterial;
        [SerializeField]
        private Material hueshiftGlowMaterial;
        public Material HueshiftGlowMaterial => hueshiftGlowMaterial;
        [SerializeField]
        private Material hueshiftSpaceMaterial;
        public Material HueshiftSpaceMaterial => HueshiftSpaceMaterial;

        public void MakeSpaceMaterialPortrait(bool b) {
            hueshiftSpaceMaterial.SetFloat("_IsPortrait", b ? 1 : 0);
        }

        public enum OrderInLayer
        {
            Background = -100,
            LayerBedrock = -7,
            LayerFloor = -6,
            LayerDecor = -5,
            TilemapLow = -2,
            Indicator = -1,
            Tilemap = 0, // player
            Foreground = 1,
        }

        /// <summary>
        /// useless
        /// </summary>
        private void StartSetSortingOrder() {
            Background.sortingOrder = (int)OrderInLayer.Background;
            Bedrock.GetComponent<TilemapRenderer>().sortingOrder = (int)OrderInLayer.LayerBedrock;
            Floor.GetComponent<TilemapRenderer>().sortingOrder = (int)OrderInLayer.LayerFloor;
            Decor.GetComponent<TilemapRenderer>().sortingOrder = (int)OrderInLayer.LayerDecor;

            Main.SetLowSortingOrder((int)OrderInLayer.TilemapLow);
            Left.SetLowSortingOrder((int)OrderInLayer.TilemapLow);
            Right.SetLowSortingOrder((int)OrderInLayer.TilemapLow);
            Up.SetLowSortingOrder((int)OrderInLayer.TilemapLow);
            Down.SetLowSortingOrder((int)OrderInLayer.TilemapLow);

            // indicator.set sortingOrder

            Main.SetSortingOrder((int)OrderInLayer.Tilemap);
            Left.SetSortingOrder((int)OrderInLayer.Tilemap);
            Right.SetSortingOrder((int)OrderInLayer.Tilemap);
            Up.SetSortingOrder((int)OrderInLayer.Tilemap);
            Down.SetSortingOrder((int)OrderInLayer.Tilemap);
        }

        [Header("Texture")]
        [SerializeField] private Sprite StandardMap_WaterSprite;
        [SerializeField] private Sprite[] SpaceshipMap_StarSprites;

        [Header("Terrain")]
        [SerializeField] private SpriteRenderer Background;
        [NonSerialized] private Transform BackgroundTransform;
        [SerializeField] private Tilemap Bedrock;
        [SerializeField] private Tilemap Floor;
        [SerializeField] private Tilemap Decor;

        [Header("Main and Belt")]
        [SerializeField] private Tilemap4 Main;



        #region belt

        [SerializeField] private Tilemap4 Left;
        [SerializeField] private Tilemap4 Right;
        [SerializeField] private Tilemap4 Up;
        [SerializeField] private Tilemap4 Down;

        private Transform LeftTrans;
        private Transform RightTrans;
        private Transform UpTrans;
        private Transform DownTrans;
        private void StartBelt() {
            LeftTrans = Left.transform;
            RightTrans = Right.transform;
            UpTrans = Up.transform;
            DownTrans = Down.transform;
        }

        /// <summary>
        /// 可插值
        /// </summary>
        private void UpdateBelt() {
            float t = G.secondT;
            LeftTrans.localPosition = Vector3.left * t;
            RightTrans.localPosition = Vector3.right * t;
            UpTrans.localPosition = Vector3.up * t;
            DownTrans.localPosition = Vector3.down * t;
        }
        #endregion


        private void RenderEntireMap(bool remove_old) {

            Map map = G.map;
            const int margin = 1;

            int end_x = map.Width - margin;
            int end_y = map.Height - margin;
            for (int i = margin; i < end_x; i++) {
                for (int j = margin; j < end_y; j++) {
                    Vec2 pos = new Vec2(i, j);
                    RenderFloorTile(map, pos, remove_old);
                    RenderItemTile(map, pos, remove_old);
                }
            }

            RenderBorder();
            StartRenderTheme();
        }
        private void ClearEntireMap() {

            Map map = G.map;
            const int margin = 1;

            int end_x = map.Width - margin;
            int end_y = map.Height - margin;
            for (int i = margin; i < end_x; i++) {
                for (int j = margin; j < end_y; j++) {
                    Vec2 pos = new Vec2(i, j);
                    ClearTile(map, pos);
                }
            }
        }


        private void RenderBorder() {
            Item prefab = Map.FindPrefab(GroundType.metal_wall);

            int width = G.map.Width;
            for (int i = -1; i < width + 1; i++) {
                Vector3Int pos3d = new Vector3Int(i, -1, 0);
                Bedrock.SetTile(pos3d, Res.I.TileOf(prefab.LayerContent));
            }
        }


        private void StartRenderTheme() {
            MapTheme theme = G.map.theme;

            Background.enabled = true;
            // BackgroundTransform.localScale = G.map.size;
            Background.color = theme.BackgroundColor;

            MapType type = G.map.type;

            //Background.material = type == MapType.spaceship ? hueshiftSpaceMaterial : hueshiftMaterial;
            //Background.sprite = type == MapType.spaceship
            //    ? SpaceshipMap_StarSprites[G.frame_seed % SpaceshipMap_StarSprites.Length]
            //    : StandardMap_WaterSprite;
            Background.material = hueshiftSpaceMaterial;
            Background.sprite = SpaceshipMap_StarSprites[G.frame_seed % SpaceshipMap_StarSprites.Length];
        }

        private void UpdateRenderTheme() {

            float reflectiveness = G.map.type == MapType.planet ? M.Clamp01(Light.I.DayLightnessCosine) : 1;

            hueshiftSpaceMaterial.SetFloat("_Reflectiveness", reflectiveness);
        }
        private void OnApplicationQuit() {
            hueshiftSpaceMaterial.SetFloat("_Reflectiveness", 1);
        }

        /// <summary>
        /// when map changed
        /// </summary>
        public void OnEnterNewMap_Rerender() {
            RenderEntireMap(false);
        }

        public void OnExitOldMap_Rerender() {
            ClearEntireMap();
        }

        /// <summary>
        /// cpu optimized
        /// </summary>
        public void ReRenderNeighbor9(Vec2 pos) {
            Map map = G.map;
            Vec2 map_size = map.size;
            int x = pos.x;
            int y = pos.y;
            ReRenderTile(map, new Vec2(x, y));
            bool y_0 = y > Map.Margin;
            bool y_m = y < map_size.y - 1 - Map.Margin;
            if (y_m) {
                ReRenderTile(map, new Vec2(x, y + 1));
            }
            if (y_0) {
                ReRenderTile(map, new Vec2(x, y - 1));
            }
            if (x > Map.Margin) {
                ReRenderTile(map, new Vec2(x - 1, y));
                if (y_0) {
                    ReRenderTile(map, new Vec2(x - 1, y - 1));
                }
                if (y_m) {
                    ReRenderTile(map, new Vec2(x - 1, y + 1));
                }
            }
            if (x < map_size.x - 1 - Map.Margin) {
                ReRenderTile(map, new Vec2(x + 1, y));
                if (y_0) {
                    ReRenderTile(map, new Vec2(x + 1, y - 1));
                }
                if (y_m) {
                    ReRenderTile(map, new Vec2(x + 1, y + 1));
                }
            }
        }
        public void ReRenderTile(Map map, Vec2 pos) {
            RenderFloorTile(map, pos, true);
            RenderItemTile(map, pos, true);
        }
        private void ClearTile(Map map, Vec2 pos) {
            ClearFloorTile(map, pos);
            ClearItemTile(map, pos);
        }

        private void RenderItemTile(Map map, Vec2 pos, bool remove_old) {
            Vector3Int pos3d = new Vector3Int(pos.x, pos.y, 0);

            Item item = map._tiles[pos.x, pos.y];

            if (item == null) {
                if (remove_old) {
                    Main.RenderItem(pos3d, null);
                    Left.RenderItem(pos3d, null);
                    Right.RenderItem(pos3d, null);
                    Up.RenderItem(pos3d, null);
                    Down.RenderItem(pos3d, null);
                }
                return;
            }

            Main.RenderItem(pos3d, item);

            if (item is IHasBelt hasBelt && hasBelt.HasBelt) {
                Left.RenderItem(pos3d, item.Left);
                Right.RenderItem(pos3d, item.Right);
                Up.RenderItem(pos3d, item.Up);
                Down.RenderItem(pos3d, item.Down);
            }
        }
        private void ClearItemTile(Map map, Vec2 pos) {
            Vector3Int pos3d = new Vector3Int(pos.x, pos.y, 0);

            Main.RenderItem(pos3d, null);
            Left.RenderItem(pos3d, null);
            Right.RenderItem(pos3d, null);
            Up.RenderItem(pos3d, null);
            Down.RenderItem(pos3d, null);

            Left.RenderItem(pos3d, null);
            Right.RenderItem(pos3d, null);
            Up.RenderItem(pos3d, null);
            Down.RenderItem(pos3d, null);
        }



        private void RenderFloorTile(Map map, Vec2 pos, bool remove_old) {
            Vector3Int pos3d = new Vector3Int(pos.x, pos.y, 0);

            RenderFloorTileLayer(Bedrock, map.terrain.bedrock, pos3d, pos.x, pos.y, remove_old);
            RenderFloorTileLayer(Floor, map.terrain.floor, pos3d, pos.x, pos.y, remove_old);
            RenderFloorTileLayer(Decor, map.terrain.decor, pos3d, pos.x, pos.y, remove_old);
        }
        private void ClearFloorTile(Map map, Vec2 pos) {
            Vector3Int pos3d = new Vector3Int(pos.x, pos.y, 0);

            Bedrock.SetTile(pos3d, null);
            Floor.SetTile(pos3d, null);
            Decor.SetTile(pos3d, null);
        }

        private void RenderFloorTileLayer(Tilemap tilemap, MapTerrainLayer layer, Vector3Int pos3d, int i, int j, bool remove_old) {

            Item prefab = Map.FindPrefab(layer[i, j]);
            if (prefab != null) {
                (prefab as __IItem__).__Pos__ = new Vec2(i, j);
                tilemap.SetTile(pos3d, Res.I.TileOf(prefab.LayerContent));
                tilemap.SetColor(pos3d, prefab.LayerColor);
            } else if (remove_old) {
                tilemap.SetTile(pos3d, null);
            }
        }

        #endregion







    }
}
