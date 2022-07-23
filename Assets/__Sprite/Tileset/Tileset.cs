
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{

    public abstract class Item8x6 : Item
    {
        private string KeyIfOn(Vec2 pos) { // no POS
            int index = TileUtility.Index_8x6((Vec2 offset) => IsSameTypeOn(pos + offset));
            if (IgnoreTileIndex(index)) return null;
            int i = index + (Height == 1 ? 0 : (int)(G.map.HashcodeOfTile(pos) % Height) * TileUtility.Size8x6);
            return StrPool.Concat(KeyBase, i);
        }
        public override string Content => !OnMap ? StrPool.Concat(KeyBase, TileUtility.Size8x6 - 1) : KeyIfOn(_Pos); // 不是Key是Content
        protected virtual string KeyBase => type.Name;
        protected virtual int Height => 1;
        protected virtual bool IgnoreTileIndex(int index) => index == TileUtility.Null8x6;

        public virtual bool IsSameTypeOn(Vec2 pos) {
            Item layer = G.map[pos]; // 唯一变化
            return layer != null && layer.type == type;
        }
    }


    public abstract class Item4x4 : Item
    {
        private string KeyIfOn(Vec2 pos) { // no POS
            int index = TileUtility.Index_4x4((Vec2 offset) => IsSameTypeOn(pos + offset));
            int i = index + (Height == 1 ? 0 : (int)(G.map.HashcodeOfTile(pos) % Height) * TileUtility.Size4x4);
            return StrPool.Concat(KeyBase, i);
        }
        public override string Content => !OnMap ? StrPool.Concat(KeyBase, TileUtility.Size4x4 - 1) : KeyIfOn(_Pos); // 不是Key是Content
        protected virtual string KeyBase => type.Name;
        protected virtual int Height => 1;

        public virtual bool IsSameTypeOn(Vec2 pos) {
            Item layer = G.map[pos]; // 唯一变化
            return layer != null && layer.type == type;
        }
    }


    public interface ILayer
    {
        string LayerContent { get; }
        Color4 LayerColor { get; }
    }


    public abstract class Ground4x4 : Item
    {
        private string KeyIfOn(Vec2 pos) { // no POS
            int index = TileUtility.Index_4x4((Vec2 offset) => IsSameTypeOn(pos + offset));
            int i = index + (Height == 1 ? 0 : (int)(G.map.HashcodeOfTile(pos) % Height) * TileUtility.Size4x4);
            return StrPool.Concat(KeyBase, i);
        }
        public override string LayerContent => KeyIfOn(_Pos);
        public override string Content => StrPool.Concat(KeyBase, 4 * 4 - 1);
        public override Color4 ContentColor => LayerColor;

        protected virtual string KeyBase => type.Name;
        protected virtual int Height => 1;

        public abstract bool IsSameTypeOn(Vec2 pos);
    }


    public abstract class Ground8x6 : Item
    {
        private string KeyIfOn(Vec2 pos) { // no POS
            int index = TileUtility.Index_8x6((Vec2 offset) => IsSameTypeOn(pos + offset));
            if (IgnoreTileIndex(index)) return null;
            int i = index + (Height == 1 ? 0 : (int)(G.map.HashcodeOfTile(pos) % Height) * TileUtility.Size8x6);
            return StrPool.Concat(KeyBase, i);
        }
        public override string LayerContent => KeyIfOn(_Pos);
        public override string Content => StrPool.Concat(KeyBase, 6 * 8 - 1);
        public override Color4 ContentColor => LayerColor;

        public const int MagicNine = 9;
        public const int MagicThirtyThree = 33;

        protected virtual string KeyBase => type.Name;
        protected virtual int Height => 1;
        protected virtual bool IgnoreTileIndex(int index) => index == TileUtility.Null8x6;

        public abstract bool IsSameTypeOn(Vec2 pos);
    }
}
