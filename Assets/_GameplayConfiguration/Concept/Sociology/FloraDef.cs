
namespace W
{




    [hide] public interface flora_product : organics { }
    [cn("根")] public interface root : flora_product { }
    [cn("茎")] public interface stem : flora_product { }


    [impl(typeof(leaf_))]
    [recipe(false, typeof(leaf))]
    [cn("叶片")] public interface leaf : flora_product { }
    public class leaf_ : Item
    {
        public override Color4 ContentColor => G.theme.FloraColor;
    }


    [cn("花粉")] public interface pollen : flora_product { }
    [cn("花朵")] public interface flower : flora_product { }
    [cn("种子")] public interface seed : flora_product { }
    [cn("藤蔓")] public interface vine : flora_product { }
    [cn("水果")] public interface fruit : flora_product { }



    [recipe(false, typeof(berry))]
    [impl(typeof(berry_))]
    [cn("浆果")] public interface berry : fruit { }
    public class berry_ : EdibleItem
    {
        public override long Satiety => 1;
        public override long Energy => Satiety * 1;


        public override Color4 ContentColor => G.theme.FloraColor;
        public override Color4 HighlightColor => G.theme.FruitColor;
        public override string Highlight => DefaultContainer; // 这里没写错
    }


    [cn("瓜果")] public interface melon : fruit { }



    // 效用    

    [hide] public interface for_vine : flora { }
    [hide] public interface for_wood : flora { }
    [hide] public interface herbal : flora { } // => 纤维.田上
    [hide] public interface woody : flora, for_wood { } // => 木材.林地上
    [cn("芳香")] public interface fragrant { }
    [cn("观赏")] public interface decorative { }

    // 分类
    [hide] public interface for_flower : flora, fragrant, decorative { }
    [hide] public interface for_fruit : flora, nutritious, for_monosaccharide { }
    [hide] public interface fruity : flora, for_fruit { }

    // 分类
    [hide] public interface for_perfume : flora, herbal, fragrant, digestable { }
    [hide] public interface for_melon : flora, herbal, fruity { }
    [hide] public interface for_berry : flora, woody, fruity { }
    [hide] public interface drupe : flora, woody, fruity { }
    [hide] public interface palm : flora, woody, fruity { }


    // 分类
    [hide] public interface herbal_flower : herbal, for_flower { }
    [hide] public interface woody_flower : woody, for_flower { }

    [cn("蔬菜")] public interface vegetable : herbal, digestable { } // itself product
    [cn("主食")] public interface staple : herbal, digestable { }
    [cn("禾谷")] public interface for_cereal : staple, for_starch { }
    [cn("薯块")] public interface for_tubers : staple, for_starch { }
    [cn("豆")] public interface for_bean : staple, for_protein { }
    [cn("坚果")] public interface for_nut : staple, for_lipid { }

    [hide] public interface fantastic_flora : fantastic, flora { }






    // sedless

    [hide] public interface seedless : flora, digestable { }
    [cn("藻")] public interface alga : seedless { }
    [cn("苔藓")] public interface bryophyta : seedless { }
    [cn("蕨")] public interface fern : seedless { }

    [cn("海藻")] public interface algae : alga { }
    [cn("紫菜")] public interface laver : alga { }
    [cn("海带")] public interface kelp : alga { }



    [cn("玉米")] public interface maize : for_cereal { }
    [cn("小米")] public interface millet : for_cereal { }
    [cn("小麦")] public interface wheat : for_cereal { }
    [cn("荞麦")] public interface buckwheat : for_cereal { }
    [cn("大麦")] public interface barley : for_cereal { }
    [cn("燕麦")] public interface oat : for_cereal { }
    [cn("大米")] public interface rice : for_cereal { }
    [cn("高粱")] public interface sorghum : for_cereal { }

    // tubers
    [cn("香芋")] public interface taro : for_tubers { }
    [cn("甘薯")] public interface yam : for_tubers { }
    [cn("木薯")] public interface cassava : for_tubers { }
    [cn("土豆")] public interface potato : for_tubers { }
    [cn("红薯")] public interface sweet_potato : for_tubers { }

    // bean
    [cn("红豆")] public interface ormosia : for_bean { }
    [cn("花生")] public interface peanut : for_bean, for_lipid { }
    [cn("黄豆")] public interface soybean : for_bean, for_lipid { }
    [cn("豌豆")] public interface pea : for_bean { }
    [cn("豇豆")] public interface vigna : for_bean { }
    [cn("蚕豆")] public interface broadbean : for_bean { }
    [cn("扁豆")] public interface hyacinthbean : for_bean { }
    [cn("刀豆")] public interface swordbean : for_bean { }

