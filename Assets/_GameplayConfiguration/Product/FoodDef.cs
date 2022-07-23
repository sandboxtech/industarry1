
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{
    [recipe(typeof(wheat), 1, false, typeof(flour), 1)]
    [cn("面粉")] public interface flour : ingredient { }




    [recipe(typeof(fish), 1, typeof(heat_energy), 1, false, typeof(fish_baked), 1)]
    [impl(typeof(fish_baked_))]
    [cn("烤鱼")] public interface fish_baked { }
    public class fish_baked_ : EdibleItem {
        public override long Satiety => 10;
        public override long Energy => Satiety * 5;
    }



    [impl(typeof(berry_baked_))]
    [recipe(typeof(berry), 1, typeof(heat_energy), 1, false, typeof(berry_baked), 1)]
    [cn("烤浆果")] public interface berry_baked { }
    public class berry_baked_ : EdibleItem {
        public override long Satiety => 10;
        public override long Energy => Satiety * 2;
    }


    [impl(typeof(dough_baked_))]
    [recipe(typeof(flour), 1, typeof(water), 1, false, typeof(dough), 1)]
    [cn("生面团")] public interface dough { }
    public class dough_ : EdibleItem {
        public override long Satiety => 3;
        public override long Energy => Satiety * 1;
    }


    [impl(typeof(dough_baked_))]
    [recipe(typeof(dough), 1, typeof(heat_energy), 1, false, typeof(dough_baked), 1)]
    [cn("烤面团")] public interface dough_baked { }
    public class dough_baked_ : EdibleItem {
        public override long Satiety => 3;
        public override long Energy => Satiety * 20;
    }


    [impl(typeof(dough_baked_))]
    [recipe(typeof(dough), 1, typeof(sugar), 1, typeof(heat_energy), 1, false, typeof(donut), 1)]
    [cn("甜甜圈")] public interface donut { }
    public class donut_ : EdibleItem
    {
        public override long Satiety => 5;
        public override long Energy => Satiety * 20;
    }



    [recipe(typeof(fish), 1, typeof(salt), 1, false, typeof(fish_salted), 1)]
    [impl(typeof(fish_salted_))]
    [cn("干物鱼")] public interface fish_salted : chordate { }
    public class fish_salted_ : EdibleItem {
        public override long Satiety => 3;
        public override long Energy => Satiety * 10;
    }






    public class clay_pottery_stuffed : Item
    {
        public override Color4 ContentColor => G.theme.ClayBakedColor;
        public override string Highlight => DefaultContainer;

        public override Scroll ExtraUseScroll => Scroll.Button("倒掉内容物", () => {
            // type = typeof(claybaked_pottery)
            G.player.HandItem = Of<claybaked_pottery>().SetQuantity(Quantity);
            Scroll.Close();
        });
    }


    [recipe(typeof(claybaked_pottery), 1, typeof(berry), 1, false, typeof(clay_pottery_wine), 1)]
    [impl(typeof(clay_pottery_wine_))]
    [cn("酿果酒")] public interface clay_pottery_wine : chordate { }
    public class clay_pottery_wine_ : clay_pottery_stuffed
    {
        public override Color4 HighlightColor => G.theme.FruitColor;
    }


    [recipe(typeof(claybaked_pottery), 1, typeof(honey), 1, false, typeof(clay_pottery_mead), 1)]
    [impl(typeof(clay_pottery_mead_))]
    [cn("酿蜜酒")] public interface clay_pottery_mead : chordate { }
    public class clay_pottery_mead_ : clay_pottery_stuffed
    {
        public override Color4 HighlightColor => G.theme.GoldColor;
    }
}
