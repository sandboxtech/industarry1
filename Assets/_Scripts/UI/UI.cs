
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;

namespace W
{
    /// <summary>
    /// execution order: after game entry, before everything else
    /// 
    /// UI 承担了多个职责：
    /// 计算输入
    /// 管理物品栏
    /// 管理界面
    /// 管理提示
    /// </summary>
    public class UI : MonoBehaviour
    {
        public static UI I { get; private set; }

        private void Awake() {
            A.Assert(I == null);
            I = this;

            CanvasTransform = Canvas.transform as RectTransform;
            mainCamTransform = mainCam.transform;
            mainPPCTransform = mainPPC.transform;
        }


        [Header("Self")]
        [SerializeField]
        private Canvas Canvas;
        private RectTransform CanvasTransform;

        [SerializeField]
        private RectTransform Scaler;

        [Header("Prefab")]
        [SerializeField] private Transform ConvexBackground;
        [SerializeField] private Transform ConcaveBackground;


        #region camera

        public const float CameraZ = -10;

        [Header("Camera")]
        [SerializeField]
        private Camera mainCam;
        private Transform mainCamTransform;
        public Camera Camera => mainCam;
        public float CameraOrthographicSize => mainCam.orthographicSize;

        [SerializeField]
        private PixelPerfectCamera mainPPC;
        private Transform mainPPCTransform;
        public PixelPerfectCamera PPC { get => mainPPC; }

        public Vector2 CameraPosition {
            set {

                mainCamTransform.position = new Vector3(value.x, value.y, CameraZ);
            }
            private get => mainCamTransform.position;
        }

        public Vector2 ScreenToCanvas(Vector2 pos) {
            Vector2 v = Camera.ScreenToViewportPoint(pos);
            return isPortrait
                ? new Vector2(v.x * C.PortraitCanvasWidth, v.y * C.PortraitCanvasHeight)
                : new Vector2(v.x * C.LandscapeCanvasWidth, v.y * C.LandscapeCanvasHeight);
        }


        #endregion



        public void OnTap(Vec2 pos) {
            if (UI_Scroll.I.Active) {
                UI_Scroll.I.Active = false;
            } else if (G.map.IsInside(pos)) {
                (G.player as __Player__).OnTapMap(pos);
            }
        }
        public void OnPointerDown(Vec2 pos) {

        }
        public void OnPointerUp(Vec2 pos) {

        }


        #region update
        private void Start() {
            UI_Joystick.I.LeftTap = () => { (G.player as __Player__).OnTapLeft(); };
            UI_Joystick.I.RightTap = () => { (G.player as __Player__).OnTapRight(); };
            // TryReCalcPortrait();

            SyncRotation();
            SyncUIColor();
        }

        public void RerenderUIColor() {
            // 切换地图，自动关闭UI
            if (UI_Scroll.I != null && UI_Scroll.I.Active) UI_Scroll.I.Active = false;

            SyncUIColor();
        }

        public void SyncRotation() {
            Screen.autorotateToLandscapeLeft = G.settings.allow_landscape_left;
            Screen.autorotateToLandscapeRight = G.settings.allow_landscape_right;
            Screen.autorotateToPortrait = G.settings.allow_portrait_up;
            Screen.autorotateToPortraitUpsideDown = G.settings.allow_portrait_down;
        }

        private void SyncUIColor() {
            Color4 uiColor = G.theme.UIColor;
            UI_Hand.I.UIColor = uiColor;
            UI_Joystick.I.UIColor = uiColor;
            UI_Scroll.I.UIColor = uiColor;
            UI_Scroll.ColorBorder(ConcaveBackground, uiColor);
            UI_Scroll.ColorBorder(ConvexBackground, uiColor);
        }

        private void Update() {
            UpdateMousePosition();
            TryRecaculate();
        }

        private void LateUpdate() {

            if (G.map.terrain.NoPlayerAvatar) {
                Vector3 pos = CameraPosition;
                pos += UI_Joystick.I.MovementRaw * Time.deltaTime * 10;
                pos = new Vector3(M.Clamp(0, G.map.size.x, pos.x), M.Clamp(0, G.map.size.y, pos.y), pos.z);
                CameraPosition = pos;

            } else {
                CameraPosition = PlayerView.I.PlayerCenter;
            }


            //// debug
            //Vec2 v = new Vec2((int)FloorPos.x, (int)FloorPos.y);
            //if (G.map.IsInside(v)) {
            //    Item item = G.map[v];
            //    string text = item == null ? "null" : item.GetType().Name;

            //    DebugText.text = $"{G.player.hand.first_empty_space}; {v.x},{v.y}: {text}{(item == null ? null : (item.type == null ? null : $"({item.type.Name})"))}";

            //    if (Input.GetMouseButtonDown(0)) {
            //        Map.ReRender9At(v);
            //    }
            //}
        }
        #endregion




