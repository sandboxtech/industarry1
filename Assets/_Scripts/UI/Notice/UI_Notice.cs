
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{
    public class UI_Notice : MonoBehaviour
    {

        #region notice
        [Header("Notice")]
        [SerializeField]
        private NoticeItem[] Notices;


        public static UI_Notice I { get; private set; }

        private void Awake() {
            A.Assert(I == null);
            I = this;
        }

        public void SendAtFirst(string s, Item icon = null, long duration = C.Second, long fade = C.Second / 2) {
            NoticeItem item = Notices[0];
            item.Send(s, icon, duration, fade);
            return;
        }

        public void Send(string s, Item icon = null, long duration = C.Second, long fade = C.Second / 2) {
            for (int i = 0; i < Notices.Length; i++) {
                NoticeItem item = Notices[i];
                if (item.Availble) {
                    item.Send(s, icon, duration, fade);
                    return;
                }
            }
            // 位置满了
            int min_index = 0;
            long min_time = long.MaxValue;
            for (int i = 0; i < Notices.Length; i++) {
                NoticeItem item = Notices[i];
                long end = item.EndTimespan;
                if (item.EndTimespan < min_time) {
                    min_index = i;
                    min_time = end;
                }
            }
            Notices[min_index].Send(s, icon, duration, fade);
        }

        #endregion

    }
}
