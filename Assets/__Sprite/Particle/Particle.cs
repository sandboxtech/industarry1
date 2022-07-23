
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{
    public partial class Particle
    {
        private static long LerpFromAnimNoise(float min, float max) => (long)(C.Second * M.Lerp(min, max, G.universe.Anim()));
        private static long PlayerInteraction => G.player.state.timespan_interacting;

        public static void ClearDestruction() { ClearSingleton("particle_slash_0"); ClearSingleton("particle_slash_1"); }
        public static void CreateDestruction(Vec2 pos) { CreateSlash0(pos); CreateSlash1(pos); }


        public static void CreateCross(Vec2 pos) => Create(pos, "particle_cross", PlayerInteraction, Color.white, MapView.OrderInLayer.Foreground, true, false);
        public static void CreateSlash0(Vec2 pos) => Create(pos, "particle_slash_0", PlayerInteraction, Color.white, MapView.OrderInLayer.Foreground, true, false);
        public static void CreateSlash1(Vec2 pos) => Create(pos, "particle_slash_1", PlayerInteraction, Color.white, MapView.OrderInLayer.Foreground, true, false);


        public static void CreateRipple(Vec2 pos) => Create(pos, "particle_ripple", PlayerInteraction, Color.white, MapView.OrderInLayer.Foreground, true, false);

        public static void CreateDust(Vec2 pos, Color color) => Create(pos, "particle_dust_0", LerpFromAnimNoise(0.25f, 0.75f), color);
        public static void CreateDust(Vec2 pos) => CreateDust(pos, G.theme.ClayColor);

        public static void CreateStarShine(Vec2 pos) => Create(pos, "particle_starshine", LerpFromAnimNoise(0.2f, 0.8f), G.theme.WaterColor);
        public static void CreateExplosion(Vec2 pos) => Create(pos, "particle_explosion", LerpFromAnimNoise(0.3f, 0.7f));

        public static void CreateSteamExplosion(Vec2 pos) => Create(pos, "particle_steam_explosion_1", LerpFromAnimNoise(0.6f, 1.2f));

        private static string[] footsteps = new string[] {
            "particle_steam_explosion_2",
            "particle_steam_explosion_3",
            "particle_steam_explosion_4",
            "particle_steam_explosion_5",
            "particle_steam_explosion_6",
            "particle_steam_explosion_7",
        };

        public static void CreateFootStepCloud(Vec2 pos) => Create(pos, Information.I.RandomOne(footsteps),
            LerpFromAnimNoise(
                1,
                1.2f
             ));
    }

}
