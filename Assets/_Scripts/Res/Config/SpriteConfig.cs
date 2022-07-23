
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{
    public class SpriteConfig : Config
    {
        [SerializeField]
        private Sprite[] Sprites;
        public override void Report() {
            for (int i = 0; i < Sprites.Length; i++) {
                A.Assert(Sprites[i] != null, () => $"sprite missing at {gameObject.name}");
            }
            if (Sprites.Length == 1) {
                A.Assert(Sprites[0].name == gameObject.name, () => $"sprite config not match. sprite:{Sprites[0].name}, obj:{gameObject.name}");
            }
            Res.I.ReportSprites(Sprites);
        }
    }
}
