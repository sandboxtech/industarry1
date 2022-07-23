
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{

    [cn("时代")] public interface age { }

    [cn("法力时代")] public interface mana_age : age { }
    [cn("炼金时代")] public interface farm_age : age { }
    [cn("蒸汽时代")] public interface steam_age : age { }
    [cn("电气时代")] public interface petrol_age : age { }
    [cn("电子时代")] public interface electro_age : age { }

    [cn("生物时代")] public interface biology_age : age { }
    [cn("太空时代")] public interface space_age : age { }
    [cn("智能时代")] public interface intelligence_age : age { }


    //[cn("采集时代")] public interface tool_period : mana_age { }
    //[cn("苇草时代")] public interface hut_period : mana_age { }
    //[cn("陶艺时代")] public interface clay_period : farm_age { }
    //[cn("木艺时代")] public interface wood_period : farm_age { }
    //[cn("石艺时代")] public interface stone_period : farm_age { }
}
