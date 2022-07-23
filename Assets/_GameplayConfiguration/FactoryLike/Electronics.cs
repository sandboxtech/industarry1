
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W {

    [impl(typeof(workbench_electronics_))]
    [recipe(typeof(metal), 1, false, typeof(workbench_electronics))]
    [cn("电子工作台")] public interface workbench_electronics : building { }
    public class workbench_electronics_ : FactoryLike
    {
        public override Vec2 Size => new Vec2(2, 1);
        public override List<Type> Recipes => null;
        private static List<Type> recipes = new List<Type>() {
            typeof(radar_array),
            typeof(wind_turbine),
            typeof(spacestation_engine),
            typeof(telescope),

            typeof(machine_air_separator),
            typeof(machine_cement),
            typeof(machine_chemicals),
            typeof(machine_plastic),
            typeof(machine_engine),
        };

        public override Scroll ExtraUseScroll => Scroll.Text("预计2022年上半年开放功能");
    }


    [impl(typeof(radar_array_))]
    [recipe(false, typeof(radar_array))]
    [cn("雷达阵列")] public interface radar_array : building { }
    public class radar_array_ : FactoryLike
    {
        public override Vec2 Size => new Vec2(5, 3);
        public override bool CanEnterPart(Vec2 pos) {
            return pos == new Vec2(0, 2) || pos == new Vec2(4, 2);
        }
        public override Color4 ContentColor => G.theme.MetalColor;
    }


    [impl(typeof(wind_turbine_))]
    [recipe(false, typeof(wind_turbine))]
    [cn("风力发电机")] public interface wind_turbine : generator { }
    public class wind_turbine_ : FactoryLike
    {
        public override Color4 ContentColor => G.theme.MetalColor;
    }





    [impl(typeof(telescope_))]
    [recipe(false, typeof(telescope))]
    [cn("望远镜")] public interface telescope : generator { }
    public class telescope_ : FactoryLike
    {
        public override Color4 ContentColor => G.theme.MetalColor;
    }

    [impl(typeof(machine_cement_))]
    [recipe(false, typeof(machine_cement))]
    [cn("水泥合成机")] public interface machine_cement : generator { }
    public class machine_cement_ : FactoryLike
    {
        public override Vec2 Size => new Vec2(2, 2);
        public override Color4 ContentColor => G.theme.MetalColor;
    }

    [impl(typeof(machine_chemicals_))]
    [recipe(false, typeof(machine_chemicals))]
    [cn("化学合成机")] public interface machine_chemicals : generator { }
    public class machine_chemicals_ : FactoryLike
    {
        public override Vec2 Size => new Vec2(2, 2);
        public override Color4 ContentColor => G.theme.MetalColor;
    }

    [impl(typeof(machine_plastic_))]
    [recipe(false, typeof(machine_plastic))]
    [cn("塑料合成机")] public interface machine_plastic : generator { }
    public class machine_plastic_ : FactoryLike
    {
        public override Vec2 Size => new Vec2(2, 2);
        public override Color4 ContentColor => G.theme.MetalColor;
    }

    [impl(typeof(machine_tool_digital_))]
    [recipe(false, typeof(machine_engine))]
    [cn("太空引擎")] public interface machine_engine : generator { }
    public class machine_tool_digital_ : FactoryLike
    {
        public override Vec2 Size => new Vec2(4, 5);
        public override Color4 ContentColor => G.theme.MetalColor;
    }


    [impl(typeof(machine_air_separator_))]
    [recipe(false, typeof(machine_air_separator))]
    [cn("空气分离机")] public interface machine_air_separator : generator { }
    public class machine_air_separator_ : FactoryLike
    {
        public override Vec2 Size => new Vec2(2, 2);
        public override Color4 ContentColor => G.theme.MetalColor;
    }


    [cn("打印机")] public interface printer : handcraft { }
    [cn("烟囱")] public interface chimney : building { }
    [cn("电池阵列")] public interface battery_array : building { }
    [cn("水池")] public interface pound : building { }
    [cn("水塔")] public interface water_tank_tower : building { }
    [cn("信号塔")] public interface signal_tower : building { }
    [cn("压缩机")] public interface refrigerator : building { }


    [cn("火箭")] public interface rocket : building { }
    [cn("星球登陆器")] public interface planet_lander : building { }
}

