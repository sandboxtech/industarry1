
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace W
{
    public class UI_Slot : MonoBehaviour
    {
        [SerializeField] public Slot Slot;
        [SerializeField] public Text Text;

        public Color4 UIColor { set {
                Slot.Background.color = value;
            } 
        }

    }
}
