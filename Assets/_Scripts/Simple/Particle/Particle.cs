
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{

    public partial class Particle : MonoBehaviour
    {
        public static Particle I { get; private set; }
        private void Awake() {
            A.Assert(I == null);
            I = this;
        }

        [SerializeField]
        private Transform Content;

        [SerializeField]
        private ParticlePrefab Prefab;
        private List<ParticlePrefab> Pool = new List<ParticlePrefab>(16);

        private ParticlePrefab GetFromPool() {

            ParticlePrefab particle;
            if (Pool.Count == 0) {
                particle = Instantiate(Prefab, Content);
            } else {
                particle = Pool[Pool.Count - 1];
                Pool.RemoveAt(Pool.Count - 1);
            }
            return particle;
        }
        private void ReturnToTool(ParticlePrefab particle) {
            Pool.Add(particle);
        }


        public static void Create(Vec2 pos, string key, long duration, MapView.OrderInLayer sortingOrder = MapView.OrderInLayer.Indicator, bool singleton = false, bool isGlow = false)
            => I._Create(pos, key, duration, Color.white, sortingOrder, singleton, isGlow);
        public static void Create(Vec2 pos, string key, long duration, Color color, MapView.OrderInLayer sortingOrder = MapView.OrderInLayer.Indicator, bool singleton = false, bool isGlow = false)
            => I._Create(pos, key, duration, color, sortingOrder, singleton, isGlow);

        private void _Create(Vec2 pos, string key, long duration, Color color, MapView.OrderInLayer sortingOrder = MapView.OrderInLayer.Indicator, bool singleton = false, bool isGlow = false) {
            AnimatedTile tile = Res.I.TileOf(key) as AnimatedTile;
            A.Assert(tile != null);

            ParticlePrefab particle = singleton ? singletons[key] : GetFromPool();
            long now = G.now;

            if (singleton) {
                particle.name = key;
                if (!singletonStartTime.ContainsKey(key)) singletonStartTime.Add(key, 0);
                singletonStartTime[key] = now;
            }

            particle.Trans.position = new Vector3(pos.x, pos.y, 0);
            particle.Renderer.sprite = tile.sprites[0];
            particle.Renderer.color = color;
            particle.Renderer.sortingOrder = (int)sortingOrder;
            particle.Renderer.sharedMaterial = isGlow ? MapView.I.HueshiftGlowMaterial : MapView.I.HueshiftMaterial;

            Tween.Add(duration, (float t) => {
                if (t >= 1) {
                    particle.Renderer.sprite = null;
                    particle.gameObject.SetActive(false);
                    if (!singleton) {
                        // 非单例则共享对象池
                        ReturnToTool(particle);
                    }
                    return true;
                } else {
                    if (singleton && singletonStartTime[key] != now) {
                        // 第二次被创建，之前的动画要停止
                        return false;
                    }
                    int index = (int)(t * tile.sprites.Length);
                    particle.Renderer.sprite = tile.sprites[index];
                    particle.gameObject.SetActive(true);
                }
                return true;
            });
        }

        private static DictionaryWithBuffer<string, ParticlePrefab> singletons = new DictionaryWithBuffer<string, ParticlePrefab>((string key) => Instantiate(I.Prefab, I.Content));
        private static Dictionary<string, long> singletonStartTime = new Dictionary<string, long>();

        public static void ClearSingleton(string key) {
            if (!singletonStartTime.ContainsKey(key)) {
                singletonStartTime.Add(key, G.now);
            } else {
                singletonStartTime[key] = G.now;
            }
        }

    }
}
