using UnityEngine;

namespace Items
{
    public class ItemInTransportController : MonoBehaviour
    {
        [SerializeField] SpriteRenderer _spriteRenderer;

        public void Init(ItemData itemData)
        {
            _spriteRenderer.sprite = Sprite.Create(itemData.Sprite.texture,
              new Rect(0, 0, itemData.Sprite.texture.width, itemData.Sprite.texture.height),
              new Vector2(0.5f, .5f), 32);

            //_spriteRenderer.sprite = itemData.Sprite;
        }
    }
}