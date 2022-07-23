
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{
    // product
    [hide] public interface fauna_product : material { }
    [cn("脂")] public interface fat : fauna_product { }
    [cn("肉")] public interface meat : fauna_product { }
    [cn("奶")] public interface milk : fauna_product { }

    [recipe(false, typeof(egg))]
    [cn("蛋")] public interface egg : fauna_product { }

    [cn("蛋壳")] public interface egg_shell : fauna_product { }
    [cn("牙")] public interface teeth : fauna_product { }
    [cn("爪")] public interface claw : fauna_product { }
    [cn("贝壳")] public interface shell : fauna_product { }
    [cn("骨")] public interface bone : fauna_product { }
    [cn("头骨")] public interface skull : fauna_product { }
    [cn("脑")] public interface brain : fauna_product { }
    [cn("鳞")] public interface flake : fauna_product { }
    [cn("羽")] public interface feather : fauna_product { }
    [cn("毛")] public interface hair : fauna_product { }
    [cn("皮肤")] public interface skin : fauna_product { }
    [cn("皮革")] public interface fur : hair, skin { }

    [recipe(false, typeof(honey))]
    [cn("蜂蜜")] public interface honey { }
    public class honey_ : EdibleItem
    {
        public override long Satiety => 1;
        public override long Energy => Satiety * 10;
    }

    // attribute
    [hide] public interface for_fauna_product : fauna { } // => 材料
    [hide] public interface for_fat : for_lipid { }
    [hide] public interface for_meat : for_protein { }
    [hide] public interface for_milk : for_protein { }
    [hide] public interface for_egg : for_protein { }
    [hide] public interface for_teeth : for_fauna_product { }
    [hide] public interface for_claw : for_fauna_product { }
    [hide] public interface for_shell : for_fauna_product { }
    [hide] public interface for_bone : for_fauna_product { }
    [hide] public interface for_flake : for_fauna_product { }
    [hide] public interface for_feather : for_fauna_product { }
    [hide] public interface for_hair : for_fauna_product, for_cellouse { }
    [hide] public interface for_skin : for_fauna_product { }
    [hide] public interface for_fur : for_fauna_product, for_skin, for_hair { }



    // classification

    [cn("脊椎")] [hide] public interface chordate : fauna { }
    [cn("无脊椎")] [hide] public interface nonchordate : fauna { }

    [cn("哺乳")] public interface mammals : chordate { }
    [cn("灵长")] public interface primate : mammals { }
    [cn("啮齿")] public interface rodent : mammals { }

    [cn("鸟")] public interface birds : chordate { }
    [cn("爬行")] public interface reptile : chordate { }
    [cn("两栖")] public interface amphibian : chordate { }


    [cn("鱼")] public interface fish : chordate { }



    [cn("甲壳")] public interface crustaceans : nonchordate { }
    [cn("棘皮")] public interface starfish_like : nonchordate { }
    [cn("腔肠")] public interface jelly_like : nonchordate { }
    [cn("软体")] public interface snail_like : nonchordate { }

    [cn("恐龙")] public interface dinosaur : reptile { }

    [hide] public interface fantastic_fauna : fantastic, fauna { }




    // instances


    [cn("人")] public interface human : primate { }
    [cn("猴")] public interface monkey : primate { }
    [cn("猿")] public interface ape : primate { }
    [cn("猩猩")] public interface chimpanzee : primate { }
    [cn("狒狒")] public interface baboon : primate { }

    [cn("河狸")] public interface beaver : rodent { }
    [cn("松鼠")] public interface squirrel : rodent { }
    [cn("老鼠")] public interface rat : rodent { }
    [cn("毛丝鼠")] public interface chinchilla : rodent { }
    [cn("土拔鼠")] public interface marmot : rodent { }
    [cn("仓鼠")] public interface hamster : rodent { }
    [cn("旅鼠")] public interface lemming : rodent { }


    [cn("兔")] public interface rabbit : mammals { }

    [cn("狐")] public interface fox : mammals { }
    [cn("犬")] public interface dog : mammals { }
    [cn("狼")] public interface wolf : mammals { }

    [cn("猫")] public interface cat : mammals { }
    [cn("豹")] public interface leopard : mammals { }
    [cn("薮猫")] public interface serval : mammals { }
    [cn("猞猁")] public interface lynx : mammals { }
    [cn("豹猫")] public interface ocelot : mammals { }


    [cn("熊")] public interface bear : mammals { }
    [cn("猫熊")] public interface panda : mammals { }


    [cn("黄鼠狼")] public interface weasel : mammals { }
    [cn("白鼬1")] public interface ferret : mammals { }
    [cn("白鼬2")] public interface stoat : mammals { }
    [cn("水獭")] public interface otter : mammals { }
    [cn("蜜獾")] public interface badger : mammals { }
    [cn("貂")] public interface mink : mammals { }
    [cn("紫貂")] public interface sable : mammals { }

    [cn("臭鼬")] public interface skunk : mammals { }
    [cn("海象")] public interface walrus : mammals { }
    [cn("海狮")] public interface sealion : mammals { }
    [cn("海豹")] public interface seal : mammals { }
    [cn("浣熊")] public interface raccoon : mammals { }
    [cn("鬣狗")] public interface hyena : mammals { }
    [cn("獴")] public interface maroon : mammals { }
    [cn("灵猫")] public interface civet : mammals { }

    [cn("猪")] public interface pig : mammals { }
    [cn("马")] public interface horse : mammals { }
    [cn("貘")] public interface tapir : mammals { }
    [cn("犀")] public interface rhinoceros : mammals { }
    [cn("骆驼")] public interface camel : mammals { }
    [cn("鹿")] public interface deer : mammals { }
    [cn("麝")] public interface muskdeer : mammals { }
    [cn("羚羊")] public interface antelope : mammals { }
    [cn("山羊")] public interface goat : mammals { }
    [cn("绵羊")] public interface sheep : mammals { }
    [cn("海牛")] public interface manatee : mammals { }
    [cn("奶牛")] public interface cow : mammals { }
    [cn("水牛")] public interface buffalo : mammals { }
    [cn("牦牛")] public interface yak : mammals { }

    [cn("鲸")] public interface whale : mammals { }
    [cn("虎鲸")] public interface killerwhale : mammals { }
    [cn("豚")] public interface porpoise : mammals { }

    [cn("穿山甲")] public interface pangolin : mammals { }
    [cn("象")] public interface elephant : mammals { }
    [cn("袋鼠")] public interface kangaroo : mammals { }
    [cn("树袋鼠")] public interface koala : mammals { }
    [cn("食蚁兽")] public interface anteater : mammals { }
    [cn("刺猬")] public interface hedgehog : mammals { }
    [cn("鸭嘴兽")] public interface platypus : mammals { }
    [cn("蝙蝠")] public interface bat : mammals { }
    [cn("犰狳")] public interface armadillo : mammals { }



    [cn("始祖鸟")] public interface archaeopteryx : birds { }
    [cn("鸵鸟")] public interface ostrich : birds { }
    [cn("企鹅")] public interface penguin : birds { }
    [cn("信天翁")] public interface albatross : birds { }
    [cn("海燕")] public interface petrel : birds { }
    [cn("鸬鹚")] public interface cormorant : birds { }
    [cn("鹈鹕")] public interface pelican : birds { }
    [cn("白鹳")] public interface stork : birds { }
    [cn("火烈鸟")] public interface flamingo : birds { }

    [cn("鸭")] public interface duck : birds { }
    [cn("鹅")] public interface goose : birds { }
    [cn("雁")] public interface wildgoose : birds { }
    [cn("鹰")] public interface hawk : birds { }
    [cn("鸡")] public interface chicken : birds { }
    [cn("孔雀")] public interface peacock : birds { }
    [cn("鹤")] public interface crane : birds { }
    [cn("鸽")] public interface pigeon : birds { }
    [cn("鹦鹉")] public interface parrot : birds { }
    [cn("杜鹃")] public interface cuckoo : birds { }
    [cn("夜莺")] public interface nightingale : birds { }
    [cn("夜鹰")] public interface nighthawk : birds { }
    [cn("雀")] public interface sparrow : birds { }
    [cn("燕")] public interface swallow : birds { }
    [cn("鸮")] public interface owl : birds { }





    [cn("龟")] public interface turtle : reptile { }
    [cn("鳄")] public interface crocodile : reptile { }
    [cn("蛇")] public interface snake : reptile { }
    [cn("蜥蜴")] public interface lizard : reptile { }


    [cn("蛇颈龙")] public interface plesiosaurus : dinosaur { }
    [cn("鱼龙")] public interface ichthyosaur : dinosaur { }
    [cn("三角龙")] public interface triceratops : dinosaur { }
    [cn("剑龙")] public interface stegosaurus : dinosaur { }
    [cn("鸭嘴龙")] public interface hadrosaurs : dinosaur { }
    [cn("暴龙")] public interface tyrannosaurus : dinosaur { }
    [cn("翼龙")] public interface pterosaur : dinosaur { }




    [cn("蛙")] public interface frog : amphibian { }
    [cn("蛤")] public interface toad : amphibian { }
    [cn("蝾螈")] public interface salamander : amphibian { }




    [cn("鲨")] public interface shark : fish { }
    [cn("鳐")] public interface skate : fish { }
    [cn("灯笼鱼")] public interface lanternfish : fish { }
    [cn("鳕鱼")] public interface cod : fish { }
    [cn("鲈鱼")] public interface perch : fish { }
    [cn("鲱鱼")] public interface herring : fish { }
    [cn("鲟鱼")] public interface sturgeon : fish { }
    [cn("鳝鱼")] public interface eel : fish { }
    [cn("安康鱼")] public interface anglerfish : fish { }
    [cn("蛞蝓鱼")] public interface amphioxus : fish { }
    [cn("鲤鱼")] public interface carp : fish { }
    [cn("草鱼")] public interface grass_carp : fish { }
    [cn("三文鱼")] public interface salmon : fish { }


    [cn("海星")] public interface sea_star : starfish_like { }
    [cn("海胆")] public interface sea_urchin : starfish_like { }

    [cn("海葵")] public interface sea_anemone : jelly_like { }
    [cn("海蜇")] public interface jellyfish : jelly_like { }
    [cn("珊瑚")] public interface coral : jelly_like { }
    [cn("蚯蚓")] public interface earthworm : jelly_like { }
    [cn("蚂蟥")] public interface leech : jelly_like { }


    [cn("螺/蜗牛")] public interface snail : snail_like { }
    [cn("蛞蝓")] public interface slug : snail_like { }
    [cn("蛤蜊")] public interface clam : snail_like { }
    [cn("扇贝")] public interface scallop : snail_like { }
    [cn("乌贼")] public interface inkfish : snail_like { }
    [cn("鱿鱼")] public interface squid : snail_like { }
    [cn("章鱼")] public interface octopus : snail_like { }




    [cn("蜘蛛")] public interface spider : crustaceans { }
    [cn("蝎")] public interface scorpion : crustaceans { }
    [cn("海鳖")] public interface sea_turtle : crustaceans { }
    [cn("蜉蝣")] public interface mayfly : crustaceans { }
    [cn("蜻蜓")] public interface dragonfly : crustaceans { }
    [cn("蟑螂")] public interface cockroach : crustaceans { }
    [cn("螳螂")] public interface mantis : crustaceans { }
    [cn("蟋蟀/蝼蛄")] public interface cricket : crustaceans { }
    [cn("白蚁")] public interface termite : crustaceans { }
    [cn("竹节虫")] public interface stick_insect  : crustaceans { }
    [cn("蝗虫")] public interface locust : crustaceans { }
    [cn("甲虫")] public interface beetle : crustaceans { }
    [cn("天牛")] public interface longicorn : crustaceans { }
    [cn("瓢虫")] public interface ladybug : crustaceans { }
    [cn("独角仙")] public interface uang : crustaceans { }
    [cn("飞蛾")] public interface moth : crustaceans { }
    [cn("蝴蝶")] public interface butterfly : crustaceans { }
    [cn("蚊")] public interface mosquito : crustaceans { }
    [cn("苍蝇")] public interface fly : crustaceans { }
    [cn("牛虻")] public interface gadfly : crustaceans { }

    [cn("虱")] public interface louse : crustaceans { }
    [cn("跳蚤")] public interface flea : crustaceans { }
    [cn("蜜蜂")] public interface bee : crustaceans { }
    [cn("黄蜂")] public interface wasp : crustaceans { }

    [cn("蚁")] public interface ant : crustaceans { }
    [cn("螃蟹")] public interface crab : crustaceans { }
    [cn("龙虾")] public interface lobster : crustaceans { }
    [cn("虾")] public interface shrimp : crustaceans { }

    // 鲛 蛞蝓




    [cn("魄罗")] public interface poro : fantastic_fauna { }
    [cn("峡谷蟹")] public interface rift_scuttler : fantastic_fauna { }
    [cn("龙猫")] public interface totoro : fantastic_fauna { }
    [cn("利维坦")] public interface leviathan : fantastic_fauna { }
    [cn("史莱姆")] public interface slime : fantastic_fauna { }
    [cn("独角兽")] public interface unicorn : fantastic_fauna { }
    [cn("天马")] public interface pegasus : fantastic_fauna { }
    [cn("地精")] public interface goblin : fantastic_fauna { }
    [cn("侏儒")] public interface gnome : fantastic_fauna { }
    [cn("矮人")] public interface dwarf : fantastic_fauna { }
    [cn("兽人")] public interface orc : fantastic_fauna { }
    [cn("精灵")] public interface elf : fantastic_fauna { }

    [cn("雪人")] public interface yeti : fantastic_fauna { }
    [cn("巨魔")] public interface troll : fantastic_fauna { }
    [cn("九头蛇")] public interface hydra : fantastic_fauna { }
    [cn("天使")] public interface angel : fantastic_fauna { }
    [cn("恶魔")] public interface devil : fantastic_fauna { }
    [cn("凤凰")] public interface pheonix : fantastic_fauna { }
    [cn("西方龙")] public interface dragon : fantastic_fauna { }
    [cn("东方龙")] public interface loong : fantastic_fauna { }
    [cn("亡灵")] public interface undead : fantastic_fauna { }
    [cn("吸血鬼")] public interface vampire : fantastic_fauna { }
    [cn("僵尸")] public interface zombie : fantastic_fauna { }
    [cn("骷髅")] public interface skeleton : fantastic_fauna { }
    [cn("狼人")] public interface werewolf : fantastic_fauna { }
    [cn("狮鹫")] public interface hippogriff : fantastic_fauna { }

    [cn("寄生虫")] public interface parasite : fantastic_fauna { }
    [cn("狮蝎")] public interface manticore : fantastic_fauna { }

    [cn("水怪")] public interface nessy : fantastic_fauna { }
    [cn("牛头人")] public interface minotaur : fantastic_fauna { }

    [cn("狮身人面")] public interface sphinix : fantastic_fauna { }
    [cn("海妖")] public interface siren : fantastic_fauna { }
    [cn("星灵")] public interface protoss : fantastic_fauna { }
    [cn("鸩")] public interface poisonous_owl : fantastic_fauna { }
    [cn("鲲")] public interface fish_leviathan : fantastic_fauna { }
    [cn("鹏")] public interface bird_leviathan : fantastic_fauna { }
}
