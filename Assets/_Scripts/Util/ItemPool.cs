
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{
    public static class ItemPool
    {
        private static Dictionary<Type, Item> pool = new Dictionary<Type, Item>();

        public static T GetImpl<T>() where T : Item => GetImpl(typeof(T)) as T;

        public static Item GetImpl(Type type) {
            A.Assert(Ty.Is<Item>(type));
            if (!pool.TryGetValue(type, out Item item)) {
                item = Creator.__CreateData(type) as Item;
                A.Assert(item != null);
                pool.Add(type, item);
            }
            return item;
        }

        private static Dictionary<Type, Item> poolOfSimpleItem = new Dictionary<Type, Item>();

        public static Item GetDef<T>() => GetDef(typeof(T));

        public static Item GetDef(Type type) {
            if (!poolOfSimpleItem.TryGetValue(type, out Item result)) {
                result = Item.Of(type);
                result.type = type;
                poolOfSimpleItem.Add(type, result);
            }
            result.Quantity = 0;
            result.SemiTransparent = false;
            return result;
        }
    }
}
