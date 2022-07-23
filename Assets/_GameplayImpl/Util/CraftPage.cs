
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{



    /// <summary>
    /// 用于瞬间合成的配方
    /// </summary>
    public static class CraftPage {
        public static Scroll Slot<T>(Item factory) => Scroll.Slot(ItemPool.GetDef<T>(), () => ShowCraftPageOfRecipe(factory, typeof(T)), SlotBackgroundType.Convex);
        public static Scroll Slot<T>(Item factory, Type recipe) => Scroll.Slot(ItemPool.GetDef<T>(), () => ShowCraftPageOfRecipe(factory, recipe), SlotBackgroundType.Convex);

        private static bool Valid(int index) {
            for (int i = 0; i < used_slot_count; i++) {
                if (input_slots[i] == index) return false;
            }
            return true;
        }

        static CraftPage() {
            for (int i = 0; i < input_slots.Length; i++) {
                input_slots[i] = -1;
            }
        }

        private static string message;
        private static int[] input_slots = new int[C.HandLength];
        private static int used_slot_count = 0;
        public const int energy_cost_per_craft = 10;


        /// <summary>
        /// 为工具栏合成打造的
        /// </summary>
        private static void ShowCraftPageOfRecipe(Item factory, Type recipe, bool first = true) {

            recipeAttribute attr = Attr.Get<recipeAttribute>(recipe);
            A.Assert(attr != null, () => $"{recipe.Name}");

            (Type, long)[] inputs_ = attr.input_;
            (Type, long)[] outputs = attr.output;

            A.Assert(inputs_ != null);
            A.Assert(outputs != null);

            List<Scroll> scrolls = new List<Scroll>();

            scrolls.Add(Scroll.ReturnButton(() => factory._WorkPage()));

            int outputSpace = outputs.Length;
            if (first) {
                if (G.player.hand.NoSpace(outputSpace)) {
                    message = $"背包空间不足, 需要{outputSpace}";
                } else {
                    message = "点此合成";
                }
                used_slot_count = inputs_.Length;
                for (int i = 0; i < used_slot_count; i++) {
                    input_slots[i] = -1;
                }
            }


            for (int j = 0; j < inputs_.Length; j++) {
                int i = j;
                var entry = inputs_[i];

                Item view = null;
                if (input_slots[i] < 0) {
                    view = ItemPool.GetDef(entry.Item1);
                    view.SemiTransparent = true;
                    view.Quantity = entry.Item2;
                } else {
                    view = G.player.hand[input_slots[i]];
                }

                scrolls.Add(Scroll.Slot(
                    Attr.CN(entry.Item1),
                    view,
                    () => {
                        if (G.player.HandItem != null && Valid(G.player.HandIndex) && Ty.Is(G.player.HandItem.type, entry.Item1)) {
                            // 空指针，背包格子未占用且合理。
                            input_slots[i] = G.player.HandIndex;
                        } else {
                            // 取消
                            input_slots[i] = -1;
                        }
                        ShowCraftPageOfRecipe(factory, recipe, false);
                    }
                ));
            }


            scrolls.Add(Scroll.Button(() => message, () => {

                if (outputSpace != 0 && G.player.hand.NoSpace(outputSpace)) {
                    message = $"背包空间不足, 需要{outputSpace}";
                    return;
                }

                for (int i = 0; i < used_slot_count; i++) {
                    if (input_slots[i] < 0) {
                        message = $"{Attr.CN(inputs_[i].Item1)} 缺失";
                        return;
                    }
                }

                long max_craft = G.settings.craft_mode_one_or_all ? 1 : 1000; // 每次最多1000

                max_craft = M.Min(max_craft, G.player.attr.energy.Value / energy_cost_per_craft);

                if (max_craft == 0) {
                    message = $"体力为零";
                    return;
                }

                for (int i = 0; i < used_slot_count; i++) {
                    var entry = inputs_[i];
                    if (entry.Item2 != 0) {
                        max_craft = M.Min(max_craft, G.player.hand[input_slots[i]].Quantity / entry.Item2);
                        if (max_craft <= 0) {
                            message = $"{Attr.CN(entry.Item1)} 不足";
                            return;
                        }
                    } else {
                        max_craft = 1;
                    }
                }

                // 消耗输入
                for (int i = 0; i < used_slot_count; i++) {
                    long cost = inputs_[i].Item2;
                    if (cost == 0) continue;
                    G.player.hand[input_slots[i]].Quantity -= cost * max_craft; // mul
                }
                // 获得输出
                for (int i = 0; i < outputs.Length; i++) {
                    Item item = Item.Of(outputs[i].Item1);
                    if (outputs[i].Item2 != 0) {
                        item.Quantity = outputs[i].Item2 * max_craft; // mul
                    }
                    G.player.hand.TryAbsorbOrAdd(item);
                }
                // 做动画
                for (int i = 0; i < used_slot_count; i++) {
                    UI_Hand.I.RenderItemAt(input_slots[i]);
                }
                // 减体力
                G.player.attr.energy.Value -= max_craft * energy_cost_per_craft;


                message = $"成功合成{max_craft}次";

                ShowCraftPageOfRecipe(factory, recipe, false);
            }));


            // previews
            for (int i = 0; i < outputs.Length; i++) {
                Item preview = ItemPool.GetDef(outputs[i].Item1);
                preview.Quantity = outputs[i].Item2;
                scrolls.Add(Scroll.SlotRight(preview, null, SlotBackgroundType.Transparent));
            }



            scrolls.Add(Scroll.Space);

            scrolls.Add(Scroll.Button(() => $"合成模式 {(G.settings.craft_mode_one_or_all ? "单件" : "批量")}",
                () => G.settings.craft_mode_one_or_all = !G.settings.craft_mode_one_or_all));

            scrolls.Add(Scroll.Text($"每次成功合成需要 {energy_cost_per_craft}体力"));

            factory.ReRender();
            Scroll.Show(scrolls);
        }
    }
}
