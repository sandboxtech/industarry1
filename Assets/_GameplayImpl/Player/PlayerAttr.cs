
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PlayerAttr : ICreatable
    {
        public void OnCreate() {
            mana = Idle.Create(0, 10_000);

            satiety_max = 10_000; // 2.4+hours to 24+hour

            satiety = Idle.Create(satiety_max / 2, long.MaxValue, C.Second, -1);


            energy_max = 100_000;

            energy = Idle.Create(energy_max / 2, long.MaxValue, long.MaxValue, 0);
        }

        [JsonProperty] public long satiety_max { get; private set; }
        [JsonProperty] public Idle satiety { get; private set; }
        [JsonProperty] public long energy_max { get; private set; }
        [JsonProperty] public Idle energy { get; private set; }

        [JsonProperty] public Idle mana { get; private set; }

        public bool IsHungry => satiety.Value < satiety_max / 10 && energy.Value < energy_max / 2;
        public string HungerStatus => IsHungry ? "饥饿" : (satiety.Value > satiety_max / 5 * 4 || energy.Value > energy_max / 5 * 4) ? "饱腹" : "正常";

        public bool TryEat(long satiety, long e, Item item, bool drink = false, Type packaging=null) {
            if (!CanEat(satiety, e, packaging)) {
                return false;
            }
            Eat(satiety, e, packaging);
            NoticeEat(satiety, e, item, drink);
            return true;
        }
        private void NoticeEat(long satiety, long e, Item item, bool drink) {
            G.player.state.TryEat();
            Item.ShakeHand();

            if (drink) {
                G.achievement.drink_consumed_count_in_life++;
                Audio.I.PlayerDrink();
            } else {
                Audio.I.PlayerEat();
                G.achievement.food_consumed_count_in_life++;
            }

            string str_satiety = satiety > 0 ? $"饱腹增加 {satiety}" : satiety < 0 ? $"饱腹减少 {-satiety}" : null;
            string str_energy = e > 0 ? $"体力增加 {e} " : e < 0 ? $"体力减少 {-e}" : null;

            long c = G.player.state.timespan_eating;
            UI_Notice.I.SendAtFirst($"{str_energy}{str_satiety}", item, c * 2, 0);
        }

        public bool CanEat(long satiety, long e, Type packaging) {
            if (packaging != null && !G.player.hand.CanAbsorb(packaging)) {
                UI_Notice.I.Send("空间不足");
                return false;
            }

            if (e < 0 && energy.Value + e < 0) {
                UI_Notice.I.Send("体力不足");
                return false; // 吃消耗体力的食物后，体力不能为负数
            } else if (e >= 0 && energy.Value >= energy_max) {
                UI_Notice.I.Send("体力旺盛");
                return false;
            }
            if (satiety < 0 && this.satiety.Value + satiety < 0) {
                UI_Notice.I.Send("已经太饿");
                return false; // 消食片，也不能让饱腹度为负
            } else if (satiety >= 0 && this.satiety.Value >= satiety_max) {
                UI_Notice.I.Send("吃不下了");
                return false;
            }
            return true;
        }
        public void Eat(long satiety, long e, Type packaging) {
            this.satiety.Value += satiety;
            energy.Value += e;
        }





        /// <summary>
        /// 能量消耗
        /// </summary>
        public bool TryConsumeEnergy(long e) {
            if (!CanConsumeEnergy(e)) {
                NoticeNotEnoughtEnergy(e);
                return false;
            }
            ConsumeEnergy(e);
            return true;
        }

        public void NoticeNotEnoughtEnergy(long e) {
            UI_Notice.I.Send($"体力不足 {e}");
        }

        public bool CanConsumeEnergy(long e) {
            return energy.Value >= e;
        }

        public void ConsumeEnergy(long e) {
            energy.Value -= e;
        }
    }
}
