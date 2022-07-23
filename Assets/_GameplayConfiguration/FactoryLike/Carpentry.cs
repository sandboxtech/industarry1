
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{


    [recipe(typeof(gold_coin), 1, false, typeof(wood_residence))] public interface buy_wood_residence { }
    [recipe(typeof(gold_coin), 1, false, typeof(wood_library))] public interface buy_wood_library { }
    [recipe(typeof(gold_coin), 1, false, typeof(wood_warehouse))] public interface buy_wood_warehouse { }
    [recipe(typeof(gold_coin), 1, false, typeof(wood_workshop))] public interface buy_wood_workshop { }

    [impl(typeof(wood_library_))]
    [recipe(typeof(wood_tile), 100, typeof(claybaked_tile), 100, false, typeof(wood_library))]
    [cn("木制图书馆")] public interface wood_library : building { }
    public class wood_library_ : MarketLike
    {
        public override Color4 ContentColor => G.theme.WoodColor; public override Vec2 Size => new Vec2(3, 3);
        public override string Container => DefaultContainer;
        public override Color4 ContainerColor => G.theme.ClayBakedColor;
        public override string Highlight => InProgress ? DefaultHighlight : null;
        public override Color4 HighlightColor => G.theme.BulbColor;


        public override List<Type> Recipes => recipes;
        private static List<Type> recipes = new List<Type>() {
            typeof(buy_wood_residence),
            typeof(buy_wood_library),
            typeof(buy_wood_warehouse),
            typeof(buy_wood_workshop),

            typeof(buy_brick_library),
        };
    }




    [impl(typeof(wood_residence_))]
    [recipe(typeof(wood_tile), 100, typeof(claybaked_tile), 100, false, typeof(wood_residence))]
    [cn("木制手工工坊")] public interface wood_residence : building { }
    public class wood_residence_ : FactoryLike
    {
        public override Color4 ContentColor => G.theme.WoodColor; public override Vec2 Size => new Vec2(3, 2);
        public override string Container => DefaultContainer;

        public override Color4 ContainerColor => G.theme.ClayBakedColor;
        public override string Highlight => InProgress ? DefaultHighlight : null;
        public override Color4 HighlightColor => G.theme.BulbColor;


        public override List<Type> Recipes => recipes;
        private static List<Type> recipes = new List<Type>() {
            typeof(wood_tile),
            typeof(wood_brick),
            typeof(wood_door),

            typeof(wood_wheel),
            typeof(wood_barrel),
            typeof(wood_bucket),
            typeof(crate),
        };

        protected override long Max => 100;
        protected override long Del => 10 * C.Hour / Max;
    }


    [recipe(typeof(wood_tile), 1, false, typeof(gold_coin))] public interface sell_wood_tile { }
    [recipe(typeof(wood_brick), 1, false, typeof(gold_coin))] public interface sell_wood_brick { }
    [recipe(typeof(wood_wheel), 1, false, typeof(gold_coin))] public interface sell_wood_wheel { }
    [recipe(typeof(wood_barrel), 1, false, typeof(gold_coin))] public interface sell_wood_barrel { }
    [recipe(typeof(wood_bucket), 1, false, typeof(gold_coin))] public interface sell_wood_bucket { }


    [impl(typeof(wood_warehouse_))]
    [recipe(typeof(wood_tile), 100, typeof(claybaked_tile), 100, false, typeof(wood_warehouse))]
    [cn("木制商品市场")] public interface wood_warehouse : building { }
    public class wood_warehouse_ : MarketLike
    {
        public override Color4 ContentColor => G.theme.WoodColor; public override Vec2 Size => new Vec2(3, 2);
        public override string Container => DefaultContainer;
        public override Color4 ContainerColor => G.theme.ClayBakedColor;
        public override string Highlight => InProgress ? DefaultHighlight : null;
        public override Color4 HighlightColor => G.theme.BulbColor;


        public override List<Type> Recipes => recipes;
        private static List<Type> recipes = new List<Type>() {
            typeof(sell_wood_tile),
            typeof(sell_wood_brick),
            typeof(sell_wood_wheel),
            typeof(sell_wood_barrel),
            typeof(sell_wood_bucket),
        };

    }


    [impl(typeof(wood_workshop_))]
    [recipe(typeof(wood_tile), 100, typeof(claybaked_tile), 100, false, typeof(wood_workshop))]
    [cn("木制建筑工坊")] public interface wood_workshop : building { } // misc
    public class wood_workshop_ : FactoryLike
    {
        public override Color4 ContentColor => G.theme.WoodColor; public override Vec2 Size => new Vec2(3, 3);
        public override string Container => DefaultContainer;
        public override Color4 ContainerColor => G.theme.ClayBakedColor;
        public override string Highlight => InProgress ? DefaultHighlight : null;
        public override Color4 HighlightColor => G.theme.BulbColor;

        public override List<Type> Recipes => recipes;
        private static List<Type> recipes = new List<Type>() {
            typeof(wood_beehive),
            typeof(wood_hencoop),
            typeof(wood_cathouse),
            null,
            typeof(wood_warehouse),
            typeof(wood_library),
            typeof(wood_workshop),
            typeof(wood_residence),
        };

        protected override long Max => 100;
        protected override long Del => 10 * C.Hour / Max;
    }




    // ...............


    [recipe(typeof(wood_tile), false, typeof(crate))]
    [impl(typeof(crate_))]
    [cn("木板箱")]
    public interface crate { }
    public class crate_ : InventoryLike
    {
        public override Color4 ContentColor => G.theme.WoodColor;
        public override void OnEnter(Vec2 pos) {
            GridPage();
        }

        public override bool CanTap => false;
        public override bool Pickable => base.Pickable;

        public override void OnTap() {
            base.OnTap();
        }
    }





    [impl(typeof(wood_hencoop_))]
    [recipe(typeof(wood_tile), 100, false, typeof(wood_hencoop))]
    [cn("木制鸡窝")] public interface wood_hencoop : building { }
    public class wood_hencoop_ : FactoryLike { public override Color4 ContentColor => G.theme.WoodColor; public override Vec2 Size => new Vec2(3, 2);

        public override Type Recipe => typeof(egg);

        protected override long Max => 100;
        protected override long Del => 10 * C.Hour / Max;
    }

    [impl(typeof(wood_beehive_))]
    [recipe(typeof(wood_tile), 100, false, typeof(wood_beehive))]
    [cn("木制蜂巢")] public interface wood_beehive : building { }
    public class wood_beehive_ : FactoryLike { public override Color4 ContentColor => G.theme.WoodColor; public override Vec2 Size => new Vec2(2, 2);
        public override Type Recipe => typeof(honey);

        protected override long Max => 100;
        protected override long Del => 10 * C.Hour / Max;
    }

    [impl(typeof(wood_cathouse_))]
    [recipe(typeof(wood_tile), 100, false, typeof(wood_cathouse))]
    [cn("木制猫窝")] public interface wood_cathouse : building { }
    public class wood_cathouse_ : FactoryLike { public override Color4 ContentColor => G.theme.WoodColor; public override Vec2 Size => new Vec2(2, 2);
        public override Type Recipe => typeof(thread);

        protected override long Max => 100;
        protected override long Del => 10 * C.Hour / Max;
    }








    // dig carpentry
    [recipe(typeof(wood_tile), 100, false, typeof(workbench_carpentry))]
    [impl(typeof(carpentry_workbench_))]
    [cn("木工工作台")] public interface workbench_carpentry : building { } // wood => this. wood => woodplank, other
    public class carpentry_workbench_ : FactoryLike
    {
        public override Vec2 Size => new Vec2(2, 1);
        public override List<Type> Recipes => recipes;
        private static List<Type> recipes = new List<Type>() {
            typeof(wood_tile),
            typeof(wood_wheel),
            typeof(wood_barrel),
            typeof(wood_bucket),
            typeof(crate),
            null,
            typeof(wood_beehive),
            typeof(wood_hencoop),
            typeof(wood_cathouse),
            null,
            typeof(wood_residence),
            typeof(wood_workshop),
            typeof(wood_library),
            typeof(wood_warehouse),
        };

        public override string Content => InProgress ? DefaultContent : DefaultContentFirstFrame;
        public override Color4 ContentColor => G.theme.WoodColor;


        protected override long Max => 100;
        protected override long Del => 10 * C.Hour / Max;
    }
}
