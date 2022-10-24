using Assets.Scripts.Resources;
using Buildings;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Assets.Scripts.Construction
{
    public class WorldGridController : Dependable
    {
        [SerializeField] Grid _grid;
        [SerializeField] Transform _placedItemsParent;
        [SerializeField] Transform _resourceNodesParent;

        [SerializeField] ResourceNodeController _coalNodeTest;

        public Transform PlacedItemsParent => _placedItemsParent;
        public Transform ResourceNodesParent => _resourceNodesParent;
        public Dictionary<Vector2Int, PlaceableItemController> PlacedItemsMatrix;
        public Dictionary<Vector2Int, ResourceNodeController> ResourceNodesMatrix;

        protected override void Start()
        {
            base.Start();
            CreateResourceNodes();
            CreatePlacedItems();
        }

        void CreatePlacedItems()
        {
            PlacedItemsMatrix = new Dictionary<Vector2Int, PlaceableItemController>();
            //fill with existing items
        }

        void CreateResourceNodes()
        {
            ResourceNodesMatrix = new Dictionary<Vector2Int, ResourceNodeController>();
            //fill with existing items
            //test
            ResourceNodesMatrix.Add(new Vector2Int(-1, 0), _coalNodeTest);
            ResourceNodesMatrix.Add(new Vector2Int(-1, -1), _coalNodeTest);
            ResourceNodesMatrix.Add(new Vector2Int(-1, 1), _coalNodeTest);
            ResourceNodesMatrix.Add(new Vector2Int(2, 1), _coalNodeTest);
            ResourceNodesMatrix.Add(new Vector2Int(1, 2), _coalNodeTest);
            ResourceNodesMatrix.Add(new Vector2Int(2, 2), _coalNodeTest);

            foreach (Vector2Int pos in ResourceNodesMatrix.Keys)
            {
                var controller = ResourceNodesMatrix[pos];
                ResourceNodeController go = Instantiate(controller);
                go.transform.position = _grid.GetCellCenterWorld(new Vector3Int(pos.x, pos.y));
                go.transform.SetParent(_resourceNodesParent);
            }
        }

        public Vector3Int GetGridIndexTo(Vector3 pos)
        {
            return _grid.WorldToCell(pos);
        }

        public Vector3 GetClosestGridPointTo(Vector3 pos)
        {
            return _grid.GetCellCenterWorld(_grid.WorldToCell(pos));
        }

        public void RegisterPlacedItem(Vector3 pos, PlaceableItemController item)
        {
            PlacedItemsMatrix[(Vector2Int) GetGridIndexTo(pos)] = item;
        }

        public bool TryGetItemOn(Vector3 pos, out PlaceableItemController output)
        {
            return PlacedItemsMatrix.TryGetValue((Vector2Int) GetGridIndexTo(pos), out output);
        }

        public bool TryGetNodeOn(Vector3 pos, out ResourceNodeController output)
        {
            return ResourceNodesMatrix.TryGetValue((Vector2Int) GetGridIndexTo(pos), out output);
        }
    }
}