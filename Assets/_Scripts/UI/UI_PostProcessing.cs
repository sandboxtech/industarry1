
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace W
{
    public class UI_PostProcessing : MonoBehaviour
    {
        public static UI_PostProcessing I { get; private set; }
        private void Awake() {
            A.Assert(I == null);
            I = this;
        }

        [SerializeField]
        private Volume Volume;
        private VolumeProfile profile;



        private void Start() {
            RecalcPP();
        }


        private Bloom bloom;
        private Tonemapping tonemapping;
        private WhiteBalance whiteBalance;

        private void RecalcPP() {

            profile = Volume.profile;

            if (!profile.TryGet(out bloom)) A.Throw();
            if (!profile.TryGet(out tonemapping)) A.Throw();
            if (!profile.TryGet(out whiteBalance)) A.Throw();

            Settings settings = G.settings;

            Volume.enabled = settings.postprocessing_intensity > C.ClampAlpha;
            if (!Volume.enabled) return;
            Volume.weight = settings.postprocessing_intensity;

            RecalcBloom();
            RecalcTonemapping();
            RecalcWhiteBalance();

            Volume.profile = profile;
        }


        private void RecalcBloom() {

            Settings settings = G.settings;

            bloom.active = settings.bloom_intensity > C.ClampAlpha && settings.bloom_scatter > C.ClampAlpha;

            if (!bloom.active) return;

            bloom.intensity.value = M.Lerp(0.2f, 1, settings.bloom_intensity);
            bloom.scatter.value = M.Lerp(0.2f, 1, settings.bloom_scatter);

            return;
        }

        private void RecalcTonemapping() {
            tonemapping.mode.value = CurrentTonemappingMode.Item2;
        }

        private void RecalcWhiteBalance() {
            whiteBalance.temperature.value = (G.settings.white_balance_temporature - 0.5f) * 2;
        }



        public (string, TonemappingMode) CurrentTonemappingMode {
            get {
                if (G.settings.tone_mapping_mode % 2 == 0) {
                    return ("色彩模式: 鲜艳", TonemappingMode.ACES);
                } else {
                    return ("色彩模式: 自然", TonemappingMode.Neutral);
                }
            }
        }
        public void SwitchTonemappingMode() {
            G.settings.tone_mapping_mode++;
            RecalcPP();
        }


        public float BloomIntensity {
            get => G.settings.bloom_intensity;
            set {
                G.settings.bloom_intensity = value;
                RecalcPP();
            }
        }

        public float BloomScatter {
            get => G.settings.bloom_scatter;
            set {
                G.settings.bloom_scatter = value;
                RecalcPP();
            }
        }

    }
}
