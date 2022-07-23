
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{

    /// <summary>
    /// Item 组成管理的列表
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class ItemList : __IItem_Container__, ICreatable
    {
        public void OnCreate() {

        }

        public static ItemList Create(int capacity, int accessible = -1) {
            A.Assert(capacity > 0 && accessible >= -1 && accessible <= capacity);

            ItemList items = Creator.__CreateData<ItemList>();
            items.children = new Item[capacity];
            items.available_slot_max = accessible == -1 ? capacity : accessible;
            items.first_empty_space = 0;
            return items;
        }


        [JsonProperty] private Item[] children { get; set; }
        [JsonProperty] public int available_slot_max { get; private set; } // 永远处于 0 - children.Length
        [JsonProperty] public int occupied_slot_count { get; private set; }
        [JsonIgnore] private int availableSlotCount => available_slot_max - occupied_slot_count;
        [JsonProperty] public int first_empty_space { get; private set; }



        private long lastTimeOfNotification;
        public bool HasSpace(int i = 1) {
            if (i <= 0) return true;
            if (availableSlotCount < i) {
                long now = G.now;
                if (now - lastTimeOfNotification > C.Second) {
                    lastTimeOfNotification = now;
                    UI_Notice.I.Send("背包太满", null, C.Second / 2, C.Second / 4);
                }
                return false;
            }
            return true;
        }
        public bool NoSpace(int i = 1) => !HasSpace(i);


        public void Extend(int i) {
            available_slot_max = M.Max(available_slot_max, available_slot_max + i);
            available_slot_max = M.Min(available_slot_max, children.Length);
        }


        [System.Runtime.Serialization.OnDeserialized]
        private void OnDeserialized(System.Runtime.Serialization.StreamingContext sc) {
            (this as __IItem_Container__).__Rebind_Items__();
            OnEnable();
        }
        private void OnEnable() {

        }


        void __IItem_Container__.__Rebind_Items__() {
            // from data
            for (int i = 0; i < children.Length; i++) {
                Item item = children[i];
                if (item == null) continue;
                __IItem__ _item = item;
                _item.__Container__ = this;
                _item.__ItemsIndex__ = i;
            }
        }
        void __IItem_Container__.__Remove_Item__(Item item) {
            __IItem__ _item = item;
            int index = _item.__ItemsIndex__;
            A.Assert(_item.__Container__ == this && children[index] == item);

            _item.__Container__ = null;
            _item.__ItemsIndex__ = -1;
            children[index] = null;
            occupied_slot_count--;
            first_empty_space = M.Min(index, first_empty_space);

            if (G.player.hand == this) {
                UI_Hand.I?.RenderItemAt(index);
            }
        }






        [JsonIgnore]
        public Item this[int index] {
            get {
                A.Assert(index >= 0 && index < available_slot_max);
                return children[index];
            }
            set {
                A.Assert(index >= 0 && index < available_slot_max);

                children[index]?.Detach();

                if (value != null) {
                    value.Detach();

                    __IItem__ _item = value;
                    _item.__Container__ = this;
                    _item.__ItemsIndex__ = index;

                    occupied_slot_count++;

                    children[index] = value;
                    if (index == first_empty_space) {
                        FindNextFirstEmptySpace();
                    }
                } else {

                    children[index] = null;
                    first_empty_space = M.Min(index, first_empty_space);
                }

                if (this == G.player.hand) {
                    UI_Hand.I?.RenderItemAt(index);
                }
            }
        }
        private void FindNextFirstEmptySpace() {
            while (first_empty_space < available_slot_max) {
                if (children[first_empty_space] == null) {
                    break;
                }
                first_empty_space++;
            }
        }


        public bool TryAdd(Item item) {
            A.Assert(item != null);

            if (NoSpace(1)) {
                return false;
            }

            this[first_empty_space] = item;
            return true;
        }

        public bool TryAbsorbOrAdd(Item other) {
            for (int i = 0; i < available_slot_max; i++) {
                Item item = this[i];
                if (item != null && item.TryAbsorb(other)) {
                    UI_Hand.I.RenderItemAt(i);
                    return true;
                }
            }
            return TryAdd(other);
        }

        public bool CanAbsorb(Type other) {
            for (int i = 0; i < available_slot_max; i++) {
                Item item = this[i];
                if (item != null && item.Absorbable && item.type == other) {
                    return true;
                }
            }
            return false;
        }
    }

}
