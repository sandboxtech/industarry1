

using System;
using UnityEngine;

namespace W
{
    public static class TileUtility
    {
        /// <summary>
        /// 计算在一个循环地图中，两个点的直角距离
        /// </summary>
        public static int DistanceCircular(Vec2 lhs, Vec2 rhs, int Width, int Height) {
            int x = DistanceCircular(lhs.x, rhs.x, Width);
            int y = DistanceCircular(lhs.y, rhs.y, Height);
            return x + y;
        }
        public static int DistanceCircular(int x1, int x2, int size) {
            int maxX;
            int minX;
            if (x1 > x2) {
                maxX = x1;
                minX = x2;
            } else {
                maxX = x2;
                minX = x1;
            }
            return M.Max(M.Abs(maxX - minX), (minX - maxX - size));
        }



        public static bool IsInRect(Vec2 pos, Vec2 bottomLeft, Vec2 topRight) {
            return pos.x >= bottomLeft.x && pos.x < topRight.x && pos.y >= bottomLeft.y && pos.y < topRight.y;
        }
        public static bool IsInRect(Vec2 pos, int x0, int y0, int x1, int y1) {
            return pos.x >= x0 && pos.x < x1 && pos.y >= y0 && pos.y < y1;
        }
        public static bool IsInRectOfSize(Vec2 pos, Vec2 bottomLeft, Vec2 size) => IsInRect(pos, bottomLeft, bottomLeft + size);
        public static bool IsInRectOfSize(Vec2 pos, int x0, int y0, int x1, int y1) => IsInRect(pos, x0, y0, x0 + x1, y0 + y1);


        /// <summary>
        /// 用于计算周边某地块数量
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="pred"></param>
        /// <returns></returns>
        public static bool All5(Vec2 pos, Func<Vec2, bool> pred) {
            return pred(pos + Vec2.zero)
                && pred(pos + Vec2.up) && pred(pos + Vec2.down)
                && pred(pos + Vec2.right) && pred(pos + Vec2.left);
        }
        public static bool All9(Vec2 pos, Func<Vec2, bool> pred) {
            return All5(pos, pred) && All4Corner(pos, pred);
        }
        public static bool All4Corner(Vec2 pos, Func<Vec2, bool> pred) {
            return pred(pos + Vec2.left_up) && pred(pos + Vec2.right_up)
                && pred(pos + Vec2.left_down) && pred(pos + Vec2.right_down);
        }



        /// <summary>
        /// 用于计算周边某地块数量
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="pred"></param>
        /// <returns></returns>
        public static int Count5(Vec2 pos, Func<Vec2, int> pred) {
            return pred(pos + Vec2.up)
                + pred(pos + Vec2.left) + pred(pos + Vec2.zero) + pred(pos + Vec2.right)
                + pred(pos + Vec2.down)
                ;
        }
        public static int Count9(Vec2 pos, Func<Vec2, int> pred) {
            return Count5(pos, pred) + Count4Corner(pos, pred);
        }
        public static int Count4Corner(Vec2 pos, Func<Vec2, int> pred) {
            return pred(pos + Vec2.left_up) + pred(pos + Vec2.right_up)
                + pred(pos + Vec2.left_down) + pred(pos + Vec2.right_down);
        }



        public const int Full8x6 = 1 + 1 * 8;
        public const int Null8x6 = 1 + 4 * 8;
        public const int Size8x6 = 6 * 8;
        public static int Index_8x6(Func<Vec2, bool> predicate) {
            if (!predicate(new Vec2(0, 0))) return _To_8x6(1, 4);

            int index = Index_8x6(
                predicate(new Vec2(-1, 1)), predicate(new Vec2(0, 1)), predicate(new Vec2(1, 1)),
                predicate(new Vec2(-1, 0)), true, predicate(new Vec2(1, 0)),
                predicate(new Vec2(-1, -1)), predicate(new Vec2(0, -1)), predicate(new Vec2(1, -1))
            );
            return index;
        }

