
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{

    [cn("电磁性质")] public interface EMwave_interaction { }

    [cn("不透明")] public interface opaque : EMwave_interaction { }
    [cn("透明")] public interface transparent : EMwave_interaction { }

    [cn("反射")] public interface reflective : EMwave_interaction { }




    [cn("机械性质")] public interface mechanical_property { }

    [cn("固体")] public interface solid : mechanical_property { }
    [cn("流体")] public interface fluid : mechanical_property { }
    [cn("液体")] public interface liquid : mechanical_property { }
    [cn("气体")] public interface gas : mechanical_property { }
    [cn("柔软")] public interface soft : solid { }




    [cn("粉末")] public interface powder : solid { }
    [cn("胶体")] public interface colloid : solid { }
    [cn("凝胶")] public interface gel : colloid { }


    [cn("晶体")] public interface crystal : solid { }
    [cn("玻璃态")] public interface amorphous : solid { }






    [cn("形状")] public interface shape { }

    [cn("凹体")] public interface concave : shape { }
    [cn("凸体")] public interface convex : shape { }

    [cn("多胞形")] public interface polytopes : shape { }
    [cn("粗糙")] public interface rough : shape { }
    [cn("光滑")] public interface smooth : shape { }
    [cn("尖锐")] public interface sharp : shape { }




    [cn("点")] public interface point : shape { }

    [cn("形状1D")] public interface shape1d : shape { }

    [cn("直线")] public interface line : shape { }
    [cn("射线")] public interface ray : shape { }
    [cn("线段")] public interface segment : shape { }


    [cn("形状2D")] public interface shape2d : shape { }

    [cn("面")] public interface plane : shape { }

    [cn("二次曲线")] public interface quadratic_curve : shape2d { }
    [cn("双曲")] public interface hyperbola : quadratic_curve { }
    [cn("椭圆")] public interface ellipse : quadratic_curve { }
    [cn("圆")] public interface round : ellipse { }

    [cn("多边形")] public interface polygon : shape2d, polytopes { }
    [cn("三角形")] public interface triangle : polygon { }
    [cn("四边形")] public interface quadrilaterals : polygon { }
    [cn("五边形")] public interface pentagon : polygon { }

    [cn("梯形")] public interface trapezoid : quadrilaterals { }
    [cn("平行四边形")] public interface parallelogram : quadrilaterals { }
    [cn("矩形")] public interface rectangle : parallelogram { }
    [cn("正方形")] public interface square : parallelogram, rectangle { }

    [cn("心形")] public interface heart_shape : shape2d { }
    [cn("星形")] public interface star_shape : shape2d { }




    [cn("形状3D")] public interface shape3d : shape { }


    [cn("长方体")] public interface cuboid : shape3d { }
    [cn("刃")] public interface blade_shape : shape3d, sharp { }
    [cn("针")] public interface needle_shape : shape3d, sharp { }



    [cn("球")] public interface ball_shape : ellipsoid_shape { }
    [cn("椭球")] public interface ellipsoid_shape : shape3d { }
    [cn("棱柱")] public interface prism_shape : shape3d { }
    [cn("圆柱")] public interface cylinder_shape : shape3d { }
    [cn("棱锥")] public interface pyramid_shape : shape3d { }
    [cn("圆锥")] public interface cone_shape : shape3d { }




    [cn("杯体")] public interface mug_shape : shape3d, concave { }
    [cn("篮框")] public interface basket_shape : shape3d, concave { }
    [cn("管道")] public interface pipe_item : shape3d, concave { }
    [cn("盒子")] public interface box_shape : shape3d, concave { }
    [cn("框架")] public interface frame_shape : shape3d, concave { }



    [cn("多面体")] public interface polyhedra : shape3d, polytopes { }
    [cn("正多面体")] public interface polyhedra_regular : polyhedra { }

    [cn("正四面体")] public interface tetrahedron : polyhedra { }
    [cn("正六面体")] public interface hexahedron : polyhedra { }
    [cn("正八面体")] public interface octahedron : polyhedra { }
    [cn("正十二面体")] public interface dodecahedron : polyhedra { }
    [cn("正二十面体")] public interface icosahedron : polyhedra { }




    [cn("砖")] public interface brick_shape : cuboid { }
    [cn("板")] public interface plank_shape : cuboid { }

    [cn("盘")] public interface plate_shape : cuboid { }
    [cn("单")] public interface sheet_shape : cuboid { }

    [cn("锭")] public interface ingot_shape : cuboid { }
    [cn("方")] public interface cube_shape : cuboid { }

}
