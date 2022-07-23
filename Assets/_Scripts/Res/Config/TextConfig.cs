
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{
    public class TextConfig : Config
    {
        [SerializeField]
        private TextAsset[] Texts;
        public override void Report() {
            for (int i = 0; i < Texts.Length; i++) {
                A.Assert(Texts[i] != null, () => $"text missing at {gameObject.name}");
            }
            if (Texts.Length == 1) {
                A.Assert(Texts[0].name == gameObject.name, () => $"text config not match. sprite:{Texts[0].name}, obj:{gameObject.name}");
            }
            Res.I.ReportTextAssets(Texts);
        }
    }
}
