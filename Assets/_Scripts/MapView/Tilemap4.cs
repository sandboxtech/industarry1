
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace W
{
    public interface ILowOrder { }

    public class Tilemap4 : MonoBehaviour
    {
        [SerializeField]
        public Tilemap ContentLowOrder;
        [SerializeField]
        public Tilemap ContainerLowOrder;
        [SerializeField]
        public Tilemap HighlightLowOrder;
        [SerializeField]
        public Tilemap DecorationLowOrder;

        [SerializeField]
        public Tilemap Content;
        [SerializeField]
        public Tilemap Container;
        [SerializeField]
        public Tilemap Highlight;
        [SerializeField]
        public Tilemap Decoration;

        public void SetLowSortingOrder(int order) {
            ContentLowOrder.GetComponent<TilemapRenderer>().sortingOrder = order;
            ContainerLowOrder.GetComponent<TilemapRenderer>().sortingOrder = order;
            HighlightLowOrder.GetComponent<TilemapRenderer>().sortingOrder = order;
            DecorationLowOrder.GetComponent<TilemapRenderer>().sortingOrder = order;
        }
        public void SetSortingOrder(int order) {
            Container.GetComponent<TilemapRenderer>().sortingOrder = order;
            Highlight.GetComponent<TilemapRenderer>().sortingOrder = order;
            Highlight.GetComponent<TilemapRenderer>().sortingOrder = order;
            Decoration.GetComponent<TilemapRenderer>().sortingOrder = order;
        }


        public void BounceContentTransform(Vector3Int pos, float x, float y, float translation) {
            Content.SetTransformMatrix(pos, Transformation.Scale(x, y, translation));
            Container.SetTransformMatrix(pos, Transformation.Scale(x, y, translation));
            Highlight.SetTransformMatrix(pos, Transformation.Scale(x, y, translation));
            Decoration.SetTransformMatrix(pos, Transformation.Scale(x, y, translation));
        }
        public void BounceContentTransformClear(Vector3Int pos) {
            Content.SetTransformMatrix(pos, Matrix4x4.identity);
            Container.SetTransformMatrix(pos, Matrix4x4.identity);
            Highlight.SetTransformMatrix(pos, Matrix4x4.identity);
            Decoration.SetTransformMatrix(pos, Matrix4x4.identity);
        }


        public void RenderItem(Vector3Int pos3d, Item item) {

            Clear(pos3d);

            if (item == null) {
                return;
            }


            string key;

            bool useLowOrder = item is ILowOrder;


            key = item.Content;
            if (key != null) {
                Tilemap tilemap = useLowOrder ? ContentLowOrder : Content;
                tilemap.SetTile(pos3d, Res.I.TileOf(key));
                tilemap.SetColor(pos3d, item.ContentColor);
            }

            key = item.Container;
            if (key != null) {
                Tilemap tilemap = useLowOrder ? ContainerLowOrder : Container;
                tilemap.SetTile(pos3d, Res.I.TileOf(key));
                tilemap.SetColor(pos3d, item.ContainerColor);
            }

            key = item.Highlight;
            if (key != null) {
                Tilemap tilemap = useLowOrder ? HighlightLowOrder : Highlight;
                tilemap.SetTile(pos3d, Res.I.TileOf(key));
                tilemap.SetColor(pos3d, item.HighlightColor);
            }

            key = item.Decoration;
            if (key != null) {
                Tilemap tilemap = useLowOrder ? DecorationLowOrder : Decoration;
                tilemap.SetTile(pos3d, Res.I.TileOf(key));
                tilemap.SetColor(pos3d, item.DecorationColor);
            }

            //Content.SetTile(pos3d, Res.I.TileOf(item.Content));
            //Container.SetTile(pos3d, Res.I.TileOf(item.Container));
            //Highlight.SetTile(pos3d, Res.I.TileOf(item.Highlight));
            //Decoration.SetTile(pos3d, Res.I.TileOf(item.Decoration));
        }

        private void Clear(Vector3Int pos3d) {
            Content.SetTile(pos3d, null);
            Container.SetTile(pos3d, null);
            Highlight.SetTile(pos3d, null);
            Decoration.SetTile(pos3d, null);

            ContentLowOrder.SetTile(pos3d, null);
            ContainerLowOrder.SetTile(pos3d, null);
            HighlightLowOrder.SetTile(pos3d, null);
            DecorationLowOrder.SetTile(pos3d, null);
        }
    }
}
