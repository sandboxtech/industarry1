
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{

    [impl(typeof(book_))]
    [cn("书籍")] public interface book { }

    public abstract class book_ : Item
    {
        public override Type ContentTypeRedirect => typeof(book);

        public override void OnUse() {
            bool firstPage = PageIndex == 0;
            bool lastPage = PageIndex == BookContents.Length;
            Scroll.Show(
                Scroll.Button(firstPage ? "阅读" : lastPage ? "关闭" : $"第{PageIndex}/{BookContents.Length - 1}页", () => {
                    PageIndex++;
                    PageIndex %= (BookContents.Length + 1);
                    if (PageIndex == 0) {
                        Scroll.Close();
                    } else {
                        OnUse();
                    }
                }),
                (firstPage || lastPage) ? Scroll.Empty : Scroll.TextMulti(BookContents[PageIndex]),
                firstPage ? Scroll.Slot(BookContents[PageIndex], this, null, SlotBackgroundType.Transparent)
                : (lastPage ? Scroll.Text("完结") : Scroll.Empty)
            );
            // OnUse();
            AfterUse();
        }

        [JsonProperty] public int PageIndex = 0;

        public abstract string[] BookContents { get; }

        protected virtual void AfterUse() {

        }
    }


    [impl(typeof(book_travel_))]
    [cn("漫游指南")]
    public interface book_travel : book { }

    public class book_travel_ : book_
    {
        public override string[] BookContents => bookContents;

        public static string[] bookContents = new string[] {
            "土木世界 漫游指南",
            "世界需要手动保存! 点击自己或飞船进行保存",
            "不用一直停留在母星, 其他星球有一些建筑和宝箱, 有不同的植被和矿产, 建筑功能也会发生变化",

            "拾取飞船, 点击飞船, 可以消耗飞船蓄电池能量, 前往太空",
            "在太空点击星球, 可以扫描星球, 查看星球详情",

            "蓄电池能量不足? 把飞船放在星球上, 可以进行太阳能充电",
            "平均充电1小时, 飞船就能再次起飞。星球太阳能效率越高, 飞船充电速度越快",
            "充满电的飞船, 足够平均6次起飞",

            "《土木世界》和《挂机工厂》有什么关系? 土木世界是《挂机工厂2》的前传, 没有流水线的灵魂, 不配进入挂机工厂系列",
            "什么时候能玩到《挂机工厂2》? 有生之年",
            "游戏不好玩? bug多? 数值差? 加qq群喷作者 710113782 (群号已经复制到剪贴板)",
        };
        protected override void AfterUse() {
            UnityEngine.GUIUtility.systemCopyBuffer = "710113782";
        }
    }

    [impl(typeof(book_survival_))]
    [cn("生存指南")]
    public interface book_survival : book { }

    public class book_survival_ : book_
    {
        public override string[] BookContents => bookContents;

        public static string[] bookContents = new string[] {
            "土木世界 生存指南",

            "经过作物, 可以进行收获",
            "点击建筑, 可以获取订单, 进行交易",

            "拿着建筑, 点击空地, 可以进行建造\n建筑的左下角会出现在点击位置",
            "拿着建材, 在空地移动, 可以进行建造",
            "拿着锤子, 向建筑移动, 可以进行拆除",
            "拿着铲子, 向地板/草地移动, 可以进行拆除",
            "拿着矿井, 进行移动, 可以发现矿藏",
            "初期银币怎么获取? 挖掘铁矿/铜矿/煤矿，出售。采集木材/树叶/树枝，出售",

            "工具栏快满了, 则有些操作无法进行",
            "工具栏格子不够, 可以携带背包",

            "大部分工作, 会消耗体力",
            "如果体力不足, 会走不动路, 甚至无法工作",
            "食用食物, 能提供体力, 同时提供饱腹值",
            "饱腹值满, 则无法继续进食。饱腹值会随时间减少",

            "土木世界的体力值, 一个鸡肋设定",

            "可以把垃圾游戏分享给其他人 (游戏网址已经复制到剪贴板)",
        };
        protected override void AfterUse() {
            UnityEngine.GUIUtility.systemCopyBuffer = "https://www.taptap.com/app/228597";
        }
    }
}
