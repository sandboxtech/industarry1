
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{


    [cn("单糖")] public interface monosaccharide : nutritious { }



    [cn("纤维素")] public interface cellouse : nutritious { }
    [cn("脂质")] public interface lipid : nutritious { }
    [cn("树脂")] public interface resin : nutritious { }
    [cn("橡胶")] public interface rubber : nutritious { }
    [cn("蛋白质")] public interface protein : nutritious { }
    [cn("淀粉")] public interface starch : nutritious { }



    [cn("可食用")] public interface digestable { } // => 食用
    [cn("含营养")] public interface nutritious : digestable, organics { } // => 食用

    [hide] public interface for_monosaccharide : nutritious { }
    [hide] public interface for_starch : nutritious, for_monosaccharide { }
    [hide] public interface for_cellouse : nutritious { }
    [hide] public interface for_protein : nutritious { }
    [hide] public interface for_lipid : nutritious { }




}
