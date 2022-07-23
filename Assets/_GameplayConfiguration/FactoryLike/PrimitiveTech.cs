
using System;
using System.Collections.Generic;

namespace W
{
    [recipe(typeof(copper_coin), 1, false, typeof(hut_fiber))] public interface buy_hut_fiber { }
    [recipe(typeof(copper_coin), 1, false, typeof(hut_fur))] public interface buy_hut_fur { }
    [recipe(typeof(copper_coin), 1, false, typeof(hut_stick))] public interface buy_hut_stick { }
    [recipe(typeof(copper_coin), 2, false, typeof(craftingplace))] public interface buy_craftingplace { }
    [recipe(typeof(silver_coin), 1, false, typeof(trading_post))] public interface buy_trading_post { }



    [recipe(typeof(fiber), 100, false, typeof(hut_fiber))]
    [impl(typeof(hut_fiber_))]
    [cn("稻草棚屋")] public interface hut_fiber : building { } // grass => this

    public class hut_fiber_ : MarketLike
    {
        public override Vec2 Size => new Vec2(3, 2);
        public override Color4 ContentColor => G.theme.DryFloraColor;
        public override bool CanEnterPart(Vec2 pos) => pos == new Vec2(1, 0);
        public override string Highlight => InProgress ? DefaultHighlight : null;
        public override Color4 HighlightColor => G.theme.BulbColor;



        public override List<Type> Recipes => Map.PlanetPosotion == planet_.HomePlanetPosition ? recipes_initial : recipes;
        private static List<Type> recipes_initial = new List<Type>() {
            typeof(sell_berry), // 最初的钱

            typeof(buy_hut_fiber), // 复制自己
            typeof(buy_hut_stick), // 矿业支线，赚钱
            typeof(buy_hut_fur), // 农业支线，赚钱
            typeof(buy_craftingplace), // 石头动力，手动
            typeof(buy_trading_post), // 主线
        };
        private static List<Type> recipes = new List<Type>() {
            typeof(buy_hut_fiber), // 复制自己
            typeof(buy_hut_stick), // 矿业支线，赚钱
            typeof(buy_hut_fur), // 农业支线，赚钱
            typeof(buy_craftingplace), // 石头动力，手动
            typeof(buy_trading_post), // 主线
        };

        protected override long Max => 5;
        protected override long Del => C.SimpleWaitTimespan;
    }




    [recipe(typeof(copper_coin), 1, false, typeof(mine))] public interface buy_mine { }

    [recipe(typeof(sand), 3, false, typeof(copper_coin))] public interface sell_sand { }
    [recipe(typeof(clay), 3, false, typeof(copper_coin))] public interface sell_clay { }
    [recipe(typeof(stone), 3, false, typeof(copper_coin))] public interface sell_stone { }
    [recipe(typeof(metal_ore), 1, false, typeof(silver_coin))] public interface sell_metal_ore { }
    [recipe(typeof(iron_ore), 1, false, typeof(silver_coin))] public interface sell_iron_ore { }
    [recipe(typeof(copper_ore), 1, false, typeof(silver_coin))] public interface sell_copper_ore { }
    [recipe(typeof(coal), 1, false, typeof(silver_coin))] public interface sell_coal { }



    [recipe(typeof(stick), 30, false, typeof(hut_stick))]
    [impl(typeof(hut_stick_))]
    [cn("木棍棚屋-矿业")] public interface hut_stick : building { } // branch => this
    public class hut_stick_ : MarketLike
    {
        public override Vec2 Size => new Vec2(2, 1);
        public override Color4 ContentColor => G.theme.WoodColor;
        public override string Highlight => InProgress ? DefaultHighlight : null;
        public override Color4 HighlightColor => G.theme.BulbColor;

        public static int count = 0;
        public override List<Type> Recipes {
            get {
                Type result;
                if (count % 2 == 0) {
                    // base
                    Type baseMineral = MapTerrain.GetBaseMineral();
                    if (baseMineral == typeof(sand)) {
                        result = typeof(sell_sand);
                    } else if (baseMineral == typeof(stone)) {
                        result = typeof(sell_stone);
                    } else if (baseMineral == typeof(clay)) {
                        result = typeof(sell_clay);
                    } else {
                        result = typeof(sell_stone);
                    }
                } else {
                    // special
                    Type baseMineral = MapTerrain.SpecialMineralOf(G.map.hashcode);
                    if (baseMineral == typeof(iron_ore)) {
                        result = typeof(sell_iron_ore);
                    } else if (baseMineral == typeof(copper_ore)) {
                        result = typeof(sell_copper_ore);
                    } else if (baseMineral == typeof(coal)) {
                        result = typeof(sell_coal);
                    }
                    else {
                        result = typeof(sell_stone);
                    }
                }
                recipes[0] = result;
                count++;
                return recipes;
            }
        }
        private static List<Type> recipes = new List<Type>() {
            // typeof(buy_mine),
            // typeof(sell_clay),
            typeof(sell_stone),
        };

        protected override long Max => 5;
        protected override long Del => C.SimpleWaitTimespan;
    }


    [recipe(typeof(wood), 1, false, typeof(stick), 2)]
    [cn("木棍")] public interface make_stick_from_wood { }
    [recipe(typeof(stone), 100, typeof(fiber), 10, false, typeof(mine), 1)]
    [cn("矿井")] public interface make_mine_from_stone { }



