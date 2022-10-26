using Assets.Scripts.Player;
using Buildings;
using Buildings.Belts;
using Items;
using UI;
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
        [SerializeField] MultiButton _constructionButton;

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
            _constructionButton.LeftClick.AddListener(OnConstructClicked);
            _constructionButton.RightClick.AddListener(OnDestructClicked);
        }

        public bool CanPlaceItemAt(ItemData itemData, Vector2 pos)
        {
            WorldGridController grid = DependencyResolver.Instance.Resolve<WorldGridController>();
            return !grid.TryGetItemOn(pos, out PlaceableItemController _);
        }

        public void PlaceItemAt(ItemData itemData, Vector2 pos)
        {
            WorldGridController grid = DependencyResolver.Instance.Resolve<WorldGridController>();

            var placeableItem = _placeableItemsProvider.GetPlaceableItem(itemData);
            PlaceableItemController controller = Instantiate(placeableItem);
            controller.gameObject.transform.position = _intendedPlacingPosition;
            controller.gameObject.transform.SetParent(grid.PlacedItemsParent);

            if (controller is BeltController belt) belt.name = $"belt{i++}";

            grid.RegisterPlacedItem(_intendedPlacingPosition, controller);
            controller.Init(_selectedItemPreview.Orientation);
            ProcessPlacementForAdjacentItems(controller);
        }

        public void DestroyItemAt(Vector2 pos)
        {
            WorldGridController grid = DependencyResolver.Instance.Resolve<WorldGridController>();

            if (!grid.TryGetItemOn(pos, out PlaceableItemController controller)) return;

            controller.OnDestroyed();
            ProcessDestructionForAdjacentItems(controller);
            grid.DestroyItemAt(pos);
            Destroy(controller.gameObject);
        }

        void ProcessPlacementForAdjacentItems(PlaceableItemController placed)
        {
            foreach(var keyItem in placed.GetSurroundingItems())
            {
                keyItem.Value.OnItemPlacedAdjacent(placed, placed.transform.position);

                if (!keyItem.Value.TryGetItemAtOrientation(out PlaceableItemController item) || item != placed) continue;
                keyItem.Value.OnItemPlacedAtOrientation(placed);
            }
        }

        void ProcessDestructionForAdjacentItems(PlaceableItemController destroyed)
        {
            foreach (var keyItem in destroyed.GetSurroundingItems())
            {
                keyItem.Value.OnItemRemovedAdjacent(destroyed, destroyed.transform.position);

                if (!keyItem.Value.TryGetItemAtOrientation(out PlaceableItemController item) || item != destroyed) continue;
                keyItem.Value.OnItemRemovedAtOrientation(destroyed);
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

        void OnConstructClicked()
        {
            if (_currentState != State.Construction) return;

            if (!CanPlaceItemAt(_selectedItemData, _intendedPlacingPosition)) return;
            PlaceItemAt(_selectedItemData, _intendedPlacingPosition);
        }

        void OnDestructClicked()
        {
            if (_currentState != State.Construction) return;

            WorldGridController grid = DependencyResolver.Instance.Resolve<WorldGridController>();
            Camera cam = DependencyResolver.Instance.Resolve<PlayerController>().PlayerCamera;

            DestroyItemAt(grid.GetClosestGridPointTo(cam.ScreenToWorldPoint(Input.mousePosition)));
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

            if (Input.GetKeyDown(KeyCode.R) && _currentState == State.Construction)
            {
                if (Input.GetKeyDown(KeyCode.LeftShift)) _selectedItemPreview.RotateCcw();
                else _selectedItemPreview.RotateCw();
            }
        }
    }
}