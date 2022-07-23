
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace W
{
    public class UI_TextMultiline : MonoBehaviour
    {
        [SerializeField] public Text Text;

        private const float Width = 144;
        private const float TopBottomMargin = 4;
        private void Start() {
            var rect = GetComponent<RectTransform>();

            rect.sizeDelta = new Vector2(Width, Text.preferredHeight + 2 * TopBottomMargin);
        }
    }
}