    // nut
    [cn("胡桃")] public interface walnut : for_nut { }
    [cn("板栗")] public interface chestnut : for_nut { }
    [cn("榛子")] public interface hazelnut : for_nut { }
    [cn("葵花子")] public interface sunflowerseed : for_nut { }
    [cn("杏仁")] public interface almond : for_nut { }



    //
    [cn("甘蔗")] public interface sugarcane : for_melon, tropical { }
    [cn("香蕉")] public interface banana : for_melon, tropical { }
    [cn("菠萝")] public interface pineapple : for_melon, tropical { }
    [cn("仙人掌")] public interface cactus : for_melon { }
    [cn("冬瓜")] public interface wintermelon : for_melon { }
    [cn("西瓜")] public interface watermelon : for_melon { }
    [cn("蜜瓜")] public interface cantaloupe : for_melon { }
    [cn("香瓜")] public interface muskmelon : for_melon { }
    [cn("雪莲果")] public interface yacon : for_melon { }
    [cn("苦瓜")] public interface bittermelon : for_melon { }
    [cn("黄瓜")] public interface cucumber : for_melon { }
    [cn("南瓜")] public interface pumpkin : for_melon { }
    [cn("丝瓜")] public interface spongem_gourd : for_melon { }

    [cn("葡萄")] public interface grape : for_berry { }
    [cn("猕猴桃")] public interface kiwifruit : for_berry { }
    [cn("月桂果")] public interface bayberry : for_berry { }
    [cn("桑葚")] public interface mulberry : for_berry { }
    [cn("蓝莓")] public interface blueberry : for_berry { }
    [cn("树莓")] public interface raspberry : for_berry { }
    [cn("草莓")] public interface strawberry : for_berry { }
    [cn("杨梅")] public interface waxberry : for_berry { }
    [cn("云梅")] public interface cranberry : for_berry { }

    [cn("苹")] public interface apple : drupe { }
    [cn("杏")] public interface apricot : drupe { }
    [cn("樱")] public interface cherry : drupe { }
    [cn("桃")] public interface peach : drupe { }
    [cn("梅")] public interface plum : drupe { }
    [cn("梨")] public interface pear : drupe { }
    [cn("山楂")] public interface hawthorn : drupe { }
    [cn("榕树果")] public interface banyan : drupe { }
    [cn("面包果")] public interface breadfruit : drupe { }
    [cn("无花果")] public interface fig : drupe { }
    [cn("柿子")] public interface persimmon : drupe { }
    [cn("石榴")] public interface pomegranate : drupe { }
    [cn("荔枝")] public interface leeche : drupe, tropical { }
    [cn("桂圆")] public interface longan : drupe, tropical { }
    [cn("火龙果")] public interface pitaya : drupe, tropical { }
    [cn("菠萝蜜")] public interface jackfruit : drupe, tropical { }
    [cn("榴莲")] public interface durian : drupe, tropical { }

    [cn("红枣")] public interface jujube : palm { }
    [cn("海枣")] public interface date : palm { }
    [cn("椰果")] public interface coconut : palm, tropical { }
    [cn("槟榔")] public interface areca : palm, tropical { }
    [cn("芭蕉")] public interface plantain : palm, tropical { }


    [cn("白萝卜")] public interface turnip : vegetable { }
    [cn("小萝卜")] public interface radish : vegetable { }
    [cn("胡萝卜")] public interface carrot : vegetable { }
    [cn("甜菜")] public interface sugarbeet : vegetable { }
    [cn("芹菜")] public interface celery : vegetable { }
    [cn("马齿苋")] public interface purslane : vegetable { }
    [cn("菠菜")] public interface spinach : vegetable { }
    [cn("通菜")] public interface water_spinach : vegetable { }
    [cn("生菜")] public interface lettuce : vegetable { }
    [cn("红辣椒")] public interface chili : vegetable { }
    [cn("甜椒")] public interface sweet_peper : vegetable { }
    [cn("茄")] public interface eggplant : vegetable { }
    [cn("油菜")] public interface cole : vegetable { }
    [cn("卷心菜")] public interface cabbage : vegetable { }
    [cn("白菜")] public interface oriental_cabbage : vegetable { }
    [cn("芥菜")] public interface leaf_mustard : vegetable { }
    [cn("西兰花")] public interface broccoli : vegetable { }
    [cn("花椰菜")] public interface cauliflower : vegetable { }
    [cn("山葵")] public interface wasabi : vegetable { }
    [cn("雪里红")] public interface crispfolia : vegetable { }
    [cn("番茄")] public interface tomato : vegetable, for_fruit { }
    [cn("小番茄")] public interface cherry_tomato : vegetable, for_fruit { }


