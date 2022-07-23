
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{
    public class AudioClipConfig : Config
    {
        [SerializeField]
        private AudioClip[] clips;
        public override void Report() {
            Res.I.ReportAudioClips(clips);
        }
    }
}
