
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{
    public class EdibleItem : Item, IEdible
    {
        public override Scroll ExtraUseScroll => Scroll.Button("食用", () => EatPage.Of(base.OnUse, Satiety, Energy, this));
        public bool TryEat(long q) => EatPage.TryEat(Satiety, Energy, this, q);
        public virtual long Satiety => 1;
        public virtual long Energy => 1;
    }



    public static class EatPage
    {
        private const int MAX = 1_0000_0000;
        private static int slider_value;


        public static bool TryEat(long satiety, long e, Item item, long q, bool drink = false) {
            if (G.player.attr.TryEat(satiety * q, e * q, item, drink)) {
                item.Quantity -= q;
                if (item.Quantity <= 0) item.Detach();
                return true;
            }
            return false;
        }

        public static void Of(Action back, long satiety, long e, Item item) {
            int quantity = item.Quantity > MAX ? MAX : (int)item.Quantity;

            if (item.Quantity <= 0) {
                item.Detach();
                Scroll.Close();
                return;
            }
            Scroll.Show(
                Scroll.ReturnButton(back),
                Scroll.Slot(item),
                Scroll.Text($"效用: {Color4.yellow_light.Dye($"体力 {e}")} {Color4.orange_light.Dye($"饱腹 {satiety}")}"),
                Scroll.Button("食用", () => {
                    G.player.state.eating_count = slider_value;
                    G.player.state.TryEat();
                    Scroll.Close();
                }),
                Scroll.SliderInt(() => $"计划食用量 {slider_value}", (int v) => { slider_value = v; }, quantity, quantity, 0)
            );
        }
    }
}
