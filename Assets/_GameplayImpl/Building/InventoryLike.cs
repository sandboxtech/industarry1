
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{

    public class InventoryLike : Item
    {
        [JsonProperty] public ItemList list { get; private set; }

        public override bool Absorbable => list == null || 0 == list.occupied_slot_count;
        public override bool Destructable => Absorbable && G.player.hand.HasSpace() && base.Destructable;
        // public override bool Pickable => true;

        private const long max = 7 * 7 * 7;

        public virtual void SetInventoryCapacity(int v) {
            A.Assert(list == null && v < max);
            list = ItemList.Create(v);
        }

        protected virtual int Adder => 1;
        protected virtual int Multiplier => 7;

        protected void GridPage() {
            if (list == null) {
                int quantity = Multiplier * (Adder + (int)Log10QuantityLong);
                A.Assert(quantity > 0 && quantity <= max);
                list = ItemList.Create(quantity);
            }
            Scroll.Show(
                
                Scroll.Grid(list.available_slot_max, OnTap),
                Scroll.CloseButton,
                Scroll.Text($"容量 {list.available_slot_max}"),
                Scroll.Button("百科", () => { WikiPage.Of(type); }),
                Scroll.Slot(this, SlotBackgroundType.Transparent),

                Scroll.Empty
            );
        }




        private Item OnTap(int i, bool manual) {
            Item item = list[i];
            if (!manual) return item;

            Item hand = G.player.HandItem;

            if (hand is InventoryLike) {
                // 无法合并，尝试加入手里
                if (item != null) {
                    if (G.player.hand.NoSpace()) return item;
                    if (G.player.hand.TryAbsorbOrAdd(item)) {
                        // 成功吸收或加入。
                        return list[i]; // 可能吸收完毕，也可能吸收一半
                    }
                }
                // 不能把任何背包类放入背包，包括自己
                UI_Notice.I.Send("无法放入");
                return item;
            } else if (item == null && hand != null) {
                // 把物体放入背包
                list[i] = hand;
                return hand;
            } else if (item != null && hand == null) {
                // 把物体从背包拿到手上
                G.player.HandItem = item;
                return null;
            } else if (item == null && hand == null) {
                // 无事发生
                return item;
            } else {
                // 尝试箱里合并手里
                if (item.TryAbsorb(hand)) {

                } else {
                    // 无法合并，尝试加入手里
                    if (G.player.hand.NoSpace()) return item;

                    if (G.player.hand.TryAbsorbOrAdd(item)) {
                        // 成功吸收或加入。
                        return list[i]; // 可能吸收完毕，也可能吸收一半
                    }
                }
                return item;
            }
        }
    }

}
