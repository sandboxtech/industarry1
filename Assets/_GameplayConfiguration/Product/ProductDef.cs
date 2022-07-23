
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{
    [cn("手工艺品")] public interface handcraft { }
    [cn("产品")] public interface product { }






    // clay

    [cn("陶瓷")] public interface cremics : mineral { }



    [from_(typeof(clay))]
    [recipe(typeof(clay), false, typeof(clay_brick))]
    [impl(typeof(clay_brick_))]
    [cn("生土砖")] public interface clay_brick : handcraft { }
    public class clay_brick_ : Item { 
        public override Color4 ContentColor => G.theme.ClayColor; 
    
    }

    [recipe(typeof(clay), false, typeof(clay_tile))]
    [impl(typeof(clay_tile_))]
    [cn("生陶片")] public interface clay_tile : mineral { }
    public class clay_tile_ : Item { public override Color4 ContentColor => G.theme.ClayColor; }


    [recipe(typeof(clay), false, typeof(clay_mould))]
    [impl(typeof(clay_mould_))]
    [cn("生粘土模具")] public interface clay_mould : product { } // for physics
    public class clay_mould_ : Item { public override Color4 ContentColor => G.theme.ClayColor; }

    [recipe(typeof(clay), false, typeof(clay_pottery))]
    [impl(typeof(clay_pottery_))]
    [cn("生陶罐")] public interface clay_pottery : mineral { }
    public class clay_pottery_ : Item { public override Color4 ContentColor => G.theme.ClayColor; }





    [from_(typeof(clay))]
    [recipe(typeof(clay_brick), typeof(heat_energy), false, typeof(claybaked_brick))]
    [impl(typeof(claybaked_brick_))]
    [cn("陶砖")] public interface claybaked_brick : handcraft { }
    public class claybaked_brick_ : BuildableItem { public override Color4 ContentColor => G.theme.ClayBakedColor;

        protected override GroundType BuildableType => GroundType.claybaked_brick_wall;
        protected override AudioClip[] BuildableSound => Audio.I.Stone;
    }
    [recipe(typeof(clay_tile), typeof(heat_energy), false, typeof(claybaked_tile))]
    [impl(typeof(claybaked_tile_))]
    [cn("陶片")] public interface claybaked_tile : mineral { }
    public class claybaked_tile_ : BuildableItem { public override Color4 ContentColor => G.theme.ClayBakedColor;
        protected override GroundType BuildableType => GroundType.claybaked_tile_floor;
        protected override AudioClip[] BuildableSound => Audio.I.Stone;
    }



    [recipe(typeof(clay_mould), typeof(heat_energy), false, typeof(claybaked_mould))]
    [impl(typeof(claybaked_mould_))]
    [cn("粘土模具")] public interface claybaked_mould : product { } // for physics
    public class claybaked_mould_ : Item { public override Color4 ContentColor => G.theme.ClayBakedColor; }

    [recipe(typeof(clay_pottery), typeof(heat_energy), false, typeof(claybaked_pottery))]
    [impl(typeof(claybaked_pottery_))]
    [cn("陶罐")] public interface claybaked_pottery : mineral { }
    public class claybaked_pottery_ : Item { 
        public override Color4 ContentColor => G.theme.ClayBakedColor;
        public override void TryMove(Vec2 pos) {
            base.TryMove(pos);
            if (G.terrain.bedrock[pos + G.player.direction] == GroundType.sea) {
                G.player.HandItem = Of<clay_pottery_seawater>().SetQuantity(Quantity);
            }
        }
    }






    [recipe(typeof(claybaked_pottery), false, typeof(clay_pottery_seawater))]
    [impl(typeof(clay_pottery_seawater_))]
    [cn("海水陶罐")] public interface clay_pottery_seawater { }
    public class clay_pottery_seawater_ : clay_pottery_stuffed
    {
        public override Color4 HighlightColor => G.theme.WaterColor;
    }

    [recipe(typeof(claybaked_pottery), typeof(water), false, typeof(clay_pottery_water))]
    [impl(typeof(clay_pottery_water_))]
    [cn("淡水陶罐")] public interface clay_pottery_water { }
    public class clay_pottery_water_ : clay_pottery_stuffed
    {
        public override Color4 HighlightColor => G.theme.WaterColor;
    }




    // wood

    // carpentry
    [impl(typeof(wood_tile_))]
    [recipe(typeof(wood), 1, false, typeof(wood_tile), 1)]
    [cn("木板")] public interface wood_tile : handcraft { }
    public class wood_tile_ : BuildableItem
    { public override Color4 ContentColor => G.theme.WoodColor;
        protected override GroundType BuildableType => GroundType.wood_tile_floor;
        protected override AudioClip[] BuildableSound => Audio.I.Wood;
    }

    [impl(typeof(wood_brick_))]
    [recipe(typeof(wood), 1, false, typeof(wood_brick), 1)]
    [cn("木砖")] public interface wood_brick : handcraft { }
    public class wood_brick_ : BuildableItem
    {
        public override Color4 ContentColor => G.theme.WoodColor;
        protected override GroundType BuildableType => GroundType.wood_brick_wall;
        protected override AudioClip[] BuildableSound => Audio.I.Wood;
    }



    [impl(typeof(wood_wheel_))]
    [recipe(typeof(wood), 1, false, typeof(wood_wheel), 1)]
    [cn("木制车轮")] public interface wood_wheel : handcraft { }
    public class wood_wheel_ : Item { public override Color4 ContentColor => G.theme.WoodColor; }

    [impl(typeof(barrel_))]
    [recipe(typeof(wood_tile), 1, false, typeof(wood_barrel), 1)]
    [cn("木制酒桶")] public interface wood_barrel : handcraft { }
    public class barrel_ : Item { public override Color4 ContentColor => G.theme.WoodColor; }

    [impl(typeof(bucket_))]
    [recipe(typeof(wood_tile), 1, false, typeof(wood_bucket), 1)]
    [cn("木制水桶")] public interface wood_bucket : handcraft { }
    public class bucket_ : Item { public override Color4 ContentColor => G.theme.WoodColor; }







    // mansory
    [impl(typeof(stone_brick_))]
    [recipe(typeof(stone), 1, false, typeof(stone_brick), 1)]
    [cn("石砖")] public interface stone_brick : handcraft { }
    public class stone_brick_ : BuildableItem
    { 
        public override Color4 ContentColor => G.theme.StoneColor;

        protected override GroundType BuildableType => GroundType.stone_brick_wall;
        protected override AudioClip[] BuildableSound => Audio.I.Stone;
    }


    [impl(typeof(stone_tile_))]
    [recipe(typeof(stone), 1, false, typeof(stone_tile), 1)]
    [cn("石板")] public interface stone_tile : handcraft { }
    public class stone_tile_ : BuildableItem
    {
        public override Color4 ContentColor => G.theme.StoneColor;

        protected override GroundType BuildableType => GroundType.stone_tile_floor;
        protected override AudioClip[] BuildableSound => Audio.I.Stone;
    }



    [impl(typeof(stone_wheel_))]
    [recipe(typeof(stone), 1, false, typeof(stone_wheel), 1)]
    [cn("石制车轮")] public interface stone_wheel : handcraft { }
    public class stone_wheel_ : Item { public override Color4 ContentColor => G.theme.StoneColor; }

    // metal
    [recipe(typeof(metal), 1, typeof(claybaked_mould), 1, typeof(heat_energy), 1, false, typeof(metal_product), 1)]
    [cn("金属产品")] public interface metal_product : metal { }



    // other
    [cn("药水")] public interface potion : handcraft { }



    [cn("预制件")] public interface prefabrication : product { }
    [cn("轻质材料")] public interface light_material : product { }
    [cn("管道段")] public interface pipe_segment : product { }

    [cn("轮胎")] public interface tyre : handcraft { }

    [cn("炸弹")] public interface bomb : handcraft { }

    [cn("显示器")] public interface screen : appliance { }
    [cn("收音机")] public interface radioset : appliance { }
    [cn("音响")] public interface speaker : product { }
    [cn("灯泡")] public interface bulb : product { }

    [cn("冲锋枪")] public interface submachinegun : product { }

    [from_(typeof(electronics_component))]
    [cn("风扇")] public interface fan : handcraft { }








    [cn("消毒剂")] public interface disinfectant : product { } // 氯氧化物
    [cn("炸药")] public interface dynamite : product { } // 硝化有机物
    [cn("化肥")] public interface fertilizer : product { } // 氮磷钾
    [cn("药物")] public interface medicine : product { }
    [cn("染料")] public interface dye : product { }
    [cn("推进剂")] public interface propellant : product { }


    [cn("聚合材料")] public interface polymer : mineral { }
    [cn("塑料")] public interface plastic : polymer { }
    [cn("酚醛塑料")] public interface bakelite : plastic { }
    [cn("合成纤维")] public interface synthetic_fiber : polymer { }
    [cn("合成橡胶")] public interface synthetic_rubber : plastic { }
    [cn("硫化橡胶")] public interface vulcanizate : synthetic_rubber { }


    [cn("纳米材料")] public interface nanomaterial : polymer { }
    [cn("瓷金")] public interface ceramometal : plastic { }
    [cn("塑钢")] public interface plasteel : plastic { }
    [cn("中子材料")] public interface neutronium : plastic { }

}
