using Items;
using UnityEngine;

namespace Assets.Scripts.Construction
{
    public class ItemPeviewController : BasePlaceableItemController
    {
        [SerializeField] float _opacity = .5f;

        public void Show(ItemData itemData)
        {
            _itemData = itemData;
            _spriteRenderer.sprite = itemData.Sprite;
            _spriteRenderer.color = new Color(1, 1, 1, _opacity);
            gameObject.SetActive(true);
            base.Init(Orientation);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void MoveTo(Vector2 pos)
        {
            transform.position = pos;
        }
    }
}