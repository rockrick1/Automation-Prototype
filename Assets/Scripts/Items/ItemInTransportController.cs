using UnityEngine;

namespace Items
{
    public class ItemInTransportController : MonoBehaviour
    {
        [SerializeField] SpriteRenderer _spriteRenderer;

        public void Init(ItemData itemData)
        {
            _spriteRenderer.sprite = itemData.Sprite;
        }
    }
}