
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{

    //public interface IInteractable
    //{
    //    bool CanInteract { get; }
    //    void OnInteract();
    //}


    public interface __IItem_Container__
    {
        /// <summary>
        /// 解除子物体关系
        /// </summary>
        void __Remove_Item__(Item item);

        /// <summary>
        /// 恢复 __属性__ 。一般要在序列化后调用
        /// [System.Runtime.Serialization.OnDeserialized]
        /// private void OnDeserialized(System.Runtime.Serialization.StreamingContext sc)
        /// </summary>
        void __Rebind_Items__();
    }

    /// <summary>
    /// 物品，要么在items里，要么在map里，要么都不是
    /// </summary>
    public interface __IItem__
    {
        __IItem_Container__ __Container__ { get; set; }
        int __ItemsIndex__ { get; set; }
        Vec2 __Pos__ { get; set; }

        void __OnAddToMap__();
        void __OnRemoveFromMap__();

    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class Item : __IItem__, ICreatable, ILayer
    {
        // dynamic creator
        public static Item Of<T>() => Of(typeof(T));
        public static Item Of(Type type) => implAttribute.Of(type);





        [JsonIgnore] __IItem_Container__ __IItem__.__Container__ { get; set; }
        [JsonIgnore] int __IItem__.__ItemsIndex__ { get; set; }
        [JsonIgnore] Vec2 __IItem__.__Pos__ { get; set; }

        [JsonIgnore] public bool OnMap => (this as __IItem__).__Container__ as MapItemMatrix != null;
        [JsonIgnore] public Vec2 _Pos => (this as __IItem__).__Pos__;
        [JsonIgnore] public int ItemListIndex => (this as __IItem__).__ItemsIndex__;
        [JsonIgnore] public ItemList ItemList => (this as __IItem__).__Container__ as ItemList;
        public void Detach() => (this as __IItem__).__Container__?.__Remove_Item__(this);



        [System.Runtime.Serialization.OnSerializing]
        private void OnSerializing(System.Runtime.Serialization.StreamingContext sc) {
            t = type == null ? null : type.FullName;
        }
        [System.Runtime.Serialization.OnDeserialized]
        private void OnDeserialized(System.Runtime.Serialization.StreamingContext sc) {
            type = t == null ? null : Type.GetType(t, true);
        }

        public virtual void OnCreate() {
        }

        [JsonIgnore] public virtual string Description => type == null ? $"class({GetType().Name})" : (Attr.CN(type) ?? $"type({type.Name})");


        [JsonProperty]
        private string t { get; set; } // 名称变换，稍微减少存档体积
        [JsonIgnore]
        public Type type { get; set; }


        #region default initializer
        /// <summary>
        /// 由于大多数物体只需要少数参数初始化，提供了几个虚函数简化操作
        /// </summary>
        public virtual Item SetQuantity(long quantity) { this.Quantity = quantity; return this; } // 链式调用支持
        #endregion


        // public virtual void OnInteract() { throw new Exception(); } // 地图上被点击。需要实现IInteractable改变行为
        // public virtual void OnPassby() { throw new Exception(); } // 玩家经过

        public virtual void OnUse() { OnUseStackable(); } // 物品栏双击

        public virtual bool TryInteract(Vec2 pos) => false;
        public virtual void OnInteract(Vec2 pos) { }

        public virtual bool CanTap => false;
        public virtual void OnTap() { }

        public virtual void TryMove(Vec2 pos) { } // 玩家尝试移动
        public virtual void OnMove(Vec2 pos) { } // 玩家移动


        public virtual void TryEnter(Vec2 pos) { } // 玩家尝试移动
        public virtual bool CanEnter => CanEnterPart(new Vec2(0, 0)); // 能否被踩踏

        public virtual bool TryPickOnEnter => true; // 被踩踏时是否能捡起
        public virtual bool ShakeOnEnter => true; // 被踩踏时是否震动
        public virtual void OnEnter(Vec2 pos) {
            if (Pickable && TryPickOnEnter) {
                if (G.player.hand.HasSpace()) {
                    if (Absorbable) {
                        G.player.hand.TryAbsorbOrAdd(this);
                    } else {
                        G.player.hand.TryAdd(this);
                    }
                } else {
                    if (ShakeOnEnter) ShakePlayerPos();
                }
            } else {
                if (ShakeOnEnter) ShakePlayerPos();
            }
        }




        public void ReRender() {
            if (OnMap) { Map.ReRender9At(_Pos); } else if (ItemList != null && ItemListIndex >= 0) UI_Hand.I.RenderItemAt(ItemListIndex);
        }

        public static void ShakeHand() {
            G.player.ReRenderAndBounceHand();
        }
        protected void ShakePlayerPos() {
            Map.ShakeAt(G.player.position);
        }
        protected void ShakeSelf() {
            if (OnMap) Map.ShakeAt(_Pos);
        }


        #region ground
        // ground
        [JsonIgnore] public virtual string LayerContent => null;
        [JsonIgnore] public virtual Color4 LayerColor => Color4.white;
        #endregion

        #region main
        public const string ColorString = "Color";
        public const string TileWithBorderString = "TileWithBorder";


        public virtual Type ContentTypeRedirect => null;

        [JsonIgnore]
        public virtual string Content {
            get {
                A.Assert(type != null, () => GetType().Name);
                // if (type == null) return null;
                Type contentRedirect = ContentTypeRedirect;
                return contentRedirect != null ? contentRedirect.Name : type.Name;
            }
        } // type == null ? null : type.Name;
        [JsonIgnore] public virtual string Container => null;
        [JsonIgnore] public virtual string Highlight => null;
        [JsonIgnore]
        public virtual string Decoration {
            get {
                if (Quantity > 1 || G.settings.quantity_index_01) {
                    Calc999();
                    return StrPool.Concat("index", (int)(first999));
                }
                return null;
            }
        }

        [JsonIgnore] protected string DefaultContentFirstFrame => StrPool.Concat(DefaultContent, 0); // $"{type.Name}_0";
        [JsonIgnore] protected string DefaultContainerFirstFrame => StrPool.Concat(DefaultContent, "_container_0"); // $"{type.Name}_container_0";
        [JsonIgnore] protected string DefaultHighlightFirstFrame => StrPool.Concat(DefaultContent, "_highlight_0"); // $"{type.Name}_highlight_0";

        [JsonIgnore] protected string DefaultContentIdle => StrPool.Concat(DefaultContent, "_idle"); //  $"{type.Name}_idle";
        [JsonIgnore] protected string DefaultContainerIdle => StrPool.Concat(DefaultContent, "_container_idle"); // $"{type.Name}_container_idle";
        [JsonIgnore] protected string DefaultHighlightIdle => StrPool.Concat(DefaultContent, "_highlight_idle"); // $"{type.Name}_highlight_idle";

        [JsonIgnore]
        protected string DefaultContent {
            get {
                Type contentRedirect = ContentTypeRedirect;
                return StrPool.GetTypeName(contentRedirect!= null ? contentRedirect : type);
            }
        }
        [JsonIgnore] protected string DefaultContainer => StrPool.Concat(DefaultContent, "_container"); // $"{type.Name}_container";
        [JsonIgnore] protected string DefaultHighlight => StrPool.Concat(DefaultContent, "_highlight"); //$"{type.Name}_highlight";
        [JsonIgnore] protected string DefaultDecoration => StrPool.Concat(DefaultContent, "_decoration"); //$"{type.Name}_decoration";


        #endregion

        #region color

        [JsonIgnore] public bool SemiTransparent = false; // 用于 preview

        [JsonIgnore] public virtual Color4 ContentColor => Color4.white;
        [JsonIgnore] public virtual Color4 ContainerColor => Color4.white;
        [JsonIgnore] public virtual Color4 HighlightColor => Color4.white;
        [JsonIgnore] public virtual Color4 DecorationColor => ((Quantity > 1 || G.settings.quantity_index_01) ? ColorOfCount000(count000) : Color4.white).SetA(OnMap ? G.settings.quantity_index_opacity : 1f);
        #endregion

        #region belt
        [JsonIgnore] public virtual Item Left => null;
        [JsonIgnore] public virtual Item Right => null;
        [JsonIgnore] public virtual Item Up => null;
        [JsonIgnore] public virtual Item Down => null;
        #endregion





        #region factory
        // [JsonProperty] public FactoryLikeDataPage _FactoryData { get; protected set; }

        public virtual void _WorkPage() { }

        [JsonProperty] public Type _MarketRecipe { get; protected set; }

        #endregion

        #region idle
        [JsonProperty] public Idle IdleData { get; protected set; }
        #endregion




        #region stackable
        [JsonProperty] private long quantity = 1;
        [JsonIgnore]
        public long Quantity {
            get => quantity; set {
                //if (value < 0) {
                //    Debug.LogWarning(value);
                //}
                A.Assert(value >= 0);
                if (value == 0) {
                    Detach();
                } else {
                    quantity = value;
                }
            }
        }

        public virtual bool Absorbable => true;

        public bool TryAbsorb(Item other) {
            if (!CanAbsorb(other)) {
                return false;
            }
            Absorb(other);
            return true;
        }
        public bool CanAbsorb(Item other) {
            return type == other.type && Absorbable && other.Absorbable && (Quantity + other.Quantity) >= 0; // overflow
        }
        private void Absorb(Item other) {
            other.Detach();
            Quantity += other.Quantity;
            if (Quantity < 0) Quantity = long.MaxValue; // overflow
            AfterAbsorb();
        }
        protected virtual void AfterAbsorb() {

        }

        public static Color4 ColorOfCount000(int count000) {
            switch (count000) {
                case 0:
                    return Color4.white;
                case 1:
                    return Color4.green;
                case 2:
                    return Color4.cyan;
                case 3:
                    return Color4.blue;
                case 4:
                    return Color4.magenta;
                case 5:
                    return Color4.red;
                case 6:
                    return Color4.orange;
                default:
                    return Color4.orange;
            }
        }
        private long first999; // 前三位数字
        private int count000; // 000 有多少个
        private void Calc999() {
            A.Assert(Quantity > 0);
            count000 = 0;
            long first999_next = Quantity;
            while (true) {
                first999 = first999_next;
                first999_next /= 1000;
                if (first999_next == 0) break;
                count000++;
            }
        }
        private static int _split_page;
        private void OnUseStackable() {
            Calc999();
            const int NearIntMax = 1_000_000_000;

            int split_init_max = Quantity > NearIntMax ? NearIntMax : (int)Quantity; // 最多分

            _split_page = M.Clamp(0, split_init_max, _split_page);

            Scroll.Show(
                    Scroll.CloseButton,
                    ExtraUseScroll,
                    Scroll.Slot(this, SlotBackgroundType.Transparent),
                    // Quantity <= 1 ? Scroll.Empty : Scroll.Prgress($"数量 {Quantity} {ExtraQuantityDescription}", (float)first999 / 1000),
                    Scroll.Button("百科", () => { WikiPage.Of(type); }),
                    Scroll.Space,

                    Scroll.Button(() => $"{(G.settings.drop_one_or_all ? "放下一个" : "放下全部")}", () => { G.settings.drop_one_or_all = !G.settings.drop_one_or_all; }),

                    Quantity <= 1 ? Scroll.Empty : Scroll.Button("拆分一个", () => {
                        if (G.player.hand.HasSpace()) {
                            G.player.hand.TryAdd(Split(1));
                            ShakeHand();
                            OnUse();
                        }
                    }),
                    Quantity <= 1 ? Scroll.Empty : Scroll.Button("拆分一组", () => {
                        if (G.player.hand.HasSpace()) {
                            long q = Log10QuantityLong;
                            long q_sub = 1;
                            for (int i = 1; i < q; i++) {
                                q_sub *= 10;
                            }
                            G.player.hand.TryAdd(Split(q_sub));
                            ShakeHand();
                            OnUse();
                        }
                    }),
                    Quantity <= 1 ? Scroll.Empty : Scroll.Button("拆分指定", () => {
                        if (_split_page == 0) return;
                        if (_split_page == split_init_max && Quantity <= NearIntMax) return;
                        if (G.player.hand.HasSpace()) {
                            G.player.hand.TryAdd(Split(_split_page));
                            ShakeHand();
                            OnUse();
                        }
                    }),
                    Quantity <= 1 ? Scroll.Empty : Scroll.SliderInt(() => $"指定数量 {_split_page}", (int x) => _split_page = x, _split_page, split_init_max, 0),
                    Scroll.Text($"总数 {Quantity}"),
                    Scroll.Space,
                    Scroll.Empty
                );
        }
        public Item Split(long q) {
            A.Assert(q > 0 && q < Quantity, () => $"{q} {Quantity}");
            Item copy = Of(type);
            PreprocessCopyWhenSplit(copy);
            copy.Quantity = q;
            Quantity -= q;
            PostprocessCopyWhenSplit(copy);
            return copy;
        }
        protected virtual void PreprocessCopyWhenSplit(Item copy) {

        }
        protected virtual void PostprocessCopyWhenSplit(Item copy) {

        }

        public virtual Scroll ExtraUseScroll => Scroll.Empty;
        //protected virtual Scroll ExtraDesctructScroll => Scroll.Empty;
        //protected virtual string ExtraQuantityDescription => null;
        public float Log10Quantity => Quantity == 0 ? 0 : (float)Math.Log10(Quantity);
        public long Log10QuantityLong => Quantity == 0 ? 0 : (long)Math.Log10(Quantity);

        #endregion



        #region large

        public virtual Vec2 Size => Vec2.zero;

        public virtual bool Pickable => true;
        public virtual bool Destructable => false;

        public virtual void Destruct() {
            A.Assert(Destructable);
            G.player.hand.TryAbsorbOrAdd(this);
        }

        public virtual bool _DroppableAt(Vec2 pos) {

            if (!DroppableAt(pos)) { // 未定义size默认1x1
                return false;
            }

            if (!G.map.IsInside(pos + Size)) {
                return false; // 两角不在地图里面
            }

            if (TileUtility.IsInRectOfSize(G.player.position, pos, Size)) {
                return false; // 玩家在里面
            }

            for (int i = 0; i < Size.x; i++) {
                for (int j = 0; j < Size.y; j++) {
                    Vec2 p = new Vec2(i, j);
                    Item item = G.map[pos + p];
                    if (item != null) {
                        return false;
                    } else if (!DroppableAt(pos + p)) {
                        return false;
                    }
                }
            }
            return true;
        }
        protected virtual bool DroppableAt(Vec2 pos) => G.map.CanEnter(pos);

        /// <summary>
        /// 定义了一个多格物体的碰撞体积
        /// </summary>
        public virtual bool CanEnterPart(Vec2 pos) => Size.x * Size.y == 0 ? true : false;
        void __IItem__.__OnAddToMap__() {
            BeforeAddTOMap();
            OnAddToMap();
        }
        protected virtual void OnAddToMap() {
            // ...
        }
        private void BeforeAddTOMap() {
            int width = Size.x;
            if (width == 0) return;
            int height = Size.y;
            if (height == 0) return;

            Vec2 pos = _Pos;
            A.Assert(G.map.IsInside(pos));
            for (int i = 0; i < Size.x; i++) {
                for (int j = 0; j < Size.y; j++) {
                    if (i == 0 && j == 0) continue;
                    Vec2 p = new Vec2(i, j);
                    G.map._tiles[i + pos.x, j + pos.y] = ItemRedirect.Create(p, CanEnterPart(p));
                }
            }
        }
        void __IItem__.__OnRemoveFromMap__() {
            BeforeRemoveFromMap();
            _BeforeOnRemoveFromMap();
        }
        protected virtual void BeforeRemoveFromMap() {
            // ...
        }
        private void _BeforeOnRemoveFromMap() {
            int width = Size.x;
            if (width == 0) return;
            int height = Size.y;
            if (height == 0) return;

            Vec2 pos = _Pos;
            int c = 0;
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    if (i == 0 && j == 0) continue;

                    ItemRedirect.Remover = new Vec2(i, j);

                    ItemRedirect item = G.map._tiles[i + pos.x, j + pos.y] as ItemRedirect;
                    item.Detach();
                    c++;
                }
            }
            ItemRedirect.Remover = Vec2.invalid;
        }

        #endregion
    }
}
