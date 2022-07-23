
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{
    /// 收集资源（徒手，手动
    /// 获取配方（消耗体力，如获取xx
    /// 进行合成（如工具
    /// 访问商店（解锁新玩意


    /// 游戏与玩家类型
    /// 手动采集（mc 生存类
    /// 半手动采集（种田
    /// 挂机自动采集（不用管
    /// 沙盒建造（建造自由，好看有用
    /// rpg（多元素，丰富属性


    [output(typeof(berry))]
    public interface collect_berry : action { }


    [condition(typeof(scroll))]
    public interface make_tool : action { }
}
