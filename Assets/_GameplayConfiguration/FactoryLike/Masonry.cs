
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{

    [recipe(typeof(gold_coin), 3, false, typeof(brick_library))] public interface buy_brick_library { }
    [recipe(typeof(gold_coin), 3, false, typeof(brick_residence))] public interface buy_brick_residence { }
    [recipe(typeof(gold_coin), 3, false, typeof(brick_warehouse))] public interface buy_brick_warehouse { }
    [recipe(typeof(gold_coin), 3, false, typeof(brick_workshop))] public interface buy_brick_workshop { }
    [recipe(typeof(gold_coin), 20, false, typeof(workbench_electronics))] public interface buy_workbench_electronics { }


    [impl(typeof(brick_library_))]
    [recipe(typeof(stone_brick), typeof(claybaked_tile), false, typeof(brick_library))]
    [cn("石制市场")] public interface brick_library : building { }
    public class brick_library_ : MarketLike
    {
        public override Color4 ContentColor => G.theme.StoneColor;
        public override string Container => DefaultContainer;
        public override Color4 ContainerColor => G.theme.ClayBakedColor;

        public override string Highlight => InProgress ? DefaultHighlight : null;
        public override Color4 HighlightColor => G.theme.BulbColor;

        public override Vec2 Size => new Vec2(4, 3);

        public override List<Type> Recipes => recipes;
        private static List<Type> recipes = new List<Type>() {
            typeof(buy_brick_library),
            typeof(buy_brick_residence),
            typeof(buy_brick_warehouse),
            typeof(buy_brick_workshop),

            typeof(buy_workbench_electronics),
        };
    }


    [recipe(typeof(qic), 1, false, typeof(gold_coin))] public interface sell_qic { }
    [recipe(typeof(flour), 1, false, typeof(gold_coin))] public interface sell_flour { }
    [recipe(typeof(stone_brick), 1, false, typeof(gold_coin))] public interface sell_stone_brick { }
    [recipe(typeof(stone_tile), 1, false, typeof(gold_coin))] public interface sell_stone_tile { }
    [recipe(typeof(stone_wheel), 1, false, typeof(gold_coin))] public interface sell_stone_wheel { }


    [impl(typeof(brick_residence_))]
    [recipe(typeof(stone_brick), typeof(claybaked_tile), false, typeof(brick_residence))]
    [cn("石制商品市场")] public interface brick_residence : building { }
    public class brick_residence_ : MarketLike
    {
        public override Color4 ContentColor => G.theme.StoneColor;
        public override string Container => DefaultContainer;
        public override Color4 ContainerColor => G.theme.ClayBakedColor;

        public override string Highlight => InProgress ? DefaultHighlight : null;
        public override Color4 HighlightColor => G.theme.BulbColor;

        public override Vec2 Size => new Vec2(3, 3);


        public override List<Type> Recipes => recipes;
        private static List<Type> recipes = new List<Type>() {
            typeof(sell_qic),
            typeof(sell_wheat),
            typeof(sell_stone_brick),
            typeof(sell_stone_tile),
            typeof(sell_stone_wheel),
        };
    }

    [impl(typeof(brick_warehouse_))]
    [recipe(typeof(stone_brick), typeof(claybaked_tile), false, typeof(brick_warehouse))]
    [cn("石制手工工坊")] public interface brick_warehouse : building { }
    public class brick_warehouse_ : FactoryLike
    {
        public override Color4 ContentColor => G.theme.StoneColor;
        public override string Container => DefaultContainer;
        public override Color4 ContainerColor => G.theme.ClayBakedColor;

        public override string Highlight => InProgress ? DefaultHighlight : null;
        public override Color4 HighlightColor => G.theme.BulbColor;

        public override Vec2 Size => new Vec2(3, 3);


        public override List<Type> Recipes => recipes;
        private static List<Type> recipes = new List<Type>() {
            typeof(stone_brick),
            typeof(stone_tile),
            typeof(stone_wheel),
            typeof(stone_door),
        };
    }

    [impl(typeof(brick_workshop_))]
    [recipe(typeof(stone_brick), typeof(claybaked_tile), false, typeof(brick_workshop))]
    [cn("石制建筑工坊")] public interface brick_workshop : building { } // misc
    public class brick_workshop_ : FactoryLike
    {
        public override Color4 ContentColor => G.theme.StoneColor;
        public override string Container => DefaultContainer;
        public override Color4 ContainerColor => G.theme.ClayBakedColor;

        public override string Highlight => InProgress ? DefaultHighlight : null;
        public override Color4 HighlightColor => G.theme.BulbColor;

        public override Vec2 Size => new Vec2(4, 3);


        public override List<Type> Recipes => recipes;
        private static List<Type> recipes = new List<Type>() {
            typeof(workbench_cooking),
            typeof(well), // 水
            typeof(millstone), // 食物加工
            typeof(fireplace), // 食物
            typeof(cauldron), // 食物加工/化学加工
            //typeof(furnance_large),
            // typeof(mounument),
            typeof(fountain),
            typeof(cellar), // 类箱子

            typeof(windmill), // 机械能
            typeof(mechanical_millstone), // millstone高级

            // typeof(bell),
        };

        protected override long Max => 24;
        protected override long Del => C.Day / Max;
    }









    [impl(typeof(cellar_))]
    [recipe(typeof(stone_brick), 100, typeof(wood_tile), 100, false, typeof(cellar))]
    [cn("地窖")] public interface cellar : building { } // 
    public class cellar_ : InventoryLike, ILowOrder
    {
        protected override int Adder => 5;
        public override bool Destructable => list == null || list.occupied_slot_count == 0;

        public override Color4 ContentColor => G.theme.StoneColor;
        public override string Container => DefaultContainer;
        public override Color4 ContainerColor => G.theme.WoodColor;

        public override bool TryPickOnEnter => false;

        public override Vec2 Size => new Vec2(2, 2);
        public override bool CanEnter => true;
        public override bool CanEnterPart(Vec2 pos) => true;

        public override bool CanTap => true;
        public override void OnTap() {
            base.OnTap();
            GridPage();
        }
    }




    [impl(typeof(well_))]
    [recipe(typeof(stone_brick), 100, typeof(rope), 10, false, typeof(well))]
    [cn("水井")] public interface well : building { } // spade
    public class well_ : FactoryLike
    {
        // public override Type Recipe => typeof(water);
        public override Type Recipe => typeof(water);

        public override Vec2 Size => new Vec2(2, 1);

        public override Color4 ContentColor => G.theme.StoneColor;
        public override string Container => DefaultContainer;
        public override Color4 ContainerColor => G.theme.WoodColor;

        protected override long Max => 240;
        protected override long Del => C.Day / Max;
    }



    [impl(typeof(millstone_))]
    [recipe(typeof(stone_brick), 30, false, typeof(millstone))]
    [cn("石磨")] public interface millstone : building { } // stonebrick => this
    public class millstone_ : FactoryLike
    {
        public override string Content => InProgress ? DefaultContent : DefaultContentFirstFrame;
        public override Color4 ContentColor => G.theme.StoneColor;
        public override string Container => InProgress ? DefaultContent : DefaultContainerFirstFrame;
        public override Color4 ContainerColor => G.theme.WoodColor;

        protected override long Max => 240;
        protected override long Del => C.Day / Max;


        public override List<Type> Recipes => recipes;
        private static List<Type> recipes = new List<Type>() {
            typeof(flour),
            typeof(metal_ore_powder),
        };
    }

    [impl(typeof(fireplace_))]
    [recipe(typeof(stone_brick), 100, false, typeof(fireplace))]
    [cn("壁炉")] public interface fireplace : building { } // stonebrick => this
    public class fireplace_ : FactoryLike
    {
        public override Vec2 Size => new Vec2(2, 1);

        public override Color4 ContentColor => G.theme.StoneColor;
        public override string Highlight => DefaultHighlight;


        public override List<Type> Recipes => recipes;
        private static List<Type> recipes = new List<Type>() {
            typeof(berry_baked),
            typeof(fish_baked),

            typeof(dough_baked),
            typeof(donut)
        };

        protected override long Max => 240;
        protected override long Del => C.Day / Max;
    }


    [recipe(typeof(stone_brick), 10, false, typeof(workbench_cooking))]
    [impl(typeof(workbench_cooking_))]
    [cn("烹饪工作台")] public interface workbench_cooking : building { } // stone => this. stone => stonebrick, other
    public class workbench_cooking_ : FactoryLike
    {
        public override Vec2 Size => new Vec2(2, 1);
        public override List<Type> Recipes => recipes;
        private static List<Type> recipes = new List<Type>() {
            typeof(clay_pottery_water),
            typeof(dough),
        };

        public override string Content => InProgress ? DefaultContent : DefaultContentFirstFrame;
        public override Color4 ContentColor => G.theme.StoneColor;

        public override string Highlight => InProgress ? DefaultHighlight : null;
        public override Color4 HighlightColor => G.theme.BulbColor;


        protected override long Max => 180;
        protected override long Del => C.Day / Max;
    }

    [recipe(typeof(iron), 100, false, typeof(cauldron))]
    [impl(typeof(cauldron_))]
    [cn("大铁锅")] public interface cauldron : building { } // metal => this
    public class cauldron_ : FactoryLike
    {
        public override Vec2 Size => new Vec2(2, 2);

        public override Color4 ContentColor => G.theme.MetalColor;
        public override string Container => InProgress ? DefaultContainer : null;


        protected override long Max => 240;
        protected override long Del => C.Day / Max;
    }








    [impl(typeof(furnance_))]
    [recipe(typeof(stone_brick), 100, false, typeof(furnance_large))]
    [cn("大熔炉")] public interface furnance_large : building { }
    public class furnance_ : FactoryLike
    {
        public override Vec2 Size => new Vec2(3, 3);

        public override Color4 ContentColor => G.theme.StoneColor;
        public override string Highlight => DefaultHighlight;

        protected override long Max => 240;
        protected override long Del => C.Day / Max;
    }

    [impl(typeof(mounument_))]
    [recipe(typeof(stone_brick), 1, false, typeof(mounument))]
    [cn("纪念碑")] public interface mounument : building { }
    public class mounument_ : FactoryLike
    {
        public override Vec2 Size => new Vec2(2, 1);

        public override Color4 ContentColor => G.theme.StoneColor;
    }

    [impl(typeof(fountain_))]
    [recipe(typeof(stone_brick), 1, false, typeof(fountain))]
    [cn("喷泉")] public interface fountain : building { }
    public class fountain_ : FactoryLike
    {
        public override Color4 ContentColor => G.theme.StoneColor;
        public override Vec2 Size => new Vec2(3, 3);
        public override string Container => DefaultContainer;
        public override Color4 ContainerColor => G.map.theme.WaterColor;

        //public override List<Type> Recipes => recipes;
        //private static List<Type> recipes = new List<Type>() {
        //    typeof(clay_pottery_water),
        //};
        public override Type Recipe => typeof(clay_pottery_water);


        protected override long Max => 240;
        protected override long Del => C.Day / Max;
    }





    [impl(typeof(windmill_))]
    [recipe(typeof(wood_tile), 100, typeof(stone_brick), 30, false, typeof(windmill))]
    [output(typeof(kinematic_energy))]
    [cn("风车")] public interface windmill : building { } // misc
    public class windmill_ : FactoryLike
    {
        public override Vec2 Size => new Vec2(3, 2);

        public override string Content => InProgress ? DefaultContent : DefaultContentFirstFrame;
        public override Color4 ContentColor => G.theme.WoodColor;
        public override string Container => InProgress ? DefaultContainer : DefaultContainerFirstFrame;
        public override Color4 ContainerColor => G.theme.StoneColor;


        public override Type Recipe => typeof(kinematic_energy);


        protected override long Max => 240 * 3;
        protected override long Del => C.Hour / Max;
    }

    [recipe(typeof(metal), false, typeof(bell))]
    [impl(typeof(bell_))]
    [from_(typeof(metal_product))]
    [cn("钟铃")] public interface bell : building { } // metal => this
    public class bell_ : FactoryLike
    {
        public override Vec2 Size => new Vec2(3, 1);
        public override bool CanEnterPart(Vec2 pos) => pos.x == 1;
        public override Color4 ContentColor => G.theme.WoodColor;
        public override string Container => DefaultContainer;


        protected override long Max => 240;
        protected override long Del => C.Day / Max;
    }




    [cn("机械磨制面粉")]
    [recipe(typeof(wheat), 1, typeof(kinematic_energy), 1, false, typeof(flour), 1)]
    public interface make_flour_with_kinematic_energy { }

    [cn("机械碎矿")]
    [recipe(typeof(metal_ore), 1, typeof(kinematic_energy), 1, false, typeof(metal_ore_powder), 1)]
    public interface make_metal_powder_with_kinematic_energy { }


    [recipe(typeof(metal), 10, typeof(stone_brick), 30, false, typeof(mechanical_millstone))]
    [impl(typeof(mechanical_millstone_))]
    [cn("机械石磨")] public interface mechanical_millstone : building { }
    public class mechanical_millstone_ : FactoryLike
    {
        public override string Content => InProgress ? DefaultContent : DefaultContentFirstFrame;
        public override Color4 ContentColor => G.theme.StoneColor;
        public override string Container => InProgress ? DefaultContainer : DefaultContainerFirstFrame;
        public override Color4 ContainerColor => G.theme.LightMetalColor;


        protected override long Max => 240 * 3;
        protected override long Del => C.Hour / Max;


        public override List<Type> Recipes => recipes;
        private static List<Type> recipes = new List<Type>() {
            typeof(make_flour_with_kinematic_energy),
            typeof(make_metal_powder_with_kinematic_energy),
        };
    }










    // masonry

    [recipe(typeof(stone_brick), 10, false, typeof(workbench_masonry))]
    [impl(typeof(workbench_masonry_))]
    [cn("石工工作台")] public interface workbench_masonry : building { } // stone => this. stone => stonebrick, other
    public class workbench_masonry_ : FactoryLike
    {
        public override Vec2 Size => new Vec2(2, 1);
        public override List<Type> Recipes => recipes;
        private static List<Type> recipes = new List<Type>() {
            typeof(stone_brick),
            typeof(stone_wheel),

            typeof(cellar),
            typeof(well),
            typeof(millstone),
            typeof(fireplace),
            typeof(furnance_large),
            typeof(mounument),
            typeof(fountain),
        };

        public override string Content => InProgress ? DefaultContent : DefaultContentFirstFrame;
        public override Color4 ContentColor => G.theme.StoneColor;

        public override string Highlight => InProgress ? DefaultHighlight : null;
        public override Color4 HighlightColor => G.theme.BulbColor;


        protected override long Max => 180;
        protected override long Del => C.Day / Max;
    }

}
