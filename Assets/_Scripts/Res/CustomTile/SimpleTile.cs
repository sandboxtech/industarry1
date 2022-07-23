
using System;
using UnityEngine;
using UnityEngine.Tilemaps;


namespace W
{
    [Serializable]
    public class SimpleTile : TileBase, ITile
    {
        public Sprite sprite { get; set; }

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData) {
            tileData.transform = Matrix4x4.identity;
            tileData.color = Color.white;
            tileData.sprite = sprite;
            tileData.colliderType = Tile.ColliderType.None;
        }

        public override bool GetTileAnimationData(Vector3Int position, ITilemap tilemap, ref TileAnimationData tileAnimationData) {
            return false;
        }
    }
}