    [cn("薰衣草")] public interface lavendar : for_perfume { }
    [cn("薄荷")] public interface mint : for_perfume { }
    [cn("橄榄")] public interface olive : for_perfume { }
    [cn("罗勒")] public interface basil : for_perfume { }
    [cn("迷迭香")] public interface rosemary : for_perfume { }
    [cn("藿香")] public interface ageratum : for_perfume { }
    [cn("芥菜")] public interface mustard : for_perfume { }
    [cn("蒜")] public interface garlic : for_perfume { }
    [cn("葱")] public interface scallion : for_perfume { }
    [cn("洋葱")] public interface onion : for_perfume { }
    [cn("韭菜")] public interface leek : for_perfume { }
    [cn("香草")] public interface vanilla : for_perfume { }
    [cn("欧芹")] public interface parsley : for_perfume { }
    [cn("香菜")] public interface oriental_parsley : for_perfume { }
    [cn("末药")] public interface myrrh : for_perfume { }
    [cn("紫苏")] public interface perilla : for_perfume { }
    [cn("栀子")] public interface gardenia : for_perfume { }
    [cn("菊花")] public interface crysanthemum : for_perfume { }
    [cn("艾草")] public interface artemisia : for_perfume { }
    [cn("茶")] public interface tea : spice, for_lipid { }

    [cn("香料")] public interface spice : for_perfume { }
    [cn("肉豆蔻")] public interface nutmeg : spice { }
    [cn("孜然")] public interface cumin : spice { }
    [cn("肉桂")] public interface cinnamon : spice { }
    [cn("生姜")] public interface ginger : spice { }
    [cn("胡椒")] public interface pepper : spice { }
    [cn("花椒")] public interface oriental_pepper : spice { }
    [cn("芝麻")] public interface sesame : spice, for_lipid { }
    [cn("可可")] public interface coacoa : spice, for_lipid { }
    [cn("咖啡")] public interface coffea : spice, for_lipid { }




    // 草
    [cn("竹")] public interface bamboo : herbal { }
    [cn("芦笋")] public interface asparagus : herbal { }
    [cn("香茅")] public interface lemongrass : herbal { }
    [cn("莎草")] public interface sedge : herbal { }
    [cn("茜草")] public interface madder : herbal { }


    // 草花
    [cn("莲花")] public interface lotus : herbal_flower { }
    [cn("菖蒲")] public interface calamus : herbal_flower { }
    [cn("紫罗兰")] public interface violet : herbal_flower { }
    [cn("牵牛花")] public interface morning_glory : herbal_flower { }
    [cn("金盏花")] public interface marigold : herbal_flower { }
    [cn("鸡冠花")] public interface celosia : herbal_flower { }
    [cn("石竹")] public interface dianthus : herbal_flower { }
    [cn("菊")] public interface chrysanthemum : herbal_flower { }
    [cn("君子兰")] public interface clivia : herbal_flower { }
    [cn("芍药")] public interface herbaceous_peony : herbal_flower { }
    [cn("郁金香")] public interface tunip : herbal_flower { }
    [cn("百合")] public interface lily : herbal_flower { }
    [cn("芦荟")] public interface aloe : herbal_flower { }
    [cn("龙舌兰")] public interface maguey : herbal_flower { }
    [cn("玫瑰")] public interface rose : herbal_flower { }
    [cn("月季")] public interface oriental_rose : herbal_flower { }
    [cn("桂花")] public interface osmanthus : herbal_flower { }
    [cn("茶花")] public interface camelia : herbal_flower { }
    [cn("玉兰")] public interface magnolia : herbal_flower { }
    [cn("腊梅")] public interface winter_sweet : herbal_flower { }
    [cn("迎春花")] public interface winter_jasmine : herbal_flower { }
    [cn("杜鹃花")] public interface azalea : herbal_flower { }
    [cn("丁香")] public interface clove_lilac : herbal_flower { }
    [cn("鸢尾")] public interface iris : herbal_flower { }
    [cn("罂粟")] public interface poppy : herbal_flower { }
    [cn("牡丹")] public interface peony : herbal_flower { }
    [cn("风信子")] public interface hyacinyh : herbal_flower { }
    [cn("向日葵")] public interface sunflower_herbal : herbal_flower { }
    [cn("薰衣草")] public interface lavender : herbal_flower { }
    [cn("茉莉")] public interface jasmine : herbal_flower { }
    [cn("睡莲")] public interface waterlily : herbal_flower { }
    [cn("水仙")] public interface narcissus : herbal_flower { }
    [cn("秋水仙")] public interface autumnale : herbal_flower { }
    [cn("金莲花")] public interface globeflower_herbal : herbal_flower { }
    [cn("香蒲")] public interface cattail : herbal_flower { }
    [cn("桃金娘")] public interface crape_myrtle : herbal_flower { }
    [cn("香石竹/康乃馨")] public interface carnation : herbal_flower { }
    [cn("茱萸")] public interface cornel : herbal_flower { }
    [cn("凤仙花")] public interface jewelweed : herbal_flower { }
    [cn("夹竹桃")] public interface oleander : herbal_flower { }
    [cn("紫藤")] public interface wisteria : herbal_flower { }
    [cn("醉蝶花")] public interface spiderflower_herbal : herbal_flower { }
    [cn("兰花")] public interface orchid : herbal_flower { }

