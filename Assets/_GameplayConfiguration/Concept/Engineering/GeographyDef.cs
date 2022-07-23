
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{
    [cn("气候")] public interface climate { }
    [cn("热带")] public interface tropical : climate { }
    [cn("幻想")] public interface fantastic { } // => 经济




    [cn("地貌")] public interface terrain : item { } // 地图上生成的。也许是地形



    [cn("稀有物品")] public interface scarce_item : terrain { }
    [cn("遗迹")] public interface remain : scarce_item { }
    [cn("宝物")] public interface treasure : scarce_item { }
}
