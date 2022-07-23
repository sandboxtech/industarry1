
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{



    [impl(typeof(workbench_alchemy_))]
    [recipe(typeof(stone), 10, false, typeof(workbench_alchemy))]
    [cn("炼金术士工作台")] public interface workbench_alchemy : building { } // stone => this. stone => stonebrick, other
    public class workbench_alchemy_ : FactoryLike
    {
        public override Vec2 Size => new Vec2(3, 1);
        public override List<Type> Recipes => recipes;
        private static List<Type> recipes = new List<Type>() {
            typeof(mine),
            typeof(mortar),
            typeof(bloomary),
            typeof(furnance_blast),
            typeof(anvil),
            typeof(windmill),
            typeof(bell),
            typeof(cauldron),
            typeof(mechanical_millstone),
            null,
            typeof(brick_residence),
            typeof(brick_workshop),
            typeof(brick_warehouse),
            typeof(brick_library),
        };

        public override string Content => InProgress ? DefaultContent : DefaultContentFirstFrame;
        public override Color4 ContentColor => G.theme.White;
    }


}
