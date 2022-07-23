
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{
    public class TypeRelation
    {
        private Dictionary<Type, Dictionary<Type, bool>> buffer;

        private Func<Type, Type, bool> checker;
        public TypeRelation(Func<Type, Type, bool> checker) {
            A.Assert(checker != null);
            this.checker = checker;
            buffer = new Dictionary<Type, Dictionary<Type, bool>>();
        }

        public bool Check(Type child, Type parent) {
            A.Assert(child != null);
            A.Assert(parent != null);

            if (!buffer.TryGetValue(child, out Dictionary<Type, bool> value)) {
                value = new Dictionary<Type, bool>();
                buffer.Add(child, value);
            }

            if (value.TryGetValue(parent, out bool result)) {
                return result;
            }

            result = checker(child, parent);

            value.Add(parent, result);
            return result;
        }
    }
}
