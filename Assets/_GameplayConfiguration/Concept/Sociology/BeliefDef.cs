
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{

    [cn("信用")] public interface credit { }
    [cn("能力")] public interface ability { }
    [cn("思维")] public interface thought { }



    [cn("货币")] public interface currency : credit { }
    [cn("声望")] public interface reputation : credit { }



    [cn("形态")] public interface ideology : thought { }
    [cn("信念")] public interface faith : thought { }
    [cn("文化")] public interface culture : thought { }
    [cn("道德")] public interface ethics : thought { }


    [cn("金锭")] public interface gold_ingot : currency { }

    
    [impl(typeof(gold_coin_))]
    [cn("金币")] public interface gold_coin : currency { }
    public class gold_coin_ : Item {
        public override Color4 ContentColor => G.theme.GoldColor;
    }

    [cn("银币")] public interface silver_coin : currency { }
    [cn("铜币")] public interface copper_coin : currency { }



    [cn("素食")] public interface vegetarian : ethics { }


    [cn("混沌")] public interface chaotic : ethics { }
    [cn("守序")] public interface lawful : ethics { }

    [cn("正义")] public interface good : ethics { }
    [cn("邪恶")] public interface evil : ethics { }


    [cn("性格")] public interface temperament : thought { }




}
