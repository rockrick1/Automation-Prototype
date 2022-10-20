using Items;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Assets.Scripts.Construction
{
    public class WorldGridController : Dependable
    {
        [SerializeField] Grid _grid;
        [SerializeField] Transform _placedItemsParent;

        Dictionary<Vector2Int, BasePlaceableItemController> _placedItemsMatrix;

        public Transform PlacedItemsParent => _placedItemsParent;
        public Dictionary<Vector2Int, BasePlaceableItemController> PlacedItemsMatrix;

        public Vector2Int GetGridIndexTo(Vector2 pos)
        {
            var ret = GetClosestGridPointTo(pos) / _grid.cellSize;
            return new Vector2Int((int) ret.x, (int) ret.y);
        }

        public Vector2 GetClosestGridPointTo(Vector2 pos)
        {
            var ret = new Vector2
            (
                (((int) pos.x) / ((int) _grid.cellSize.x)) + _grid.cellSize.x / 2,
                (((int) pos.y) / ((int) _grid.cellSize.y)) + _grid.cellSize.y / 2
            );

            ret.x -= pos.x < 0 ? 1 : 0;
            ret.y -= pos.y < 0 ? 1 : 0;

            return ret;
        }
    }
}