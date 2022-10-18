using Items;
using UnityEngine;
using Utils;

namespace Assets.Scripts.Construction
{
    public class ConstructionController : Dependable
    {
        PlaceableItemsProvider _placeableItemsProvider;

        void Start()
        {
            _placeableItemsProvider = new PlaceableItemsProvider();
        }

        public bool CanPlaceItemAt(ItemData itemData, Vector2 coord)
        {
            return true;
        }

        public void PlaceItemAt(ItemData itemData, Vector2 coord)
        {
            if (!CanPlaceItemAt(itemData, coord)) return;

            var placeableItem = _placeableItemsProvider.GetPlaceableItem(itemData);
            placeableItem.transform.position = coord;
            Instantiate(placeableItem);
        }
    }
}