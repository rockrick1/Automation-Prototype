using UnityEngine;

namespace Items
{
    public class BasePlaceableItemController : MonoBehaviour
    {
        [SerializeField] ItemData _itemData;

        public ItemData ItemData => _itemData;
    }
}