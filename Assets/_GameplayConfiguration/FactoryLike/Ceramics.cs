
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{

    [recipe(typeof(silver_coin), 3, false, typeof(clayfiber_residence))] public interface buy_clayfiber_residence { }
    [recipe(typeof(silver_coin), 3, false, typeof(clayfiber_workshop_clay))] public interface buy_clayfiber_workshop_clay { }
    [recipe(typeof(silver_coin), 3, false, typeof(clayfiber_workshop_fiber))] public interface buy_clayfiber_workshop_fiber { }

    [recipe(typeof(copper_coin), 1, false, typeof(silver_coin), 1)] public interface trade_copper_for_silver { }
    [recipe(typeof(silver_coin), 1, false, typeof(copper_coin), 30)] public interface trade_silver_for_copper { }



    [recipe(typeof(fiber), false, typeof(trading_post))]
    [impl(typeof(trading_post_))]
    [cn("贸易小屋")] public interface trading_post : building { }
    public class trading_post_ : MarketLike
    {
        public override Vec2 Size => new Vec2(3, 2);

        public override Color4 ContentColor => G.theme.DryFloraColor;
        public override string Container => DefaultContainer;


        public override List<Type> Recipes => recipes;
        private static List<Type> recipes = new List<Type>() {

            typeof(buy_clayfiber_residence),
            typeof(buy_clayfiber_workshop_fiber),
            typeof(buy_clayfiber_workshop_clay),
            typeof(buy_trading_post),

            typeof(buy_wood_library),

            typeof(trade_copper_for_silver),
            typeof(trade_silver_for_copper),
            typeof(buy_fiber_plant),
        };
    }





    [recipe(typeof(clay), 30, typeof(fiber), 30, false, typeof(clayfiber_residence))]
    [impl(typeof(clayfiber_residence_))]
    [cn("土草手工工坊")] public interface clayfiber_residence : building { }
    public class clayfiber_residence_ : FactoryLike
    {
        public override string Container => DefaultContainer;
        public override Color4 ContainerColor => G.theme.DryFloraColor;
        public override Color4 ContentColor => G.theme.ClayColor; public override Vec2 Size => new Vec2(3, 2);


        public override string Highlight => InProgress ? DefaultHighlight : null;
        public override Color4 HighlightColor => G.theme.BulbColor;


        public override List<Type> Recipes => recipes;
        private static List<Type> recipes = new List<Type>() {

            typeof(thread),
            typeof(textile),
            typeof(rope),
            typeof(yarn),
            // typeof(bandage),
            typeof(backpack),

            typeof(clay_brick),
            typeof(clay_pottery),
            typeof(clay_tile),
            typeof(clay_mould),
        };

        protected override long Max => 100;
        protected override long Del => C.Minute;
    }



    [recipe(typeof(clay), 100, false, typeof(clayfiber_workshop_clay))]
    [impl(typeof(clayfiber_workshop_clay_))]
    [cn("土草建筑工坊")] public interface clayfiber_workshop_clay : building { }
    public class clayfiber_workshop_clay_ : FactoryLike
    {
        public override Color4 ContentColor => G.theme.ClayColor; public override Vec2 Size => new Vec2(4, 2);

        public override string Highlight => InProgress ? DefaultHighlight : null;
        public override Color4 HighlightColor => G.theme.BulbColor;

        public override List<Type> Recipes => recipes;
        private static List<Type> recipes = new List<Type>() {
            typeof(fermentation_pot),
            typeof(clay_oven),
            typeof(clay_charcoal_klin),
            typeof(clay_klin),
            typeof(distillation_facility),

            // metal-working
            typeof(mortar), // 磨粉
            typeof(bloomary), // 冶炼
            typeof(furnance_blast), // 铸造
            typeof(anvil), // 铸造2

            typeof(clayfiber_residence),
            typeof(clayfiber_workshop_clay),
            typeof(clayfiber_workshop_fiber),
        };

        protected override long Max => 1;
        protected override long Del => 30 * C.Minute;
    }



    [recipe(typeof(thread), 70, false, typeof(gold_coin))] public interface sell_thread { }
    [recipe(typeof(textile), 50, false, typeof(gold_coin))] public interface sell_textile { }
    [recipe(typeof(rope), 50, false, typeof(gold_coin))] public interface sell_rope { }
    [recipe(typeof(yarn), 50, false, typeof(gold_coin))] public interface sell_yarn { }
    [recipe(typeof(bandage), 50, false, typeof(gold_coin))] public interface sell_bandage { }


    [recipe(typeof(claybaked_brick), 10, false, typeof(gold_coin))] public interface sell_claybaked_brick { }
    [recipe(typeof(claybaked_mould), 10, false, typeof(gold_coin))] public interface sell_claybaked_mould { }
    [recipe(typeof(claybaked_pottery), 10, false, typeof(gold_coin))] public interface sell_claybaked_pottery { }


    [recipe(typeof(metal), 6, false, typeof(gold_coin))] public interface sell_metal { }
    [recipe(typeof(metal_product), 3, false, typeof(gold_coin))] public interface sell_metal_product { }
    [recipe(typeof(nail), 1, false, typeof(gold_coin))] public interface sell_nail { }
    [recipe(typeof(gear), 1, false, typeof(gold_coin))] public interface sell_gear { }
    [recipe(typeof(screw), 1, false, typeof(gold_coin))] public interface sell_screw { }
    [recipe(typeof(capnut), 1, false, typeof(gold_coin))] public interface sell_capnut { }


    [recipe(typeof(clay_pottery_wine), 3, false, typeof(gold_coin))] public interface sell_clay_pottery_wine { }
    [recipe(typeof(clay_pottery_mead), 1, false, typeof(gold_coin))] public interface sell_clay_pottery_mead { }
    [recipe(typeof(fish_salted), 1, false, typeof(gold_coin))] public interface sell_fish_salted { }


    [recipe(typeof(claybaked_tile), 10, false, typeof(gold_coin))] public interface sell_claybaked_tile { }
    [recipe(typeof(clay), 30, typeof(fiber), 30, false, typeof(clayfiber_workshop_fiber))]
    [impl(typeof(clayfiber_workshop_fiber_))]
    [cn("土草商品市场")] public interface clayfiber_workshop_fiber : building { }
    public class clayfiber_workshop_fiber_ : MarketLike
    {
        public override string Container => DefaultContainer;
        public override Color4 ContainerColor => G.theme.DryFloraColor;
        public override Color4 ContentColor => G.theme.ClayColor; public override Vec2 Size => new Vec2(3, 2);

        public override string Highlight => InProgress ? DefaultHighlight : null;
        public override Color4 HighlightColor => G.theme.BulbColor;

        public override List<Type> Recipes => recipes;
        private static List<Type> recipes = new List<Type>() {

            typeof(sell_thread),
            typeof(sell_textile),
            typeof(sell_rope),
            typeof(sell_yarn),
            // typeof(sell_bandage),

            typeof(sell_metal),
            typeof(sell_metal_product),
            typeof(sell_nail),
            typeof(sell_gear),
            typeof(sell_screw),
            typeof(sell_capnut),

            typeof(sell_claybaked_brick),
            typeof(sell_claybaked_mould),
            typeof(sell_claybaked_pottery),
            typeof(sell_claybaked_tile),

            typeof(sell_clay_pottery_wine),
            typeof(sell_clay_pottery_mead),
            typeof(sell_fish_salted),

        };
    }



    [recipe(typeof(stone_brick), 10, false, typeof(mine))]
    [impl(typeof(mine_))]
    [cn("矿坑")] public interface mine : building { } // spade
    public class mine_ : FactoryLike
    {
        // public override Type Recipe => typeof(metal_ore);

        //public override List<Type> Recipes => recipes;
        //private static List<Type> recipes = new List<Type>() {
        //    typeof(clay),
        //    typeof(stone),
        //    typeof(metal_ore),
        //};

        public override Type Recipe => !OnMap ? null : recipeOf(_Pos);

        //private Type recipeOf(Vec2 pos) => G.map.HashcodeOfTile(pos) % MapTerrain.MineralRichnessOf(G.map.hashcode) == 0
        //    ? MapTerrain.SpecialMineralOf(G.map.hashcode) : MapTerrain.GetBaseMineral(); // recipes[(int)(G.map.HashcodeOfTile(pos) % recipes.Count)];

        private Type recipeOf(Vec2 pos) {
            long richness = MapTerrain.MineralRichnessOf(G.map.hashcode);
            Vec2 size = Size;
            for (int i = 0; i < size.x; i++) {
                for (int j = 0; j < size.y; j++) {
                    if (G.map.HashcodeOfTile(pos + new Vec2(i, j)) % richness == 0) {
                        return MapTerrain.SpecialMineralOf(G.map.hashcode);
                    }
                }
            }
            return MapTerrain.GetBaseMineral();
        }
        public Type mineralOf(Vec2 pos) {
            long richness = MapTerrain.MineralRichnessOf(G.map.hashcode);
            if (G.map.HashcodeOfTile(pos) % richness == 0) {
                return MapTerrain.SpecialMineralOf(G.map.hashcode);
            }
            return MapTerrain.GetBaseMineral();
        }

        public override void OnMove(Vec2 pos) {
            Type mineral = mineralOf(pos);
            Type baseMineral = MapTerrain.GetBaseMineral();
            if (mineral != baseMineral) {
                UI_Notice.I.Send($"脚下探测到 {Attr.CN(mineral)}", ItemPool.GetDef(mineral).SetQuantity(1));
            }
        }


        protected override bool DoTryCollect => true;

        public override Vec2 Size => new Vec2(2, 2);

        public override Color4 ContentColor => G.theme.StoneColor;
        public override string Container => InProgress ? DefaultContainer : DefaultContainerIdle;
        public override Color4 ContainerColor => G.theme.WoodColor;

        protected override long Max => 30;
        protected override long Del => C.Minute;
    }



    [impl(typeof(clay_oven_))]
    [recipe(typeof(clay), 20, false, typeof(clay_oven))]
    [cn("土火炉")] public interface clay_oven : building { } // clay => pottery
    public class clay_oven_ : FactoryLike
    {
        public override List<Type> Recipes => recipes;
        private static List<Type> recipes = new List<Type>() {
            typeof(make_fire_from_fiber),
            typeof(make_fire_from_branch),
            typeof(make_fire_from_wood),
            typeof(make_fire_from_charcoal),
            typeof(make_fire_from_coal),
        };

        public override string Content => InProgress ? DefaultContent : DefaultContentIdle;

        public override Color4 ContentColor => G.theme.ClayColor;
        public override string Highlight => InProgress ? DefaultHighlight : null;

        protected override long Max => 100;
        protected override long Del => 10 * C.Minute;
    }

    [impl(typeof(clay_charcoal_klin_))]
    [recipe(typeof(clay), 30, false, typeof(clay_charcoal_klin))]
    [cn("土炭窑")] public interface clay_charcoal_klin : building { } // wood => charcoal
    public class clay_charcoal_klin_ : FactoryLike
    {
        public override Vec2 Size => new Vec2(2, 2);

        //public override List<Type> Recipes => recipes;
        //private static List<Type> recipes = new List<Type>() {
        //    typeof(charcoal),
        //};
        public override Type Recipe => typeof(charcoal);

        public override string Content => InProgress ? DefaultContent : DefaultContentIdle;
        public override Color4 ContentColor => G.theme.ClayColor;


        protected override long Max => 30;
        protected override long Del => 10 * C.Minute;
    }



    /// <summary>
    /// 粘土类的冶炼，好用
    /// </summary>
    [impl(typeof(clay_klin_))]
    [recipe(typeof(clay), 50, false, typeof(clay_klin))]
    [cn("土窑")] public interface clay_klin : building { } // clay => brick
    public class clay_klin_ : FactoryLike
    {
        public override Vec2 Size => new Vec2(2, 2);
        public override List<Type> Recipes => recipes;
        private static List<Type> recipes = new List<Type>() {
            typeof(claybaked_brick),
            typeof(claybaked_mould),
            typeof(claybaked_tile),
            typeof(claybaked_pottery),
        };

        public override Color4 ContentColor => G.theme.ClayColor;
        public override string Highlight => InProgress ? DefaultHighlight : null;

        protected override long Max => 30;
        protected override long Del => 10 * C.Minute;
    }

    [recipe(typeof(pumpkin), false, typeof(sugar))]
    [cn("南瓜糖")] public interface make_sugar_from_pumpkin { }


    [recipe(typeof(clay), 10, false, typeof(fermentation_pot))]
    [impl(typeof(fermentation_pot_))]
    [cn("发酵陶罐")] public interface fermentation_pot : building { } // pottery => this
    public class fermentation_pot_ : FactoryLike
    {
        public override string Content => InProgress ? DefaultContent : DefaultContentIdle;
        public override Color4 ContentColor => G.theme.ClayColor;

        public override List<Type> Recipes => recipes;
        private static List<Type> recipes = new List<Type>() {
            typeof(clay_pottery_wine),
            typeof(clay_pottery_mead),
            typeof(fish_salted),

            typeof(make_sugar_from_pumpkin),
        };


        protected override long Max => 30;
        protected override long Del => C.Hour;
    }



    [recipe(typeof(clay_pottery_seawater), typeof(heat_energy), false, typeof(clay_pottery_water), typeof(salt))]
    [cn("海水蒸馏")] public interface make_clay_pottery_water_from_seawater { }



    [recipe(typeof(clay), 20, false, typeof(distillation_facility))]
    [impl(typeof(distillation_facility_))]
    [cn("蒸馏设备")] public interface distillation_facility : building { } // sea water => water
    public class distillation_facility_ : FactoryLike
    {
        public override Vec2 Size => new Vec2(2, 1);

        public override string Content => InProgress ? DefaultContent : DefaultContentIdle;
        public override Color4 ContentColor => G.theme.ClayColor;
        public override string Highlight => InProgress ? DefaultHighlight : null;

        public override List<Type> Recipes => recipes;
        private static List<Type> recipes = new List<Type>() {
            typeof(make_clay_pottery_water_from_seawater),
        };


        protected override long Max => 30;
        protected override long Del => 10 * C.Minute;
    }




    // metal tool and machine
    [impl(typeof(mortar_))]
    [recipe(typeof(stone), false, typeof(mortar))]
    [cn("磨钵")] public interface mortar : building { } // stone => this . ore => ore powder
    public class mortar_ : FactoryLike
    {
        //public override List<Type> Recipes => recipes;
        //private static List<Type> recipes = new List<Type>() {
        //    typeof(metal_ore_powder),
        //};

        public override List<Type> Recipes => recipes;
        private static List<Type> recipes = new List<Type>() {
            typeof(metal_ore_powder),
            typeof(iron_ore_powder),
            typeof(copper_ore_powder),
        };

        public override string Content => InProgress ? DefaultContent : DefaultContentFirstFrame;
        public override Color4 ContentColor => G.theme.StoneColor;


        protected override long Max => 24;
        protected override long Del => C.Day / Max;
    }


    [impl(typeof(bloomary_))]
    [recipe(typeof(clay), 100, false, typeof(bloomary))]
    [cn("土炼金炉")] public interface bloomary : building { } // clay => this . ore powder => ingot
    public class bloomary_ : FactoryLike
    {
        public override Vec2 Size => new Vec2(2, 2);

        public override List<Type> Recipes => recipes;
        private static List<Type> recipes = new List<Type>() {
            typeof(metal),
            typeof(iron),
            typeof(copper),
        };
        // public override Type Recipe => typeof(metal);

        public override Color4 ContentColor => G.theme.ClayColor;
        public override string Highlight => InProgress ? DefaultHighlight : null;

        protected override long Max => 24;
        protected override long Del => C.Day / Max;
    }

    [cn("铁器制作")]
    [recipe(typeof(iron), 1, typeof(claybaked_mould), 1, typeof(heat_energy), 1, false, typeof(metal_product), 1)]
    public interface make_metal_product_with_iron { }
    [cn("铜器制作")]
    [recipe(typeof(copper), 1, typeof(claybaked_mould), 1, typeof(heat_energy), 1, false, typeof(metal_product), 1)]
    public interface make_metal_product_with_copper { }


    [recipe(typeof(clay), 100, typeof(wood_tile), 10, false, typeof(furnance_blast))]
    [impl(typeof(furnance_blast_))]
    [cn("鼓风炉")] public interface furnance_blast : building { } // woodplank, stonebrick => this . ingot => misc
    public class furnance_blast_ : FactoryLike
    {
        public override List<Type> Recipes => recipes;
        private static List<Type> recipes = new List<Type>() {
            typeof(metal_product),
            typeof(make_metal_product_with_iron),
            typeof(make_metal_product_with_copper),
        };
        // public override Type Recipe => typeof(metal_product);


        public override Color4 ContentColor => G.theme.ClayColor;
        public override string Container => DefaultContainer;
        public override Color4 ContainerColor => G.theme.WoodColor;

        protected override long Max => 24;
        protected override long Del => C.Day / Max;
    }


    [recipe(typeof(metal), 100, false, typeof(anvil))]
    [impl(typeof(anvil_))]
    [cn("铁砧")] public interface anvil : building { } // metal => this
    public class anvil_ : FactoryLike
    {
        public override Color4 ContentColor => G.theme.MetalColor;

        public override List<Type> Recipes => recipes;
        private static List<Type> recipes = new List<Type>() {
            typeof(nail),
            typeof(gear),
            typeof(screw),
            typeof(capnut),
        };

        protected override long Max => 24;
        protected override long Del => C.Day / Max;
    }













    [impl(typeof(ceramic_workbench_))]
    [recipe(typeof(clay_brick), 10, false, typeof(workbench_ceramic))]
    [cn("陶艺工作台")] public interface workbench_ceramic : building { }
    public class ceramic_workbench_ : FactoryLike
    {
        public override Vec2 Size => new Vec2(2, 1);
        public override List<Type> Recipes => recipes;
        private static List<Type> recipes = new List<Type>() {
            typeof(clay_brick),
            typeof(clay_pottery),
            typeof(clay_tile),
            typeof(clay_mould),
        };

        public override string Content => InProgress ? DefaultContent : DefaultContentFirstFrame;
        public override Color4 ContentColor => G.theme.ClayColor;

        protected override long Max => 100;
        protected override long Del => C.Minute;
    }



}
