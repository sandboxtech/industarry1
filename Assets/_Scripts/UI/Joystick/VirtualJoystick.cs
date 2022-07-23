
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace W
{
    public interface IJoystick
    {
        Vector2 direction { get; }
        float magnitude { get; }
        float opacity { set; }
    }

    public class VirtualJoystick : MonoBehaviour, IJoystick, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
    {
        public const int Size = 40;
        public const int Margin = 20;

        public float opacity {
            set {
                float o = M.Clamp01(value);
                if (o < C.ClampAlpha) o = 0;
                if (o == 0) {
                    gameObject.SetActive(false);
                } else {
                    gameObject.SetActive(true);

                    Color backgroundColor = JoystickBackImage.color; backgroundColor.a = o;
                    JoystickBackImage.color = backgroundColor;
                    Color imageColor = JoystickImage.color; imageColor.a = o;
                    JoystickImage.color = imageColor;
                }
            }
        }


        [SerializeField]
        private RectTransform JoystickBack;
        [SerializeField]
        private RectTransform Joystick;

        private Image JoystickBackImage;
        private Image JoystickImage;
        public Color4 UIColor { set { JoystickBackImage.color = value; JoystickImage.color = value; } }

        private void Awake() {
            GetComponent<Image>().alphaHitTestMinimumThreshold = C.ClampAlpha;

            JoystickBackImage = JoystickBack.GetComponent<Image>();
            JoystickImage = Joystick.GetComponent<Image>();
        }


        private bool dragging = false;
        private Vector2 begin_drag_position;

        public Vector2 direction { get; private set; }
        public float magnitude { get; private set; }

        public void OnBeginDrag(PointerEventData eventData) {
            dragging = true;
            begin_drag_position = JoystickBack.anchoredPosition + JoystickBack.sizeDelta / 2; // Cam.I.ScreenToCanvas(eventData.position);

            Joystick.gameObject.SetActive(dragging);
        }

        public const float outer_radius = 20;
        public const float inner_radius = 10;
        public const float delta_radius = outer_radius - inner_radius;
        public const float dead_zone = 4;

        public void OnDrag(PointerEventData eventData) {

            Vector2 current_position = UI.I.ScreenToCanvas(eventData.position);
            direction = current_position - begin_drag_position;

            magnitude = M.Clamp01(M.InverseLerp(dead_zone * dead_zone, outer_radius * outer_radius, direction.sqrMagnitude));
            direction = direction.normalized;

            // circle
            // Joystick.anchoredPosition = direction * magnitude * radius * (1f / 4);

            // rect
            //float x = M.Clamp(-radius / 2, radius / 2, current_position.x - begin_drag_position.x);
            //float y = M.Clamp(-radius / 2, radius / 2, current_position.y - begin_drag_position.y);
            //Joystick.anchoredPosition = new Vector2(x, y);

            // diamond
            float x = current_position.x - begin_drag_position.x;
            float y = current_position.y - begin_drag_position.y;
            float xr;
            float yr;
            bool bx = x < 0 ? true : false;
            bool by = y < 0 ? true : false;
            if (bx) x = -x;
            if (by) y = -y;
            if (y + x < delta_radius) {
                xr = bx ? -x : x;
                yr = by ? -y : y;
            } else {
                yr = (y - x + delta_radius) / 2;
                xr = (x - y + delta_radius) / 2;
                if (yr < 0) {
                    xr = bx ? -delta_radius : delta_radius;
                    yr = 0;
                } else if (xr < 0) {
                    xr = 0;
                    yr = by ? -delta_radius : delta_radius;
                } else {
                    xr = bx ? -xr : xr;
                    yr = by ? -yr : yr;
                }
            }

            Joystick.anchoredPosition = new Vector2(xr, yr);
        }

        public void OnEndDrag(PointerEventData eventData) {
            dragging = false;
            Joystick.gameObject.SetActive(dragging);

            direction = Vector2.zero;
            Joystick.anchoredPosition = Vector2.zero;
        }

        public Action OnTap { set; private get; }
        public void OnPointerClick(PointerEventData eventData) {
            if (!eventData.dragging) {
                OnTap?.Invoke();
            }
        }
    }
}
