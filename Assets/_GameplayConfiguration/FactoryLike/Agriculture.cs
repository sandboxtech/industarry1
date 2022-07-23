
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{


    [recipe(typeof(berry), 5, false, typeof(copper_coin), 1)] public interface sell_berry { }
    [recipe(typeof(fiber), 1, false, typeof(copper_coin), 1)] public interface sell_fiber { }
    [recipe(typeof(wheat), 1, false, typeof(silver_coin), 1)] public interface sell_wheat { }
    [recipe(typeof(cactus), 1, false, typeof(silver_coin), 1)] public interface sell_cactus { }
    [recipe(typeof(eggplant), 1, false, typeof(silver_coin), 1)] public interface sell_eggplant { }
    [recipe(typeof(potato), 1, false, typeof(silver_coin), 1)] public interface sell_potato { }
    [recipe(typeof(pumpkin), 1, false, typeof(silver_coin), 1)] public interface sell_pumpkin { }
    [recipe(typeof(turnip), 1, false, typeof(silver_coin), 1)] public interface sell_turnip { }



    [recipe(typeof(copper_coin), 1, false, typeof(berry_plant))] public interface buy_berry_plant { }
    [recipe(typeof(copper_coin), 1, false, typeof(fiber_plant))] public interface buy_fiber_plant { }
    [recipe(typeof(silver_coin), 1, false, typeof(wheat_plant))] public interface buy_wheat_plant { }
    [recipe(typeof(silver_coin), 1, false, typeof(cactus_plant))] public interface buy_cactus_plant { }
    [recipe(typeof(silver_coin), 1, false, typeof(eggplant_plant))] public interface buy_eggplant_plant { }
    [recipe(typeof(silver_coin), 1, false, typeof(potato_plant))] public interface buy_potato_plant { }
    [recipe(typeof(silver_coin), 1, false, typeof(pumpkin_plant))] public interface buy_pumpkin_plant { }
    [recipe(typeof(silver_coin), 1, false, typeof(turnip_plant))] public interface buy_turnip_plant { }




    [recipe(typeof(branch), 1, false, typeof(silver_coin), 5)] public interface sell_branch { }
    [recipe(typeof(wood), 1, false, typeof(silver_coin), 5)] public interface sell_wood { }
    [recipe(typeof(leaf), 1, false, typeof(silver_coin), 5)] public interface sell_leaf { }

    [recipe(typeof(silver_coin), 1, false, typeof(tree_branch))] public interface buy_tree_branch { }
    [recipe(typeof(silver_coin), 1, false, typeof(tree_wood))] public interface buy_tree_wood { }
    [recipe(typeof(silver_coin), 1, false, typeof(tree_leaf))] public interface buy_tree_leaf { }



    [recipe(typeof(fiber), 10, false, typeof(hut_fiber))]
    [impl(typeof(hut_fur_))]

    [cn("毛皮棚屋-农业")] public interface hut_fur : building { } // fur, branch => this
    public class hut_fur_ : MarketLike
    {
        public override Vec2 Size => new Vec2(2, 1);
        public override Color4 ContentColor => G.theme.ClayColor;
        // public override string Container => DefaultContainer;

        public override string Highlight => InProgress ? DefaultHighlight : null;
        public override Color4 HighlightColor => G.theme.BulbColor;

        public static int count = 0;
        public override List<Type> Recipes {
            get {
                if (lastMapHashcode != G.map.hashcode) {
                    lastMapHashcode = G.map.hashcode;
                    CalculateRecipes();
                }

                return recipes;
            }
        }
        private static uint lastMapHashcode = 0;
        private void CalculateRecipes() {
            CalcBush();
            CalcTree();
        }
        private void CalcBush() {
            Type bush = MapTerrain.SpecialBushOf(G.map.hashcode);
            Type buyBush = typeof(buy_berry_plant);
            Type sellBush = typeof(sell_berry);

            if (bush == typeof(cactus_plant)) {
                buyBush = typeof(buy_berry_plant);
                sellBush = typeof(sell_berry);
            } else if (bush == typeof(eggplant_plant)) {
                buyBush = typeof(buy_eggplant_plant);
                sellBush = typeof(sell_eggplant);
            } else if (bush == typeof(potato_plant)) {
                buyBush = typeof(buy_potato_plant);
                sellBush = typeof(sell_potato);
            } else if (bush == typeof(pumpkin_plant)) {
                buyBush = typeof(buy_pumpkin_plant);
                sellBush = typeof(sell_pumpkin);
            } else if (bush == typeof(turnip_plant)) {
                buyBush = typeof(buy_turnip_plant);
                sellBush = typeof(sell_turnip);
            } else {
                buyBush = typeof(buy_berry_plant);
                sellBush = typeof(sell_berry);
            }
            recipes[0] = buyBush;
            recipes[1] = sellBush;
        }
        private void CalcTree() {
            Type tree = MapTerrain.SpecialTreeOf(G.map.hashcode);
            Type buyTree;
            Type sellTree;

            if (tree == typeof(tree_leaf)) {
                buyTree = typeof(buy_tree_leaf);
                sellTree = typeof(sell_leaf);

            } else if (tree == typeof(tree_branch)) {
                buyTree = typeof(buy_tree_branch);
                sellTree = typeof(sell_leaf);
            } 
            else if (tree == typeof(tree_wood)) {
                buyTree = typeof(buy_tree_wood);
                sellTree = typeof(sell_wood);
            }
            else {
                buyTree = typeof(buy_tree_branch);
                sellTree = typeof(sell_branch);
            }
            recipes[2] = buyTree;
            recipes[3] = sellTree;
        }

        private static List<Type> recipes = new List<Type>() {
            typeof(sell_berry),
            typeof(sell_berry),
            typeof(sell_berry),
            typeof(sell_berry),

            //typeof(sell_fiber),
            //typeof(sell_wheat),
            //typeof(sell_branch),
            //typeof(sell_wood),

            //typeof(buy_berry_plant),
            //typeof(buy_fiber_plant),
            //typeof(buy_wheat_plant),
            //typeof(buy_tree_branch),
            //typeof(buy_tree_wood),
        };

        protected override long Max => 5;
        protected override long Del => C.SimpleWaitTimespan;
    }




    /// <summary>
    /// ////////////////////////////////////////////////////////////////
    /// </summary>







    public interface plant { }



    public abstract class _PlantLike : FactoryLike
    {
        private int lastID;

        private int Same(int thisID) {
            if (thisID != lastID && !Information.MapEnteringFrame && OnMap) Map.ShakeAt(_Pos); // for the "shake" of ...
            lastID = thisID;
            return thisID;
        }
        protected int Lerp(int width) => Same((int)M.Lerp(0, width - 1, OnMap ? TotalProgress : 1));
        protected string LerpContent(int width) => StrPool.Concat(DefaultContent, Lerp(width));
        protected string LerpContainer(int width) => StrPool.Concat(DefaultContainer, Lerp(width));
        protected string LerpHighlight(int width) => StrPool.Concat(DefaultHighlight, Lerp(width));

        public override void OnCreate() {
            base.OnCreate();
            A.Assert(Recipes == null); // 暂时都是单配方植物
        }
        protected override bool PlantLike => true;


        protected override bool TryRunCondition => OnMap && G.terrain.floor[_Pos] == GroundType.grassland; // Quantity == 1 && 
        protected override void BeforeRemoveFromMap() {
            if (TryStop()) {
                RemoveEffect();
            }
        }
        protected virtual void RemoveEffect() {

        }
    }

    public abstract class _BushLike : _PlantLike, IScythable
    {

        public virtual long EnergyConsumption => 1;

        public override bool CanEnterPart(Vec2 pos) => true;
        public override void OnEnter(Vec2 pos) {
            long e = EnergyConsumption;
            if (G.player.attr.CanConsumeEnergy(e) && TryCollect_IfNoInputsRequired()) {
                G.player.attr.TryConsumeEnergy(e);
            } else {
                ShakeSelf();
            }
        }

        public virtual bool Scythable => G.player.hand.HasSpace();
        public void Scythe(float timeScale) {
            TryStop();
            G.player.hand.TryAbsorbOrAdd(this);
        }

        protected override void RemoveEffect() {
            Particle.CreateDust(_Pos, G.theme.FloraColor);
            Audio.I.PlayRandomSoundClip(Audio.I.Grass);
        }

        // 兼容锤子
        public override bool Destructable => Scythable;
        public override void Destruct() {
            Scythe(1);
        }
    }

    public abstract class _TreeLike : _PlantLike, IChoppable
    {
        public override bool CanEnterPart(Vec2 pos) => false;

        protected override bool DoTryCollect => IdleData.Maxed;

        public virtual bool Choppable => G.player.hand.HasSpace();
        public void Chop(float timeScale) {
            TryStop();
            G.player.hand.TryAbsorbOrAdd(this);
        }
        protected override void RemoveEffect() {
            Particle.CreateDust(_Pos, G.theme.WoodColor);
            Audio.I.PlayRandomSoundClip(Audio.I.Wood);
        }

        // 兼容锤子
        public override bool Destructable => Choppable;
        public override void Destruct() {
            Chop(1);
        }
    }



    /// <summary>
    /// //////////////////////////////
    /// </summary>



    [impl(typeof(berry_plant_))]
    [cn("浆果丛")] public interface berry_plant : plant { }
    public class berry_plant_ : _BushLike
    {
        public override Type Recipe => typeof(berry);

        private const int length = 4;
        public override string Content => LerpContent(length);
        public override Color4 ContentColor => G.theme.FloraColor;
        public override string Highlight => LerpHighlight(length);
        public override Color4 HighlightColor => G.theme.FruitColor;


        public override long EnergyConsumption => 0;
        protected override long Max => length - 1;
        protected override long Del => C.Minute / Max;
    }


    [impl(typeof(fiber_plant_))]
    [cn("苇草植株")] public interface fiber_plant : plant { }
    public class fiber_plant_ : _BushLike
    {
        public override Type Recipe => typeof(fiber);

        public override string Content => LerpContent(4);
        public override Color4 ContentColor => G.theme.FloraColor;

        protected override long Max => 1;
        protected override long Del => C.Minute / Max;
    }


    [impl(typeof(wheat_plant_))]
    [cn("小麦植株")] public interface wheat_plant : plant { }
    public class wheat_plant_ : _BushLike
    {
        public override Type Recipe => typeof(wheat);

        public override string Content => LerpContent(4);

        protected override long Max => 1;
        protected override long Del => C.Hour / Max;
    }

    [impl(typeof(cactus_plant_))]
    [cn("仙人掌植株")] public interface cactus_plant : plant { }
    public class cactus_plant_ : _BushLike
    {
        public override Type Recipe => typeof(cactus);

        public override string Content => LerpContent(4);
        public override Color4 ContentColor => G.theme.FloraColor;

        protected override long Max => 1;
        protected override long Del => C.Hour / Max;
    }

    [impl(typeof(eggplant_plant_))]
    [cn("茄子植株")] public interface eggplant_plant : plant { }
    public class eggplant_plant_ : _BushLike
    {
        public override Type Recipe => typeof(eggplant);

        public override string Content => LerpContent(4);
        public override Color4 ContentColor => G.theme.FloraColor;
        public override string Container => LerpContainer(4);

        protected override long Max => 1;
        protected override long Del => C.Hour / Max;
    }

    [impl(typeof(potato_plant_))]
    [cn("土豆植株")] public interface potato_plant : plant { }
    public class potato_plant_ : _BushLike
    {
        public override Type Recipe => typeof(potato);

        public override string Content => LerpContent(4);
        public override Color4 ContentColor => G.theme.FloraColor;
        public override string Container => LerpContainer(4);

        protected override long Max => 1;
        protected override long Del => C.Hour / Max;
    }

    [impl(typeof(pumpkin_plant_))]
    [cn("南瓜植株")] public interface pumpkin_plant : plant { }
    public class pumpkin_plant_ : _BushLike
    {
        public override Type Recipe => typeof(pumpkin);

        public override string Content => LerpContent(4);
        public override Color4 ContentColor => G.theme.FloraColor;
        public override string Container => LerpContainer(4);

        protected override long Max => 1;
        protected override long Del => C.Hour / Max;
    }

    [impl(typeof(turnip_plant_))]
    [cn("白萝卜植株")] public interface turnip_plant : plant { }
    public class turnip_plant_ : _BushLike
    {
        public override Type Recipe => typeof(turnip);

        public override string Content => LerpContent(4);
        public override Color4 ContentColor => G.theme.FloraColor;
        public override string Container => LerpContainer(4);

        protected override long Max => 1;
        protected override long Del => C.Hour / Max;
    }







    [impl(typeof(tree_branch_))]
    [cn("枝树")] public interface tree_branch : plant { }
    public class tree_branch_ : _TreeLike
    {
        public override Type Recipe => typeof(branch);

        public override bool Choppable => true;

        public override string Content => LerpContent(4);
        public override Color4 ContentColor => G.theme.WoodColor;
        public override string Container => LerpContainer(4);
        public override Color4 ContainerColor => G.theme.FloraColor;


        protected override long Max => 1;
        protected override long Del => C.Hour / 2 / Max;
    }


    [impl(typeof(tree_wood_))]
    [cn("木树")] public interface tree_wood : plant { }
    public class tree_wood_ : _TreeLike
    {
        public override Type Recipe => typeof(wood);

        public override bool Choppable => true;

        private const int length = 4;
        public override string Content => LerpContent(length);
        public override Color4 ContentColor => G.theme.WoodColor;
        public override string Container => LerpContainer(length);
        public override Color4 ContainerColor => G.theme.FloraColor;


        protected override long Max => 1;
        protected override long Del => C.Hour / Max;
    }

    [impl(typeof(tree_leaf_))]
    [cn("叶树")] public interface tree_leaf : plant { }
    public class tree_leaf_ : _TreeLike
    {
        public override Type Recipe => typeof(leaf);

        public override bool Choppable => true;

        private const int length = 4;
        public override string Content => LerpContent(length);
        public override Color4 ContentColor => G.theme.WoodColor;
        public override string Container => LerpContainer(length);
        public override Color4 ContainerColor => G.theme.FloraColor;

        protected override long Max => length - 1;
        protected override long Del => C.Hour / Max;
    }


}
