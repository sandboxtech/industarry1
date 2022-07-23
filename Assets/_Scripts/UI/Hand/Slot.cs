
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace W
{
    //public interface ISprite4
    //{
    //    string Content { get; }
    //    string Container { get; }
    //    string Highlight { get; }
    //    string Decoration { get; }
    //}
    //public interface IColor44 : ISprite4
    //{
    //    Color4 ContentColor { get; }
    //    Color4 ContainerColor { get; }
    //    Color4 HighlightColor { get; }
    //    Color4 DecorationColor { get; }
    //}
    //public interface IBelt4
    //{
    //    Item Right { get; }
    //    Item Up { get; }
    //    Item Left { get; }
    //    Item Down { get; }
    //}
    public interface IHasBelt { 
        bool HasBelt { get; }
    }


    public class Slot : MonoBehaviour
    {
        [Header("sprite")]
        [SerializeField] private Sprite Transparent;
        [SerializeField] private Sprite Concave;
        [SerializeField] private Sprite Convex;

        [Header("other")]
        [SerializeField]
        private Transform Trans;
        public Transform Transform => Trans;
        [SerializeField]
        private Image background;
        public Image Background => background;


        public void SetSlotBg(SlotBackgroundType bg) {
            switch(bg) {
                case SlotBackgroundType.Transparent:
                    background.sprite = Transparent;
                    button.interactable = false;
                    break;
                case SlotBackgroundType.Convex:
                    background.sprite = Convex;
                    break;
                case SlotBackgroundType.Concave:
                    background.sprite = Concave;
                    break;
                case SlotBackgroundType.Default:
                    break;
            }
        }


        [SerializeField]
        private Button button;
        public Button Button => button;

        public Color UIColor {
            set {
                background.color = value;
            }
        }



        [SerializeField]
        public Image Content;
        [SerializeField]
        public Image Container;
        [SerializeField]
        public Image Highlight;
        [SerializeField]
        public Image Decoration;

        [SerializeField]
        public RectTransform ContentRectTransform;
        [SerializeField]
        public RectTransform ContainerRectTransform;
        [SerializeField]
        public RectTransform HighlightRectTransform;
        [SerializeField]
        public RectTransform DecorationRectTransform;



        private bool hasSomething;

        public Action OnTap;
        public void Tap() {
            OnTap?.Invoke();
        }


        public void BounceContentTransform(float x, float y) {
            ContentRectTransform.localScale = new Vector3(x, y, 1);
            ContainerRectTransform.localScale = new Vector3(x, y, 1);
            HighlightRectTransform.localScale = new Vector3(x, y, 1);
        }
        public void BounceContentTransformClear() {
            ContentRectTransform.localScale = new Vector3(1, 1, 1);
            ContainerRectTransform.localScale = new Vector3(1, 1, 1);
            HighlightRectTransform.localScale = new Vector3(1, 1, 1);
        }


        public void RenderItem(Item item) {
            if (hasSomething) {
                Clear();
            }


            if (item != null) {

                float semi = item.SemiTransparent ? 0.25f : 1;

                string key;

                key = item.Content;
                if (key != null) {
                    Content.sprite = Res.I.SpriteOf(key);
                    Content.enabled = true;
                    Content.color = item.ContentColor.SetA(semi);
                }

                key = item.Container;
                if (key != null) {
                    Container.sprite = Res.I.SpriteOf(key);
                    Container.enabled = true;
                    Container.color = item.ContainerColor.SetA(semi);
                }

                key = item.Highlight;
                if (key != null) {
                    Highlight.sprite = Res.I.SpriteOf(key);
                    Highlight.enabled = true;
                    Highlight.color = item.HighlightColor.SetA(semi);
                }

                key = item.Decoration;
                if (key != null) {
                    Decoration.sprite = Res.I.SpriteOf(key);
                    Decoration.enabled = true;
                    // Decoration.color = item.DecorationColor.SetA(semi);
                    Decoration.color = item.DecorationColor;
                }

                hasSomething = true;
            }
        }

        private void Clear() {
            Content.sprite = null;
            Container.sprite = null;
            Highlight.sprite = null;
            Decoration.sprite = null;

            Content.enabled = false;
            Container.enabled = false;
            Highlight.enabled = false;
            Decoration.enabled = false;

            hasSomething = false;
        }
    }
}
