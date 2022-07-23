
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{
    /// <summary>
    /// 重定向块
    /// 占领多个地块的物品，会使用重定向块
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class ItemRedirect : Item
    {
        //public override string Content => "wood";
        //public override Color4 ContentColor => Color4.red;
        public override string Content => null;

        public override string Decoration => null;


        private static bool __unlocked__ = false;

        public override void OnCreate() {
            base.OnCreate();
            A.Assert(__unlocked__);
        }
        public static ItemRedirect Create(Vec2 pos, bool canEnter) {
            __unlocked__ = true;
            ItemRedirect item = Creator.__CreateData<ItemRedirect>();
            __unlocked__ = false;

            item.can_enter = canEnter;
            item.redirect = pos;

            return item;
        }

        [JsonProperty] private bool can_enter;
        public override bool CanEnter => can_enter;

        [JsonProperty] private Vec2 redirect;
        public Vec2 RedirectPos => _Pos - redirect;

        public override bool Pickable => false;

        public static Vec2 Remover;
        protected override void BeforeRemoveFromMap() {
            A.Assert(Remover == redirect);
        }
    }
}
