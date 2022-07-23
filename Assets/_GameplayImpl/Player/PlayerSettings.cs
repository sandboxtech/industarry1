
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{

    public static class PlayerMenu
    {
        public static void Page() {
            Scroll.Show(

                Scroll.CloseButton,

                (G.settings.camera_size_min == G.settings.camera_size_max
                    ? Scroll.Empty
                    : Scroll.SliderInt(() => CameraSizeDescriptiong(G.settings.camera_size), (int x) => UI.I.CameraSize = x,
                    G.settings.camera_size, G.settings.camera_size_max, G.settings.camera_size_min)),

                Scroll.Button("保存", PlayerSettings.SavePage),
                Scroll.Button("设置", PlayerSettings.Page),

                Scroll.Button("成就", AchievementPage),
                Scroll.Button("帮助", HelpPage),

                Scroll.Button("法术", PlayerMagic.Page),
                Scroll.Button("体力", BodyStatusPage),
                // Scroll.Button("测试", TestPage),
                Scroll.Empty
            );
        }

        private static string CameraSizeDescriptiong(int i) {
            switch (i) {
                case 0: return "相机尺寸 微";
                case 1: return "相机尺寸 小";
                case 2: return "相机尺寸 中";
                case 3: return "相机尺寸 大";
                case 4: return "相机尺寸 巨";
                default: return "???";
            }
        }



        private static void BodyStatusPage() {
            const int width = 4;
            PlayerAttr attr = G.player.attr;
            Scroll.Show(
                Scroll.ReturnButton(PlayerMenu.Page),

                Scroll.Text(() => $"营养状况 {attr.HungerStatus}"),
                Scroll.Progress(() => $"体力值{attr.energy.Value,width} - {attr.energy_max}", () => M.Clamp01((float)attr.energy.Value / attr.energy_max)),
                Scroll.Progress(() => $"饱腹值{attr.satiety.Value,width} - {attr.satiety_max}", () => M.Clamp01((float)attr.satiety.Value / attr.satiety_max)),

                Scroll.Space,

                Scroll.Progress(() => $"饱腹减少 {M.ToPercent(attr.satiety.OneSubProgress),width}%", () => attr.satiety.OneSubProgress),

                Scroll.Empty
            );
        }

        private static void AchievementPage() {
            Scroll.Show(
                Scroll.ReturnButton(PlayerMenu.Page),

                Scroll.Text($"行路里程 {G.achievement.step_count_in_life}米"),
                Scroll.Text($"完成行动 {G.achievement.interaction_completion_count_in_life}次"),
                Scroll.Text($"吃过食物 {G.achievement.food_consumed_count_in_life}次"),
                Scroll.Text($"完成交易 {G.achievement.transaction_count}次"),
                // Scroll.Text($"喝过饮料 {G.achievement.drink_consumed_count_in_life}次"),
                Scroll.Empty
            );
        }

        private static void HelpPage() {
            Scroll.Show(

                Scroll.ReturnButton(Page),
                Scroll.Text("建筑可以离线挂机运行"),
                Scroll.Text("需要手动保存游戏"),
                Scroll.Text("切换地图,游戏自动保存"),
                //Scroll.TextMulti("建筑可以离线挂机运行\n需要手动点击保存退出游戏"),
                //Scroll.TextMulti("切换地图时, 会自动保存游戏"),

                Scroll.Button("如何拆除和建造", AboutDestruct),

                Scroll.Button("被墙困住了怎么办?", AboutEscape),

                Scroll.Button("初期攻略", AboutStrategy),

                Scroll.Text($"..."),
                Scroll.Empty
            );
        }

        private static void AboutEscape() {
            Scroll.Show(
                Scroll.ReturnButton(HelpPage),

                Scroll.Text("点击角色, 点击法术, 进行闪现或传送"),

                Scroll.Empty
            );
        }

        private static void AboutDestruct() {
            Scroll.Show(
                Scroll.ReturnButton(HelpPage),

                Scroll.Text("拆除工具如上"),
                Scroll.Slot(ItemPool.GetDef<spade>().SetQuantity(1)),
                Scroll.Slot(ItemPool.GetDef<hammer>().SetQuantity(1)),
                Scroll.Text("可建造拆除物品举例如上"),
                Scroll.Slot(ItemPool.GetDef<wood_tile>().SetQuantity(1)),
                Scroll.Slot(ItemPool.GetDef<wood_door>().SetQuantity(1)),
                Scroll.Slot(ItemPool.GetDef<stone_brick>().SetQuantity(1)),
                Scroll.Slot(ItemPool.GetDef<claybaked_brick>().SetQuantity(1)),
                Scroll.Slot(ItemPool.GetDef<fiber>().SetQuantity(1)),
                Scroll.Empty
            );
        }

        private static void AboutStrategy() {
            Scroll.Show(
                Scroll.ReturnButton(HelpPage),

                Scroll.TextMulti($"如何赚取银币? 矿业方法"),
                Scroll.Slot($"出售 矿物", ItemPool.GetDef<hut_stick>().SetQuantity(1)),
                Scroll.Slot($"购买 矿井", ItemPool.GetDef<mine>().SetQuantity(1)),
                Scroll.Slot($"收集 矿物", ItemPool.GetDef<stone>().SetQuantity(1)),

                Scroll.Text($"《土木世界》没有灵魂, 不如挂机工厂1"),

                Scroll.Empty
            );
        }
    }

    public static class PlayerMagic
    {
        private static void CannotFindTarget() {
            UI_Notice.I.Send("找不到目标");
        }

        public static void Page() {
            const int width = 4;
            PlayerAttr attr = G.player.attr;
            Scroll.Show(

                Scroll.ReturnButton(PlayerMenu.Page),

                Scroll.Progress(() => $"法力值{attr.mana.Value,width} - {attr.mana.Max}", () => attr.mana.TotalProgress),
                Scroll.Progress(() => $"法力增加 {M.ToPercent(attr.mana.Progress),width}%", () => attr.mana.Progress),

                //Scroll.Button("点燃", () => {
                //    const int time = 7;
                //    int i = 0;
                //    for (; i < time; i++) {
                //        Vec2 pos = G.player.position + G.player.direction * (i + 1);
                //        if (G.map.IsInside(pos) && TryLightOnFire(pos)) {
                //            break;
                //        }
                //    }
                //    if (i == time) CannotFindTarget();
                //    UI_Scroll.I.Active = false;
                //}),
                Scroll.Button("闪现", () => {
                    if (G.player.attr.mana.Value < flashManaCost) {
                        UI_Notice.I.Send($"法力不足 {flashManaCost}");
                        return;
                    }

                    const int time = 7;
                    int i = 0;
                    for (; i < time; i++) {
                        Vec2 pos = G.player.position + G.player.direction * (i + 2);
                        if (G.map.IsInside(pos) && TryFlashOn(pos)) {
                            G.player.attr.mana.Value -= flashManaCost;
                            break;
                        }
                    }
                    if (i == time) CannotFindTarget();
                    Scroll.Close();
                }),
                Scroll.Button("传送", () => {
                    if (G.player.attr.mana.Value < teleportManaCost) {
                        UI_Notice.I.Send($"法力不足 {teleportManaCost}");
                        return;
                    }

                    const int time = 32;
                    const int Margin = 3 * Map.Margin;
                    int i = 0;
                    for (; i < time; i++) {
                        uint x = G.frame_seed % (uint)(G.map.size.x - 2 * Margin) + Margin;
                        uint y = (G.frame_seed >> 4) % (uint)(G.map.size.y - 2 * Margin) + Margin;
                        Vec2 pos = new Vec2(x, y);
                        if (G.map.IsInside(pos) && TryTeleport(pos)) {
                            G.player.attr.mana.Value -= teleportManaCost;
                            break;
                        }
                    }
                    if (i == time) CannotFindTarget();
                    Scroll.Close();
                }),
                Scroll.Empty
            );
        }
        public static bool CanLightOnFire(Vec2 pos) => false;
        public static bool TryLightOnFire(Vec2 pos) {
            if (CanLightOnFire(pos)) {
                G.map[pos] = Item.Of<wildfire>();
                Particle.CreateExplosion(pos);
                return true;
            }
            return false;
        }
        private const long flashManaCost = 100;
        public static bool TryFlashOn(Vec2 pos) {
            if (G.map.CanEnter(pos)) {
                Particle.CreateDust(G.player.position);
                G.player.position = pos;
                Particle.CreateDust(pos);
                return true;
            }
            return false;
        }
        private const long teleportManaCost = 100;
        public static bool TryTeleport(Vec2 pos) {
            if (G.map.CanEnter(pos)) {
                G.player.position = pos;
                Particle.CreateDust(pos);
                return true;
            }
            return false;
        }
    }

    public static class PlayerSettings
    {
        public static void Page() {
            Scroll.Show(

                Scroll.ReturnButton(PlayerMenu.Page),

                Scroll.Text($"一天时间 {(int)(Light.I.DayProgress * 24)}点"),
                // Scroll.ReturnButton(PlayerPage),
                Scroll.Button("保存游戏", SavePage),
                Scroll.Button("保存退出", () => { GameEntry.I.Save(); GameEntry.I.ExitGame(); }),

                Scroll.Button("控制设置", ControlSettingsPage),

                Scroll.Button("动画设置", AnimSettings),
                Scroll.Button("画面设置", GraphicSettingsPage),
                Scroll.Button("音频设置", AudioSettingsPage),


                Scroll.Space,
                Scroll.Button("全部恢复默认设置", () => { G.settings.RestoreDefaultSettings(); RestoreSettingsSuccessPage(); }),
                Scroll.Empty
            );
        }

        public const string ENABLED = "已开启";
        public const string DISABLED = "已关闭";
        public static string Able(bool b) => b ? ENABLED : DISABLED;


        public static void SavePage() {
            GameEntry.I.Save();
            Scroll.Show(
                Scroll.CloseButton,
                // Scroll.ReturnButton(PlayerPage),
                Scroll.Text("游戏已经成功保存"),
                Scroll.Button("退出游戏", GameEntry.I.ExitGame),

                Scroll.Space,
                // Scroll.Button("删除存档", DeleteSavePage),
                Scroll.Empty
            );
        }
        private static void DeleteSavePage() {
            Scroll.Show(
                Scroll.CloseButton,
                Scroll.ReturnButton(SavePage),
                Scroll.Text($"确认删除存档? 需要重启游戏"),
                Scroll.Button(Color4.red.Dye("确定删除存档"), () => {
                    GameEntry.I.DeleteSave();
                    GameEntry.I.ExitGame();
                }),
                Scroll.Empty
            );
        }

        private static void RestoreSettingsSuccessPage() {
            Scroll.Show(
                Scroll.Text("已恢复默认设置"),
                Scroll.Text("部分设置需要保存并退出游戏才能生效"),
                Scroll.CloseButton,
                Scroll.ReturnButton(Page),
                Scroll.Space,
                Scroll.Button("保存游戏", SavePage),
                Scroll.Empty
            );
        }


        public static int AutosaveSeconds { get => 120 + (int)(G.settings.autosave_interval * 30f * 60f); }
        private static void ControlSettingsPage() {
            Scroll.Show(
                Scroll.ReturnButton(Page),

                Scroll.Button($"自动存档{(G.settings.autosave_enabled ? "已开启" : "已关闭")}", () => {
                    G.settings.autosave_enabled = !G.settings.autosave_enabled;
                    ControlSettingsPage();
                }),

                Scroll.Slider(
                    () => $"自动存档间隔 {AutosaveSeconds}秒",
                    (float x) => G.settings.autosave_interval = x,
                    G.settings.autosave_interval
                ),


                Scroll.Slider(
                    () => $"长按时长分界 {G.settings.hold_tap_time:0.00} 秒",
                    (float x) => G.settings.hold_tap_value = x,
                    G.settings.hold_tap_value
                ),

                Scroll.Button(
                    () => $"移动进行建造 {Able(G.settings.buildable_active)}",
                    () => G.settings.buildable_active = !G.settings.buildable_active),

                Scroll.Button(
                    () => $"移动取消交互 {Able(G.settings.interaction_cancalable)}",
                    () => G.settings.interaction_cancalable = !G.settings.interaction_cancalable),


                //Scroll.Button(
                //    () => $"碰撞时使用建筑 {Able(G.settings.try_enter_factorylike)}",
                //    () => G.settings.try_enter_factorylike = !G.settings.try_enter_factorylike),

                Scroll.Button(
                    () => $"单个物品显示角标 {Able(G.settings.quantity_index_01)}",
                    () => G.settings.quantity_index_01 = !G.settings.quantity_index_01),

                Scroll.Button("摇杆设置", JoystickSettings),
                Scroll.Button("屏幕旋转设置", ScreenRorationPage),
                Scroll.Space,
                Scroll.Button("恢复默认控制设置", () => { G.settings.RestoreControlSettings(); RestoreSettingsSuccessPage(); }),
                Scroll.Empty
            );
        }



        private static void JoystickSettings() {
            Scroll.Show(
                Scroll.ReturnButton(ControlSettingsPage),


                Scroll.Text("摇杆不透明度调节至0，可禁用摇杆"),
                Scroll.Slider(
                    () => StrPool.Format("自由摇杆不透明度 {0,3}%", (int)(UI_Joystick.I.FreeOpacity * 100)),
                    (float x) => UI_Joystick.I.FreeOpacity = x,
                    UI_Joystick.I.FreeOpacity),
                Scroll.Slider(
                    () => StrPool.Format("固定摇杆不透明度 {0,3}%", (int)(UI_Joystick.I.FixedOpacity * 100)),
                    (float x) => UI_Joystick.I.FixedOpacity = x,
                    UI_Joystick.I.FixedOpacity),

                Scroll.Empty
            );
        }

        private static void ScreenRorationPage() {
            Scroll.Show(
                Scroll.ReturnButton(ControlSettingsPage),

                Scroll.Button(() => $"横屏正向 {Able(G.settings.allow_landscape_left)}",
                () => { bool v = G.settings.allow_landscape_left = !G.settings.allow_landscape_left; UI.I.SyncRotation(); }),
                Scroll.Button(() => $"横屏反向 {Able(G.settings.allow_landscape_right)}",
                () => { bool v = G.settings.allow_landscape_right = !G.settings.allow_landscape_right; UI.I.SyncRotation(); }),
                Scroll.Button(() => $"竖屏正向 {Able(G.settings.allow_portrait_up)}",
                () => { bool v = G.settings.allow_portrait_up = !G.settings.allow_portrait_up; UI.I.SyncRotation(); }),
                Scroll.Button(() => $"竖屏反向 {Able(G.settings.allow_portrait_down)}",
                () => { bool v = G.settings.allow_portrait_down = !G.settings.allow_portrait_down; UI.I.SyncRotation(); }),

                Scroll.Empty
            );
        }



        private static void AnimSettings() {
            Scroll.Show(
                Scroll.ReturnButton(Page),

                Scroll.Slider(() => $"抖动动画幅度 {(int)(100 * G.settings.anim_shake_amplitude)}%", (float x) => {
                    G.settings.anim_shake_amplitude = x;
                }, G.settings.anim_shake_amplitude),
                Scroll.Slider(() => $"抖动动画时长 {(int)(100 * G.settings.anim_shake_duration)}%", (float x) => {
                    G.settings.anim_shake_duration = x;
                }, G.settings.anim_shake_duration),

                Information.I.IsInStandalone ? Scroll.Slider(
                    () => $"建筑蓝图不透明度 {(int)(G.settings.blueprint_opacity * 100),3}%",
                    (float x) => G.settings.blueprint_opacity = x,
                    UI_Joystick.I.FreeOpacity) : Scroll.Empty,
                Information.I.IsInStandalone ? Scroll.Slider(
                    () => $"建筑蓝图移动时间 {(int)(G.settings.blueprint_damping * 100),3}%",
                    (float x) => G.settings.blueprint_damping = x,
                    G.settings.blueprint_damping) : Scroll.Empty,

                Scroll.Space,
                Scroll.Button("恢复默认动画设置", () => { G.settings.RestoreAnimSettings(); RestoreSettingsSuccessPage(); }),

                Scroll.Empty
            );
        }



        private static void GraphicSettingsPage() {
            Scroll.Show(
                Scroll.ReturnButton(Page),

                Scroll.Text("荧光强度调节至零, 可降低耗电发热"),
                Scroll.Slider(
                    () => StrPool.Format("荧光强度 {0,3}%", (int)(UI_PostProcessing.I.BloomIntensity * 100)),
                    (float x) => UI_PostProcessing.I.BloomIntensity = x,
                    UI_PostProcessing.I.BloomIntensity),
                Scroll.Slider(
                    () => StrPool.Format("荧光散射 {0,3}%", (int)(UI_PostProcessing.I.BloomScatter * 100)),
                    (float x) => UI_PostProcessing.I.BloomScatter = x,
                    UI_PostProcessing.I.BloomScatter),

                Scroll.Slider(
                    () => StrPool.Format("夜间光照 {0,3}%", (int)(Light.I.NightLightness * 100)),
                    (float x) => Light.I.NightLightness = x,
                    Light.I.NightLightness),

                Scroll.Button( // 色彩模式
                    () => UI_PostProcessing.I.CurrentTonemappingMode.Item1,
                    UI_PostProcessing.I.SwitchTonemappingMode
                ),

                Scroll.Button("恢复默认图形设置", () => { G.settings.RestoreGraphicSettings(); RestoreSettingsSuccessPage(); }),
                Scroll.Empty
            );
        }

        private static void AudioSettingsPage() {
            Scroll.Show(

                Scroll.ReturnButton(Page),

                Audio.I.PlayingMusicName == null ? Scroll.Empty : Scroll.Text($"当前播放音乐 {Audio.I.PlayingMusicName}"),

                Scroll.Button($"播放下一首", () => { Audio.I.PlayNextMusic();  AudioSettingsPage(); }),

                Scroll.Slider(
                    () => StrPool.Format("主音量   {0,3}%", (int)(Audio.I.MasterVolume * 100)),
                    (float x) => Audio.I.MasterVolume = x,
                    Audio.I.MasterVolume),
                Scroll.Slider(
                    () => StrPool.Format("音效音量 {0,3}%", (int)(Audio.I.SoundVolume * 100)),
                    (float x) => Audio.I.SoundVolume = x,
                    Audio.I.SoundVolume),
                Scroll.Slider(
                    () => StrPool.Format("音乐音量 {0,3}%", (int)(Audio.I.MusicVolume * 100)),
                    (float x) => Audio.I.MusicVolume = x,
                    Audio.I.MusicVolume),

                Scroll.Space,
                Scroll.Button("恢复默认音频设置", () => { G.settings.RestoreAudioSettings(); RestoreSettingsSuccessPage(); }),

                Scroll.Empty
            );
        }
    }
}
