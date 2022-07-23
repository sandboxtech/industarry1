
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{
    public partial class Audio : MonoBehaviour
    {
        public static Audio I { get; private set; }
        private void Awake() {
            A.Assert(I == null);
            I = this;
        }


        private uint musicIndex;

        private void Start() {
            RecalculateVolume();

            musicIndex = H.Hash((uint)G.now_real);
            silencedTime = musicInterval * 0.9f;
        }

        public string PlayingMusicName = null;

        private float silencedTime = 0;
        private float musicInterval = 60f;


        private void Update() {
            if (!Music.isPlaying && G.settings.music_volume > 0) {
                silencedTime += Time.deltaTime;
                if (silencedTime >= musicInterval) {
                    silencedTime -= musicInterval;

                    PlayNextMusic();
                }
            }
            if (Music.isPlaying) {
                Music.volume = Mathf.SmoothDamp(Music.volume, 1, ref v, 5f); // fade in
            } else {
                Music.volume = Mathf.SmoothDamp(Music.volume, 0, ref v, 5f); // fade out
            }
        }
        private float v;

        public void PlayNextMusic() {
            AudioClip[] clips = Light.I.DayProgress > 0.25f && Light.I.DayProgress < 0.75f ? MusicsHappy : MusicsEmotional;
            AudioClip clip = clips[musicIndex % clips.Length];
            PlayingMusicName = clip.name;
            PlayMusicClip(clip);
            musicIndex++;
        }


        [Header("Musics")]
        [SerializeField] private AudioClip[] MusicsEmotional;
        [SerializeField] private AudioClip[] MusicsHappy;


        [Header("Mixer")]
        [SerializeField] private UnityEngine.Audio.AudioMixer Mixer;

        public const string MasterStr = "Master";
        public const string SoundStr = "Sound";
        public const string MusicStr = "Music";

        public float MasterVolume {
            get {
                return G.settings.master_volume;
            }
            set {
                G.settings.master_volume = value;
                RecalculateVolume();
                // Mixer.SetFloat(MasterStr, M.Lerp(-80, 20, value));
            }
        }
        public float SoundVolume {
            get {
                return G.settings.sound_volume;
            }
            set {
                G.settings.sound_volume = value;
                RecalculateVolume();
                // Mixer.SetFloat(SoundStr, M.Lerp(-80, 10, value));
            }
        }
        public float MusicVolume {
            get {
                return G.settings.music_volume;
            }
            set {
                G.settings.music_volume = value;
                RecalculateVolume();
                // Mixer.SetFloat(MusicStr, M.Lerp(-80, 0, value));
            }
        }

        private void RecalculateVolume() {
            Sound.volume = 2 * SoundVolume * MasterVolume;
            Music.volume = 2 * MusicVolume * MasterVolume;

        }


        [Header("Source")]
        [SerializeField] private AudioSource Sound;
        [SerializeField] private AudioSource Music;


        [Header("UI")]
        [SerializeField] private AudioClip uiClick;
        [SerializeField] private AudioClip uiClose;
        [SerializeField] private AudioClip uiError;

        public void PlayerClick(float volumeScale = 1) => PlaySoundClip(uiClick, volumeScale);
        public void PlayerClose(float volumeScale = 1) => PlaySoundClip(uiClose, volumeScale);
        public void PlayerError(float volumeScale = 1) => PlaySoundClip(uiError, volumeScale);

        [Header("Interaction")]
        [SerializeField] private AudioClip[] drink;
        public void PlayerDrink(float volumeScale = 1) => PlaySoundClip(Information.I.RandomOne(drink), volumeScale);
        [SerializeField] private AudioClip[] eat;
        public void PlayerEat(float volumeScale = 1) => PlaySoundClip(Information.I.RandomOne(eat), volumeScale);
        [SerializeField] private AudioClip[] pickdrop;
        public void PlayerPick(float volumeScale = 1) => PlaySoundClip(Information.I.RandomOne(pickdrop), volumeScale);

        [Header("FootSteps")]
        [SerializeField] private AudioClip[] grass; public AudioClip[] Grass => grass;
        [SerializeField] private AudioClip[] stone; public AudioClip[] Stone => stone;
        [SerializeField] private AudioClip[] wood; public AudioClip[] Wood => wood;
        [SerializeField] private AudioClip[] gravel; public AudioClip[] Gravel => gravel;

        [SerializeField] private AudioClip[] snow; public AudioClip[] Snow => snow;
        [SerializeField] private AudioClip[] sand; public AudioClip[] Sand => sand;
        [SerializeField] private AudioClip[] cloth; public AudioClip[] Cloth => cloth;


        // private long lastPlay = 0;
        public void PlaySoundClip(AudioClip clip, float volumeScale, bool randomPitch = true) {
            // Debug.Log("where last");

            //// 测试期间。一帧内不能有两次声音播放
            //long now = G.now;
            //A.Assert(now != lastPlay, () => clip.name);
            //lastPlay = now;
            Sound.pitch = randomPitch ? M.Lerp(0.8f, 1.25f, H.HashFloat(Information.I.FrameSeed, 0)) : 1f;
            Sound.PlayOneShot(clip, volumeScale);
        }

        public void PlayMusicClip(AudioClip clip) {
            Music.PlayOneShot(clip, G.settings.music_volume);
        }

        public void PlayRandomSoundClip(AudioClip[] clips, float volumeScale = 1f) {
            AudioClip clip = Information.I.RandomOne(clips); //  clips[Information.I.FrameSeed % clips.Length];
            PlaySoundClip(clip, volumeScale);
        }

        public void PlaySoundClip(string key, float volumeScale = 1f) {
            AudioClip clip = Res.I.ClipOf(key);
            Sound.PlayOneShot(clip, volumeScale);
        }
    }
}
