
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{
    public static class WikiPage
    {

        public static void Of<T>() => Of(typeof(T));
        public static void Of(Type type) => Scroll.Show(WikiScrollsWithHistory(type));


        private static DictionaryWithBuffer<Type, List<Scroll>> wikiScrollsBuffer = new DictionaryWithBuffer<Type, List<Scroll>>(GetWikiScrollsOfType);

        private const int return_close_button_index = 0;
        private static List<Type> browseHistory = new List<Type>(10);
        private static IReadOnlyList<Scroll> WikiScrollsWithHistory(Type type) {
            A.Assert(type != null);

            List<Scroll> list = wikiScrollsBuffer[type];

            if (browseHistory.Count >= 100) { // 太多历史则清除
                browseHistory.Clear();
            }
            if (browseHistory.Count == 0 || browseHistory[browseHistory.Count - 1] != type) {
                browseHistory.Add(type); // 记录历史
            }

            // 修改第一个，但别增加删减
            if (browseHistory.Count > 1) {
                list[return_close_button_index] = Scroll.ReturnButton(() => {
                    browseHistory.RemoveAt(browseHistory.Count - 1);
                    Of(browseHistory[browseHistory.Count - 1]);
                });
            } else {
                list[return_close_button_index] = Scroll.CloseButton;
            }

            return list;
        }

        private static DictionaryWithBuffer<Type, Type[]> interfacesBuffer = new DictionaryWithBuffer<Type, Type[]>((Type type) => type.GetInterfaces());

        private static List<Scroll> GetWikiScrollsOfType(Type type) {
            List<Scroll> list = new List<Scroll>(8);

            list.Add(Scroll.Empty); // return button

            Item item = ItemPool.GetDef(type);
            list.Add(Scroll.Slot(ItemPool.GetDef(type), SlotBackgroundType.Transparent));

            list.Add(Scroll.Space);

            Type[] interfaces = interfacesBuffer[type]; // type.GetInterfaces();

            if (interfaces.Length > 0) {
                list.Add(Scroll.Text("属性"));
            } else {
                list.Add(Scroll.Text("无属性"));
            }

            for (int i = 0; i < interfaces.Length; i++) {
                Type _interface = interfaces[i];
                string s = Attr.CN(_interface);
                if (s != null) {
                    list.Add(Scroll.Slot(ItemPool.GetDef(_interface), () => Of(_interface), SlotBackgroundType.Convex));
                }
            }
            return list;
        }
    }
}
