
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{
    public class star_map_ : Ground4x4
    {
        private string KeyIfOn(Vec2 pos) { // no POS
            int selection = (int)(G.map.hashcode % 9);
            int random = (int)(G.map.HashcodeOfTile(pos) % 16);

            return StrPool.Concat("star_map", selection * 16 + random);
        }
        public override string Content => null;

        public override bool IsSameTypeOn(Vec2 pos) => true;
        public override string Description => "星空";

        public override string LayerContent => KeyIfOn(_Pos);
        // public override string Content => StrPool.Concat(KeyBase, 4 * 4 - 1);
        // public override Color4 ContentColor => LayerColor;
    }





    public class sea_ : Ground8x6
    {
        public override bool CanEnter => false;

        public override Color4 LayerColor => G.theme.BedrockColor;
        protected override int Height => 4;
        protected override string KeyBase => "bedrock";
        public override string Description => "海洋";
        public override string Content => TileWithBorderString; // StrPool.Concat(KeyBase, MagicNine);
        public override Color4 ContentColor => G.theme.WaterColor;

        public override bool IsSameTypeOn(Vec2 pos) => G.map.terrain.bedrock[pos] == GroundType.sea;

        protected override bool IgnoreTileIndex(int index) => index == MagicNine;
    }

    public class bedrock_ : Ground8x6
    {
        public override Color4 LayerColor => G.theme.BedrockColor;
        protected override int Height => 4;
        protected override string KeyBase => "bedrock";
        public override string Description => "基岩";
        public override string Content => StrPool.Concat(KeyBase, MagicNine);

        public override bool IsSameTypeOn(Vec2 pos) => G.map.terrain.bedrock[pos] == GroundType.sea;

        protected override bool IgnoreTileIndex(int index) => index == TileUtility.Full8x6;
    }


    //public class sea_sand_ : Ground8x6
    //{
    //    public override bool CanEnter => false;

    //    public override Color4 LayerColor => G.theme.SandColor;
    //    protected override int Height => 4;
    //    protected override string KeyBase => "bedrock_sand";
    //    public override string Description => "海洋";
    //    public override string Content => TileWithBorderString; // StrPool.Concat(KeyBase, MagicNine);
    //    public override Color4 ContentColor => G.theme.WaterColor;

    //    public override bool IsSameTypeOn(Vec2 pos) => G.map.terrain.bedrock[pos] == GroundType.sea_sand;

    //    protected override bool IgnoreTileIndex(int index) => index == MagicNine;
    //}

    //public class bedrock_sand_ : Ground8x6
    //{
    //    public override Color4 LayerColor => G.theme.SandColor;
    //    protected override int Height => 4;
    //    protected override string KeyBase => "bedrock_sand";
    //    public override string Description => "沙地";
    //    public override string Content => StrPool.Concat(KeyBase, MagicNine);

    //    public override bool IsSameTypeOn(Vec2 pos) => G.map.terrain.bedrock[pos] == GroundType.sea_sand;

    //    protected override bool IgnoreTileIndex(int index) => index == TileUtility.Full8x6;
    //}



    public class grassland_ : Ground8x6
    {
        public override Color4 LayerColor => G.theme.FloraColor;
        protected override int Height => 4;
        protected override string KeyBase => "grassland";
        public override string Description => "草地";
        public override string Content => StrPool.Concat(KeyBase, MagicThirtyThree);

        public override bool IsSameTypeOn(Vec2 pos) => G.map.terrain.floor[pos] == GroundType.grassland;
    }

    public class hill_sand_ : Ground8x6
    {
        public override bool CanEnter => false;
        public override Color4 LayerColor => G.theme.SandColor;
        protected override int Height => 1;
        protected override string KeyBase => "hill_sand";
        public override string Description => "沙丘";

        public override bool IsSameTypeOn(Vec2 pos) => G.map.terrain.floor[pos] == GroundType.hill_sand;
    }
    public class hill_stone_ : Ground8x6
    {
        public override bool CanEnter => false;
        public override Color4 LayerColor => G.theme.StoneColor;
        protected override int Height => 1;
        protected override string KeyBase => "hill_stone";
        public override string Description => "石山";

        public override bool IsSameTypeOn(Vec2 pos) => G.map.terrain.floor[pos] == GroundType.hill_stone;
    }

    public class hill_clay_ : Ground8x6
    {
        public override bool CanEnter => false;
        public override Color4 LayerColor => G.theme.ClayColor;
        protected override int Height => 1;
        protected override string KeyBase => "hill_clay";
        public override string Description => "土山";

        public override bool IsSameTypeOn(Vec2 pos) => G.map.terrain.floor[pos] == GroundType.hill_clay;
    }

    public class farmland_ : Ground8x6
    {
        public override bool CanEnter => true;
        public override Color4 LayerColor => G.theme.ClayColor;
        protected override int Height => 1;
        protected override string KeyBase => "farmland";
        public override string Description => "农田";

        public override bool IsSameTypeOn(Vec2 pos) => G.map.terrain.decor[pos] == GroundType.farmland;
    }





    public class metal_wall_ : Ground8x6
    {
        public override bool CanEnter => false;
        public override Color4 LayerColor => G.theme.LightMetalColor;
        protected override int Height => 1;
        protected override string KeyBase => "metal_wall";
        public override string Description => "金属墙面";

        public override bool IsSameTypeOn(Vec2 pos) => G.map.terrain.floor[pos] == GroundType.metal_wall;
    }


    public class concrete_wall_ : Ground8x6
    {
        public override bool CanEnter => false;
        public override Color4 LayerColor => G.theme.ConcreteColor;
        protected override int Height => 1;
        protected override string KeyBase => "concrete_wall";
        public override string Description => "水泥墙面";

        public override bool IsSameTypeOn(Vec2 pos) => G.map.terrain.floor[pos] == GroundType.concrete_wall;
    }

    public class brick_wall_ : Ground4x4
    {
        public override bool CanEnter => false;
        public override Color4 LayerColor => G.theme.ClayBakedColor;
        protected override int Height => 1;
        protected override string KeyBase => "brick_wall";
        public override string Description => "土砖墙面";
        public override bool IsSameTypeOn(Vec2 pos) => G.map.terrain.floor[pos] == GroundType.claybaked_brick_wall;
    }

    public class stonebrick_wall_ : Ground4x4
    {
        public override bool CanEnter => false;
        public override Color4 LayerColor => G.theme.StoneColor;
        protected override int Height => 1;
        protected override string KeyBase => "stonebrick_wall";
        public override string Description => "石砖墙面";
        public override bool IsSameTypeOn(Vec2 pos) => G.map.terrain.floor[pos] == GroundType.stone_brick_wall;
    }

    public class plank_wall_ : Ground4x4
    {
        public override bool CanEnter => false;
        public override Color4 LayerColor => G.theme.WoodColor;
        protected override int Height => 1;
        protected override string KeyBase => "plank_wall";
        public override string Description => "木板墙面";
        public override bool IsSameTypeOn(Vec2 pos) => G.map.terrain.floor[pos] == GroundType.wood_brick_wall;
    }



    public class stone_tile_floor : Ground8x6
    {
        public override bool CanEnter => true;
        public override Color4 LayerColor => G.theme.StoneColor;
        protected override int Height => 1;
        protected override string KeyBase => "stone_tile_floor";
        public override string Description => "石砖地面";
        public override bool IsSameTypeOn(Vec2 pos) => G.map.terrain.floor[pos] == GroundType.stone_tile_floor;
    }
    public class wood_tile_floor : Ground8x6
    {
        public override bool CanEnter => true;
        public override Color4 LayerColor => G.theme.WoodColor;
        protected override string KeyBase => "wood_tile_floor";
        public override string Description => "木板地面";
        public override bool IsSameTypeOn(Vec2 pos) => G.map.terrain.floor[pos] == GroundType.wood_tile_floor;
    }

    public class claybaked_tile_floor : Ground8x6
    {
        public override bool CanEnter => true;
        public override Color4 LayerColor => G.theme.ClayBakedColor;
        protected override string KeyBase => "claybaked_tile_floor";
        public override string Description => "土砖地面";
        public override bool IsSameTypeOn(Vec2 pos) => G.map.terrain.floor[pos] == GroundType.claybaked_tile_floor;
    }
}

