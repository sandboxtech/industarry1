
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{
    [impl(typeof(spaceship0_))]
    [cn("零号飞船")] public interface spaceship0 : spaceship { }

    [impl(typeof(spaceship1_))]
    [cn("初始飞船")] public interface spaceship1 : spaceship { }

    public class spaceship0_ : Item
    {
        public override bool TryPickOnEnter => false;
        public override string Content => "spaceship0";
        public override string Highlight => "spaceship0_highlight";
        public override Color4 ContentColor => G.theme.LightMetalColor;

        public override Vec2 Size => new Vec2(5, 2);

        public override bool Pickable => false;

        public override bool CanTap => true;

        public override void OnTap() {
            base.OnTap();
            Scroll.Show(
                Scroll.CloseButton,
                Scroll.Text("未修好")
            );
        }

    }


    public enum AdSelection
    {
        HourBonus,
        ChargeShip,
        CopperCoin10,
        SilverCoin3,
        GoldCoin1,
    }

    public class spaceship1_ : Item
    {

        public override void OnCreate() {
            base.OnCreate();
            IdleData = Idle.Create(1, 3, C.Minute); // 广告

            SpaceshipCharge = Idle.Create(ChargeMax / 2 * ChargeCost, ChargeMax * ChargeCost, C.Second, ChargeTimeEnergyPerSecond);
        }

        [JsonProperty] public Idle SpaceshipCharge;
        public const long ChargeTimeEnergyPerSecond = 100;
        public const long ChargeCost = C.Hour / C.Second * ChargeTimeEnergyPerSecond;
        public const long ChargeMax = 6;



        public override bool TryPickOnEnter => false;
        [JsonProperty] public bool Repaired { get; private set; }


        public override string Content => "spaceship1";
        public override string Highlight => "spaceship1_highlight";
        public override Color4 ContentColor => G.theme.LightMetalColor;

        public override Vec2 Size => new Vec2(4, 2);

        public override bool Pickable => true;

        public override bool CanTap => true;



        protected override void OnAddToMap() {
            base.OnAddToMap();
            SpaceshipCharge.Inc = (long)(ChargeTimeEnergyPerSecond * MapTerrain.PlanetTakeOffCostMultiplier(G.map.hashcode));
        }
        protected override void BeforeRemoveFromMap() {
            base.BeforeRemoveFromMap();
            SpaceshipCharge.Inc = 0;
        }

        public override void OnTap() {
            OnUse();
        }

        public override bool Destructable => true;

        public override void OnUse() {
            List<Scroll> scrolls = new List<Scroll>() {
            };

            long takeoffCost =  (long)(ChargeCost * MapTerrain.PlanetTakeOffCostMultiplier(G.map.hashcode));

            bool onMap = OnMap;

            if (onMap) {
                scrolls.Add(Scroll.Destruct(this));
            }

            // scrolls.Add(Scroll.Button("一分钟加速", () => G.universe.bonus_ticks += C.Minute));
            scrolls.Add(Scroll.Button("保存 <color=#ffffff66>游戏</color>", PlayerSettings.SavePage));
            scrolls.Add(Scroll.Button("<color=#ffffff66>观看</color> 广告", ViewAd));

            scrolls.Add(Scroll.Slot($"<color=#ffffff66>地名</color> {MapTerrain.Caculate_PlaceName(G.map.hashcode)}", this, null, SlotBackgroundType.Transparent));

            if (G.universe.bonus_ticks > 0) {
                scrolls.Add(Scroll.Text(() => $"额外时间 {Universe.TimespanDescription(G.universe.bonus_ticks)}"));
            }

            if (!onMap) {
                scrolls.Add(Scroll.Button(SpaceshipCharge.Value >= takeoffCost ? $"前往星空 <color=#ffff00ff>-{takeoffCost}</color>" : $"<color=#ffff00ff>能量</color>不足 {takeoffCost}", () => {
                    if (SpaceshipCharge.Value >= takeoffCost) {
                        SpaceshipCharge.Value -= takeoffCost;
                        Maps.GotoStarSystem();
                    }
                }));
            }

            scrolls.Add(Scroll.IdleTotalProgress(SpaceshipCharge, "<color=#ffff00cc>蓄电池</color> "));
            if (onMap) {
                scrolls.Add(Scroll.IdleProgress(SpaceshipCharge, "<color=#ffff00cc>太阳能</color> "));
            }

            // scrolls.Add(Scroll.Text($"当前世界坐标 {G.universe.planet_pos}"));

            Scroll.Show(scrolls);
        }

        private bool TryPlayAdCooldown() {
            bool result = IdleData.Value > 0;
            return result;
        }

        public const long CopperCount = 10;
        public const long SilverCount = 3;
        public const long GoldCount = 1;

        public void ViewAd() {
            bool noSpace = G.player.hand.NoSpace(1);

            long now = G.now;
            bool hourCreationTime = now - G.universe.creation_time < 3 * C.Hour;
            bool dayCreationTime = now - G.universe.creation_time < 2 * C.Day;

            List<Scroll> scrolls = new List<Scroll>() {
                Scroll.CloseButton,
                Scroll.Text("无广告"),
                //Scroll.Button("观看广告，世界加速1小时", () => {
                //    if (!TryPlayAdCooldown()) return;
                //    PangleAd.AdSuccessCallBack = () => {
                //        G.universe.bonus_ticks += C.Hour;
                //        IdleData.Value -= 1;
                //    };
                //    PangleAd.Instance.PlayerAdvertisement();
                //}),

                //Scroll.Button("观看广告，充满飞船电池", () => {
                //    if (!TryPlayAdCooldown()) return;
                //    PangleAd.AdSuccessCallBack = () => {
                //        SpaceshipCharge.Value = SpaceshipCharge.Max;
                //        IdleData.Value -= 1;
                //    };

                //    PangleAd.Instance.PlayerAdvertisement();
                //}),

                //Scroll.Progress(() => $"广告充能 {IdleData.Value} / {IdleData.Max}", () => IdleData.TotalProgress),

                //G.universe.bonus_ticks <= 0 ? Scroll.Empty : Scroll.Text(() => $"剩余加速时间 {Universe.TimespanDescription(G.universe.bonus_ticks)}"),
                //// Scroll.Button("一分钟加速", () => G.universe.bonus_ticks += C.Minute)

                //!noSpace ? Scroll.Empty : Scroll.Text("物品栏空间不足, 部分广告已禁用"),

                //Scroll.Space,

                //noSpace ? Scroll.Empty : Scroll.Slot(ItemPool.GetDef<copper_coin>().SetQuantity(CopperCount)),
                //noSpace ? Scroll.Empty : Scroll.Button($"观看广告，获取{CopperCount}铜币", () => {
                //    if (!TryPlayAdCooldown()) return;
                //    PangleAd.AdSuccessCallBack = () => {
                //        G.player.hand.TryAbsorbOrAdd(Item.Of<copper_coin>().SetQuantity(spaceship1_.CopperCount));
                //        IdleData.Value -= 1;
                //    };
                //    PangleAd.Instance.PlayerAdvertisement();
                //}),

                //noSpace || hourCreationTime? Scroll.Empty : Scroll.Slot(ItemPool.GetDef<silver_coin>().SetQuantity(SilverCount)),
                //noSpace || hourCreationTime? Scroll.Empty : Scroll.Button($"观看广告，获取{SilverCount}银币", () => {
                //    if (!TryPlayAdCooldown()) return;
                //    PangleAd.AdSuccessCallBack = () => {
                //        G.player.hand.TryAbsorbOrAdd(Item.Of<silver_coin>().SetQuantity(spaceship1_.SilverCount));
                //        IdleData.Value -= 1;
                //    };
                //    PangleAd.Instance.PlayerAdvertisement();
                //}),

                //noSpace || dayCreationTime? Scroll.Empty : Scroll.Slot(ItemPool.GetDef<gold_coin>().SetQuantity(GoldCount)),
                //noSpace || dayCreationTime? Scroll.Empty : Scroll.Button($"观看广告，获取{GoldCount}金币", () => {
                //    if (!TryPlayAdCooldown()) return;
                //    PangleAd.AdSuccessCallBack = () => {
                //        G.player.hand.TryAbsorbOrAdd(Item.Of<gold_coin>().SetQuantity(GoldCount));
                //        IdleData.Value -= 1;
                //    };
                //    PangleAd.Instance.PlayerAdvertisement();
                //}),
            };

            Scroll.Show(scrolls);
        }
    }

}



