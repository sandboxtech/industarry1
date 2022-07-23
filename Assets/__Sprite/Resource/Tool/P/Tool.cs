
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{
    [recipe_half(typeof(stick), 1, typeof(stone), 1)] [cn("工具")] public interface tool { } // 定义了独特的行为。和craft的区别：没有界面，自动使用。如1代锤子磁铁




    [recipe(0, typeof(tool), typeof(axe))]
    [impl(typeof(axe_))]
    [cn("斧")] public interface axe : tool { }


    [recipe(0, typeof(tool), typeof(scythe))]
    [impl(typeof(scythe_))]
    [cn("镰")] public interface scythe : tool { }


    [recipe(0, typeof(tool), typeof(pickaxe))]
    [impl(typeof(pickaxe_))]
    [cn("镐")] public interface pickaxe : tool { }

    [recipe(0, typeof(tool), typeof(spade))]
    [impl(typeof(spade_))]
    [cn("铲")] public interface spade : tool { }


    [recipe(0, typeof(tool), typeof(hoe))]
    [impl(typeof(hoe_))]
    [cn("锄")] public interface hoe : tool { }






    [recipe(0, typeof(tool), typeof(hammer))]
    [impl(typeof(hammer_))]
    [cn("锤")] public interface hammer : tool { }



    [recipe(typeof(branch), 1, false, typeof(torch))]
    [impl(typeof(torch_))]
    [cn("火把")] public interface torch : tool { }




    [impl(typeof(fishingrod_))]
    [cn("鱼竿")] public interface fishingrod : tool { }

    [impl(typeof(chainsaw_))]
    [cn("电锯")] public interface chainsaw : tool { }

    [impl(typeof(driller_))]
    [cn("电钻")] public interface driller : tool { }

    [impl(typeof(wrench_))]
    [cn("扳手")] public interface wrench : tool { }


    [impl(typeof(wand_))]
    [cn("法杖")] public interface wand : tool { }






    public abstract class Tool : Item
    {
        public static float ToolInteractionTime;
        protected float ToolTime => 3 / (3 + Log10Quantity);
        // protected override string ExtraQuantityDescription => $"加成 {M.ToPercent(1 / ToolTime),4}%";

        public static bool TryDestruct(Vec2 pos, GroundType type, Type itemType, Color4 color, AudioClip[] clips) {
            if (G.terrain.floor[pos] == type) {
                G.player.TryInteract(ToolInteractionTime, () => {
                    if (G.player.hand.NoSpace() && !G.player.hand.CanAbsorb(itemType)) return;

                    G.player.hand.TryAbsorbOrAdd(Of(itemType));
                    G.terrain.floor[pos] = GroundType.none;

                    Map.ReRender9At(pos);
                    Particle.CreateDust(pos, color);
                    Audio.I.PlayRandomSoundClip(clips);
                });
                ShakeHand();
                Particle.CreateDestruction(pos);
                Audio.I.PlayRandomSoundClip(clips);
                return true;
            }
            return false;
        }
    }



    public abstract class BuildableItem : Item
    {
        protected abstract GroundType BuildableType { get; }
        protected virtual AudioClip[] BuildableSound => null;
        public override void OnMove(Vec2 pos) {
            if (!G.settings.buildable_active) return;
            if (G.terrain.floor[pos] == GroundType.none && G.map[pos] == null) {
                G.terrain.floor[pos] = BuildableType;
                if (Quantity > 1) ShakeHand();
                Quantity--; // 自减

                var sound = BuildableSound;
                if (sound != null) {
                    Audio.I.PlayRandomSoundClip(sound);
                }
            }
        }
    }



    public interface IChoppable
    {
        bool Choppable { get; }
        void Chop(float timeScale);
    }
    public class axe_ : Tool
    {
        public override string Content => type.Name;
        public override string Container => DefaultContainer;
        public override Color4 ContentColor => G.theme.WoodColor;
        public override Color4 ContainerColor => G.theme.MetalColor;

        public override void TryMove(Vec2 pos) {
            // 砍前方
            TryChop(pos);
        }
        private bool TryChop(Vec2 pos) {
            if (G.map[pos] is IChoppable choppable && choppable.Choppable) {
                if (!G.player.attr.TryConsumeEnergy(1)) return false;

                choppable.Chop(ToolTime);
                ShakeHand();
                return true;
            }
            return false;
        }
    }

    public interface IMinable
    {
        bool Minable { get; }
        void Mine(float timeScale);
    }

    public class pickaxe_ : Tool
    {
        public override string Container => DefaultContainer;
        public override Color4 ContentColor => G.theme.WoodColor;
        public override Color4 ContainerColor => G.theme.MetalColor;

        public override void TryMove(Vec2 pos) {
            ToolInteractionTime = ToolTime;
            if (TryMine(pos)) {
                // 挖前方
            } else if (G.map[pos] != null) {

            } else if (TryDestruct(pos, GroundType.hill_stone, typeof(stone), G.theme.StoneColor, Audio.I.Stone)) {

            } else if (TryDestruct(pos, GroundType.hill_clay, typeof(clay), G.theme.ClayColor, Audio.I.Gravel)) {

            }
        }
        private bool TryMine(Vec2 pos) {
            if (G.map[pos] is IMinable minableFront && minableFront.Minable) {
                if (!G.player.attr.TryConsumeEnergy(1)) return false;

                minableFront.Mine(ToolTime);
                ShakeHand();
                return true;
            }
            return false;
        }
    }
    public interface IDiggable
    {
        bool Diggable { get; }
        void Dig(float timeScale);
    }

    public class spade_ : Tool
    {
        public override string Container => DefaultContainer;
        public override Color4 ContentColor => G.theme.WoodColor;
        public override Color4 ContainerColor => G.theme.MetalColor;


        public override void TryMove(Vec2 pos) {
            ToolInteractionTime = ToolTime;
            if (TryDig(pos)) {
                // 铲前面
            } else if (G.map[pos] != null) {

            } else if (TryDestruct(pos, GroundType.hill_clay, typeof(clay), G.theme.ClayColor, Audio.I.Gravel)) {

            } else if (TryDestruct(pos, GroundType.grassland, typeof(fiber), G.theme.FloraColor, Audio.I.Grass)) {

            } else if (TryDestruct(pos, GroundType.claybaked_tile_floor, typeof(claybaked_tile), G.theme.ClayBakedColor, Audio.I.Stone)) {

            } else if (TryDestruct(pos, GroundType.stone_tile_floor, typeof(stone_brick), G.theme.StoneColor, Audio.I.Stone)) {

            } else if (TryDestruct(pos, GroundType.wood_tile_floor, typeof(wood_tile), G.theme.WoodColor, Audio.I.Wood)) {

            }
        }
        private bool TryDig(Vec2 pos) {
            if (G.map[pos] is IDiggable diggableFront && diggableFront.Diggable) {
                if (!G.player.attr.TryConsumeEnergy(1)) return false;

                diggableFront.Dig(ToolTime);
                ShakeHand();
                return true;
            }
            return false;
        }
    }



    /// <summary>
    /// 对 decor.None => decor.Farmland
    /// </summary>
    public class hoe_ : Tool
    {
        public override string Container => DefaultContainer;
        public override Color4 ContentColor => G.theme.WoodColor;
        public override Color4 ContainerColor => G.theme.MetalColor;

        public override void TryMove(Vec2 pos) {

            if (G.map[pos] != null) return;

            ToolInteractionTime = ToolTime;

            // 对于全草地，挖掘
            if (G.terrain.decor[pos] == GroundType.none && TileUtility.All9(pos, (Vec2 p) => G.terrain.floor[p] == GroundType.grassland)) {

                G.player.TryInteract(ToolTime, () => {

                    if (G.player.hand.NoSpace()) return;


                    G.player.hand.TryAbsorbOrAdd(Of<fiber>());

                    G.terrain.decor[pos] = GroundType.farmland;

                    Map.ReRender9At(pos);
                    Particle.CreateDust(pos, G.theme.ClayColor);
                    Audio.I.PlayRandomSoundClip(Audio.I.Gravel);
                });

                ShakeHand();
                Particle.CreateDestruction(pos);
                Audio.I.PlayRandomSoundClip(Audio.I.Gravel);
            }
        }
    }

    public interface IScythable
    {
        bool Scythable { get; }
        void Scythe(float timeScale);
    }

    public class scythe_ : Tool
    {
        public override string Container => DefaultContainer;
        public override Color4 ContentColor => G.theme.WoodColor;
        public override Color4 ContainerColor => G.theme.MetalColor;

        public override void TryMove(Vec2 pos) {
            TryScythe(pos);
        }
        private bool TryScythe(Vec2 pos) {

            ToolInteractionTime = ToolTime;

            // 收割的是脚下，不是面前。为了更好的动画
            if (G.map[G.player.position] is IScythable scythable && scythable.Scythable) {
                if (!G.player.attr.TryConsumeEnergy(1)) return false;
                scythable.Scythe(ToolTime);
                return true;
            }
            return false;
        }
    }


    public class hammer_ : Tool
    {
        public override string Container => DefaultContainer;
        public override Color4 ContentColor => G.theme.WoodColor;
        public override Color4 ContainerColor => G.theme.MetalColor;

        public override void TryMove(Vec2 pos) {

            // 铲脚下
            if (TryDestruct(G.player.position)) {
                return;
            }

            ToolInteractionTime = ToolTime;

            Item frontItem = G.map[pos];
            // 铲前面
            if (frontItem != null) {
                if (frontItem.Destructable) {
                    Map.ShakeAt(pos);
                    Particle.CreateDestruction(pos);
                    Audio.I.PlayRandomSoundClip(Audio.I.Stone);
                    G.player.TryInteract(ToolInteractionTime, () => {
                        frontItem.Destruct();
                        ShakeHand();
                    });
                    return;
                }
                return;
            } else if (TryDestruct(pos, GroundType.hill_stone, typeof(stone), G.theme.StoneColor, Audio.I.Stone)) {

            } else if (TryDestruct(pos, GroundType.hill_clay, typeof(clay), G.theme.ClayColor, Audio.I.Gravel)) {

            } else if (TryDestruct(pos, GroundType.claybaked_brick_wall, typeof(claybaked_brick), G.theme.ClayBakedColor, Audio.I.Stone)) {

            } else if (TryDestruct(pos, GroundType.stone_brick_wall, typeof(stone_brick), G.theme.StoneColor, Audio.I.Stone)) {

            } else if (TryDestruct(pos, GroundType.wood_brick_wall, typeof(wood_brick), G.theme.WoodColor, Audio.I.Wood)) {

            }
        }
        private bool TryDestruct(Vec2 pos) {
            Item item = G.map[pos];
            if (item != null && item.Destructable) {
                item.Destruct();
                ShakeHand();
                return true;
            }
            return false;
        }

        //public override Scroll ExtraUseScroll => Scroll.Button("制造建筑", _WorkPage);
        //public override void _WorkPage() {
        //    Scroll.Show(
        //        Scroll.Slot(this, SlotBackgroundType.Transparent),

        //        CraftPage.Slot<hut_fiber>(this),
        //        CraftPage.Slot<hut_stick>(this),

        //        Scroll.Empty
        //    );
        //}
    }


    [impl(typeof(wildfire_))]
    [cn("野火")]
    public interface wildfire { }

    public class wildfire_ : Item
    {
        public override void OnCreate() {
            base.OnCreate();
            IdleData = Idle.Create(0, 1, C.Minute);
        }

        public override string Content {
            get {
                if (IdleData.Maxed) Detach();
                return null;
            }
        }
        public override string Highlight => IdleData.Maxed ? null : DefaultHighlight;
        public override bool CanEnter => true;
        public override bool Absorbable => false;
        public override bool TryPickOnEnter => false;

        public override bool CanTap => true; // cannot pick
        public override void OnTap() {
            Scroll.Show(
                Scroll.Slot(this),
                Scroll.Button("扑灭", () => { Detach(); UI_Scroll.I.Active = false; }),
                Scroll.Progress(() => $"燃烧中{M.ToPercent(IdleData.Progress),3}%", () => IdleData.OneSubProgress)
            );
        }
    }


    public class torch_ : Tool
    {
        public override Color4 ContentColor => G.theme.WoodColor;
        public override string Highlight => DefaultHighlight;

        public override void TryMove(Vec2 pos) {
            if (PlayerMagic.CanLightOnFire(pos)) {
                G.player.TryInteract(ToolTime, () => {
                    PlayerMagic.TryLightOnFire(pos);
                });
            }
        }
    }

    public class fishingrod_ : Tool
    {
    }


    public class chainsaw_ : Tool
    {
    }

    public class driller_ : Tool
    {
    }


    public class wrench_ : Tool
    {
    }





    public class wand_ : Tool
    {
    }


}
