
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{


    [cn("材料")] public interface material { }

    [cn("Q.I.C")] public interface qic : mineral { }



    [impl(typeof(stone_))]
    [cn("石头")] public interface stone : mineral { } // 拾取 挖山
    public class stone_ : BuildableItem
    {
        public override Color4 ContentColor => G.theme.StoneColor;

        protected override GroundType BuildableType => GroundType.hill_stone;
        protected override AudioClip[] BuildableSound => Audio.I.Stone;
    }


    [impl(typeof(clay_))]
    [cn("粘土")] public interface clay : mineral { } // 挖山
    public class clay_ : BuildableItem
    {
        public override Color4 ContentColor => G.theme.ClayColor;
        protected override GroundType BuildableType => GroundType.hill_clay;
        protected override AudioClip[] BuildableSound => Audio.I.Gravel;
    }

    [cn("砂子")] public interface sand : mineral { }



    [impl(typeof(wood_))]
    [recipe(false, typeof(wood))]
    [cn("木材")] public interface wood : flora_product { }
    public class wood_ : Item { public override Color4 ContentColor => G.theme.WoodColor; }


    [impl(typeof(branch_))]
    [recipe(false, typeof(branch))]
    [cn("树枝")] public interface branch : flora_product { }
    public class branch_ : Item { public override Color4 ContentColor => G.theme.WoodColor; }


    [impl(typeof(stick_))]
    [recipe(typeof(branch), false, typeof(stick))]
    [cn("木棍")] public interface stick : flora_product { }
    public class stick_ : Item { public override Color4 ContentColor => G.theme.WoodColor; }


    [recipe(false, typeof(fiber))]
    [impl(typeof(fiber_))]
    [cn("苇草")] public interface fiber : flora_product { }
    public class fiber_ : BuildableItem
    {
        public override Color4 ContentColor => G.theme.DryFloraColor;

        protected override GroundType BuildableType => GroundType.grassland;
        protected override AudioClip[] BuildableSound => Audio.I.Grass;
    }



    // 无机
    [cn("有机物")] public interface organics : material { }
    [cn("矿物")] public interface mineral : material { }
    [cn("化学品")] public interface chemicals : material { }


    // 有机
    [cn("食物")] public interface food : organics { }
    [cn("食材")] public interface ingredient : organics { }




    // 燃料
    [cn("燃料")] public interface fuel : mineral { }




    [recipe(typeof(wood), typeof(heat_energy), false, typeof(charcoal))]
    [cn("木炭")] public interface charcoal : fuel { }
    [cn("煤炭")] public interface coal : mineral, fuel { }
    [cn("天然气")] public interface natural_gas : mineral, fuel { }
    [cn("原油")] public interface crude_oil : mineral, fuel { }
    [cn("生物燃油")] public interface biofuel : fuel, product { }
    [cn("汽油")] public interface light_oil : fuel, product { }
    [cn("柴油")] public interface heavy_oil : fuel, product { }



    // 非金属矿物
    [cn("泥土")] public interface dirt : mineral { }
    [cn("宝石")] public interface gem : shape3d { }
    [cn("钻石")] public interface diamond : gem { }

    // 非金属初级产品
    [cn("瓷器")] public interface porcelain : mineral { }
    [cn("硅晶")] public interface silicon_crystal : mineral { }
    [cn("玻璃")] public interface glass : mineral { }


    // 草木灰，硝酸钾
    [cn("灰烬")] public interface ash : mineral { }
    [cn("火药")] public interface gunpowder : mineral { }





    // 金属
    [impl(typeof(metal_ore_))]
    [recipe(false, typeof(metal_ore))]
    [cn("金属矿")] public interface metal_ore : mineral { }
    public class metal_ore_ : Item
    {
        public override Type ContentTypeRedirect => typeof(metal_ore);
        public override Color4 ContentColor => G.theme.StoneColor;
        public override string Container => DefaultContainer;
        public override Color4 ContainerColor => G.theme.LightMetalColor;
    }


    [impl(typeof(metal_ore_powder_))]
    [recipe(typeof(metal_ore), false, typeof(metal_ore_powder))]
    [cn("金属矿粉")] public interface metal_ore_powder : metal_ore { }
    public class metal_ore_powder_ : Item
    {
        public override Type ContentTypeRedirect => typeof(metal_ore_powder);
        public override Color4 ContentColor => G.theme.StoneColor;
        public override string Container => DefaultContainer;
        public override Color4 ContainerColor => G.theme.LightMetalColor;
    }

    [impl(typeof(metal_))]
    [recipe(typeof(metal_ore_powder), 1, typeof(heat_energy), 1, false, typeof(metal), 1)]
    [cn("金属")] public interface metal : mineral { }
    public class metal_ : Item
    {
        public override Type ContentTypeRedirect => typeof(metal);
        public override Color4 ContentColor => G.theme.StoneColor;
    }




    // 矿石子类
    [recipe(typeof(iron_ore), 1, typeof(heat_energy), 1, false, typeof(metal), 1)]
    [impl(typeof(iron_))]
    [cn("铁")] public interface iron : metal { }
    public class iron_ : metal_
    {
        public override Color4 ContentColor => G.theme.MetalColor;
    }

    [impl(typeof(iron_ore_powder_))]
    [recipe(typeof(iron_ore), false, typeof(iron_ore_powder))]
    [cn("铁矿粉")] public interface iron_ore_powder : metal_ore { }
    public class iron_ore_powder_ : metal_ore_powder_
    {
        public override Color4 ContainerColor => G.theme.IronOreColor;
    }

    [impl(typeof(iron_ore_))]
    [cn("铁矿")] public interface iron_ore : metal_ore { }
    public class iron_ore_ : metal_ore_
    {
        public override Color4 ContainerColor => G.theme.IronOreColor;
    }


    [recipe(typeof(copper_ore), 1, typeof(heat_energy), 1, false, typeof(metal), 1)]
    [impl(typeof(copper_))]
    [cn("铜")] public interface copper : metal { }
    public class copper_ : metal_
    {
        public override Color4 ContentColor => G.theme.CopperColor;
    }

    [impl(typeof(copper_ore_powder_))]
    [recipe(typeof(copper_ore), false, typeof(copper_ore_powder))]
    [cn("铜矿粉")] public interface copper_ore_powder : metal_ore { }
    public class copper_ore_powder_ : metal_ore_powder_
    {
        public override Color4 ContainerColor => G.theme.CopperOreColor;
    }

    [impl(typeof(copper_ore_))]
    [cn("铜矿")] public interface copper_ore : metal_ore { }
    public class copper_ore_ : metal_ore_ {
        public override Color4 ContainerColor => G.theme.CopperOreColor;
    }





    [impl(typeof(titanium_ore_))]
    [cn("钛矿")] public interface titanium_ore : metal_ore { }
    public class titanium_ore_ : metal_ore_ { }



    // 金属子类
    [cn("钛")] public interface titanium : metal { }

    [cn("贵金属")] public interface precious_metal : metal { }
    [cn("金")] public interface gold : precious_metal { }
    [cn("银")] public interface silver : precious_metal { }
    [cn("锡")] public interface tin : precious_metal { }

    // 放射元素子类
    [cn("放射核素")] public interface radionuclide : metal { }
    [cn("铀")] public interface uranium : radionuclide { }
    [cn("钚")] public interface plutonium : radionuclide { }
    [cn("钍")] public interface thorium : radionuclide { }
    [cn("镭")] public interface radium : radionuclide { }



    // 化工，化合物，混合物

    [cn("磁铁")] public interface magnet : metal { }
    [cn("合金")] public interface alloy : metal { }
    [cn("钢铁")] public interface steel : alloy { }
    [cn("青铜")] public interface bronze : alloy { }
    [cn("黄铜")] public interface brass : alloy { }


    // 化合物
    [cn("化合物")] public interface compound { }
    [cn("固体化合物")] public interface solid_compound : compound { }
    [cn("流体化合物")] public interface fluid_compound : compound { }
    [cn("气态化合物")] public interface gas_compound : compound { }
    [cn("液态化合物")] public interface liquid_compound : compound { }
    [cn("流体化合物")] public interface solution : compound { }

    [impl(typeof(water_))]
    [recipe(false, typeof(water))]
    [cn("水")] public interface water : liquid_compound { }
    public class water_ : Item
    {
        public override Color4 ContentColor => G.theme.WaterColor;
    }


    [cn("糖")] public interface sugar : solid_compound, mineral { }


    [cn("盐")] public interface salt : solid_compound, mineral { }
    [cn("酸")] public interface acid : solution, mineral { }
    [cn("碱")] public interface alkali : solution, mineral { }
    [cn("氢气")] public interface hydrogen : gas_compound { }
    [cn("氧气")] public interface oxygen : gas_compound { }
    [cn("氮气")] public interface nitrogen : gas_compound { }
    [cn("氯气")] public interface chlorine : gas_compound { }
    [cn("氨")] public interface ammonia : gas_compound { }
    [cn("苏打")] public interface soda : solid_compound { } // 碳酸钠

    [cn("烧碱")] public interface caustic_soda : alkali { } // 氢氧化钠
    [cn("硫酸")] public interface sulphuric_acid : acid { }
    [cn("盐酸")] public interface hydrochloric_acid : acid { }
    [cn("硝酸")] public interface nitric_acid : acid { }


}
