
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{
    public class StarSystem : MapTerrain
    {
        private bool OutOfRange(int i, int j, int mid, int r) {
            int dx = i - mid;
            int dy = j - mid;
            return dx * dx + dy * dy >= r * r;
        }

        public override Vec2 Portal => planet_.HomePlanetPosition;

        public override bool NoPlayerAvatar => true;
        public override void AfterCreate() {
            base.AfterCreate();

            int mid = M.Min(G.map.size.x, G.map.size.y) / 2;
            int radius = mid - 2;

            Vec2 center = new Vec2(mid, mid);
            Vec2 size = G.map.size;


            // 小行星
            for (int j = 0; j < size.y; j++) {
                for (int i = 0; i < size.x; i++) {
                    if (!OutOfRange(i, j, mid, radius) && OutOfRange(i, j, mid, radius - 4)) {
                        G.terrain.bedrock[i, j] = GroundType.star;
                    } else if (!OutOfRange(i, j, mid, radius / 2) && OutOfRange(i, j, mid, radius / 2 - 2)) {
                        G.terrain.bedrock[i, j] = GroundType.star;
                    }
                }
            }

            for (int j = Margin; j < size.y - Margin; j++) {
                for (int i = Margin; i < size.x - Margin; i++) {
                    if (OutOfRange(i, j, mid, radius)) continue; // 
                    if (!OutOfRange(i, j, mid, 3)) continue; // too close to the star

                    Vec2 pos = new Vec2(i, j);
                    if (G.map.HashcodeOfTile(pos) % Prime._31 == 0 && NothingNearby(pos)) {
                        planet_ planet = Item.Of<planet>() as planet_;
                        if (planet == null) throw new Exception();

                        planet.Distance = (pos - center).Magnitude / radius;
                        G.map[pos] = planet;
                    }
                }
            }
            G.map[planet_.HomePlanetPosition] = Item.Of<planet>();

            G.map[center] = Item.Of<star>();
        }
        private bool NothingNearby(Vec2 pos) {
            if (G.map[pos + Vec2.left] != null) return false;
            if (G.map[pos + Vec2.right] != null) return false;
            if (G.map[pos + Vec2.up] != null) return false;
            if (G.map[pos + Vec2.down] != null) return false;
            return true;
        }
    }


    [impl(typeof(planet_))]
    [cn("星球")]
    public interface planet { }
    public class planet_ : Item
    {
        public static Vec2 HomePlanetPosition => new Vec2(20, 20);

        public override bool CanTap => true;

        public bool IsDeserted => OnMap && G.map.HashcodeOfTile(_Pos) % 3 != 0;



        public override void OnTap() {
            base.OnTap();


            string mapKey = Maps.PlanetKeyOf(_Pos);
            uint hashcode = Map.HashcodeOfMapKey(mapKey);
            Type bush = MapTerrain.SpecialBushOf(hashcode);
            Type wood = MapTerrain.SpecialTreeOf(hashcode);
            Type mineral = MapTerrain.SpecialMineralOf(hashcode);

            string planetKey = Maps.PlanetKeyOf(_Pos);
            uint planetHashcode = Map.HashcodeOfMapKey(planetKey);

            string planetName = MapTerrain.Caculate_PlaceName(hashcode);

            if (IsDeserted) {
                Scroll.Show(
                    Scroll.Button($"离开", Scroll.Close),
                    Scroll.Slot($"不宜居星球暂未开放 {planetName}", this, null, SlotBackgroundType.Transparent),
                    Scroll.Text($"不宜居星球暂未开放")
                );
            } else {
                Scroll.Show(

                    Scroll.CloseButton,
                    Scroll.Button("扫描星球", ScanPlanet),
                    Scroll.Button("进入星球", EnterPlanet),
                    Scroll.Slot(planetName, this, null, SlotBackgroundType.Transparent),

                    Scroll.Empty
                );
            }
        }
        private void EnterPlanet() {
            Maps.GotoPlanet(_Pos);
        }

        private void ScanPlanet() {
            string mapKey = Maps.PlanetKeyOf(_Pos);
            uint hashcode = Map.HashcodeOfMapKey(mapKey);
            Type bush = MapTerrain.SpecialBushOf(hashcode);
            Type wood = MapTerrain.SpecialTreeOf(hashcode);
            Type mineral = MapTerrain.SpecialMineralOf(hashcode);

            string planetKey = Maps.PlanetKeyOf(_Pos);
            uint planetHashcode = Map.HashcodeOfMapKey(planetKey);

            string planetName = MapTerrain.Caculate_PlaceName(hashcode);

            Scroll.Show(
                Scroll.ReturnButton(OnTap),
                Scroll.Slot(Item.Of(bush), SlotBackgroundType.Transparent),
                Scroll.Slot(Item.Of(wood), SlotBackgroundType.Transparent),
                Scroll.Slot(Item.Of(mineral), SlotBackgroundType.Transparent),

                // Scroll.Slot(ItemPool.GetDef(flora), SlotBackgroundType.Transparent),

                Scroll.Text($"星球重力 {M.ToPercent(MapTerrain.PlanetTakeOffCostMultiplier(hashcode))}%"),
                Scroll.Text($"太阳能效率 {M.ToPercent(MapTerrain.PlanetSolarPanelEfficiency(hashcode))}%"),

                Scroll.Slot(planetName, this, null, SlotBackgroundType.Transparent),

                Scroll.Empty
            );
        }

        public override Color4 ContentColor => _Pos == HomePlanetPosition ? Color4.white : IsDeserted ? TemporatureMapColorOfDistance(Distance) : Color4.green_light;

        [SerializeField] public float Distance = 0.5f;
        public static Color4 TemporatureMapColorOfDistance(float t) => Color4.Lerp(Color.grey, Color4.Lerp(Color4.red_light, Color4.blue_light, t), 1 - 2 * (t - 0.5f) * (t - 0.5f));
    }

    [impl(typeof(star_))]
    [cn("恒星")]
    public interface star { }
    public class star_ : Item
    {
        public static Vec2 ThePosition => new Vec2(20, 20);

        public override Vec2 Size => new Vec2(2, 2);

        public override bool CanTap => true;

        public override void OnTap() {
            base.OnTap();
            Scroll.Show(
                Scroll.Text("暂无恒星系开发能力"),
                Scroll.Text("暂无恒星际旅行能力"),
                Scroll.Empty
            );
        }
        private void EnterPlanet() {
            Maps.GotoPlanet(_Pos);
        }

        public override Color4 ContentColor => Color4.orange;
    }


}
