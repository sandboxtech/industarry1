
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{

    public interface _UI_Joystick
    {
        void _Update_Portrait(bool portrait);
    }

    public class UI_Joystick : MonoBehaviour, _UI_Joystick
    {

        public static UI_Joystick I { get; private set; }
        private void Awake() {
            A.Assert(I == null);
            I = this;
        }

        private void Start() {
            FreeOpacity = G.settings.free_joystick_opacity;
            FixedOpacity = G.settings.fixed_joystick_opacity;
        }

        #region input

        public bool HoldTap => FreeJoystick.HoldTap;
        public IJoystick Free => FreeJoystick;
        public IJoystick Left => VirtualJoystickLeft;
        public IJoystick Right => VirtualJoystickRight;


        public Color4 UIColor {
            set {
                FreeJoystick.UIColor = value;
                VirtualJoystickLeft.UIColor = value;
                VirtualJoystickRight.UIColor = value;
            }
        }


        [Header("Joystick")]
        [SerializeField]
        private FreeJoystick FreeJoystick;
        [SerializeField]
        private VirtualJoystick VirtualJoystickLeft;
        [SerializeField]
        private VirtualJoystick VirtualJoystickRight;

        public float FreeOpacity {
            get => G.settings.free_joystick_opacity;
            set {
                float opacity = value;
                G.settings.free_joystick_opacity = opacity;
                FreeJoystick.opacity = opacity;
            }
        }
        public float FixedOpacity {
            get => G.settings.fixed_joystick_opacity;
            set {
                float opacity = value;
                G.settings.fixed_joystick_opacity = opacity;
                VirtualJoystickLeft.opacity = opacity;
                VirtualJoystickRight.opacity = opacity;
            }
        }


        public Action LeftTap { set => VirtualJoystickLeft.OnTap = value; }
        public Action RightTap { set => VirtualJoystickRight.OnTap = value; }

        public Vector3 MovementRaw {
            get {
                Vector3 result = Left.direction;
                if (G.settings.free_joystick_opacity >= C.ClampAlpha) result = result + (Vector3)Free.direction;
                if (Information.I.IsInStandalone) {
                    if (Input.anyKey) {
                        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
                            result += Vector3.right;
                        }
                        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
                            result += Vector3.left;
                        }
                        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
                            result += Vector3.up;
                        }
                        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
                            result += Vector3.down;
                        }
                    }
                }
                return result;
            }
        }

        private Vec2 movementFrameBuffer;
        private long lastFrame;

        public Vec2 Movement => Information.FrameBuffer(ref movementFrameBuffer, ref lastFrame, () => UI_Scroll.I.Active ? Vec2.zero : ToInt(MovementRaw));

        public Vector3 FacingRaw {
            get {
                return Right.direction;
            }
        }

        public Vec2 Facing => ToInt(FacingRaw);
        private Vec2 ToInt(Vector2 v) {
            if (v.magnitude < 0.0001f) { return Vec2.zero; }

            float absx = M.Abs(v.x);
            float absy = M.Abs(v.y);
            if (absx > absy) {
                return v.x > 0 ? Vec2.right : Vec2.left;
            } else {
                return v.y > 0 ? Vec2.up : Vec2.down;
            }
        }

        void _UI_Joystick._Update_Portrait(bool portrait) {
            // 固定摇杆位置
            float offset = portrait ? VirtualJoystick.Margin + 4 : VirtualJoystick.Margin;
            (VirtualJoystickLeft.transform as RectTransform).anchoredPosition = portrait
                ? new Vector2(VirtualJoystick.Margin, offset)
                : new Vector2(VirtualJoystick.Margin, offset);
            (VirtualJoystickRight.transform as RectTransform).anchoredPosition = portrait
                ? new Vector2(C.LandscapeCanvasHeight - VirtualJoystick.Size - VirtualJoystick.Margin, offset)
                : new Vector2(C.LandscapeCanvasWidth - VirtualJoystick.Size - VirtualJoystick.Margin, offset);

        }

        #endregion



    }
}
