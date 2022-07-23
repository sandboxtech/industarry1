
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace W
{
    public class UI_Hand : MonoBehaviour
    {
        [Header("Hand")]

        [SerializeField]
        private Image Indicator;
        [SerializeField]
        private RectTransform IndicatorTransform;

        [SerializeField]
        private Slot[] Hand;

        public static UI_Hand I { get; private set; }
        private void Awake() {
            A.Assert(I == null);
            I = this;
        }

        private void Start() {
            A.Assert(Hand.Length == C.HandLength);
            SyncView();
            BindEvent();
        }


        [Header("Border")]
        [SerializeField] private Image[] Borders;

        public Color4 UIColor {
            set {
                for (int i = 0; i < Borders.Length; i++) {
                    Borders[i].color = value;
                }
                for (int i = 0; i < Hand.Length; i++) {
                    Hand[i].UIColor = value;
                }
            }
        }


        private void BindEvent() {
            for (int i = 0; i < C.HandLength; i++) {
                int j = i; // closure
                Hand[i].OnTap = () => OnTap(j);
            }
        }
        public void OnTap(int i) {
            if (i != G.player.HandIndex) {
                MoveIndicator(i);
                Audio.I.PlayerClick();
            }
            (G.player as __Player__).OnTapHand(i);
            RenderItemAt(i);
        }
        private void MoveIndicator(int i) {
            IndicatorTransform.SetParent(Hand[i].Transform, false);
            IndicatorTransform.anchoredPosition = Vector2.zero;

        }

        private void SyncView() {
            MoveIndicator(G.player.HandIndex);
            for (int i = 0; i < C.HandLength; i++) {
                RenderItemAt(i);
            }
        }

        public void RenderEntireHand() {
            for (int i = 0; i < Hand.Length; i++) {
                Item item = G.player.hand[i];
                Hand[i].RenderItem(item);
            }

        }
        public void RenderItemAt(int i) {
            Slot slot = Hand[i];
            slot.RenderItem(G.player.hand[i]);
            if (G.player.hand[i] != null) {
                Bounce(i, 0.5f);
            }
        }
        private void Bounce(int i, float second = 0.5f) {
            if (G.settings.anim_shake_amplitude <= 0) return;
            if (G.settings.anim_shake_duration <= 0) return;
            Tween.Add((long)(C.Second * second * G.settings.anim_shake_duration), (float t) => {

                float offset = M.Sin(t * 2 * M.PI) * (G.universe.Anim() + 0.25f) * 0.5f * G.settings.anim_shake_amplitude;
                if (t < 1) {
                    Hand[i].BounceContentTransform(1 + offset, 1 - offset);
                } else {
                    Hand[i].BounceContentTransformClear();
                }

                return G.player.hand[i] != null;
            });
        }

    }
}
