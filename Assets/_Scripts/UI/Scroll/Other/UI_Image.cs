
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace W
{
    public class UI_Image : MonoBehaviour
    {
        [SerializeField]
        public Image Image;

        [SerializeField]
        public RectTransform RectOfImage;

        [SerializeField]
        public RectTransform Rect;

        public Func<string> TextGetter;
    }
}
