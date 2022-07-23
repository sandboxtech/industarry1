
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{

    [cn("建筑")] public interface building { }




    // misc
    [cn("木船")] public interface boat : building { }
    [cn("直升飞机")] public interface helicopter : building { }
    [cn("书架")] public interface bookshelf : handcraft { }


    [output(typeof(culture))]
    [cn("钢琴")] public interface piano : building { }
    [cn("街灯")] public interface lamp : building { }



    /// <summary>
    /// pile!
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public abstract class FactoryLike : Item
    {
        /// <summary>
        /// _FactoryData.Recipe == null 代表着，多配方物品，正在选配方
        /// _FactoryData.Recipe != null 代表着，多配方物品已经选了配方，或者是单配方建筑
        /// </summary>
        /// 

        protected virtual bool PlantLike => false; // 像植物，则不会自动拾取、开始、中途提取
        public bool InProgress => working && IdleData.Value < IdleData.Max;
        public float TotalProgress => IdleData.TotalProgress;


        public override bool TryPickOnEnter => false;
        public override bool Absorbable => !working; // Recipe == null ? DefaultFactorySlots.Recipe == null : !DefaultFactorySlots.working;
        public override bool CanEnter => CanEnterPart(Vec2.zero);
        public override Vec2 Size => new Vec2(1, 1);

        public override bool Destructable => Absorbable && G.player.hand.HasSpace();  // 锤子拆除功能


        [JsonProperty] private Type active_recipe;
        [JsonIgnore]
        public Type ActiveRecipe {
            get => onlyRecipe ?? active_recipe; set {
                A.Assert(!working);
                active_recipe = value;
            }
        }


        [JsonProperty] public bool working { get; private set; }
        [JsonProperty] public long Value => !working ? 0 : IdleData.Value;
        [JsonProperty] private long max;
        protected virtual long Max => 10; // 默认上限100
        protected virtual long Del => C.Minute / C.TestMarketDelDivider; // 默认时间1秒


        // 在这些slots里的东西，一定是满足要求，能合并的。暂时假设都是ItemStackable
        [JsonProperty] private ItemList list;


        private const long max_craft_max = 1_000_000_000_000;
        private static long lastValue; // 动态面板
        private static string message;

        // 增速一条
        public override void OnCreate() {
            base.OnCreate();
            RecalcMax();
            IdleData = Idle.Create(0, 0, Del / C.TestFactoryDelDivider, 1);
            list = ItemList.Create(4); // 暂时输入最多4
        }
        protected override void AfterAbsorb() {
            base.AfterAbsorb();
            RecalcMax();
        }

        private void RecalcMax() {
            max = Max * M.Min(Quantity, max_craft_max);
        }
        public override Item SetQuantity(long quantity) {
            Item result = base.SetQuantity(quantity);
            RecalcMax();
            return result;
        }


        // 拆分时，配方也拆分
        protected override void PreprocessCopyWhenSplit(Item copy) {
            base.PreprocessCopyWhenSplit(copy);
            RecalcMax();
            (copy as FactoryLike).ActiveRecipe = ActiveRecipe;
        }
        protected override void PostprocessCopyWhenSplit(Item copy) {
            base.PostprocessCopyWhenSplit(copy);
            RecalcMax();
            (copy as FactoryLike)?.RecalcMax();
        }

        // 放置到地上时，如果此物品不需要输入，则自动开始运行
        protected override void OnAddToMap() {
            base.OnAddToMap();
            if (TryRunCondition) TryRun_IfNoInputsRequired();
        }
        protected virtual bool TryRunCondition => true;


        // 点击使用
        public override bool CanTap => true;
        public override void OnTap() {
            base.OnTap();
            _WorkPage();
        }

        public long TryCollectEnergyConsumption => 1;
        public override void TryEnter(Vec2 pos) {
            base.TryEnter(pos);
            if (DoTryCollect && G.player.attr.CanConsumeEnergy(TryCollectEnergyConsumption) && IdleData.Value > 0) {
                G.player.TryInteract(() => {
                    if (TryCollect_IfNoInputsRequired()) {
                        G.player.attr.TryConsumeEnergy(TryCollectEnergyConsumption);
                        ReRender();
                    }
                    // ShakeSelf();
                });
            }
        }

        public virtual List<Type> Recipes => null;
        public virtual Type Recipe => null;
        public override void _WorkPage() {
            Type recipeOnly = Recipe;
            List<Type> recipes = recipeOnly == null ? Recipes : null;
            if (recipeOnly == null && ActiveRecipe == null) {
                SelectRecipePage(recipes);
            } else {
                MainPage(true);
            }
        }

        public override Scroll ExtraUseScroll => Scroll.Button("选择配方", ChangeRecipe);
        public void ChangeRecipe() {
            if (ActiveRecipe == null) {
                SelectRecipePage(Recipes);
            } else {
                List<Scroll> scrolls = new List<Scroll>() {
                    Scroll.ReturnButton(OnUse),
                    Scroll.Text($"已选择配方: {Attr.CN(ActiveRecipe)}"),
                    Scroll.Text("需要放置在地上才能生效"),
                    Scroll.Button($"改变配方", () => {
                        ActiveRecipe = null;
                        OnUse();
                    })
                };
                Scroll.Show(scrolls);
            }
        }

        private void SelectRecipePage(List<Type> recipes) {

            List<Scroll> scrolls = new List<Scroll>();

            if (recipes == null) {
                scrolls.Add(Scroll.Text($"{Attr.CN(type)} 未定义功能"));
                scrolls.Add(Scroll.Destruct(this));
                Scroll.Show(scrolls);
                return;
            }

            int count = recipes.Count;
            for (int i = 0; i < count; i++) {
                Type recipe = recipes[i];
                if (recipe == null) {
                    scrolls.Add(Scroll.Space);
                    continue;
                }
                scrolls.Add(SlotButtonToPage(recipe));
            }
            if (!working) scrolls.Add(Scroll.Destruct(this));

            scrolls.Add(Scroll.Slot(this, SlotBackgroundType.Transparent));

            Scroll.Show(scrolls);
        }


        private Scroll SlotButtonToPage(Type type) {
            A.Assert(!working);
            string s = Attr.CN(type);
            recipeAttribute a = Attr.Get<recipeAttribute>(type);

            if (a != null && a.example == null) return Scroll.Slot($"{s} 未实装", this, null, SlotBackgroundType.Transparent);

            Type displayType = a == null ? type : a.example;

            return Scroll.Slot(s, ItemPool.GetDef(displayType).SetQuantity(1), () => {
                A.Assert(!working);
                ActiveRecipe = type;
                MainPage(true);
            }, SlotBackgroundType.Convex);
        }

        private bool hasValidRecipe = false;
        private (Type, long)[] inputs;
        private (Type, long)[] outputs;
        private Type onlyRecipe;
        private void TryGetRecipeContentFrom_recipeAttribute() {

            onlyRecipe = Recipe;

            // 提取配方
            Type selectedRecipe = ActiveRecipe;

            hasValidRecipe = onlyRecipe != null || selectedRecipe != null;
            if (!hasValidRecipe) return;

            active_recipe = selectedRecipe;

            recipeAttribute attr = Attr.Get<recipeAttribute>(selectedRecipe);
            // A.Assert(attr != null, () => $"未定义配方{selectedRecipe.Name}");

            if (attr != null) {
                inputs = attr.input_;
                outputs = attr.output;
            } else {
                inputs = null;
                outputs = new (Type, long)[] { (selectedRecipe, 1) };
            }


            A.Assert(inputs == null || list.available_slot_max >= inputs.Length); // haha
        }



        private void MainPage(bool first) {

            TryGetRecipeContentFrom_recipeAttribute();

            // ----------------------------------------------------------------------------------------------------

            // 数量影响参数。以后还会调整
            long storageFactor = PlantLike ? 1 : M.FakeCubicRoot(Log10QuantityLong);


            // 初始化文本
            if (working) {
                if (InProgress) {
                    message = "暂停";
                } else {
                    message = "完成";
                }
            } else if (first) {
                message = "开始";
            }

            // 按钮
            lastValue = IdleData.Value;

            List<Scroll> scrolls = new List<Scroll>();
            if (!PlantLike && OnMap) scrolls.Add(Scroll.Button(message, () => {
                if (working) {
                    // 停工
                    TryStop();

                } else {
                    // 开工
                    TryRun();
                }
                MainPage(false);
                return;
            }));

            // 进度
            if (working) {
                scrolls.Add(Scroll.Progress(() => $"{(int)(IdleData.Progress * 100),3}% {IdleData.ProgressTimeLeftDescription}", () => IdleData.Progress));
                scrolls.Add(Scroll.Progress(() => $"{IdleData.Value,3} - {IdleData.Max}", () => {

                    // 在这里检测变化，全面更新。比较低效
                    long v = IdleData.Value;
                    if (v != lastValue) {
                        v = lastValue;
                        MainPage(false);
                    }

                    return IdleData.TotalProgress;
                }));
            } else {
                scrolls.Add(Scroll.Progress($"加工时长 {Universe.TimespanDescription(IdleData.Del)}", 0));

                scrolls.Add(Scroll.Progress($"最高容量 {max}", 0));
            }

            // 输入框
            CreateInputView(scrolls);

            // 输出预览
            if (outputs != null) for (int i = 0; i < outputs.Length; i++) {
                    Item preview = ItemPool.GetDef(outputs[i].Item1);
                    if (working) {
                        preview.Quantity = M.Max(1, IdleData.Value * outputs[i].Item2); // 展示已有多少个
                    } else {
                        preview.Quantity = outputs[i].Item2;
                    }
                    if (preview.Quantity == 0) preview.SemiTransparent = true;
                    scrolls.Add(Scroll.SlotRight(preview, null, SlotBackgroundType.Transparent));
                }


            if (!working && onlyRecipe == null) {
                // 更改配方按钮
                scrolls.Add(Scroll.Button("更改配方", () => {
                    if (list.occupied_slot_count > 0) {
                        for (int i = 0; i < list.available_slot_max; i++) {
                            Item item = list[i];
                            if (item != null) {
                                if (G.player.hand.TryAbsorbOrAdd(item)) {
                                    if (list.occupied_slot_count == 0) break;
                                } else {
                                    break;
                                }
                            }
                        }
                    }
                    if (list.occupied_slot_count == 0) {
                        ActiveRecipe = null;
                    }
                    _WorkPage();
                }));
            }

            // 拆除自身按钮
            if (working && !PlantLike) {
                scrolls.Add(Scroll.Button("拿走产出", () => {
                    if (outputs == null || G.player.hand.HasSpace(outputs.Length)) {
                        long crafted = IdleData.Value;
                        if (crafted != 0) {
                            if (inputs != null) for (int i = 0; i < inputs.Length; i++) {
                                    //if (list[i] == null) { // BUG
                                    //    Detach();
                                    //    Scroll.Close();
                                    //    return;
                                    //}
                                    // 消耗输入
                                    list[i].Quantity -= crafted * inputs[i].Item2;
                                }

                            if (outputs != null) for (int i = 0; i < outputs.Length; i++) {
                                    // 获取输出
                                    G.player.hand.TryAbsorbOrAdd(Item.Of(outputs[i].Item1).SetQuantity(outputs[i].Item2 * crafted));
                                }
                            IdleData.Value -= crafted;
                            if (inputs != null) { // 无输入配方，不减少数量
                                IdleData.Max -= crafted;
                            }

                            if (IdleData.Max == 0) {
                                // Debug.Log($"before {working}");
                                // TryStop();
                                IdleData.Value = 0;
                                IdleData.Max = 0;
                                working = false;
                                // Debug.Log($"after {working}");
                                return;
                            }
                        }
                    }
                    else {

                    }
                }));
            } else {
                if (!working) scrolls.Add(Scroll.Destruct(this));
            }

            // 展示效率
            if (storageFactor > 1 && working && !PlantLike) {
                scrolls.Add(Scroll.Text($"容量加成 {(int)((storageFactor) * 100)}%"));
            }

            // icon
            scrolls.Add(Scroll.Slot(this, SlotBackgroundType.Transparent));

            ReRender();
            Scroll.Show(scrolls);
        }

        public bool TryStop() {
            if (!working) return false;
            // Debug.Log("before stop");
            // 停工
            if (outputs == null || G.player.hand.HasSpace(outputs.Length)) {

                long crafted = IdleData.Value;
                if (crafted != 0) {
                    if (inputs != null) for (int i = 0; i < inputs.Length; i++) {
                            //if (list[i] == null) { // BUG
                            //    Detach();
                            //    Scroll.Close();
                            //    return false;
                            //}
                            // 消耗输入
                            list[i].Quantity -= crafted * inputs[i].Item2;
                        }

                    if (outputs != null) for (int i = 0; i < outputs.Length; i++) {
                            // 获取输出
                            G.player.hand.TryAbsorbOrAdd(Item.Of(outputs[i].Item1).SetQuantity(outputs[i].Item2 * crafted));
                        }
                }

                IdleData.Value = 0;
                IdleData.Max = 0;
                working = false;
                message = "开始";
                return true;
            } else {
                message = "背包太满";
                return false;
            }
        }
        public bool TryRun() {
            if (working) return false;

            long max_craft = max_craft_max;
            max_craft = M.Min(max_craft, max); // 不能合成超过进度条允许的最大量

            if (inputs != null) for (int i = 0; i < inputs.Length; i++) {

                    Item item = list[i];
                    if (item == null) {
                        message = $"{Attr.CN(inputs[i].Item1)} 缺失";
                        return false;
                    }

                    max_craft = M.Min(max_craft, item.Quantity / inputs[i].Item2); // 找到最大加工数量

                    if (max_craft == 0) {
                        message = $"{Attr.CN(inputs[i].Item1)} 不足";
                        return false;
                    }
                }

            IdleData.Max = max_craft;
            working = true;
            IdleData.Inc = 1; // 加速

            IdleData.Clear(); // v time

            message = "加工中";

            return true;
        }

        private void CreateInputView(List<Scroll> scrolls) {
            if (inputs == null) return;

            for (int _i = 0; _i < inputs.Length; _i++) {
                int i = _i;
                Type type = inputs[i].Item1;
                long value = inputs[i].Item2;
                if (inputs[i].Item1 == null) continue;

                // string cn = value <= 1 ? Attr.CN(type) : $"{Attr.CN(type)} {value}";

                Item view;
                if (!working) {
                    view = list[i];
                    if (view == null) {
                        view = ItemPool.GetDef(type);
                        view.Quantity = value;
                        view.SemiTransparent = true;
                    }
                } else {
                    view = ItemPool.GetDef(type);
                    view.SemiTransparent = false;
                    // view.Quantity = M.Max(0, view.Quantity - IdleData.Value * value); // BUG
                    if (list[i] == null) { // BUG in version 1
                        Detach();
                        Scroll.Close();
                        return;
                    }
                    view.Quantity = M.Max(0, list[i].Quantity - IdleData.Value * value);
                }

                //Item view = s.working ? ItemPool.GetDef(type) : s.s[j];
                //if (!s.working && view == null) { }
                //if (s.working) view.Quantity = s.s[j].Quantity - s.v.Value * value;

                scrolls.Add(Scroll.Slot(Attr.CN(type), view, () => {
                    Item slot = list[i];
                    if (working) {
                        // 工作中，不能交互
                        Item preview = ItemPool.GetDef(type);
                        preview.Quantity = M.Max(1, slot.Quantity - IdleData.Value * value);
                        return;
                    }

                    Item hand = G.player.HandItem;

                    if (slot != null && G.player.hand.HasSpace()) {
                        // 取出输入
                        if (hand == null) {
                            G.player.HandItem = slot;
                            MainPage(false);
                            return;
                        } else {
                            G.player.hand.TryAbsorbOrAdd(slot);
                            MainPage(false);
                            return;
                        }
                    }

                    if (slot == null && hand != null && Ty.Is(hand.type, type)) {
                        // 加入输入
                        long needed = max * inputs[i].Item2;
                        if (hand.Quantity <= needed) {
                            list[i] = hand;
                        } else {
                            list[i] = hand.Split(needed);
                            Item.ShakeHand();
                        }
                        MainPage(false);
                        return;
                    }

                    return;
                }, working ? SlotBackgroundType.Transparent : SlotBackgroundType.Concave));

            }
        }

        protected virtual bool DoTryCollect => false;
        /// <summary>
        /// 中途取出产出
        /// </summary>
        public bool TryCollect_IfNoInputsRequired() {

            // 提取配方
            Type selectedRecipe = ActiveRecipe;

            // 特殊改变
            if (selectedRecipe == null) return false;

            //recipeAttribute attr = Attr.Get<recipeAttribute>(selectedRecipe);
            //A.Assert(attr != null, () => $"未定义配方{selectedRecipe.Name}");

            //(Type, long)[] inputs = attr.input_;
            //(Type, long)[] outputs = attr.output;

            TryGetRecipeContentFrom_recipeAttribute();

            // ----------------------------------------------------------------------------------------------------

            if (G.player.hand.NoSpace(outputs.Length)) return false; // 装不下


            long crafted = IdleData.Value;
            if (crafted != 0) {
                if (inputs != null) for (int i = 0; i < inputs.Length; i++) {
                        // 消耗输入
                        list[i].Quantity -= crafted * inputs[i].Item2;
                    }

                if (outputs != null) for (int i = 0; i < outputs.Length; i++) {
                        // 获取输出
                        G.player.hand.TryAbsorbOrAdd(Item.Of(outputs[i].Item1).SetQuantity(outputs[i].Item2 * crafted));
                    }

                IdleData.Value -= crafted;
                if (inputs != null) { // 无输入配方，不减少数量
                    IdleData.Max -= crafted;
                }

                return true;
            } else {
                return false;
            }
        }


        /// <summary>
        /// 无成本开始建筑
        /// </summary>
        public void TryRun_IfNoInputsRequired() {

            TryGetRecipeContentFrom_recipeAttribute();

            // 尝试自动开始运行
            if (hasValidRecipe && inputs == null) {
                // 开始
                long max_craft = max_craft_max;
                max_craft = M.Min(max_craft, max); // 不能合成超过进度条允许的最大量

                IdleData.Max = max_craft;
                working = true;
                IdleData.Inc = 1; // 加速

                IdleData.Clear(); // v time
            }
        }


        /////////////////////////  link



        public bool HasLink => references.Count > 0;

        [JsonProperty] public bool auto_working;
        [JsonProperty] public List<Link> references;


        public void TryAutoRun() {

        }

        public void TryAutoStop() {

        }
    }


    public struct Link
    {
        public Type t; // type
        public long q; // quantity
        public int d; // direction
        public int x; // subpositionx
        public int y; // subpositiony
    }
}
