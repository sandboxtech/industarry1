
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{

    /// <summary>
    /// 概念
    /// </summary>
    [cn("存在")] public interface being { }

    [cn("空间")] public interface space : being { }
    [cn("时间")] public interface time : being { }


    [cn("物质")] public interface matter : being { }
    [cn("能量")] public interface energy : being { }
    [cn("信息")] public interface information : being { }



    [cn("法力")] public interface mana : energy { }

    [cn("电力")] public interface electricity : energy { }

    [cn("电磁波")] public interface electromagnetic_wave : energy { }
    [cn("电场")] public interface electric_field : energy { }
    [cn("磁场")] public interface magnetic_field : energy { }


}

