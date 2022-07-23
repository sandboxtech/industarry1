
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{
    // textile
    [impl(typeof(thread_))]
    [recipe(typeof(fiber), false, typeof(thread))]
    [cn("线")] public interface thread : handcraft { }
    public class thread_ : Item { public override Color4 ContentColor => G.theme.DryFloraColor; }

    [impl(typeof(textile_))]
    [recipe(typeof(thread), false, typeof(textile))]
    [cn("织物")] public interface textile : handcraft { }
    public class textile_ : Item { public override Color4 ContentColor => G.theme.DryFloraColor; }

    [impl(typeof(rope_))]
    [recipe(typeof(thread), false, typeof(rope))]
    [cn("绳")] public interface rope : handcraft { }
    public class rope_ : Item { public override Color4 ContentColor => G.theme.DryFloraColor; }


    [impl(typeof(yarn_))]
    [recipe(typeof(thread), false, typeof(yarn))]
    [cn("毛线团")] public interface yarn : handcraft { }
    public class yarn_ : Item { public override Color4 ContentColor => G.theme.DryFloraColor; }

    [impl(typeof(bandage_))]
    [recipe(typeof(textile), false, typeof(bandage))]
    [cn("绷带")] public interface bandage : handcraft { }
    public class bandage_ : Item { public override Color4 ContentColor => G.theme.DryFloraColor; }



    [recipe(typeof(textile), false, typeof(backpack))]
    [impl(typeof(backpack_))]
    [cn("草包")] public interface backpack { }
    public class backpack_ : InventoryLike
    {
        public override Color4 ContentColor => G.theme.DryFloraColor;
        public override void OnUse() {
            GridPage();
        }
    }



    [cn("纺机")] public interface loom : building { } // misc


    [recipe(typeof(fiber), 1, false, typeof(workbench_textile))]
    [impl(typeof(textile_workbench_))]
    [cn("纺织工作台")] public interface workbench_textile : building { } // stone => this. stone => stonebrick, other
    public class textile_workbench_ : FactoryLike
    {
        public override Vec2 Size => new Vec2(2, 1);
        public override List<Type> Recipes => recipes;
        private static List<Type> recipes = new List<Type>() {
            typeof(thread),
            typeof(textile),
            typeof(rope),
            typeof(yarn),
            // typeof(bandage),
            typeof(backpack),
        };

        public override string Content => InProgress ? DefaultContent : DefaultContentFirstFrame;
        public override Color4 ContentColor => G.theme.DryFloraColor;
    }
}
