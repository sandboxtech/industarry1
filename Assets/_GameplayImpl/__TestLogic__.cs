

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;

namespace W
{
    public static class __TestLogic__
    {
        public static bool debug = false;
        public static void OnCreate() {

            if (debug) {

                backpack_ b_resource = Item.Of<backpack>() as backpack_;
                b_resource.SetInventoryCapacity(35);
                List<Item> resource_list = new List<Item> {
                Item.Of<berry>().SetQuantity(999),
                Item.Of<stone>().SetQuantity(999),
                Item.Of<branch>().SetQuantity(999),

                Item.Of<stick>().SetQuantity(999),
                Item.Of<wood>().SetQuantity(999),
                Item.Of<fiber>().SetQuantity(999),
                Item.Of<clay>().SetQuantity(999),
                Item.Of<metal_ore>().SetQuantity(999),
                Item.Of<wheat>().SetQuantity(999),

                Item.Of<metal_ore_powder>().SetQuantity(999),
                Item.Of<clay_pottery>().SetQuantity(999),
                Item.Of<claybaked_tile>().SetQuantity(999),
                Item.Of<claybaked_brick>().SetQuantity(999),
                Item.Of<wood_tile>().SetQuantity(999),
                Item.Of<wood_brick>().SetQuantity(999),
                Item.Of<stone_tile>().SetQuantity(999),
                Item.Of<stone_brick>().SetQuantity(999),

                Item.Of<metal_product>().SetQuantity(999),
                Item.Of<metal>().SetQuantity(999),
                Item.Of<heat_energy>().SetQuantity(999),
                Item.Of<kinematic_energy>().SetQuantity(999),

                Item.Of<claybaked_pottery>().SetQuantity(999),

                Item.Of<copper_coin>().SetQuantity(999),
                Item.Of<silver_coin>().SetQuantity(999),
                Item.Of<gold_coin>().SetQuantity(999),
            };
                foreach (var item in resource_list) {
                    bool r = b_resource.list.TryAdd(item);
                    A.Assert(r);
                }
                G.player.hand.TryAdd(b_resource);



                backpack_ b_building = Item.Of<backpack>() as backpack_;
                b_building.SetInventoryCapacity(35);
                List<Item> building_list = new List<Item> {
                // Item.Of<axe>().SetQuantity(99),
                // Item.Of<pickaxe>().SetQuantity(99),
                Item.Of<spade>().SetQuantity(99),
                Item.Of<hammer>().SetQuantity(99),
                // Item.Of<hoe>().SetQuantity(99),
                // Item.Of<scythe>().SetQuantity(99),

                Item.Of<craftingplace>().SetQuantity(99),
                Item.Of<hearth>().SetQuantity(99),
                Item.Of<hut_fur>().SetQuantity(99),
                Item.Of<hut_stick>().SetQuantity(99),
                Item.Of<hut_fiber>().SetQuantity(99),

                Item.Of<trading_post>().SetQuantity(99),

                Item.Of<clayfiber_workshop_clay>().SetQuantity(99),
                Item.Of<clayfiber_residence>().SetQuantity(99),
                Item.Of<clayfiber_workshop_fiber>().SetQuantity(99),

                Item.Of<wood_residence>().SetQuantity(99),
                Item.Of<wood_library>().SetQuantity(99),
                Item.Of<wood_workshop>().SetQuantity(99),
                Item.Of<wood_warehouse>().SetQuantity(99),

                Item.Of<brick_residence>().SetQuantity(99),
                Item.Of<brick_library>().SetQuantity(99),
                Item.Of<brick_workshop>().SetQuantity(99),
                Item.Of<brick_warehouse>().SetQuantity(99),

                Item.Of<fermentation_pot>().SetQuantity(99),
                Item.Of<clay_oven>().SetQuantity(99),
                Item.Of<clay_klin>().SetQuantity(99),
                Item.Of<distillation_facility>().SetQuantity(99),
                Item.Of<mortar>().SetQuantity(99),
                Item.Of<bloomary>().SetQuantity(99),
                Item.Of<furnance_blast>().SetQuantity(99),
                Item.Of<anvil>().SetQuantity(99),

                Item.Of<wind_turbine>().SetQuantity(99),
                Item.Of<windmill>().SetQuantity(99),
                Item.Of<machine_air_separator>().SetQuantity(99),
                Item.Of<machine_chemicals>().SetQuantity(99),

                Item.Of<mine>().SetQuantity(99),
                Item.Of<crate>().SetQuantity(99),

                //Item.Of<well>().SetQuantity(99),
                //Item.Of<millstone>().SetQuantity(99),
                //Item.Of<fireplace>().SetQuantity(99),
                //Item.Of<cauldron>().SetQuantity(99),
                //Item.Of<cellar>().SetQuantity(99),
                //Item.Of<mechanical_millstone>().SetQuantity(99),
                //Item.Of<windmill>().SetQuantity(99),
                //Item.Of<workbench_cooking>().SetQuantity(99),

                //Item.Of<clay_oven>().SetQuantity(99),
                //Item.Of<clay_charcoal_klin>().SetQuantity(99),
                //Item.Of<clay_klin>().SetQuantity(99),
                //Item.Of<fermentation_pot>().SetQuantity(99),
                //Item.Of<distillation_facility>().SetQuantity(99),

            };

                foreach (var item in building_list) {
                    b_building.list.TryAdd(item);
                }
                G.player.hand.TryAdd(b_building);
            }

        }
    }
}
