
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{
    public abstract class MarketLike : Item
    {


        protected virtual long Max => 10;
        protected virtual long Del => C.Minute / C.TestMarketDelDivider;


        public override void OnCreate() {
            base.OnCreate();
            // _FactoryData = new FactoryLikeDataPage(null, Max, Del);
            IdleData = Idle.Create(Max - 1, Max, Del, 1);
        }
        protected override void OnAddToMap() {
            IdleData.Clear();
        }

        public virtual bool InProgress => G.at_night;

        public override bool Destructable => true;

        public override bool TryPickOnEnter => false;
        public override bool Absorbable => true;
        public override bool CanEnter => false;


        public override bool CanTap => true;
        public override void OnTap() {
            base.OnTap();
            _WorkPage();
        }

        public override void _WorkPage() {
            Work();
        }

        /// <summary>
        /// 从底下随机选择
        /// </summary>
        public virtual List<Type> Recipes => null;
        public virtual Type MarketRecipe => null;

        protected virtual bool ExtraCondition => true;
        protected virtual void ExtraAction() { }


        private void Work(bool first = true) {

            List<Scroll> scrolls = new List<Scroll>();

            scrolls.Add(Scroll.Destruct(this));
            scrolls.Add(Scroll.Button("刷新订单", () => { TryAssignRecipe(); Work(); }));


            if (recipe_not_defined) scrolls.Add(Scroll.Text("未定义订单"));
            if (_MarketRecipe == null) scrolls.Add(Scroll.Text("暂无订单"));



            if (_MarketRecipe == null || recipe_not_defined) {
                scrolls.Add(Scroll.IdleProgress(IdleData));
                scrolls.Add(Scroll.IdleTotalProgress(IdleData));

                // icon
                scrolls.Add(Scroll.Slot(this, SlotBackgroundType.Transparent));

                Scroll.Show(scrolls);
                return;
            }

            // 开始读取订单配方

            recipeAttribute attr = Attr.Get<recipeAttribute>(_MarketRecipe);
            A.Assert(attr != null, () => $"找不到配方 {_MarketRecipe.Name}");

            (Type, long)[] inputs_ = attr.input_;
            (Type, long)[] outputs = attr.output;

            // A.Assert(inputs_ != null);
            // A.Assert(outputs != null);

            long quantity = M.Min(Quantity, 10000);

            // 第一次打开界面初始化

            int outputSpace = outputs == null ? 0 : outputs.Length;
            if (first) {
                if (outputSpace != 0 && G.player.hand.NoSpace(outputSpace)) {
                    message = $"背包空间不足, 需要{outputSpace}";
                } else {
                    if (discount == 10) {
                        message = "点此交易 原价";
                    } else {
                        message = Color4.Lerp(Color4.orange, Color4.white, (float)discount / discount_max).Dye($"点此交易 {discount} 折");
                    }
                }
                used_slot_count = inputs_ == null ? 0 : inputs_.Length;
                if (inputs_ != null) for (int i = 0; i < used_slot_count; i++) {
                        input_slots[i] = -1;
                    }
            }

            // 读取输入

            if (inputs_ != null) for (int j = 0; j < inputs_.Length; j++) {
                    int i = j;
                    var entry = inputs_[i];

                    Item view = null;
                    if (input_slots[i] < 0) {
                        view = ItemPool.GetDef(entry.Item1);
                        view.SemiTransparent = true;
                        view.Quantity = entry.Item2 * discount * quantity;
                    } else {
                        view = G.player.hand[input_slots[i]];
                    }

                    scrolls.Add(Scroll.Slot(
                        Attr.CN(entry.Item1),
                        view,
                        () => {
                            if (G.player.HandItem != null && Valid(G.player.HandIndex) && Ty.Is(G.player.HandItem.type, entry.Item1)) {
                                input_slots[i] = G.player.HandIndex;
                            } else {
                                input_slots[i] = -1;
                            }
                            Work(false);
                        }
                    ));
                }

            // 交易按钮

            scrolls.Add(Scroll.Button(() => message, () => {

                if (!ExtraCondition) {
                    message = $"暂时不能完成订单";
                    Work(false);
                    return;
                }

                if (outputSpace != 0 && G.player.hand.NoSpace(outputSpace)) {
                    message = $"背包空间不足, 需要{outputSpace}";
                    Work(false);
                    return;
                }

                if (inputs_ != null) for (int i = 0; i < used_slot_count; i++) {
                        if (input_slots[i] < 0) {
                            message = $"{Attr.CN(inputs_[i].Item1)} 缺失";
                            Work(false);
                            return;
                        }
                    }

                long max_craft = 1; // 每次最多1000

                if (inputs_ != null) for (int i = 0; i < used_slot_count; i++) {
                        var entry = inputs_[i];
                        if (entry.Item2 != 0) {
                            max_craft = M.Min(max_craft, G.player.hand[input_slots[i]].Quantity / (entry.Item2 * discount));
                            if (max_craft <= 0) {
                                message = $"{Attr.CN(entry.Item1)} 不足";
                                Work(false);
                                return;
                            }
                        }
                    }


                // 其他影响
                ExtraAction();
                // 消耗输入
                if (inputs_ != null) for (int i = 0; i < used_slot_count; i++) {
                        long cost = inputs_[i].Item2 * discount;
                        if (cost == 0) continue;
                        G.player.hand[input_slots[i]].Quantity -= cost * max_craft * quantity; // mul
                    }
                // 获得输出
                if (outputs != null) for (int i = 0; i < outputs.Length; i++) {
                        Item item = Item.Of(outputs[i].Item1);
                        if (outputs[i].Item2 != 0) {
                            item.Quantity = outputs[i].Item2 * max_craft * quantity; // mul
                        }
                        G.player.hand.TryAbsorbOrAdd(item);
                    }
                // 做动画
                for (int i = 0; i < used_slot_count; i++) {
                    UI_Hand.I.RenderItemAt(input_slots[i]);
                }

                _MarketRecipe = null; // remove recipe
                complete_count++;
                G.achievement.transaction_count++;
                TryAssignRecipe();

                message = $"成功交易";
                Work();
            }));


            // previews
            if (outputs != null) for (int i = 0; i < outputs.Length; i++) {
                    Item preview = ItemPool.GetDef(outputs[i].Item1);
                    preview.Quantity = outputs[i].Item2 * quantity;
                    scrolls.Add(Scroll.SlotRight(preview, null, SlotBackgroundType.Transparent));
                }


            scrolls.Add(Scroll.IdleProgress(IdleData));
            scrolls.Add(Scroll.IdleTotalProgress(IdleData));

            // icon
            scrolls.Add(Scroll.Slot(this, SlotBackgroundType.Transparent));

            scrolls.Add(Scroll.Space);

            ReRender();

            Scroll.Show(scrolls);
        }

        private void TryAssignRecipe() {
            if (IdleData.Value < 1) return;

            IdleData.Value--;

            AssignRecipe();
        }

        [JsonProperty] private int index = 0;
        [JsonProperty] private int complete_count = 0; // 已完成订单数
        [JsonProperty] private int discount = 0; // 折扣，0代表全价，9代表1折
        private const uint discount_max = 10;

        private static bool recipe_not_defined = false;
        private void AssignRecipe() {
            recipe_not_defined = false;

            Type randomRecipe = MarketRecipe;
            if (randomRecipe != null) {
                _MarketRecipe = randomRecipe;
            } else {
                List<Type> recipes = Recipes;
                if (recipes != null && recipes.Count > 0) {
                    // _MarketRecipe = Information.I.RandomOne(recipes);
                    _MarketRecipe = recipes[index % recipes.Count];
                    index++;
                } else {
                    recipe_not_defined = true;
                }
            }
            if (!recipe_not_defined) {
                discount = CalcDiscount();
            }
        }
        private int CalcDiscount() {

            int quantityBonus = (int)Log10QuantityLong - 4;

            uint discount0 = H.Hash(Information.I.FrameSeed, (uint)Salt.MarketDiscount);
            uint discount1 = (discount0 >> 4) % discount_max;
            discount0 %= discount_max;
            if (discount0 * discount1 % discount_max == 0) return M.Clamp(1, 10, (int)(discount_max) - quantityBonus);

            return M.Clamp(1, 10, (int)((discount_max * discount_max - discount0 * discount1) % discount_max) - quantityBonus);
        }

        /// <summary>
        /// MarketLike 自己负责实现交易功能
        /// </summary>
        private static bool Valid(int index) {
            for (int i = 0; i < used_slot_count; i++) {
                if (input_slots[i] == index) return false;
            }
            return true;
        }

        static MarketLike() {
            for (int i = 0; i < input_slots.Length; i++) {
                input_slots[i] = -1;
            }
        }

        private static string message;
        private static int[] input_slots = new int[C.HandLength];
        private static int used_slot_count = 0;
        public const int energy_cost_per_craft = 10;
    }
}
