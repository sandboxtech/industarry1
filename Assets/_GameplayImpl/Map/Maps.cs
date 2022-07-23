
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{
    public static class Maps
    {
        public static void GotoInitialMap() {
            // GotoStarSystem();
            GotoPlanet(planet_.HomePlanetPosition);
            // GotoSpaceMap();
        }

        // private const int world_width_ = 3;
        // private const int world_height = 3;
        public static void GotoPlanet(Vec2 pos) {
            // pos.x %= world_width_; if (pos.x < 0) pos.x += world_width_;
            // pos.y %= world_height; if (pos.x < 0) pos.y += world_height;
            Map.PlanetPosotion = pos;
            G.universe.planet_pos = pos;
            GameEntry.I.PlayerEnterMap(PlanetKeyOf(Map.PlanetPosotion), MapType.planet);
        }
        public static string PlanetKeyOf(Vec2 pos) => $"planet_{pos}";

        //public static void GotoSpaceMap() {
        //    GameEntry.I.PlayerEnterMap("space", MapType.spaceship); // create Map
        //}

        public static void GotoStarSystem() {
            GameEntry.I.PlayerEnterMap($"star_system", MapType.star_system);
        }
    }
}
