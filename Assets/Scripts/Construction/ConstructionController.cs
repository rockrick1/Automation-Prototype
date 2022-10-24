using Assets.Scripts.Player;
using Buildings;
using Buildings.Belts;
using Items;
using System;
using UnityEngine;
using Utils;

namespace Assets.Scripts.Construction
{
    public class ConstructionController : Dependable
    {
        enum State
        {
            Standby,
            Construction,
            Destruction
        }
        static int i = 0;

        [SerializeField] ItemPeviewController _selectedItemPreview;

        PlaceableItemsProvider _placeableItemsProvider;
        ItemData _selectedItemData;
        Vector3 _intendedPlacingPosition;
        State _currentState;

        protected override void Start()
        {
            base.Start();
            _placeableItemsProvider = new PlaceableItemsProvider();
            _currentState = State.Standby;
            _selectedItemPreview.Hide();
        }

        public bool CanPlaceItemAt(ItemData itemData, Vector2 pos)
        {
            return true;
        }

        public void PlaceItemAt(ItemData itemData, Vector2 pos)
        {
            WorldGridController grid = DependencyResolver.Instance.Resolve<WorldGridController>();

            var placeableItem = _placeableItemsProvider.GetPlaceableItem(itemData) as PlaceableItemController;
            PlaceableItemController controller = Instantiate(placeableItem);
            controller.gameObject.transform.position = _intendedPlacingPosition;
            controller.gameObject.transform.SetParent(grid.PlacedItemsParent);

            grid.RegisterPlacedItem(_intendedPlacingPosition, controller);
            ProcessAdjacentItems(controller);
            if (controller is BeltController) controller.name = $"belt {i++}";

            controller.Init(_selectedItemPreview.Orientation);
        }

        void ProcessAdjacentItems(PlaceableItemController placed)
        {
            foreach(var keyItem in placed.GetSurroundingItems())
            {
                keyItem.Value.OnItemPlacedAdjacent(placed, placed.transform.position);

                if (!keyItem.Value.TryGetItemAtOrientation(out PlaceableItemController item) || item != placed) continue;
                keyItem.Value.OnItemPlacedAtOrientation(placed);
            }
        }

        public void EnterConstructionMode(ItemData itemData)
        {
            _currentState = State.Construction;
            _selectedItemData = itemData;

            _selectedItemPreview.Show(itemData);
        }

        void UpdateItemPreviewPosition()
        {
            WorldGridController grid = DependencyResolver.Instance.Resolve<WorldGridController>();

            Camera cam = DependencyResolver.Instance.Resolve<PlayerController>().PlayerCamera;
            Vector3 pos = grid.GetClosestGridPointTo(cam.ScreenToWorldPoint(Input.mousePosition));
            _selectedItemPreview.transform.position = pos;
            _intendedPlacingPosition = pos;
        }

        void Update()
        {
            if (_selectedItemPreview.gameObject.activeInHierarchy)
            {
                UpdateItemPreviewPosition();
            }

            //TODO this should be a OnStateExit on a state machine, with only the state transition being triggered by the esc key
            if (Input.GetKeyDown(KeyCode.Escape) && _currentState == State.Construction)
            {
                _currentState = State.Standby;
                _selectedItemPreview.Hide();
                _selectedItemData = null;
            }

            if (Input.GetMouseButtonDown(0) && _currentState == State.Construction)
            {
                if (!CanPlaceItemAt(_selectedItemData, _intendedPlacingPosition)) return;
                PlaceItemAt(_selectedItemData, _intendedPlacingPosition);
            }

            if (Input.GetKeyDown(KeyCode.R) && _currentState == State.Construction)
            {
                _selectedItemPreview.RotateCw();
            }
        }
    }
}