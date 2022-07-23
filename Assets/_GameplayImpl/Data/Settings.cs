
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{


    [JsonObject(MemberSerialization.OptIn)]
    public class Settings : ICreatable
    {
        [JsonProperty] public bool drop_one_or_all = false;

        [JsonProperty] public bool autosave_enabled = false;
        [JsonProperty] public float autosave_interval = 5f;

        // game
        public float hold_tap_time => M.Lerp(0.25f, 1.25f, hold_tap_value);
        [JsonProperty] public float hold_tap_value;

        [JsonProperty] public bool interaction_cancalable;
        [JsonProperty] public bool buildable_active;
        [JsonProperty] public bool craft_mode_one_or_all;
        [JsonProperty] public bool quantity_index_01;
        [JsonProperty] public float quantity_index_opacity;

        // camera
        [JsonProperty] public int camera_size;
        [JsonProperty] public int camera_size_min; // min 0
        [JsonProperty] public int camera_size_max; // max 4
        [JsonProperty] public bool ppc_enable = true;

        // joystick
        [JsonProperty] public float blueprint_opacity;
        [JsonProperty] public float blueprint_damping;
        [JsonProperty] public float fixed_joystick_opacity;
        [JsonProperty] public float free_joystick_opacity;

        // screen
        [JsonProperty] public bool allow_landscape_left;
        [JsonProperty] public bool allow_landscape_right;
        [JsonProperty] public bool allow_portrait_up;
        [JsonProperty] public bool allow_portrait_down;

        // anim
        [JsonProperty] public float anim_shake_amplitude;
        [JsonProperty] public float anim_shake_duration;

        // graphic
        [JsonProperty] public float postprocessing_intensity;
        [JsonProperty] public float bloom_intensity;
        [JsonProperty] public float bloom_scatter;
        [JsonProperty] public float tone_mapping_mode;
        [JsonProperty] public float white_balance_temporature;

        [JsonProperty] public float night_ambient_lightness;

        // audio
        [JsonProperty] public float master_volume;
        [JsonProperty] public float sound_volume;
        [JsonProperty] public float music_volume;

        // shader custom
        [JsonProperty] public float text_light_weight = 1; // 文字受光照影响强度


        public void RestoreDefaultSettings() {
            RestoreControlSettings();
            RestoreAnimSettings();
            RestoreGraphicSettings();
            RestoreAudioSettings();
        }

        public void RestoreControlSettings() {
            hold_tap_value = 0.25f;

            craft_mode_one_or_all = true;
            interaction_cancalable = true;
            buildable_active = true;
            quantity_index_01 = false;
            quantity_index_opacity = 1f;

            // camera
            camera_size = 2;
            camera_size_min = 1; // min 0
            camera_size_max = 3; // max 4
            ppc_enable = true;

            // joystick
            fixed_joystick_opacity = 0;
            free_joystick_opacity = 0.5f;

            // screen
            allow_landscape_left = true;
            allow_landscape_right = true;
            allow_portrait_up = true;
            allow_portrait_down = true;
        }

        public void RestoreAnimSettings() {
            // anim
            anim_shake_amplitude = 0.5f;
            anim_shake_duration = 0.5f;
            blueprint_opacity = 0.5f;
            blueprint_damping = 0.5f;
        }

        public void RestoreGraphicSettings() {

            // graphic
            postprocessing_intensity = 1;
            bloom_intensity = 0.4f;
            bloom_scatter = 0.7f;
            tone_mapping_mode = 1;
            white_balance_temporature = 0;
            night_ambient_lightness = 0.8f;
        }

        public void RestoreAudioSettings() {
            // audio
            master_volume = 0.5f;
            sound_volume = 0.5f;
            music_volume = 0.5f;
        }


        /// <summary>
        /// 首次创建时
        /// </summary>
        void ICreatable.OnCreate() {
            RestoreDefaultSettings();
            // initialize
        }

    }
}
