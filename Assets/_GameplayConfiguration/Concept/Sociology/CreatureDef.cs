
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{
    [cn("生物")] public interface creature { }

    [cn("病毒")] public interface virus : creature { }
    [cn("细菌")] public interface bacterium : creature { }
    [cn("真菌")] public interface fungus : creature { }
    [cn("植物")] public interface flora : creature { }
    [cn("动物")] public interface fauna : creature { }
    [cn("幻想生物")] public interface fantasy : creature { }

    [cn("地衣")] public interface lichen : fungus, flora { }

    [cn("蘑菇")] public interface mushroom : fungus { }
    [cn("松露")] public interface truffle : fungus { }
    [cn("伞菌")] public interface agaric : fungus { }
    [cn("香菇")] public interface edodes : fungus { }
    [cn("银耳")] public interface silver_ear : fungus { }
    [cn("木耳")] public interface wood_ear : fungus { }
    [cn("灵芝")] public interface lucidum : fungus { }
    [cn("虫草")] public interface cordycep : fungus { }


}
