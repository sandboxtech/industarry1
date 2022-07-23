
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{

    public class recipe_halfAttribute : Attribute
    {
        public readonly (Type, long)[] input_;
        public recipe_halfAttribute(Type i0, long i0v) {
            input_ = new (Type, long)[] { (i0, i0v) };
        }
        public recipe_halfAttribute(Type i0, long i0v, Type i1, long i1v) {
            input_ = new (Type, long)[] { (i0, i0v), (i1, i1v), };
        }
    }


    public class recipeAttribute : Attribute
    {
        public const long dv = 1; // default value

        public readonly Type example;
        public readonly (Type, long)[] input_;
        public readonly (Type, long)[] output;


        public recipeAttribute() {

        }

        // recipe 这个很特殊，用到了
        public recipeAttribute(int _, Type i, Type o, long ov = 1) {
            recipe_halfAttribute attr = Attr.Get<recipe_halfAttribute>(i);
            input_ = attr.input_;
            output = new (Type, long)[] { (o, ov) };
            example = o;
        }

        // 1

        public recipeAttribute(bool _, Type o0) : this(_, o0, dv) { }
        public recipeAttribute(bool _, Type o0, long o0v) { // 纯支出
            input_ = null;
            output = new (Type, long)[] { (o0, o0v) };
            example = o0;
        }

        public recipeAttribute(Type i0, bool _) : this(i0, dv, _) { }
        public recipeAttribute(Type i0, long i0v, bool _) { // 纯消耗
            input_ = new (Type, long)[] { (i0, i0v) };
            output = null;
            example = i0;
        }

        // 2
        public recipeAttribute(Type i0, bool _, Type o0) : this(i0, dv, _, o0, dv) { }
        public recipeAttribute(Type i0, bool _, Type o0, long o0v) : this(i0, dv, _, o0, o0v) { }
        public recipeAttribute(Type i0, long i0v, bool _, Type o0) : this(i0, i0v, _, o0, dv) { }
        public recipeAttribute(Type i0, long i0v, bool _, Type o0, long o0v) {
            input_ = new (Type, long)[] { (i0, i0v) };
            output = new (Type, long)[] { (o0, o0v) };
            example = o0;
        }

        // 3
        public recipeAttribute(Type i0, Type i1, bool _, Type o0) {
            input_ = new (Type, long)[] { (i0, dv), (i1, dv), };
            output = new (Type, long)[] { (o0, dv), };
            example = o0;
        }
        public recipeAttribute(Type i0, bool _, Type o0, Type o1) {
            input_ = new (Type, long)[] { (i0, dv), };
            output = new (Type, long)[] { (o0, dv), (o1, dv), };
            example = o0;
        }

        public recipeAttribute(Type i0, long i0v, Type i1, long i1v, bool _, Type o0) : this(i0, i0v, i1, i1v, _, o0, dv) { }
        public recipeAttribute(Type i0, long i0v, Type i1, long i1v, bool _, Type o0, long o0v) {
            input_ = new (Type, long)[] { (i0, i0v), (i1, i1v), };
            output = new (Type, long)[] { (o0, o0v), };
            example = o0;
        }
        public recipeAttribute(Type i0, long i0v, bool _, Type o0, long o0v, Type o1, long o1v) {
            input_ = new (Type, long)[] { (i0, i0v), };
            output = new (Type, long)[] { (o0, o0v), (o1, o1v), };
            example = o0;
        }

        // 4
        public recipeAttribute(Type i0, Type i1, bool _, Type o0, Type o1) {
            input_ = new (Type, long)[] { (i0, dv), (i1, dv), };
            output = new (Type, long)[] { (o0, dv), (o1, dv), };
            example = o0;
        }
        public recipeAttribute(Type i0, long i0v, Type i1, long i1v, bool _, Type o0, long o0v, Type o1, long o1v) {
            input_ = new (Type, long)[] { (i0, i0v), (i1, i1v), };
            output = new (Type, long)[] { (o0, o0v), (o1, o1v), };
            example = o0;
        }


        public recipeAttribute(Type i0, long i0v, Type i1, long i1v, Type i2, long i2v, bool _, Type o0, long o0v) {
            input_ = new (Type, long)[] { (i0, i0v), (i1, i1v), (i2, i2v), };
            output = new (Type, long)[] { (o0, o0v), };
            example = o0;
        }
    }
}
