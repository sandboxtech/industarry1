
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace W
{
    public interface ITile
    {

    }

    public class Res : MonoBehaviour
    {
        private Dictionary<string, TextAsset> texts = new Dictionary<string, TextAsset>();

        private Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

        private Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();

        private Dictionary<string, TileBase> tiles = new Dictionary<string, TileBase>();


        public AudioClip ClipOf(string key) {
            if (!audioClips.TryGetValue(key, out AudioClip clip)) {
                A.Throw($"audioclip not found: “{key}”");
            }
            return clip;
        }

        public TileBase TileOf(string key) {
            if (key == null) return null;
            if (!tiles.TryGetValue(key, out TileBase tile)) {
                // A.Throw($"tile not found: “{key}”");
                return tiles[DefaultSprite.name];
            }
            return tile;
        }

        [SerializeField]
        public Sprite DefaultSprite;

        public Sprite SpriteOf(string key) {
            if (key == null) return null;
            if (!sprites.TryGetValue(key, out Sprite sprite)) {
                return DefaultSprite;
            }
            return sprite;
        }

        public TextAsset TextOf(string key) {
            if (key == null) return null;
            if (!texts.TryGetValue(key, out TextAsset text)) {
                A.Throw($"text not found: “{key}”");
            }
            return text;
        }


        public static Res I { get; private set; }

        private void Awake() {
            A.Assert(I == null);
            I = this;

            ReportSprite(DefaultSprite);
            ReportChild(transform);
        }

        /// <summary>
        /// 检测所有Config组件，并且在Res注册
        /// </summary>
        /// <param name="trans"></param>
        private void ReportChild(Transform trans) {
            Config config = trans.GetComponent<Config>();

            if (config != null) {
                config.Report();
            } else {
                A.Assert(trans.childCount > 0, () => $"Res 底下，“{trans.name}” 既没有config也没有子物体");
            }
            if (trans.childCount > 0) {
                foreach (Transform child in trans) {
                    ReportChild(child);
                }
            }
        }

        public void ReportSprites(Sprite[] sprites) {
            for (int i = 0; i < sprites.Length; i++) {
                ReportSprite(sprites[i]);
            }
        }

        public void ReportSprite(Sprite sprite) {
            A.Assert(sprite != null, () => $"sprite missing");
            string name = sprite.name;

            //if (sprites.ContainsKey(name)) {
            //    errors.Add(name);
            //    return;
            //}
            sprites.Add(name, sprite);

            SimpleTile tile = ScriptableObject.CreateInstance<SimpleTile>();
            tile.sprite = sprite;
            tiles.Add(name, tile);
        }


        public void ReportTextAssets(TextAsset[] texts) {
            for (int i = 0; i < texts.Length; i++) {
                ReportTextAsset(texts[i]);
            }
        }
        public void ReportTextAsset(TextAsset text) {
            A.Assert(text != null, () => $"text missing");
            string name = text.name;

            texts.Add(name, text);
        }

        //private HashSet<string> errors = new HashSet<string>();
        //private void Update() {
        //    if (errors.Count > 0) {
        //        foreach (var error in errors) {
        //            Debug.Log($"重复: {error}");
        //        }
        //    }
        //    else {
        //        Debug.Log("没问题了，注释掉这里");
        //    }
        //}

        public void ReportAnimatedTile(string name, Sprite inventoryOverride, Sprite[] frames, float speed, float speedMax, Func<float> speedGetter = null) {
            A.Assert(frames != null && frames.Length > 0 && speed >= 0.25f);

            AnimatedTile tile = ScriptableObject.CreateInstance<AnimatedTile>();

            sprites.Add(name, inventoryOverride == null ? frames[0] : inventoryOverride);

            //for (int i = 0; i < frames.Length; i++) {
            //    Sprite frame = frames[i];
            //    sprites.Add(frame.name, frame);
            //}
            ReportSprite(frames[0]);

            tile.sprites = frames;
            tile.speed = speed;
            tile.speedMax = speedMax;
            tile.speedGetter = speedGetter;

            tiles.Add(name, tile);
        }

        public void ReportAudioClips(AudioClip[] clips) {
            for (int i = 0; i < clips.Length; i++) {
                ReportAudioClip(clips[i]);
            }
        }

        public void ReportAudioClip(AudioClip clip) {
            audioClips.Add(clip.name, clip);
        }
    }
}
