
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{
    [impl(typeof(magnet_tool_))]
    [cn("磁铁")] public interface magnet_tool : tool { }


    public class magnet_tool_ : Tool
    {
        public override bool TryInteract(Vec2 pos) {
            Item item = G.map[pos];
            FactoryLike factory = item as FactoryLike;
            if (factory == null) return false;

            if (factory.working) return false;

            if (!factory.HasLink) {
                AutoConsume(pos); // 先
                if (factory.auto_working) {
                    factory.TryAutoStop();
                }
                else {
                    factory.TryAutoRun();
                }
            }
            else {
                AutoProvide_Undo(pos);
                if (factory.working) {
                    factory.TryAutoStop();
                }
                AutoConsume_Undo(pos);
            }

            return true;
        }
        private void AutoConsume(Vec2 pos) {

        }
        private void AutoConsume_Undo(Vec2 pos) {

        }
        private void AutoProvide(Vec2 pos) {

        }
        private void AutoProvide_Undo(Vec2 pos) {

        }

    }


    [impl(typeof(road_))]
    [cn("道路")] public interface road { }
    public class road_ : Item4x4, ILowOrder, IHasBelt
    {

        public bool HasBelt => true;

        public override bool TryInteract(Vec2 pos) {
            if (G.map[pos] is road_) {
                G.map[pos] = null;
                return true;
            } else {
                G.map[pos] = Of<road>();
                return true;
            }
        }

        public override bool IsSameTypeOn(Vec2 pos) {
            return G.map[pos] is road_;
        }

        public override bool Pickable => false;
        public override bool TryPickOnEnter => false;
        public override bool ShakeOnEnter => false;
    }
}
