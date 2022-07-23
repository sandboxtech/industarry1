
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace W
{
    public class Light : MonoBehaviour
    {
        public static Light I { get; private set; }

        private void Awake() {
            A.Assert(I == null);
            I = this;

            PlayerLightTransform = PlayerLight.transform;
            UILightTransform = UILight.transform;
        }


        [SerializeField]
        private Light2D DayLight;
        [SerializeField]
        private Gradient DayLightGradient;

        [SerializeField]
        private Light2D PlayerLight;
        [SerializeField]
        private Light2D UILight;


        private Transform PlayerLightTransform { get; set; }
        private Transform UILightTransform { get; set; }


        private Vector3 velocity;
        public Vector3 PlayerLightSmoothPosition {
            set => PlayerLightTransform.position = Vector3.SmoothDamp(
                PlayerLightTransform.position, value, ref velocity, 0.2f);
        }


        private void Start() {
            NightLightness = NightLightness;
        }

        public float NightLightness {
            get => G.settings.night_ambient_lightness;
            set {
                G.settings.night_ambient_lightness = value;

                bool able = value < 1 - C.ClampAlpha;
                if (!able) {
                    DayLight.intensity = 1;
                }

                enabled = able; // cancel late update
                PlayerLight.enabled = able;
            }
        }


        private void LateUpdate() {


            int day_duration = 60 * 10; // G.map.Terrain.DayLength;
            DayProgress = G.ProgressOf(G.now, day_duration * C.Second); // (G.now / C.Second % day_duration + G.secondT) / day_duration;


            // PlayerLight.enabled = !UI_Scroll.I.Active;
            //if (!UI_Scroll.I.Active) {
            //    PlayerLightTransform.position = PlayerView.I.PlayerFrontOf(0.25f);
            //    PlayerLightTransform.rotation = PlayerView.I.PlayerRotation;
            //}

            UILightTransform.position = PlayerView.I.PlayerBottomLeft + new Vector3(0.5f, -UI.I.CameraOrthographicSize, 0);
        }

        public float DayLightnessCosine { get; private set; }

        private float dayProgress;
        public float DayLightness { get; private set; }
        /// <summary>
        /// 0为凌晨0点，0.25为早上6点
        /// </summary>
        public float DayProgress {
            get => dayProgress;
            set {
                dayProgress = value;

                switch (G.map.type) {
                    case MapType.planet:
                        float t = -value * M.PI2;
                        float cos = M.Cos(t);
                        DayLightnessCosine = cos;

                        DayLightness = M.Max(M.Clamp(0, 1, -cos + 0.625f), NightLightness);
                        DayLightness = M.Clamp01(DayLightness);

                        DayLight.intensity = DayLightness;

                        float intensity = M.Max(0, (1 - DayLightness));
                        // PlayerLight.intensity = M.Clamp01(1.5f * intensity) * 2f;
                        UILight.intensity = M.Clamp01(1.125f * intensity) * 1.25f;

                        DayLight.color = DayLightGradient.Evaluate(value);

                        break;

                    default:
                        DayLightness = 1;
                        DayLight.intensity = DayLightness;
                        // PlayerLight.intensity = 0;
                        UILight.intensity = 0;

                        DayLight.color = Color4.white;

                        break;
                }
            }
        }
    }
}
