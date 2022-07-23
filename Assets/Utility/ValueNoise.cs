
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{
    public static class ValueNoise
    {
        public static float Get(uint salt, uint x, float c) {
            //uint x = (uint)(G.now / C.Second);
            //uint x_ = x + 1;
            //float c = G.secondT;
            uint x_ = x + 1;
            c = M.Clamp01(c);
            c = Transformation.ease_in_out_sine(c);
            float hash_x = H.HashFloat(x, salt);
            float hash_x_ = H.HashFloat(x_, salt);
            float result = c * hash_x + (1 - c) * hash_x_;
            return result;
        }
    }
}
