
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{
    public class AnimatedTileConfig : Config
    {
        [SerializeField]
        private float Speed = 10;
        [SerializeField]
        private float SpeedMax = 0;
        [SerializeField]
        private Sprite Sprite; // in inventory
        [SerializeField]
        private Sprite[] Sprites;

        public override void Report() {
            A.Assert(Sprites != null && Sprites.Length > 0);
            A.Assert(Sprites[0].name.StartsWith(gameObject.name), () => $"不匹配 gameObject: {gameObject.name} sprite: {Sprites[0].name}");
            A.Assert(Sprites[0].name.Length == gameObject.name.Length + 2, () => $"不匹配 gameObject: {gameObject.name} sprite: {Sprites[0].name}");

            Res.I.ReportAnimatedTile(gameObject.name, Sprite, Sprites, Speed, SpeedMax, null);
        }
    }
}
