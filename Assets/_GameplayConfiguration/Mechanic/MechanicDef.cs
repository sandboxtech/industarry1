
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{


    [cn("资源")] public interface item { } // 资源，无参数。一定用接口定义了子类偏序关系。大多定义了其他recipe。可能定义了其他行为


    [cn("行为")] public interface action { }




    // 下面的和生产玩法有关了

    //[cn("工厂类")] public interface factorylike { } // inc => inc
    //[cn("仓库类")] public interface storagelike { } // inc => val





    // 下面的和物流玩法有关了。暂时不用管

    [cn("下载器")] public interface downloader : building { } // map.inc => belt.inc 。1代手推车终点
    [cn("上传器")] public interface uploader : building { } // belt.inc => map.inc 。1代手推车起点
    [cn("运输")] public interface transportation : building { } // belt.inc == belt.inc 相邻自动选择，类型不变

    [cn("合并器")] public interface merger : building { } // belt.inc 。1代道路合并
    [cn("分离器")] public interface spliter : building { } // 定量 分离 belt.inc == belt.inc + belt.inc

}