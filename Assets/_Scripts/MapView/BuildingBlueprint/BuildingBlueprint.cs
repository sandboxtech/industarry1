
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{
    public class BuildingBlueprint : MonoBehaviour
    {
        [SerializeField] private Transform Trans;
        public Transform Transform => Trans;

        [SerializeField] private SpriteRenderer Content;
        [SerializeField] private SpriteRenderer Container;
        [SerializeField] private SpriteRenderer Highlight;
        [SerializeField] private SpriteRenderer Decoration;



        private bool hasSomething;

        public void RenderItem(Item item, Color colorMul) {
            if (hasSomething) {
                Clear();
            }

            if (item != null) {

                string key;

                key = item.Content;
                if (key != null) {
                    Content.sprite = Res.I.SpriteOf(key);
                    Content.enabled = true;
                    Color color = item.ContentColor * colorMul;
                    color.a *= G.settings.blueprint_opacity;
                    Content.color = color;
                }

                key = item.Container;
                if (key != null) {
                    Container.sprite = Res.I.SpriteOf(key);
                    Container.enabled = true;
                    Color color = item.ContainerColor * colorMul;
                    color.a *= G.settings.blueprint_opacity;
                    Container.color = color;
                }

                key = item.Highlight;
                if (key != null) {
                    Highlight.sprite = Res.I.SpriteOf(key);
                    Highlight.enabled = true;
                    Color color = item.HighlightColor * colorMul;
                    color.a *= G.settings.blueprint_opacity;
                    Highlight.color = color;
                }

                key = item.Decoration;
                if (key != null) {
                    Decoration.sprite = Res.I.SpriteOf(key);
                    Decoration.enabled = true;
                    Color color = item.DecorationColor * colorMul;
                    color.a *= G.settings.blueprint_opacity;
                    Decoration.color = color;
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
