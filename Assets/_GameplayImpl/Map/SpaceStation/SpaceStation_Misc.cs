
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{

    public abstract class StaticSpaceStationObject : Item
    {
        public override bool TryPickOnEnter => false;

        public override bool Absorbable => false;

        public override bool CanTap => true;
        public override void OnTap() {
            base.OnTap();
            // do nothing
        }
    }

    public abstract class StaticSpaceStationObject_MarketLike : MarketLike
    {
        public override bool TryPickOnEnter => false;

        public override bool Absorbable => false;

        public override bool CanEnter => false;
    }

   


    [impl(typeof(spacestation_engine_))]
    [recipe(false, typeof(spacestation_engine))]
    [cn("涡轮机")] public interface spacestation_engine : generator { }
    public class spacestation_engine_ : MarketLike
    {
        public override Vec2 Size => new Vec2(5, 5);
        public override Color4 ContentColor => G.theme.MetalColor;

        public override Type MarketRecipe => typeof(use_qic);

        protected override bool ExtraCondition => true; // 能加速
        protected override void ExtraAction() {
            base.ExtraAction();
        }
    }




    [impl(typeof(billboard_spacestation_))]
    [recipe(typeof(wood_tile), 1, false, typeof(billboard_spacestation))]
    [cn("空间站公告板")] public interface billboard_spacestation : building { }
    public class billboard_spacestation_ : StaticSpaceStationObject
    {
        public override Color4 ContentColor => G.theme.MetalColor;

        public override Vec2 Size => new Vec2(2, 1);

        public override void OnTap() {
            base.OnTap();
            Scroll.Show(
                Scroll.Text("当前  控制中心"),
                Scroll.Text("上方  飞船港口"),
                Scroll.Text("下方  引擎仓库"),
                Scroll.Text("左方  订单中心"),
                Scroll.Text("右方  科研中心"),

                Scroll.Text("右方  科研中心"),
                Scroll.Text($"{G.theme.QICColor.Dye("Q.I.C")}  量子智能方块"),
                Scroll.Empty
            );
        }
    }

    [recipe(false, typeof(qic))]
    public interface get_qic { }

    [recipe(typeof(qic), false)]
    public interface use_qic { }


    [impl(typeof(space_console_qic_))]
    [cn("Q.I.C 控制台")] public interface space_console_qic : building { }
    public class space_console_qic_ : StaticSpaceStationObject_MarketLike
    {
        public override Vec2 Size => new Vec2(2, 2);

        public override Color4 ContentColor => G.theme.MetalColor;
        public override string Container => DefaultContainer;

        public override Type MarketRecipe => typeof(get_qic);
        protected override bool ExtraCondition => true; // 能播放广告
    }



    //[impl(typeof(spacestation_radar_))]
    //[recipe(typeof(wood_plank), 1, false, typeof(billboard_spacestation))]
    //[cn("空间站雷达")] public interface spacestation_radar : building { }
    //public class spacestation_radar_ : StaticSpaceStationObject
    //{
    //    public override Color4 ContentColor => G.theme.MetalColor;

    //    public override Vec2 Size => new Vec2(1, 1);

    //    public override void OnTap() {
    //        base.OnTap();
    //        Work();
    //    }

    //    private void Work() {
    //        Type nextP = G.universe.age_max_i + 1 == Universe.ages.Count ? null : Universe.ages[G.universe.age_max_i + 1];

    //        Scroll.Show(
    //            Scroll.Button($"订单选择 {G.universe.age_current_i + 1} {Attr.CN(G.universe.age_current)}", TrySetOldPeriod),

    //            Scroll.Text($"当前时代 {G.universe.age_max_i + 1} {Attr.CN(G.universe.age_max)}"),
    //            nextP == null ? Scroll.Space : Scroll.Text($"下一时代 {G.universe.age_current_i + 2} {Attr.CN(nextP)}"),
    //            nextP == null ? Scroll.Space : Scroll.Button($"前进时代", () => {
    //                AdvanceAge();
    //                Work();
    //            }),
    //            Scroll.Space,

    //            Scroll.Empty
    //        );
    //    }

    //    private void AdvanceAge() {
    //        G.universe.age_max_i++;
    //        G.universe.age_current_i = G.universe.age_max_i;
    //    }

    //    private void TrySetOldPeriod() {
    //        List<Scroll> scrolls = new List<Scroll>();
    //        scrolls.Add(Scroll.ReturnButton(Work));

    //        scrolls.Add(Scroll.Text("可以接取旧时代的订单"));

    //        for (int _i = 0; _i <= G.universe.age_max_i; _i++) {
    //            int i = _i;
    //            Type period = Universe.ages[i];
    //            scrolls.Add(Scroll.Button($"{i + 1} {Attr.CN(Universe.ages[i])}", () => {
    //                G.universe.age_current_i = i;
    //                OnTap();
    //            }));
    //        }

    //        for (int _i = G.universe.age_max_i + 1; _i < Universe.ages.Count - 1; _i++) {
    //            int i = _i;
    //            Type period = Universe.ages[i];
    //            scrolls.Add(Scroll.Text($"{i + 1} {Attr.CN(Universe.ages[i])}"));
    //        }

    //        Scroll.Show(scrolls);
    //    }
    //}



}