    // 木花
    [cn("秋海棠")] public interface begonia : woody_flower { }
    [cn("海棠")] public interface crabapple : woody_flower { }
    [cn("芙蓉")] public interface hibiscus_mutabilis : woody_flower { }
    [cn("木槿")] public interface hibiscus_syriacus : woody_flower { }
    [cn("扶桑")] public interface hibiscus_sinensis : woody_flower { }
    [cn("桃花")] public interface peachblossom : woody_flower { }
    [cn("樱花")] public interface cherryblossom : woody_flower { }



    // 木裸
    [cn("银杏")] public interface ginkgo : woody { }
    [cn("苏铁")] public interface cycas : woody { }
    [cn("松")] public interface pine : woody { }
    [cn("落叶松")] public interface larch : woody { }
    [cn("红松")] public interface red_pine : woody { }
    [cn("柏")] public interface cypress : woody { }
    [cn("杉")] public interface fir : woody { }
    [cn("红杉")] public interface red_fir : woody { }

    // 木种
    [cn("榆")] public interface elm : woody { }
    [cn("杨")] public interface willow : woody { }
    [cn("杨")] public interface poplar : woody { }
    [cn("桦")] public interface birch : woody { }
    [cn("桉")] public interface eucalyptus : woody { }
    [cn("橡")] public interface ork : woody { }
    [cn("樟")] public interface camphor : woody { }
    [cn("枫")] public interface maple : woody { }
    [cn("檀")] public interface sandalwood : woody { }
    [cn("紫檀")] public interface red_sandalwood : woody { }
    [cn("山毛榉")] public interface beech : woody { }
    [cn("猴面包树")] public interface baobab : woody { }
    [cn("冬青")] public interface holly : woody { }
    [cn("红树")] public interface mangrove : woody { }
    [cn("苏木")] public interface sappanwood : woody { }
    [cn("金合欢")] public interface acacia : woody { }
    [cn("楠木")] public interface phoebe : woody { }
    [cn("梧桐")] public interface phoenix_tree : woody { }


    // 幻想
    [cn("火球花")] public interface fireflower : fantastic_flora { }
    [cn("曼陀罗华")] public interface mandarava : fantastic_flora { }
    [cn("曼珠沙华")] public interface manjusaka : fantastic_flora { }
    [cn("曼德拉草")] public interface mandrake : fantastic_flora { }
    [cn("豌豆射手")] public interface peashooter : fantastic_flora { }
    [cn("行路草")] public interface oddish : fantastic_flora { }
    [cn("魔法蘑菇")] public interface magicmushroom : fantastic_flora { }
    [cn("金苹果")] public interface goldenapple : fantastic_flora { }
    [cn("爆炸果实")] public interface blastcone : fantastic_flora { }
    [cn("蜜糖果实")] public interface honeyfruit : fantastic_flora { }
    [cn("占卜果实")] public interface scryers_bloom : fantastic_flora { }




}
