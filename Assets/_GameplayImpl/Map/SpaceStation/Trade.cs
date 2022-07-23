
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{

    //public static class Trade
    //{

    //    /// <summary>
    //    /// 卖工坊、市场
    //    /// </summary>
    //    public static Dictionary<Type, List<Type>> sellers;
    //    /// <summary>
    //    /// 买产品
    //    /// </summary>
    //    public static Dictionary<Type, List<Type>> buyers;

    //    static Trade() {
    //        sellers = new Dictionary<Type, List<Type>>();

    //        buyers = new Dictionary<Type, List<Type>>();

    //        // 时代1 工具
    //        sellers.Add(typeof(tool_period), new List<Type>() {
    //            typeof(buy_axe),
    //            typeof(buy_pickaxe),
    //            typeof(buy_spade),
    //            typeof(buy_scythe),
    //            typeof(buy_hoe),
    //            typeof(buy_hammer),
    //        });

    //        buyers.Add(typeof(tool_period), new List<Type>() {
    //            typeof(sell_berry),
    //            typeof(sell_branch),
    //        });


    //        // 时代2 小屋
    //        sellers.Add(typeof(hut_period), new List<Type>() {
    //            typeof(buy_craftingplace), // 再生工具。sellers of last age
    //            typeof(buy_hut_fur), // 再生浆果
    //            typeof(buy_hut_stick), // 再生树木
    //            typeof(buy_hut_fiber), // 再生苇草
    //        });

    //        buyers.Add(typeof(hut_period), new List<Type>() {
    //            typeof(sell_stick),
    //            typeof(sell_wood),
    //            typeof(sell_wood_plank),
    //        });


    //        // 时代3 苇草粘土建筑
    //        sellers.Add(typeof(clay_period), new List<Type>() {
    //            typeof(buy_hearth), // 热能，木炭，烤浆果、烤鱼，石砖、土砖
    //            typeof(buy_clayfiber_workshop_fiber),
    //            typeof(buy_clayfiber_workshop_clay),
    //            typeof(buy_clayfiber_market),
    //        });

    //        buyers.Add(typeof(clay_period), new List<Type>() {
    //            typeof(sell_charcoal),
    //            typeof(sell_claybaked_brick),
    //            typeof(sell_claybaked_pottery),
    //        });


    //        // 时代4 木房
    //        sellers.Add(typeof(wood_period), new List<Type>() {
    //            // buy wood_xxx
    //        });

    //        buyers.Add(typeof(wood_period), new List<Type>() {
    //            typeof(sell_wood_plank),
    //        });


    //        // 时代4 石房
    //        sellers.Add(typeof(stone_period), new List<Type>() {
    //            // buy brick_xxx
    //        });

    //        buyers.Add(typeof(stone_period), new List<Type>() {
    //            typeof(sell_stone_brick),
    //        });
    //    }
    //}





    [impl(typeof(space_console_seller_))]
    [cn("出售-控制台")] public interface space_console_seller : building { }
    public class space_console_seller_ : StaticSpaceStationObject_MarketLike
    {
        public override Vec2 Size => new Vec2(2, 2);

        public override Color4 ContentColor => G.theme.MetalColor;
        public override string Container => DefaultContainer;

        // protected override List<Type> MarketRecipes => Trade.sellers[G.universe.age_current];
    }



    [impl(typeof(space_console_buyer_))]
    [cn("购买-控制台")] public interface space_console_buyer : building { }
    public class space_console_buyer_ : StaticSpaceStationObject_MarketLike
    {
        public override Vec2 Size => new Vec2(2, 2);

        public override Color4 ContentColor => G.theme.MetalColor;
        public override string Container => DefaultContainer;

        // protected override List<Type> MarketRecipes => Trade.buyers[G.universe.age_current];
    }



}
