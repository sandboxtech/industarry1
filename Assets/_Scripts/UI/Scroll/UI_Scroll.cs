
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace W
{
    public enum SlotBackgroundType
    {
        Default,
        Convex,
        Concave,
        Transparent,
    }

    public enum ScrollType
    {
        None,

        Image,
        Text,
        MultiText,
        Slot,
        Button,
        Grid,

        Slider,
        Progress,
    }

    public enum UISound
    {
        None, Click, Close, Error,
    }

    public struct Scroll
    {
        public static void Show(IEnumerable<Scroll> scrolls, Item caller = null) {
            UI_Scroll.I.Show(scrolls);
        }
        public static void Show(params Scroll[] scrolls) => Show(scrolls as IEnumerable<Scroll>);


        public static void Close() {
            UI_Scroll.I.Active = false;
        }


        public ScrollType type;

        public Color4 color;

        public string text;
        public Func<string> text_dynamic;

        public Action on_tap;
        public UISound ui_sound;

        public Action<float> slider_setter;
        public Action<int> slider_int_setter;
        public Func<float> slider_getter;
        public float slider_initial_value;
        public int slider_int_min_value;
        public int slider_int_max_value;

        public int grid_count;
        public Func<int, bool, Item> grid_ontap;

        public Item slot_item;
        // public Func<(string, Item)> slot_getter;
        public bool slot_right_used;
        public SlotBackgroundType slot_bg;


        public static Scroll Empty => new Scroll {
            type = ScrollType.None,
        };

        public static Scroll Space => new Scroll {
            type = ScrollType.Text,
            text = " ",
        };

        public static Scroll Destruct(Item item) => Button("拆除", () => { if (item.Destructable) { item.Destruct(); UI_Scroll.I.Active = false; } });

        public static Scroll Image(string key) => Image(key, Color4.white);

        public static Scroll Image(string key, Color4 color) => new Scroll {
            type = ScrollType.Image,
            text = key,
            color = color,
        };
        public static Scroll Image(Func<string> key) => Image(key, Color.white);
        public static Scroll Image(Func<string> key, Color4 color) => new Scroll {
            type = ScrollType.Image,
            text_dynamic = key,
            color = color,
        };

        public static Scroll Text(string text) => new Scroll {
            type = ScrollType.Text,
            text = text,
        };
        public static Scroll Text(Func<string> text_dynamic) => new Scroll {
            type = ScrollType.Text,
            text_dynamic = text_dynamic,
        };

        public static Scroll TextMulti(string text) => new Scroll {
            type = ScrollType.MultiText,
            text = text,
        };

        public static Scroll CloseButton => new Scroll {
            type = ScrollType.Button,
            text = "关闭",
            on_tap = () => { UI_Scroll.I.Active = false; Audio.I.PlayerClose(); },
        };

        public static Scroll ReturnButton(Action tap) => new Scroll {
            type = ScrollType.Button,
            text = "返回",
            on_tap = tap,
            ui_sound = UISound.Close,
        };

        public static Scroll Button(string text, Action tap) => new Scroll {
            type = ScrollType.Button,
            text = text,
            on_tap = tap,
        };

        public static Scroll Button(Func<string> text, Action tap) => new Scroll {
            type = ScrollType.Button,
            text_dynamic = text,
            on_tap = tap,
        };



        public static Scroll Slider(Func<string> dynamic_text, Action<float> setter, float initial_value) => new Scroll {
            type = ScrollType.Slider,
            text_dynamic = dynamic_text,
            slider_setter = setter,
            slider_initial_value = initial_value,
        };

        public static Scroll SliderInt(Func<string> dynamic_text, Action<int> setter, int initial_value, int max_value, int min_value = 0) => new Scroll {
            type = ScrollType.Slider,
            text_dynamic = dynamic_text,
            slider_int_setter = setter,
            slider_initial_value = (float)initial_value,
            slider_int_max_value = max_value,
            slider_int_min_value = min_value,
        };

        public static Scroll Progress(Func<string> dynamic_text, Func<float> getter) => new Scroll {
            type = ScrollType.Progress,
            text_dynamic = dynamic_text,
            slider_getter = getter,
        };

        public static Scroll Progress(string text, float slider_initial_value) => new Scroll {
            type = ScrollType.Progress,
            text = text,
            slider_initial_value = slider_initial_value,
        };

        public static Scroll IdleProgress(Idle idle, string prefix = null) => Progress(() => $"{prefix}{M.ToPercent(idle.Progress)}%  {idle.ProgressTimeLeftDescription}", () => idle.Progress);
        public static Scroll IdleTotalProgress(Idle idle, string prefix = null) => Progress(() => $"{prefix}{idle.Value} / {idle.Max}", () => idle.TotalProgress);


        public static Scroll Slot(string text, Item item, Action on_tap = null, SlotBackgroundType bg = SlotBackgroundType.Default) => new Scroll {
            type = ScrollType.Slot,
            text = text,
            slot_item = item,
            on_tap = on_tap,
            slot_bg = bg,
        };
        public static Scroll Slot(Item item, Action on_tap, SlotBackgroundType bg = SlotBackgroundType.Default) => Slot(item.Description, item, on_tap, bg);
        public static Scroll Slot(Item item, SlotBackgroundType bg = SlotBackgroundType.Default) => Slot(item, null, bg);
        public static Scroll SlotRight(string text, Item item, Action on_tap = null, SlotBackgroundType bg = SlotBackgroundType.Default) => new Scroll {
            type = ScrollType.Slot,
            text = text,
            slot_item = item,
            on_tap = on_tap,
            slot_bg = bg,
            slot_right_used = true,
        };
        public static Scroll SlotRight(Item item, Action on_tap, SlotBackgroundType bg = SlotBackgroundType.Default) => SlotRight(item.Description, item, on_tap, bg);
        public static Scroll SlotRight(Item item, SlotBackgroundType bg = SlotBackgroundType.Default) => SlotRight(item, null, bg);



        public static Scroll Grid(int grid_count, Func<int, bool, Item> tap_grid) => new Scroll {
            type = ScrollType.Grid,
            grid_count = grid_count,
            grid_ontap = tap_grid,
        };
    }


    public class UI_Scroll : MonoBehaviour
    {

        public static UI_Scroll I { get; private set; }
        private void Awake() {
            A.Assert(I == null);
            I = this;
        }

        [Header("Border")]
        [SerializeField] private Image Background;
        [SerializeField] private Transform Border;


        [Header("Scroll")]

        [SerializeField]
        private ScrollRect Scroll;
        [SerializeField]
        private RectTransform Content;

        [Header("Scroll Prefab")]


        [SerializeField] private UI_Text UI_Text_Prefab;
        [SerializeField] private UI_Image UI_Image_Prefab;
        [SerializeField] private UI_TextMultiline UI_MultiText_Prefab;
        [SerializeField] private UI_Button UI_Button_Prefab;
        [SerializeField] private UI_Slider UI_Slider_Prefab;
        [SerializeField] private UI_Slot UI_SlotLeft_Prefab;
        [SerializeField] private UI_Slot UI_SlotRight_Prefab;

        [SerializeField] private UI_Grid UI_Grid_Prefab;
        [SerializeField] private Slot Slot_Prefab;


        public Color4 UIColor {
            set {
                Background.color = value;
                ColorBorder(Border, value);
                UI_Button_Prefab.UIColor = value;
                UI_Slider_Prefab.UIColor = value;
                UI_SlotLeft_Prefab.UIColor = value;
                UI_SlotRight_Prefab.UIColor = value;
                Slot_Prefab.UIColor = value;
            }
        }

        private void ClearTransform(Transform trans) {
            int count = trans.childCount;
            for (int i = count - 1; i >= 0; i--) {
                Destroy(trans.GetChild(i).gameObject);
            }
        }

        public bool Active {
            get => Scroll.gameObject.activeSelf;
            set {
                if (!value) {
                    if (Active) {
                        PlaySound(UISound.Close);
                    }
                    ClearScroll();
                } else {
                    if (!Active) {
                        PlaySound(UISound.Click);
                    }
                }
                Scroll.gameObject.SetActive(value);
            }
        }

        private void ClearScroll() {
            ClearTransform(Content);
            ClearUpdate();
        }

        // public void Show(params Scroll[] items) => ShowItems(items);

        private long lastShow = 0;
        public void Show(IEnumerable<Scroll> items) {
            long now = G.now;
            A.Assert(now != lastShow, () => "同一帧发生两次ui展示事件，是不是有问题");
            lastShow = now;

            if (Active) {
                ClearScroll();
            } else {
                Active = true;
            }

            if (items == null) { return; }


            foreach (var scroll in items) {

                AddItem(scroll);
            }
        }
        private List<Scroll> reverse_buffer = new List<Scroll>(16);

        public static void ColorBorder(Transform border, Color color) {
            foreach (Transform child in border) {
                child.GetComponent<Image>().color = color;
            }
        }


        private void AddItem(Scroll item) {
            bool isTextDynamic;
            string text = item.text;
            switch (item.type) {
                case ScrollType.Text:
                    UI_Text text_comp = Instantiate(UI_Text_Prefab, Content);
                    text_comp.transform.SetAsFirstSibling();

                    isTextDynamic = item.text_dynamic != null;
                    text_comp.Text.text = isTextDynamic ? item.text_dynamic() : item.text;
                    if (isTextDynamic) {
                        text_comp.TextGetter = item.text_dynamic;
                        texts.Add(text_comp);
                    }

                    break;

                case ScrollType.Image:
                    UI_Image image = Instantiate(UI_Image_Prefab, Content);
                    image.transform.SetAsFirstSibling();

                    image.Image.color = item.color;
                    image.Image.preserveAspect = true;
                    image.Rect.sizeDelta = image.RectOfImage.sizeDelta;

                    isTextDynamic = item.text_dynamic != null;

                    image.Image.sprite = Res.I.SpriteOf(isTextDynamic ? item.text_dynamic() : item.text);
                    if (isTextDynamic) {
                        image.TextGetter = item.text_dynamic;
                        images.Add(image);
                    }

                    break;

                case ScrollType.MultiText:
                    UI_TextMultiline textMultiline = Instantiate(UI_MultiText_Prefab, Content);
                    textMultiline.transform.SetAsFirstSibling();

                    textMultiline.Text.text = text;
                    break;

                case ScrollType.Button:
                    UI_Button button = Instantiate(UI_Button_Prefab, Content);
                    button.transform.SetAsFirstSibling();

                    button.Button.enabled = item.on_tap != null;

                    isTextDynamic = item.text_dynamic != null;
                    button.Text.text = isTextDynamic ? item.text_dynamic() : item.text;

                    if (isTextDynamic) {
                        button.OnTap = () => {
                            PlaySound(item.ui_sound);
                            item.on_tap?.Invoke();
                            button.Text.text = item.text_dynamic();
                        };
                    } else {
                        button.OnTap = () => {
                            PlaySound(item.ui_sound);
                            item.on_tap?.Invoke();
                        };
                    }

                    break;

                case ScrollType.Slot:
                    UI_Slot slot = item.slot_right_used ? Instantiate(UI_SlotRight_Prefab, Content) : Instantiate(UI_SlotLeft_Prefab, Content);
                    slot.transform.SetAsFirstSibling();

                    slot.Text.text = item.text;
                    slot.Slot.RenderItem(item.slot_item);
                    slot.Slot.SetSlotBg(item.slot_bg);

                    slot.Slot.OnTap = () => {
                        PlaySound(item.ui_sound);
                        item.on_tap?.Invoke();
                    };
                    break;


                case ScrollType.Slider:
                    UI_Slider slider = Instantiate(UI_Slider_Prefab, Content);
                    slider.transform.SetAsFirstSibling();

                    slider.Slider.interactable = true;

                    isTextDynamic = item.text_dynamic != null;
                    slider.Text.text = isTextDynamic ? item.text_dynamic() : item.text;

                    bool isIntSlider = item.slider_setter == null;

                    A.Assert((item.slider_setter != null || item.slider_int_setter != null) && item.text_dynamic != null);

                    if (isIntSlider) {
                        slider.Slider.minValue = item.slider_int_min_value;
                        slider.Slider.maxValue = item.slider_int_max_value;
                        slider.Slider.wholeNumbers = true;

                        if (isTextDynamic) {
                            slider.Setter = (float x) => {
                                item.slider_int_setter.Invoke((int)x);
                                slider.Text.text = item.text_dynamic();
                            };  // sync dynamic text
                        } else {
                            slider.Setter = (float x) => item.slider_int_setter((int)x);
                        }
                    } else {
                        //slider.Setter = isTextDynamic
                        //    ? (float x) => {
                        //        item.slider_setter.Invoke(x);
                        //        slider.Text.text = item.text_dynamic();
                        //    }
                        //: item.slider_setter;
                        if (isTextDynamic) {
                            slider.Setter = (float x) => {
                                item.slider_setter.Invoke(x);
                                slider.Text.text = item.text_dynamic();
                            };
                        }
                        else {
                            slider.Setter = item.slider_setter;
                        }
                    }

                    slider.Slider.value = item.slider_initial_value;

                    break;

                case ScrollType.Progress:
                    UI_Slider progress = Instantiate(UI_Slider_Prefab, Content);
                    progress.transform.SetAsFirstSibling();

                    if (item.slider_getter != null) {
                        progresses.Add(progress);
                        progress.Slider.value = item.slider_getter.Invoke();

                    } else {
                        progress.Slider.value = item.slider_initial_value;
                    }

                    progress.SliderBackground.raycastTarget = false;
                    progress.Slider.interactable = false;
                    progress.Slider.transition = Selectable.Transition.None; // 禁用交互无动画

                    isTextDynamic = item.text_dynamic != null;
                    progress.Text.text = isTextDynamic ? item.text_dynamic() : item.text;

                    progress.Getter = item.slider_getter;
                    progress.TextGetter = item.text_dynamic;
                    break;

                case ScrollType.Grid:

                    UI_Grid grid = Instantiate(UI_Grid_Prefab, Content);
                    grid.transform.SetAsFirstSibling();

                    int grid_count = item.grid_count;
                    for (int i = 0; i < grid_count; i++) {
                        Slot slot_grid = Instantiate(Slot_Prefab, grid.Trans);
                        if (item.grid_ontap != null) {
                            int j = i;
                            slot_grid.RenderItem(item.grid_ontap(j, false));
                            slot_grid.OnTap = () => {
                                Item slot_item = item.grid_ontap(j, true);
                                slot_grid.RenderItem(slot_item);
                                PlaySound(UISound.Click);
                            };
                        }
                    }

                    break;

                case ScrollType.None:
                    break;
                default:
                    A.Throw();
                    break;
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(Content);
        }

        private List<UI_Slider> progresses = new List<UI_Slider>();
        private List<UI_Text> texts = new List<UI_Text>();
        private List<UI_Image> images = new List<UI_Image>();
        private void ClearUpdate() {
            progresses.Clear();
            texts.Clear();
            images.Clear();
        }
        private void Update() {
            for (int i = 0; i < progresses.Count; i++) {
                UI_Slider progress = progresses[i];

                progress.Slider.value = progress.Getter();
                progress.Text.text = progress.TextGetter();
            }
            for (int i = 0; i < texts.Count; i++) {
                UI_Text text = texts[i];
                text.Text.text = text.TextGetter();
            }
            for (int i = 0; i < images.Count; i++) {
                UI_Image image = images[i];
                image.Image.sprite = Res.I.SpriteOf(image.TextGetter());
            }
        }



        private void PlaySound(UISound sound) {
            switch (sound) {
                case UISound.None:
                    Audio.I.PlayerClick();
                    break;
                case UISound.Click:
                    Audio.I.PlayerClick();
                    break;
                case UISound.Close:
                    Audio.I.PlayerClose();
                    break;
                case UISound.Error:
                    Audio.I.PlayerError();
                    break;
                default:
                    Audio.I.PlayerClick();
                    break;
            }
        }
    }
}
