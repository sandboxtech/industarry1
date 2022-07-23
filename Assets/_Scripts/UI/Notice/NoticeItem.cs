
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace W
{
    public class NoticeItem : MonoBehaviour
    {
        [SerializeField]
        private Text Text;
        [SerializeField]
        private Slot Slot;


        public long EndTimespan => end_visible - G.now;
        public bool Availble => !Text.enabled;

        private Color InitialColor;
        private void Awake() {
            Text.enabled = false;
            InitialColor = Text.color;
        }

        private long end_opaque;
        private long end_visible;

        private long start;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"> 显示内容 </param>
        /// <param name="duration"> 持续时长 </param>
        /// <param name="fade"> 淡出时长 </param>
        public void Send(string content, Item item = null, long duration = C.Second * 2, long fade = C.Second / 2) {
            Text.text = content;
            Text.enabled = true;
            Text.color = new Color(InitialColor.r, InitialColor.g, InitialColor.b, 1);

            Slot.RenderItem(item);

            start = G.now;
            end_opaque = start + duration;
            end_visible = end_opaque + fade;
        }

        private void Update() {

            if (Text.enabled) {
                float opacity;
                long now = G.now;
                if (now >= end_visible) {
                    opacity = 0;
                    Text.enabled = false;
                    Text.text = null;
                    Text.color = new Color(InitialColor.r, InitialColor.g, InitialColor.b, opacity);

                    Slot.RenderItem(null);
                } else if (now >= end_opaque) {
                    float t = (float)(now - end_opaque) / (end_visible - end_opaque);
                    opacity = 1 - t;
                    Text.color = new Color(InitialColor.r, InitialColor.g, InitialColor.b, opacity);
                }
            }
        }
    }
}
