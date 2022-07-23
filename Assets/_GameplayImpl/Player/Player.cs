
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{

    public interface __Player__
    {
        void UpdateState();
        void OnEnterMap(Map map);
        void OnExitMap(Map map);
        void OnTapMap(Vec2 pos);
        void OnTapLeft();
        void OnTapRight();

        void OnTapHand(int i);
    }

    public interface IEdible
    {
        bool TryEat(long q);
    }


    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class Player : ICreatable, __Player__
    {

        [JsonProperty] private int hand_index { get; set; } // 选择物品下标
        public int HandIndex { get => hand_index; set => hand_index = value % C.HandLength; }
        [JsonProperty] public ItemList hand { get; private set; } // 物品栏
        [JsonIgnore] public Item HandItem { get => hand[hand_index]; set => hand[hand_index] = value; } // 选择物品


        [JsonProperty] public Vec2 position { get; set; }
        [JsonProperty] public Vec2 direction { get; set; }
        [JsonIgnore] public Vec2 front => position + direction;
        [JsonIgnore] public Vec2 back => position - direction;
        [JsonIgnore] public Vec2 left => position + DirectionNumberToDirection(DirectionNumber(direction) + 1); // +1 为逆时针旋转
        [JsonIgnore] public Vec2 right => position + DirectionNumberToDirection(DirectionNumber(direction) - 1); // -1为顺时针旋转

        [JsonProperty] public PlayerState state { get; private set; }
        [JsonProperty] public PlayerAttr attr { get; private set; }



        public void OnCreate() {
            state = Creator.__CreateData<PlayerState>();
            attr = Creator.__CreateData<PlayerAttr>();
            // direction and position position set by map
            hand = ItemList.Create(C.HandLength);
        }

        public static int DirectionNumber(Vec2 direction) {
            // 0 1 2 3 代表 右上左下
            int r = 0;
            if (direction == Vec2.right) {
                r = 0;
            } else if (direction == Vec2.up) {
                r = 1;
            } else if (direction == Vec2.left) {
                r = 2;
            } else if (direction == Vec2.down) {
                r = 3;
            }
            return r;
        }
        public static Vec2 DirectionNumberToDirection(int i) {
            int ii = i % 4;
            if (ii < 0) ii += 4;
            // 0 1 2 3 代表 右上左下
            switch (ii) {
                case 0:
                    return Vec2.right;
                case 1:
                    return Vec2.up;
                case 2:
                    return Vec2.left;
                case 3:
                    return Vec2.down;
                default:
                    throw new Exception();
            }
        }






        #region update

        /// <summary>
        /// 尝试走路
        /// </summary>
        void __Player__.UpdateState() {


            Vec2 movement = UI_Joystick.I.Movement;
            bool movement_zero = movement == Vec2.zero;


            // 吃东西状态
            if (state.Type == PlayerStateType.eating) {
                if (!movement_zero || state.eating_count <= 0) {
                    state.Type = PlayerStateType.idle;
                } else if (HandItem is IEdible edible) {
                    if (state.Finished) {
                        state.eating_count--;
                        state.TryEat();
                        if (edible.TryEat(1)) {

                        } else {
                            // 停止吃
                            state.eating_count = 0;
                        }
                    }
                } else {
                    state.Type = PlayerStateType.idle;
                }
            }

            // 尝试开始走路
            if (state.Type == PlayerStateType.idle) {
                state.Finish(); // 立刻完成
                if (!movement_zero) {
                    TryTakeAStep(movement);
                }
            }
            // 完成走路
            else if (state.Type == PlayerStateType.walking && state.Finished) {
                // 停止走路 / 继续走路
                if (movement_zero || !TryTakeAStep(movement)) { // 走路state不能中断，因为动画
                    state.Type = PlayerStateType.idle;
                }
            } else if (state.Type == PlayerStateType.interacting) {
                // 交互完成
                if (state.Finished) {
                    onComplete?.Invoke(); // complete not nullable
                    onComplete = null;
                    onCancel = null;
                    state.Type = PlayerStateType.idle;

                    G.achievement.interaction_completion_count_in_life++;
                }
                // 交互被另一个方向的行动取消
                else if (!movement_zero && ((onComplete == null) || movement != interactingDirection
                    && G.settings.interaction_cancalable)) { // 没回调纯动画，或者设置可以取消，则可以取消动作
                    onCancel?.Invoke();
                    onComplete = null;
                    onCancel = null;
                    state.Type = PlayerStateType.idle;
                }
            }

            if (state.Type == PlayerStateType.idle) {
                // 原地转向
                Vec2 facing = UI_Joystick.I.Facing;
                if (facing != Vec2.zero) {
                    direction = facing;
                } else if (!movement_zero) {
                    direction = movement;
                }
            }
        }
        /// <returns> state changed or continues </returns>
        private bool TryTakeAStep(Vec2 movement) {
            direction = movement;
            Vec2 target = position + direction;
            // 不能进地图边缘
            if (!G.map.IsInside(target)) {
                return false;
            }

            Item item = G.map[target];
            Vec2 interactionTarget = item == null ? target : item._Pos;
            bool hasInteractionTarget = interactionTarget == target;

            // 使用手里工具
            Item hand = HandItem;
            if (hand != null) {
                HandItem.TryMove(target); // state may change here
            } else {
                // 空手默认工具。暂无实现
                TryEnter(target);
            }
            // 如果有空，尝试进入前方。被动行为
            if (state.Finished) item?.TryEnter(target);


            if (!state.Finished) { // state changed
                // TryEnter行为阻止了行走
                EndTryTakeStep(target, hasInteractionTarget, interactionTarget);
                return true; // occupied by step operation
            }

            if (!G.map.CanEnter(target)) {
                // 前方物品阻止了行走
                EndTryTakeStep(target, hasInteractionTarget, interactionTarget);
                return false;
            }

            // 成功行走
            position = target;
            state.TakeAStep();
            // 使用工具
            HandItem?.OnMove(position);
            G.map[position]?.OnEnter(position);

            OnEnterEffect();

            EndTryTakeStep(target, hasInteractionTarget, interactionTarget);
            return true;
        }
        // 无论是否成功行走，需要更新一下走过的位置的物品
        private void EndTryTakeStep(Vec2 target, bool hasInteractionTarget, Vec2 interactionTarget) {
            Map.ReRender9At(target);
            if (hasInteractionTarget) Map.ReRender9At(interactionTarget);
        }


        /// <summary>
        /// 玩家进入格子
        /// </summary>
        /// <param name="pos"></param>
        private void OnEnterEffect() {

            Particle.ClearDestruction(); // 如果有cross动画，停止
            Particle.CreateFootStepCloud(position - direction);

            // 发出脚步声音
            if (state.step_count_since_walk_start % 2 == 1) {
                G.map.PlayFootstepSoundOn(position);
            }
        }
        private void TryEnter(Vec2 pos) {
            // ??
        }

        /// <summary>
        /// 撤销交互方向条件
        /// </summary>
        private Vec2 interactingDirection;
        public void TryInteract(float timeScale = 1f, Action onComplete = null, Action onCancel = null) => TryInteract(onComplete, onCancel, timeScale);
        public void TryInteract(Action onComplete, Action onCancel = null, float timeScale = 1f) {
            // if (Subtype != Type.idle) return;
            interactingDirection = direction;
            state.TryIteract(timeScale);
            this.onComplete = onComplete;
            this.onCancel = onCancel;
        }
        private Action onComplete;
        private Action onCancel;

        #endregion







        void __Player__.OnEnterMap(Map map) {
        }
        void __Player__.OnExitMap(Map map) {
        }

        void __Player__.OnTapMap(Vec2 pos) {
            if (!G.map.terrain.NoPlayerAvatar && pos == position) {
                OnTapSelf();
            } else {
                OnTapMap(pos);
                //if ((pos - G.player.position).sqrMagnitude <= C.PlayerReach * C.PlayerReach) {
                //    OnTapMap(pos);
                //} else {
                //    UI_Notice.I.Send("太远了", null, C.Second, C.Second / 2);
                //}
            }
        }

        private void OnTapSelf() {
            Audio.I.PlayerClick();
            // G.player.hand.TryAbsorbOrAdd(Item.Of<thought_initial>());
            // book_initial_.UseThis();
            PlayerMenu.Page();
        }

        private void OnTapMap(Vec2 pos) {
            Item hand = G.player.HandItem;
            Item ground = G.map[pos];

            if (hand != null && hand.TryInteract(pos)) {
                // interacted with hand
            }
            else if (ground == null) {
                if (hand != null && hand._DroppableAt(pos)) {
                    // 能丢弃

                    if (G.terrain.NoPlayerAvatar) {
                        // 太空暂时不能放下物品
                    }
                    else {
                        if (G.settings.drop_one_or_all && hand.Quantity > 1) {
                            // 放下一个
                            DropItem(pos, hand.Split(1));
                            ReRenderAndBounceHand();
                        } else {
                            // 放下所有
                            DropItem(pos, hand);
                        }
                        if (hand != null) Map.ShakeAt(pos);
                    }
                }
                else {
                    // 展示信息
                    // G.map.NoticeGrounds(pos);
                    // UI_Notice.I.Send(pos.ToString());
                }
            } else {
                Item h = G.player.HandItem;

                Map.ShakeAt(ground._Pos);

                if (ground.CanTap) {
                    // 能交互
                    ground.OnTap();

                } else if (h != null && h.TryAbsorb(ground)) {
                    // 能合并
                    ReRenderAndBounceHand();
                } else if (ground.Pickable) {
                    // 能拾取
                    if (hand == null) {
                        G.selected = ground;
                        PickItem(pos, ground);
                    } else if (G.player.hand.TryAbsorbOrAdd(ground)) {
                        PickItem(pos, ground);
                    }
                }
            }
        }

        private void DropItem(Vec2 pos, Item hand) {
            G.map[pos] = hand;
            UI_Notice.I.Send($"<color=#cccccc99>放下</color> {hand.Description}", hand);
            if (G.player.state.Type == PlayerStateType.idle) TryInteract(); // anim
            Audio.I.PlayerPick();
            // Map.ShakeAt(pos); shake 在 onTapMap里
        }
        private void PickItem(Vec2 pos, Item ground) {
            UI_Notice.I.Send($"<color=#cccccc99>拾取</color> {ground.Description}", ground);
            if (G.player.state.Type == PlayerStateType.idle) TryInteract(); // anim
            Audio.I.PlayerPick();

            // hand.Bounce 在 Hand: Items.RenderItem里
        }

        public void ReRenderAndBounceHand() {
            UI_Hand.I.RenderItemAt(HandIndex);
        }

        /// <summary>
        /// 按下坐标按钮，则向前一步
        /// </summary>
        void __Player__.OnTapLeft() {
            if (state.Type == PlayerStateType.walking) return;
            TryTakeAStep(direction);
        }
        /// <summary>
        /// 按下右边按钮，则使用物品
        /// </summary>
        void __Player__.OnTapRight() {
            HandItem?.OnUse();
        }

        /// <summary>
        /// 按下工具栏，切换物品，或者使用物品
        /// </summary>
        void __Player__.OnTapHand(int i) {
            if (i != hand_index) {
                hand_index = i;

                Item handItem = HandItem;
                if (handItem != null) {
                    UI_Notice.I.Send($"<color=#cccccc99>选择</color> {handItem.Description}", handItem);
                }
            } else {
                HandItem?.OnUse();
            }
        }
    }
}
