
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace W
{
    public class UI_Button : MonoBehaviour
    {
        [SerializeField] public Button Button;
        [SerializeField] public Image Background;
        [SerializeField] public Text Text;
        [SerializeField] public Transform Border;

        public Action OnTap;
        public void Tap() {
            OnTap?.Invoke();
        }


        public Color UIColor {
            set {
                Background.color = value;
                UI_Scroll.ColorBorder(Border, value);
                UI_Scroll.ColorBorder(Border, value);
            }
        }
    }
}
