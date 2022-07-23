
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{


    public class Planet : MapTerrain
    {
        private CircularPerlinNoise2D altitude_noise { get; set; } // altitude
        private CircularPerlinNoise2D clay_stone_noise { get; set; } // altitude
        private CircularPerlinNoise2D grassland_noise0 { get; set; } // moisture
        private CircularPerlinNoise2D grassland_noise1 { get; set; } // moisture



        private bool[,] land;
        private bool[,] grassland;

        private bool[,] hill_stone;
        private bool[,] hill_clay_or_sand;

        private int[,] grassland9;



        public override void AfterCreate() {
            base.AfterCreate();

            CreateGround();

            PlaceSea();
            PlaceInterior();
            PlaceHillSurface();
            PlaceExterior();
        }



        private void CreateGround() {
            uint hashcode = G.map.hashcode;

            Vec2 size = G.map.size;
            altitude_noise = new CircularPerlinNoise2D(size / 7, size, H.Hashed(ref hashcode));
            clay_stone_noise = new CircularPerlinNoise2D(size / 11, size, H.Hashed(ref hashcode));
            grassland_noise0 = new CircularPerlinNoise2D(size / 8, size, H.Hashed(ref hashcode));
            grassland_noise1 = new CircularPerlinNoise2D(size / 13, size, H.Hashed(ref hashcode));

            int width = size.x;
            int height = size.y;

            room = new bool[width, height];
            land = new bool[width, height];
            grassland = new bool[width, height];


            // 计算两轮迭代
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    Vec2 pos = new Vec2(i, j);
                    land[i, j] = _IsLand(pos);
                    grassland[i, j] = M.Lerp(grassland_noise0.On(pos), grassland_noise1.On(pos), 0.3f) > -0.06f;
                }
            }
            CellularAutomataUtility.GameOfTerrain_IterateBackAndForth(land, cellular_buffer, width, height);
            CellularAutomataUtility.GameOfTerrain_IterateBackAndForth(grassland, cellular_buffer, width, height);

            hill_stone = new bool[width, height];
            hill_clay_or_sand = new bool[width, height];

            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    Vec2 pos = new Vec2(i, j);

                    land[i, j] = land[i, j] && rect[i, j];

                    bool far = distance_to_portal[i, j] >= 7;

                    grassland[i, j] = land[i, j] && grassland[i, j];

                    if (!far) continue;

                    float noise = clay_stone_noise.On(pos);
                    if (noise > 0.35f) {
                        hill_stone[i, j] = true;

                    } else if (noise < -0.3f) {
                        hill_clay_or_sand[i, j] = true;
                    }
                }
            }

            grassland9 = new int[width, height];
            for (int j = Margin; j < height - Margin; j++) {
                for (int i = Margin; i < width - Margin; i++) {
                    Vec2 pos = new Vec2(i, j);
                    grassland9[i, j] = TileUtility.Count9(pos, (Vec2 p) => grassland[p.x, p.y] ? 1 : 0);
                }
            }
        }

        private bool _IsLand(Vec2 pos) {

            Map map = G.map;
            int width = map.Width;
            int height = map.Height;
            int min = M.Min(width, height);
            int i = pos.x;
            int j = pos.y;

            int minDistace = M.Min(M.Min(i, width - 1 - i), M.Min(j, height - 1 - j));

            if (minDistace < Margin) return true;
            else if (minDistace > min - 1 - Margin) return false;


            float t = minDistace / (width / 2f); // 距离边界的距离 1在中心，0在边界
            A.Assert(t >= 0 && t <= 1);

            float t2 = -4 * (t - 0.5f) * (t - 0.5f) + 1; // -2 * (t - 0.5f) * (t - 0.5f) + 1;

            float altitude = altitude_noise.On(pos);
            altitude = M.Lerp(t > 0.5f ? 1 : -1, altitude, t2);

            return altitude > -0.04f;
        }


        private void PlaceInterior() {
            Vec2 portal = Portal;
            if (Map.PlanetPosotion == planet_.HomePlanetPosition) {
                //Vec2 size = new Vec2(7, 7);
                //Vec2 pos = new Vec2(-size.x / 2, size.y / 2) + portal;

                //PrefabBuilding.MakeWoodRoom(pos + new Vec2(-6, 0), size, Vec2.right);
                //PrefabBuilding.MakeWoodRoom(pos + new Vec2(0, 0), size, Vec2.down);
                //PrefabBuilding.MakeWoodRoom(pos + new Vec2(6, 0), size, Vec2.left);

                //PrefabBuilding.MakeBrickRoom(pos + new Vec2(-6, -10), size, Vec2.right);
                //PrefabBuilding.MakeStoneRoom(pos + new Vec2(6, -10), size, Vec2.left);

                for (int i = 0; i < 5; i++) {
                    for (int j = 0; j < 5; j++) {
                        Vec2 pos = portal + new Vec2(i, j + 3);
                        G.terrain.floor[pos] = GroundType.grassland;
                        G.terrain.bedrock[pos] = GroundType.bedrock;

                        G.map[pos] = Item.Of<berry_plant>();
                    }
                }
                G.map[portal + new Vec2(1, 5 + 3)] = Item.Of<hut_fiber>();

                backpack_ c = Item.Of<backpack>() as backpack_;
                c.SetInventoryCapacity(2 * 7);
                c.list.TryAdd(Item.Of<copper_coin>().SetQuantity(300));
                c.list.TryAdd(Item.Of<hut_fiber>().SetQuantity(1));
                c.list.TryAdd(Item.Of<hut_stick>().SetQuantity(1));
                c.list.TryAdd(Item.Of<mine>().SetQuantity(1));
                c.list.TryAdd(Item.Of<hammer>());
                c.list.TryAdd(Item.Of<spade>());
                c.list.TryAdd(Item.Of<wood_brick>().SetQuantity(21));
                c.list.TryAdd(Item.Of<wood_tile>().SetQuantity(9));
                c.list.TryAdd(Item.Of<wood_door>().SetQuantity(1));
                c.list.TryAdd(Item.Of<book_travel>().SetQuantity(1));
                c.list.TryAdd(Item.Of<book_survival>().SetQuantity(1));

                G.map[portal + new Vec2(4, 5 + 3)] = c;

                G.map[portal + new Vec2(-2, -5)] = Item.Of<spaceship1>();
            } else if (G.map.type == MapType.planet) {
                // G.map[portal + new Vec2(-2, 3)] = Item.Of<spaceship1>();

                PlaceRandomStuffNearSpaceship(portal + new Vec2(-2, 3));
                PlaceTreasureAroundMap();
            }
        }
        private void PlaceRandomStuffNearSpaceship(Vec2 center) {
            uint hash = G.map.hashcode;

            Type[] stuffs = RandomStuffs[H.Hashed(ref hash) % RandomStuffs.Length];

            int repeat = (int)(H.Hashed(ref hash) % 4) + 2;
            for (int j = 0; j < repeat; j++) {

                int horizontal = (int)(H.Hashed(ref hash) % 15 + 4) * (H.Hashed(ref hash) % 2 == 0 ? 1 : -1) - 1;
                int vertical = (int)(H.Hashed(ref hash) % 15) - 14;

                for (int i = 0; i < stuffs.Length; i++) {
                    if (H.Hashed(ref hash) % 12 == 0) return;
                    if (H.Hashed(ref hash) % 6 == 0) continue;

                    int horizontalOffset = (int)(H.Hashed(ref hash) % 1) + 3;
                    int verticalOffset = (int)(H.Hashed(ref hash) % 1) + 3;
                    horizontalOffset *= (H.Hashed(ref hash) % 2 == 0 ? 1 : -1);
                    verticalOffset *= (H.Hashed(ref hash) % 2 == 0 ? 1 : -1);

                    Type stuffType = stuffs[i % stuffs.Length];
                    Item stuff = Item.Of(stuffType);
                    Vec2 place = center + new Vec2(horizontal + horizontalOffset, vertical + verticalOffset);
                    if (G.map.CanPlace(place, stuff.Size)) {
                        G.map[place] = stuff;
                        // Debug.Log($"{stuffType.Name}");
                    } else {
                        break;
                    }
                }
            }
        }

        private void PlaceTreasureAroundMap() {
            const int chest_max = 3;
            int chest_count = 0;
            const int iteration_max = 12;
            uint hash = G.map.hashcode;

            int radius = M.Min(G.map.size.x, G.map.size.y) / 3;

            for (int i = 0; i < iteration_max; i++) {
                if (chest_count >= chest_max) break;

                int horizontal = (int)(H.Hashed(ref hash) % radius + 4) * (H.Hashed(ref hash) % 2 == 0 ? 1 : -1) - 1;
                int vertical = (int)(H.Hashed(ref hash) % radius + 4) * (H.Hashed(ref hash) % 2 == 0 ? 1 : -1) - 1;
                Vec2 pos = new Vec2(horizontal, vertical);

                if (G.map.CanPlace(pos, Vec2.one)) {
                    chest_count++;


                    crate_ crate = Item.Of<crate>() as crate_;
                    crate.SetInventoryCapacity(14);

                    uint contentCount = H.Hashed(ref hash) % 5 + 1;
                    for (uint j = 0; j < contentCount; j++) {
                        (Type, long)[] contentList = H.Hashed(ref hash) % 5 == 0 ? chestContent : chestScaceContent;

                        (Type, long) content = chestContent[H.Hashed(ref hash) % chestContent.Length];
                        long quantity = (H.Hashed(ref hash) % content.Item2 + 1) * (H.Hashed(ref hash) % content.Item2 + 1) / (content.Item2 * content.Item2);
                        crate.list.TryAdd(Item.Of(content.Item1).SetQuantity(quantity));
                    }

                    G.map[pos] = crate;
                }
                // Vec2 randomPos = G.map.Center + radius * 1;
            }
        }

        private static (Type, long)[] chestContent = new (Type, long)[] {
            (typeof(copper_coin), 10),
            (typeof(silver_coin), 3),
            (typeof(gold_coin), 1),
            (typeof(copper_coin), 20),
            (typeof(silver_coin), 6),
            (typeof(gold_coin), 2),
            (typeof(copper_coin), 30),
            (typeof(silver_coin), 9),
            (typeof(gold_coin), 3),

            (typeof(wood_tile), 30),
            (typeof(wood_brick), 30),
            (typeof(stone_tile), 30),
            (typeof(stone_brick), 30),
            (typeof(claybaked_tile), 30),
            (typeof(claybaked_brick), 30),

            (typeof(honey), 300),
            (typeof(salt), 100),
            (typeof(sugar), 100),

            (typeof(circuitboard), 100),
            (typeof(chip), 100),
            (typeof(sugar), 100),
        };

        private static (Type, long)[] chestScaceContent = new (Type, long)[] {
            (typeof(claw), 30),
            (typeof(feather), 30),
            (typeof(fur), 10),
            (typeof(bone), 10),
            (typeof(skull), 3),
            (typeof(brain), 1),

            (typeof(tablet), 1),
            (typeof(dynamite), 1),
            (typeof(hard_disc), 1),
            (typeof(submachinegun), 1),
            (typeof(hourglass), 1),
            (typeof(computer), 1),
            (typeof(electric_motor), 1),
            (typeof(appliance), 1),
        };


        private static Type[][] RandomStuffs = new Type[][] {
            new Type[] {
                typeof(hut_fiber),
                typeof(hut_fiber),
                typeof(trading_post),
                typeof(hut_fiber),
            },
            new Type[] {
                typeof(hut_fiber),
                typeof(hut_stick),
                typeof(mine),
                typeof(mine),
            },
            new Type[] {
                typeof(clayfiber_residence),
                typeof(clayfiber_workshop_clay),
                typeof(clayfiber_workshop_fiber),
                typeof(trading_post),
            },
            new Type[] {
                typeof(wood_residence),
                typeof(wood_library),
                typeof(wood_warehouse),
                typeof(wood_workshop),
            },
            new Type[] {
                typeof(brick_residence),
                typeof(brick_workshop),
                typeof(brick_warehouse),
                typeof(brick_library),
            },
        };

        private void PlaceSea() {
            Vec2 size = G.map.size;

            GroundType bedrock_type = GroundType.bedrock;
            GroundType sea_type = GroundType.sea;
            for (int i = 0; i < size.x - 0; i++) {
                for (int j = 0; j < size.y - 0; j++) {
                    if (land[i, j]) {
                        bedrock[i, j] = bedrock_type;
                    }
                    else {
                        bedrock[i, j] = sea_type;
                    }
                }
            }
        }

        private void PlaceHillSurface() {
            Vec2 size = G.map.size;

            GroundType grass_type = G.map.subtype == MapSubType.sand ? GroundType.none : GroundType.grassland;
            GroundType stone0_type = G.map.subtype == MapSubType.sand ? GroundType.hill_sand : (G.map.subtype == MapSubType.stone ? GroundType.hill_stone : GroundType.hill_clay);
            GroundType stone1_type = G.map.subtype == MapSubType.stone ? GroundType.hill_sand : GroundType.hill_stone;

            for (int i = Margin; i < size.x - Margin; i++) {
                for (int j = Margin; j < size.y - Margin; j++) {

                    if (!G.map.CanEnter(new Vec2(i, j))) {
                        continue;
                    }

                    if (grassland[i, j] && G.map.subtype != MapSubType.sand) {
                        floor[i, j] = grass_type;
                    } else if (hill_stone[i, j]) {
                        floor[i, j] = stone0_type;
                    } else if (hill_clay_or_sand[i, j]) {
                        floor[i, j] = stone1_type;
                    }
                }
            }
        }

        private void PlaceExterior() {

            Map map = G.map;

            Vec2 size = G.map.size;

            // map[Portal.x - 2, Portal.y + 2] = Item.Of<spaceship1>();

            Type specialBush = MapTerrain.SpecialBushOf(G.map.hashcode); // G.terrain.SpecialBush;
            A.Assert(specialBush != null);
            Type specialTree = MapTerrain.SpecialTreeOf(G.map.hashcode); // G.terrain.SpecialTree;
            A.Assert(specialTree != null);

            for (int i = Margin; i < size.x - Margin; i++) {
                for (int j = Margin; j < size.y - Margin; j++) {

                    if (map[i, j] != null) continue;

                    if (room[i, j]) continue;
                    if (distance_to_portal[i, j] <= 6) continue;

                    if (!land[i, j]) continue;
                    if (hill_stone[i, j] || hill_clay_or_sand[i, j]) continue;

                    uint hash = hashcodes[i, j];

                    if (grassland9[i, j] == 9 && H.Hashed(ref hash) % Prime._11 == 0) {
                        Item flora = map[i, j] = Item.Of(specialBush);
                        //if (!(flora is _PlantLike)) {
                        //    Debug.LogWarning(specialFlora.Name);
                        //}
                        //if (flora.IdleData == null) {
                        //    Debug.LogWarning(flora.GetType().Name);
                        //}
                        flora.IdleData?.RandomizeAllProgress(hash);
                    }
                    else if (grassland[i, j] && H.Hashed(ref hash) % Prime._29 == 0) {
                        Item tree = map[i, j] = Item.Of(specialTree);
                        tree.IdleData?.RandomizeAllProgress(hash);
                    }

                }
            }
        }
    }
}
