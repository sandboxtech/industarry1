
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{


    [recipe(typeof(stick), 1, typeof(stone), 1, false, typeof(paper_maker), 1)]
    [impl(typeof(paper_maker_))]
    [cn("研磨器")] public interface paper_maker : building { }
    public class paper_maker_ : FactoryLike
    {
        public override Vec2 Size => new Vec2(2, 1);
        public override List<Type> Recipes => recipes;
        private static List<Type> recipes = new List<Type>() {
            typeof(paper_maker),
            typeof(paper),
        };
        public override Color4 ContentColor => G.theme.WoodColor;
        public override Color4 ContainerColor => G.theme.StoneColor;
        public override string Content => InProgress ? DefaultContent : DefaultContentFirstFrame;
        public override string Container => InProgress ? DefaultContainer : DefaultContainerFirstFrame;
    }


    [impl(typeof(workbench_writing_))]
    [recipe(typeof(wood_tile), 1, false, typeof(workbench_writing))]
    [cn("书桌")] public interface workbench_writing : building { }
    public class workbench_writing_ : FactoryLike
    {
        public override string Content => InProgress ? DefaultContent : DefaultContentFirstFrame;
        public override Color4 ContentColor => G.theme.WoodColor; public override Vec2 Size => new Vec2(2, 1);
    }
}