        #region mouse position
        private Vector3 screenPos { get; set; }
        private Vector3 worldPos { get; set; }
        public Vector3 FloorPos { get; private set; }
        public Vec2 FloorPosInt { get; private set; }
        public bool AnyInputThisFrame { get; private set; }
        public bool AnyInputLastFrame { get; private set; }
        private void UpdateMousePosition() {
            if (Information.I.IsInStandalone) {
                screenPos = Input.mousePosition;
                worldPos = Camera.ScreenToWorldPoint(screenPos);
                worldPos = new Vector3(worldPos.x, worldPos.y, 0);
                FloorPosInt = new Vec2(M.Floor(worldPos.x), M.Floor(worldPos.y));
                FloorPos = new Vector3(FloorPosInt.x, FloorPosInt.y, 0);
            }
            AnyInputLastFrame = AnyInputThisFrame;
            AnyInputThisFrame = Input.anyKeyDown;
        }
        #endregion



        #region portrait

        private bool isPortrait;

        private Vector2Int lastScreenSize;
        private void TryRecaculate() {
            int width = Screen.width;
            int height = Screen.height;
            Vector2Int thisScreenSize = new Vector2Int(width, height);
            if (lastScreenSize == thisScreenSize) {
                return;
            }
            lastScreenSize = thisScreenSize;

            Recaculate();
        }

        public void Recaculate() {
            int width = Screen.width;
            int height = Screen.height;

            isPortrait = Screen.width < Screen.height;
            MapView.I.MakeSpaceMaterialPortrait(isPortrait);

            Settings settings = G.settings;

            int upscale = isPortrait
                ? M.Min(width / C.PortraitCanvasWidth, height / C.PortraitCanvasHeight)
                : M.Min(width / C.LandscapeCanvasWidth, height / C.LandscapeCanvasHeight);


            int theLong = upscale * C.LandscapeCanvasWidth;
            int theShort = upscale * C.LandscapeCanvasHeight;

            float scale = settings.camera_size;
            PPC.enabled = settings.ppc_enable;
            if (PPC.enabled) {
                if (scale <= 0) {
                    scale = 0.25f;
                    PPC.assetsPPU = 128;
                } else if (scale == 1) {
                    scale = 0.5f;
                    PPC.assetsPPU = 64;
                } else if (scale == 2) {
                    scale = 1;
                    PPC.assetsPPU = 32;
                } else if (scale == 3) {
                    scale = 2;
                    PPC.assetsPPU = 16;
                } else {
                    scale = 4;
                    PPC.assetsPPU = 8;
                }

                PPC.refResolutionX = isPortrait ? C.PortraitCameraWidth : C.LandscapeCameraWidth;
                PPC.refResolutionY = isPortrait ? C.PortraitCameraHeight : C.LandscapeCameraHeight;
            } else {
                // crop frames
                Camera.pixelRect = isPortrait
                    ? new Rect((width - theShort) / 2, (height - theLong) / 2, theShort, theLong)
                    : new Rect((width - theLong) / 2, (height - theShort) / 2, theLong, theShort);

                // adjust orthographc size
                Camera.orthographicSize = isPortrait
                    ? C.PortraitCameraOrthographicSize * scale
                    : C.LandscapeCameraOrthographicSize * scale;
            }


            #region canvas transformation
            CanvasTransform.anchoredPosition = (isPortrait
                ? new Vector2(-C.LandscapeCameraOrthographicSize, -C.PortraitCameraOrthographicSize)
                : new Vector2(-C.PortraitCameraOrthographicSize, -C.LandscapeCameraOrthographicSize)) * scale;
            CanvasTransform.sizeDelta = -CanvasTransform.anchoredPosition * 2;

            Scaler.sizeDelta = isPortrait
                ? new Vector2(C.PortraitCanvasWidth, C.PortraitCanvasHeight)
                : new Vector2(C.LandscapeCanvasWidth, C.LandscapeCanvasHeight);
            Scaler.localScale = new Vector3(C.PPUReciprocal * scale, C.PPUReciprocal * scale, 1);

            #endregion

            // reposition uiitems like joystick
            (UI_Joystick.I as _UI_Joystick)._Update_Portrait(isPortrait);
        }

        public const int CameraSizeMax = 4;
        public int CameraSize {
            get => G.settings.camera_size;
            set {
                int v = M.Clamp(0, CameraSizeMax, value);
                G.settings.camera_size = v;
                Recaculate();
            }
        }

        public bool EnablePPC {
            get => G.settings.ppc_enable;
            set {
                G.settings.ppc_enable = value;
                PPC.enabled = value;
                Recaculate();
            }
        }

        #endregion

    }

}
