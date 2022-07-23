
using System;
using System.Collections.Generic;
using System.Linq;

namespace W
{

    public static class Ty
    {
        public static bool Is<T>(Type child) {
            return typeof(T).IsAssignableFrom(child);
        }

        private static TypeRelation relation = new TypeRelation((Type child, Type parent) => parent.IsAssignableFrom(child));
        public static bool Is(Type child, Type parent) => relation.Check(child, parent);
    }

}
