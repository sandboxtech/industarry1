
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{

    //public static class TimeUtility
    //{
    //    public const long MiniSecond = 10000;
    //    public const long Second = 1000 * MiniSecond;
    //    public const long Minute = 60 * Second;
    //    public const long Hour = 60 * Minute;
    //    public const long Day = 24 * Hour;



    //    public static long GetTicks() => GameloopSystem.Ins.Timestamp;  // GameEntry.FrameBuffer(ref lastTicks, ref lastFrame, () => DateTime.Now.Ticks);

    //    public static long GetMiniSeconds() {
    //        return GetTicks() / MiniSecond;
    //    }
    //    public static long GetSeconds() {
    //        return GetTicks() / Second;
    //    }

    //    public static double GetSecondsInDouble() {
    //        return (double)GetTicks() / Second;
    //    }

    //    public static double GetMiniSecondsInDouble() {
    //        return (double)GetTicks() / MiniSecond;
    //    }

    //    public static int GetFrame(float framerate, int spriteCount) {
    //        return (int)(GetTicks() / ((long)(Second * framerate)) % spriteCount);
    //    }

    //    public static int GetAnimationFrame(float framerate, int spriteCount) {
    //        return (int)(Time.time / framerate) % spriteCount;
    //    }

    //    public static int GetAnimationFrame(float framerate) {
    //        return (int)(Time.time / framerate);
    //    }
    //}
}

