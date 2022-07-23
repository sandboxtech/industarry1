
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{

    /// <summary>
    /// mechanics 从数值、机制、代码：入手设计
    /// conceptdef 从类型关系，知识体系，内容题材
    /// experience 从玩家认知
    /// </summary>




    // condition a d: b => c

    // 主要给科技物品标记输入输出
    // [input_ b] a
    // [output c] a
    // [cond d] a

    // 偶尔物品标注
    // [from b] c
    // [for_ c] b
    // [from_cond a] b
    // [for_cond  a] c


    /// <summary>
    /// 配方输入
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface)]
    public class input_Attribute : Attribute
    {
        public Type[] content { get; private set; }
        public input_Attribute(params Type[] types) { content = types; }
    }
    [AttributeUsage(AttributeTargets.Interface)]
    public class outputAttribute : Attribute
    {
        public Type[] content { get; private set; }
        public outputAttribute(params Type[] types) { content = types; }
    }
    [AttributeUsage(AttributeTargets.Interface)]
    public class conditionAttribute : Attribute
    {
        public Type[] content { get; private set; }
        public conditionAttribute(params Type[] types) { content = types; }
    }


    [AttributeUsage(AttributeTargets.Interface)]
    public class unlockAttribute : Attribute // 科技解锁条件
    {
        public Type[] content { get; private set; }
        public unlockAttribute(params Type[] types) { content = types; }
    }

    [AttributeUsage(AttributeTargets.Interface)]
    public class unlock_byAttribute : Attribute // 科技解锁条件
    {
        public Type[] content { get; private set; }
        public unlock_byAttribute(params Type[] types) { content = types; }
    }

    [AttributeUsage(AttributeTargets.Interface)]
    public class from_Attribute : Attribute
    {
        public Type[] content { get; private set; }
        public from_Attribute(params Type[] types) { content = types; }
    }
    [AttributeUsage(AttributeTargets.Interface)]
    public class for__Attribute : Attribute
    {
        public Type[] content { get; private set; }
        public for__Attribute(params Type[] types) { content = types; }
    }
    [AttributeUsage(AttributeTargets.Interface)]
    public class from_conditionAttribute : Attribute
    {
        public Type[] content { get; private set; }
        public from_conditionAttribute(params Type[] types) { content = types; }
    }
    [AttributeUsage(AttributeTargets.Interface)]
    public class for__conditionAttribute : Attribute
    {
        public Type[] content { get; private set; }
        public for__conditionAttribute(params Type[] types) { content = types; }
    }





    public interface ILocalizationAttribute { }

    public class cnAttribute : Attribute, ILocalizationAttribute
    {
        public string content { get; private set; }
        public cnAttribute(string content) { this.content = content; }
    }

    public class hideAttribute : Attribute, ILocalizationAttribute
    {
        public hideAttribute() { }
    }

}
