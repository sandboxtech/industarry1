
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{
    /// <summary>
    /// 用于领域相关性的标注
    /// </summary>
    public class relateAttribute : Attribute
    {
        public Type[] content { get; private set; }
        public relateAttribute(params Type[] content) { this.content = content; }
    }


    /// <summary>
    /// 特殊资源
    /// </summary>
    [cn("劳力")] public interface workforce { }
    [cn("储存")] public interface storage { }
    [cn("消费市场")] public interface consumption { } // 指 res <=> currency 的机会

    [impl(typeof(heat_energy_))]
    [cn("热能")] public interface heat_energy { } // for glass clay metal refinery
    public class heat_energy_ : Item
    {
        public override string Content => null;
        public override string Highlight => DefaultContent;
    }

    [impl(typeof(kinematic_energy_))]
    [cn("动能")] public interface kinematic_energy { }
    public class kinematic_energy_ : Item
    {
        public override string Content => null;
        public override string Highlight => DefaultContent;
    }



    [impl(typeof(electric_energy_))]
    [cn("电能")] public interface electric_energy { }

    public class electric_energy_ : Item
    {
        public override string Content => null;
        public override string Highlight => DefaultContent;
        public override Color4 HighlightColor => G.theme.BulbColor;
    }



    /// <summary>
    /// 偏理论。或偏应用
    /// </summary>
    [cn("技艺")] public interface skill { }
    [cn("技术")] public interface technology { }
    [cn("科学")] public interface science { }


    /// <summary>
    /// 三大分类。人文，物理，应用
    /// </summary>
    [cn("物理学")] public interface physics { }
    [cn("社会学")] public interface sociology { }
    [cn("工程学")] public interface engineering { }

    /// <summary>
    /// 子类
    /// </summary>
    [cn("数学")] public interface math : physics { }
    [cn("化学")] public interface chemistry : engineering { }
    [cn("生物学")] public interface biology : sociology { }
    [cn("地理学")] public interface geography : sociology { }




    [input_(typeof(mana))]
    [cn("魔法")] public interface sorcery : skill { }
    [cn("魔法卷轴制作工艺")] public interface scroll_making : skill { }
    [cn("魔法药水制作工艺")] public interface potion_making : skill { }



    [cn("手工")] public interface handcrafting : skill { }

    [cn("建筑工")] public interface construction : skill { }

    [relate(typeof(clay))]
    [cn("陶工")] public interface ceramic_working : skill { }

    [relate(typeof(wood))]
    [cn("木工")] public interface carpentry : skill { }

    [relate(typeof(stone))]
    [cn("石工")] public interface masonry : skill { }

    [relate(typeof(metal))]
    [cn("冶金")] public interface metal_working : skill { }

    [relate(typeof(ingredient))]
    [cn("烹饪")] public interface cooking : skill { }




    [cn("历法")] public interface calender : geography { }
    [cn("地质学")] public interface geology : geography { }
    [cn("农学")] public interface agriculture : geography { }



    [cn("算数")] public interface arithmetics : math { }
    [cn("几何")] public interface geometry : math { }
    [cn("代数")] public interface algebra : math { }
    [cn("分析")] public interface caculus : math { }


    [cn("力学")] public interface mechanics : physics { }
    [cn("动力学")] public interface dynamics : physics { }
    [cn("光学")] public interface optics : physics { }
    [cn("声学")] public interface acoustics : physics { }
    [cn("电动力学")] public interface electromagnetics : physics { }
    [cn("热动力学")] public interface thermodynamics : physics { }
    [cn("量子物理")] public interface quantum_physics : physics { }
    [cn("粒子物理")] public interface particle_physics : physics { }
    [cn("宇宙学")] public interface cosmology : physics { }



    [cn("核工程")] public interface nuclear_engineering : engineering { }
    [cn("语言自动机")] public interface automata : engineering { }
    [cn("人工智能")] public interface ai : engineering { }


    [cn("元素理论")] public interface element_study : chemistry { }
    [cn("有机化学")] public interface organic_chemistry : chemistry { }
    [cn("晶体化学")] public interface crystallochemistry : chemistry { }
    [cn("电化学")] public interface electrochemistry : chemistry { }


    [cn("植物学")] public interface botany : biology { }
    [cn("动物学")] public interface zoology : biology { }
    [cn("进化论")] public interface evolution_theory : biology { }
    [cn("遗传学")] public interface genetics : biology { }
    [cn("仿生学")] public interface bionics : biology { }


    [cn("人类学")] public interface anthropology : sociology { }
    [cn("历史学")] public interface history : sociology { }
    [cn("考古学")] public interface archaeology : sociology { }

}
