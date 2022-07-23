
using System;
using UnityEngine;
using UnityEngine.Tilemaps;


namespace W
{
    [Serializable]
    public class AnimatedTile : TileBase, ITile
    {
        public Sprite[] sprites { get; set; }
        public float speed { private get; set; } = 1f;
        public float speedMax { private get; set; } = 0;

        public Func<float> speedGetter { private get; set; } = null;

        public int start_frame = 0;

        /// <param name="tileData">Data to render the tile.</param>
        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData) {
            tileData.transform = Matrix4x4.identity;
            tileData.color = Color.white;
            if (sprites != null && sprites.Length > 0) {
                tileData.sprite = sprites[0];
                tileData.colliderType = Tile.ColliderType.None;
            }
        }

        private static uint hash = 0;
        /// <summary>
        /// Retrieves any tile animation data from the scripted tile.
        /// </summary>
        /// <param name="position">Position of the Tile on the Tilemap.</param>
        /// <param name="tilemap">The Tilemap the tile is present on.</param>
        /// <param name="tileAnimationData">Data to run an animation on the tile.</param>
        /// <returns>Whether the call was successful.</returns>
        public override bool GetTileAnimationData(Vector3Int position, ITilemap tilemap, ref TileAnimationData tileAnimationData) {
            if (sprites != null && sprites.Length > 0) {
                tileAnimationData.animatedSprites = sprites;
                tileAnimationData.animationSpeed = speedGetter == null ? (speedMax == 0 ? speed : M.Lerp(speed, speedMax, H.HashedFloat(ref hash))) : speedGetter.Invoke();

                if (0 < start_frame && start_frame <= sprites.Length) {
                    var tilemapComponent = tilemap.GetComponent<Tilemap>();
                    if (tilemapComponent != null && tilemapComponent.animationFrameRate > 0) {
                        tileAnimationData.animationStartTime = (start_frame - 1) / tilemapComponent.animationFrameRate;
                    }
                } else {
                    tileAnimationData.animationStartTime = 0;
                }
                return true;
            }
            return false;
        }
    }
}