        /// <summary>
        /// standard: 8x6 tileset
        /// </summary>
        public static int Index_8x6(
                bool topLeft, bool top, bool topRight,
                bool left, bool self, bool right,
                bool bottomLeft, bool bottom, bool bottomRight
            ) {

            if (!self) throw new Exception();

            if (top) {
                if (bottom) {
                    if (left) {
                        if (right) {
                            // main block 16 variations
                            if (topLeft) {
                                if (topRight) {
                                    if (bottomLeft) {
                                        if (bottomRight) {
                                            return _To_8x6(1, 1);
                                        } else {
                                            return _To_8x6(0, 3);
                                        }
                                    } else {
                                        if (bottomRight) {
                                            return _To_8x6(2, 3);

                                        } else {
                                            return _To_8x6(1, 3);
                                        }
                                    }
                                } else {
                                    if (bottomLeft) {
                                        if (bottomRight) {
                                            return _To_8x6(0, 5);

                                        } else {
                                            return _To_8x6(0, 4);
                                        }
                                    } else {
                                        if (bottomRight) {
                                            return _To_8x6(7, 0);
                                        } else {
                                            return _To_8x6(6, 3);
                                        }
                                    }
                                }
                            } else {
                                if (topRight) {
                                    if (bottomLeft) {
                                        if (bottomRight) {
                                            return _To_8x6(2, 5);

                                        } else {
                                            return _To_8x6(6, 0);
                                        }
                                    } else {
                                        if (bottomRight) {
                                            return _To_8x6(2, 4);

                                        } else {
                                            return _To_8x6(7, 3);
                                        }
                                    }
                                } else {
                                    if (bottomLeft) {
                                        if (bottomRight) {
                                            return _To_8x6(1, 5);

                                        } else {
                                            return _To_8x6(6, 4);
                                        }
                                    } else {
                                        if (bottomRight) {
                                            return _To_8x6(7, 4);

                                        } else {
                                            return _To_8x6(4, 1);
                                        }
                                    }
                                }
                            }
                        } else {
                            // !right
                            if (topLeft) {
                                if (bottomLeft) {
                                    return _To_8x6(2, 1);
                                } else {
                                    return _To_8x6(7, 2);
                                }
                            } else {
                                if (bottomLeft) {
                                    return _To_8x6(7, 1);
                                } else {
                                    return _To_8x6(5, 1);
                                }
                            }
                        }
                    } else {
                        // !left
                        if (right) {
                            if (topRight) {
                                if (bottomRight) {
                                    return _To_8x6(0, 1);
                                } else {
                                    return _To_8x6(6, 2);
                                }
                            } else {
                                if (bottomRight) {
                                    return _To_8x6(6, 1);
                                } else {
                                    return _To_8x6(3, 1);
                                }
                            }
                        } else {
                            // !right
                            return _To_8x6(3, 4);
                        }
                    }
                } else {
                    // !bottom
                    if (left) {
                        if (right) {
                            if (topLeft) {
                                if (topRight) {
                                    return _To_8x6(1, 2);
                                } else {
                                    return _To_8x6(5, 4);
                                }
                            } else {
                                if (topRight) {
                                    return _To_8x6(4, 4);
                                } else {
                                    return _To_8x6(4, 2);
                                }
                            }
                        } else {
                            // !right
                            return topLeft ? _To_8x6(2, 2) : _To_8x6(5, 2);
                        }
                    } else {
                        // !left
                        if (right) {
                            return topRight ? _To_8x6(0, 2) : _To_8x6(3, 2);
                        } else {
                            // !right
                            return _To_8x6(3, 5);
                        }
                    }
                }
            } else {
                // !top
                if (bottom) {
                    if (left) {
                        if (right) {
                            if (bottomLeft) {
                                if (bottomRight) {
                                    return _To_8x6(1, 0);
                                } else {
                                    return _To_8x6(5, 3);
                                }
                            } else {
                                if (bottomRight) {
                                    return _To_8x6(4, 3);
                                } else {
                                    return _To_8x6(4, 0);
                                }
                            }
                        } else {
                            // !right
                            return bottomLeft ? _To_8x6(2, 0) : _To_8x6(5, 0);
                        }
                    } else {
                        // !left
                        if (right) {
                            return bottomRight ? _To_8x6(0, 0) : _To_8x6(3, 0);
                        } else {
                            // !right
                            return _To_8x6(3, 3);
                        }
                    }
                } else {
                    // !bottom
                    if (left) {
                        if (right) {
                            return _To_8x6(5, 5);
                        } else {
                            // !right
                            return _To_8x6(6, 5);
                        }
                    } else {
                        // !left
                        if (right) {
                            return _To_8x6(4, 5);
                        } else {
                            // !right
                            return _To_8x6(7, 5);
                        }
                    }
                }
            }
            throw new Exception($"\n{topLeft} {top} {topRight} \n {left} {self} {right}\n {bottomLeft} {bottom} {bottomRight}\n");
        }
        private const int size8x6 = 8;
        private static int _To_8x6(int i, int j) {
            return i + j * size8x6;
        }



        public const int Size4x4 = 4 * 4;
        public static int Index_4x4(Func<Vec2, bool> predicate) {
            int index = Index_4x4(
                predicate(new Vec2(-1, 0)), predicate(new Vec2(1, 0)),
                predicate(new Vec2(0, 1)), predicate(new Vec2(0, -1))
            );
            return index;
        }
        public static int Index_4x4(bool left, bool right, bool up, bool down) {
            if (left) {
                if (right) {
                    if (up) {
                        // ^ v < >
                        if (down) return 1 * 4 + 1;
                        return 2 * 4 + 1;
                    } else {
                        //   v < >
                        if (down) return 0 * 4 + 1;
                        return 3 * 4 + 1;
                    }
                } else {
                    if (up) {
                        // ^ v <  
                        if (down) return 1 * 4 + 2;
                        return 2 * 4 + 2;
                    } else {
                        //   v <  
                        if (down) return 0 * 4 + 2;
                        return 3 * 4 + 2;
                    }
                }
            } else {
                if (right) {
                    if (up) {
                        // ^ v   >
                        if (down) return 1 * 4 + 0;
                        return 2 * 4 + 0;
                    } else {
                        //   v   >
                        if (down) return 0 * 4 + 0;
                        return 3 * 4 + 0;
                    }
                } else {
                    if (up) {
                        // ^ v    
                        if (down) return 1 * 4 + 3;
                        return 2 * 4 + 3;
                    } else {
                        //   v    
                        if (down) return 0 * 4 + 3;
                        return 3 * 4 + 3;
                    }
                }
            }
            throw new Exception();
        }

    }
}

