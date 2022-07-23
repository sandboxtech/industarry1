
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{

    [from_(typeof(stone))]
    [cn("雕刻")] public interface carving { } // for knowledge

    [from_(typeof(fur))]
    [cn("卷轴")] public interface scroll { }





    [cn("笔")] public interface pen { }

    [from_(typeof(feather))]
    [cn("羽毛笔")] public interface quill : pen { }
    [from_(typeof(charcoal))]
    [cn("墨水")] public interface ink { }

    [from_(typeof(charcoal))]
    [cn("铅笔")] public interface pencil : pen { }
    [from_(typeof(wood))]

    [recipe(typeof(fiber), false, typeof(paper))]
    [cn("纸张")] public interface paper { }



    [cn("测绘工具")] public interface measurement { } // for geography

    [from_(typeof(magnet))]
    [cn("指南针")] public interface compass : measurement { } // for geography
    [from_(typeof(metal))]
    [cn("尺子")] public interface ruler : measurement { } // for market and construction
    [from_(typeof(metal))]
    [cn("天平")] public interface balance : measurement { }
    [from_(typeof(wood))]
    [cn("算盘")] public interface abacus : measurement { } // for mathematics & informatics



    // 机械
    [from_(typeof(metal))]
    [cn("零件")] public interface component_simple : metal_product { }

    [recipe(typeof(metal_product), false, typeof(nail))]
    [cn("钉子")] public interface nail : component_simple { }
    [recipe(typeof(metal_product), false, typeof(gear))]
    [cn("齿轮")] public interface gear : component_simple { }
    [recipe(typeof(metal_product), false, typeof(screw))]
    [cn("螺丝")] public interface screw : component_simple { }
    [recipe(typeof(metal_product), false, typeof(capnut))]
    [cn("螺母")] public interface capnut : component_simple { }


    // 计算

    [from_(typeof(component_simple), typeof(machine_simple))]
    [cn("简易机械")] public interface machine_simple : product { }
    [cn("机械钟表")] public interface mechanical_clock : machine_simple { }
    [cn("差分机")] public interface difference_engine : machine_simple { }
    [cn("打字机")] public interface typewriter { }





    // 观察
    [from_(typeof(glass))]
    [cn("玻璃制品")] public interface glassware : product { } // for physics

    [cn("沙漏")] public interface hourglass : glassware { } // for physics
    [cn("曲颈瓶")] public interface retort : glassware { } // for chemistry
    [cn("烧杯")] public interface beaker : glassware { }
    [cn("试管")] public interface cuvette : glassware { }
    [cn("棱锥")] public interface prism_glass : glassware { } // for optics
    [cn("透镜")] public interface lens : glassware { } // build scope

    [cn("显微镜")] public interface microscope : glassware, machine_simple { } // for biology





    // 物流
    [from_(typeof(component_simple))]
    [cn("机械组件")] public interface component : product { }

    [cn("活塞")] public interface piston : component { }
    [cn("热机")] public interface combustion_engine : component { }


    [cn("线圈")] public interface coil : component { }
    [cn("转子")] public interface rotor : component { }
    [cn("静子")] public interface stator : component { }
    [cn("电机")] public interface electric_motor : component { } // for transporation


    [from_(typeof(component))]
    [cn("重型机械")] public interface machine_complex : product { }

    [cn("车轮")] public interface wheel : handcraft { }
    [cn("推车")] public interface cart : handcraft { }
    [cn("马车")] public interface carriage : handcraft { }
    [cn("火车")] public interface train : machine_complex { }
    [cn("汽车")] public interface car : machine_complex { }
    [cn("飞船")] public interface spaceship : machine_complex { }
    [cn("空间站")] public interface space_station : machine_complex { }
    [cn("动力外骨骼")] public interface exoskeletons : machine_complex { }

    [cn("采矿无人机")] public interface mining_drone : machine_complex { }
    [cn("星核开采机")] public interface deepcore_miner : machine_complex { }
    [cn("气态巨行星提取机")] public interface gasgaint_extractor : machine_complex { }
    [cn("中子星开采机")] public interface neutrostar_miner : machine_complex { }
    [cn("黑洞开采机")] public interface blackhole_miner : machine_complex { }
    [cn("戴森环")] public interface dyson_rine : machine_complex { }
    [cn("戴森球")] public interface dyson_sphere : machine_complex { }
    [cn("盖亚机器")] public interface gaia_machine : machine_complex { }


    // 电力
    [from_(typeof(metal))]
    [cn("金属导线")] public interface wire : product { }
    [from_(typeof(rubber), typeof(crystal))]
    [cn("光缆")] public interface optical_cable : product { }
    [from_(typeof(rubber), typeof(wire))]
    [cn("电缆")] public interface electricity_cable : product { }


    [cn("雷达组件")] public interface radar_component : product { }
    [cn("电动机械")] public interface electric_machine : product { }
    [cn("电报机")] public interface telegraph : electric_machine { }
    [cn("家电")] public interface appliance : electric_machine { }




    // 化工 信息
    [from_(typeof(plastic), typeof(chemicals), typeof(magnet))]
    [cn("信息载体")] public interface record : product { }

    [cn("胶卷")] public interface film : record { }
    [cn("磁带")] public interface tape : record { }
    [cn("唱片")] public interface disk : record { }
    [cn("软盘")] public interface soft_disc : record { }
    [cn("硬盘")] public interface hard_disc : record { }



    // 电子，软件，智能
    [from_(typeof(chemicals), typeof(plastic), typeof(component))]
    [cn("电子元件")] public interface electronics_component : product { }

    [cn("电感元件")] public interface inductance : electronics_component { }
    [cn("电容元件")] public interface capacitance : electronics_component { }
    [cn("电阻元件")] public interface resistance : electronics_component { }
    [cn("电路板")] public interface circuitboard : electronics_component { } // for electronics

    [cn("计算器")] public interface caculator : electronics_component { } // for electronics
    [cn("编码器")] public interface encoder : electronics_component { }
    [cn("解码器")] public interface decoder : electronics_component { }
    [cn("随机器")] public interface hasher : electronics_component { }
    [cn("路由器")] public interface router : electronics_component { }



    [from_(typeof(silicon_crystal), typeof(electronics_component))]
    [cn("芯片")] public interface chip : electronics_component { }
    [cn("内存")] public interface memory : chip { }
    [cn("处理器")] public interface cpu : chip { }
    [cn("图形卡")] public interface graphic_card : chip { }


    // 智能
    [from_(typeof(chip))]
    [cn("智能设备")] public interface intelligent_machine : product { }
    [cn("计算机")] public interface computer : intelligent_machine { }
    [cn("平板电脑")] public interface tablet : intelligent_machine { }
    [cn("机器人")] public interface bot : intelligent_machine { }
    [cn("智能人")] public interface droid : intelligent_machine { }
    [cn("合成人")] public interface synthetic_bot : intelligent_machine { }
    [cn("无人机")] public interface drone : intelligent_machine { }
    [cn("纳米机器人")] public interface nano_robot : machine_complex { }


    // 能源
    [cn("发电机")] public interface generator : product { }


    [from_(typeof(silicon_crystal))]
    [cn("太阳能板")] public interface solar_panel : generator { }


    [from_(typeof(chemicals))]
    [cn("电池")] public interface battery : product { }
    [cn("铅酸蓄电池")] public interface leadacid_battery : battery { }
    [cn("锂电池")] public interface lithium_battery : battery { }
    [cn("干电池")] public interface dry_battery : battery { }
}