    [recipe(typeof(stone), 10, false, typeof(craftingplace))]
    [impl(typeof(craftingplace_))]
    [cn("手工台")] public interface craftingplace : building { }
    public class craftingplace_ : FactoryLike
    {
        public override List<Type> Recipes => recipes;
        private static List<Type> recipes = new List<Type>() {
            typeof(stick),
            typeof(make_stick_from_wood),
            //typeof(axe),
            //typeof(pickaxe),
            typeof(spade),
            //typeof(hoe),
            //typeof(scythe),
            typeof(hammer),
            typeof(craftingplace),
            typeof(hut_fiber),
            typeof(hut_stick),

            typeof(hearth),
            typeof(fishtrap),
            typeof(mine),
            typeof(make_mine_from_stone),
        };

        public override Color4 ContentColor => G.theme.StoneColor;
        public override string Container => InProgress ? DefaultContainer : DefaultContainerFirstFrame;


        protected override long Max => __TestLogic__.debug ? 3 : 30; // 30;
        protected override long Del => __TestLogic__.debug ? C.Second : C.Hour / Max;
    }


    [recipe(typeof(fiber), 20, false, typeof(heat_energy), 1)]
    [cn("热能 - 纤维")]
    public interface make_fire_from_fiber { }

    [recipe(typeof(branch), 1, false, typeof(heat_energy), 1)]
    [cn("热能 - 树枝")]
    public interface make_fire_from_branch { }

    [recipe(typeof(stick), 1, false, typeof(heat_energy), 1)]
    [cn("热能 - 木棍")]
    public interface make_fire_from_stick { }

    [recipe(typeof(wood), 1, false, typeof(heat_energy), 1)]
    [cn("热能 - 木头")]
    public interface make_fire_from_wood { }
    [recipe(typeof(charcoal), 1, false, typeof(heat_energy), 10)]
    [cn("热能 - 木炭")]
    public interface make_fire_from_charcoal { }

    [recipe(typeof(coal), 1, false, typeof(heat_energy), 10)]
    [cn("热能 - 煤炭")]
    public interface make_fire_from_coal { }


    [recipe(typeof(stone), 10, false, typeof(hearth))]
    [impl(typeof(hearth_))]
    [cn("篝火")] public interface hearth : building { }
    public class hearth_ : FactoryLike
    {
        public override List<Type> Recipes => recipes;
        private static List<Type> recipes = new List<Type>() {
            typeof(make_fire_from_fiber),
            typeof(make_fire_from_branch),
            // typeof(make_fire_from_stick),
            typeof(make_fire_from_wood),
            // typeof(make_fire_from_charcoal),
            typeof(berry_baked),
            typeof(fish_baked),
        };

        public override string Content => InProgress ? DefaultContent : DefaultContentIdle;
        public override Color4 ContentColor => G.theme.StoneColor;
        public override string Highlight => InProgress ? DefaultHighlight : null;

        protected override long Max => 10;
        protected override long Del => C.Minute * 10;
    }




    /// <summary>
    /// 科技
    /// </summary>
    [recipe(typeof(stone_brick), 1, false, typeof(sundial))]
    [impl(typeof(sundial_))]
    [cn("日晷")] public interface sundial : building { } // stone => this. => science
    public class sundial_ : FactoryLike
    {
        public override Color4 ContentColor => G.theme.StoneColor;
    }





    /// <summary>
    /// 食物
    /// </summary>
    [recipe(typeof(stick), 10, false, typeof(fishtrap))]
    [impl(typeof(fishtrap_))]
    [cn("捕鱼陷阱")] public interface fishtrap : building { } // branch => this
    public class fishtrap_ : FactoryLike
    {
        protected override bool DroppableAt(Vec2 pos) => G.terrain.bedrock[pos] == GroundType.sea;
        public override Type Recipe => typeof(fish);

        protected override bool DoTryCollect => true;
        public override Vec2 Size => new Vec2(2, 2);
        public override string Content => IdleData.Value >= 1 ? DefaultContent : DefaultContentIdle;
        public override Color4 ContentColor => G.theme.WoodColor;
        public override string Container => IdleData.Value >= 1 ? DefaultContainer : null;


        protected override long Max => 10;
        protected override long Del => C.Hour / Max;
    }












    // construction
    [impl(typeof(workbench_construction_))]
    [recipe(typeof(stick), 2, false, typeof(workbench_construction))]
    [cn("手工桌")] public interface workbench_construction : building { } // stone => this. stone => stonebrick, other

    public class workbench_construction_ : FactoryLike
    {

        public override List<Type> Recipes => recipes;
        private static List<Type> recipes = new List<Type>() {
            typeof(craftingplace),
            typeof(workbench_construction),
            null,
            typeof(trading_post),
            null,
            typeof(workbench_textile),
            typeof(workbench_ceramic),
            typeof(workbench_carpentry),
            typeof(workbench_masonry),
            typeof(workbench_alchemy),
            typeof(workbench_writing),
            typeof(workbench_electronics),
        };

        public override string Content => InProgress ? DefaultContent : DefaultContentFirstFrame;
        public override string Container => InProgress ? DefaultContainer : null;
        public override Color4 ContentColor => G.theme.WoodColor;
    }


}



