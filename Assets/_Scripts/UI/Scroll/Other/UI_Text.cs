
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace W
{
    public class UI_Text : MonoBehaviour
    {
        [SerializeField] public Text Text;

        [NonSerialized] public Func<string> TextGetter;
    }
}
