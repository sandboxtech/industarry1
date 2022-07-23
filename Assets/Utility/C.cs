
using System;
using System.Collections.Generic;
using UnityEngine;


namespace W
{
    /// <summary>
    /// C for constant
    /// </summary>
    public static class C
    {
        public const long TestFactoryDelDivider = 1;
        public const long TestMarketDelDivider = 1;


        public const int V = 4; // 宽度 4 Content, Container, Highlight, Decoration

        public const int FloorHeight = 3;

        public const int PlayerReach = 8;

        public const int PixelPerUnit = 16;
        public const float PPUReciprocal = 1f / 16;

        public const float ClampAlpha = 1f / 64;

        // player sprite
        public const int PlayerSpriteHeight = 10;
        public const int PlayerSpriteWidth = 7;
        public const int PlayerInteractionSpriteWidth = 7;
        public const int PlayerEatingSpriteWidth = 5;

        public const int LandscapeCanvasWidth = 320;
        public const int LandscapeCanvasHeight = 180;
        public const int PortraitCanvasWidth = LandscapeCanvasHeight;
        public const int PortraitCanvasHeight = LandscapeCanvasWidth;

        public const int CanvasToCamera = 2;
        public const int LandscapeCameraWidth = LandscapeCanvasWidth * CanvasToCamera;
        public const int LandscapeCameraHeight = LandscapeCanvasHeight * CanvasToCamera;
        public const int PortraitCameraWidth = LandscapeCameraHeight;
        public const int PortraitCameraHeight = LandscapeCameraWidth;


        public const float LandscapeRatio = (float)LandscapeCanvasWidth / LandscapeCanvasHeight;
        public const float PortraitRatio = (float)LandscapeCanvasHeight / LandscapeCanvasWidth;

        public const float LandscapeCameraOrthographicSize = (float)LandscapeCanvasHeight / (2 * PixelPerUnit);
        public const float PortraitCameraOrthographicSize = (float)PortraitCanvasHeight / (2 * PixelPerUnit);



        public const int HandLength = 8; // 背包界面的宽度
        public const int InventoryWidth = 7; // 背包界面的宽度


        public const long MicroSecond = 10;
        public const long MilliSecond = 1000 * MicroSecond;
        public const long Second = 1000 * MilliSecond;
        public const long Minute = 60 * Second;
        public const long Hour = 60 * Minute;
        public const long Day = 24 * Hour;
        public const long Year = 365 * Day;

        public const long SimpleWaitTimespan = 20 * Second;
        public const long DefaultOnlineTimespan = 3 * Minute; // 6 * Hour;

        public const float ClickTap = 0.1f;

    }

}
