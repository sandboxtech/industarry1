
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace W
{
    public class UI_Slider : MonoBehaviour
    {
        [SerializeField] public Slider Slider;
        [SerializeField] public Text Text;

        [SerializeField] public Transform Border;
        [SerializeField] public Image SliderBackground;
        [SerializeField] public Image SliderFill;

        [NonSerialized] public Action<float> Setter;
        [NonSerialized] public Func<float> Getter;

        [NonSerialized] public Func<string> TextGetter;

        public void Slide() {
            Setter?.Invoke(Slider.value);
        }

        public Color UIColor {
            set {
                UI_Scroll.ColorBorder(Border, value);
                SliderBackground.color = value;
                SliderFill.color = value;
            }
        }
    }
}
